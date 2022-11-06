using EmptyKeys.UserInterface.Mvvm;

namespace TTRPG.Engine.Demo2.Controls
{
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
	}
}
