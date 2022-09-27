using System.Collections.Generic;
using System.IO;
using System.Linq;
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
				_roles = Directory.GetFiles(_options.JsonFileStorageOptions.RolesFileDirectory)
					.SelectMany(filename => JsonFileReader.ReadFile<List<Role>>(filename))
					.ToList();
			}

			return Task.FromResult(_roles);
		}

		public Task<List<SequenceItem>> GetSequenceItemsAsync()
		{
			if (_sequenceItems == null)
			{
				_sequenceItems = Directory.GetFiles(_options.JsonFileStorageOptions.SequenceItemsFileDirectory)
					.SelectMany(filename => JsonFileReader.ReadFile<List<SequenceItem>>(filename))
					.ToList();
			}

			return Task.FromResult(_sequenceItems);
		}

		public Task<List<Sequence>> GetSequencesAsync()
		{
			if (_sequences == null)
			{
				_sequences = Directory.GetFiles(_options.JsonFileStorageOptions.SequencesFileDirectory)
					.SelectMany(filename => JsonFileReader.ReadFile<List<Sequence>>(filename))
					.ToList();
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
