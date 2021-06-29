//using TTRPG.Engine.Conditions;
//using TTRPG.Engine.Equations;
//using TTRPG.Engine.Mappings;
//using TTRPG.Engine.SequenceItems;
//using System;
//using System.Collections.Generic;
//using Microsoft.Extensions.DependencyInjection;

//namespace TTRPG.Engine.Demo.Demos
//{
//	public class UpdateAttributeCommand
//	{
//		public Role Entity { get; set; }
//		public string Attribute { get; set; }
//	}

//	public class CombatDemo
//	{
//		int RoundNumber = 0;
//		Role Player = new Role("Player", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
//		Role Computer = new Role("Bandit", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

//		Role Attacker;
//		Role Defender;

//		IEquationResolver Resolver;

//		Random Gen = new Random();

//		public CombatDemo()
//		{
//			var services = new ServiceCollection();
//			services.AddTTRPGEngineServices();
//			var provider = services.BuildServiceProvider();
//			Resolver = provider.GetRequiredService<IEquationResolver>();

//			Player.Attributes["MAX_HP"] = "20";
//			Player.Attributes["HP"] = "20";
//			Player.Attributes["Str"] = "18";
//			Player.Attributes["Dex"] = "18";
//			Player.Attributes["Ac"] = "2";
//			Player.Attributes["Con"] = "12";
//			Player.Attributes["Potions"] = "5";
//			Player.Attributes["Weapon.Hits"] = "1";
//			Player.Attributes["Weapon.Min"] = "1";
//			Player.Attributes["Weapon.Max"] = "6";

//			Computer.Attributes["MAX_HP"] = "50";
//			Computer.Attributes["HP"] = "50";
//			Computer.Attributes["Str"] = "18";
//			Computer.Attributes["Dex"] = "14";
//			Computer.Attributes["Ac"] = "2";
//			Computer.Attributes["Con"] = "20";
//			Computer.Attributes["Potions"] = "2";
//			Computer.Attributes["Weapon.Hits"] = "1";
//			Computer.Attributes["Weapon.Min"] = "1";
//			Computer.Attributes["Weapon.Max"] = "8";
//		}

//		#region Actions
//		SequenceResult UsePotion(Role target)
//		{
//			var updateHPCommand = new UpdateAttributeCommand
//			{
//				Attribute = "HP",
//				Entity = target
//			};
//			var updatePotionsCommand = new UpdateAttributeCommand
//			{
//				Attribute = "Potions",
//				Entity = target
//			};
//			var sequence = new Sequence()
//			{
//				Name = "Use Potion",
//				Items = new List<ISequenceItem>
//				{
//					new DieSequenceItem("Calculate Heal Amount", "min(max_hp - old_hp, random(1,3,12) + floor(con / 4))", "heal_amt"),
//					new DieSequenceItem("Apply Healing", "heal_amt + old_hp", "new_hp"),
//					new DieSequenceItem("Deduct Potion", "potions - 1", "new_potions"),
//					new MessageSequenceItem("Heal", "Healed {heal_amt}. HP : {old_hp} => {new_hp}"),
//					new MessageSequenceItem("Heal", "Used 1 potion. Potions: {potions} => {new_potions}"),
//					new DataSequenceItem<UpdateAttributeCommand>("Update Attribute", "new_hp", updateHPCommand),
//					new DataSequenceItem<UpdateAttributeCommand>("Update Attribute", "new_potions", updatePotionsCommand)
//				},
//				Conditions = new List<ICondition>
//				{
//					new Condition("Calculate Heal Amount", "old_hp != max_hp", throwOnFail: true, failureMessage: "Already at Max HP"),
//					new Condition("Calculate Heal Amount", "potions > 0", throwOnFail: true, failureMessage: "No potions left")
//				},
//				Mappings = new List<IMapping>
//				{
//					new RoleMapping("potions", "potions", "target"),
//					new RoleMapping("con", "con", "target"),
//					new RoleMapping("hp", "old_hp", "target"),
//					new RoleMapping("max_hp", "max_hp", "target"),
//				}
//			};
//			var roles = new List<Role>
//			{
//				target.CloneAs("target")
//			};

//			return sequence.Process(Resolver, null, roles);
//		}

//		SequenceResult Attack(Role attacker, Role defender)
//		{
//			var updateHPCommand = new UpdateAttributeCommand
//			{
//				Attribute = "HP",
//				Entity = defender
//			};
//			var sequence = new Sequence()
//			{
//				Name = "Attack",
//				Items = new List<ISequenceItem>
//				{
//					new DieSequenceItem("To Hit", "random(1,1,20) + dex", "toHit", false),
//					new DieSequenceItem("Dodge", "random(1,1,20) + dex", "dodge", false),
//					new MessageSequenceItem("Report Hit", "The attack lands! (To Hit: {toHit}, Dodge: {dodge})"),
//					new MessageSequenceItem("Report Miss", "Miss! (To Hit: {toHit}, Dodge: {dodge})"),
//					new DieSequenceItem("Damage", "max(random(wHits,wMin,wMax) - ac + floor(str / 4), 0)", "damage", false),
//					new MessageSequenceItem("Report Damage", "Dealt {damage} damage from {wHits} attacks."),
//					new DieSequenceItem("Take Damage", "hp - damage", "newHp", false),
//					new MessageSequenceItem("Report Damage Taken", "Took {damage} damage. HP : {oldHp} => {newHp}."),
//					new DataSequenceItem<UpdateAttributeCommand>("Update Attribute", "newHp", updateHPCommand)
//				},
//				Conditions = new List<ICondition>
//				{
//					new Condition(itemName: "Report Miss", equation: "dodge >= toHit"),
//					new Condition(itemNames: new string[] { "Report Hit", "Damage" }, equation: "dodge < toHit"),
//					new Condition(itemName: "Report Damage", dependentOnItem: "Damage"),
//					new Condition(itemNames: new string[]{ "Take Damage", "Report Damage Taken", "Update Attribute"}, dependentOnItem: "Damage", equation: "damage > 0")
//				},
//				Mappings = new List<IMapping>
//				{
//					new RoleMapping("dex", "dex", "attacker", "To Hit"),
//					new RoleMapping("dex", "dex", "defender", "Dodge"),
//					new RoleMapping("Weapon.Hits", "wHits", "attacker", "Report Damage"),
//					new RoleMapping("ac", "ac", "defender", "Damage"),
//					new RoleMapping("str", "str", "attacker", "Damage"),
//					new RoleMapping("Weapon.Hits", "wHits", "attacker", "Damage"),
//					new RoleMapping("Weapon.Min", "wMin", "attacker", "Damage"),
//					new RoleMapping("Weapon.Max", "wMax", "attacker", "Damage"),
//					new RoleMapping("hp", "hp", "defender", "Take Damage"),
//					new RoleMapping("hp", "oldHp", "defender", "Report Damage Taken")
//				}
//			};
//			var roles = new List<Role>
//			{
//				attacker.CloneAs("attacker"),
//				defender.CloneAs("defender")
//			};

