using System.Collections.Generic;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Conditions
{
	public interface ICondition
	{
		bool Check(string itemName, IEquationResolver equationResolver, IDictionary<string, string> inputs, SequenceResult results);
	}
}