using DieEngine.Exceptions;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine
{
	public class RollCondition
	{
		public RollCondition(){}

		public RollCondition(string equation)
		{
			Equation = equation;
		}

		/// Equation to evaluate; >= 1 is true otherwise false
		public virtual string Equation { get; set; }

		/// Determine if die should be rolled
		public virtual bool ShouldRoll(IDictionary<string, double> inputs)
		{
			var exp = new Expression(Equation);
			if (inputs != null)
			{
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
					throw new ConditionInputArgumentException($"Missing params: {string.Join(", ", missingValues)}");
				throw new ConditionInputArgumentException(exp.getErrorMessage());
			}
			return result >= 1;
		}
	}
}
