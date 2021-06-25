using DieEngine.Conditions;
using DieEngine.Equations;
using DieEngine.Exceptions;
using DieEngine.Mappings;
using DieEngine.SequencesItems;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture(Category="Integration")]
	[TestOf(typeof(Sequence))]
	public class SequenceTests
	{
		IEquationResolver EquationResolver;

		[OneTimeSetUp]
		public void SetupTests()
		{
			var services = new ServiceCollection();
			services.AddDieEngineServices();
			var provider = services.BuildServiceProvider();
			EquationResolver = provider.GetRequiredService<IEquationResolver>();
		}

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

			var result = sequence.Process(EquationResolver);

			Assert.That(result.Results[0].Result, Is.EqualTo("2"));
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

			var result = sequence.Process(EquationResolver);

			Assert.That(result.Results[0].Result, Is.EqualTo("2"));
			Assert.That(result.Results[1].Result, Is.EqualTo("3"));
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
				Conditions = new List<ICondition>
				{
					new Condition("b", "ar = 2")
				}
			};

			var result = sequence.Process(EquationResolver);

			Assert.That(result.Results[0].Result, Is.EqualTo("2"));
			Assert.That(result.Results[1].Result, Is.EqualTo("3"));
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
				Conditions = new List<ICondition>
				{
					new Condition("a", "br = 2")
				}
			};

			Assert.Throws<EquationInputArgumentException>(() => sequence.Process(EquationResolver));
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
				Conditions = new List<ICondition>
				{
					new Condition("b", "ar = 2", throwOnFail: true)
				}
			};

			Assert.Throws<ConditionFailedException>(() => sequence.Process(EquationResolver));
		}

		/// Test that inputs are different instances for each die roll
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

			var result = sequence.Process(EquationResolver);

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
				Conditions = new List<ICondition>
				{
					new Condition("a", "a > 0"),
					new Condition("a", "a < 2")
				}
			};
			var inputs = new Dictionary<string, string>
			{
				{ "a", "1" }
			};

			var result = sequence.Process(EquationResolver, inputs);

			Assert.That(result.Results[0].Result, Is.EqualTo("1"));
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
				Conditions = new List<ICondition>
				{
					new Condition("a", "a > 0", throwOnFail: true),
					new Condition("a", "a < 1", throwOnFail: true)
				}
			};
			var inputs = new Dictionary<string, string>
			{
				{ "a", "1" }
			};

			Assert.Throws<ConditionFailedException>(() => sequence.Process(EquationResolver, inputs));
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
				Conditions = new List<ICondition>
				{
					new Condition("a", "0")
				}
			};

			var result = sequence.Process(EquationResolver);

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
				Conditions = new List<ICondition>
				{
					new Condition("a", "0")
				}
			};

			var result = sequence.Process(EquationResolver);

			Assert.That(result.Results, Has.Count.EqualTo(1));
			Assert.That(result.Results[0].Result, Is.EqualTo("2"));
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
				Conditions = new List<ICondition>
				{
					new Condition("a", "0", throwOnFail: true, failureMessage: customExMessage)
				}
			};

			var ex = Assert.Throws<ConditionFailedException>(() => sequence.Process(EquationResolver));
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
				Mappings = new List<IMapping>
				{
					new Mapping("i", "im", "a")
				}
			};
			var inputs = new Dictionary<string, string>
			{
				{ "i", "1" }
			};

			var results = sequence.Process(EquationResolver, inputs);

			Assert.That(results.Results[0].Result, Is.EqualTo("1"));
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
				Mappings = new List<IMapping>
				{
					new Mapping("ar", "arr", "b")
				}
			};

			var results = sequence.Process(EquationResolver);

			Assert.That(results.Results[1].Result, Is.EqualTo("1"));
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
				Mappings = new List<IMapping>
				{
					new Mapping("ar", "arr", "b")
				}
			};

			var results = sequence.Process(EquationResolver);

			Assert.That(results.Results[1].Result, Is.EqualTo("2"));
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
				Mappings = new List<IMapping>
				{
					new Mapping("ar", "arr", "c"),
					new Mapping("br", "brr", "c"),
				}
			};

			var results = sequence.Process(EquationResolver);

			Assert.That(results.Results[2].Result, Is.EqualTo("2"));
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
				Mappings = new List<IMapping>
				{
					new Mapping("ar", "arr", "c")
				}
			};

			// die c throws an exception because it does not know about the mapped input
			Assert.Throws<EquationInputArgumentException>(() => sequence.Process(EquationResolver));
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
					new DieSequenceItem("a", "random(1,minValue,maxValue)", "ar"),
				},
				Mappings = new List<IMapping>
				{
					new Mapping("minRange", "minValue", "a"),
					new Mapping("maxRange", "maxValue", "a")
				}
			};
			var inputs = new Dictionary<string, string>
			{
				{ "minRange", "1" },
				{ "maxRange", "6" }
			};

			var results = sequence.Process(EquationResolver, inputs);

			Assert.That(results.Results[0].Result, Is.GreaterThanOrEqualTo("1"));
			Assert.That(results.Results[0].Result, Is.LessThanOrEqualTo("6"));
		}

		/// Test that the result contains the custom data
		[Test]
		public void DataSequenceItemSequencedTest()
		{
			string customData = "some instruction";
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DataSequenceItem<string>("a", "1", customData),
				}
			};

			var results = sequence.Process(EquationResolver);

			Assert.That(results.Results[0].ResolvedItem, Is.TypeOf<DataSequenceItem<string>>());
			Assert.That(((DataSequenceItem<string>)results.Results[0].ResolvedItem).Data, Is.EqualTo(customData));
			Assert.That(results.Results[0].Result, Is.EqualTo("1"));
		}

		/// Test that a mapping with no item specified applies to all items
		[Test]
		public void UnorderedMappingTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "a", "ar"),
					new DieSequenceItem("b", "a", "br"),
				},
				Mappings = new List<IMapping>
				{
					new Mapping("b", "a")
				}
			};

			var results = sequence.Process(EquationResolver, new Dictionary<string, string>
			{
				{ "b", "1" }
			});

			Assert.That(results.Results[0].Result, Is.EqualTo("1"));
			Assert.That(results.Results[1].Result, Is.EqualTo("1"));
		}

		/// Test that a role's attribute is mapped properly and injected into the equation
		[Test]
		public void RoleSequenceItemTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "aa", "ar"),
				},
				Mappings = new List<IMapping>
				{
					new RoleMapping("a", "aa", "r1", "a")
				}
			};
			var roles = new List<Role>
			{
				new Role("r1", new Dictionary<string, string>
				{
					{ "a", "1" }
				})
			};

			var results = sequence.Process(EquationResolver, roles: roles);

			Assert.That(results.Results[0].Result, Is.EqualTo("1"));
		}

		/// Test that a item's result is missing when publishresult is false
		[Test]
		public void UnpublishedResultTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<ISequenceItem>
				{
					new DieSequenceItem("a", "1", "ar", false),
				},
				Mappings = new List<IMapping>
				{
				}
			};

			var results = sequence.Process(EquationResolver);

			Assert.IsEmpty(results.Results);
		}
	}
}