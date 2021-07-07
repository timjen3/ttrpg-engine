using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Equations.Extensions;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo.Engine
{
	public class CombatDemoService
	{
		Random Gen = new Random();

		private readonly Action<string> _writeMessage;
		private readonly IEquationResolver _resolver;

		/// output any messages found in results
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

		private void UsePotion(Role target)
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

		private void Attack(Role attacker, Role defender, Role weapon)
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

		/// occassionally heal when wounded, otherwise attack
		private void AiDecision()
		{
			bool missingHalfHp = int.Parse(Computer.Attributes["HP"]) < int.Parse(Computer.Attributes["MAX_HP"]) / 2;
			if (missingHalfHp && int.Parse(Computer.Attributes["Potions"]) > 0 && Gen.Next(3) == 1)
			{
				UsePotion(Computer);
			}
			else
			{
				Attack(Computer, Player, ComputerWeapon);
			}
		}

		Sequence UsePotionSequence;
		Sequence AttackSequence;
		Role PlayerWeapon;
		Role ComputerWeapon;
		Role Player;
		Role Computer;

		public CombatDemoService(Action<string> writeMessage)
		{
			_writeMessage = writeMessage;
			var func1 = new Function("random", new RandomFunctionExtension());
			var func2 = new Function("toss", new CoinTossFunctionExtension());
			var funcs = new Function[] { func1, func2 };
			_resolver = new EquationResolver(funcs);
			var loader = new CombatSequenceDataLoader();
			loader.Load();
			UsePotionSequence = loader.UsePotionSequence;
			AttackSequence = loader.AttackSequence;
			Player = loader.Player;
			PlayerWeapon = loader.PlayerWeapon;
			Computer = loader.Computer;
			ComputerWeapon = loader.ComputerWeapon;
		}


		public string PlayerPotions => Player.Attributes["Potions"];

		public string PlayerHPStatus => $"{Player.Attributes["HP"]} / {Player.Attributes["MAX_HP"]}";

		public string ComputerPotions => Computer.Attributes["Potions"];

		public string ComputerHPStatus => $"{Computer.Attributes["HP"]} / {Computer.Attributes["MAX_HP"]}";

		public bool CheckPlayerAttack()
		{
			var sequence = AttackSequence;
			var roles = new List<Role>
			{
				Player.CloneAs("attacker"),
				Computer.CloneAs("defender"),
				PlayerWeapon.CloneAs("weapon")
			};

			return sequence.Check(_resolver, null, roles);
		}

		public bool CheckPlayerUsePotion()
		{
			var sequence = UsePotionSequence;
			var roles = new List<Role>
			{
				Player.CloneAs("target")
			};

			return sequence.Check(_resolver, null, roles);
		}

		public void PlayerAttack()
		{
			Attack(Player, Computer, PlayerWeapon);
			_writeMessage("------------------");
			if (!(int.Parse(Computer.Attributes["HP"]) <= 0))
			{
				AiDecision();
				_writeMessage("------------------");
			}
		}

		public void PlayerUsePotion()
		{
			UsePotion(Player);
			_writeMessage("------------------");
			AiDecision();
			_writeMessage("------------------");
		}

		public bool IsGameOver()
			=> int.Parse(Player.Attributes["HP"]) <= 0
				|| int.Parse(Computer.Attributes["HP"]) <= 0;
	}
}