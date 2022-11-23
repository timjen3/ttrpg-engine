using System.Collections.Generic;
using System.Linq;

namespace TTRPG.Engine.Demo.Helpers;

internal static class ListChangedHelpers
{
	internal static bool ItemsChanged(this IEnumerable<object> currentItems, IEnumerable<object> updatedItems)
	{
		if (ReferenceEquals(currentItems, updatedItems))
			return false;
		if (currentItems == null && updatedItems != null || currentItems != null && updatedItems == null)
			return true;
		if (currentItems.Count() != updatedItems.Count())
			return true;

		var updatedItemsEnumerable = updatedItems.GetEnumerator();
		foreach (var item in currentItems)
		{
			updatedItemsEnumerable.MoveNext();
			if (!item.Equals(updatedItemsEnumerable.Current))
			{
				return true;
			}
		}

		return false;
	}
}
