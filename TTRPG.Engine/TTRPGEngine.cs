using System.Collections.Generic;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.Engine;
using TTRPG.Engine.Engine.Events;

namespace TTRPG.Engine
{
	/// <summary>
	///		Parses and processes commands
	/// </summary>
	public class TTRPGEngine
	{
		private readonly ICommandProcessorFactory _factory;
		private readonly IAutomaticCommandFactory _autoCommandFactory;
		private readonly ITTRPGEventHandler _eventHandler;

		/// <summary>
		///		Create new instance of the TTRPGEngine
		/// </summary>
		/// <param name="factory"></param>
		/// <param name="autoCommandFactory"></param>
		/// <param name="eventHandler"></param>
		public TTRPGEngine(ICommandProcessorFactory factory, IAutomaticCommandFactory autoCommandFactory, ITTRPGEventHandler eventHandler)
		{
			_factory = factory;
			_autoCommandFactory = autoCommandFactory;
			_eventHandler = eventHandler;
		}

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
				return ProcessedCommand.InvalidCommandList();
			}
			var result = processor.Process();
			_eventHandler.ProcessResult(result);
			var results = new List<ProcessedCommand> { result };
			foreach (var autoCommand in _autoCommandFactory.GetAutomaticCommands(result))
			{
				var moreResults = Process(autoCommand);
				results.AddRange(moreResults);
			}

			return results;
		}
	}
}
