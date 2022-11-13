using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TTRPG.Engine.Engine.Events;

namespace TTRPG.Engine.Data.TtrpgDataLoaders
{
	internal class TTRPGEventConfigJsonConverter : JsonConverter<IEventConfig>
	{
		public override IEventConfig ReadJson(JsonReader reader, Type objectType, IEventConfig existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			var data = serializer.Deserialize<JObject>(reader);
			if (data == null)
			{
				return null;
			}
			var eventType = data["EventType"].ToString();
			if (eventType.Equals("UpdateAttributes", StringComparison.OrdinalIgnoreCase))
			{
				return data.ToObject<UpdateAttributesEventConfig>();
			}
			else if (eventType.ToString().Equals("Message", StringComparison.OrdinalIgnoreCase))
			{
				return data.ToObject<MessageEventConfig>();
			}
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, IEventConfig value, JsonSerializer serializer)
			=> serializer.Serialize(writer, value);
	}
}
