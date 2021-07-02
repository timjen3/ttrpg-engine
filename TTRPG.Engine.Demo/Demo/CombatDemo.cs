//using org.mariuszgromada.math.mxparser;
//using System;
//using TTRPG.Engine.Equations;
//using TTRPG.Engine.Equations.Extensions;
//using TTRPG.Engine.SequenceItems;

//namespace TTRPG.Engine.Demo.Demo
//{
//	public class UpdateAttributeCommand
//	{
//		public Role Entity { get; set; }
//		public string Attribute { get; set; }
//	}

//	public class CombatDemo
//	{
//		int RoundNumber = 0;
//		CombatSequences Sequences;

//		public CombatDemo()
//		{
//			var func1 = new Function("random", new RandomFunctionExtension());
//			var func2 = new Function("toss", new CoinTossFunctionExtension());
//			var funcs = new Function[] { func1, func2 };
//			var resolver = new EquationResolver(funcs);
//			Sequences = new CombatSequences(resolver);
//		}

//		Role Player = CombatRoles.Player;
//		Role Computer = CombatRoles.Computer;

//		Role Attacker;
//		Role Defender;

//		Random Gen = new Random();

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
//				var result = Sequences.UsePotion(Attacker);
//				HandleResults(result, Attacker);
//			}
//			else
//			{
//				var result = Sequences.Attack(Attacker, Defender);
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
//						var result = Sequences.Attack(Attacker, Defender);
//						HandleResults(result, Defender);
//						break;
//					}
//					else if (keyPress.KeyChar == '2')
//					{
//						Console.Write("\r");
//						var result = Sequences.UsePotion(Attacker);
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
//				var result = Sequences.Attack(Attacker, Defender);
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
