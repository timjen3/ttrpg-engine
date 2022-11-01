using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.CommandParsing.Parsers;

namespace TTRPG.Engine
{
	/// <summary>
	///		Parses and processes commands
	/// </summary>
	public class TTRPGEngine
	{
		private readonly TTRPGEngineOptions _options;
		private readonly ICommandProcessorFactory _factory;
		private readonly IEnumerable<ICommandParser> _parsers;

		private IEnumerable<string> GetAutoCommands(ProcessedCommand processed)
		{
			if (!processed.Valid || processed.Failed) return new string[0];

			return _options.AutomaticCommands
				.Where(x => processed.CommandCategories.Contains(x.Key))
				.Select(x => x.Value);
		}

		public TTRPGEngine(TTRPGEngineOptions options, ICommandProcessorFactory factory, IEnumerable<ICommandParser> parsers)
		{
			_options = options;
			_factory = factory;
			_parsers = parsers;
		}

		/// <summary>
		///		Returns example commands from all parsers
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetExampleCommands()
		{
			return _parsers
				.SelectMany(p => p.GetExampleCommands());
		}

		/// <summary>
		///		Parse and process a command
		/// </summary>
		/// <param name="command"></param>
		/// <param name="handleMessages">if true, messages will go through MessageCreated event handler</param>
		public List<ProcessedCommand> Process(string command, bool handleMessages)
		{
			var processed = new List<ProcessedCommand>();
			var parsedCommand = _factory.ParseCommand(command);
			var processor = _factory.Build(parsedCommand);
			if (!processor.IsValid())
			{
				if (handleMessages && MessageCreated != null) MessageCreated(this, "Invalid command.");
				processed.Add(ProcessedCommand.InvalidCommand());

				return processed;
			}
			var result = processor.Process();
			processed.Add(result);
			if (handleMessages && MessageCreated != null)
			{
				foreach (var message in result.Messages)
				{
					MessageCreated(this, message);
				}
			}
			foreach (var autoCommand in GetAutoCommands(result))
			{
				var results = Process(autoCommand, false);
				processed.AddRange(results);
			}
			return processed;
		}

		/// add message handlers
		public event EventHandler<string> MessageCreated;
	}
}
