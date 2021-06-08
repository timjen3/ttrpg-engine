using System.Collections.Generic;
using System.Linq;

namespace DieEngine
{
	public class DieSequence
	{
		/// <summary>
		///		Renames input variables per the mappings.
		///		If no mapping is present, the variable is passed in as-is.
		/// </summary>
		/// <param name="inputs"></param>
		/// <returns></returns>
		private IDictionary<string, double> GetMappedInputs(int order, IDictionary<string, double> inputs)
		{
			var mappings = Mappings.SingleOrDefault(x => x.Order == order);
			if (mappings == null) return inputs;

			var mappedInputs = new Dictionary<string, double>(inputs);
			foreach (var input in inputs)
			{
				if (mappings.Mappings.ContainsKey(input.Key))
				{
					mappedInputs[mappings.Mappings[input.Key]] = input.Value;
					continue;
				}
				mappedInputs[input.Key] = input.Value;
			}
			return mappedInputs;
		}

		public string Name { get; set; }

		public List<Die> Dice { get; set; } = new List<Die>();

		public List<RollCondition> Conditions { get; set; } = new List<RollCondition>();

		public List<DieMapping> Mappings { get; set; } = new List<DieMapping>();

		public DieSequenceResult RollAll(IDictionary<string, double> inputs = null)
		{
			inputs = inputs ?? new Dictionary<string, double>();
			var result = new DieSequenceResult();
			for (int dieNum = 0; dieNum < Dice.Count; dieNum++)
			{
				var die = Dice[dieNum];
				var conditions = Conditions.Where(x => x.Order == dieNum);
				var valid = true;
				var mappedInputs = GetMappedInputs(dieNum, inputs);
				foreach (var condition in conditions)
				{
					if (!condition.Check(mappedInputs))
						valid = false;
				}
				if (!valid) continue;
				DieRoll roll = die.Roll(mappedInputs);
				inputs[die.ResultName] = roll.Result;
				result.Rolls.Add(roll);
			}
			return result;
		}
	}
}
