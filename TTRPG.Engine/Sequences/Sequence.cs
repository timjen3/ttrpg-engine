using System.Collections.Generic;
using TTRPG.Engine.SequenceItems;

namespace TTRPG.Engine.Sequences
{
	public class Sequence
	{
		public string Name { get; set; }

		public List<SequenceItem> Items { get; set; } = new List<SequenceItem>();

		public List<Condition> Conditions { get; set; } = new List<Condition>();

		public List<Mapping> Mappings { get; set; } = new List<Mapping>();
	}
}
