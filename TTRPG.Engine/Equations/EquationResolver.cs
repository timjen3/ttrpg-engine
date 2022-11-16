using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Jace;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.Equations
{
	/// Resolves an equation using jace
	public class EquationResolver : IEquationResolver
	{
		private readonly CalculationEngine _engine;

		/// <summary>
		///		The previous math parsing implementation supported injecting string variables
		///		Inputs could look like this:
		///			{"a": "1", "b": "random(1,2,3)"}
		///		so equation: "a + b" would be resolved as "a + random(1,2,3)"
		///		since jace doesn't support this, a tweak has been made where string inputs to be injected must have a key matching the regex: "^~.+~$"
		/// </summary>
		#region Embedded Functions
		private bool IsEmbeddedFunction(string value) => Regex.IsMatch(value, @"^~.+~$");

		private IDictionary<string, string> GetEmbeddedFunctions(IDictionary<string, string> inputs) => inputs
				.Where(kvp => IsEmbeddedFunction(kvp.Key))
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		private string InjectEmbeddedFunctions(string equation, IDictionary<string, string> inputs)
		{
			var specialInputs = GetEmbeddedFunctions(inputs);
			foreach (var specialInput in specialInputs)
			{
				equation = equation.Replace(specialInput.Key, specialInput.Value);
			}

			return equation;
		}
		#endregion

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
				equation = InjectEmbeddedFunctions(equation, inputs);
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
