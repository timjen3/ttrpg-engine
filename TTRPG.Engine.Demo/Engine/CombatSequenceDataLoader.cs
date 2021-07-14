using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo.Engine
{
	public class CombatSequenceDataLoader
	{

		private static T ReadFromJsonFile<T>(string filePath)
		{
			var jsonText = File.ReadAllText(filePath);

			return JsonConvert.DeserializeObject<T>(jsonText, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.None,
			});
		}

		public static string SerializeObject(object value)
		{
			return JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
			{
				Converters = new List<JsonConverter> { new StringEnumConverter() },
				TypeNameHandling = TypeNameHandling.None
			});
		}

		public GameObject Load()
		{
			var gameObject = new GameObject();
			var sequences = ReadFromJsonFile<IEnumerable<Sequence>>("DataFiles/sequences.json");
			gameObject.UsePotionSequence = sequences.FirstOrDefault(x => x.Name == "UsePotion");
			gameObject.AttackSequence = sequences.FirstOrDefault(x => x.Name == "Attack");
			var roles = ReadFromJsonFile<IEnumerable<Role>>("DataFiles/roles.json");
			gameObject.Player = roles.FirstOrDefault(x => x.Name == "Player");
			gameObject.PlayerWeapon = roles.FirstOrDefault(x => x.Name == "Sword");
			gameObject.Targets = roles.Where(x => x.Categories.Contains("Entity") && x.Categories.Contains("Enemy")).ToList();
			gameObject.ComputerWeapon = roles.FirstOrDefault(x => x.Name == "Crude Sword");

			return gameObject;
		}
	}

	public class GameObject
	{
		private Role _target;
		List<Role> _targets;

		public Sequence UsePotionSequence { get; set; }

		public Sequence AttackSequence { get; set; }

		public Role Player { get; set; }

		public Role PlayerWeapon { get; set; }

		public List<Role> Targets
		{
			get => _targets;
			set
			{
				_targets = value;
				_target = _targets?.FirstOrDefault();
			}
		}

		public Role ComputerWeapon { get; set; }

		public Role Target => _target;

		public void SetTarget(string name)
		{
			_target = Targets?.FirstOrDefault(x => x.Name == name);
		}
	}
}
