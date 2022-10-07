using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.Equations
{
	/// Resolves an equation using mxParser
	public class EquationResolver : IEquationResolver
	{
		private readonly Dictionary<string, Expression> _compiledFunctions = new Dictionary<string, Expression>();
		private readonly Function[] _functions;

		private Expression GetExpression(string equation)
		{
			if (!_compiledFunctions.ContainsKey(equation))
			{
				_compiledFunctions[equation] = new Expression(equation);
				_compiledFunctions[equation].removeAllConstants();  // reduce confusion from variables like "c" already existing
				_compiledFunctions[equation].addFunctions(_functions);
			}
			return _compiledFunctions[equation];
		}

		private void SetArguments(Expression exp, IDictionary<string, string> inputs)
		{
			if (inputs != null)
			{
				foreach (var kvp in inputs)
				{
					Argument arg = exp.getArgument(kvp.Key.Trim());
					if (arg == null)
					{
						arg = new Argument(kvp.Key.Trim(), kvp.Value);
						exp.addArguments(arg);
					}
					if (double.TryParse(kvp.Value, out double parsedDouble))
					{
						arg.setArgumentValue(parsedDouble);
					}
					else
					{
						// unlike the setArgumentValue method this causes recompilation
						arg.setArgumentExpressionString(kvp.Value);
					}
				}
			}
		}

		/// Constructor for Equation Resolver
		public EquationResolver(IEnumerable<Function> functions)
		{
			_functions = functions.ToArray();
		}

		/// adds inputs as arguments and resolves equation with mxParser
		public double Process(string equation, IDictionary<string, string> inputs)
		{
			var exp = GetExpression(equation);
			// resolve function with mxparser
			SetArguments(exp, inputs);
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
