using TTRPG.Engine.Equations;
using System.Collections.Generic;

namespace TTRPG.Engine.SequenceItems
{
	/// <summary>
	///		The result is loaded into inputs to be made available to following equations
	/// </summary>
	public class DieSequenceItem : BaseSequenceItem
	{
		public string ResultName { get; set; }

		public DieSequenceItem(){}

		public DieSequenceItem(string name, string equation, string resultName, bool publishResult = true)
		{
			Name = name;
			Equation = equation;
			ResultName = resultName;
			PublishResult = publishResult;
		}

		public override SequenceItemResult GetResult(int order, IEquationResolver equationResolver, ref Dictionary<string, string> inputs, IDictionary<string, string> mappedInputs = null)
		{
			var result = base.GetResult(order, equationResolver, mappedInputs);
			if (inputs != null)
			{
				inputs[ResultName] = result.Result.ToString();
			}

			return result;
		}
	}
}
