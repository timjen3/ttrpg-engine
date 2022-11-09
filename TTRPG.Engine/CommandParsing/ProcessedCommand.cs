using System.Collections.Generic;

namespace TTRPG.Engine.CommandParsing
{
	public class ProcessedCommand
	{
		public EngineCommand Source { get; set; }

		public List<string> CommandCategories { get; set; } = new List<string>();

		public Dictionary<string, Dictionary<string, string>> CategoryParams { get; set; } = new Dictionary<string, Dictionary<string, string>>();

		public List<string> Messages { get; set; } = new List<string>();

		public bool Valid { get; set; } = true;

		public bool Failed { get; set; }

		public bool Completed { get; set; }

		public static ProcessedCommand InvalidCommand() => new ProcessedCommand
		{
			Valid = false,
			Failed = true,
			Messages = new List<string> { "Invalid command." }
		};
	}
}
