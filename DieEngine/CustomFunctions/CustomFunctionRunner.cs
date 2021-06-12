using DieEngine.Exceptions;
using SmartFormat;
using SmartFormat.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DieEngine.CustomFunctions
{
	public class CustomFunctionRunner
	{
		private static readonly IDictionary<string, ICustomFunction> _equationsLookup;
		private static readonly SmartFormatter _formatter;

		private IEnumerable<string> ParseCustomFunctions(string rawEquation)
		{
			MatchCollection matches = Regex.Matches(rawEquation, @"(?<=\[).+?(?=\])");
			if (matches.Count == 0)
				return new string[0];

			List<string> matchStrings = new List<string>();
			foreach (var match in matches)
			{
				matchStrings.Add(match.ToString());
			}

			return matchStrings;
		}

		// todo: maybe do this with dependency injection instead; particularly if functions need access to other services
		static CustomFunctionRunner()
		{
			// load functions with reflection
			_equationsLookup = new Dictionary<string, ICustomFunction>(StringComparer.OrdinalIgnoreCase);
			var customFunctions = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => !x.IsAbstract && !x.IsInterface && typeof(ICustomFunction).IsAssignableFrom(x))
				.Select(x => (ICustomFunction) Activator.CreateInstance(x));
			foreach (var customFunction in customFunctions)
			{
				_equationsLookup[customFunction.FunctionName] = customFunction;
			}
			// set up formatter for injecting variables into custom function params
			_formatter = Smart.CreateDefaultSmartFormat();
			_formatter.AddExtensions(new DictionarySource(_formatter));
		}

		public bool VerifyEquation(string rawEquation)
		{
			throw new NotImplementedException();
		}

		public string InsertEquations(string rawEquation, IDictionary<string, double> inputs)
		{
			string formattedEquation = rawEquation;
			var customFunctions = ParseCustomFunctions(rawEquation);
			if (!customFunctions.Any())
				return rawEquation;

			foreach (var function in customFunctions)
			{
				var functionParts = function.Split(':');
				string functionName = functionParts[0];
				string functionParams = functionParts[1];
				functionParams = _formatter.Format(functionParams, inputs);  // replace function parameters with inputs if any matches
				if (_equationsLookup.TryGetValue(functionName, out ICustomFunction value))
				{
					string[] functionParamsSplit = functionParams.Split(',');
					var result = value.DoFunction(functionParamsSplit);
					Regex rex = new Regex($@"\[{function}\]", RegexOptions.IgnoreCase);
					formattedEquation = rex.Replace(formattedEquation, result.ToString(), 1);
				}
				else
				{
					throw new UnknownCustomFunctionException($"Unknown function named {functionName}.");
				}
			}
			return formattedEquation;
		}
	}
}
