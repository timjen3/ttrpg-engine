namespace TTRPG.Engine
{
	/// <summary>
	///		Manages inventory for entities
	/// </summary>
	public interface IInventoryService
	{
		/// <summary>
		///		Remove item from bag
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="itemName"></param>
		void DropItem(Entity entity, string itemName);

		/// <summary>
		///		Adds item to bag
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="item"></param>
		void PickupItem(Entity entity, Entity item);

		/// <summary>
		///		Equip inventory item from bag
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="itemName"></param>
		/// <param name="equipAs"></param>
		void Equip(Entity entity, string itemName, string equipAs);

		/// <summary>
		///		Unequip inventory item and put in bag
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="itemName"></param>
		void Unequip(Entity entity, string itemName);
	}
}
