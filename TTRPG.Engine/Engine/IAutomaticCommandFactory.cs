using System.Collections.Generic;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.Engine
{
	public interface IAutomaticCommandFactory
	{
		/// <summary>
		///		Returns commands triggered by the processed command
		/// </summary>
		/// <param name="processed"></param>
		/// <returns></returns>
		IEnumerable<EngineCommand> GetSequenceAutomaticCommands(ProcessedCommand processed);

		/// <summary>
		///		Returns commands not triggered by sequences in particular
		/// </summary>
		/// <returns></returns>
		IEnumerable<EngineCommand> GetAutomaticCommands(IEnumerable<Entity> entities);
	}
}
