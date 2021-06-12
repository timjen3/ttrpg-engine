using DieEngine.CustomFunctions;
using DieEngine.Exceptions;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine
{
	public class EquationResolver
	{
		// todo: downsides of tight coupling here?
		private static CustomFunctionRunner _customFunctionRunner = new CustomFunctionRunner();

		public double Process(string equation, IDictionary<string, double> inputs)
		{
			string processedEquation = _customFunctionRunner.InsertEquations(equation, inputs);
			var exp = new Expression(processedEquation);
			exp.removeAllConstants();  // reduce confusion from variables like "c" already existing
			if (inputs != null)
			{
				inputs = new Dictionary<string, double>(inputs);
				foreach (var kvp in inputs)
				{
					exp.addArguments(new Argument(kvp.Key.Trim(), kvp.Value));
				}
			}
			var result = exp.calculate();
			if (double.IsNaN(result))
			{
				string[] missingValues = exp.getMissingUserDefinedArguments();
				if (missingValues.Any())
					throw new EquationInputArgumentException($"Missing params: {string.Join(", ", missingValues)}");
				throw new EquationInputArgumentException(exp.getErrorMessage());
			}
			return result;
		}
	}
}
