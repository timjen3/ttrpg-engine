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
		Role Troy = new Role("Troy", new Dictionary<string, string>());
		Role Sebastian = new Role("Sebastian", new Dictionary<string, string>());

		public CombatDemo()
		{
			Troy.Attributes["HP"] = "12";
			Troy.Attributes["Str"] = "18";
			Troy.Attributes["Dex"] = "16";
			Troy.Attributes["Ac"] = "2";
			Troy.Attributes["Weapon.Hits"] = "2";
			Troy.Attributes["Weapon.Min"] = "1";
			Troy.Attributes["Weapon.Max"] = "6";

			Sebastian.Attributes["HP"] = "12";
			Sebastian.Attributes["Str"] = "18";
			Sebastian.Attributes["Dex"] = "16";
			Sebastian.Attributes["Ac"] = "2";
			Sebastian.Attributes["Weapon.Hits"] = "1";
			Sebastian.Attributes["Weapon.Min"] = "1";
			Sebastian.Attributes["Weapon.Max"] = "8";
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
					new MessageSequenceItem("Report Damage", "Dealt {damage} damage from {wHits} attacks."),
					new DieSequenceItem("Take Damage", "hp - damage", "newHp", false),
					new MessageSequenceItem("Report Damage Taken", "Took {damage} damage and now has {newHp} HP."),
					new DataSequenceItem<UpdateAttributeCommand>("Update Attribute", "newHp", updateHPCommand)
				},
				Conditions = new List<ICondition>
				{
					new Condition(itemName: "Damage", equation: "dodge < toHit"),
					new Condition(itemName: "Report Damage", dependentOnItem: "Damage"),
					new Condition(itemName: "Take Damage", dependentOnItem: "Damage", equation: "damage > 0"),
					new Condition(itemName: "Report Damage Taken", dependentOnItem: "Damage", equation: "damage > 0"),
					new Condition(itemName: "Update Attribute", dependentOnItem: "Damage", equation: "damage > 0"),
				},
				Mappings = new List<IMapping>
				{
					new RoleMapping("dex", "dex", "attacker", "To Hit"),
					new RoleMapping("dex", "dex", "defender", "Dodge"),
					new RoleMapping("Weapon.Hits", "wHits", "attacker", "Report Damage"),
					new RoleMapping("ac", "ac", "defender", "Damage"),
					new RoleMapping("Weapon.Hits", "wHits", "attacker", "Damage"),
					new RoleMapping("Weapon.Min", "wMin", "attacker", "Damage"),
					new RoleMapping("Weapon.Max", "wMax", "attacker", "Damage"),
					new RoleMapping("hp", "hp", "defender", "Take Damage")
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
			Role attacker = Troy;
			Role defender = Sebastian;
			while (int.Parse(Troy.Attributes["hp"]) > 0 && int.Parse(Sebastian.Attributes["hp"]) > 0)
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
						defender.Attributes[command.Data.Attribute] = itemResult.Result;
					}
				}
				Role swap = attacker;
				attacker = defender;
				defender = swap;
				Console.WriteLine("--------------------");
			}
			if (int.Parse(Troy.Attributes["hp"]) <= 0) Console.WriteLine($"{Troy.Name} was defeated!");
			if (int.Parse(Sebastian.Attributes["hp"]) <= 0) Console.WriteLine($"{Sebastian.Name} was defeated!");
		}
	}
}
