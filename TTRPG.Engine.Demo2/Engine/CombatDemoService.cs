using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

		public EquationParts GetEquationPartsFromCommand(string command)
		{
			var parts = new EquationParts();
			// get sequence
			var sequenceName = Regex.Match(command, @"^.*?(?=\s)");
			if (sequenceName.Success)
			{
				parts.Sequence = Data.Sequences.FirstOrDefault(x => x.Name.Equals(sequenceName.Value, StringComparison.OrdinalIgnoreCase));
			}
			// get roles
			var rolesText = Regex.Match(command, @"\[.+?\]");
			if (rolesText.Success)
			{
				parts.Roles = new List<Role>();
				var rolesTextParts = rolesText.Value.Replace("[", "").Replace("]", "").Split(",");
				foreach (var nextRolePart in rolesTextParts)
				{
					var from = nextRolePart.Split(":")[0];
					var to = nextRolePart.Split(":")[1];
					var role = Data.Roles.FirstOrDefault(x => x.Name.Equals(from, StringComparison.OrdinalIgnoreCase));
					if (role != null)
					{
						parts.Roles.Add(role.CloneAs(to));
					}
				}
			}

			return parts;
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