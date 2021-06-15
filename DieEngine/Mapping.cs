using System.Collections.Generic;

namespace DieEngine
{
	public class Mapping
	{
		public Mapping(){}

		public Mapping(int order, IDictionary<string, string> mappings)
		{
			Order = order;
			Mappings = mappings;
		}

		// sequence item to apply mapping to
		public int Order { get; set; }

		// mappings to be applied
		public IDictionary<string, string> Mappings { get; set; }
	}
}
