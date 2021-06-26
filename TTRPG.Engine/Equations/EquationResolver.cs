using TTRPG.Engine.Equations.Extensions;
using TTRPG.Engine.Exceptions;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;

namespace TTRPG.Engine.Equations
{
	public class EquationResolver : IEquationResolver
	{
		public double Process(string equation, IDictionary<string, string> inputs)
		{
			// resolve function with mxparser
			var exp = new Expression(equation);
			var func = new Function("random", new RandomFunctionExtension());
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
