using System;
using System.Collections.Generic;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine
{
	public class EngineCommand
	{
		public string MainCommand { get; set; }
		public List<Entity> Entities { get; set; } = new List<Entity>();
		public Dictionary<string, string> Inputs { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}
}
