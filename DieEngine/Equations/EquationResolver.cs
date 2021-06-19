using DieEngine.Equations.Extensions;
using DieEngine.Exceptions;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine.Equations
{
	public class EquationResolver : IEquationResolver
	{
		public double Process(string equation, IDictionary<string, double> inputs)
		{
			// resolve function with mxparser
			var exp = new Expression(equation);
			var func = new Function("random", new DiceFunctionExtension());
			exp.addFunctions(func);
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
				string[] missingFunctions = exp.getMissingUserDefinedFunctions();
				if (missingFunctions.Any())
					throw new UnknownCustomFunctionException($"Unknown functions: {string.Join(", ", missingFunctions)}");
				string[] missingValues = exp.getMissingUserDefinedArguments();
				if (missingValues.Any())
					throw new EquationInputArgumentException($"Missing params: {string.Join(", ", missingValues)}");
				throw new EquationInputArgumentException(exp.getErrorMessage());
			}
			return result;
		}
	}
}
