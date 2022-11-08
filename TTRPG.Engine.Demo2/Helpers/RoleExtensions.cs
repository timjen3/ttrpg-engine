using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Demo2.Controls;

namespace TTRPG.Engine.Demo2.Helpers;

internal static class RoleExtensions
{
	internal static IEnumerable<Role> FilterToLiveTargets(this IEnumerable<Role> roles, string category) =>
		roles.Where(x => !string.IsNullOrWhiteSpace(category) && x.Categories.Contains(category, StringComparer.OrdinalIgnoreCase)
			&& int.Parse(x.Attributes["hp"]) > 0);

	internal static DragDropItem MakeDragDropItem(this Role liveTarget)
	{
		if (liveTarget.Categories.Contains("Crop")
			&& liveTarget.Attributes["Age"] == liveTarget.Attributes["Maturity"])
		{
			return new DragDropItem
			{
				Name = $"{liveTarget.Name} (Mature)",
				Code = liveTarget.Name
			};
		}
		if (liveTarget.Categories.Contains("Animal"))
		{
			return new DragDropItem
			{
				Name = $"{liveTarget.Name} (AC: {liveTarget.Attributes["AC"]})",
				Code = liveTarget.Name
			};
		}
		return new DragDropItem
		{
			Name = liveTarget.Name,
			Code = liveTarget.Name
		};
	}

	internal static HashSet<string> GetCommodityNames(this List<Role> roles)
		=> roles.Where(x => x.Categories.Contains("Commodity", StringComparer.OrdinalIgnoreCase))
		.Select(x => x.Attributes["resource"])
		.ToHashSet(StringComparer.OrdinalIgnoreCase);

	internal static List<InventoryDataGridItem> GetInventoryItems(this Role player) =>
		player.InventoryItems
			.Where(x => x.Value.Attributes.ContainsKey("equipAs"))
			.Select(x => new InventoryDataGridItem
			{
				Name = x.Value.Name,
				EquipAs = x.Value.Attributes["EquipAs"],
				Value = x.Value.Attributes["Value"]
			})
			.OrderBy(x => x.Value)
			.OrderBy(x => x.EquipAs)
			.ToList();

	internal static List<InventoryDataGridItem> GetBagItems(this Role player) =>
		player.Bag
			.Where(x => x.Attributes.ContainsKey("equipAs"))
			.Select(x => new InventoryDataGridItem
			{
				Name = x.Name,
				EquipAs = x.Attributes["EquipAs"],
				Value = x.Attributes["Value"]
			})
			.OrderBy(x => x.Value)
			.OrderBy(x => x.EquipAs)
			.ToList();
}
