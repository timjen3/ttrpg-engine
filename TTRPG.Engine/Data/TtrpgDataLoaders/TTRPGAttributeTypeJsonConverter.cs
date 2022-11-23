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
			var attributeTypeString = data["AttributeType"].ToString();
			var attributeType = (RoleAttributeType) Enum.Parse(typeof(RoleAttributeType), attributeTypeString);
			// convert to specified subclass
			if (attributeType == RoleAttributeType.IntRange)
			{
				return data.ToObject<IntRangeAttributeDefinition>();
			}
			else if (attributeType == RoleAttributeType.StaticInt)
			{
				return data.ToObject<StaticIntAttributeDefinition>();
			}
			else if (attributeType == RoleAttributeType.StaticString)
			{
				return data.ToObject<StaticStringAttributeDefinition>();
			}
			else if (attributeType == RoleAttributeType.Reference)
			{
				return data.ToObject<ReferenceAttributeDefinition>();
			}
			else if (attributeType == RoleAttributeType.Derived)
			{
				var derAttribute = data.ToObject<DerivedAttributeDefinition>();
				if (!DerivedAttributeDefinition.IsEmbeddedFunction(derAttribute.Key))
				{
					throw DerivedAttributeDefinition.CallOutInvalidDerivedAttribute(derAttribute.Key);
				}

				return derAttribute;
			}
			throw new NotImplementedException($"Unrecognized role attribute type: {attributeTypeString}");
		}

		public override void WriteJson(JsonWriter writer, RoleAttributeDefinition value, JsonSerializer serializer)
			=> serializer.Serialize(writer, value);
	}
}
