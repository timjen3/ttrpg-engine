using System.Collections.Generic;
using System.Threading.Tasks;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Data.TtrpgDataLoaders
{
	public class JsonTTRPGDataRepository : ITTRPGDataRepository
	{
		private readonly TTRPGEngineDataOptions _options;
		private List<Role> _roles;
		private List<Sequence> _sequences;
		private List<SequenceItem> _sequenceItems;

		public JsonTTRPGDataRepository(TTRPGEngineDataOptions options)
		{
			_options = options;
		}

		public Task<List<Role>> GetRolesAsync()
		{
			if (_roles == null)
			{
				var filename = _options.JsonFileStorageOptions.RolesFileName;
				_roles = JsonFileReader.ReadFile<List<Role>>(filename);
			}

			return Task.FromResult(_roles);
		}

		public Task<List<SequenceItem>> GetSequenceItemsAsync()
		{
			if (_sequenceItems == null)
			{
				var filename = _options.JsonFileStorageOptions.SequenceItemsFileName;
				_sequenceItems = JsonFileReader.ReadFile<List<SequenceItem>>(filename);
			}

			return Task.FromResult(_sequenceItems);
		}

		public Task<List<Sequence>> GetSequencesAsync()
		{
			if (_sequences == null)
			{
				var filename = _options.JsonFileStorageOptions.SequencesFileName;
				_sequences = JsonFileReader.ReadFile<List<Sequence>>(filename);
			}

			return Task.FromResult(_sequences);
		}

		public Task ReloadAsync()
		{
			_roles = null;
			_sequences = null;
			_sequenceItems = null;

			return Task.CompletedTask;
		}
	}
}
