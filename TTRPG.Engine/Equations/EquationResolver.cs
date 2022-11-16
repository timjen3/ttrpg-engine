using System;
using System.Collections.Generic;
using System.Linq;
using Jace;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.Equations
{
	/// Resolves an equation using mxParser
	public class EquationResolver : IEquationResolver
	{
		private readonly CalculationEngine _engine;

		private IDictionary<string, double> GetDoubleInputs(IDictionary<string, string> inputs) => inputs
				.Where(kvp => double.TryParse(kvp.Value, out double _))
				.ToDictionary(kvp => kvp.Key, kvp => double.Parse(kvp.Value));

		/// Constructor for Equation Resolver
		public EquationResolver(CalculationEngine engine) => _engine = engine;

		/// adds inputs as arguments and resolves equation with jace.net
		public double Process(string equation, IDictionary<string, string> inputs)
		{
			try
			{
				var dInputs = GetDoubleInputs(inputs);
				var result = _engine.Calculate(equation, dInputs);

				return result;
			}
			catch (VariableNotDefinedException ex)
			{
				throw new CustomFunctionArgumentException(equation, ex);
			}
			catch (Exception ex)
			{
				throw new EquationResolverException(equation, ex);
			}
		}
	}
}
