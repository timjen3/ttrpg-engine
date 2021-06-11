using System.Collections.Generic;
using System.Linq;

namespace DieEngine
{
	public class DieSequence
	{
		private IDictionary<string, double> GetMappedInputs(int order, IDictionary<string, double> inputs)
		{
			var mappedInputs = new Dictionary<string, double>(inputs);  // copy to prevent any changes to source inputs
			var mappings = Mappings.SingleOrDefault(x => x.Order == order);
			if (mappings == null) return mappedInputs;

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

		public List<Condition> Conditions { get; set; } = new List<Condition>();

		/// <summary>
		///		Renames input variables according to mappings before using them in conditions or die rolls.
		///		The inputs are always copied to a new dictionary before changes are made to isolate changes for each roll.
		/// </summary>
		public List<DieMapping> Mappings { get; set; } = new List<DieMapping>();

		public DieSequenceResult RollAll(IDictionary<string, double> inputs = null)
		{
			inputs = inputs ?? new Dictionary<string, double>();
			var result = new DieSequenceResult();
			for (int dieNum = 0; dieNum < Dice.Count; dieNum++)
			{
				var die = Dice[dieNum];
				var conditions = Conditions.Where(x => x.Order == dieNum);
				var mappedInputs = GetMappedInputs(dieNum, inputs);
				var isValid = true;
				foreach (var condition in conditions)
				{
					isValid = condition.Check(mappedInputs);
				}
				if (!isValid) continue;
				DieRoll roll = die.Roll(mappedInputs);
				inputs[die.ResultName] = roll.Result;
				result.Rolls.Add(roll);
			}
			return result;
		}
	}
}
