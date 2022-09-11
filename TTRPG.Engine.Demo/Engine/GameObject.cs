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


		public Sequence UsePotionSequence => Sequences.Single(x => x.Name == "UsePotion");

		public Sequence AttackSequence => Sequences.Single(x => x.Name == "Attack");

		public Sequence CheckIsDead => Sequences.Single(x => x.Name == "CheckIsDead");

		public Sequence MissingHalfHP => Sequences.Single(x => x.Name == "MissingHalfHP");


		public Role Player => Roles.Single(x => x.Name == "Player");


		public List<Role> Targets => Roles.Where(x => x.Categories.Contains("Entity") && x.Categories.Contains("Enemy")).ToList();


		public SequenceItem HitPoints => SequenceItems.Single(x => x.Name == "HitPoints");

		public SequenceItem Potions => SequenceItems.Single(x => x.Name == "Potions");


		public Role Target { get; set; }


		public void SetTarget(string name)
		{
			Target = Targets?.FirstOrDefault(x => x.Name == name);
		}

		public void NewGame()
		{
			_loader.ReloadAsync().GetAwaiter().GetResult();
			Target = Targets.First();
		}
	}
}
