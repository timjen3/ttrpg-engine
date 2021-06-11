using DieEngine.Algorithm;
using System.Collections.Generic;

namespace DieEngine
{
	public class Die
	{
		// todo: dependency injection
		static EquationResolver _equatonResolver = new EquationResolver();

		public Die(){}

		public Die(string name, string equation, string resultName)
		{
			Name = name;
			Equation = equation;
			ResultName = resultName;
		}

		// Name of Die
		public string Name { get; set; }

		// What the result of this die roll should be called
		public string ResultName { get; set; }

		// Algorithm for this die
		public string Equation { get; set; }

		// Roll this die
		public DieRoll Roll(IDictionary<string, double> inputs = null)
		{
			var dieRoll = new DieRoll();
			dieRoll.Inputs = inputs;
			dieRoll.RolledDie = this;
			dieRoll.Result = _equatonResolver.Process(Equation, inputs);

			return dieRoll;
		}
	}
}
