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
        private readonly TTRPGEngineOptions _options;
        private readonly GameObject _data;
        private readonly ICommandProcessorFactory _factory;
        private readonly IEnumerable<ICommandParser> _parsers;

        private IEnumerable<ParsedCommand> GetAutoCommands(ProcessedCommand processed)
        {
            if (!processed.Valid || processed.Failed)
                return new ParsedCommand[0];

            var commands = new List<ParsedCommand>();
            var matches = _options.AutomaticCommands
                .Where(x => processed.CommandCategories.Contains(x.SequenceCategory));
            foreach (var match in matches)
            {
                var rolesMatching = _data.Roles.Where(x => match.Filter(x));
                foreach (var roleMatching in rolesMatching)
                {
                    var command = new ParsedCommand();
                    command.MainCommand = match.Command;
                    command.Inputs = match.Inputs;
                    if (!string.IsNullOrWhiteSpace(match.AliasRolesAs))
                        command.Roles.Add(roleMatching.CloneAs(match.AliasRolesAs));
                    else
                        command.Roles.Add(roleMatching);
                    commands.Add(command);
                }
            }
            return commands;
        }

        public TTRPGEngine(TTRPGEngineOptions options, GameObject data, ICommandProcessorFactory factory, IEnumerable<ICommandParser> parsers)
        {
            _options = options;
            _data = data;
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
            var parsedCommand = _factory.ParseCommand(command);

            return Process(parsedCommand, handleMessages);
        }

        /// <summary>
        ///		Parse and process a command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="handleMessages">if true, messages will go through MessageCreated event handler</param>
        public List<ProcessedCommand> Process(ParsedCommand command, bool handleMessages)
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
