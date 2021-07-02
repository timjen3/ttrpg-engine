using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTRPG.Engine.Conditions;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Mappings;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo.Demo
{
	public class CombatSequences
	{
		private readonly IEquationResolver _resolver;

		private void HandleResultItems(SequenceResult result)
		{
			foreach (var itemResult in result.Results)
			{
				if (itemResult.ResolvedItem is MessageSequenceItem)
				{
					Console.WriteLine(itemResult.Result);
				}
			}
		}

		public CombatSequences(IEquationResolver resolver)
		{
			_resolver = resolver;
		}

		public void UsePotion(Role target)
		{
			var sequence = new Sequence()
			{
				Name = "UsePotion",
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("Calculate Heal Amount", "min(max_hp - old_hp, random(1,3,12) + floor(con / 4))", "heal_amt"),
					new DieSequenceItem("Apply Healing", "heal_amt + old_hp", "new_hp"),
					new DieSequenceItem("Deduct Potion", "potions - 1", "new_potions"),
					new MessageSequenceItem("Heal", "Healed {heal_amt}. HP : {old_hp} => {new_hp}"),
					new MessageSequenceItem("Heal", "Used 1 potion. Potions: {potions} => {new_potions}")
				},
				Conditions = new List<ICondition>
				{
					new Condition("Calculate Heal Amount", "old_hp != max_hp", throwOnFail: true, failureMessage: "Already at Max HP"),
					new Condition("Calculate Heal Amount", "potions > 0", throwOnFail: true, failureMessage: "No potions left")
				},
				Mappings = new List<IMapping>
				{
					new RoleMapping("potions", "potions", "target"),
					new RoleMapping("con", "con", "target"),
					new RoleMapping("hp", "old_hp", "target"),
					new RoleMapping("max_hp", "max_hp", "target"),
				}
			};
			var roles = new List<Role>
			{
				target.CloneAs("target")
			};

			var result = sequence.Process(_resolver, null, roles);
			HandleResultItems(result);

			// update attributes
			if (result.Output.ContainsKey("new_hp"))
			{
				target.Attributes["HP"] = result.Output["new_hp"];
				target.Attributes["Potions"] = result.Output["new_potions"];
			}
		}

		public void Attack(Role attacker, Role defender)
		{
			var sequence = new Sequence()
			{
				Name = "Attack",
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("To Hit", "random(1,1,20) + dex", "to_hit"),
					new DieSequenceItem("Dodge", "random(1,1,20) + dex", "dodge"),
					new MessageSequenceItem("Report Hit", "The attack lands! (To Hit: {to_hit}, Dodge: {dodge})"),
					new MessageSequenceItem("Report Miss", "Miss! (To Hit: {to_hit}, Dodge: {dodge})"),
					new DieSequenceItem("Damage", "max(random(wHits,wMin,wMax) - ac + floor(str / 4), 0)", "damage"),
					new MessageSequenceItem("Report Damage", "Dealt {damage} damage from {wHits} attacks."),
					new DieSequenceItem("Take Damage", "hp - damage", "new_hp"),
					new MessageSequenceItem("Report Damage Taken", "Took {damage} damage. HP : {old_hp} => {new_hp}.")
				},
				Conditions = new List<ICondition>
				{
					new Condition(itemName: "Report Miss", equation: "dodge >= to_hit"),
					new Condition(itemNames: new string[] { "Report Hit", "Damage" }, equation: "dodge < to_hit"),
					new Condition(itemName: "Report Damage", dependentOnItem: "Damage"),
					new Condition(itemNames: new string[]{ "Take Damage", "Report Damage Taken", "Update Attribute"}, dependentOnItem: "Damage", equation: "damage > 0")
				},
				Mappings = new List<IMapping>
				{
					new RoleMapping("dex", "dex", "attacker", "To Hit"),
					new RoleMapping("dex", "dex", "defender", "Dodge"),
					new RoleMapping("Weapon.Hits", "wHits", "attacker", "Report Damage"),
					new RoleMapping("ac", "ac", "defender", "Damage"),
					new RoleMapping("str", "str", "attacker", "Damage"),
					new RoleMapping("Weapon.Hits", "wHits", "attacker", "Damage"),
					new RoleMapping("Weapon.Min", "wMin", "attacker", "Damage"),
					new RoleMapping("Weapon.Max", "wMax", "attacker", "Damage"),
					new RoleMapping("hp", "hp", "defender", "Take Damage"),
					new RoleMapping("hp", "old_hp", "defender", "Report Damage Taken")
				}
			};
			var roles = new List<Role>
			{
				attacker.CloneAs("attacker"),
				defender.CloneAs("defender")
			};

			var result = sequence.Process(_resolver, null, roles);
			HandleResultItems(result);

			// update attributes
			if (result.Output.ContainsKey("new_hp"))
				defender.Attributes["HP"] = result.Output["new_hp"];
		}
	}
}
