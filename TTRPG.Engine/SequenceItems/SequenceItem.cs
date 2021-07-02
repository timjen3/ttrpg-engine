using FormatWith;
using System.Collections.Generic;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.SequenceItems
{
	/// <summary>
	///		The result is loaded into inputs to be made available to following equations
	/// </summary>
	public class SequenceItem : ISequenceItem
	{
		public SequenceItem(){}

		public SequenceItem(string name, string equation, string resultName, SequenceItemType sequenceItemType)
		{
			Name = name;
			Equation = equation;
			ResultName = resultName;
			SequenceItemType = sequenceItemType;
		}

		public string Name { get; set; }
		public string Equation { get; set; }
		public string ResultName { get; set; }
		public SequenceItemType SequenceItemType { get; set; }

		public SequenceItemResult GetResult(int order, IEquationResolver equationResolver, ref Dictionary<string, string> inputs, IDictionary<string, string> mappedInputs = null)
		{
			var result = new SequenceItemResult();
			result.Order = order;
			result.Inputs = mappedInputs;
			result.ResolvedItem = this;
			if (SequenceItemType == SequenceItemType.Algorithm)
			{
				result.Result = equationResolver.Process(Equation, mappedInputs).ToString();
			}
			else if (SequenceItemType == SequenceItemType.Message)
			{
				result.Result = Equation.FormatWith(mappedInputs);
			}
			if (inputs != null)
			{
				inputs[ResultName] = result.Result.ToString();
			}
			return result;
		}
	}
}
