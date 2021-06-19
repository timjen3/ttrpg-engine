using System.Collections.Generic;

namespace DieEngine.Equations
{
	public interface IEquationResolver
	{
		double Process(string equation, IDictionary<string, double> inputs);
	}
}