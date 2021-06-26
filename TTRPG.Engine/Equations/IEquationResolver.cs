using System.Collections.Generic;

namespace TTRPG.Engine.Equations
{
	public interface IEquationResolver
	{
		double Process(string equation, IDictionary<string, string> inputs);
	}
}