using System.Collections.Generic;

namespace TTRPG.Engine.Engine
{
	public delegate bool RoleMatch(Role role);

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
		///		Delegate for roles to be included
		/// </summary>
		public RoleMatch Filter { get; set; }

		/// <summary>
		///		How to alias included roles
		/// </summary>
		public string AliasRolesAs { get; set; }

		/// <summary>
		///		Additional inputs to include
		/// </summary>
		public Dictionary<string, string> Inputs { get; set; } = new Dictionary<string, string>();

		/// <summary>
		///		When true only fires for completed sequences
		/// </summary>
		public bool CompletedOnly { get; set; } = true;
	}
}
