using DieEngine.Exceptions;
using DieEngine.SequencesItems;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture]
	[TestOf(typeof(Sequence))]
	public class SequenceTests
	{
		/// Test a single die roll
		[Test]
		public void OneDie_RollAll_HasDieResult()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1 + 1", "ar")
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Results[0].Result, Is.EqualTo(2));
		}

		/// Test multiple die rolls
		[Test]
		public void TwoDice_RollAll_HasDieResults()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1 + 1", "ar"),
					new DieSequenceItem("b", "1 + ar", "br"),
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Results[0].Result, Is.EqualTo(2));
			Assert.That(result.Results[1].Result, Is.EqualTo(3));
		}

		/// Test multiple die rolls where second condition is driven by a variable and succeeds
		[Test]
		public void TwoDiceSecondConditionalSucceeds_RollAll_Rolls()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1 + 1", "ar"),
					new DieSequenceItem("b", "1 + ar", "br"),
				},
				Conditions = new List<Condition>
				{
					new Condition("ar = 2", 1)
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Results[0].Result, Is.EqualTo(2));
			Assert.That(result.Results[1].Result, Is.EqualTo(3));
		}

		/// Test if condition is missing required vars it throws
		[Test]
		public void ConditionalMissingRequiredParams_ThrowsException()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1 + 1", "ar")
				},
				Conditions = new List<Condition>
				{
					new Condition("br = 2", 0)
				}
			};

			Assert.Throws<EquationInputArgumentException>(() => sequence.RollAll());
		}

		/// Test multiple die rolls where second condition is driven by a variable and fails
		[Test]
		public void TwoDiceSecondConditionalBasedOnResult_RollAll_ThrowsException()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
					new DieSequenceItem("b", "1 + ar", "br"),
				},
				Conditions = new List<Condition>
				{
					new Condition("ar = 2", 1, true)
				}
			};

			Assert.Throws<ConditionFailedException>(() => sequence.RollAll());
		}

		/// Test that inputs is a different instance for each die roll
		[Test]
		public void ConfirmInputDictionaryIsCopiedPerDieTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
					new DieSequenceItem("b", "1", "br"),
				}
			};

			var result = sequence.RollAll();

			Assert.IsNotNull(result.Results[0].Inputs);
			Assert.IsNotNull(result.Results[1].Inputs);
			Assert.That(result.Results[0].Inputs != result.Results[1].Inputs);
			Assert.That(result.Results[0].Inputs.Count, Is.EqualTo(0));
			Assert.That(result.Results[1].Inputs.Count, Is.EqualTo(1));
		}

		/// Test two conditions
		[Test]
		public void MultipleConditionsPassTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
				},
				Conditions = new List<Condition>
				{
					new Condition("a > 0", 0),
					new Condition("a < 2", 0)
				}
			};
			var inputs = new Dictionary<string, double>
			{
				{ "a", 1 }
			};

			var result = sequence.RollAll(inputs);

			Assert.That(result.Results[0].Result, Is.EqualTo(1));
		}

		/// Test two conditions
		[Test]
		public void MultipleConditionsOneFails_ThrowsException()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
				},
				Conditions = new List<Condition>
				{
					new Condition("a > 0", 0, true),
					new Condition("a < 1", 0, true)
				}
			};
			var inputs = new Dictionary<string, double>
			{
				{ "a", 1 }
			};

			Assert.Throws<ConditionFailedException>(() => sequence.RollAll(inputs));
		}

		/// Test that a die is skipped when it fails condition check but isn't supposed to throw
		[Test]
		public void ConditionFailsButDoesNotThrow_RollIsSkipped()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
				},
				Conditions = new List<Condition>
				{
					new Condition("0", 0, false)
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Results, Is.Empty);
		}

		/// Test that a die is skipped when it fails condition check but isn't supposed to throw, but the non-failing die is rolled
		[Test]
		public void MultipleDieOneConditionFailsButDoesNotThrow_OneRolledOneSkipped()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
					new DieSequenceItem("b", "2", "br"),
				},
				Conditions = new List<Condition>
				{
					new Condition("0", 0, false)
				}
			};

			var result = sequence.RollAll();

			Assert.That(result.Results, Has.Count.EqualTo(1));
			Assert.That(result.Results[0].Result, Is.EqualTo(2));
		}

		/// Test that a custom exception message is added when a condition check fails
		[Test]
		public void ConditionWithCustomExceptionMessageFails_ThrownWithCustomMessage()
		{
			string customExMessage = "Custom Exception Message";
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
				},
				Conditions = new List<Condition>
				{
					new Condition("0", 0, true, customExMessage)
				}
			};

			var ex = Assert.Throws<ConditionFailedException>(() => sequence.RollAll());
			Assert.That(ex.Message, Is.EqualTo(customExMessage));
		}

		/// Inputs are renamed per mappings
		[Test]
		public void MapInputToDifferentNameTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "im", "ar"),
				},
				Mappings = new List<Mapping>
				{
					new Mapping(0, new Dictionary<string, string>
					{
						{ "i", "im" }
					})
				}
			};
			var inputs = new Dictionary<string, double>
			{
				{ "i", 1 }
			};

			var results = sequence.RollAll(inputs);

			Assert.That(results.Results[0].Result, Is.EqualTo(1));
		}

		/// Results are renamed per mappings
		[Test]
		public void MapDieResultToDifferentNameForNextDieTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
					new DieSequenceItem("b", "arr", "br"),
				},
				Mappings = new List<Mapping>
				{
					new Mapping(1, new Dictionary<string, string>
					{
						{ "ar", "arr" }
					})
				}
			};

			var results = sequence.RollAll();

			Assert.That(results.Results[1].Result, Is.EqualTo(1));
		}

		/// The pre-mapped input name is kept after mapping
		[Test]
		public void OriginalNameIsNotLostWhenMappingNameTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
					new DieSequenceItem("b", "ar + arr", "br"),
				},
				Mappings = new List<Mapping>
				{
					new Mapping(1, new Dictionary<string, string>
					{
						{ "ar", "arr" }
					})
				}
			};

			var results = sequence.RollAll();

			Assert.That(results.Results[1].Result, Is.EqualTo(2));
		}

		/// Test 2 input mappings
		[Test]
		public void Map2DieResultsToDifferentNameForNextDieTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
					new DieSequenceItem("b", "1", "br"),
					new DieSequenceItem("c", "arr + brr", "cr"),
				},
				Mappings = new List<Mapping>
				{
					new Mapping(2, new Dictionary<string, string>
					{
						{ "ar", "arr" },
						{ "br", "brr" }
					})
				}
			};

			var results = sequence.RollAll();

			Assert.That(results.Results[2].Result, Is.EqualTo(2));
		}

		/// Test that mapped input is available to specified die only
		[Test]
		public void MapDieResultToDieAndLaterDieDoesNotSeeItTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar"),
					new DieSequenceItem("b", "arr", "br"),
					new DieSequenceItem("c", "arr", "cr"),
				},
				Mappings = new List<Mapping>
				{
					new Mapping(2, new Dictionary<string, string>
					{
						{ "ar", "arr" }
					})
				}
			};

			// die c throws an exception because it does not know about the mapped input
			Assert.Throws<EquationInputArgumentException>(() => sequence.RollAll());
		}

		/// Test inject mapped input variables into custom function
		[Test]
		[Repeat(100)]
		public void InjectMappedVarsIntoCustomDiceFunction_PerformsDieRoll()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "[dice:{minRoll},{maxRoll}]", "ar"),
				},
				Mappings = new List<Mapping>
				{
					new Mapping(0, new Dictionary<string, string>
					{
						{ "dieMin", "minRoll" },
						{ "dieMax", "maxRoll" },
					})
				}
			};
			var inputs = new Dictionary<string, double>
			{
				{ "dieMin", 1 },
				{ "dieMax", 6 }
			};

			var results = sequence.RollAll(inputs);

			Assert.That(results.Results[0].Result, Is.GreaterThanOrEqualTo(1));
			Assert.That(results.Results[0].Result, Is.LessThanOrEqualTo(6));
		}

		/// Test processing an action
		[Test]
		public void ActionSequenceItemTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new ActionSequenceItem<string>("a", "1", "some instruction"),
				}
			};

			var results = sequence.RollAll();

			Assert.That(results.Results[0].ResolvedItem, Is.TypeOf<ActionSequenceItem<string>>());
			Assert.That(results.Results[0].Result, Is.EqualTo(1));
		}
	}
}