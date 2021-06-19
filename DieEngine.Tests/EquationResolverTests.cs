using DieEngine.Equations;
using DieEngine.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(EquationResolver))]
	public class EquationResolverTests
	{
		EquationResolver EquationResolver;

		[SetUp]
		public void SetupTest()
		{
			EquationResolver = new EquationResolver();
		}

		[Test]
		public void Process_OnePlusOne_Equals2()
		{
			var equation = "1 + 1";
			var inputs = new Dictionary<string, double>()
			{
			};

			var result = EquationResolver.Process(equation, inputs);

			Assert.That(result, Is.EqualTo(2));
		}

		[Test]
		public void Process_VariableSubstitution_Test()
		{
			var equation = "a";
			var inputs = new Dictionary<string, double>()
			{
				{ "a", 1 }
			};

			var result = EquationResolver.Process(equation, inputs);

			Assert.That(result, Is.EqualTo(1));
		}

		[Test]
		public void Process_MissingInput_Throws()
		{
			var equation = "a";
			var inputs = new Dictionary<string, double>()
			{
			};

			Assert.Throws<EquationInputArgumentException>(() => EquationResolver.Process(equation, inputs));
		}

		[Test]
		public void Process_MissingFunc_Throws()
		{
			var equation = "foobar(1,2,3)";
			var inputs = new Dictionary<string, double>()
			{
			};

			Assert.Throws<UnknownCustomFunctionException>(() => EquationResolver.Process(equation, inputs));
		}

		[Test]
		[Repeat(1000)]
		public void Process_RandomFunction_InsertsRandomValue()
		{
			var equation = "random(1,1,6)";
			var inputs = new Dictionary<string, double>()
			{
			};

			var result = EquationResolver.Process(equation, inputs);

			TestContext.WriteLine(result);
			Assert.That(result, Is.GreaterThanOrEqualTo(1).And.LessThanOrEqualTo(6));
		}

		[Test]
		[Repeat(1000)]
		public void Process_RandomFunctionWithVars_InsertsRandomValue()
		{
			var equation = "random(count,minRange,maxRange)";
			var inputs = new Dictionary<string, double>()
			{
				{ "count", 1 },
				{ "minRange", 1 },
				{ "maxRange", 6 }
			};

			var result = EquationResolver.Process(equation, inputs);

			TestContext.WriteLine(result);
			Assert.That(result, Is.GreaterThanOrEqualTo(1).And.LessThanOrEqualTo(6));
		}
	}
}
