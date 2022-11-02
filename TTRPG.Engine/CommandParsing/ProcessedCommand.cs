using System.Collections.Generic;

namespace TTRPG.Engine.CommandParsing
{
	public class ProcessedCommand
	{
		public ParsedCommand Source { get; set; }

		public List<string> CommandCategories { get; set; } = new List<string>();

		public List<string> Messages { get; set; } = new List<string>();

		public bool Valid { get; set; } = true;

		public bool Failed { get; set; }

		public static ProcessedCommand InvalidCommand()
		{
			return new ProcessedCommand
			{
				Valid = false,
				Failed = true,
				Messages = new List<string> { "Invalid command." }
			};
		}
	}
}
