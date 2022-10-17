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
		private readonly ICommandProcessorFactory _factory;
		private readonly IEnumerable<ICommandParser> _parsers;

		public TTRPGEngine(ICommandProcessorFactory factory, IEnumerable<ICommandParser> parsers)
		{
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
		public IEnumerable<string> Process(string command, bool handleMessages)
		{
			var parsedCommand = _factory.ParseCommand(command);
			var processor = _factory.Build(parsedCommand);
			if (!processor.IsValid())
			{
				MessageCreated(this, "Invalid command.");
				return new string[] { "Invalid command." };
			}
			var results = processor.Process();
			if (handleMessages && MessageCreated != null)
			{
				foreach (var message in results)
				{
					MessageCreated(this, message);
				}
			}
			return results;
		}

		/// add message handlers
		public event EventHandler<string> MessageCreated;
	}
}
