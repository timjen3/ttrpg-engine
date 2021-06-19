using System.Collections.Generic;

namespace DieEngine.Mappings
{
	public interface IMapping
	{
		void Apply(int order, ref Dictionary<string, double> inputs, IEnumerable<Role> roles);
	}
}