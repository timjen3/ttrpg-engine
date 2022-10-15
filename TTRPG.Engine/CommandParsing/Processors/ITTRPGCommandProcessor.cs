using System.Collections.Generic;

namespace TTRPG.Engine.CommandParsing.Processors
{
	public interface ITTRPGCommandProcessor
	{
		/// processes command and returns messages
		IEnumerable<string> Process();

		bool IsValid();
	}
}
