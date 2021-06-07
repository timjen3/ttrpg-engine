using DieEngine.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture]
	[TestOf(typeof(DieSequence))]
	public class DieSequenceTests
	{
		/// Test a single die roll
		[Test]
		public void OneDie_RollAll_HasDieResult()
		{
			var sequence = new DieSequence()
			{
				Dice = new List<Die>
				{
					new Die("a", "1 + 1", "ar")
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
					new RollCondition("ar = 2", 1)
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
					new RollCondition("br = 2", 0)
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
					new RollCondition("ar = 2", 1)
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
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Rolls[0].Inputs != result.Rolls[1].Inputs);
			Assert.That(result.Rolls[0].Inputs.Count, Is.EqualTo(0));
			Assert.That(result.Rolls[1].Inputs.Count, Is.EqualTo(1));
		}

		/// Test two conditions
		[Test]
		public void MultipleConditionsPassTest()
		{
			var sequence = new DieSequence()
			{
				Dice = new List<Die>
				{
					new Die("a", "1", "ar"),
				},
				Conditions = new List<RollCondition>
				{
					new RollCondition("a > 0", 0),
					new RollCondition("a < 2", 0)
				}
			};
			var inputs = new Dictionary<string, double>
			{
				{ "a", 1 }
			};

			var result = sequence.RollAll(inputs);

			Assert.That(result.Rolls[0].Result, Is.EqualTo(1));
		}

		/// Test two conditions
		[Test]
		public void MultipleConditionsOneFails_ThrowsException()
		{
			var sequence = new DieSequence()
			{
				Dice = new List<Die>
				{
					new Die("a", "1", "ar"),
				},
				Conditions = new List<RollCondition>
				{
					new RollCondition("a > 0", 0),
					new RollCondition("a < 1", 0)
				}
			};
			var inputs = new Dictionary<string, double>
			{
				{ "a", 1 }
			};

			Assert.Throws<RollConditionFailedException>(() => sequence.RollAll(inputs));
		}
	}
}