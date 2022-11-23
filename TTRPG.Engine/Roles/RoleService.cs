using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Roles.Attributes;

namespace TTRPG.Engine.Roles
{
	public class RoleService : IRoleService
	{
		private static readonly Random _random = new Random();

		public Entity Birth(Role role, Dictionary<string, string> inputs, string name = null)
		{
			var entity = new Entity();
			entity.Name = role.Next(name);
			entity.Attributes["Name"] = entity.Name;
			foreach (var attribute in role.Attributes.Where(x => !(x is ReferenceAttributeDefinition)))
			{
				if (attribute is StaticIntAttributeDefinition intAttribute)
				{
					entity.Attributes[attribute.Key] = intAttribute.Value.ToString();
				}
				else if (attribute is IntRangeAttributeDefinition intRangeAttribute)
				{
					entity.Attributes[attribute.Key] = _random.Next(intRangeAttribute.MinValue, intRangeAttribute.MaxValue + 1).ToString();
				}
				else if (attribute is StaticStringAttributeDefinition strAttribute)
				{
					entity.Attributes[attribute.Key] = strAttribute.Value;
				}
				else if (attribute is DerivedAttributeDefinition derAttribute)
				{
					entity.Attributes[attribute.Key] = derAttribute.Value;
				}
			}
			// set reference attributes last
			foreach (var attribute in role.Attributes)
			{
				if (attribute is ReferenceAttributeDefinition refAttribute)
				{
					entity.Attributes[refAttribute.Key] = entity.Attributes[refAttribute.ReferenceKey];
				}
			}
			// set any additional values from inputs
			foreach (var kvp in inputs)
			{
				entity.Attributes[kvp.Key] = kvp.Value;
			}
			entity.Categories = role.Categories;

			return entity;
		}
	}
}
