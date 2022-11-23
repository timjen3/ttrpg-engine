using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using TTRPG.Engine.Data.TtrpgDataLoaders;
using TTRPG.Engine.Roles.Attributes;

namespace TTRPG.Engine.Roles
{
	public class RoleIndex
	{
		private int _index = 0;

		public int Next() => Interlocked.Increment(ref _index);
	}

	public class Role
	{
		private ConcurrentDictionary<string, RoleIndex> _roleIndex = new ConcurrentDictionary<string, RoleIndex>(StringComparer.OrdinalIgnoreCase);

		public Role()
		{
			Attributes = new List<RoleAttributeDefinition>();
			Categories = new List<string>();
		}

		public string Name { get; set; }

		[JsonProperty(ItemConverterType = typeof(TTRPGAttributeTypeJsonConverter))]
		public List<RoleAttributeDefinition> Attributes { get; set; }

		public List<string> Categories { get; set; }

		/// Get next available name
		public string Next(string name = null)
		{
			name ??= Name;
			if (!_roleIndex.ContainsKey(name))
			{
				_roleIndex[name] = new RoleIndex();
			}
			var nextIndex = _roleIndex[name].Next();
			if (nextIndex == 1)
			{
				return name;
			}
			else
			{
				return $"{name}-{nextIndex}";
			}
		}
	}
}
