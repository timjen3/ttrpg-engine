using DieEngine.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace DieEngine.Mappings
{
	public class RoleMapping : IMapping
	{
		public RoleMapping() { }

		public RoleMapping(string from, string to, string roleName, int? order = null, bool throwOnFailure = false)
		{
			From = from;
			To = to;
			RoleName = roleName;
			Order = order;
			ThrowOnFailure = throwOnFailure;
		}

		public string From { get; set; }

		public string To { get; set; }

		public int? Order { get; set; }

		public string RoleName { get; set; }

		public bool ThrowOnFailure { get; set; }

		public void Apply(int order, ref Dictionary<string, double> inputs, IEnumerable<Role> roles)
		{
			if (!Order.HasValue || Order == order)
			{
				inputs[To] = 0;
				var role = roles.SingleOrDefault(x => x.Name.Equals(RoleName, System.StringComparison.OrdinalIgnoreCase));
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
