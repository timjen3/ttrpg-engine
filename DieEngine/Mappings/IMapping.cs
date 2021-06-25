using System.Collections.Generic;

namespace DieEngine.Mappings
{
	public interface IMapping
	{
		void Apply(int order, ref Dictionary<string, string> inputs, IEnumerable<Role> roles);
	}
}