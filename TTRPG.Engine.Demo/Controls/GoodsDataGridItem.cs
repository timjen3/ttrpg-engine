namespace TTRPG.Engine.Demo2.Controls;

internal class GoodsDataGridItem
{
	public string Name { get; set; }
	public string Amount { get; set; }

	public static bool operator ==(GoodsDataGridItem lhs, GoodsDataGridItem rhs)
		=> lhs?.Name == rhs?.Name && lhs?.Amount == rhs?.Amount;

	public static bool operator !=(GoodsDataGridItem lhs, GoodsDataGridItem rhs)
		=> lhs?.Name != rhs?.Name || lhs?.Amount != rhs?.Amount;

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

		if (obj is GoodsDataGridItem tObj)
		{
			return this == tObj;
		}

		throw new System.NotImplementedException();
	}
}
