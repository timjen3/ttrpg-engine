using System.Collections.Generic;
using TTRPG.Engine.Engine.Events;
using TTRPG.Engine.SequenceItems;

namespace TTRPG.Engine.Sequences
{
	public class SequenceResult
	{
		public Sequence Sequence { get; set; }
		public bool Completed { get; set; }
		public List<SequenceItemResult> Results { get; set; } = new List<SequenceItemResult>();
		public List<TTRPGEvent> Events { get; set; } = new List<TTRPGEvent>();
	}
}
