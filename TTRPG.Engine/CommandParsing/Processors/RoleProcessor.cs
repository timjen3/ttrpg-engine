using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Engine.Events;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.CommandParsing.Processors
{
	public class RoleProcessor : ITTRPGCommandProcessor
	{
		private readonly IRoleService _service;
		private readonly EngineCommand _command;
		private readonly GameObject _data;

		public RoleProcessor(IRoleService service, GameObject data, EngineCommand command)
		{
			_service = service;
			_data = data;
			_command = command;
		}

		public bool IsValid()
		{
			if (_command.MainCommand.ToLower().Trim() == "birth")
			{
				return _command != null
					&& _data.Roles.Any(r => r.Name.Equals(_command.Inputs["roleName"], StringComparison.OrdinalIgnoreCase));
			}
			if (_command.MainCommand.ToLower().Trim() == "bury")
			{
				return _command != null && _data.Entities.Any(x => x.Name.Equals(_command.Inputs["entityName"], StringComparison.OrdinalIgnoreCase));
			}
			return false;
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
					case "birth":
					{
						_service.Birth(_command.Inputs["roleName"]);
						var message = $"Birthed {_command.Inputs["roleName"]}.";
						processed.Events.Add(new MessageEvent { Level = MessageEventLevel.Info, Message = message });
						processed.Completed = true;
						break;
					}
					case "bury":
					{
						_service.Bury(_command.Inputs["entityName"]);
						var message = $"Buried {_command.Inputs["entityName"]}.";
						processed.Events.Add(new MessageEvent { Level = MessageEventLevel.Info, Message = message });
						processed.Completed = true;
						break;
					}
					default:
					{
						throw new NotImplementedException($"Unknown role command {_command.MainCommand}.");
					}
				}
			}
			catch (RoleServiceException ex)
			{
				processed.Events.Add(new MessageEvent { Level = MessageEventLevel.Info, Message = ex.Message });
				processed.Failed = true;
			}

			return processed;
		}
	}
}
