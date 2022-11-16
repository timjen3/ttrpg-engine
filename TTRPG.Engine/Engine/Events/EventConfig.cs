namespace TTRPG.Engine.Engine.Events
{
	public abstract class EventConfig
	{
		public TTRPGEventType EventType { get; set; }

		public string Name { get; set; }

		public string Condition { get; set; }
	}

	public class MessageEventConfig : EventConfig
	{
		public MessageEventLevel Level { get; set; } = MessageEventLevel.Info;

		public string TemplateFilename { get; set; }

		public string MessageTemplate { get; set; }

		public MessageEventConfig() => EventType = TTRPGEventType.Message;
	}

	public class UpdateAttributesEventConfig : EventConfig
	{
		public string AttributeName { get; set; }

		public string Source { get; set; }

		public string EntityAlias { get; set; }

		public UpdateAttributesEventConfig() => EventType = TTRPGEventType.UpdateAttributes;
	}
}
