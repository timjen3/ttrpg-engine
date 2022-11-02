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
		///		Check if a sequence can be resolved with the given arguments for the specified role.
		/// </summary>
		/// <remarks>
		///		Sequence Conditions are used for validation.
		/// </remarks>
		/// <param name="sequence"></param>
		/// <param name="role"></param>
		/// <param name="inputs"></param>
		/// <returns></returns>
		bool Check(Sequence sequence, Role role, IDictionary<string, string> inputs = null);

		/// <summary>
		///		Check if a Sequence can be processed with the given arguments.
		/// </summary>
		/// <remarks>
		///		Sequence Conditions are used for validation.
		/// </remarks>
		/// <param name="sequence"></param>
		/// <param name="inputs"></param>
		/// <param name="roles"></param>
		/// <returns></returns>
		bool Check(Sequence sequence, IDictionary<string, string> inputs = null, IEnumerable<Role> roles = null);

		/// <summary>
		///		Process a single sequence item and get the result
		/// </summary>
		/// <param name="item">item to be processed</param>
		/// <param name="role">(optional) inject role's attributes</param>
		/// <param name="inputs">(optional) inject</param>
		/// <returns></returns>
		SequenceItemResult Process(SequenceItem item, Role role = null, IDictionary<string, string> inputs = null);

		/// <summary>
		///		Process a Sequence and get a SequenceResult.
		/// </summary>
		/// <param name="sequence"></param>
		/// <param name="inputs"></param>
		/// <param name="roles"></param>
		/// <returns></returns>
		SequenceResult Process(Sequence sequence, IDictionary<string, string> inputs = null, IEnumerable<Role> roles = null);
	}
}
