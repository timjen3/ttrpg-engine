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
		public Mapping() { }

		/// Constructor for an Entity Mapping
		public Mapping(string from, string to, string entityName, string itemName = null, bool throwOnFailure = false)
		{
			From = from;
			To = to;
			EntityName = entityName;
			ThrowOnFailure = throwOnFailure;
			ItemName = itemName;
			MappingType = MappingType.Entity;
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

		/// Constructor for an InventoryItem Mapping
		public Mapping(string from, string to, string entityName, string inventoryItemName, string itemName = null, bool throwOnFailure = false)
		{
			if (inventoryItemName == null)
				throw new ArgumentNullException($"nameof{inventoryItemName} cannot be null for an inventory item mapping.");
			From = from;
			To = to;
			EntityName = entityName;
			ThrowOnFailure = throwOnFailure;
			ItemName = itemName;
			InventoryItemName = inventoryItemName;
			MappingType = MappingType.InventoryItem;
		}

		/// source property key
		public string From { get; set; }

		/// destination property key
		public string To { get; set; }

		/// sequence item to apply mapping to
		public string ItemName { get; set; }

		/// entity to pull properties from
		public string EntityName { get; set; }

		/// inventory item to pull properties from
		public string InventoryItemName { get; set; }

		/// type of mapping
		public MappingType MappingType
		{
			get => _mappingType;
			set
			{
				if (!Enum.IsDefined(typeof(MappingType), value))
					throw new ArgumentException($"{value} is an invalid value for Mapping property '{nameof(MappingType)}'.");
				_mappingType = value;
			}
		}

		/// whether to throw exception when From key is missing from inputs. If false, value will be set to 0
		public bool ThrowOnFailure { get; set; }
	}
}
