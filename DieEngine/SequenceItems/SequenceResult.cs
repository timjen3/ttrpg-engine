using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	public class SequenceResult
	{
		public Sequence Input { get; set; }
		public List<SequenceItemResult> Results { get; set; } = new List<SequenceItemResult>();
	}
}
