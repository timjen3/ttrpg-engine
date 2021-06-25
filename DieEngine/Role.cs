using System;
using System.Collections.Generic;

namespace DieEngine
{
	public class Role
	{
		public Role()
		{
		}

		public Role(string name, Dictionary<string, string> attributes)
		{
			Name = name;
			Attributes = attributes;
		}

		public string Name { get; set; }
		public Dictionary<string, string> Attributes { get; set; }

		public Role CloneAs(string name)
		{
			var clonedAttributes = new Dictionary<string, string>(Attributes, StringComparer.OrdinalIgnoreCase);
			var clone = new Role(name, clonedAttributes);

			return clone;
		}
	}
}
