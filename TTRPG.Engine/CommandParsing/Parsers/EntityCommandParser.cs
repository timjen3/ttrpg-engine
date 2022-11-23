using System;
using System.Collections.Generic;
using TTRPG.Engine.CommandParsing.Processors;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.CommandParsing.Parsers
{
	public class EntityCommandParser : ICommandParser
	{
		private static readonly HashSet<string> CommandLookup = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"birth",
			"bury",
		};

		private readonly IRoleService _service;
		private readonly GameObject _data;

		public EntityCommandParser(IRoleService service, GameObject data)
		{
			_service = service;
			_data = data;
		}

		public bool IsDefault => false;

		public bool CanProcess(string commandType) => CommandLookup.Contains(commandType);

		public IEnumerable<string> GetExampleCommands() => new string[]
			{
				"Birth {roleName:bear}",
				"Bury {entityName:Bear1}"
			};

		public ITTRPGCommandProcessor GetProcessor(EngineCommand command)
		{
			return new EntityProcessor(_service, _data, command);
		}
	}
}
