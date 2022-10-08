using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.Demo.Engine.CommandParsing
{
	public class CommandParser
	{
		private static readonly Dictionary<string, TTRPGCommandType> CommandLookup = new Dictionary<string, TTRPGCommandType>(StringComparer.OrdinalIgnoreCase)
		{
			{ "equip", TTRPGCommandType.Inventory },
			{ "unequip", TTRPGCommandType.Inventory },
			{ "drop", TTRPGCommandType.Inventory },
			{ "pickup", TTRPGCommandType.Inventory }
		};

		public TTRPGCommandType CommandType { get; } = TTRPGCommandType.Equation;
		public string MainCommand { get; }
		public List<Role> Roles { get; set; }
		public Dictionary<string, string> Inputs { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		public GameObject Data { get; }

		// try to parse command
		public CommandParser(string command, GameObject data)
		{
			Data = data;
			// get sequence
			var mainCommand = Regex.Match(command, @"^.*?(?=\s)");
			if (mainCommand.Success)
			{
				MainCommand = mainCommand.Value;
			}
			if (CommandLookup.ContainsKey(MainCommand))
			{
				CommandType = CommandLookup[MainCommand];
			}
			// get roles
			var rolesText = Regex.Match(command, @"\[.+?\]");
			if (rolesText.Success)
			{
				Roles = new List<Role>();
				var rolesTextParts = rolesText.Value.Replace("[", "").Replace("]", "").Split(",");
				foreach (var nextRolePart in rolesTextParts)
				{
					var roleParts = nextRolePart.Split(":");
					if (roleParts.Length == 2)
					{
						// alias role
						var from = nextRolePart.Split(":")[0];
						var to = nextRolePart.Split(":")[1];
						var nextRole = data.Roles.FirstOrDefault(x => x.Name.Equals(from, StringComparison.OrdinalIgnoreCase));
						if (nextRole != null)
						{
							Roles.Add(nextRole.CloneAs(to));
						}
					}
					else
					{
						// do not alias role
						var nextRole = data.Roles.FirstOrDefault(x => x.Name.Equals(nextRolePart, StringComparison.OrdinalIgnoreCase));
						Roles.Add(nextRole);
					}
				}
			}
			// get inputs
			var inputsText = Regex.Match(command, @"\{.+?\}");
			if (inputsText.Success)
			{
				var inputsTextParts = inputsText.Value.Replace("{", "").Replace("}", "").Split(",");
				foreach (var nextInputPart in inputsTextParts)
				{
					var from = nextInputPart.Split(":")[0];
					var to = nextInputPart.Split(":")[1];
					if (from != null)
					{
						Inputs[from] = to;
					}
				}
			}
		}

		public ITTRPGCommandProcessor Build(IEquationService equationService, IInventoryService inventoryService)
		{
			if (CommandType == TTRPGCommandType.Equation)
			{
				return new EquationProcessor(
					equationService,
					MainCommand,
					Roles,
					Inputs,
					Data
				);
			}
			else if (CommandType == TTRPGCommandType.Inventory)
			{
				return new InventoryProcessor(
					MainCommand,
					Roles.FirstOrDefault(),
					Inputs,
					inventoryService
				);
			}
			else
			{
				throw new NotImplementedException("Unknown command.");
			}
		}
	}
}
