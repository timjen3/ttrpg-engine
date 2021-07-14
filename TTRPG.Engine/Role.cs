using System;
using System.Collections.Generic;

namespace TTRPG.Engine
{
	/// a role carries a set of attributes into equations
	public class Role
	{
		/// role parameterless constructor
		public Role()
		{
		}

		/// role constructor
		public Role(string name, Dictionary<string, string> attributes, List<string> categories)
		{
			Name = name;
			Attributes = attributes;
			Categories = categories;
			Alias = name;
		}

		/// role's name
		public string Name { get; set; }
		/// role's attributes
		public Dictionary<string, string> Attributes { get; set; }
		/// categories that role belongs to
		public List<string> Categories { get; set; }
		/// on originals matches Name, on clones it differs
		public string Alias { get; protected set; }

		/// creates a clone with a different name
		public Role CloneAs(string alias)
		{
			var clonedAttributes = new Dictionary<string, string>(Attributes, StringComparer.OrdinalIgnoreCase);
			var clonedCategories = new List<string>(Categories);
			var clone = new Role(Name, clonedAttributes, clonedCategories);
			clone.Alias = alias;

			return clone;
		}
	}
}
