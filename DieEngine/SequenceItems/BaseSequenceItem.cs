using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	public abstract class BaseSequenceItem : ISequenceItem
	{
		// todo: dependency injection
		static EquationResolver _equationResolver = new EquationResolver();

		public string Name { get; set; }
		public string Equation { get; set; }

		public SequenceItemResult GetResult(IDictionary<string, double> inputs = null)
		{
			var result = new SequenceItemResult();
			result.Inputs = inputs;
			result.ResolvedItem = this;
			result.Result = _equationResolver.Process(Equation, inputs);

			return result;
		}
	}
}
