using DieEngine.Exceptions;
using FormatWith;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DieEngine.CustomFunctions
{
	public class CustomFunctionRunner : ICustomFunctionRunner
	{
		private readonly IDictionary<string, ICustomFunction> _equationsLookup;

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

		public CustomFunctionRunner(IEnumerable<ICustomFunction> equations)
		{
			_equationsLookup = new Dictionary<string, ICustomFunction>(StringComparer.OrdinalIgnoreCase);
			foreach (var equation in equations)
			{
				_equationsLookup[equation.FunctionName] = equation;
			}
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
				if (inputs != null)
				{
					// todo: avoid copy here (FormatWith library shortcoming)
					var inputs2 = new Dictionary<string, string>();
					foreach (var item in inputs)
						inputs2[item.Key] = item.Value.ToString();
					functionParams = functionParams.FormatWith(inputs2, MissingKeyBehaviour.ThrowException);  // replace function parameters with inputs if any matches
				}
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
