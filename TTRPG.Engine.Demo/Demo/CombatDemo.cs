using EasyConsole;
using org.mariuszgromada.math.mxparser;
using System;
using System.Threading;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Equations.Extensions;

namespace TTRPG.Engine.Demo.Demo
{
	public class CombatDemo
	{
		CombatSequences Sequences;

		Role Player = CombatRoles.Player;
		Role Computer = CombatRoles.Computer;

		Role Attacker;
		Role Defender;

		Random Gen = new Random();

		bool PlayerIsDead => int.Parse(Player.Attributes["hp"]) <= 0;
		bool ComputerIsDead => int.Parse(Computer.Attributes["hp"]) <= 0;

		public CombatDemo()
		{
			var func1 = new Function("random", new RandomFunctionExtension());
			var func2 = new Function("toss", new CoinTossFunctionExtension());
			var funcs = new Function[] { func1, func2 };
			var resolver = new EquationResolver(funcs);
			Sequences = new CombatSequences(resolver);
			Attacker = Computer;
			Defender = Player;
		}

		void TurnEnd()
		{
			Role swap = Attacker;
			Attacker = Defender;
			Defender = swap;
			Console.WriteLine();
			Console.WriteLine("--------------------");
			Console.WriteLine();
			Thread.Sleep(2000);
			Console.Clear();
		}

		void AiDecision()
		{
			// occassionally heal when wounded, otherwise attack
			bool missingHalfHp = int.Parse(Attacker.Attributes["HP"]) < int.Parse(Attacker.Attributes["MAX_HP"]) / 2;
			if (missingHalfHp && int.Parse(Attacker.Attributes["Potions"]) > 0 && Gen.Next(3) == 1)
			{
				Sequences.UsePotion(Attacker);
			}
			else
			{
				Console.WriteLine($"{Attacker.Name} swings his blade wildly.");
				Sequences.Attack(Attacker, Defender);
			}
		}

		void PlayerDecision()
		{
			Output.WriteLine("Your turn!");
			Output.WriteLine($"{Player.Attributes["HP"]} / {Player.Attributes["MAX_HP"]} HP");
			var menu = new Menu()
				.Add("Attack", () => Sequences.Attack(Attacker, Defender))
				.Add("Use Potion", () => Sequences.UsePotion(Attacker));
			menu.Display();
		}

		void ReportVictor()
		{
			if (PlayerIsDead) Console.WriteLine($"{Player.Name} was defeated!");
			if (ComputerIsDead) Console.WriteLine($"{Computer.Name} was defeated!");
		}

		public void DoDemo()
		{
			while (!PlayerIsDead && !ComputerIsDead)
			{
				Output.WriteLine(ConsoleColor.Green, $"[ {Attacker.Name}'s turn! ]");
				if (Attacker == Player) PlayerDecision();
				else AiDecision();
				TurnEnd();
			}
			ReportVictor();
		}
	}
}