using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.CommandParsing.Processors
{
	public class InventoryProcessor : ITTRPGCommandProcessor
	{
		private readonly IInventoryService _service;
		private readonly ParsedCommand _command;
		private readonly GameObject _data;
		private readonly Role _entity;

		private Role GetClonedInventoryItemByName(string itemName)
		{
			var item = _data.Roles.Single(r => r.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

			return item.CloneAs("_");
		}

		public InventoryProcessor(IInventoryService service, GameObject data, ParsedCommand command)
		{
			_service = service;
			_data = data;
			_command = command;
			_entity = command.Roles.FirstOrDefault();
		}

		public bool IsValid()
		{
			if (_command.MainCommand.ToLower().Trim() == "pickup")
			{
				return _command != null
					&& _entity != null
					&& _data.Roles.Any(r => r.Name.Equals(_command.Inputs["itemname"], StringComparison.OrdinalIgnoreCase));
			}
			if (_command.MainCommand.ToLower().Trim() == "equip")
			{
				if (!_command.Inputs.ContainsKey("equipas")) return false;
			}
			return _command != null && _entity != null;
		}

		public IEnumerable<string> Process()
		{
			try
			{
				switch (_command.MainCommand.ToLower().Trim())
				{
					case "equip":
					{
						_service.Equip(_entity, itemName: _command.Inputs["itemname"], equipAs: _command.Inputs["equipas"]);
						return new string[] { $"Equipped {_command.Inputs["itemname"]}." };
					}
					case "unequip":
					{
						_service.Unequip(_entity, itemName: _command.Inputs["itemName"]);
						return new string[] { $"Unequipped {_command.Inputs["itemname"]}." };
					}
					case "drop":
					{
						_service.DropItem(_entity, itemName: _command.Inputs["itemName"]);
						return new string[] { $"Dropped {_command.Inputs["itemname"]}." };
					}
					case "pickup":
					{
						var item = GetClonedInventoryItemByName(_command.Inputs["itemname"]);
						_service.PickupItem(_entity, item: item);
						return new string[] { $"Picked up {_command.Inputs["itemname"]}." };
					}
					default:
					{
						throw new NotImplementedException($"Unknown inventory command {_command.MainCommand}.");
					}
				}
			}
			catch (InventoryServiceException ex)
			{
				return new string[] { ex.Message };
			}
		}
	}
}
