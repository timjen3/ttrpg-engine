using System.Collections.Generic;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Equations
{
	/// <summary>
	///		Processes Sequences.
	/// </summary>
	public interface IEquationService
	{
		/// <summary>
		///		Check if a sequence can be resolved with the given arguments for the specified entity.
		/// </summary>
		/// <remarks>
		///		Sequence Conditions are used for validation.
		/// </remarks>
		/// <param name="sequence"></param>
		/// <param name="entity"></param>
		/// <param name="inputs"></param>
		/// <returns></returns>
		bool Check(Sequence sequence, Entity entity, IDictionary<string, string> inputs = null);

		/// <summary>
		///		Check if a Sequence can be processed with the given arguments.
		/// </summary>
		/// <remarks>
		///		Sequence Conditions are used for validation.
		/// </remarks>
		/// <param name="sequence"></param>
		/// <param name="inputs"></param>
		/// <param name="entities"></param>
		/// <returns></returns>
		bool Check(Sequence sequence, IDictionary<string, string> inputs = null, IEnumerable<Entity> entities = null);

		/// <summary>
		///		Process a single sequence item and get the result
		/// </summary>
		/// <param name="item">item to be processed</param>
		/// <param name="entity">(optional) inject entity's attributes</param>
		/// <param name="inputs">(optional) inject</param>
		/// <returns></returns>
		SequenceItemResult Process(SequenceItem item, Entity entity = null, IDictionary<string, string> inputs = null);

		/// <summary>
		///		Process a Sequence and get a SequenceResult.
		/// </summary>
		/// <param name="sequence"></param>
		/// <param name="inputs"></param>
		/// <param name="entities"></param>
		/// <returns></returns>
		SequenceResult Process(Sequence sequence, IDictionary<string, string> inputs = null, IEnumerable<Entity> entities = null);
	}
}
