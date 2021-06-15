using System.Collections.Generic;

namespace DieEngine.CustomFunctions
{
	public interface ICustomFunctionRunner
	{
		string InsertEquations(string rawEquation, IDictionary<string, double> inputs);
		bool VerifyEquation(string rawEquation);
	}
}