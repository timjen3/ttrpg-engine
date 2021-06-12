using DieEngine.CustomFunctions;
using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	public abstract class BaseSequenceItem : ISequenceItem
	{
		// todo: dependency injection
		static EquationResolver _equationResolver = new EquationResolver();

		public string Name { get; set; }
		public string Equation { get; set; }

		// Roll this die
		public SequenceItemResult GetResult(IDictionary<string, double> inputs = null)
		{
			var dieRoll = new SequenceItemResult();
			dieRoll.Inputs = inputs;
			dieRoll.ResolvedItem = this;
			dieRoll.Result = _equationResolver.Process(Equation, inputs);

			return dieRoll;
		}
	}
}
