using System.Collections.Generic;

namespace TTRPG.Engine.OutputGroupings
{
	public class OutputGroupingResult<T> : IOutputGroupingResult
	{
		public string Name { get; set; }
		public Dictionary<string, string> Results { get; set; } = new Dictionary<string, string>();
		public T Payload { get; set; }
	}
}
