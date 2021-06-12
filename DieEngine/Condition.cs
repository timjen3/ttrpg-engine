using DieEngine.CustomFunctions;
using DieEngine.Exceptions;
using System.Collections.Generic;

namespace DieEngine
{
	public class Condition
	{
		// todo: dependency injection
		static readonly EquationResolver _equationResolver = new EquationResolver();
		const string DEFAULT_FAILURE_MESSAGE = "The condition failed.";
		public Condition(){}

		public Condition(string equation, int order, bool throwOnFail = false, string failureMessage = null)
		{
			Equation = equation;
			Order = order;
			ThrowOnFail = throwOnFail;
			FailureMessage = failureMessage;
		}

		/// Equation to evaluate; >= 1 is true otherwise false
		public string Equation { get; set; }

		/// Sequence number to bind to; 0-indexed
		public int Order { get; set; }

		/// Whether exception should be thrown when Check fails
		public bool ThrowOnFail { get; set; }

		/// Custom message for failure if ThrowOnFail is true
		public string FailureMessage { get; set; }

		/// Determine if the condition fails based on input variables
		public virtual bool Check(IDictionary<string, double> inputs)
		{
			var result = _equationResolver.Process(Equation, inputs);
			var valid = result >= 1;
			if (!valid && ThrowOnFail)
			{
				throw new ConditionFailedException(FailureMessage ?? DEFAULT_FAILURE_MESSAGE);
			}
			return valid;
		}
	}
}
