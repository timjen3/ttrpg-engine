using System;
using System.Collections.Generic;
using System.Linq;
using FormatWith;
using TTRPG.Engine.Engine.Events;
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
		IDictionary<string, string> GetSourceMappingData(Mapping mapping, Dictionary<string, string> inputs, IEnumerable<Entity> entities)
		{
			IDictionary<string, string> source;
			switch (mapping.MappingType)
			{
				case MappingType.InventoryItem:
				case MappingType.Entity:
				{
					Entity entity = null;
					if (mapping.EntityName == null)
					{
						entity = entities.FirstOrDefault();
						if (mapping.ThrowOnFailure && entity == null)
							throw new MissingEntityException($"Mapping failed due to no entities being passed.");
					}
					else
					{
						entity = entities.SingleOrDefault(x => x.Alias != null && x.Alias.Equals(mapping.EntityName, StringComparison.OrdinalIgnoreCase));
						if (mapping.ThrowOnFailure && entity == null)
							throw new MissingEntityException($"Mapping failed due to missing entity: '{mapping.EntityName}'.");
					}
					if (mapping.InventoryItemName != null)
					{
						Entity item = null;
						if (entity != null && entity.InventoryItems.ContainsKey(mapping.InventoryItemName))
							item = entity.InventoryItems[mapping.InventoryItemName];
						if (mapping.ThrowOnFailure && item == null)
							throw new MissingEntityException($"Mapping failed due to entity not having item: '{mapping.InventoryItemName}'.");
						source = item?.Attributes;
						break;
					}

					source = entity?.Attributes;
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

		/// Checks entity conditions against provided entities
		bool CheckEntityConditions(Sequence sequence, IEnumerable<Entity> entities)
		{
			var entityConditionsMet = sequence.EntityConditions == null || sequence.EntityConditions.All(condition =>
			{
				var target = entities?.FirstOrDefault(entity => entity?.Alias.Equals(condition.EntityName, StringComparison.OrdinalIgnoreCase) ?? false);
				if (target == null)
					return false;

				return condition.RequiredCategories.All(x => target.Categories.Contains(x, StringComparer.OrdinalIgnoreCase));
			});

			return entityConditionsMet;
		}

		/// EquationService Constructor
		public EquationService(IEquationResolver equationResolver)
		{
			_equationResolver = equationResolver;
		}

		/// Determine if the condition passes for the sequence
		internal bool Check(Condition condition, IDictionary<string, string> inputs, ref HashSet<string> failureMessages)
		{
			return Check(condition, null, inputs, null, ref failureMessages);
		}

		/// Determine if the condition passes for a sequence item
		internal bool Check(Condition condition, string itemName, IDictionary<string, string> inputs, SequenceResult results, ref HashSet<string> failureMessages)
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
			if (!valid && !condition.ThrowOnFail && !string.IsNullOrWhiteSpace(condition.FailureMessage))
			{
				var formattedErrorMessage = condition.FailureMessage.FormatWith(inputs);
				failureMessages.Add(formattedErrorMessage);
			}
			return valid;
		}

		/// Injects the specified mapping into <param name="inputs"/> from the configured source determined by <see cref="MappingType"/>
		/// <param name="itemName">The sequence item name mapping is being applied for. If the mapping does not apply to that item nothing will happen.</param>
		/// <param name="entities">All entities available to the sequence.</param>
		internal void Apply(Mapping mapping, string itemName, ref Dictionary<string, string> inputs, IEnumerable<Entity> entities)
		{
			if (string.IsNullOrWhiteSpace(mapping.ItemName) || string.Equals(itemName, mapping.ItemName, StringComparison.OrdinalIgnoreCase))
			{
				var source = GetSourceMappingData(mapping, inputs, entities);
				// always use inputs for the formatter
				// ensure the mapped inputs take precedence
				var mapFormatSource = source.Union(inputs)
					.GroupBy(x => x.Key)
					.Select(g => new KeyValuePair<string, string>(g.Key, g.First().Value))
					.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
				var mapTo = mapping.To.FormatWith(mapFormatSource);
				var mapFrom = mapping.From.FormatWith(mapFormatSource);
				if (source.ContainsKey(mapFrom))
				{
					inputs[mapTo] = source[mapFrom];
				}
				else if (mapping.ThrowOnFailure)
					throw new MappingFailedException($"Mapping failed due to missing key: '{mapping.From}'.");
				else
					inputs[mapTo] = "0";
			}
		}

		/// Process a sequence item and get the result
		internal SequenceItemResult GetResult(SequenceItem item, int order, ref Dictionary<string, string> inputs, IDictionary<string, string> mappedInputs = null)
		{
			var result = new SequenceItemResult();
			result.Order = order;
			result.Inputs = mappedInputs;
			result.ResolvedItem = item;
			result.Result = _equationResolver.Process(item.Equation, mappedInputs).ToString();
			if (inputs != null)
			{
				inputs[item.ResultName] = result.Result.ToString();
			}
			return result;
		}

		/// Process event config and set results
		internal List<TTRPGEvent> ProcessResults(IEnumerable<EventConfig> events, IDictionary<string, string> inputs, IEnumerable<Entity> entities)
		{
			var results = new List<TTRPGEvent>();
			foreach (var @event in events)
			{
				if (@event.Condition != null && _equationResolver.Process(@event.Condition, inputs) == 0)
				{
					continue;
				}

				if (@event is UpdateAttributesEventConfig attEvent)
				{
					if (inputs.TryGetValue(attEvent.Source, out string value))
					{
						var entityToUpdate = entities.Single(x => x.Alias.Equals(attEvent.EntityAlias, StringComparison.OrdinalIgnoreCase));
						var result = new UpdateAttributesEvent();
						result.Name = attEvent.Name;
						result.AttributeToUpdate = attEvent.AttributeName.FormatWith(inputs, MissingKeyBehaviour.ThrowException);
						result.OldValue = entityToUpdate.Attributes[result.AttributeToUpdate];
						result.NewValue = inputs[attEvent.Source];
						result.EntityName = entityToUpdate.Name;  // set to true name
						if (result.OldValue != result.NewValue)
						{
							results.Add(result);
						}
					}
				}
				else if (@event is MessageEventConfig mEvent)
				{
					var result = new MessageEvent();
					result.Name = mEvent.Name;
					result.Message = mEvent.MessageTemplate.FormatWith(inputs, MissingKeyBehaviour.ThrowException);
					result.Level = mEvent.Level;
					results.Add(result);
				}
			}
			return results;
		}

		/// <see cref="IEquationService.Process(SequenceItem, Entity, IDictionary{string, string})"/>
		public SequenceItemResult Process(SequenceItem item, Entity entity = null, IDictionary<string, string> inputs = null)
		{
			var sArgs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);  // isolate changes to this method
			var result = new SequenceItemResult();
			result.Order = 0;
			result.Inputs = sArgs;
			result.ResolvedItem = item;
			var mappedInputs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);
			if (entity != null)
			{
				foreach (var kvp in entity.Attributes)
					mappedInputs[kvp.Key] = kvp.Value;
			}
			result.Result = _equationResolver.Process(item.Equation, mappedInputs).ToString();

			return result;
		}

		/// <see cref="IEquationService.Check(Sequence, Entity, IDictionary{string, string})"/>
		public bool Check(Sequence sequence, Entity entity, IDictionary<string, string> inputs = null)
		{
			return Check(sequence, inputs, new Entity[] { entity });
		}

		/// <see cref="IEquationService.Check(Sequence, IDictionary{string, string}, IEnumerable{Entity})"/>
		public bool Check(Sequence sequence, IDictionary<string, string> inputs = null, IEnumerable<Entity> entities = null)
		{
			if (!CheckEntityConditions(sequence, entities))
				return false;

			inputs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);  // isolate changes to this method
			var mappedInputs = new Dictionary<string, string>(inputs, StringComparer.OrdinalIgnoreCase);  // isolate mapping changes to current sequence item
			sequence.Mappings.ForEach(x => Apply(x, null, ref mappedInputs, entities));
			var errorMessages = new HashSet<string>();

			return sequence.Conditions.All(x => Check(x, mappedInputs, ref errorMessages));
		}

		/// <see cref="IEquationService.Process(Sequence, IDictionary{string, string}, IEnumerable{Entity})"/>
		public SequenceResult Process(Sequence sequence, IDictionary<string, string> inputs = null, IEnumerable<Entity> entities = null)
		{
			var sArgs = new Dictionary<string, string>(inputs ?? new Dictionary<string, string>(), StringComparer.OrdinalIgnoreCase);  // isolate changes to this method
			var globalMappings = sequence.Mappings.Where(x => string.IsNullOrWhiteSpace(x.ItemName)).ToList();
			globalMappings.ForEach(x => Apply(x, null, ref sArgs, entities));  // apply global mappings to all inputs
			var result = new SequenceResult();
			result.Sequence = sequence;
			if (sequence.Items.All(x => !x.SetComplete))
			{
				result.Completed = true;
			}
			if (!CheckEntityConditions(sequence, entities))
			{
				throw new EntityConditionFailedException("Entity conditions not met for this sequence!");
			}
			var firedEvents = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			var errorMessages = new HashSet<string>();
			for (int order = 0; order < sequence.Items.Count; order++)
			{
				var item = sequence.Items[order];
				var mappedInputs = new Dictionary<string, string>(sArgs, StringComparer.OrdinalIgnoreCase);  // isolate mapping changes to current sequence item
				sequence.Mappings.Where(x => !string.IsNullOrWhiteSpace(x.ItemName)).ToList().ForEach(x => Apply(x, item.Name, ref mappedInputs, entities));
				if (!sequence.Conditions.All(x => Check(x, item.Name, mappedInputs, result, ref errorMessages)))
					continue;
				var itemResult = GetResult(item, order, ref sArgs, mappedInputs);
				result.Results.Add(itemResult);
				item.Produces.ForEach(e => firedEvents.Add(e));
				if (item.SetComplete)
				{
					result.Completed = true;
				}
			}

			var validEvents = sequence.Events
				.Where(e => firedEvents.Contains(e.Name));
			result.Events = ProcessResults(validEvents, sArgs, entities);
			result.Events.AddRange(errorMessages
				.Select(errorMessage => new MessageEvent
				{
					Message = errorMessage,
					Level = MessageEventLevel.Error
				}));

			return result;
		}
	}
}
