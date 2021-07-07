using TTRPG.Engine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TTRPG.Engine.Mappings
{
	/// <summary>
	///		Renames input variables according to mappings before using them in conditions or sequence item equations.
	///		The inputs are always copied to a new dictionary before changes are made to isolate changes to equations and reduce side effects.
	/// </summary>
	public class Mapping : IMapping
	{
		/// Returns source data depending on MappingType
		private IDictionary<string, string> GetSourceData(Dictionary<string, string> inputs, IEnumerable<Role> roles)
		{
			switch (MappingType)
			{
				case MappingType.Input:
					{
						return inputs;
					}
				case MappingType.Role:
					{
						var role = roles.SingleOrDefault(x => x.Name.Equals(RoleName, StringComparison.OrdinalIgnoreCase));
						if (ThrowOnFailure && role == null) throw new MissingRoleException($"Mapping failed due to missing role: '{RoleName}'.");

						return role.Attributes;
					}
				default:
					throw new ArgumentException($"Unknown Mapping configured: {MappingType}.");
			}
		}

		/// Parameterless constructor
		public Mapping(){}

		/// Constructor for a Role Mapping
		public Mapping(string from, string to, string roleName, string itemName = null, bool throwOnFailure = false)
		{
			From = from;
			To = to;
			RoleName = roleName;
			ThrowOnFailure = throwOnFailure;
			ItemName = itemName;
			MappingType = MappingType.Role;
		}

		/// Constructor for an Input Mapping
		public Mapping(string from, string to, string itemName = null, bool throwOnFailure = false)
		{
			From = from;
			To = to;
			ItemName = itemName;
			ThrowOnFailure = throwOnFailure;
			MappingType = MappingType.Input;
		}

		/// source property key
		public string From { get; set; }

		/// destination property key
		public string To { get; set; }

		/// sequence item to apply mapping to
		public string ItemName { get; set; }

		/// role to pull properties from
		public string RoleName { get; set; }

		/// type of mapping
		public MappingType MappingType { get; set; }

		/// whether to throw exception when From key is missing from inputs. If false, value will be set to 0
		public bool ThrowOnFailure { get; set; }

		/// Injects the specified mapping into <param name="inputs"/> from the configured source determined by <see cref="MappingType"/>
		/// <param name="itemName">The sequence item name mapping is being applied for. If the mapping does not apply to that item nothing will happen.</param>
		/// <param name="roles">All roles available to the sequence.</param>
		public void Apply(string itemName, ref Dictionary<string, string> inputs, IEnumerable<Role> roles)
		{
			if (string.IsNullOrWhiteSpace(ItemName) || string.Equals(itemName, ItemName, StringComparison.OrdinalIgnoreCase))
			{
				inputs[To] = "0";
				var source = GetSourceData(inputs, roles);
				if (source.ContainsKey(From))
				{
					inputs[To] = source[From];
				}
				if (ThrowOnFailure) throw new MappingFailedException($"Mapping failed due to missing key: '{From}'.");
			}
		}
	}
}
