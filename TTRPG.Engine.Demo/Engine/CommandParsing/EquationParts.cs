using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo.Engine.CommandParsing
{
	public class EquationParts
	{
		public Sequence Sequence { get; }
		public Dictionary<string, string> Inputs { get; }
		public List<Role> Roles { get; }

		public EquationParts(CommandParser parser, GameObject data)
		{
			Sequence = data.Sequences.FirstOrDefault(x => x.Name.Equals(parser.MainCommand, StringComparison.OrdinalIgnoreCase));
			Roles = parser.Roles;
			Inputs = parser.Inputs;
		}

		public bool IsValid() => Sequence != null && Roles != null && Roles.Count > 0;

		public SequenceResult Process(IEquationService service)
		{
			return service.Process(Sequence, Inputs, Roles);
		}
	}
}
