using System;
using System.Collections.Generic;
using System.Linq;

namespace TTRPG.Engine
{
	/// Condition on Sequence or SequenceItem
	public class Condition
	{
		internal const string DEFAULT_FAILURE_MESSAGE = "The condition failed.";

		/// constructor for condition
		public Condition() { }

		/// constructor for sequence-level condition
		public Condition(string equation)
		{
			if (string.IsNullOrWhiteSpace(equation)) throw new ArgumentNullException($"Argument {nameof(equation)} is required.");
			Equation = equation;
		}

		/// constructor for condition for 1 item
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
	}
}
