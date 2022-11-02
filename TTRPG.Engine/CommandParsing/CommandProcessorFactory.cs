using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TTRPG.Engine.CommandParsing.Parsers;
using TTRPG.Engine.CommandParsing.Processors;

namespace TTRPG.Engine.CommandParsing
{
    public class CommandProcessorFactory : ICommandProcessorFactory
    {
        private readonly GameObject _data;
        private readonly IEnumerable<ICommandParser> _parsers;

        public CommandProcessorFactory(GameObject data, IEnumerable<ICommandParser> parsers)
        {
            _data = data;
            _parsers = parsers;
        }

        public ParsedCommand ParseCommand(string fullCommand)
        {
            var parsedCommand = new ParsedCommand();
            // get sequence
            var mainCommand = Regex.Match(fullCommand, @"^\w*");
            if (mainCommand.Success && !string.IsNullOrWhiteSpace(mainCommand.Value))
            {
                parsedCommand.MainCommand = mainCommand.Value;
            }
            // get roles
            var rolesText = Regex.Match(fullCommand, @"\[.+?\]");
            if (rolesText.Success)
            {
                parsedCommand.Roles = new List<Role>();
                var rolesTextParts = rolesText.Value.Replace("[", "").Replace("]", "").Split(',');
                foreach (var nextRolePart in rolesTextParts)
                {
                    var roleParts = nextRolePart.Split(':');
                    if (roleParts.Length == 2)
                    {
                        // alias role
                        var from = nextRolePart.Split(':')[0];
                        var to = nextRolePart.Split(':')[1];
                        var nextRole = _data.Roles.FirstOrDefault(x => x.Name.Equals(from, StringComparison.OrdinalIgnoreCase));
                        if (nextRole != null)
                        {
                            parsedCommand.Roles.Add(nextRole.CloneAs(to));
                        }
                    }
                    else
                    {
                        // do not alias role
                        var nextRole = _data.Roles.FirstOrDefault(x => x.Name.Equals(nextRolePart, StringComparison.OrdinalIgnoreCase));
                        parsedCommand.Roles.Add(nextRole);
                    }
                }
            }
            // get inputs
            var inputsText = Regex.Match(fullCommand, @"\{.+?\}");
            if (inputsText.Success)
            {
                var inputsTextParts = inputsText.Value.Replace("{", "").Replace("}", "").Split(',');
                foreach (var nextInputPart in inputsTextParts)
                {
                    var from = nextInputPart.Split(':')[0];
                    var to = nextInputPart.Split(':')[1];
                    if (from != null)
                    {
                        parsedCommand.Inputs[from] = to;
                    }
                }
            }
            return parsedCommand;
        }

        public ITTRPGCommandProcessor Build(ParsedCommand parsedCommand)
        {
            ICommandParser parser = null;
            var matches = _parsers.Where(x => !x.IsDefault && x.CanProcess(parsedCommand.MainCommand));
            if (matches.Count() > 1)
                throw new Exception("Found multiple parsers able to process this command.");
            if (matches.Count() == 1)
                parser = matches.Single();
            if (parser == null)
                parser = _parsers.FirstOrDefault(x => x.IsDefault);
            if (parser == null)
                throw new Exception("Unknown command.");

            return parser.GetProcessor(parsedCommand);
        }
    }
}
