using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TTRPG.Engine.CommandParsing.Parsers;
using TTRPG.Engine.CommandParsing.Processors;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.CommandParsing
{
	public class CommandProcessorFactory : ICommandProcessorFactory
	{
		private readonly GameObject _data;
		private readonly IEnumerable<ICommandParser> _parsers;

		public CommandProcessorFactory(GameObject data, IEnumerable<ICommandParser> parsers)
		{
			_data = data;
			_parsers = parsers;
		}

		public EngineCommand ParseCommand(string fullCommand)
		{
			var parsedCommand = new EngineCommand();
			// get sequence
			var mainCommand = Regex.Match(fullCommand, @"^\w*");
			if (mainCommand.Success && !string.IsNullOrWhiteSpace(mainCommand.Value))
			{
				parsedCommand.MainCommand = mainCommand.Value;
			}
			// get entities
			var entitiesText = Regex.Match(fullCommand, @"\[.+?\]");
			if (entitiesText.Success)
			{
				parsedCommand.Entities = new List<Entity>();
				var entitiesTextParts = entitiesText.Value.Replace("[", "").Replace("]", "").Split(',');
				foreach (var nextEntityPart in entitiesTextParts)
				{
					var entityParts = nextEntityPart.Split(':');
					if (entityParts.Length == 2)
					{
						// alias entity
						var from = nextEntityPart.Split(':')[0];
						var to = nextEntityPart.Split(':')[1];
						var nextEntity = _data.Entities.FirstOrDefault(x => x.Name.Equals(from, StringComparison.OrdinalIgnoreCase));
						if (nextEntity != null)
						{
							parsedCommand.Entities.Add(nextEntity.CloneAs(to));
						}
					}
					else
					{
						// do not alias entity
						var nextEntity = _data.Entities.FirstOrDefault(x => x.Name.Equals(nextEntityPart, StringComparison.OrdinalIgnoreCase));
						parsedCommand.Entities.Add(nextEntity);
					}
				}
			}
			// get inputs
			var inputsText = Regex.Match(fullCommand, @"\{.+?\}");
			if (inputsText.Success)
			{
				var inputsTextParts = inputsText.Value.Replace("{", "").Replace("}", "").Split(',');
				foreach (var nextInputPart in inputsTextParts)
				{
					var from = nextInputPart.Split(':')[0];
					var to = nextInputPart.Split(':')[1];
					if (from != null)
					{
						parsedCommand.Inputs[from] = to;
					}
				}
			}
			return parsedCommand;
		}

		public ITTRPGCommandProcessor Build(EngineCommand parsedCommand)
		{
			ICommandParser parser = null;
			var matches = _parsers.Where(x => !x.IsDefault && x.CanProcess(parsedCommand.MainCommand));
			if (matches.Count() > 1)
				throw new Exception("Found multiple parsers able to process this command.");
			if (matches.Count() == 1)
				parser = matches.Single();
			if (parser == null)
				parser = _parsers.FirstOrDefault(x => x.IsDefault);
			if (parser == null)
				throw new Exception("Unknown command.");

			return parser.GetProcessor(parsedCommand);
		}
	}
}
