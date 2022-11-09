namespace TTRPG.Engine.Demo2.Controls;

internal class InventoryDataGridItem
{
	public string Name { get; set; }
	public string EquipAs { get; set; }
	public string Value { get; set; }

	public static bool operator ==(InventoryDataGridItem lhs, InventoryDataGridItem rhs)
		=> lhs?.Name == rhs?.Name && lhs?.EquipAs == rhs?.EquipAs && lhs?.Value == rhs?.Value;

	public static bool operator !=(InventoryDataGridItem lhs, InventoryDataGridItem rhs)
		=> lhs?.Name != rhs?.Name || lhs?.EquipAs != rhs?.EquipAs || lhs?.Value != rhs?.Value;

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (ReferenceEquals(obj, null))
		{
			return false;
		}

		if (obj is InventoryDataGridItem tObj)
		{
			return this == tObj;
		}

		throw new System.NotImplementedException();
	}
}
