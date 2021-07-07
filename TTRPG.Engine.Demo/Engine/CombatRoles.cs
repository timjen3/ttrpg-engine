using System;
using System.Collections.Generic;

namespace TTRPG.Engine.Demo2.Engine
{
	public static class CombatRoles
	{
		public static Role Player => new Role("Player", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "Name", "Player" },
			{ "MAX_HP", "20" },
			{ "HP", "20" },
			{ "Str", "18" },
			{ "Dex", "18" },
			{ "Ac", "2" },
			{ "Con", "12" },
			{ "Potions", "5" }
		});

		public static Role PlayerWeapon => new Role("Sword", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "Name", "Sword" },
			{ "Action", "slices sword" },
			{ "Hits", "1" },
			{ "Min", "1" },
			{ "Max", "6" }
		});

		public static Role Computer => new Role("Bandit", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "Name", "Bandit" },
			{ "MAX_HP", "50" },
			{ "HP", "50" },
			{ "Str", "18" },
			{ "Dex", "14" },
			{ "Ac", "2" },
			{ "Con", "20" },
			{ "Potions", "2" }
		});

		public static Role ComputerWeapon => new Role("Crude Sword", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "Name", "Crude Sword" },
			{ "Action", "swings sword wildly" },
			{ "Hits", "1" },
			{ "Min", "1" },
			{ "Max", "8" }
		});
	}
}
