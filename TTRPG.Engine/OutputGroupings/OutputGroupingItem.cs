namespace TTRPG.Engine.OutputGroupings
{
	public class OutputGroupingItem
	{
		public OutputGroupingItem()
		{
		}

		public OutputGroupingItem(string name, string source, bool throwIfMissing = false)
		{
			Name = name;
			Source = source;
			ThrowIfMissing = throwIfMissing;
		}

		public string Name { get; set; }
		public string Source { get; set; }
		public bool ThrowIfMissing { get; set; }
	}
}
