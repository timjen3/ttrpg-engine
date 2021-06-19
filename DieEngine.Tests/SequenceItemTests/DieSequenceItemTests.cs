using DieEngine.CustomFunctions;
using DieEngine.Exceptions;
using DieEngine.SequencesItems;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture]
	[TestOf(typeof(DieSequenceItem))]
	public class DieSequenceItemTests
	{
		IEquationResolver EquationResolver;
		DieSequenceItem TestDie;
		Dictionary<string, double> Inputs = new Dictionary<string, double>();

		[SetUp]
		public void Setup()
		{
			var services = new ServiceCollection();
			services.AddDieEngineServices();
			var provider = services.BuildServiceProvider();
			EquationResolver = provider.GetRequiredService<IEquationResolver>();
			TestDie = new DieSequenceItem()
			{
				Name = "a",
				ResultName = "ar"
			};
		}
		
		/// Test some basic maths
		[Test]
		public void OnePlusOne_Equals_Two()
		{
			TestDie.Equation = "1 + 1";

			var result = TestDie.GetResult(EquationResolver, ref Inputs);

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

			var result = TestDie.GetResult(EquationResolver, ref inputs, inputs);

			Assert.That(result.Result, Is.EqualTo(2));
		}

		/// Test variable substitution with a rename
		[Test]
		public void EquationMissingVariable_Roll_ThrowsException()
		{
			TestDie.Equation = "1 + a";

			Assert.Throws<EquationInputArgumentException>(() => TestDie.GetResult(EquationResolver, ref Inputs));
		}

		/// Test custom dice function
		[Test]
		[Repeat(100)]
		public void CustomRandomFunction_GeneratesRandomNumber()
		{
			TestDie.Equation = "[random:1,1,6]";

			var result = TestDie.GetResult(EquationResolver, ref Inputs);
			TestContext.WriteLine($"Result: {result.Result}");

			Assert.That(result.Result, Is.GreaterThanOrEqualTo(1));
			Assert.That(result.Result, Is.LessThanOrEqualTo(6));
		}

		/// Test custom dice function with 2 instances
		[Test]
		[Repeat(100)]
		public void CustomRandomFunction_Generates2RandomNumbers()
		{
			TestDie.Equation = "[random:1,1,6] + [random:1,1,6]";

			var result = TestDie.GetResult(EquationResolver, ref Inputs);
			TestContext.WriteLine($"Result: {result.Result}");

			Assert.That(result.Result, Is.GreaterThanOrEqualTo(2));
			Assert.That(result.Result, Is.LessThanOrEqualTo(12));
		}

		/// Test unknown custom function throws
		[Test]
		public void UnknownCustomFunction_ThrowsException()
		{
			TestDie.Equation = "[unknown:1,6]";

			Assert.Throws<UnknownCustomFunctionException>(() => TestDie.GetResult(EquationResolver, ref Inputs));
		}

		/// Test inject input variables into custom function
		[Test]
		[Repeat(100)]
		public void InjectVarsIntoCustomDiceFunction_PerformsDieRoll()
		{
			var inputs = new Dictionary<string, double>
			{
				{ "minValue", 1 },
				{ "maxValue", 6 }
			};
			TestDie.Equation = "[random:1,{minValue},{maxValue}]";

			var result = TestDie.GetResult(EquationResolver, ref inputs, inputs);
			TestContext.WriteLine($"Result: {result.Result}");

			Assert.That(result.Result, Is.GreaterThanOrEqualTo(1));
			Assert.That(result.Result, Is.LessThanOrEqualTo(6));
		}
	}
}