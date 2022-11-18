namespace TTRPG.Engine.Roles
{
	public interface IRoleService
	{
		/// <summary>
		///		Create a new entity from a role
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		Entity Birth(string roleName);

		/// <summary>
		///		Remove an entity from the game
		/// </summary>
		/// <param name="entityName"></param>
		void Bury(string entityName);
	}
}
