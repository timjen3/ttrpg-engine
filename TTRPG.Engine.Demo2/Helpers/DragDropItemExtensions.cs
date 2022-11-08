using System.Collections.ObjectModel;
using TTRPG.Engine.Demo2.Controls;

namespace TTRPG.Engine.Demo2.Helpers;

internal static class DragDropItemExtensions
{
	internal static bool ItemsChanged(this ObservableCollection<DragDropItem> currentItems, DragDropItem[] updatedItems)
	{
		if (currentItems == null && updatedItems == null)
			return false;
		if (currentItems == null && updatedItems != null || currentItems != null && updatedItems == null)
			return true;
		if (currentItems.Count != updatedItems.Length)
			return true;

		for (var i = 0; i < updatedItems.Length; i++)
		{
			if (updatedItems[i] != currentItems[i])
				return true;
		}

		return false;
	}
}
