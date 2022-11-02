using System.Collections.Generic;

namespace TTRPG.Engine.Engine
{
	public class TTRPGEngineOptions
	{
		private List<AutomaticCommand> _autoSequences = new List<AutomaticCommand>();

		/// <summary>
		///		Commands to be ran automatically
		/// </summary>
		public IList<AutomaticCommand> AutomaticCommands
		{
			get
			{
				return _autoSequences;
			}
			set
			{
				_autoSequences = new List<AutomaticCommand>(value);
			}
		}
	}
}
