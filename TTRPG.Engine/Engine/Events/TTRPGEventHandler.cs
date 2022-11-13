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
					var entity = _data.Entities.Single(x => x.Name == attEvent.EntityName);
					entity.Attributes[attEvent.AttributeToUpdate] = attEvent.NewValue;
				}
			}
		}
	}
}
