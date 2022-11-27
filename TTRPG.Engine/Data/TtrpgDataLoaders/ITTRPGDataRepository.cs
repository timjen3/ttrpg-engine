using System.Collections.Generic;
using System.Threading.Tasks;
using TTRPG.Engine.Roles;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Data.TtrpgDataLoaders
{
	public interface ITTRPGDataRepository
	{
		Task<List<Sequence>> GetSequencesAsync();

		Task<List<Entity>> GetEntitiesAsync();

		Task<List<Role>> GetRolesAsync();

		Task<List<string>> GetCommandsAsync();
	}
}
