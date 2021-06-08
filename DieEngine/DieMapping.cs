using System.Collections.Generic;

namespace DieEngine
{
	public class DieMapping
	{
		public DieMapping(){}

		public DieMapping(int order, Dictionary<string, string> mappings)
		{
			Order = order;
			Mappings = mappings;
		}

		// die to apply mapping to
		public int Order { get; set; }

		// mappings to be applied
		public Dictionary<string, string> Mappings { get; set; }
	}
}
