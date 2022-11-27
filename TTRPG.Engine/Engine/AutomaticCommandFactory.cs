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
				if (commandConfig is SequenceAutoCommand sequenceConfig)
				{
					if (sequenceConfig.SequenceCategory != null && categoryParams != null
						&& categoryParams.TryGetValue(sequenceConfig.SequenceCategory, out var extraInputs))
					{
						foreach (var kvp in extraInputs)
						{
							command.Inputs[kvp.Key] = kvp.Value;
						}
					}
				}
				command.Entities.Add(entityMatching.CloneAs(commandConfig.AliasEntitiesAs ?? entityMatching.Name));
				commands.Add(command);
			}
			return commands;
		}

		public AutomaticCommandFactory(AutomaticCommandFactoryOptions options, GameObject data)
		{
			_options = options;
			_data = data;
		}

		public IEnumerable<EngineCommand> GetSequenceAutomaticCommands(ProcessedCommand processed)
		{
			if (!processed.Valid || processed.Failed)
				return new EngineCommand[0];

			var results = new List<EngineCommand>();

			var triggeredCommands = _options.AutomaticCommands
				.Where(auto => auto is SequenceAutoCommand)
				.Cast<SequenceAutoCommand>()
				.Where(auto => processed.CommandCategories.Contains(auto.SequenceCategory)
					&& (!auto.CompletedOnly || processed.Completed));

			foreach (var command in triggeredCommands)
			{
				var entities = _data.Entities.Where(x => command.EntityFilter(x));
				results.AddRange(BuildEngineCommands(command, entities, processed.CategoryParams));
			}

			return results;
		}

		public IEnumerable<EngineCommand> GetAutomaticCommands(IEnumerable<Entity> entities)
		{
			var results = new List<EngineCommand>();

			var autoCommands = _options.AutomaticCommands
				.Where(x => x.GetType() == typeof(AutomaticCommand))
				.Cast<AutomaticCommand>();

			foreach (var command in autoCommands)
			{
				var matchingEntities = entities.Where(entity => command.EntityFilter(entity));
				results.AddRange(BuildEngineCommands(command, matchingEntities));
			}

			return results;
		}
	}
}
