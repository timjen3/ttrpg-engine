using TTRPG.Engine.CommandParsing;

namespace TTRPG.Engine.Engine.Events
{
	public interface ITTRPGEventHandler
	{
		void ProcessResult(ProcessedCommand result);
	}
}
