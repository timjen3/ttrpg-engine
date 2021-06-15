using DieEngine.Exceptions;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine.CustomFunctions
{
	public class EquationResolver : IEquationResolver
	{
		private readonly ICustomFunctionRunner _customFunctionRunner;

		public EquationResolver(ICustomFunctionRunner customFunctionRunner)
		{
			_customFunctionRunner = customFunctionRunner;
		}

		public double Process(string equation, IDictionary<string, double> inputs)
		{
			// resolve custom functions
			string processedEquation = _customFunctionRunner.InsertEquations(equation, inputs);
			// resolve function with mxparser
			var exp = new Expression(processedEquation);
			exp.removeAllConstants();  // reduce confusion from variables like "c" already existing
			if (inputs != null)
			{
				foreach (var kvp in inputs)
				{
					exp.addArguments(new Argument(kvp.Key.Trim(), kvp.Value));
				}
			}
			var result = exp.calculate();
			// if function failed to resolve throw exception
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
