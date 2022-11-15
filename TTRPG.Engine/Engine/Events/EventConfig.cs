namespace TTRPG.Engine.Engine.Events
{
	public abstract class EventConfig
	{
		public TTRPGEventType EventType { get; }

		public string Name { get; set; }

		public string Condition { get; set; }
	}

	public class MessageEventConfig : EventConfig
	{
		public MessageEventLevel Level { get; set; } = MessageEventLevel.Info;

		public string TemplateFilename { get; set; }

		public string MessageTemplate { get; set; }
	}

	public class UpdateAttributesEventConfig : EventConfig
	{
		public string AttributeName { get; set; }

		public string Source { get; set; }

		public string EntityAlias { get; set; }
	}
}
