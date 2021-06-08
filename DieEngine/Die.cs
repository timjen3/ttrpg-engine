using DieEngine.Algorithm;
using DieEngine.Exceptions;
using org.mariuszgromada.math.mxparser;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine
{
	public class Die
	{
		// todo: downsides of tight coupling here?
		private static CustomFunctionRunner _customFunctionRunner = new CustomFunctionRunner();

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
			dieRoll.RolledDie = this;
			// inject custom function results
			Equation = _customFunctionRunner.InsertEquations(Equation);
			// perform maths
			var exp = new Expression(Equation);
			exp.removeAllConstants();  // reduce confusion from variables like "c" already existing
			if (inputs != null)
			{
				dieRoll.Inputs = new Dictionary<string, double>(inputs);
				foreach (var kvp in inputs)
				{
					exp.addArguments(new Argument(kvp.Key.Trim(), kvp.Value));
				}
			}
			dieRoll.Result = exp.calculate();
			if (double.IsNaN(dieRoll.Result))
			{
				string[] missingValues = exp.getMissingUserDefinedArguments();
				if (missingValues.Any())
					throw new DieInputArgumentException($"Missing params: {string.Join(", ", missingValues)}");
				throw new DieInputArgumentException(exp.getErrorMessage());
			}

			return dieRoll;
		}
	}
}
