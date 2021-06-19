using DieEngine.CustomFunctions;
using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	/// <summary>
	///		The result is loaded into inputs to be made available to following equations
	/// </summary>
	public class DieSequenceItem : BaseSequenceItem
	{
		public string ResultName { get; set; }

		public DieSequenceItem(){}

		public DieSequenceItem(string name, string equation, string resultName)
		{
			Name = name;
			Equation = equation;
			ResultName = resultName;
		}

		public override SequenceItemResult GetResult(IEquationResolver equationResolver, ref Dictionary<string, double> inputs, IDictionary<string, double> mappedInputs = null)
		{
			var result = base.GetResult(equationResolver, mappedInputs);
			if (inputs != null)
			{
				inputs[ResultName] = result.Result;
			}

			return result;
		}
	}
}
