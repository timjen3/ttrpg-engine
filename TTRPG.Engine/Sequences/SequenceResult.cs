using System.Collections.Generic;
using TTRPG.Engine.SequenceItems;

namespace TTRPG.Engine.Sequences
{
	public class SequenceResult
	{
		public Sequence Sequence { get; set; }
		public List<SequenceItemResult> Results { get; set; } = new List<SequenceItemResult>();
		public IDictionary<string, string> Output { get; set; }
	}
}
