using System;
using System.Collections.Generic;
using System.Linq;

namespace TTRPG.Engine.Demo.Engine.CommandParsing
{
	public class InventoryParts
	{
		public string Command { get; }
		public Role Entity { get; }
		public Dictionary<string, string> Inputs { get; }

		public InventoryParts(CommandParser parser)
		{
			Command = parser.MainCommand;
			Entity = parser.Roles.FirstOrDefault();
			Inputs = parser.Inputs;
		}

		public bool IsValid() => !string.IsNullOrWhiteSpace(Command) && Entity != null;

		public string Process(IInventoryService service)
		{
			switch (Command.ToLower().Trim())
			{
				case "equip":
				{
					service.Equip(Entity, itemName: Inputs["itemname"], equipAs: Inputs["equipas"]);
					return $"Equipped {Inputs["itemname"]} as {Inputs["equipas"]}";
				}
				case "unequip":
				{
					service.Unequip(Entity, itemName: Inputs["itemName"]);
					return $"Unequipped {Inputs["itemname"]}.";
					}
				case "drop":
				{
					service.DropItem(Entity, itemName: Inputs["itemName"]);
					return $"Dropped {Inputs["itemname"]}.";
					}
				default:
				{
					throw new NotImplementedException($"Unknown inventory command {Command}.");
				}
			}
		}

		public static string[] GetInventoryCommandExamples()
		{
			return new string[]
			{
				"Equip [miner] {itemName:stone pickaxe,equipAs:pick}",
				"Unequip [miner] {itemName:stone pickaxe}",
				"Drop [miner] {itemName:stone pickaxe}"
			};
		}
	}
}
