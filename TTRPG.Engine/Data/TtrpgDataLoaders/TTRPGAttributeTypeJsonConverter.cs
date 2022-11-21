using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TTRPG.Engine.Roles.Attributes;

namespace TTRPG.Engine.Data.TtrpgDataLoaders
{
	internal class TTRPGAttributeTypeJsonConverter : JsonConverter<RoleAttributeDefinition>
	{
		public override RoleAttributeDefinition ReadJson(JsonReader reader, Type objectType, RoleAttributeDefinition existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			var data = serializer.Deserialize<JObject>(reader);
			if (data == null)
			{
				return null;
			}
			var eventType = data["AttributeType"].ToString();
			if (eventType.Equals("IntRange", StringComparison.OrdinalIgnoreCase))
			{
				return data.ToObject<IntRangeAttributeDefinition>();
			}
			else if (eventType.Equals("StaticInt", StringComparison.OrdinalIgnoreCase))
			{
				return data.ToObject<StaticIntAttributeDefinition>();
			}
			else if (eventType.ToString().Equals("StaticString", StringComparison.OrdinalIgnoreCase))
			{
				return data.ToObject<StaticStringAttributeDefinition>();
			}
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, RoleAttributeDefinition value, JsonSerializer serializer)
			=> serializer.Serialize(writer, value);
	}
}
