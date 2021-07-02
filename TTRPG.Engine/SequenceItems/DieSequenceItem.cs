using TTRPG.Engine.Equations;
using System.Collections.Generic;

namespace TTRPG.Engine.SequenceItems
{
	/// <summary>
	///		The result is loaded into inputs to be made available to following equations
	/// </summary>
	public class DieSequenceItem : ISequenceItem
	{
		public DieSequenceItem(){}

		public DieSequenceItem(string name, string equation, string resultName)
		{
			Name = name;
			Equation = equation;
			ResultName = resultName;
		}

		public string Name { get; set; }
		public string Equation { get; set; }
		public string ResultName { get; set; }

		public SequenceItemResult GetResult(int order, IEquationResolver equationResolver, ref Dictionary<string, string> inputs, IDictionary<string, string> mappedInputs = null)
		{
			var result = new SequenceItemResult();
			result.Order = order;
			result.Inputs = mappedInputs;
			result.ResolvedItem = this;
			result.Result = equationResolver.Process(Equation, mappedInputs).ToString();
			if (inputs != null)
			{
				inputs[ResultName] = result.Result.ToString();
			}
			return result;
		}
	}
}
