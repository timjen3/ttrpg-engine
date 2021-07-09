using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
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
		private readonly EquationService _equationService;

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

			var result = _equationService.Process(sequence, null, roles);
			HandleResultItems(result);
			_writeMessage("------------------");

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

			var result = _equationService.Process(sequence, null, roles);
			HandleResultItems(result);
			_writeMessage("------------------");

			// update attributes
			if (result.Output.ContainsKey("new_hp"))
				defender.Attributes["HP"] = result.Output["new_hp"];
		}

		/// occassionally heal when wounded, otherwise attack
		private void AiDecision()
		{
			var liveTargets = Targets.Where(x => int.Parse(x.Attributes["HP"]) > 0);
			foreach (var target in liveTargets)
			{
				bool missingHalfHp = int.Parse(target.Attributes["HP"]) < int.Parse(target.Attributes["MAX_HP"]) / 2;
				if (missingHalfHp && int.Parse(target.Attributes["Potions"]) > 0 && Gen.Next(3) == 1)
				{
					UsePotion(target);
				}
				else
				{
					Attack(target, Player, ComputerWeapon);
				}
			}
		}

		Sequence UsePotionSequence;
		Sequence AttackSequence;
		Role PlayerWeapon;
		Role ComputerWeapon;
		Role Player;
		Role Computer;
		List<Role> Targets;

		public CombatDemoService(Action<string> writeMessage)
		{
			_writeMessage = writeMessage;
			var func1 = new Function("random", new RandomFunctionExtension());
			var func2 = new Function("toss", new CoinTossFunctionExtension());
			var funcs = new Function[] { func1, func2 };
			var resolver = new EquationResolver(funcs);
			_equationService = new EquationService(resolver);
			var loader = new CombatSequenceDataLoader();
			loader.Load();
			UsePotionSequence = loader.UsePotionSequence;
			AttackSequence = loader.AttackSequence;
			Player = loader.Player;
			PlayerWeapon = loader.PlayerWeapon;
			Targets = loader.Targets;
			ComputerWeapon = loader.ComputerWeapon;
			Computer = Targets.FirstOrDefault();
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

			return _equationService.Check(sequence, null, roles);
		}

		public bool CheckPlayerUsePotion()
		{
			var sequence = UsePotionSequence;
			var roles = new List<Role>
			{
				Player.CloneAs("target")
			};

			return _equationService.Check(sequence, null, roles);
		}

		public void SetTarget(string name)
		{
			Computer = Targets.FirstOrDefault(x => x.Name == name);
		}

		public IEnumerable<string> ListTargetNames()
		{
			return Targets.Select(x => x.Name);
		}

		public void PlayerAttack()
		{
			Attack(Player, Computer, PlayerWeapon);
			AiDecision();
		}

		public void PlayerUsePotion()
		{
			UsePotion(Player);
			AiDecision();
		}

		public bool IsGameOver()
			=> int.Parse(Player.Attributes["HP"]) <= 0
				|| Targets.All(x => int.Parse(x.Attributes["HP"]) <= 0);
	}
}