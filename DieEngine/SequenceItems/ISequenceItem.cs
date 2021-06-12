using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	public interface ISequenceItem
	{
		string Name { get; }

		string Equation { get; }

		SequenceItemResult GetResult(IDictionary<string, double> inputs = null);
	}
}
