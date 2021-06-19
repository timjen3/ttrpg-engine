using System.Collections.Generic;

namespace DieEngine
{
	public class Role
	{
		public Role()
		{
		}

		public Role(string name, Dictionary<string, double> attributes)
		{
			Name = name;
			Attributes = attributes;
		}

		public string Name { get; set; }
		public Dictionary<string, double> Attributes { get; set; }
	}
}
