using DieEngine.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture]
	[TestOf(typeof(DieSequence))]
	public class DieSequenceTests
	{
		RollCondition AlwaysRoll = new RollCondition
		{
			Equation = "1"
		};
		
		/// Test a single die roll
		[Test]
		public void OneDie_RollAll_HasDieResult()
		{
			var sequence = new DieSequence()
			{
				Dice = new List<Die>
				{
					new Die("a", "1 + 1", "ar")
				},
				Conditions = new List<RollCondition>
				{
					AlwaysRoll,
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Rolls[0].Result, Is.EqualTo(2));
		}

		/// Test multiple die rolls
		[Test]
		public void TwoDice_RollAll_HasDieResults()
		{
			var sequence = new DieSequence()
			{
				Dice = new List<Die>
				{
					new Die("a", "1 + 1", "ar"),
					new Die("b", "1 + ar", "br"),
				},
				Conditions = new List<RollCondition>
				{
					AlwaysRoll,
					AlwaysRoll,
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Rolls[0].Result, Is.EqualTo(2));
			Assert.That(result.Rolls[1].Result, Is.EqualTo(3));
		}

		/// Test multiple die rolls where second condition is driven by a variable and succeeds
		[Test]
		public void TwoDiceSecondConditionalSucceeds_RollAll_Rolls()
		{
			var sequence = new DieSequence()
			{
				Dice = new List<Die>
				{
					new Die("a", "1 + 1", "ar"),
					new Die("b", "1 + ar", "br"),
				},
				Conditions = new List<RollCondition>
				{
					AlwaysRoll,
					new RollCondition("ar = 2")
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Rolls[0].Result, Is.EqualTo(2));
			Assert.That(result.Rolls[1].Result, Is.EqualTo(3));
		}

		/// Test if condition is missing required vars it throws
		[Test]
		public void ConditionalMissingRequiredParams_ThrowsException()
		{
			var sequence = new DieSequence()
			{
				Dice = new List<Die>
				{
					new Die("a", "1 + 1", "ar")
				},
				Conditions = new List<RollCondition>
				{
					new RollCondition("br = 2")
				}
			};

			Assert.Throws<ConditionInputArgumentException>(() => sequence.RollAll());
		}

		/// Test multiple die rolls where second condition is driven by a variable and fails
		[Test]
		public void TwoDiceSecondConditionalBasedOnResult_RollAll_ThrowsException()
		{
			var sequence = new DieSequence()
			{
				Dice = new List<Die>
				{
					new Die("a", "1", "ar"),
					new Die("b", "1 + ar", "br"),
				},
				Conditions = new List<RollCondition>
				{
					AlwaysRoll,
					new RollCondition("ar = 2")
				}
			};

			Assert.Throws<RollConditionFailedException>(() => sequence.RollAll());
		}

		/// Test that inputs is a different instance for each die roll
		[Test]
		public void ConfirmInputDictionaryIsCopiedPerDieTest()
		{
			var sequence = new DieSequence()
			{
				Dice = new List<Die>
				{
					new Die("a", "1", "ar"),
					new Die("b", "1", "br"),
				},
				Conditions = new List<RollCondition>
				{
					AlwaysRoll,
					AlwaysRoll,
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Rolls[0].Inputs != result.Rolls[1].Inputs);
			Assert.That(result.Rolls[0].Inputs.Count, Is.EqualTo(0));
			Assert.That(result.Rolls[1].Inputs.Count, Is.EqualTo(1));
		}
	}
}