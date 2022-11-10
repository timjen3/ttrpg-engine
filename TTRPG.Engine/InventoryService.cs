using System;
using System.Linq;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine
{
	public class InventoryService : IInventoryService
	{
		/// <see cref="IInventoryService.Equip(Entity, string, string)"/>
		public void Equip(Entity entity, string itemName, string equipAs)
		{
			// if existing item in slot, unequip
			if (entity.InventoryItems.ContainsKey(equipAs))
			{
				Unequip(entity, equipAs);
			}

			var item = entity.Bag
				.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
			if (item == null)
				throw new InventoryServiceException("Tried to equip item that is not in bag.");

			entity.Bag.Remove(item);
			entity.InventoryItems[equipAs] = item;
		}

		/// <see cref="IInventoryService.Unequip(Entity, string)"/>
		public void Unequip(Entity entity, string itemName)
		{
			if (!entity.InventoryItems.ContainsKey(itemName))
				throw new InventoryServiceException("Tried to unequip item that is not equipped.");

			var item = entity.InventoryItems[itemName];
			entity.InventoryItems.Remove(itemName);
			entity.Bag.Add(item);
		}

		/// <see cref="IInventoryService.DropItem(Entity, string)"/>
		public void DropItem(Entity entity, string itemName)
		{
			var item = entity.Bag
				.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
			if (item == null)
				throw new InventoryServiceException("Tried to drop item that is not in bag.");

			entity.Bag.Remove(item);
		}

		/// <see cref="IInventoryService.PickupItem(Entity, Entity)"/>
		public void PickupItem(Entity entity, Entity item)
		{
			entity.Bag.Add(item);
		}
	}
}
