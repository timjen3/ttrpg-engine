using System.Collections.Generic;
using TTRPG.Engine.CommandParsing.Processors;

namespace TTRPG.Engine.CommandParsing
{
	public interface ICommandProcessorFactory
	{
		ITTRPGCommandProcessor Build(string fullCommand);
	}
}