namespace TTRPG.Engine.Roles.Attributes
{
	/// <summary>
	///		Set after all other attributes.
	///		Set to the value of the referenced attribute
	/// </summary>
	public class ReferenceAttributeDefinition : RoleAttributeDefinition
	{
		public string ReferenceKey { get; set; }
	}
}
