using FormatWith;
using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Equations
{
	/// contains logic for sequence components
	public class EquationService
	{
		private readonly IEquationResolver _equationResolver;

		/// EquationService Constructor
		public EquationService(IEquationResolver equationResolver)
		{
			_equationResolver = equationResolver;
		}

		/// Returns source data depending on MappingType
		IDictionary<string, string> GetSourceMappingData(Mapping mapping, Dictionary<string, string> inputs, IEnumerable<Role> roles)
		{
			switch (mapping.MappingType)
			{
				case MappingType.Role:
					{
						var role = roles.SingleOrDefault(x => x.Name.Equals(mapping.RoleName, StringComparison.OrdinalIgnoreCase));
						if (mapping.ThrowOnFailure && role == null) throw new MissingRoleException($"Mapping failed due to missing role: '{mapping.RoleName}'.");

						return role.Attributes;
					}
				case MappingType.Input:
				default:
					{
						return inputs;
					}
			}
		}

		/// Determine if the condition passes for the sequence
		public bool Check(Condition condition, IDictionary<string, string> inputs)
		{
			return Check(condition, null, inputs, null);
		}

		/// Determine if the condition passes for a sequence item
		public bool Check(Condition condition, string itemName, IDictionary<string, string> inputs, SequenceResult results)
		{
			var valid = true;
			if (condition.ItemNames == null || condition.ItemNames.Contains(itemName))
			{
				if (!string.IsNullOrWhiteSpace(condition.DependentOnItem))
				{
					valid = results.Results.Any(y => string.Equals(y.ResolvedItem.Name, condition.DependentOnItem, StringComparison.OrdinalIgnoreCase));
				}
				// only check equation if dependency is fulfilled; since dependency may be responsible for defining requisite variables
				if (valid && !string.IsNullOrWhiteSpace(condition.Equation))
				{
					var result = _equationResolver.Process(condition.Equation, inputs);
					valid = result >= 1;
				}
			}
			if (!valid && condition.ThrowOnFail)
			{
				throw new ConditionFailedException(condition.FailureMessage ?? Condition.DEFAULT_FAILURE_MESSAGE);
			}
			return valid;
		}

		/// Injects the specified mapping into <param name="inputs"/> from the configured source determined by <see cref="MappingType"/>
		/// <param name="itemName">The sequence item name mapping is being applied for. If the mapping does not apply to that item nothing will happen.</param>
		/// <param name="roles">All roles available to the sequence.</param>
		public void Apply(Mapping mapping, string itemName, ref Dictionary<string, string> inputs, IEnumerable<Role> roles)
		{
			if (string.IsNullOrWhiteSpace(mapping.ItemName) || string.Equals(itemName, mapping.ItemName, StringComparison.OrdinalIgnoreCase))
			{
				inputs[mapping.To] = "0";
				var source = GetSourceMappingData(mapping, inputs, roles);
				if (source.ContainsKey(mapping.From))
				{
					inputs[mapping.To] = source[mapping.From];
				}
				if (mapping.ThrowOnFailure) throw new MappingFailedException($"Mapping failed due to missing key: '{mapping.From}'.");
			}
		}

		/// Process a sequence item and get the result
		public SequenceItemResult GetResult(SequenceItem item, int order, ref Dictionary<string, string> inputs, IDictionary<string, string> mappedInputs = null)
		{
			var result = new SequenceItemResult();
			result.Order = order;
			result.Inputs = mappedInputs;
			result.ResolvedItem = item;
			if (item.SequenceItemType == SequenceItemType.Algorithm)
			{
				result.Result = _equationResolver.Process(item.Equation, mappedInputs).ToString();
			}
			else if (item.SequenceItemType == SequenceItemType.Message)
			{
				result.Result = item.Equation.FormatWith(mappedInputs);
			}
			if (inputs != null)
			{
				inputs[item.ResultName] = result.Result.ToString();
			}
			return result;
		}

		/// <summary>
		///		Check if sequence can be executed with the provided parameters
		/// </summary>
		/// <param name="inputs"></param>
		/// <param name="roles"></param>
		/// <returns></returns>
		public bool Check(Sequence sequence, Dictionary<string, string> inputs = null, IEnumerable<Role> roles = null)
		{
			inputs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);  // isolate changes to this method
			var mappedInputs = new Dictionary<string, string>(inputs, StringComparer.OrdinalIgnoreCase);  // isolate mapping changes to current sequence item
			sequence.Mappings.ForEach(x => Apply(x, null, ref mappedInputs, roles));

			return sequence.Conditions.All(x => Check(x, mappedInputs));
		}

		/// <summary>
		///		Process sequence items and get result
		/// </summary>
		/// <param name="inputs"></param>
		/// <param name="roles"></param>
		/// <returns></returns>
		public SequenceResult Process(Sequence sequence, Dictionary<string, string> inputs = null, IEnumerable<Role> roles = null)
		{
			inputs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);  // isolate changes to this method
			var result = new SequenceResult();
			result.Sequence = sequence;
			for (int order = 0; order < sequence.Items.Count; order++)
			{
				var item = sequence.Items[order];
				var mappedInputs = new Dictionary<string, string>(inputs, StringComparer.OrdinalIgnoreCase);  // isolate mapping changes to current sequence item
				sequence.Mappings.ForEach(x => Apply(x, item.Name, ref mappedInputs, roles));
				if (!sequence.Conditions.All(x => Check(x, item.Name, mappedInputs, result))) continue;
				var itemResult = GetResult(item, order, ref inputs, mappedInputs);
				result.Results.Add(itemResult);
			}
			result.Output = inputs;

			return result;
		}
	}
}
