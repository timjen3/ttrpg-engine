using System.Collections.Generic;

namespace TTRPG.Engine.SequenceItems
{
	/// <summary>
	///		The result is loaded into inputs to be made available to following equations
	/// </summary>
	public class SequenceItem
	{
		public SequenceItem() { }

		public SequenceItem(string name, string equation, string resultName, bool setComplete = false, List<string> produces = null)
		{
			Name = name;
			Equation = equation;
			ResultName = resultName;
			SetComplete = setComplete;
			Produces = produces ?? new List<string>();
		}

		public string Name { get; set; }

		public string Equation { get; set; }

		public string ResultName { get; set; }

		public bool SetComplete { get; set; }

		public List<string> Produces { get; set; } = new List<string>();
	}
}
