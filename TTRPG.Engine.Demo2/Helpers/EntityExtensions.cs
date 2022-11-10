using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Demo2.Controls;

namespace TTRPG.Engine.Demo2.Helpers;

internal static class EntityExtensions
{
	internal static IEnumerable<Entity> FilterToLiveTargets(this IEnumerable<Entity> entities, string category) =>
		entities.Where(x => !string.IsNullOrWhiteSpace(category) && x.Categories.Contains(category, StringComparer.OrdinalIgnoreCase)
			&& int.Parse(x.Attributes["hp"]) > 0);

	internal static DragDropItem MakeDragDropItem(this Entity liveTarget)
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

	internal static HashSet<string> GetCommodityNames(this List<Entity> entities)
		=> entities.Where(x => x.Categories.Contains("Commodity", StringComparer.OrdinalIgnoreCase))
		.Select(x => x.Attributes["resource"])
		.ToHashSet(StringComparer.OrdinalIgnoreCase);

	internal static List<InventoryDataGridItem> GetInventoryItems(this Entity player) =>
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

	internal static List<InventoryDataGridItem> GetBagItems(this Entity player) =>
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

	internal static List<GoodsDataGridItem> GetGoods(this Entity player, HashSet<string> commodityNames) =>
		player.Attributes
			.Where(x => commodityNames.Contains(x.Key))
			.Select(x => new GoodsDataGridItem
			{
				Name = x.Key,
				Amount = x.Value
			})
			.ToList();
}
