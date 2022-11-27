using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TTRPG.Engine.Engine.Events;
using TTRPG.Engine.Roles;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Data.TtrpgDataLoaders
{
	public class JsonTTRPGDataRepository : ITTRPGDataRepository
	{
		private readonly TTRPGEngineDataOptions _options;

		private async Task<Dictionary<string, string>> GetMessageTemplatesAsync()
		{
			var messageTemplates = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			if (!string.IsNullOrWhiteSpace(_options.JsonFileStorageOptions.MessageTemplatesDirectory))
			{
				var fileNames = Directory.GetFiles(_options.JsonFileStorageOptions.MessageTemplatesDirectory);
				foreach (var fileName in fileNames)
				{
					var fileInfo = new FileInfo(fileName);
					var templateName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf('.'));
					var template = await File.ReadAllTextAsync(fileInfo.FullName);
					messageTemplates[templateName] = template;
				}
			}

			return messageTemplates;
		}

		public JsonTTRPGDataRepository(TTRPGEngineDataOptions options)
		{
			_options = options;
		}

		public async Task<List<Entity>> GetEntitiesAsync()
		{
			var entities = new List<Entity>();
			if (!string.IsNullOrWhiteSpace(_options.JsonFileStorageOptions.EntitiesFileDirectory))
			{
				var fileNames = Directory.GetFiles(_options.JsonFileStorageOptions.EntitiesFileDirectory);
				foreach (var fileName in fileNames)
				{
					var moreEntities = await JsonFileReader.ReadFileAsync<List<Entity>>(fileName);
					entities.AddRange(moreEntities);
				}
			}

			return entities;
		}

		public async Task<List<Sequence>> GetSequencesAsync()
		{
			var sequences = new List<Sequence>();
			if (!string.IsNullOrWhiteSpace(_options.JsonFileStorageOptions.SequencesFileDirectory))
			{
				var fileNames = Directory.GetFiles(_options.JsonFileStorageOptions.SequencesFileDirectory);
				foreach (var fileName in fileNames)
				{
					var moreSequences = await JsonFileReader.ReadFileAsync<List<Sequence>>(fileName);
					sequences.AddRange(moreSequences);
				}
			}
			if (!sequences.Any())
			{
				return sequences;
			}

			// replace message with template if match is found
			var templates = await GetMessageTemplatesAsync();
			foreach (var sequence in sequences)
			{
				foreach (var @event in sequence.Events)
				{
					if (@event is MessageEventConfig mEvent)
					{
						if (mEvent.TemplateFilename != null && templates.ContainsKey(mEvent.TemplateFilename))
						{
							mEvent.MessageTemplate = templates[mEvent.TemplateFilename];
						}
					}
				}
			}

			return sequences;
		}

		public async Task<List<Role>> GetRolesAsync()
		{
			var roles = new List<Role>();
			if (!string.IsNullOrWhiteSpace(_options.JsonFileStorageOptions.RolesFileDirectory))
			{
				var fileNames = Directory.GetFiles(_options.JsonFileStorageOptions.RolesFileDirectory);
				foreach (var fileName in fileNames)
				{
					var moreRoles = await JsonFileReader.ReadFileAsync<List<Role>>(fileName);
					roles.AddRange(moreRoles);
				}
			}

			return roles;
		}

		public async Task<List<string>> GetCommandsAsync()
		{
			var commands = new List<string>();
			if (!string.IsNullOrWhiteSpace(_options.JsonFileStorageOptions.CommandsDirectory))
			{
				var fileNames = Directory.GetFiles(_options.JsonFileStorageOptions.CommandsDirectory);
				foreach (var fileName in fileNames)
				{
					var moreCommandsText = await File.ReadAllTextAsync(fileName);
					var splitCommands = moreCommandsText.Split("\n", StringSplitOptions.RemoveEmptyEntries)
						.Select(command => command.Trim())
						.Where(command => !string.IsNullOrWhiteSpace(command) && !command.StartsWith("//"));
					commands.AddRange(splitCommands);
				}
			}

			return commands;
		}
	}
}
