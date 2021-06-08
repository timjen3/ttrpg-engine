using System.Collections.Generic;
using System.Linq;

namespace DieEngine
{
	public class DieSequence
	{
		public string Name { get; set; }

		public List<Die> Dice { get; set; } = new List<Die>();

		public List<RollCondition> Conditions { get; set; } = new List<RollCondition>();

		public DieSequenceResult RollAll(IDictionary<string, double> inputs = null)
		{
			inputs = inputs ?? new Dictionary<string, double>();
			var result = new DieSequenceResult();
			for (int dieNum = 0; dieNum < Dice.Count; dieNum++)
			{
				var die = Dice[dieNum];
				var conditions = Conditions.Where(x => x.Order == dieNum);
				var valid = true;
				foreach (var condition in conditions)
				{
					if (!condition.Check(inputs))
						valid = false;
				}
				if (!valid) continue;
				DieRoll roll = die.Roll(inputs);
				inputs[die.ResultName] = roll.Result;
				result.Rolls.Add(roll);
			}
			return result;
		}
	}
}
