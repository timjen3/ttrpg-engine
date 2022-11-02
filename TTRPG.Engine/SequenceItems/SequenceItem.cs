using System;

namespace TTRPG.Engine.SequenceItems
{
    /// <summary>
    ///		The result is loaded into inputs to be made available to following equations
    /// </summary>
    public class SequenceItem
    {
        private SequenceItemEquationType _sequenceItemEquationType;

        public SequenceItem() { }

        public SequenceItem(string name, string equation, string resultName, SequenceItemEquationType sequenceItemType)
        {
            Name = name;
            Equation = equation;
            ResultName = resultName;
            SequenceItemEquationType = sequenceItemType;
        }

        public string Name { get; set; }

        public string Equation { get; set; }

        public string ResultName { get; set; }

        public SequenceItemEquationType SequenceItemEquationType
        {
            get => _sequenceItemEquationType;
            set
            {
                if (!Enum.IsDefined(typeof(SequenceItemEquationType), value))
                    throw new ArgumentException($"{value} is an invalid value for Mapping property '{nameof(SequenceItemEquationType)}'.");
                _sequenceItemEquationType = value;
            }
        }
    }
}
