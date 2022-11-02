using EmptyKeys.UserInterface.Mvvm;

namespace TTRPG.Engine.Demo2.Controls
{
	internal class DragDropItem : BindableBase
	{
		private string name;

		public string Name
		{
			get => name;
			set => SetProperty(ref name, value);
		}
	}
}
