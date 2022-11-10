using System;
using System.Collections.Generic;

namespace TTRPG.Engine
{
	/// an entity carries a set of attributes into equations
	public class Entity
	{
		private string _alias;

		/// entity parameterless constructor
		public Entity()
		{
			Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Categories = new List<string>();
			InventoryItems = new Dictionary<string, Entity>(StringComparer.OrdinalIgnoreCase);
			Bag = new List<Entity>();
		}

		/// entity constructor
		public Entity(string name, Dictionary<string, string> attributes = null, List<string> categories = null, Dictionary<string, Entity> inventoryItems = null, List<Entity> bag = null)
		{
			Name = name;
			if (attributes != null)
				Attributes = new Dictionary<string, string>(attributes, StringComparer.OrdinalIgnoreCase);
			else
				Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Categories = categories ?? new List<string>();
			if (inventoryItems != null)
				InventoryItems = new Dictionary<string, Entity>(inventoryItems, StringComparer.OrdinalIgnoreCase);
			else
				InventoryItems = new Dictionary<string, Entity>(StringComparer.OrdinalIgnoreCase);
			Bag = bag ?? new List<Entity>();
		}

		/// entity's name
		public string Name { get; set; }
		/// entity's attributes
		public Dictionary<string, string> Attributes { get; }
		/// categories that entity belongs to
		public List<string> Categories { get; }
		/// inventory items
		public Dictionary<string, Entity> InventoryItems { get; }
		/// loose inventory items
		public List<Entity> Bag { get; }
		/// on originals matches Name, on clones it differs
		public string Alias
		{
			get => _alias ?? Name;
			set => _alias = value;
		}

		/// creates a clone with a different name
		public Entity CloneAs(string alias = null)
		{
			var clonedAttributes = new Dictionary<string, string>(Attributes);
			var clonedCategories = new List<string>(Categories);
			var clonedInventoryItems = new Dictionary<string, Entity>(StringComparer.OrdinalIgnoreCase);
			foreach (var item in InventoryItems)
				clonedInventoryItems.Add(item.Key, item.Value.CloneAs());
			var clone = new Entity(Name, clonedAttributes, clonedCategories, clonedInventoryItems);
			if (alias != null)
				clone.Alias = alias;

			return clone;
		}

		/// Removes Alias
		public void RemoveAlias() => Alias = Name;
	}
}
