using TTRPG.Engine.Demo2.Controls;

namespace TTRPG.Engine.Demo2.Helpers;

internal static class InventoryDataGridItemExtensions
{
	internal static string MakeUnequipCommand(this InventoryDataGridItem selectedItem)
	{
		if (selectedItem == null)
		{
			return "";
		}
		return $"Unequip [miner] {{itemName:{selectedItem.EquipAs}}}";
	}

	internal static string MakeEquipCommand(this InventoryDataGridItem selectedItem)
	{
		if (selectedItem == null)
		{
			return "";
		}
		return $"Equip [miner] {{itemName:{selectedItem.Name},equipAs:{selectedItem.EquipAs}}}";
	}
}
