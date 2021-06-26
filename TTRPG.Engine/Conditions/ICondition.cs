using TTRPG.Engine.Equations;
using TTRPG.Engine.SequenceItems;
using System.Collections.Generic;

namespace TTRPG.Engine.Conditions
{
	public interface ICondition
	{
		bool Check(string itemName, IEquationResolver equationResolver, IDictionary<string, string> inputs, SequenceResult results);
	}
}