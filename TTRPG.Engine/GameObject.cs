using System.Collections.Generic;
using TTRPG.Engine.Data.TtrpgDataLoaders;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine
{
	public class GameObject
	{
		private readonly ITTRPGDataRepository _loader;

		public GameObject(ITTRPGDataRepository loader)
		{
			_loader = loader;
			NewGame();
		}

		public List<Role> Roles => _loader.GetRolesAsync().GetAwaiter().GetResult();

		public List<Sequence> Sequences => _loader.GetSequencesAsync().GetAwaiter().GetResult();

		public List<SequenceItem> SequenceItems => _loader.GetSequenceItemsAsync().GetAwaiter().GetResult();


		public void NewGame()
		{
			_loader.ReloadAsync().GetAwaiter().GetResult();
		}
	}
}
