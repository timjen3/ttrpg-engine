using System.Collections.Generic;

namespace TTRPG.Engine.Roles
{
	public class EntityCondition
	{
		public string EntityName { get; set; }

		public List<string> RequiredCategories { get; set; } = new List<string>();
	}
}
