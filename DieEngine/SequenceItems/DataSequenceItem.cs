using DieEngine.CustomFunctions;
using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	/// <summary>
	///		Sequence item that carries additional data with it.
	/// </summary>
	/// <typeparam name="T">Data payload type</typeparam>
	public class DataSequenceItem<T> : BaseSequenceItem
	{
		public DataSequenceItem(string name, string equation, T data)
		{
			Name = name;
			Equation = equation;
			Data = data;
		}

		public T Data { get; set; }

		public override SequenceItemResult GetResult(IEquationResolver equationResolver, ref Dictionary<string, double> inputs, IDictionary<string, double> mappedInputs = null)
		{
			return base.GetResult(equationResolver, mappedInputs);
		}
	}
}
