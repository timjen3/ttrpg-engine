using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTRPG.Engine.Data.TtrpgDataLoaders;
using TTRPG.Engine.Roles;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine
{
	public class GameObject
	{
		private readonly ITTRPGDataRepository _loader;

		public GameObject(ITTRPGDataRepository loader) => _loader = loader;

		public async Task LoadAsync(TTRPGEngine engine)
		{
			Roles = await _loader.GetRolesAsync();
			Entities = await _loader.GetEntitiesAsync();
			Sequences = await _loader.GetSequencesAsync();
			SequenceItems = await _loader.GetSequenceItemsAsync();
			// process commands
			var commands = await _loader.GetCommandsAsync();
			foreach (var command in commands)
			{
				engine.Process(command);
			}
		}

		public List<Entity> Entities { get; private set; }

		public List<Sequence> Sequences { get; private set; }

		public List<SequenceItem> SequenceItems { get; private set; }

		public List<Role> Roles { get; private set; }

		/// <summary>
		///		Remove specified entity from game
		/// </summary>
		/// <param name="entityName"></param>
		public void Bury(string entityName) => Entities.RemoveAll(r => r.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));
	}
}
