using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Equations;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo.Engine.CommandParsing
{
	public class EquationProcessor : ITTRPGCommandProcessor
	{
		public Sequence Sequence { get; }
		public Dictionary<string, string> Inputs { get; }
		public List<Role> Roles { get; }
		public IEquationService Service { get; }

		private void HandleResultItems(SequenceResult result, GameObject data, Action<string> writeMessage)
		{
			foreach (var itemResult in result.Results)
			{
				if (itemResult.ResolvedItem.SequenceItemEquationType == SequenceItemEquationType.Message)
				{
					writeMessage(itemResult.Result);
				}
			}
			writeMessage("------------------");
			foreach (var itemResult in result.ResultItems)
			{
				if (itemResult.Category.StartsWith("UpdateAttribute", StringComparison.OrdinalIgnoreCase))
				{
					var role = data.Roles.Single(x => x.Name == itemResult.Role.Name);
					var attributeToUpdate = itemResult.FormatMessage ?? itemResult.Name;
					role.Attributes[attributeToUpdate] = itemResult.Result;
				}
			}
		}

		public EquationProcessor(IEquationService service, string command, List<Role> roles, Dictionary<string, string> inputs, GameObject data)
		{
			Service = service;
			Sequence = data.Sequences.FirstOrDefault(x => x.Name.Equals(command, StringComparison.OrdinalIgnoreCase));
			Roles = roles;
			Inputs = inputs;
		}

		public bool IsValid() => Sequence != null && Roles != null && Roles.Count > 0;

		public void Process(Action<string> writeMessage, GameObject data)
		{
			try
			{
				var results = Service.Process(Sequence, Inputs, Roles);
				HandleResultItems(results, data, writeMessage);
			}
			catch (Exception ex)
			{
				writeMessage($"Failed to process command.'\n");
				writeMessage(ex.Message);
				writeMessage(ex.StackTrace);
			}
		}
	}
}
