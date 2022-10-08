using System;

namespace TTRPG.Engine.Demo.Engine.CommandParsing
{
	public interface ITTRPGCommandProcessor
	{
		/// <summary>
		///		Processes command and returns result.
		/// </summary>
		/// <param name="writeMessage"></param>
		public void Process(Action<string> writeMessage, GameObject data);

		/// <summary>
		///		Validates the command.
		/// </summary>
		/// <returns></returns>
		public bool IsValid();
	}
}
