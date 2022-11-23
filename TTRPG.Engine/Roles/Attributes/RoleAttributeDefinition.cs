namespace TTRPG.Engine.Roles.Attributes
{
	public enum RoleAttributeType
	{
		IntRange = 0,
		StaticString = 1,
		StaticInt = 2,
		Reference = 3,
		Derived = 4
	}

	public abstract class RoleAttributeDefinition
	{
		public string Key { get; set; }

		public RoleAttributeType AttributeType { get; set; }
	}
}
