using System.Collections.Generic;

namespace DieEngine
{
	public class DieRoll
	{
		/// <summary>
		///		Inputs used for die roll
		/// </summary>
		public IDictionary<string, double> Inputs { get; set; }

		/// <summary>
		///		The die that was rolled
		/// </summary>
		public Die RolledDie { get; set; }

		/// <summary>
		///		Ordered results
		/// </summary>
		public double Result { get; set; }
	}
}
