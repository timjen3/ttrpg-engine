using TTRPG.Engine.Equations;
using System.Collections.Generic;

namespace TTRPG.Engine.SequenceItems
{
	public interface ISequenceItem
	{
		string Name { get; }

		/// <summary>
		///		Process equation and get result.
		/// </summary>
		/// <param name="order">Order of this item in sequence</param>
		/// <param name="equationResolver"></param>
		/// <param name="sharedInputs">Global shared inputs; may be modified to pass data to downstream items</param>
		/// <param name="mappedInputs">Inputs mapped for this specific item</param>
		/// <returns></returns>
		SequenceItemResult GetResult(int order, IEquationResolver equationResolver, ref Dictionary<string, string> mappedInputs, IDictionary<string, string> sharedInputs);
	}
}
