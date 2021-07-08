using System.Collections.Generic;

namespace TTRPG.Engine.SequenceItems
{
	/// <summary>
	///		Result for one resolved SequenceItem
	/// </summary>
	public class SequenceItemResult
	{
		/// <summary>
		///		The order of the processed sequence item in sequence, considered as if all items were processed
		/// </summary>
		public int Order { get; set; }

		/// <summary>
		///		Snapshot of inputs used by this item (may vary from later items)
		/// </summary>
		public IDictionary<string, string> Inputs { get; set; }

		/// <summary>
		///		The item that was rolled
		/// </summary>
		public SequenceItem ResolvedItem { get; set; }

		/// <summary>
		///		What item resolved to
		/// </summary>
		public string Result { get; set; }
	}
}
