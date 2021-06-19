using DieEngine.Equations;
using DieEngine.Mappings;
using DieEngine.SequencesItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine
{
	public class Sequence
	{
		public IEqualityComparer<string> KeyComparer { get; set; } = StringComparer.OrdinalIgnoreCase;

		public string Name { get; set; }

		public List<ISequenceItem> Items { get; set; } = new List<ISequenceItem>();

		public List<ICondition> Conditions { get; set; } = new List<ICondition>();

		public List<IMapping> Mappings { get; set; } = new List<IMapping>();

		public SequenceResult Process(IEquationResolver equationResolver, Dictionary<string, double> inputs = null, IEnumerable<Role> roles = null)
		{
			inputs = new Dictionary<string, double>(inputs ?? new Dictionary<string, double>(), KeyComparer);  // isolate changes to this method
			var result = new SequenceResult();
			for (int order = 0; order < Items.Count; order++)
			{
				var item = Items[order];
				var mappedInputs = new Dictionary<string, double>(inputs, KeyComparer);  // isolate mapping changes to current sequence item
				Mappings.ForEach(x => x.Apply(order, ref mappedInputs, roles));
				if (!Conditions.All(x => x.Check(order, equationResolver, mappedInputs))) continue;
				var itemResult = item.GetResult(equationResolver, ref inputs, mappedInputs);
				result.Results.Add(itemResult);
			}
			return result;
		}
	}
}
