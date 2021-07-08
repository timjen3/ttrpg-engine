using System;

namespace TTRPG.Engine
{
	/// <summary>
	///		Renames input variables according to mappings before using them in conditions or sequence item equations.
	///		The inputs are always copied to a new dictionary before changes are made to isolate changes to equations and reduce side effects.
	/// </summary>
	public class Mapping
	{
		private MappingType _mappingType;

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
		public MappingType MappingType
		{
			get => _mappingType;
			set
			{
				if (!Enum.IsDefined(typeof(MappingType), value)) throw new ArgumentException($"{value} is an invalid value for Mapping property '{nameof(MappingType)}'.");
				_mappingType = value;
			}
		}

		/// whether to throw exception when From key is missing from inputs. If false, value will be set to 0
		public bool ThrowOnFailure { get; set; }
	}
}
