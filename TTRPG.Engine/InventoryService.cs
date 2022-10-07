using System;
using System.Linq;

namespace TTRPG.Engine
{
	public class InventoryService : IInventoryService
	{
		/// <see cref="IInventoryService.Equip(Role, string, string)"/>
		public void Equip(Role role, string itemName, string equipAs)
		{
			// if existing item in slot, unequip
			if (role.InventoryItems.ContainsKey(equipAs))
			{
				Unequip(role, equipAs);
			}

			var item = role.Bag
				.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
			if (item == null) throw new Exception("Tried to equip item that is not in bag.");

			role.Bag.Remove(item);
			role.InventoryItems[equipAs] = item;
		}

		/// <see cref="IInventoryService.Unequip(Role, string)"/>
		public void Unequip(Role role, string itemName)
		{
			if (!role.InventoryItems.ContainsKey(itemName)) throw new Exception("Tried to unequip item that is not equipped.");

			var item = role.InventoryItems[itemName];
			role.InventoryItems.Remove(itemName);
			role.Bag.Add(item);
		}

		/// <see cref="IInventoryService.DropItem(Role, string)"/>
		public void DropItem(Role role, string itemName)
		{
			var item = role.Bag
				.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
			if (item == null) throw new Exception("Tried to drop item that is not in bag.");

			role.Bag.Remove(item);
		}

		/// <see cref="IInventoryService.PickupItem(Role, Role)"/>
		public void PickupItem(Role role, Role item)
		{
			role.Bag.Add(item);
		}


	}
}
