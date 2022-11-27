namespace TTRPG.Engine.Engine
{
	public class SequenceAutoCommand : AutomaticCommand
	{
		/// <summary>
		///		Command fires for any sequences with this category
		/// </summary>
		public string SequenceCategory { get; set; }

		/// <summary>
		///		When true only fires for completed sequences
		/// </summary>
		public bool CompletedOnly { get; set; } = true;
	}
}
