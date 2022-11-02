using TTRPG.Engine.CommandParsing.Processors;

namespace TTRPG.Engine.CommandParsing
{
	public interface ICommandProcessorFactory
	{
		ParsedCommand ParseCommand(string fullCommand);
		ITTRPGCommandProcessor Build(ParsedCommand parsedCommand);
	}
}
