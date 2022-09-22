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
	public class EquationService : IEquationService
	{
		private readonly IEquationResolver _equationResolver;

		/// Returns source data depending on MappingType
		IDictionary<string, string> GetSourceMappingData(Mapping mapping, Dictionary<string, string> inputs, IEnumerable<Role> roles)
		{
			IDictionary<string, string> source;
			switch (mapping.MappingType)
			{
				case MappingType.InventoryItem:
				case MappingType.Role:
					{
						Role role = null;
						if (mapping.RoleName == null)
						{
							role = roles.FirstOrDefault();
							if (mapping.ThrowOnFailure && role == null) throw new MissingRoleException($"Mapping failed due to no roles being passed.");
						}
						else
						{
							role = roles.SingleOrDefault(x => x.Alias != null && x.Alias.Equals(mapping.RoleName, StringComparison.OrdinalIgnoreCase));
							if (mapping.ThrowOnFailure && role == null) throw new MissingRoleException($"Mapping failed due to missing role: '{mapping.RoleName}'.");
						}
						if (mapping.InventoryItemName != null)
						{
							Role item = null;
							if (role != null && role.InventoryItems.ContainsKey(mapping.InventoryItemName))
								item = role.InventoryItems[mapping.InventoryItemName];
							if (mapping.ThrowOnFailure && item == null) throw new MissingRoleException($"Mapping failed due to role not having item: '{mapping.InventoryItemName}'.");
							source = item?.Attributes;
							break;
						}

						source = role?.Attributes;
						break;
					}
				case MappingType.Input:
				default:
					{
						source = inputs;
						break;
					}
			}
			return source ?? new Dictionary<string, string>();
		}

		/// Checks role conditions against provided roles
		bool CheckRoleConditions(Sequence sequence, IEnumerable<Role> roles)
		{
			var roleConditionsMet = sequence.RoleConditions == null || sequence.RoleConditions.All(condition =>
			{
				var target = roles?.FirstOrDefault(role => role?.Name.Equals(condition.RoleName) ?? false);
				if (target == null) return false;

				return condition.RequiredCategories.All(x => target.Categories.Contains(x, StringComparer.OrdinalIgnoreCase));
			});

			return roleConditionsMet;
		}

		/// EquationService Constructor
		public EquationService(IEquationResolver equationResolver)
		{
			_equationResolver = equationResolver;
		}

		/// Determine if the condition passes for the sequence
		internal bool Check(Condition condition, IDictionary<string, string> inputs)
		{
			return Check(condition, null, inputs, null);
		}

		/// Determine if the condition passes for a sequence item
		internal bool Check(Condition condition, string itemName, IDictionary<string, string> inputs, SequenceResult results)
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
		internal void Apply(Mapping mapping, string itemName, ref Dictionary<string, string> inputs, IEnumerable<Role> roles)
		{
			if (string.IsNullOrWhiteSpace(mapping.ItemName) || string.Equals(itemName, mapping.ItemName, StringComparison.OrdinalIgnoreCase))
			{
				inputs[mapping.To] = "0";
				var source = GetSourceMappingData(mapping, inputs, roles);
				if (source.ContainsKey(mapping.From))
				{
					inputs[mapping.To] = source[mapping.From];
				}
				else if (mapping.ThrowOnFailure) throw new MappingFailedException($"Mapping failed due to missing key: '{mapping.From}'.");
			}
		}

		/// Process a sequence item and get the result
		internal SequenceItemResult GetResult(SequenceItem item, int order, ref Dictionary<string, string> inputs, IDictionary<string, string> mappedInputs = null)
		{
			var result = new SequenceItemResult();
			result.Order = order;
			result.Inputs = mappedInputs;
			result.ResolvedItem = item;
			if (item.SequenceItemEquationType == SequenceItemEquationType.Algorithm)
			{
				result.Result = _equationResolver.Process(item.Equation, mappedInputs).ToString();
			}
			else if (item.SequenceItemEquationType == SequenceItemEquationType.Message)
			{
				result.Result = item.Equation.FormatWith(mappedInputs);
			}
			if (inputs != null)
			{
				inputs[item.ResultName] = result.Result.ToString();
			}
			return result;
		}

		/// Process a sequence item and get the result
		internal List<SequenceResultItem> ProcessResults(IEnumerable<ResultItem> items, IDictionary<string, string> inputs, IEnumerable<Role> roles)
		{
			var results = new List<SequenceResultItem>();
			foreach (var item in items)
			{
				var result = new SequenceResultItem();
				result.Name = item.Name;
				result.Category = item.Category;
				if (inputs.TryGetValue(item.Source, out string value))
				{
					result.Result = value;
					if (!string.IsNullOrWhiteSpace(item.RoleName))
					{
						result.Role = roles.SingleOrDefault(x => x.Alias != null && x.Alias.Equals(item.RoleName, StringComparison.OrdinalIgnoreCase));
					}
					else if (item.FirstRole)
					{
						result.Role = roles.FirstOrDefault();
					}
					results.Add(result);
				}
			}

			return results;
		}

		/// <see cref="IEquationService.Process(SequenceItem, Role, IDictionary{string, string})"/>
		public SequenceItemResult Process(SequenceItem item, Role role = null, IDictionary<string, string> inputs = null)
		{
			var sArgs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);  // isolate changes to this method
			var result = new SequenceItemResult();
			result.Order = 0;
			result.Inputs = sArgs;
			result.ResolvedItem = item;
			var mappedInputs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);
			if (role != null)
			{
				foreach (var kvp in role.Attributes) mappedInputs[kvp.Key] = kvp.Value;
			}
			if (item.SequenceItemEquationType == SequenceItemEquationType.Algorithm)
			{
				result.Result = _equationResolver.Process(item.Equation, mappedInputs).ToString();
			}
			else if (item.SequenceItemEquationType == SequenceItemEquationType.Message)
			{
				result.Result = item.Equation.FormatWith(mappedInputs);
			}
			return result;
		}

		/// <see cref="IEquationService.Check(Sequence, Role, IDictionary{string, string})"/>
		public bool Check(Sequence sequence, Role role, IDictionary<string, string> inputs = null)
		{
			return Check(sequence, inputs, new Role[] { role });
		}

		/// <see cref="IEquationService.Check(Sequence, IDictionary{string, string}, IEnumerable{Role})"/>
		public bool Check(Sequence sequence, IDictionary<string, string> inputs = null, IEnumerable<Role> roles = null)
		{
			if (!CheckRoleConditions(sequence, roles)) return false;

			inputs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);  // isolate changes to this method
			var mappedInputs = new Dictionary<string, string>(inputs, StringComparer.OrdinalIgnoreCase);  // isolate mapping changes to current sequence item
			sequence.Mappings.ForEach(x => Apply(x, null, ref mappedInputs, roles));

			return sequence.Conditions.All(x => Check(x, mappedInputs));
		}

		/// <see cref="IEquationService.Process(Sequence, IDictionary{string, string}, IEnumerable{Role})"/>
		public SequenceResult Process(Sequence sequence, IDictionary<string, string> inputs = null, IEnumerable<Role> roles = null)
		{
			var sArgs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);  // isolate changes to this method
			var result = new SequenceResult();
			result.Sequence = sequence;
			if (!CheckRoleConditions(sequence, roles))
			{
				throw new RoleConditionFailedException("Role conditions not met for this sequence!");
			}
			for (int order = 0; order < sequence.Items.Count; order++)
			{
				var item = sequence.Items[order];
				var mappedInputs = new Dictionary<string, string>(sArgs, StringComparer.OrdinalIgnoreCase);  // isolate mapping changes to current sequence item
				sequence.Mappings.ForEach(x => Apply(x, item.Name, ref mappedInputs, roles));
				if (!sequence.Conditions.All(x => Check(x, item.Name, mappedInputs, result))) continue;
				var itemResult = GetResult(item, order, ref sArgs, mappedInputs);
				result.Results.Add(itemResult);
			}
			result.ResultItems = ProcessResults(sequence.ResultItems, sArgs, roles);

			return result;
		}
	}
}
