using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Engine.Events;

namespace TTRPG.Engine.CommandParsing
{
	public class ProcessedCommand
	{
		public EngineCommand Source { get; set; }

		public List<string> CommandCategories { get; set; } = new List<string>();

		public Dictionary<string, Dictionary<string, string>> CategoryParams { get; set; } = new Dictionary<string, Dictionary<string, string>>();

		public List<TTRPGEvent> Events { get; set; } = new List<TTRPGEvent>();

		public List<string> Messages => Events
			.Where(x => x is MessageEvent)
			.Cast<MessageEvent>()
			.Select(x => x.Message)
			.ToList();

		public bool Valid { get; set; } = true;

		public bool Failed { get; set; }

		public bool Completed { get; set; }

		public static ProcessedCommand InvalidCommand(string message = null) => new ProcessedCommand
		{
			Valid = false,
			Failed = true,
			Events = new List<TTRPGEvent> { new MessageEvent
				{
					Level = MessageEventLevel.Error,
					Message = message ?? "Invalid command."
				}
			}
		};

		public static List<ProcessedCommand> InvalidCommandList() => new List<ProcessedCommand>
		{
			InvalidCommand()
		};
	}
}
