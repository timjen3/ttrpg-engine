using System;
using System.Collections.Generic;

namespace TTRPG.Engine
{
	public class EngineCommand
	{
		public string MainCommand { get; set; }
		public List<Role> Roles { get; set; } = new List<Role>();
		public Dictionary<string, string> Inputs { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}
}
