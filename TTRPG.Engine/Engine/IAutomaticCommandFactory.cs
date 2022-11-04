using System.Collections.Generic;
using TTRPG.Engine.CommandParsing;

namespace TTRPG.Engine.Engine
{
	public interface IAutomaticCommandFactory
	{
		IEnumerable<EngineCommand> GetAutomaticCommands(ProcessedCommand processed);
	}
}