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
		public GameObject Data { get; }

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
					var role = Data.Roles.Single(x => x.Name == itemResult.Role.Name);
					role.Attributes[itemResult.Name] = itemResult.Result;
				}
			}
		}

		private void UsePotion(Role target)
		{
			var results = _equationService.Process(Data.UsePotionSequence, null, new Role[] { target });
			HandleResultItems(results);
		}

		private void Attack(Role attacker, Role defender)
		{
			var sequence = Data.AttackSequence;
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
			var liveTargets = Data.Targets.Where(x => !_equationService.Check(Data.CheckIsDead, x));
			foreach (var target in liveTargets)
			{
				if (_equationService.Check(Data.MissingHalfHP, target)
					&& _equationService.Check(Data.UsePotionSequence, target) && Gen.Next(3) == 1)
				{
					UsePotion(target);
				}
				else
				{
					Attack(target, Data.Player);
				}
			}
		}

		public CombatDemoService(Action<string> writeMessage, IEquationService equationService, GameObject gameObject)
		{
			_writeMessage = writeMessage;
			_equationService = equationService;
			Data = gameObject;
		}

		public string PlayerPotions => _equationService.Process(Data.Potions, Data.Player).Result;

		public string PlayerHPStatus => _equationService.Process(Data.HitPoints, Data.Player).Result;

		public string ComputerPotions => _equationService.Process(Data.Potions, Data.Target).Result;

		public string ComputerHPStatus => _equationService.Process(Data.HitPoints, Data.Target).Result;

		public bool CheckPlayerAttack()
		{
			var sequence = Data.AttackSequence;
			var roles = new List<Role>
			{
				Data.Player.CloneAs("attacker"),
				Data.Target.CloneAs("defender")
			};

			return _equationService.Check(sequence, null, roles);
		}

		public bool CheckPlayerUsePotion() => _equationService.Check(Data.UsePotionSequence, Data.Player);

		public void SetTarget(string name)
		{
			Data.SetTarget(name);
		}

		public IEnumerable<string> ListTargetNames() => Data.Targets.Select(x => x.Name);

		public void PlayerAttack()
		{
			Attack(Data.Player, Data.Target);
			AiDecision();
		}

		public void PlayerUsePotion()
		{
			UsePotion(Data.Player);
			AiDecision();
		}

		public bool IsGameOver() => _equationService.Check(Data.CheckIsDead, Data.Player)
										|| Data.Targets.All(x => _equationService.Check(Data.CheckIsDead, x));
	}
}