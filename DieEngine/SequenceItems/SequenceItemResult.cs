using System.Collections.Generic;

namespace DieEngine.SequencesItems
{
	public class SequenceItemResult
	{
		/// <summary>
		///		Snapshot of inputs used by this item (may vary from later items)
		/// </summary>
		public IDictionary<string, double> Inputs { get; set; }

		/// <summary>
		///		The item that was rolled
		/// </summary>
		public ISequenceItem ResolvedItem { get; set; }

		/// <summary>
		///		What item resolved to
		/// </summary>
		public double Result { get; set; }
	}
}
