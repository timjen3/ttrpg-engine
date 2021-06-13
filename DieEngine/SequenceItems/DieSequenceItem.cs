namespace DieEngine.SequencesItems
{
	/// <summary>
	///		The result is loaded into inputs to be made available to following equations
	/// </summary>
	public class DieSequenceItem : BaseSequenceItem
	{
		public string ResultName { get; set; }

		public DieSequenceItem(){}

		public DieSequenceItem(string name, string equation, string resultName)
		{
			Name = name;
			Equation = equation;
			ResultName = resultName;
		}
	}
}
