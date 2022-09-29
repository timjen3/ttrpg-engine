using System.Collections.Generic;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo.Engine
{
	public class EquationParts
	{
		public Sequence Sequence { get; set; }
		public Dictionary<string, string> Inputs { get; set; } = new Dictionary<string, string>();
		public List<Role> Roles { get; set; }

		public bool IsValid() => Sequence != null && Roles != null && Roles.Count > 0;
	}
}
