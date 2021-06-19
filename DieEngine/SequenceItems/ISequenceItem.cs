using DieEngine.Equations;
using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	public interface ISequenceItem
	{
		string Name { get; }

		string Equation { get; }

		/// <summary>
		///		Process equation and get result.
		/// </summary>
		/// <param name="equationResolver"></param>
		/// <param name="sharedInputs">Global shared inputs; may be modified to pass data to downstream items</param>
		/// <param name="mappedInputs">Inputs mapped for this specific item</param>
		/// <returns></returns>
		SequenceItemResult GetResult(IEquationResolver equationResolver, ref Dictionary<string, double> mappedInputs, IDictionary<string, double> sharedInputs);
	}
}
