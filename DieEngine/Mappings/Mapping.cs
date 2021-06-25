using DieEngine.Exceptions;
using System;
using System.Collections.Generic;

namespace DieEngine.Mappings
{
	/// <summary>
	///		Renames input variables according to mappings before using them in conditions or sequence item equations.
	///		The inputs are always copied to a new dictionary before changes are made to isolate changes to equations and reduce side effects.
	/// </summary>
	public class Mapping : IMapping
	{
		public Mapping(){}

		public Mapping(string from, string to, string itemName = null, bool throwOnFailure = false)
		{
			From = from;
			To = to;
			ItemName = itemName;
			ThrowOnFailure = throwOnFailure;
		}

		// source property key
		public string From { get; set; }

		// destination property key
		public string To { get; set; }

		// sequence item to apply mapping to
		public string ItemName { get; set; }

		// whether to throw exception when From key is missing from inputs. If false, value will be set to 0
		public bool ThrowOnFailure { get; set; }

		public void Apply(string itemName, ref Dictionary<string, string> inputs, IEnumerable<Role> roles)
		{
			if (string.IsNullOrWhiteSpace(ItemName) || string.Equals(itemName, ItemName, StringComparison.OrdinalIgnoreCase))
			{
				if (inputs.ContainsKey(From))
				{
					inputs[To] = inputs[From];
					return;
				}
				if (ThrowOnFailure) throw new MappingFailedException($"Mapping failed due to missing key: '{From}'.");
				inputs[To] = "0";
			}
		}
	}
}
