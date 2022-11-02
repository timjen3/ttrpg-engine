namespace TTRPG.Engine
{
    /// <summary>
    ///		Manages inventory for roles
    /// </summary>
    public interface IInventoryService
    {
        /// <summary>
        ///		Remove item from bag
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="itemName"></param>
        void DropItem(Role entity, string itemName);

        /// <summary>
        ///		Adds item to bag
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        void PickupItem(Role entity, Role item);

        /// <summary>
        ///		Equip inventory item from bag
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="itemName"></param>
        /// <param name="equipAs"></param>
        void Equip(Role entity, string itemName, string equipAs);

        /// <summary>
        ///		Unequip inventory item and put in bag
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="itemName"></param>
        void Unequip(Role entity, string itemName);
    }
}
