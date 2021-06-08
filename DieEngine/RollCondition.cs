using DieEngine.Exceptions;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine
{
	public class RollCondition
	{
		const string DEFAULT_FAILURE_MESSAGE = "Die could not be rolled due to failed validation.";

		public RollCondition(){}

		public RollCondition(string equation, int order, bool throwOnFail = false, string failureMessage = null)
		{
			Equation = equation;
			Order = order;
			ThrowOnFail = throwOnFail;
			FailureMessage = failureMessage;
		}

		/// Equation to evaluate; >= 1 is true otherwise false
		public string Equation { get; set; }

		/// Die number to bind to; 0-indexed
		public int Order { get; set; }

		/// Whether exception should be thrown when Check fails
		public bool ThrowOnFail { get; set; }

		/// Custom message for failure if ThrowOnFail is true
		public string FailureMessage { get; set; }

		/// Determine if die passes condition (and should be rolled)
		public virtual bool Check(IDictionary<string, double> inputs)
		{
			var exp = new Expression(Equation);
			exp.removeAllConstants();  // reduce confusion from variables like "c" already existing
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
			var valid = result >= 1;
			if (!valid && ThrowOnFail)
			{
				throw new RollConditionFailedException(FailureMessage ?? DEFAULT_FAILURE_MESSAGE);
			}
			return valid;
		}
	}
}
