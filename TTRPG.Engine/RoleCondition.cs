using System.Collections.Generic;

namespace TTRPG.Engine
{
	public class RoleCondition
	{
		public string RoleName { get; set; }

		public List<string> RequiredCategories { get; set; } = new List<string>();
	}
}
