using System;
using System.Linq;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine
{
    public class InventoryService : IInventoryService
    {
        /// <see cref="IInventoryService.Equip(Role, string, string)"/>
        public void Equip(Role entity, string itemName, string equipAs)
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

        /// <see cref="IInventoryService.Unequip(Role, string)"/>
        public void Unequip(Role entity, string itemName)
        {
            if (!entity.InventoryItems.ContainsKey(itemName))
                throw new InventoryServiceException("Tried to unequip item that is not equipped.");

            var item = entity.InventoryItems[itemName];
            entity.InventoryItems.Remove(itemName);
            entity.Bag.Add(item);
        }

        /// <see cref="IInventoryService.DropItem(Role, string)"/>
        public void DropItem(Role entity, string itemName)
        {
            var item = entity.Bag
                .FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (item == null)
                throw new InventoryServiceException("Tried to drop item that is not in bag.");

            entity.Bag.Remove(item);
        }

        /// <see cref="IInventoryService.PickupItem(Role, Role)"/>
        public void PickupItem(Role entity, Role item)
        {
            entity.Bag.Add(item);
        }
    }
}
