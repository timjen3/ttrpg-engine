using TTRPG.Engine.CommandParsing.Processors;

namespace TTRPG.Engine.CommandParsing
{
	public interface ICommandProcessorFactory
	{
		EngineCommand ParseCommand(string fullCommand);
		ITTRPGCommandProcessor Build(EngineCommand parsedCommand);
	}
}
