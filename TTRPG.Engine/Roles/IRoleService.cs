using System.Collections.Generic;

namespace TTRPG.Engine.Roles
{
	public interface IRoleService
	{
		/// <summary>
		///		Create a new entity from a role
		/// </summary>
		/// <param name="role"></param>
		/// <param name="inputs"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		Entity Birth(Role role, Dictionary<string, string> inputs, string name = null);
	}
}
