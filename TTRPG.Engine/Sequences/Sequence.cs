using TTRPG.Engine.Conditions;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Mappings;
using TTRPG.Engine.SequenceItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TTRPG.Engine.Sequences
{
	public class Sequence
	{
		static IEqualityComparer<string> KeyComparer = StringComparer.OrdinalIgnoreCase;

		public string Name { get; set; }

		public List<ISequenceItem> Items { get; set; } = new List<ISequenceItem>();

		public List<ICondition> Conditions { get; set; } = new List<ICondition>();

		public List<IMapping> Mappings { get; set; } = new List<IMapping>();

		/// <summary>
		///		Check if sequence can be executed with the provided parameters
		/// </summary>
		/// <param name="equationResolver"></param>
		/// <param name="inputs"></param>
		/// <param name="roles"></param>
		/// <returns></returns>
		public bool Check(IEquationResolver equationResolver, Dictionary<string, string> inputs = null, IEnumerable<Role> roles = null)
		{
			inputs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), KeyComparer);  // isolate changes to this method
			var mappedInputs = new Dictionary<string, string>(inputs, KeyComparer);  // isolate mapping changes to current sequence item
			Mappings.ForEach(x => x.Apply(null, ref mappedInputs, roles));

			return Conditions.All(x => x.Check(equationResolver, mappedInputs));
		}

		/// <summary>
		///		Process sequence items and get result
		/// </summary>
		/// <param name="equationResolver"></param>
		/// <param name="inputs"></param>
		/// <param name="roles"></param>
		/// <returns></returns>
		public SequenceResult Process(IEquationResolver equationResolver, Dictionary<string, string> inputs = null, IEnumerable<Role> roles = null)
		{
			inputs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), KeyComparer);  // isolate changes to this method
			var result = new SequenceResult();
			result.Sequence = this;
			for (int order = 0; order < Items.Count; order++)
			{
				var item = Items[order];
				var mappedInputs = new Dictionary<string, string>(inputs, KeyComparer);  // isolate mapping changes to current sequence item
				Mappings.ForEach(x => x.Apply(item.Name, ref mappedInputs, roles));
				if (!Conditions.All(x => x.Check(item.Name, equationResolver, mappedInputs, result))) continue;
				var itemResult = item.GetResult(order, equationResolver, ref inputs, mappedInputs);
				result.Results.Add(itemResult);
			}
			result.Output = inputs;  // final state of input becomes output

			return result;
		}
	}
}
