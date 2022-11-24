using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.Engine
{
	public class AutomaticCommandFactory : IAutomaticCommandFactory
	{
		private readonly AutomaticCommandFactoryOptions _options;
		private readonly GameObject _data;

		private IEnumerable<EngineCommand> BuildEngineCommands(AutomaticCommand commandConfig, IEnumerable<Entity> entitiesMatching, Dictionary<string, Dictionary<string, string>> categoryParams = null)
		{
			var commands = new List<EngineCommand>();
			foreach (var entityMatching in entitiesMatching)
			{
				var command = new EngineCommand();
				command.MainCommand = commandConfig.Command;
				command.Inputs = commandConfig.DefaultInputs;
				if (commandConfig.SequenceCategory != null && categoryParams != null
					&& categoryParams.TryGetValue(commandConfig.SequenceCategory, out var extraInputs))
				{
					foreach (var kvp in extraInputs)
					{
						command.Inputs[kvp.Key] = kvp.Value;
					}
				}
				if (!string.IsNullOrWhiteSpace(commandConfig.AliasEntitiesAs))
					command.Entities.Add(entityMatching.CloneAs(commandConfig.AliasEntitiesAs));
				else
					command.Entities.Add(entityMatching);
				commands.Add(command);
			}
			return commands;
		}

		public AutomaticCommandFactory(AutomaticCommandFactoryOptions options, GameObject data)
		{
			_options = options;
			_data = data;
		}

		public IEnumerable<EngineCommand> GetAutomaticCommands(ProcessedCommand processed)
		{
			if (!processed.Valid || processed.Failed)
				return new EngineCommand[0];

			var results = new List<EngineCommand>();
			var triggeredCommands = _options.AutomaticCommands
				.Where(auto => auto.SequenceCategory != null
					&& processed.CommandCategories.Contains(auto.SequenceCategory)
					&& (!auto.CompletedOnly || processed.Completed));
			foreach (var command in triggeredCommands)
			{
				var entities = _data.Entities.Where(x => command.Filter(x));
				results.AddRange(BuildEngineCommands(command, entities, processed.CategoryParams));
			}

			return results;
		}

		public IEnumerable<EngineCommand> GetAutomaticCommands(IEnumerable<Entity> entities)
		{
			var results = new List<EngineCommand>();

			var sequencelessCommands = _options.AutomaticCommands
				.Where(x => x.SequenceCategory == null);
			foreach (var command in sequencelessCommands)
			{
				var matchingEntities = entities.Where(entity => command.Filter(entity));
				results.AddRange(BuildEngineCommands(command, matchingEntities));
			}

			return results;
		}
	}
}
