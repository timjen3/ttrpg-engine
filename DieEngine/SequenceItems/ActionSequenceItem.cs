namespace DieEngine.SequencesItems
{
	public class ActionSequenceItem<T> : BaseSequenceItem
	{
		public ActionSequenceItem(string name, string equation, T data)
		{
			Name = name;
			Equation = equation;
			Data = data;
		}

		public T Data { get; set; }
	}
}
