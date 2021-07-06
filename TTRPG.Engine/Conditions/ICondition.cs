using System.Collections.Generic;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Conditions
{
	/// a condition that is either for a sequence or a sequence item
	public interface ICondition
	{
		/// Determine if the condition passes for the sequence
		bool Check(IEquationResolver equationResolver, IDictionary<string, string> inputs);
		/// Determine if the condition passes for a sequence item
		bool Check(string itemName, IEquationResolver equationResolver, IDictionary<string, string> inputs, SequenceResult results);
	}
}