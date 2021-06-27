using System.Collections.Generic;

namespace TTRPG.Engine.OutputGroupings
{
	public interface IOutputGroupingResult
	{
		string Name { get; set; }
		Dictionary<string, string> Results { get; set; }
	}
}