using DieEngine.Conditions;
using DieEngine.Equations;
using DieEngine.Mappings;
using DieEngine.SequencesItems;
using System;
using System.Collections.Generic;

namespace DieEngine.Demo.Demos
{
	public class UpdateAttributeCommand
	{
		public Role Entity { get; set; }
		public string Attribute { get; set; }
	}

	public class CombatDemo
	{
		EquationResolver Resolver = new EquationResolver();
		Role Hero = new Role("Troy", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
		Role Bandit = new Role("Sebastian", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

		public CombatDemo()
		{
			Hero.Attributes["HP"] = "12";
			Hero.Attributes["Str"] = "18";
			Hero.Attributes["Dex"] = "16";
			Hero.Attributes["Ac"] = "2";
			Hero.Attributes["Weapon.Hits"] = "2";
			Hero.Attributes["Weapon.Min"] = "1";
			Hero.Attributes["Weapon.Max"] = "6";

			Bandit.Attributes["HP"] = "8";
			Bandit.Attributes["Str"] = "12";
			Bandit.Attributes["Dex"] = "16";
			Bandit.Attributes["Ac"] = "2";
			Bandit.Attributes["Weapon.Hits"] = "1";
			Bandit.Attributes["Weapon.Min"] = "1";
			Bandit.Attributes["Weapon.Max"] = "6";
		}

		public SequenceResult Attack(Role attacker, Role defender)
		{
			var updateHPCommand = new UpdateAttributeCommand
			{
				Attribute = "HP",
				Entity = defender
			};
			var sequence = new Sequence()
			{
				Name = "Weapon",
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("To Hit", "random(1,1,20) + dex", "toHit", false),
					new DieSequenceItem("Dodge", "random(1,1,20) + dex", "dodge", false),
					new MessageSequenceItem("Report To Hit", "To Hit of {toHit} vs Dodge of {dodge}."),
					new DieSequenceItem("Damage", "max(random(wHits,wMin,wMax) - ac, 0)", "damage", false),
					new MessageSequenceItem("Report Damage", "Dealt {damage} damage."),
					new DieSequenceItem("TakeDamage", "hp - damage", "newHp", false),
					new MessageSequenceItem("Report Damage Taken", "Took {damage} damage and now has {newHp} HP."),
					new DataSequenceItem<UpdateAttributeCommand>("Update Attribute", "newHp", updateHPCommand)
				},
				Conditions = new List<ICondition>
				{
					new Condition(itemName: "Damage", equation: "dodge < toHit"),
					new Condition(itemName: "Report Damage", dependentOnItem: "Damage"),
					new Condition(itemName: "TakeDamage", dependentOnItem: "Damage", equation: "damage > 0"),
					new Condition(itemName: "Report Damage Taken", dependentOnItem: "Damage", equation: "damage > 0"),
					new Condition(itemName: "Update Attribute", dependentOnItem: "Damage", equation: "damage > 0"),
				},
				Mappings = new List<IMapping>
				{
					new RoleMapping("dex", "dex", "attacker", 0),
					new RoleMapping("dex", "dex", "defender", 1),
					new RoleMapping("ac", "ac", "defender", 3),
					new RoleMapping("Weapon.Hits", "wHits", "attacker", 3),
					new RoleMapping("Weapon.Min", "wMin", "attacker", 3),
					new RoleMapping("Weapon.Max", "wMax", "attacker", 3),
					new RoleMapping("hp", "hp", "defender", 5)
				}
			};
			var roles = new List<Role>
			{
				attacker.CloneAs("attacker"),
				defender.CloneAs("defender")
			};

			return sequence.Process(Resolver, null, roles);
		}

		public void DoDemo()
		{
			Role attacker = Hero;
			Role defender = Bandit;
			while (int.Parse(Hero.Attributes["hp"]) > 0 && int.Parse(Bandit.Attributes["hp"]) > 0)
			{
				Console.WriteLine($"  {attacker.Name}'s turn!");
				var result = Attack(attacker, defender);
				foreach (var itemResult in result.Results)
				{
					if (itemResult.ResolvedItem is MessageSequenceItem)
					{
						Console.WriteLine(itemResult.Result);
					}
					if (itemResult.ResolvedItem is DataSequenceItem<UpdateAttributeCommand> command)
					{
						Bandit.Attributes[command.Data.Attribute] = itemResult.Result;
					}
				}
				Role swap = attacker;
				attacker = defender;
				defender = swap;
				Console.WriteLine("--------------------");
			}
			if (int.Parse(Hero.Attributes["hp"]) <= 0) Console.WriteLine("Hero was defeated!");
			if (int.Parse(Bandit.Attributes["hp"]) <= 0) Console.WriteLine("Bandit was defeated!");
		}
	}
}
