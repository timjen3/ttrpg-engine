using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.OutputGroupings
{
	public class OutputGrouping<T> : IOutputGrouping
	{
		public OutputGrouping()
		{
		}

		public OutputGrouping(string name, T payload, IEnumerable<OutputGroupingItem> items)
		{
			Name = name;
			Payload = payload;
			Items = items;
		}

		public string Name { get; set; }
		public T Payload { get; set; }
		public IEnumerable<OutputGroupingItem> Items { get; set; }

		public IOutputGroupingResult GetResult(IDictionary<string, string> inputs)
		{
			var result = new OutputGroupingResult<T>();
			result.Name = Name;
			result.Payload = Payload;
			if (Items != null && Items.Any())
			{
				foreach (var item in Items)
				{
					result.Results[item.Name] = null;
					if (inputs.TryGetValue(item.Source, out string value))
					{
						result.Results[item.Name] = value;
					}	
					if (result.Results[item.Name] == null && item.ThrowIfMissing)
					{
						throw new MissingOutputGroupingItemException($"Output grouping item '{item.Name}' was missing.");
					}
				}
			}	
			return result;
		}
	}
}
