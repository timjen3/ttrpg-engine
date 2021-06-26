using System.Collections.Generic;

namespace TTRPG.Engine.Mappings
{
	public interface IMapping
	{
		void Apply(string itemName, ref Dictionary<string, string> inputs, IEnumerable<Role> roles);
	}
}