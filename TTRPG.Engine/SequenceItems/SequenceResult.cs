using System.Collections.Generic;

namespace TTRPG.Engine.SequenceItems
{
	public class SequenceResult
	{
		public Sequence Input { get; set; }
		public List<SequenceItemResult> Results { get; set; } = new List<SequenceItemResult>();
	}
}
