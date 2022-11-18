using System;
using System.Linq;
using TTRPG.Engine.Roles.Attributes;

namespace TTRPG.Engine.Roles
{
	public class RoleService : IRoleService
	{
		private static readonly Random _random = new Random();
		private readonly GameObject _gameObject;

		public RoleService(GameObject gameObject) => _gameObject = gameObject;

		public Entity Birth(string roleName)
		{
			var role = _gameObject.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
			if (role == null)
			{
				throw new ArgumentException($"Unknown role: {roleName}.", nameof(roleName));
			}
			var entity = new Entity();
			entity.Name = role.Next();
			entity.RoleName = role.Name;
			// todo: don't add derived attributes to entities. get them from the role when they're needed
			foreach (var attribute in role.DerivedAttributes)
			{
				entity.Attributes[attribute.Key] = attribute.Value;
			}
			foreach (var attribute in role.Attributes)
			{
				if (attribute is StaticIntAttributeDefinition intAttribute)
				{
					entity.Attributes[attribute.Key] = intAttribute.Value.ToString();
				}
				if (attribute is IntRangeAttributeDefinition intRangeAttribute)
				{
					entity.Attributes[attribute.Key] = _random.Next(intRangeAttribute.MinValue, intRangeAttribute.MaxValue + 1).ToString();
				}
				else if (attribute is StaticStringAttributeDefinition strAttribute)
				{
					entity.Attributes[attribute.Key] = strAttribute.Value;
				}
			}
			entity.Categories = role.Categories;
			_gameObject.Entities.Add(entity);

			return entity;
		}

		public void Bury(string entityName)
			=> _gameObject.Entities.RemoveAll(r => r.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));
	}
}
