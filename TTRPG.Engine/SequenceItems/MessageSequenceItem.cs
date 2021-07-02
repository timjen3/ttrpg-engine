using TTRPG.Engine.Equations;
using System.Collections.Generic;
using FormatWith;

namespace TTRPG.Engine.SequenceItems
{
	/// <summary>
	///		Sequence item that creates a formatted message.
	/// </summary>
	public class MessageSequenceItem : ISequenceItem
	{
		public MessageSequenceItem(string name, string equation)
		{
			Name = name;
			Equation = equation;
		}

		public string Name { get; set; }
		public string Equation { get; set; }

		public SequenceItemResult GetResult(int order, IEquationResolver equationResolver, ref Dictionary<string, string> inputs, IDictionary<string, string> mappedInputs = null)
		{
			var result = new SequenceItemResult();
			result.Order = order;
			result.Inputs = mappedInputs;
			result.ResolvedItem = this;
			result.Result = Equation.FormatWith(mappedInputs);

			return result;
		}
	}
}
