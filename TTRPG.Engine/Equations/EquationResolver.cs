using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.Equations
{
	/// Resolves an equation using mxParser
	public class EquationResolver : IEquationResolver
	{
		private readonly Function[] _functions;

		/// Constructor for Equation Resolver
		public EquationResolver(IEnumerable<Function> functions)
		{
			_functions = functions.ToArray();
		}

		/// adds inputs as arguments and resolves equation with mxParser
		public double Process(string equation, IDictionary<string, string> inputs)
		{
			// resolve function with mxparser
			var exp = new Expression(equation);
			exp.addFunctions(_functions);
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
