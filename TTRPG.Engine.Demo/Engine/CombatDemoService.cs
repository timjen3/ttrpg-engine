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
		GameObject Repo;

		/// output any messages found in results
		private void HandleResultItems(SequenceResult result)
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
					var role = Repo.Roles.Single(x => x.Name == itemResult.Role.Name);
					role.Attributes[itemResult.Name] = itemResult.Result;
				}
			}
		}

		private void UsePotion(Role target)
		{
			var results = _equationService.Process(Repo.UsePotionSequence, null, new Role[] { target });
			HandleResultItems(results);
		}

		private void Attack(Role attacker, Role defender)
		{
			var sequence = Repo.AttackSequence;
			var roles = new List<Role>
			{
				attacker.CloneAs("attacker"),
				defender.CloneAs("defender")
			};

			var result = _equationService.Process(sequence, null, roles);
			HandleResultItems(result);
		}

		/// occassionally heal when wounded, otherwise attack
		private void AiDecision()
		{
			var liveTargets = Repo.Targets.Where(x => !_equationService.Check(Repo.CheckIsDead, x));
			foreach (var target in liveTargets)
			{
				if (_equationService.Check(Repo.MissingHalfHP, target)
					&& _equationService.Check(Repo.UsePotionSequence, target) && Gen.Next(3) == 1)
				{
					UsePotion(target);
				}
				else
				{
					Attack(target, Repo.Player);
				}
			}
		}

		public CombatDemoService(Action<string> writeMessage, IEquationService equationService, CombatSequenceDataLoader loader)
		{
			_writeMessage = writeMessage;
			_equationService = equationService;
			_loader = loader;
			Repo = _loader.Load();
		}

		public string PlayerPotions => _equationService.Process(Repo.Potions, Repo.Player).Result;

		public string PlayerHPStatus => _equationService.Process(Repo.HitPoints, Repo.Player).Result;

		public string ComputerPotions => _equationService.Process(Repo.Potions, Repo.Target).Result;

		public string ComputerHPStatus => _equationService.Process(Repo.HitPoints, Repo.Target).Result;

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
				Repo.Target.CloneAs("defender")
			};

			return _equationService.Check(sequence, null, roles);
		}

		public bool CheckPlayerUsePotion() => _equationService.Check(Repo.UsePotionSequence, Repo.Player);

		public void SetTarget(string name)
		{
			Repo.SetTarget(name);
		}

		public IEnumerable<string> ListTargetNames() => Repo.Targets.Select(x => x.Name);

		public void PlayerAttack()
		{
			Attack(Repo.Player, Repo.Target);
			AiDecision();
		}

		public void PlayerUsePotion()
		{
			UsePotion(Repo.Player);
			AiDecision();
		}

		public bool IsGameOver() => _equationService.Check(Repo.CheckIsDead, Repo.Player)
										|| Repo.Targets.All(x => _equationService.Check(Repo.CheckIsDead, x));
	}
}