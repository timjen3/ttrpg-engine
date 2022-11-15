namespace TTRPG.Engine.Engine.Events
{
	public abstract class TTRPGEvent
	{
		public abstract TTRPGEventType Category { get; }

		public string Name { get; set; }

		public string Condition { get; set; }
	}
}
