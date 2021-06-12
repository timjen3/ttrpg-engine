using DieEngine.Exceptions;
using DieEngine.SequencesItems;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture]
	[TestOf(typeof(DieSequenceItem))]
	public class DieSequenceItemTests
	{
		DieSequenceItem TestDie;

		[SetUp]
		public void Setup()
		{
			TestDie = new DieSequenceItem
			{
				Name = ""
			};
		}
		
		/// Test some basic maths
		[Test]
		public void OnePlusOne_Equals_Two()
		{
			TestDie.Equation = "1 + 1";

			var result = TestDie.GetResult();

			Assert.That(result.Result, Is.EqualTo(2));
		}

		/// Test variable substitution
		[Test]
		public void OnePlusOneWithVarSubstitution_Equals_Two()
		{
			TestDie.Equation = "1 + a";
			var inputs = new Dictionary<string, double>
			{
				{ "a", 1 },
			};

			var result = TestDie.GetResult(inputs);

			Assert.That(result.Result, Is.EqualTo(2));
		}

		/// Test variable substitution with a rename
		[Test]
		public void EquationMissingVariable_Roll_ThrowsException()
		{
			TestDie.Equation = "1 + a";

			Assert.Throws<EquationInputArgumentException>(() => TestDie.GetResult());
		}

		/// Test custom dice function
		[Test]
		[Repeat(100)]
		public void CustomDiceFunction_PerformsDieRoll()
		{
			TestDie.Equation = "[dice:1,6]";

			var result = TestDie.GetResult();
			TestContext.WriteLine($"Result: {result.Result}");

			Assert.That(result.Result, Is.GreaterThanOrEqualTo(1));
			Assert.That(result.Result, Is.LessThanOrEqualTo(6));
		}

		/// Test custom dice function with 2 instances
		[Test]
		[Repeat(100)]
		public void CustomDiceFunction_Performs2DieRolls()
		{
			TestDie.Equation = "[dice:1,6] + [dice:1,6]";

			var result = TestDie.GetResult();
			TestContext.WriteLine($"Result: {result.Result}");

			Assert.That(result.Result, Is.GreaterThanOrEqualTo(2));
			Assert.That(result.Result, Is.LessThanOrEqualTo(12));
		}

		/// Test unknown custom function throws
		[Test]
		public void UnknownCustomFunction_ThrowsException()
		{
			TestDie.Equation = "[unknown:1,6]";

			Assert.Throws<UnknownCustomFunctionException>(() => TestDie.GetResult());
		}

		/// Test inject input variables into custom function
		[Test]
		[Repeat(100)]
		public void InjectVarsIntoCustomDiceFunction_PerformsDieRoll()
		{
			var inputs = new Dictionary<string, double>
			{
				{ "minRoll", 1 },
				{ "maxRoll", 6 }
			};
			TestDie.Equation = "[dice:{minRoll},{maxRoll}]";

			var result = TestDie.GetResult(inputs);
			TestContext.WriteLine($"Result: {result.Result}");

			Assert.That(result.Result, Is.GreaterThanOrEqualTo(1));
			Assert.That(result.Result, Is.LessThanOrEqualTo(6));
		}
	}
}