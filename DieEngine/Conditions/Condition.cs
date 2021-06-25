using DieEngine.Equations;
using DieEngine.Exceptions;
using DieEngine.SequencesItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine.Conditions
{
	public class Condition : ICondition
	{
		public const string DEFAULT_FAILURE_MESSAGE = "The condition failed.";

		public Condition() { }

		public Condition(string itemName, string equation = null, string dependentOnItem = null, bool throwOnFail = false, string failureMessage = null)
		{
			if (string.IsNullOrWhiteSpace(equation) && string.IsNullOrWhiteSpace(dependentOnItem)) throw new ArgumentNullException($"Either of arguments: {nameof(equation)} or {nameof(dependentOnItem)} are required.");
			ItemName = itemName;
			Equation = equation;
			DependentOnItem = dependentOnItem;
			ThrowOnFail = throwOnFail;
			FailureMessage = failureMessage;
		}

		/// Condition requires that this item was not skipped
		public string DependentOnItem { get; set; }

		/// Equation to evaluate; >= 1 is true otherwise false
		public string Equation { get; set; }

		/// Name of sequence item to bind to
		public string ItemName { get; set; }

		/// Whether exception should be thrown when Check fails
		public bool ThrowOnFail { get; set; }

		/// Custom message for failure if ThrowOnFail is true
		public string FailureMessage { get; set; }

		/// Determine if the condition fails based on input variables
		public virtual bool Check(string itemName, IEquationResolver equationResolver, IDictionary<string, string> inputs, SequenceResult results)
		{
			var valid = true;
			if (itemName == ItemName)
			{
				if (!string.IsNullOrWhiteSpace(DependentOnItem))
				{
					valid = results.Results.Any(y => string.Equals(y.ResolvedItem.Name, DependentOnItem, StringComparison.OrdinalIgnoreCase));
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
