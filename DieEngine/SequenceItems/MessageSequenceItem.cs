using DieEngine.Equations;
using System.Collections.Generic;
using FormatWith;

namespace DieEngine.SequencesItems
{
	/// <summary>
	///		Sequence item that creates a formatted message.
	/// </summary>
	public class MessageSequenceItem : BaseSequenceItem
	{
		public MessageSequenceItem(string name, string equation, bool publishResult = true)
		{
			Name = name;
			Equation = equation;
			PublishResult = publishResult;
		}

		public override SequenceItemResult GetResult(int order, IEquationResolver equationResolver, ref Dictionary<string, string> inputs, IDictionary<string, string> mappedInputs = null)
		{
			var result = new SequenceItemResult();
			result.Inputs = mappedInputs;
			result.ResolvedItem = this;
			result.Result = Equation.FormatWith(mappedInputs);

			return result;
		}
	}
}
