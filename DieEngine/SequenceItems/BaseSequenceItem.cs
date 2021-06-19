using DieEngine.Equations;
using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	public abstract class BaseSequenceItem : ISequenceItem
	{
		public string Name { get; set; }
		public string Equation { get; set; }

		protected SequenceItemResult GetResult(IEquationResolver equationResolver, IDictionary<string, double> inputs)
		{
			var result = new SequenceItemResult();
			result.Inputs = inputs;
			result.ResolvedItem = this;
			result.Result = equationResolver.Process(Equation, inputs);

			return result;
		}

		public abstract SequenceItemResult GetResult(IEquationResolver equationResolver, ref Dictionary<string, double> mappedInputs, IDictionary<string, double> sharedInputs);
	}
}