//			return sequence.Process(Resolver, null, roles);
//		}
//		#endregion

//		#region Events
//		void CombatStart()
//		{
//			int playerInitiative = (int.Parse(Player.Attributes["Dex"]) / 4) + Gen.Next(1, 20);
//			int computerInitiative = (int.Parse(Computer.Attributes["Dex"]) / 4) + Gen.Next(1, 20);
//			if (playerInitiative > computerInitiative)
//			{
//				Attacker = Player;
//				Defender = Computer;
//			}
//			else
//			{
//				Attacker = Computer;
//				Defender = Player;
//			}
//		}

//		void RoundStart(Role current)
//		{
//			RoundNumber++;
//			if ((RoundNumber - 1) % 2 == 0)
//			{
//				Console.ForegroundColor = ConsoleColor.Blue;
//				Console.WriteLine($"---BEGIN ROUND {(RoundNumber / 2) + 1}---");
//			}
//			Console.ForegroundColor = ConsoleColor.Green;
//			Console.WriteLine($"[ {current.Name}'s turn! ]");
//			Console.ResetColor();
//			Console.WriteLine();
//		}

//		void TurnEnd()
//		{
//			Role swap = Attacker;
//			Attacker = Defender;
//			Defender = swap;
//			Console.WriteLine();
//			Console.WriteLine("--------------------");
//			Console.WriteLine();
//		}

//		void HandleResults(SequenceResult result, Role target)
//		{
//			foreach (var itemResult in result.Results)
//			{
//				if (itemResult.ResolvedItem is MessageSequenceItem)
//				{
//					Console.WriteLine(itemResult.Result);
//				}
//				if (itemResult.ResolvedItem is DataSequenceItem<UpdateAttributeCommand> command)
//				{
//					target.Attributes[command.Data.Attribute] = itemResult.Result;
//				}
//			}
//		}

//		void AiDecision()
//		{
//			// occassionally heal when wounded, otherwise attack
//			bool missingHalfHp = int.Parse(Attacker.Attributes["HP"]) < int.Parse(Attacker.Attributes["MAX_HP"]) / 2;
//			if (missingHalfHp && int.Parse(Attacker.Attributes["Potions"]) > 0 && Gen.Next(3) == 1)
//			{
//				var result = UsePotion(Attacker);
//				HandleResults(result, Attacker);
//			}
//			else
//			{
//				var result = Attack(Attacker, Defender);
//				HandleResults(result, Defender);
//			}
//		}

//		void PlayerDecision()
//		{
//			bool missingHp = int.Parse(Attacker.Attributes["HP"]) < int.Parse(Attacker.Attributes["MAX_HP"]);
//			if (missingHp && int.Parse(Attacker.Attributes["Potions"]) > 0)
//			{
//				Console.Write($"What do you do? HP is {Attacker.Attributes["HP"]} (1: Attack 2: Heal)\n");
//				while (true)
//				{
//					var keyPress = Console.ReadKey();
//					if (keyPress.KeyChar == '1')
//					{
//						Console.Write("\r");
//						var result = Attack(Attacker, Defender);
//						HandleResults(result, Defender);
//						break;
//					}
//					else if (keyPress.KeyChar == '2')
//					{
//						Console.Write("\r");
//						var result = UsePotion(Attacker);
//						HandleResults(result, Attacker);
//						break;
//					}
//					else
//					{
//						Console.Write("\r");
//						Console.WriteLine("Invalid Input.");
//					}
//				}
//			}
//			else
//			{
//				var result = Attack(Attacker, Defender);
//				HandleResults(result, Defender);
//			}
//		}

//		void ReportVictor()
//		{
//			if (int.Parse(Player.Attributes["hp"]) <= 0) Console.WriteLine($"{Player.Name} was defeated!");
//			if (int.Parse(Computer.Attributes["hp"]) <= 0) Console.WriteLine($"{Computer.Name} was defeated!");
//		}
//		#endregion

//		public void DoDemo()
//		{
//			CombatStart();
//			while (int.Parse(Player.Attributes["hp"]) > 0 && int.Parse(Computer.Attributes["hp"]) > 0)
//			{
//				RoundStart(Attacker);
//				if (Attacker == Player) PlayerDecision();
//				else AiDecision();
//				TurnEnd();
//				Console.ReadKey();
//				Console.Write("\r");
//			}
//			ReportVictor();
//		}
//	}
//}
