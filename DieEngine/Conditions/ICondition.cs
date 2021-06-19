using DieEngine.Equations;
using System.Collections.Generic;

namespace DieEngine
{
	public interface ICondition
	{
		bool Check(int order, IEquationResolver equationResolver, IDictionary<string, double> inputs);
	}
}