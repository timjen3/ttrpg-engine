using System;
using System.Collections.Generic;
using TTRPG.Engine.Conditions;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Mappings;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo2.Engine
{
	public class CombatSequences
	{
		private readonly IEquationResolver _resolver;
		private readonly Action<string> _writeMessage;

		private void HandleResultItems(SequenceResult result)
		{
			foreach (var itemResult in result.Results)
			{
				if (itemResult.ResolvedItem.SequenceItemType == SequenceItemType.Message)
				{
					_writeMessage(itemResult.Result);
				}
			}
		}

		public CombatSequences(IEquationResolver resolver, Action<string> writeMessage)
		{
			_resolver = resolver;
			_writeMessage = writeMessage;
		}

		public static Sequence UsePotionSequence => new Sequence()
		{
			Name = "UsePotion",
			Items = new List<ISequenceItem>
				{
					new SequenceItem("Calculate Heal Amount", "min(max_hp - old_hp, random(1,3,12) + floor(con / 4))", "heal_amt", SequenceItemType.Algorithm),
					new SequenceItem("Apply Healing", "heal_amt + old_hp", "new_hp", SequenceItemType.Algorithm),
					new SequenceItem("Deduct Potion", "potions - 1", "new_potions", SequenceItemType.Algorithm),
					new SequenceItem("Heal", "{target_name} used 1 potion ({potions} => {new_potions})", "PotionMsg", SequenceItemType.Message),
					new SequenceItem("Heal", "{target_name} healed {heal_amt} HP. ({old_hp} => {new_hp})", "HealMsg", SequenceItemType.Message)
				},
			Conditions = new List<ICondition>
				{
					new Condition("old_hp != max_hp"),
					new Condition("potions > 0")
				},
			Mappings = new List<IMapping>
				{
					new Mapping("name", "target_name", roleName: "target"),
					new Mapping("potions", "potions", roleName: "target"),
					new Mapping("con", "con", roleName: "target"),
					new Mapping("hp", "old_hp", roleName: "target"),
					new Mapping("max_hp", "max_hp", roleName: "target"),
				}
		};

		public static Sequence AttackSequence => new Sequence()
		{
			Name = "Attack",
			Items = new List<ISequenceItem>
				{
					new SequenceItem("Intention", "{attacker_name} {wAction} at {defender_name}.", "intention", SequenceItemType.Message),
					new SequenceItem("To Hit", "random(1,1,20) + dex", "to_hit", SequenceItemType.Algorithm),
					new SequenceItem("Dodge", "random(1,1,20) + dex", "dodge", SequenceItemType.Algorithm),
					new SequenceItem("Report Hit", "The attack lands! ({to_hit} To Hit > {dodge} dodge)", "ToHitMsg", SequenceItemType.Message),
					new SequenceItem("Report Miss", "Miss! ({to_hit} To Hit <= {dodge} Dodge)", "ToHitMsg", SequenceItemType.Message),
					new SequenceItem("Damage", "max(random(wHits,wMin,wMax) - ac + floor(str / 4), 0)", "damage", SequenceItemType.Algorithm),
					new SequenceItem("Report Damage", "{attacker_name} dealt {damage} damage to {defender_name} from {wHits} attacks.", "DamageMsg", SequenceItemType.Message),
					new SequenceItem("Take Damage", "hp - damage", "new_hp", SequenceItemType.Algorithm),
					new SequenceItem("Report Damage Taken", "{defender_name} HP : {old_hp} => {new_hp}", "DamageTakenMsg", SequenceItemType.Message)
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
					new Mapping("name", "attacker_name", roleName: "attacker"),
					new Mapping("name", "defender_name", roleName: "defender"),
					new Mapping("dex", "dex", roleName: "attacker", itemName: "To Hit"),
					new Mapping("dex", "dex", roleName: "defender", itemName: "Dodge"),
					new Mapping("hits", "wHits", roleName: "weapon", itemName: "Report Damage"),
					new Mapping("ac", "ac", roleName: "defender", itemName: "Damage"),
					new Mapping("str", "str", roleName: "attacker", itemName: "Damage"),
					new Mapping("hits", "wHits", roleName: "weapon", itemName: "Damage"),
					new Mapping("min", "wMin", roleName: "weapon", itemName: "Damage"),
					new Mapping("max", "wMax", roleName: "weapon", itemName: "Damage"),
					new Mapping("action", "wAction", roleName: "weapon", itemName: "Intention"),
					new Mapping("hp", "hp", roleName: "defender", itemName: "Take Damage"),
					new Mapping("hp", "old_hp", roleName: "defender", itemName: "Report Damage Taken")
				}
		};

		public bool CheckUsePotion(Role target)
		{
			var sequence = UsePotionSequence;
			var roles = new List<Role>
			{
				target.CloneAs("target")
			};

			return sequence.Check(_resolver, null, roles);
		}

		public void UsePotion(Role target)
		{
			var sequence = UsePotionSequence;
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

		public bool CheckAttack(Role attacker, Role defender, Role weapon)
		{
			var sequence = AttackSequence;
			var roles = new List<Role>
			{
				attacker.CloneAs("attacker"),
				defender.CloneAs("defender"),
				weapon.CloneAs("weapon")
			};

			return sequence.Check(_resolver, null, roles);
		}

		public void Attack(Role attacker, Role defender, Role weapon)
		{
			var sequence = AttackSequence;
			var roles = new List<Role>
			{
				attacker.CloneAs("attacker"),
				defender.CloneAs("defender"),
				weapon.CloneAs("weapon")
			};

			var result = sequence.Process(_resolver, null, roles);
			HandleResultItems(result);

			// update attributes
			if (result.Output.ContainsKey("new_hp"))
				defender.Attributes["HP"] = result.Output["new_hp"];
		}
	}
}
