using System;
using System.Collections.Generic;

namespace TTRPG.Engine
{
	public class TTRPGEngineOptions
	{
		private Dictionary<string, string> _autoSequences = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		///		Commands to be ran automatically when a command is processed with the specified category
		/// </summary>
		public IDictionary<string, string> AutomaticCommands
		{
			get
			{
				return _autoSequences;
			}
			set
			{
				_autoSequences = new Dictionary<string, string>(value, StringComparer.OrdinalIgnoreCase);
			}
		}
	}
}
