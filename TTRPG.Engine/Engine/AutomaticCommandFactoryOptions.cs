using System.Collections.Generic;

namespace TTRPG.Engine.Engine
{
	public class AutomaticCommandFactoryOptions
	{
		private List<AutomaticCommand> _autoSequences = new List<AutomaticCommand>();

		/// <summary>
		///		Commands to be ran automatically
		/// </summary>
		public IList<AutomaticCommand> AutomaticCommands
		{
			get => _autoSequences;
			set => _autoSequences = new List<AutomaticCommand>(value);
		}
	}
}
