using TTRPG.Engine.Equations;
using System.Collections.Generic;

namespace TTRPG.Engine.SequenceItems
{
	/// <summary>
	///		Sequence item that carries additional data with it.
	/// </summary>
	/// <typeparam name="T">Data payload type</typeparam>
	public class DataSequenceItem<T> : BaseSequenceItem
	{
		public DataSequenceItem(string name, string equation, T data, bool publishResult = true)
		{
			Name = name;
			Equation = equation;
			Data = data;
			PublishResult = publishResult;
		}

		public T Data { get; set; }

		public override SequenceItemResult GetResult(int order, IEquationResolver equationResolver, ref Dictionary<string, string> inputs, IDictionary<string, string> mappedInputs = null)
		{
			return base.GetResult(order, equationResolver, mappedInputs);
		}
	}
}
