using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.CommandParsing.Processors;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.CommandParsing.Parsers
{
	public class EquationCommandParser : ICommandParser
	{
		private readonly IEquationService _service;
		private readonly GameObject _data;

		public EquationCommandParser(IEquationService service, GameObject data)
		{
			_service = service;
			_data = data;
		}

		public bool IsDefault => true;

		public bool CanProcess(string commandType)
		{
			// since this is the default processor it does not implement this method
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetExampleCommands()
		{
			return _data.Sequences
				.Select(s => s.Example);
		}

		public ITTRPGCommandProcessor GetProcessor(EngineCommand command)
		{
			var processor = new EquationProcessor(_service, _data, command);
			if (!processor.IsValid())
			{
				throw new Exception("Invalid command.");
			}

			return processor;
		}
	}
}
