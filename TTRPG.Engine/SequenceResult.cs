using System.Collections.Generic;
using TTRPG.Engine.SequenceItems;

namespace TTRPG.Engine
{
	public class SequenceResult
	{
		public Sequence Input { get; set; }
		public List<SequenceItemResult> Results { get; set; } = new List<SequenceItemResult>();
	}
}
