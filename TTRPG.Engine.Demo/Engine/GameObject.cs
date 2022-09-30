using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Data.TtrpgDataLoaders;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo.Engine
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

		
		public Sequence MineTerrain => Sequences.Single(x => x.Name == "MineTerrain");


		public List<Role> GetLiveTargets(string category) => Roles.Where(x => x.Categories.Contains(category, StringComparer.OrdinalIgnoreCase))
			.Where(x => int.Parse(x.Attributes["hp"]) > 0)
			.ToList();


		public void NewGame()
		{
			_loader.ReloadAsync().GetAwaiter().GetResult();
		}
	}
}
