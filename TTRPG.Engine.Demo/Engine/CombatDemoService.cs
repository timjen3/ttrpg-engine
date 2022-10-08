using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Demo.Engine.CommandParsing;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo.Engine
{
	public class CombatDemoService
	{
		private readonly Action<string> _writeMessage;
		public GameObject Data { get; }

		/// output any messages found in results
		public CombatDemoService(Action<string> writeMessage, GameObject gameObject)
		{
			_writeMessage = writeMessage;
			Data = gameObject;
		}

		public IEnumerable<string> ListTargetNames(string category) => Data.GetLiveTargets(category).Select(x => x.Name);
		
		public EquationParts GetEquationPartsFromCommand(CommandParser parser)
		{
			return new EquationParts(parser, Data);
		}

		public InventoryParts GetInventoryPartsFromCommand(CommandParser parser)
		{
			return new InventoryParts(parser);
		}

		public CommandParser ParseMainCommand(string command)
		{
			return new CommandParser(command, Data);
		}

		public void HandleResultItems(SequenceResult result)
		{
			foreach (var itemResult in result.Results)
			{
				if (itemResult.ResolvedItem.SequenceItemEquationType == SequenceItemEquationType.Message)
				{
					_writeMessage(itemResult.Result);
				}
			}
			_writeMessage("------------------");
			foreach (var itemResult in result.ResultItems)
			{
				if (itemResult.Category.StartsWith("UpdateAttribute", StringComparison.OrdinalIgnoreCase))
				{
					var role = Data.Roles.Single(x => x.Name == itemResult.Role.Name);
					var attributeToUpdate = itemResult.FormatMessage ?? itemResult.Name;
					role.Attributes[attributeToUpdate] = itemResult.Result;
				}
			}
		}
	}
}