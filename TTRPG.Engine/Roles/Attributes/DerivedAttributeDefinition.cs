using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TTRPG.Engine.Roles.Attributes
{
	/// <summary>
	///		The previous math parsing implementation supported injecting string variables
	///		Inputs could look like this:
	///			{"a": "1", "b": "rnd(1,2,3)"}
	///		so equation: "a + b" would be resolved as "a + rnd(1,2,3)"
	///		since jace doesn't support this, a tweak has been made where string inputs to be injected must have a key matching the regex: "^\[\[.+\]\]$"
	///		Inputs must look like this:
	///			{"a": "1", "[[b]]": "rnd(1,2,3)"}
	///		and equation: "a + [[b]]" will be resolved as "a + rnd(1,2,3)"
	/// </summary>
	public class DerivedAttributeDefinition : RoleAttributeDefinition
	{
		#region Derived Attribute Static Methods
		private static IDictionary<string, string> GetEmbeddedFunctions(IDictionary<string, string> inputs) => inputs
			.Where(kvp => IsEmbeddedFunction(kvp.Key))
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		public static bool IsEmbeddedFunction(string value) => Regex.IsMatch(value, @"\[\[.+\]\]$");

		public static FormatException CallOutInvalidDerivedAttribute(string key) => new FormatException($"'{key}' does not match required derived attribute format of '[[key]]'.");

		public static string InjectEmbeddedFunctions(string equation, IDictionary<string, string> inputs)
		{
			var specialInputs = GetEmbeddedFunctions(inputs);
			foreach (var specialInput in specialInputs)
			{
				equation = equation.Replace(specialInput.Key, specialInput.Value);
			}

			return equation;
		}
		#endregion

		// attribute properties
		public string Value { get; set; }
	}
}
