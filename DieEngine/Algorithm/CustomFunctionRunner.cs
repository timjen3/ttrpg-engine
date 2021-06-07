using DieEngine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DieEngine.Algorithm
{
	public class CustomFunctionRunner
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

		public CustomFunctionRunner()
		{
			// todo: use reflection to add all known equations
			_equationsLookup = new Dictionary<string, ICustomFunction>(StringComparer.OrdinalIgnoreCase);
			_equationsLookup["Dice"] = new DiceFunction();
		}

		public bool VerifyEquation(string rawEquation)
		{
			throw new NotImplementedException();
		}

		public string InsertEquations(string rawEquation)
		{
			string formattedEquation = rawEquation;
			var customFunctions = ParseCustomFunctions(rawEquation);
			if (!customFunctions.Any())
				return rawEquation;

			foreach (var function in customFunctions)
			{
				var functionParts = function.Split(':');
				string functionName = functionParts[0];
				if (_equationsLookup.TryGetValue(functionName, out ICustomFunction value))
				{
					string[] functionParams = functionParts[1].Split(',');
					var result = value.DoFunction(functionParams);
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
