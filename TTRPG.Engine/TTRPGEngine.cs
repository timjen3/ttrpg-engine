using System;
using System.Collections.Generic;
using System.Linq;
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

		/// recursive; process a command and process any automatic commands triggered
		private List<ProcessedCommand> InternalProcess(EngineCommand command)
		{
			var currentCommand = command;
			var results = new List<ProcessedCommand>();
			try
			{
				var processor = _factory.Build(command);
				if (!processor.IsValid())
				{
					return ProcessedCommand.InvalidCommandList();
				}
				var result = processor.Process();
				_eventHandler.ProcessResult(result);
				results.Add(result);
				foreach (var autoCommand in _autoCommandFactory.GetSequenceAutomaticCommands(result))
				{
					currentCommand = autoCommand;
					var moreResults = InternalProcess(autoCommand);
					results.AddRange(moreResults);
				}
			}
			catch (Exception ex)
			{
				results.Add(ProcessedCommand.InvalidCommand($"{currentCommand.MainCommand}: {ex.Message}"));
			}
			return results;
		}

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
			var currentCommand = command;
			// process command and any commands specifically triggered by it
			var results = InternalProcess(command);
			// process commands
			try
			{
				var affectedEntities = results.SelectMany(x => x.Source.Entities);
				if (affectedEntities.Any())
				{
					foreach (var autoCommand in _autoCommandFactory.GetAutomaticCommands(affectedEntities))
					{
						currentCommand = autoCommand;
						var moreResults = InternalProcess(autoCommand);
						results.AddRange(moreResults);
					}
				}
			}
			catch (Exception ex)
			{
				results.Add(ProcessedCommand.InvalidCommand($"{currentCommand.MainCommand}: {ex.Message}"));
			}

			return results;
		}
	}
}
