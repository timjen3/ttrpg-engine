using System;
using System.Collections.Generic;

namespace TTRPG.Engine.Demo.Demo
{
	public static class CombatRoles
	{
		public static Role Player => new Role("Player", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "MAX_HP", "20" },
			{ "HP", "20" },
			{ "Str", "18" },
			{ "Dex", "18" },
			{ "Ac", "2" },
			{ "Con", "12" },
			{ "Potions", "5" },
			{ "Weapon.Hits", "1" },
			{ "Weapon.Min", "1" },
			{ "Weapon.Max", "6" }
		});

		public static Role Computer => new Role("Bandit", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "MAX_HP", "50" },
			{ "HP", "50" },
			{ "Str", "18" },
			{ "Dex", "14" },
			{ "Ac", "2" },
			{ "Con", "20" },
			{ "Potions", "2" },
			{ "Weapon.Hits", "1" },
			{ "Weapon.Min", "1" },
			{ "Weapon.Max", "8" }
		});
	}
}
