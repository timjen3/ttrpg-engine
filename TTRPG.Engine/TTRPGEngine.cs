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

		private List<ProcessedCommand> ProcessResult(ProcessedCommand result)
		{
			var results = new List<ProcessedCommand> { result };

			foreach (var autoCommand in _autoCommandFactory.GetAutomaticCommands(result))
			{
				var moreResults = Process(autoCommand);
				results.AddRange(moreResults);
			}

			return results;
		}

		/// <summary>
		///		Create new instance of the TTRPGEngine
		/// </summary>
		/// <param name="factory"></param>
		/// <param name="parsers"></param>
		/// <param name="autoCommandFactory"></param>
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
		public List<ProcessedCommand> Process(string command)
		{
			var parsedCommand = _factory.ParseCommand(command);

			return Process(parsedCommand);
		}

		/// <summary>
		///		Parse and process a command
		/// </summary>
		/// <param name="command"></param>
		public List<ProcessedCommand> Process(EngineCommand command)
		{
			var processor = _factory.Build(command);
			if (!processor.IsValid())
			{
				return new List<ProcessedCommand>
				{
					ProcessedCommand.InvalidCommand()
				};
			}
			var result = processor.Process();

			return ProcessResult(result);
		}
	}
}
