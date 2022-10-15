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

		public IEnumerable<string> GetExampleCommands()
		{
			return _parsers
				.SelectMany(p => p.GetExampleCommands());
		}

		/// <summary>
		///		Parse and process a command
		///		Handle results
		/// </summary>
		/// <param name="command"></param>
		public void Process(string command)
		{
			var processor = _factory.Build(command);
			if (!processor.IsValid())
			{
				MessageCreated(this, "Invalid command.");
				return;
			}
			var results = processor.Process();
			foreach (var message in results)
			{
				MessageCreated(this, message);
			}
		}

		/// add message handlers
		public event EventHandler<string> MessageCreated;
	}
}
