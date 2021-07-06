using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Conditions
{
	public class Condition : ICondition
	{
		public const string DEFAULT_FAILURE_MESSAGE = "The condition failed.";

		public Condition() { }

		/// sequence-level condition
		public Condition(string equation)
		{
			if (string.IsNullOrWhiteSpace(equation)) throw new ArgumentNullException($"Argument {nameof(equation)} is required.");
			Equation = equation;
		}

		/// condition for 1 item
		public Condition(string itemName, string equation = null, string dependentOnItem = null, bool throwOnFail = false, string failureMessage = null)
			: this(new string[] { itemName }, equation, dependentOnItem, throwOnFail, failureMessage)
		{
		}

		/// condition for 1+ items
		public Condition(IEnumerable<string> itemNames, string equation = null, string dependentOnItem = null, bool throwOnFail = false, string failureMessage = null)
		{
			if (string.IsNullOrWhiteSpace(equation) && string.IsNullOrWhiteSpace(dependentOnItem)) throw new ArgumentNullException($"Either of arguments: {nameof(equation)} or {nameof(dependentOnItem)} are required.");
			if (itemNames == null || !itemNames.Any()) throw new ArgumentNullException($"Argument: {nameof(itemNames)} cannot be null for a sequence item condition.");
			ItemNames = itemNames;
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
		public IEnumerable<string> ItemNames { get; set; }

		/// Whether exception should be thrown when Check fails
		public bool ThrowOnFail { get; set; }

		/// Custom message for failure if ThrowOnFail is true
		public string FailureMessage { get; set; }

		/// Determine if the condition passes for the sequence
		public bool Check(IEquationResolver equationResolver, IDictionary<string, string> inputs)
		{
			return Check(null, equationResolver, inputs, null);
		}

		/// Determine if the condition passes for a sequence item
		public bool Check(string itemName, IEquationResolver equationResolver, IDictionary<string, string> inputs, SequenceResult results)
		{
			var valid = true;
			if (ItemNames == null || ItemNames.Contains(itemName))
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
