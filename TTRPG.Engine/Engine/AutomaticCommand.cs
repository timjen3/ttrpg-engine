using System.Collections.Generic;

namespace TTRPG.Engine.Engine
{
	public delegate bool EntityMatch(Entity entity);

	public class AutomaticCommand
	{
		/// <summary>
		///		Fire this command after any sequences matching the specified category
		/// </summary>
		public string SequenceCategory { get; set; }

		/// <summary>
		///		Command to be executed
		/// </summary>
		public string Command { get; set; }

		/// <summary>
		///		Delegate for entities to be included
		/// </summary>
		public EntityMatch Filter { get; set; }

		/// <summary>
		///		How to alias included entities
		/// </summary>
		public string AliasEntitiesAs { get; set; }

		/// <summary>
		///		Additional inputs to include
		///		Will be overriden by sequence-specific CategoryParams associated with this command's SequenceCategory
		/// </summary>
		public Dictionary<string, string> DefaultInputs { get; set; } = new Dictionary<string, string>();

		/// <summary>
		///		When true only fires for completed sequences
		/// </summary>
		public bool CompletedOnly { get; set; } = true;
	}
}
