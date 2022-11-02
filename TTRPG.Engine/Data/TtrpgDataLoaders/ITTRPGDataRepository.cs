using System.Collections.Generic;
using System.Threading.Tasks;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Data.TtrpgDataLoaders
{
	public interface ITTRPGDataRepository
	{
		Task<List<Sequence>> GetSequencesAsync();

		Task<List<SequenceItem>> GetSequenceItemsAsync();

		Task<List<Role>> GetRolesAsync();

		Task ReloadAsync();
	}
}
