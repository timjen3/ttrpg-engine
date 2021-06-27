using System.Collections.Generic;

namespace TTRPG.Engine.OutputGroupings
{
	public interface IOutputGrouping
	{
		IEnumerable<OutputGroupingItem> Items { get; set; }
		string Name { get; set; }

		IOutputGroupingResult GetResult(IDictionary<string, string> inputs);
	}
}