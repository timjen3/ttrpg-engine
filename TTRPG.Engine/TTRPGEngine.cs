using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.CommandParsing.Parsers;
using TTRPG.Engine.Engine;

namespace TTRPG.Engine
{
	/// <summary>
	///		Parses and processes commands
	/// </summary>
	public class TTRPGEngine
	{
		private readonly ICommandProcessorFactory _factory;
		private readonly IEnumerable<ICommandParser> _parsers;
		private readonly IAutomaticCommandFactory _autoCommandFactory;

		public TTRPGEngine(ICommandProcessorFactory factory, IEnumerable<ICommandParser> parsers, IAutomaticCommandFactory autoCommandFactory)
		{
			_factory = factory;
			_parsers = parsers;
			_autoCommandFactory = autoCommandFactory;
		}

		/// <summary>
		///		Returns example commands from all parsers
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetExampleCommands() => _parsers
				.SelectMany(p => p.GetExampleCommands());

		/// <summary>
		///		Parse and process a command
		/// </summary>
		/// <param name="command"></param>
		/// <param name="handleMessages">if true, messages will go through MessageCreated event handler</param>
		public List<ProcessedCommand> Process(string command, bool handleMessages)
		{
			var parsedCommand = _factory.ParseCommand(command);

			return Process(parsedCommand, handleMessages);
		}

		/// <summary>
		///		Parse and process a command
		/// </summary>
		/// <param name="command"></param>
		/// <param name="handleMessages">if true, messages will go through MessageCreated event handler</param>
		public List<ProcessedCommand> Process(EngineCommand command, bool handleMessages)
		{
			var processed = new List<ProcessedCommand>();
			var processor = _factory.Build(command);
			if (!processor.IsValid())
			{
				if (handleMessages && MessageCreated != null)
					MessageCreated(this, "Invalid command.");
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
			foreach (var autoCommand in _autoCommandFactory.GetAutomaticCommands(result))
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
