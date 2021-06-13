using DieEngine.SequencesItems;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine
{
	public class Sequence
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

		public List<ISequenceItem> Items { get; set; } = new List<ISequenceItem>();

		public List<Condition> Conditions { get; set; } = new List<Condition>();

		/// <summary>
		///		Renames input variables according to mappings before using them in conditions or sequence item equations.
		///		The inputs are always copied to a new dictionary before changes are made to isolate changes to equations and reduce side effects.
		/// </summary>
		public List<Mapping> Mappings { get; set; } = new List<Mapping>();

		public SequenceResult RollAll(IDictionary<string, double> inputs = null)
		{
			inputs = inputs ?? new Dictionary<string, double>();
			var result = new SequenceResult();
			for (int i = 0; i < Items.Count; i++)
			{
				var item = Items[i];
				var conditions = Conditions.Where(x => x.Order == i);
				var mappedInputs = GetMappedInputs(i, inputs);
				var isValid = true;
				foreach (var condition in conditions)
				{
					isValid = condition.Check(mappedInputs);
				}
				if (!isValid) continue;
				SequenceItemResult itemResult = item.GetResult(mappedInputs);
				if (item is DieSequenceItem die)  // make result available to following equations
				{
					inputs[die.ResultName] = itemResult.Result;
				}
				result.Results.Add(itemResult);
			}
			return result;
		}
	}
}
