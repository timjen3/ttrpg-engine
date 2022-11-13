using System;
using System.Linq;
using TTRPG.Engine.CommandParsing;

namespace TTRPG.Engine.Engine.Events
{
	public class TTRPGEventHandler : ITTRPGEventHandler
	{
		private readonly GameObject _data;

		public TTRPGEventHandler(GameObject data) => _data = data;

		public void ProcessResult(ProcessedCommand result)
		{
			foreach (var @event in result.Events)
			{
				if (@event is UpdateAttributesEvent attEvent)
				{
					var entityToUpdate = _data.Entities.FirstOrDefault(x => x.Name.Equals(attEvent.EntityName, StringComparison.OrdinalIgnoreCase));
					if (entityToUpdate == null)
					{
						throw new Exception($"Tried to update attribute: {attEvent.AttributeToUpdate} but could not find entity to update: {attEvent.EntityName}");
					}
					entityToUpdate.Attributes[attEvent.AttributeToUpdate] = attEvent.NewValue;
				}
			}
		}
	}
}
