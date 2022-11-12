namespace TTRPG.Engine.Engine.Events
{
	public class UpdateAttributesEvent : TTRPGEvent
	{
		public override TTRPGEventType Category => TTRPGEventType.UpdateAttributes;

		public string EntityName { get; set; }
		public string AttributeToUpdate { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
	}
}
