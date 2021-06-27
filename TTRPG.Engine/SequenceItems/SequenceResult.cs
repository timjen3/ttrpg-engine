using System.Collections.Generic;
using TTRPG.Engine.OutputGroupings;

namespace TTRPG.Engine.SequenceItems
{
	public class SequenceResult
	{
		public Sequence Input { get; set; }
		public List<SequenceItemResult> Results { get; set; } = new List<SequenceItemResult>();
		public List<IOutputGroupingResult> GroupingResults { get; set; } = new List<IOutputGroupingResult>();
	}
}
