using System.IO;
using Newtonsoft.Json;

namespace TTRPG.Engine.Data
{
	public static class JsonFileReader
	{
		public static T ReadFile<T>(string filePath)
		{
			var jsonText = File.ReadAllText(filePath);

			return JsonConvert.DeserializeObject<T>(jsonText, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.None,
			});
		}
	}
}
