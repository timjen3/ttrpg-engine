using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Equations;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Demo.Engine
{
	public class CombatDemoService
	{
		Random Gen = new Random();

		private readonly Action<string> _writeMessage;
		private readonly IEquationService _equationService;
		private readonly CombatSequenceDataLoader _loader;

		/// output any messages found in results
		private void HandleResultItems(SequenceResult result, IEnumerable<Role> originalRoles)
		{
			foreach (var itemResult in result.Results)
			{
				if (itemResult.ResolvedItem.SequenceItemEquationType == SequenceItemEquationType.Message)
				{
					_writeMessage(itemResult.Result);
				}
			}
			_writeMessage("------------------");
			foreach (var itemResult in result.ResultItems)
			{
				if (itemResult.Category == "UpdateAttribute")
				{
					var role = originalRoles.Single(x => x.Name == itemResult.Role.Name);
					role.Attributes[itemResult.Name] = itemResult.Result;
				}
			}
		}

		private void UsePotion(Role target)
		{
			var sequence = Repo.UsePotionSequence;
			var roles = new List<Role>
			{
				target.CloneAs("target")
			};

			var result = _equationService.Process(sequence, null, roles);
			HandleResultItems(result, new Role[] { target });
		}

		private void Attack(Role attacker, Role defender, Role weapon)
		{
			var sequence = Repo.AttackSequence;
			var roles = new List<Role>
			{
				attacker.CloneAs("attacker"),
				defender.CloneAs("defender"),
				weapon.CloneAs("weapon")
			};

			var result = _equationService.Process(sequence, null, roles);
			HandleResultItems(result, new Role[] { attacker, defender });
		}

		/// occassionally heal when wounded, otherwise attack
		private void AiDecision()
		{
			var liveTargets = Repo.Targets.Where(x => int.Parse(x.Attributes["HP"]) > 0);
			foreach (var target in liveTargets)
			{
				bool missingHalfHp = int.Parse(target.Attributes["HP"]) < int.Parse(target.Attributes["MAX_HP"]) / 2;
				if (missingHalfHp && int.Parse(target.Attributes["Potions"]) > 0 && Gen.Next(3) == 1)
				{
					UsePotion(target);
				}
				else
				{
					Attack(target, Repo.Player, Repo.ComputerWeapon);
				}
			}
		}

		GameObject Repo;

		public CombatDemoService(Action<string> writeMessage, IEquationService equationService, CombatSequenceDataLoader loader)
		{
			_writeMessage = writeMessage;
			_equationService = equationService;
			_loader = loader;
			Repo = _loader.Load();
		}

		public string PlayerPotions => Repo.Player.Attributes["Potions"];

		public string PlayerHPStatus => $"{Repo.Player.Attributes["HP"]} / {Repo.Player.Attributes["MAX_HP"]}";

		public string ComputerPotions => Repo.Target.Attributes["Potions"];

		public string ComputerHPStatus => $"{Repo.Target.Attributes["HP"]} / {Repo.Target.Attributes["MAX_HP"]}";

		public void NewGame()
		{
			Repo = _loader.Load();
		}

		public bool CheckPlayerAttack()
		{
			var sequence = Repo.AttackSequence;
			var roles = new List<Role>
			{
				Repo.Player.CloneAs("attacker"),
				Repo.Target.CloneAs("defender"),
				Repo.PlayerWeapon.CloneAs("weapon")
			};

			return _equationService.Check(sequence, null, roles);
		}

		public bool CheckPlayerUsePotion()
		{
			var sequence = Repo.UsePotionSequence;
			var roles = new List<Role>
			{
				Repo.Player.CloneAs("target")
			};

			return _equationService.Check(sequence, null, roles);
		}

		public void SetTarget(string name)
		{
			Repo.SetTarget(name);
		}

		public IEnumerable<string> ListTargetNames()
		{
			return Repo.Targets.Select(x => x.Name);
		}

		public void PlayerAttack()
		{
			Attack(Repo.Player, Repo.Target, Repo.PlayerWeapon);
			AiDecision();
		}

		public void PlayerUsePotion()
		{
			UsePotion(Repo.Player);
			AiDecision();
		}

		public bool IsGameOver()
			=> int.Parse(Repo.Player.Attributes["HP"]) <= 0
				|| Repo.Targets.All(x => int.Parse(x.Attributes["HP"]) <= 0);
	}
}