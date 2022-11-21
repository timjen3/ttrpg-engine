using System;
using System.Linq;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.CommandParsing.Processors
{
	public class EquationProcessor : ITTRPGCommandProcessor
	{
		private readonly IEquationService _service;
		private readonly Sequence _sequence;
		private readonly GameObject _data;
		private readonly EngineCommand _command;

		public EquationProcessor(IEquationService service, GameObject data, EngineCommand command)
		{
			_service = service;
			_data = data;
			_sequence = _data.Sequences
				.FirstOrDefault(x => x.Name.Equals(command.MainCommand, StringComparison.OrdinalIgnoreCase));
			_command = command;
		}

		public bool IsValid() => _sequence != null && _command.Entities != null && _command.Entities.Count > 0;

		public ProcessedCommand Process()
		{
			var result = _service.Process(_sequence, _command.Inputs, _command.Entities);
			var processed = new ProcessedCommand
			{
				Source = _command,
				CommandCategories = _sequence.Categories,
				CategoryParams = _sequence.CategoryParams,
				Completed = result.Completed,
				Events = result.Events
			};

			return processed;
		}
	}
}
