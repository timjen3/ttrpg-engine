namespace TTRPG.Engine.Engine.Events
{
	public interface IEventConfig
	{
		TTRPGEventType EventType { get; set; }
	}

	public class MessageEventConfig : IEventConfig
	{
		public TTRPGEventType EventType { get; set; }
	}

	public class UpdateAttributesEventConfig : IEventConfig
	{
		public TTRPGEventType EventType { get; set; }

		public string AttributeName { get; set; }

		public string Source { get; set; }

		public string EntityAlias { get; set; }
	}
}
