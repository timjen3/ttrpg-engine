using System.Collections.Generic;
using TTRPG.Engine.CommandParsing.Processors;

namespace TTRPG.Engine.CommandParsing.Parsers
{
    public interface ICommandParser
    {
        /// <summary>
        ///		Returns true if this processor will be used when no other processors apply
        ///		The result of CanProcess() method does not matter when this is true
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        ///		Returns true if this parser handles the given type of command
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        bool CanProcess(string commandType);

        /// <summary>
        ///		Returns a command processor for the command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        ITTRPGCommandProcessor GetProcessor(ParsedCommand command);

        /// <summary>
        ///		Returns example commands for related command handlers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetExampleCommands();
    }
}
