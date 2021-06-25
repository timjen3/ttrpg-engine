using DieEngine.Equations;
using DieEngine.SequencesItems;
using System.Collections.Generic;

namespace DieEngine.Conditions
{
	public interface ICondition
	{
		bool Check(string itemName, IEquationResolver equationResolver, IDictionary<string, string> inputs, SequenceResult results);
	}
}