using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Engine.Events;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.CommandParsing.Processors
{
	public class EntityProcessor : ITTRPGCommandProcessor
	{
		private readonly IRoleService _service;
		private readonly EngineCommand _command;
		private readonly GameObject _data;

		public EntityProcessor(IRoleService service, GameObject data, EngineCommand command)
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
					&& _command.Inputs.ContainsKey("roleName") && _command.Inputs.ContainsKey("name")
					&& _data.Roles.Any(r => r.Name.Equals(_command.Inputs["roleName"], StringComparison.OrdinalIgnoreCase));
			}
			if (_command.MainCommand.ToLower().Trim() == "bury")
			{
				return _command != null && _command.Entities.Any();
			}
			return false;
		}

		public ProcessedCommand Process()
		{
			var processed = new ProcessedCommand();
			processed.Source = _command;
			processed.CommandCategories = new List<string> { "Roles", "Entities" };
			try
			{
				switch (_command.MainCommand.ToLower().Trim())
				{
					case "birth":
					{
						var role = _data.Roles.Single(x => x.Name.Equals(_command.Inputs["roleName"], StringComparison.OrdinalIgnoreCase));
						var name = _command.Inputs["name"];
						_command.Inputs.Remove("roleName");
						_command.Inputs.Remove("name");
						var entity = _service.Birth(role, _command.Inputs, name);
						_data.Entities.Add(entity);
						var message = $"Birthed {role.Name}.";
						processed.Events.Add(new MessageEvent { Level = MessageEventLevel.Info, Message = message });
						processed.Completed = true;
						break;
					}
					case "bury":
					{
						var entity = _command.Entities.Single();
						_data.Bury(entity.Name);
						var message = $"Buried {entity.Name}.";
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
