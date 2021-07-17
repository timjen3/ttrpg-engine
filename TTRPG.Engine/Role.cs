using System;
using System.Collections.Generic;

namespace TTRPG.Engine
{
	/// a role carries a set of attributes into equations
	public class Role
	{
		private string _alias;

		/// role parameterless constructor
		public Role()
		{
			Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Categories = new List<string>();
		}

		/// role constructor
		public Role(string name, Dictionary<string, string> attributes, List<string> categories)
		{
			Name = name;
			if (attributes != null) Attributes = new Dictionary<string, string>(attributes, StringComparer.OrdinalIgnoreCase);
			else Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Categories = categories ?? new List<string>();
		}

		/// role's name
		public string Name { get; set; }
		/// role's attributes
		public Dictionary<string, string> Attributes { get; }
		/// categories that role belongs to
		public List<string> Categories { get; }
		/// on originals matches Name, on clones it differs
		public string Alias { get => _alias ?? Name; set { _alias = value; } }

		/// creates a clone with a different name
		public Role CloneAs(string alias)
		{
			var clonedAttributes = new Dictionary<string, string>(Attributes);
			var clonedCategories = new List<string>(Categories);
			var clone = new Role(Name, clonedAttributes, clonedCategories);
			clone.Alias = alias;

			return clone;
		}
	}
}
