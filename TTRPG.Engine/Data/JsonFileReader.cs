using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TTRPG.Engine.Data
{
	public static class JsonFileReader
	{
		public static async Task<T> ReadFileAsync<T>(string filePath)
		{
			var jsonText = await File.ReadAllTextAsync(filePath);

			return JsonConvert.DeserializeObject<T>(jsonText, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.None
			});
		}
	}
}
