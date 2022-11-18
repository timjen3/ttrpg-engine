using System;
using System.Text.RegularExpressions;

namespace TTRPG.Engine.Roles.Attributes
{
	public enum RoleAttributeType
	{
		IntRange = 0,
		StaticString = 1,
		StaticInt = 2
	}

	public abstract class RoleAttributeDefinition
	{
		public string Key { get; set; }

		public RoleAttributeType AttributeType { get; set; }

		public static bool IsEmbeddedFunction(string value) => Regex.IsMatch(value, @"\[\[.+\]\]$");

		public static FormatException CallOutInvalidDerivedAttribute(string key) => new FormatException($"'{key}' does not match required derived attribute format of '[[key]]'.");
	}
}
