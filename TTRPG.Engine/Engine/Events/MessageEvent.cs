namespace TTRPG.Engine.Engine.Events
{
	public class MessageEvent : TTRPGEvent
	{
		public override TTRPGEventType Category => TTRPGEventType.Message;

		public string Message { get; set; }

		public MessageEventLevel Level { get; set; }
	}
}
