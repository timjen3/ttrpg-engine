using DieEngine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine.Mappings
{
	public class RoleMapping : IMapping
	{
		public RoleMapping() { }

		public RoleMapping(string from, string to, string roleName, string itemName = null, bool throwOnFailure = false)
		{
			From = from;
			To = to;
			RoleName = roleName;
			ItemName = itemName;
			ThrowOnFailure = throwOnFailure;
		}

		public string From { get; set; }

		public string To { get; set; }

		public string ItemName { get; set; }

		public string RoleName { get; set; }

		public bool ThrowOnFailure { get; set; }

		public void Apply(string itemName, ref Dictionary<string, string> inputs, IEnumerable<Role> roles)
		{
			if (string.IsNullOrWhiteSpace(ItemName) || string.Equals(itemName, ItemName, StringComparison.OrdinalIgnoreCase))
			{
				inputs[To] = "0";
				var role = roles.SingleOrDefault(x => x.Name.Equals(RoleName, StringComparison.OrdinalIgnoreCase));
				if (ThrowOnFailure && role == null) throw new MissingRoleException($"Mapping failed due to missing role: '{RoleName}'.");
				else if (ThrowOnFailure && role != null && !role.Attributes.ContainsKey(From)) throw new MappingFailedException($"Mapping failed due to missing key: '{From}'.");
				if (role != null && role.Attributes.ContainsKey(From))
				{
					inputs[To] = role.Attributes[From];
				}
			}
		}
	}
}
