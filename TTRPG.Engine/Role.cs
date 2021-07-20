using System;
using System.Collections.Generic;

namespace TTRPG.Engine
{
	/// a role carries a set of attributes into equations
	public class Role
	{
		private string _alias;

		/// role parameterless constructor
		public Role()
		{
			Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Categories = new List<string>();
			InventoryItems = new Dictionary<string, Role>(StringComparer.OrdinalIgnoreCase);
		}

		/// role constructor
		public Role(string name, Dictionary<string, string> attributes = null, List<string> categories = null, Dictionary<string, Role> inventoryItems = null)
		{
			Name = name;
			if (attributes != null) Attributes = new Dictionary<string, string>(attributes, StringComparer.OrdinalIgnoreCase);
			else Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Categories = categories ?? new List<string>();
			if (inventoryItems != null) InventoryItems = new Dictionary<string, Role>(inventoryItems, StringComparer.OrdinalIgnoreCase);
			else InventoryItems = new Dictionary<string, Role>(StringComparer.OrdinalIgnoreCase);
		}

		/// role's name
		public string Name { get; set; }
		/// role's attributes
		public Dictionary<string, string> Attributes { get; }
		/// categories that role belongs to
		public List<string> Categories { get; }
		/// inventory items
		public Dictionary<string, Role> InventoryItems { get; }
		/// on originals matches Name, on clones it differs
		public string Alias { get => _alias ?? Name; set { _alias = value; } }

		/// creates a clone with a different name
		public Role CloneAs(string alias = null)
		{
			var clonedAttributes = new Dictionary<string, string>(Attributes);
			var clonedCategories = new List<string>(Categories);
			var clonedInventoryItems = new Dictionary<string, Role>(StringComparer.OrdinalIgnoreCase);
			foreach (var item in InventoryItems)
				clonedInventoryItems.Add(item.Key, item.Value.CloneAs());
			var clone = new Role(Name, clonedAttributes, clonedCategories, clonedInventoryItems);
			if (alias != null) clone.Alias = alias;

			return clone;
		}
	}
}
