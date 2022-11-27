using System;
using System.Collections.Generic;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.Engine
{
	public delegate bool EntityMatch(Entity entity);

	public class AutomaticCommand
	{
		/// <summary>
		///		Command to be executed
		/// </summary>
		public string Command { get; set; }

		/// <summary>
		///		Delegate for entities to be included
		/// </summary>
		public EntityMatch EntityFilter { get; set; }

		/// <summary>
		///		How to alias included entities
		/// </summary>
		public string AliasEntitiesAs { get; set; }

		/// <summary>
		///		Additional inputs to include
		/// </summary>
		public Dictionary<string, string> DefaultInputs { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}
}
