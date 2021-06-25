using System.Collections.Generic;

namespace DieEngine.Mappings
{
	public interface IMapping
	{
		void Apply(string itemName, ref Dictionary<string, string> inputs, IEnumerable<Role> roles);
	}
}