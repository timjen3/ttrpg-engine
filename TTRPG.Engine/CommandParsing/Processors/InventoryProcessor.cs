using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Engine.Events;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.CommandParsing.Processors
{
	public class InventoryProcessor : ITTRPGCommandProcessor
	{
		private readonly IInventoryService _service;
		private readonly EngineCommand _command;
		private readonly GameObject _data;
		private readonly Entity _entity;

		private Entity GetClonedInventoryItemByName(string itemName)
		{
			var item = _data.Entities.Single(r => r.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

			return item.CloneAs("_");
		}

		public InventoryProcessor(IInventoryService service, GameObject data, EngineCommand command)
		{
			_service = service;
			_data = data;
			_command = command;
			_entity = command.Entities.FirstOrDefault();
		}

		public bool IsValid()
		{
			if (_command.MainCommand.ToLower().Trim() == "pickup")
			{
				return _command != null
					&& _entity != null
					&& _data.Entities.Any(r => r.Name.Equals(_command.Inputs["itemname"], StringComparison.OrdinalIgnoreCase));
			}
			if (_command.MainCommand.ToLower().Trim() == "equip")
			{
				if (!_command.Inputs.ContainsKey("equipas"))
					return false;
			}
			return _command != null && _entity != null;
		}

		public ProcessedCommand Process()
		{
			var processed = new ProcessedCommand();
			processed.Source = _command;
			processed.CommandCategories = new List<string> { "Inventory" };
			try
			{
				switch (_command.MainCommand.ToLower().Trim())
				{
					case "equip":
					{
						_service.Equip(_entity, itemName: _command.Inputs["itemname"], equipAs: _command.Inputs["equipas"]);
						var message = $"Equipped {_command.Inputs["itemname"]}.";
						processed.Events.Add(new MessageEvent { Level = MessageEventLevel.Info, Message = message });
						processed.Completed = true;
						break;
					}
					case "unequip":
					{
						_service.Unequip(_entity, itemName: _command.Inputs["itemName"]);
						var message = $"Unequipped {_command.Inputs["itemname"]}.";
						processed.Events.Add(new MessageEvent { Level = MessageEventLevel.Info, Message = message });
						processed.Completed = true;
						break;
					}
					case "drop":
					{
						_service.DropItem(_entity, itemName: _command.Inputs["itemName"]);
						var message = $"Dropped {_command.Inputs["itemname"]}.";
						processed.Events.Add(new MessageEvent { Level = MessageEventLevel.Info, Message = message });
						processed.Completed = true;
						break;
					}
					case "pickup":
					{
						var item = GetClonedInventoryItemByName(_command.Inputs["itemname"]);
						_service.PickupItem(_entity, item: item);
						var message = $"Picked up {_command.Inputs["itemname"]}.";
						processed.Events.Add(new MessageEvent { Level = MessageEventLevel.Info, Message = message });
						processed.Completed = true;
						break;
					}
					default:
					{
						throw new NotImplementedException($"Unknown inventory command {_command.MainCommand}.");
					}
				}
			}
			catch (InventoryServiceException ex)
			{
				processed.Events.Add(new MessageEvent { Level = MessageEventLevel.Info, Message = ex.Message });
				processed.Failed = true;
			}

			return processed;
		}
	}
}
