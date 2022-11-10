using System;
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
		private List<Entity> _entities;
		private List<Sequence> _sequences;
		private List<SequenceItem> _sequenceItems;
		private Dictionary<string, string> _messageTemplates;

		private Task<Dictionary<string, string>> GetMessageTemplatesAsync()
		{
			if (_messageTemplates == null && !string.IsNullOrWhiteSpace(_options.JsonFileStorageOptions.MessageTemplatesDirectory))
			{
				_messageTemplates = Directory.GetFiles(_options.JsonFileStorageOptions.MessageTemplatesDirectory)
					.Select(filename => new FileInfo(filename))
					.Select(fileInfo =>
					{
						var templateName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf('.'));
						var template = File.ReadAllText(fileInfo.FullName);

						return new KeyValuePair<string, string>(templateName, template);
					})
					.ToDictionary(kvp => $"[[{kvp.Key}]]", kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
			}
			else if (_messageTemplates == null)
				_messageTemplates = new Dictionary<string, string>();

			return Task.FromResult(_messageTemplates);
		}

		public JsonTTRPGDataRepository(TTRPGEngineDataOptions options)
		{
			_options = options;
		}

		public Task<List<Entity>> GetEntitiesAsync()
		{
			if (_entities == null && !string.IsNullOrWhiteSpace(_options.JsonFileStorageOptions.EntitiesFileDirectory))
			{
				_entities = Directory.GetFiles(_options.JsonFileStorageOptions.EntitiesFileDirectory)
					.SelectMany(filename => JsonFileReader.ReadFile<List<Entity>>(filename))
					.ToList();
			}
			else if (_entities == null)
				_entities = new List<Entity>();

			return Task.FromResult(_entities);
		}

		public Task<List<SequenceItem>> GetSequenceItemsAsync()
		{
			if (_sequenceItems == null && !string.IsNullOrWhiteSpace(_options.JsonFileStorageOptions.SequenceItemsFileDirectory))
			{
				_sequenceItems = Directory.GetFiles(_options.JsonFileStorageOptions.SequenceItemsFileDirectory)
					.SelectMany(filename => JsonFileReader.ReadFile<List<SequenceItem>>(filename))
					.ToList();
			}
			else if (_sequenceItems == null)
				_sequenceItems = new List<SequenceItem>();

			return Task.FromResult(_sequenceItems);
		}

		public async Task<List<Sequence>> GetSequencesAsync()
		{
			if (_sequences == null && !string.IsNullOrWhiteSpace(_options.JsonFileStorageOptions.SequencesFileDirectory))
			{
				_sequences = Directory.GetFiles(_options.JsonFileStorageOptions.SequencesFileDirectory)
					.SelectMany(filename => JsonFileReader.ReadFile<List<Sequence>>(filename))
					.ToList();
			}
			else if (_sequences == null)
				_sequences = new List<Sequence>();
			// replace message with template if match is found
			var templates = await GetMessageTemplatesAsync();
			foreach (var sequence in _sequences)
			{
				foreach (var item in sequence.Items)
				{
					if (item.SequenceItemEquationType == SequenceItemEquationType.Message)
					{
						if (templates.ContainsKey(item.Equation))
						{
							item.Equation = templates[item.Equation];
						}
					}
				}
			}

			return _sequences;
		}

		public Task ReloadAsync()
		{
			_entities = null;
			_sequences = null;
			_sequenceItems = null;
			_messageTemplates = null;

			return Task.CompletedTask;
		}
	}
}
