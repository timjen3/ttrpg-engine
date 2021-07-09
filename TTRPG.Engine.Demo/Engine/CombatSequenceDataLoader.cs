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

		public void Load()
		{
			var sequences = ReadFromJsonFile<IEnumerable<Sequence>>("DataFiles/sequences.json");
			UsePotionSequence = sequences.FirstOrDefault(x => x.Name == "UsePotion");
			AttackSequence = sequences.FirstOrDefault(x => x.Name == "Attack");
			var roles = ReadFromJsonFile<IEnumerable<Role>>("DataFiles/roles.json");
			Player = roles.FirstOrDefault(x => x.Name == "Player");
			PlayerWeapon = roles.FirstOrDefault(x => x.Name == "Sword");
			Computer = roles.FirstOrDefault(x => x.Name == "Bandit");
			ComputerWeapon = roles.FirstOrDefault(x => x.Name == "Crude Sword");
		}

		public Sequence UsePotionSequence { get; private set; }

		public Sequence AttackSequence { get; private set; }

		public Role Player { get; private set; }

		public Role PlayerWeapon { get; private set; }

		public Role Computer { get; private set; }

		public Role ComputerWeapon { get; private set; }
	}
}
