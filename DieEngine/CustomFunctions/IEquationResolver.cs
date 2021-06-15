using System.Collections.Generic;

namespace DieEngine.CustomFunctions
{
	public interface IEquationResolver
	{
		double Process(string equation, IDictionary<string, double> inputs);
	}
}