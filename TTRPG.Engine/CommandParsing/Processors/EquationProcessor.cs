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
        private readonly ParsedCommand _command;

        public EquationProcessor(IEquationService service, GameObject data, ParsedCommand command)
        {
            _service = service;
            _data = data;
            _sequence = _data.Sequences
                .FirstOrDefault(x => x.Name.Equals(command.MainCommand, StringComparison.OrdinalIgnoreCase));
            _command = command;
        }

        public bool IsValid() => _sequence != null && _command.Roles != null && _command.Roles.Count > 0;

        public ProcessedCommand Process()
        {
            var result = _service.Process(_sequence, _command.Inputs, _command.Roles);

            // update attributes
            foreach (var itemResult in result.ResultItems)
            {
                if (itemResult.Category.StartsWith("UpdateAttribute", StringComparison.OrdinalIgnoreCase))
                {
                    var role = _data.Roles.Single(x => x.Name == itemResult.Role.Name);
                    var attributeToUpdate = itemResult.FormatMessage ?? itemResult.Name;
                    role.Attributes[attributeToUpdate] = itemResult.Result;
                }
            }
            var messages = result.Results
                .Where(x => x.ResolvedItem.SequenceItemEquationType == SequenceItems.SequenceItemEquationType.Message)
                .Select(x => x.Result);

            return new ProcessedCommand
            {
                Source = _command,
                CommandCategories = _sequence.Categories,
                Messages = messages.ToList()
            };
        }
    }
}
