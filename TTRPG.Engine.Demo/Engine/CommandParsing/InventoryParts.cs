using System;
using System.Collections.Generic;

namespace TTRPG.Engine.Demo.Engine.CommandParsing
{
	public class InventoryProcessor : ITTRPGCommandProcessor
	{
		public string Command { get; }
		public Role Entity { get; }
		public Dictionary<string, string> Inputs { get; }
		IInventoryService Service { get; }

		public InventoryProcessor(string command, Role entity, Dictionary<string, string> inputs, IInventoryService service)
		{
			Command = command;
			Entity = entity;
			Inputs = inputs;
			Service = service;
		}

		public bool IsValid() => !string.IsNullOrWhiteSpace(Command) && Entity != null;

		public void Process(Action<string> writeMessage, GameObject data)
		{
			try
			{
				switch (Command.ToLower().Trim())
				{
					case "equip":
					{
						Service.Equip(Entity, itemName: Inputs["itemname"], equipAs: Inputs["equipas"]);
						writeMessage($"Equipped {Inputs["itemname"]} as {Inputs["equipas"]}");
						break;
					}
					case "unequip":
					{
						Service.Unequip(Entity, itemName: Inputs["itemName"]);
						writeMessage($"Unequipped {Inputs["itemname"]}.");
						break;
					}
					case "drop":
					{
						Service.DropItem(Entity, itemName: Inputs["itemName"]);
						writeMessage($"Dropped {Inputs["itemname"]}.");
						break;
					}
					default:
					{
						throw new NotImplementedException($"Unknown inventory command {Command}.");
					}
				}
			}
			catch
			{
				writeMessage("Invalid command.");
			}
		}

		public static string[] GetInventoryCommandExamples()
		{
			return new string[]
			{
				"Equip [miner] {itemName:stone pickaxe,equipAs:pick}",
				"Unequip [miner] {itemName:pick}",
				"Drop [miner] {itemName:stone pickaxe}"
			};
		}
	}
}
