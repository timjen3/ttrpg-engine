using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category="Integration")]
	[TestOf(typeof(Sequence))]
	public class SequenceTests
	{
		EquationService EquationService;

		[OneTimeSetUp]
		public void SetupTests()
		{
			var services = new ServiceCollection();
			services.AddTTRPGEngineServices();
			var provider = services.BuildServiceProvider();
			EquationService = provider.GetRequiredService<EquationService>();
		}

		/// Test a single die roll
		[Test]
		public void OneDie_RollAll_HasDieResult()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1 + 1", "ar", SequenceItemType.Algorithm)
				}
			};

			var result = EquationService.Process(sequence);

			Assert.That(result.Results[0].Result, Is.EqualTo("2"));
		}

		/// Test multiple die rolls
		[Test]
		public void TwoDice_RollAll_HasDieResults()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1 + 1", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "1 + ar", "br", SequenceItemType.Algorithm),
				}
			};

			var result = EquationService.Process(sequence);

			Assert.That(result.Results[0].Result, Is.EqualTo("2"));
			Assert.That(result.Results[1].Result, Is.EqualTo("3"));
		}

		/// Test multiple die rolls where second condition is driven by a variable and succeeds
		[Test]
		public void TwoDiceSecondConditionalSucceeds_RollAll_Rolls()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1 + 1", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "1 + ar", "br", SequenceItemType.Algorithm),
				},
				Conditions = new List<Condition>
				{
					new Condition("b", "ar = 2")
				}
			};

			var result = EquationService.Process(sequence);

			Assert.That(result.Results[0].Result, Is.EqualTo("2"));
			Assert.That(result.Results[1].Result, Is.EqualTo("3"));
		}

		/// Test if condition is missing required vars it throws
		[Test]
		public void ConditionalMissingRequiredParams_ThrowsException()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1 + 1", "ar", SequenceItemType.Algorithm)
				},
				Conditions = new List<Condition>
				{
					new Condition("a", "br = 2")
				}
			};

			Assert.Throws<EquationInputArgumentException>(() => EquationService.Process(sequence));
		}

		/// Test multiple die rolls where second condition is driven by a variable and fails
		[Test]
		public void TwoDiceSecondConditionalBasedOnResult_RollAll_ThrowsException()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "1 + ar", "br", SequenceItemType.Algorithm),
				},
				Conditions = new List<Condition>
				{
					new Condition("b", "ar = 2", throwOnFail: true)
				}
			};

			Assert.Throws<ConditionFailedException>(() => EquationService.Process(sequence));
		}

		/// Test that inputs are different instances for each die roll
		[Test]
		public void ConfirmInputDictionaryIsCopiedPerDieTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "1", "br", SequenceItemType.Algorithm),
				}
			};

			var result = EquationService.Process(sequence);

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
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
				},
				Conditions = new List<Condition>
				{
					new Condition("a", "a > 0"),
					new Condition("a", "a < 2")
				}
			};
			var inputs = new Dictionary<string, string>
			{
				{ "a", "1" }
			};

			var result = EquationService.Process(sequence, inputs);

			Assert.That(result.Results[0].Result, Is.EqualTo("1"));
		}

		/// Test two conditions
		[Test]
		public void MultipleConditionsOneFails_ThrowsException()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
				},
				Conditions = new List<Condition>
				{
					new Condition("a", "a > 0", throwOnFail: true),
					new Condition("a", "a < 1", throwOnFail: true)
				}
			};
			var inputs = new Dictionary<string, string>
			{
				{ "a", "1" }
			};

			Assert.Throws<ConditionFailedException>(() => EquationService.Process(sequence, inputs));
		}

		/// Test that a die is skipped when it fails condition check but isn't supposed to throw
		[Test]
		public void ConditionFailsButDoesNotThrow_RollIsSkipped()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
				},
				Conditions = new List<Condition>
				{
					new Condition("a", "0")
				}
			};

			var result = EquationService.Process(sequence);

			Assert.That(result.Results, Is.Empty);
		}

		/// Test that a die is skipped when it fails condition check but isn't supposed to throw, but the non-failing die is rolled
		[Test]
		public void MultipleDieOneConditionFailsButDoesNotThrow_OneRolledOneSkipped()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "2", "br", SequenceItemType.Algorithm),
				},
				Conditions = new List<Condition>
				{
					new Condition("a", "0")
				}
			};

			var result = EquationService.Process(sequence);

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
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
				},
				Conditions = new List<Condition>
				{
					new Condition("a", "0", throwOnFail: true, failureMessage: customExMessage)
				}
			};

			var ex = Assert.Throws<ConditionFailedException>(() => EquationService.Process(sequence));
			Assert.That(ex.Message, Is.EqualTo(customExMessage));
		}

		/// Inputs are renamed per mappings
		[Test]
		public void MapInputToDifferentNameTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "im", "ar", SequenceItemType.Algorithm),
				},
				Mappings = new List<Mapping>
				{
					new Mapping("i", "im", itemName: "a")
				}
			};
			var inputs = new Dictionary<string, string>
			{
				{ "i", "1" }
			};

			var results = EquationService.Process(sequence, inputs);

			Assert.That(results.Results[0].Result, Is.EqualTo("1"));
		}

		/// Results are renamed per mappings
		[Test]
		public void MapDieResultToDifferentNameForNextDieTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "arr", "br", SequenceItemType.Algorithm),
				},
				Mappings = new List<Mapping>
				{
					new Mapping("ar", "arr", itemName: "b")
				}
			};

			var results = EquationService.Process(sequence);

			Assert.That(results.Results[1].Result, Is.EqualTo("1"));
		}

		/// The pre-mapped input name is kept after mapping
		[Test]
		public void OriginalNameIsNotLostWhenMappingNameTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "ar + arr", "br", SequenceItemType.Algorithm),
				},
				Mappings = new List<Mapping>
				{
					new Mapping("ar", "arr", itemName: "b")
				}
			};

			var results = EquationService.Process(sequence);

			Assert.That(results.Results[1].Result, Is.EqualTo("2"));
		}

		/// Test 2 input mappings
		[Test]
		public void Map2DieResultsToDifferentNameForNextDieTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "1", "br", SequenceItemType.Algorithm),
					new SequenceItem("c", "arr + brr", "cr", SequenceItemType.Algorithm),
				},
				Mappings = new List<Mapping>
				{
					new Mapping("ar", "arr", itemName: "c"),
					new Mapping("br", "brr", itemName: "c"),
				}
			};

			var results = EquationService.Process(sequence);

			Assert.That(results.Results[2].Result, Is.EqualTo("2"));
		}

		/// Test that mapped input is available to specified die only
		[Test]
		public void MapDieResultToDieAndLaterDieDoesNotSeeItTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "arr", "br", SequenceItemType.Algorithm),
					new SequenceItem("c", "arr", "cr", SequenceItemType.Algorithm),
				},
				Mappings = new List<Mapping>
				{
					new Mapping("ar", "arr", itemName: "c")
				}
			};

			// die c throws an exception because it does not know about the mapped input
			Assert.Throws<EquationInputArgumentException>(() => EquationService.Process(sequence));
		}

		/// Test inject mapped input variables into custom function
		[Test]
		[Repeat(100)]
		public void InjectMappedVarsIntoCustomDiceFunction_PerformsDieRoll()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "random(1,minValue,maxValue)", "ar", SequenceItemType.Algorithm),
				},
				Mappings = new List<Mapping>
				{
					new Mapping("minRange", "minValue", itemName: "a"),
					new Mapping("maxRange", "maxValue", itemName: "a")
				}
			};
			var inputs = new Dictionary<string, string>
			{
				{ "minRange", "1" },
				{ "maxRange", "6" }
			};

			var results = EquationService.Process(sequence, inputs);

			Assert.That(results.Results[0].Result, Is.GreaterThanOrEqualTo("1"));
			Assert.That(results.Results[0].Result, Is.LessThanOrEqualTo("6"));
		}

		/// Test that a mapping with no item specified applies to all items
		[Test]
		public void UnorderedMappingTest()
		{
			var sequence = new Sequence()
			{
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "a", "ar", SequenceItemType.Algorithm),
					new SequenceItem("b", "a", "br", SequenceItemType.Algorithm),
				},
				Mappings = new List<Mapping>
				{
					new Mapping("b", "a")
				}
			};

			var results = EquationService.Process(sequence, new Dictionary<string, string>
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
				Items = new List<SequenceItem>
				{
					new SequenceItem("a", "aa", "ar", SequenceItemType.Algorithm),
				},
				Mappings = new List<Mapping>
				{
					new Mapping("a", "aa", roleName: "r1", itemName: "a")
				}
			};
			var roles = new List<Role>
			{
				new Role("r1", new Dictionary<string, string>
				{
					{ "a", "1" }
				}, new List<string>())
			};

			var results = EquationService.Process(sequence, roles: roles);

			Assert.That(results.Results[0].Result, Is.EqualTo("1"));
		}

		[Test]
		public void Check_NoConditions_ReturnsTrue()
		{
			var sequence = new Sequence();

			var valid = EquationService.Check(sequence);

			Assert.IsTrue(valid);
		}

		[Test]
		public void Check_ValidSequenceCondition_ReturnsTrue()
		{
			var sequence = new Sequence()
			{
				Conditions = new List<Condition>
				{
					new Condition("1")
				}
			};

			var valid = EquationService.Check(sequence);

			Assert.IsTrue(valid);
		}

		[Test]
		public void Check_ValidSequenceCondition_ReturnsFalse()
		{
			var sequence = new Sequence()
			{
				Conditions = new List<Condition>
				{
					new Condition("0")
				}
			};

			var valid = EquationService.Check(sequence);

			Assert.IsFalse(valid);
		}

		[Test]
		public void Check_RoleMappingIntoSequenceCondition_ReturnsTrue()
		{
			var sequence = new Sequence()
			{
				Mappings = new List<Mapping>
				{
					new Mapping("a", "aa", roleName: "r1")
				},
				Conditions = new List<Condition>
				{
					new Condition("aa = 1")
				}
			};
			var roles = new List<Role>
			{
				new Role("r1", new Dictionary<string, string>
				{
					{ "a", "1" }
				}, new List<string>())
			};

			var valid = EquationService.Check(sequence, roles: roles);

			Assert.IsTrue(valid);
		}

		[Test]
		public void Check_RoleMappingIntoSequenceCondition_ReturnsFalse()
		{
			var sequence = new Sequence()
			{
				Mappings = new List<Mapping>
				{
					new Mapping("a", "aa", roleName: "r1")
				},
				Conditions = new List<Condition>
				{
					new Condition("aa = 1")
				}
			};
			var roles = new List<Role>
			{
				new Role("r1", new Dictionary<string, string>
				{
					{ "a", "0" }
				}, new List<string>())
			};

			var valid = EquationService.Check(sequence, roles: roles);

			Assert.IsFalse(valid);
		}

		[Test]
		public void Check_InputMappingIntoSequenceCondition_ReturnsTrue()
		{
			var sequence = new Sequence()
			{
				Mappings = new List<Mapping>
				{
					new Mapping("a", "aa")
				},
				Conditions = new List<Condition>
				{
					new Condition("a")
				}
			};
			var inputs = new Dictionary<string, string> { { "a", "1" } };

			var valid = EquationService.Check(sequence, inputs: inputs);

			Assert.IsTrue(valid);
		}

		[Test]
		public void Check_InputMappingIntoSequenceCondition_ReturnsFalse()
		{
			var sequence = new Sequence()
			{
				Mappings = new List<Mapping>
				{
					new Mapping("a", "aa")
				},
				Conditions = new List<Condition>
				{
					new Condition("a")
				}
			};
			var inputs = new Dictionary<string, string> { { "a", "0" } };

			var valid = EquationService.Check(sequence, inputs: inputs);

			Assert.IsFalse(valid);
		}
	}
}