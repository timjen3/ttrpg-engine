using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using TTRPG.Engine.Data.TtrpgDataLoaders;
using TTRPG.Engine.Roles.Attributes;

namespace TTRPG.Engine.Roles
{
	public class Role
	{
		private static int _index;

		public Role(int startingIndex = 0)
		{
			_index = startingIndex;
			Attributes = new List<RoleAttributeDefinition>();
			DerivedAttributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			Categories = new List<string>();
		}

		public string Name { get; set; }

		[JsonProperty(ItemConverterType = typeof(TTRPGAttributeTypeJsonConverter))]
		public List<RoleAttributeDefinition> Attributes { get; set; }

		public Dictionary<string, string> DerivedAttributes { get; set; }

		public List<string> Categories { get; set; }

		/// Get next available name
		public string Next() => $"{Name}-{Interlocked.Increment(ref _index)}";
	}
}
