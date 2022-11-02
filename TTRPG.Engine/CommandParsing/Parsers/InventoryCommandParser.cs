using System;
using System.Collections.Generic;
using TTRPG.Engine.CommandParsing.Processors;

namespace TTRPG.Engine.CommandParsing.Parsers
{
	public class InventoryCommandParser : ICommandParser
	{
		private static readonly HashSet<string> CommandLookup = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"equip",
			"unequip",
			"drop",
			"pickup",
		};

		private readonly IInventoryService _service;
		private readonly GameObject _data;

		public InventoryCommandParser(IInventoryService service, GameObject data)
		{
			_service = service;
			_data = data;
		}

		public bool IsDefault => false;

		public bool CanProcess(string commandType)
		{
			return CommandLookup.Contains(commandType);
		}

		public IEnumerable<string> GetExampleCommands()
		{
			return new string[]
			{
				"Equip [miner] {itemName:stone pickaxe,equipAs:pick}",
				"Unequip [miner] {itemName:pick}",
				"Drop [miner] {itemName:stone pickaxe}",
				"Pickup [miner] {itemName:pebble}"
			};
		}

		public ITTRPGCommandProcessor GetProcessor(ParsedCommand command)
		{
			return new InventoryProcessor(_service, _data, command);
		}
	}
}
