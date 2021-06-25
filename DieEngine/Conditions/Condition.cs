using DieEngine.Equations;
using DieEngine.Exceptions;
using DieEngine.SequencesItems;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine.Conditions
{
	public class Condition : ICondition
	{
		public const string DEFAULT_FAILURE_MESSAGE = "The condition failed.";

		public Condition() { }

		public Condition(string equation, int order, bool throwOnFail = false, string failureMessage = null)
		{
			Order = order;
			Equation = equation;
			ThrowOnFail = throwOnFail;
			FailureMessage = failureMessage;
		}

		public Condition(int order, int dependency, bool throwOnFail = false, string failureMessage = null)
		{
			Order = order;
			Dependency = dependency;
			ThrowOnFail = throwOnFail;
			FailureMessage = failureMessage;
		}

		public Condition(int order, string equation = null, int? dependency = null, bool throwOnFail = false, string failureMessage = null)
		{
			Order = order;
			Equation = equation;
			Dependency = dependency;
			ThrowOnFail = throwOnFail;
			FailureMessage = failureMessage;
		}

		/// Condition requires item with this order having been processed
		public int? Dependency { get; set; }

		/// Equation to evaluate; >= 1 is true otherwise false
		public string Equation { get; set; }

		/// Sequence number to bind to; 0-indexed
		public int Order { get; set; }

		/// Whether exception should be thrown when Check fails
		public bool ThrowOnFail { get; set; }

		/// Custom message for failure if ThrowOnFail is true
		public string FailureMessage { get; set; }

		/// Determine if the condition fails based on input variables
		public virtual bool Check(int order, IEquationResolver equationResolver, IDictionary<string, string> inputs, SequenceResult results)
		{
			var valid = true;
			if (order == Order)
			{
				if (Dependency.HasValue)
				{
					valid = results.Results.Any(y => y.Order == Dependency);
				}
				// only check equation if dependency is fulfilled; since dependency may be responsible for defining requisite variables
				if (valid && !string.IsNullOrWhiteSpace(Equation))
				{
					var result = equationResolver.Process(Equation, inputs);
					valid = result >= 1;
				}
			}
			if (!valid && ThrowOnFail)
			{
				throw new ConditionFailedException(FailureMessage ?? DEFAULT_FAILURE_MESSAGE);
			}
			return valid;
		}
	}
}
