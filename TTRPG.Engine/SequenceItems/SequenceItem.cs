using System;

namespace TTRPG.Engine.SequenceItems
{
	/// <summary>
	///		The result is loaded into inputs to be made available to following equations
	/// </summary>
	public class SequenceItem
	{
		private SequenceItemType _sequenceItemType;

		public SequenceItem(){}

		public SequenceItem(string name, string equation, string resultName, SequenceItemType sequenceItemType)
		{
			Name = name;
			Equation = equation;
			ResultName = resultName;
			SequenceItemType = sequenceItemType;
		}

		public string Name { get; set; }

		public string Equation { get; set; }

		public string ResultName { get; set; }

		public SequenceItemType SequenceItemType
		{
			get => _sequenceItemType;
			set
			{
				if (!Enum.IsDefined(typeof(SequenceItemType), value)) throw new ArgumentException($"{value} is an invalid value for Mapping property '{nameof(SequenceItemType)}'.");
				_sequenceItemType = value;
			}
		}
	}
}
