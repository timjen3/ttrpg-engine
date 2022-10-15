using System;
using System.Collections.Generic;

namespace TTRPG.Engine.CommandParsing
{
	public class ParsedCommand
	{
		public string MainCommand { get; set; }
		public List<Role> Roles { get; set; }
		public Dictionary<string, string> Inputs { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}
}
