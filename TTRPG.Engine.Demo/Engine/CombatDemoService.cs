using org.mariuszgromada.math.mxparser;
using System;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Equations.Extensions;

namespace TTRPG.Engine.Demo2.Engine
{
	public class CombatDemoService
	{
		CombatSequences Sequences;

		public Role Player = CombatRoles.Player;
		public Role Computer = CombatRoles.Computer;

		Random Gen = new Random();

		private Action<string> _writeMessage;

		public CombatDemoService(Action<string> writeMessage)
		{
			_writeMessage = writeMessage;
			var func1 = new Function("random", new RandomFunctionExtension());
			var func2 = new Function("toss", new CoinTossFunctionExtension());
			var funcs = new Function[] { func1, func2 };
			var resolver = new EquationResolver(funcs);
			Sequences = new CombatSequences(resolver, writeMessage);
		}

		void AiDecision()
		{
			// occassionally heal when wounded, otherwise attack
			bool missingHalfHp = int.Parse(Computer.Attributes["HP"]) < int.Parse(Computer.Attributes["MAX_HP"]) / 2;
			if (missingHalfHp && int.Parse(Computer.Attributes["Potions"]) > 0 && Gen.Next(3) == 1)
			{
				Sequences.UsePotion(Computer);
			}
			else
			{
				Sequences.Attack(Computer, Player);
			}
		}

		public void PlayerAttack()
		{
			Sequences.Attack(Player, Computer);
			_writeMessage("------------------");
			if (!(int.Parse(Computer.Attributes["hp"]) <= 0))
			{
				AiDecision();
				_writeMessage("------------------");
			}
		}

		public void PlayerUsePotion()
		{
			Sequences.UsePotion(Player);
			_writeMessage("------------------");
			AiDecision();
			_writeMessage("------------------");
		}

		public bool IsGameOver()
			=> int.Parse(Player.Attributes["hp"]) <= 0
				|| int.Parse(Computer.Attributes["hp"]) <= 0;
	}
}