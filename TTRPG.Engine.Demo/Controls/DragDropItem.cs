using EmptyKeys.UserInterface.Mvvm;

namespace TTRPG.Engine.Demo.Controls;

internal class DragDropItem : BindableBase
{
	private string name;
	private string code;

	public string Name
	{
		get => name;
		set => SetProperty(ref name, value);
	}

	public string Code
	{
		get => code;
		set => SetProperty(ref code, value);
	}

	public static bool operator ==(DragDropItem lhs, DragDropItem rhs)
		=> lhs?.Name == rhs?.Name && lhs?.Code == rhs?.Code;

	public static bool operator !=(DragDropItem lhs, DragDropItem rhs)
		=> lhs?.Name != rhs?.Name || lhs?.Code != rhs?.Code;

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

		if (obj is DragDropItem tObj)
		{
			return this == tObj;
		}

		throw new System.NotImplementedException();
	}
}
