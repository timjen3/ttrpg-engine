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

		public Die(string name, string equation, string resultName, Dictionary<string, string> mappings = null)
		{
			Name = name;
			Equation = equation;
			ResultName = resultName;
			Mappings = mappings ?? new Dictionary<string, string>();
		}

		/// <summary>
		///		Renames input variables per the mappings.
		///		If no mapping is present, the variable is passed in as-is.
		/// </summary>
		/// <param name="inputs"></param>
		/// <returns></returns>
		protected IDictionary<string, double> GetMappedInputs(IDictionary<string, double> inputs)
		{
			var mappedInputs = new Dictionary<string, double>();
			foreach (var input in inputs)
			{
				if (Mappings.ContainsKey(input.Key))
				{
					mappedInputs[Mappings[input.Key]] = input.Value;
					continue;
				}
				mappedInputs[input.Key] = input.Value;
			}
			return mappedInputs;
		}

		// Name of Die
		public string Name { get; set; }

		// What the result of this die roll should be called
		public string ResultName { get; set; }

		// Renames input variables before plugging them into the formula
		public Dictionary<string, string> Mappings { get; set; } = new Dictionary<string, string>();

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
			if (inputs != null)
			{
				dieRoll.Inputs = new Dictionary<string, double>(inputs);
				var mappedInputs = GetMappedInputs(inputs);
				foreach (var kvp in mappedInputs)
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
