using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(EquationResolver))]
	public class EquationResolverTests
	{
		IEquationResolver EquationResolver;

		[OneTimeSetUp]
		public void SetupTests()
		{
			var services = new ServiceCollection();
			services.AddTTRPGEngineServices();
			var provider = services.BuildServiceProvider();
			EquationResolver = provider.GetRequiredService<IEquationResolver>();
		}

		[Test]
		public void ServiceCollectionExtensions_TypeIsEquationResolver()
		{
			Assert.That(EquationResolver, Is.TypeOf<EquationResolver>());
		}

		[Test]
		public void Process_OnePlusOne_Equals2()
		{
			var equation = "1 + 1";
			var inputs = new Dictionary<string, string>()
			{
			};

			var result = EquationResolver.Process(equation, inputs);

			Assert.That(result, Is.EqualTo(2));
		}

		[Test]
		public void Process_VariableSubstitution_Test()
		{
			var equation = "a";
			var inputs = new Dictionary<string, string>()
			{
				{ "a", "1" }
			};

			var result = EquationResolver.Process(equation, inputs);

			Assert.That(result, Is.EqualTo(1));
		}

		[Test]
		public void Process_MissingInput_Throws()
		{
			var equation = "a";
			var inputs = new Dictionary<string, string>()
			{
			};

			Assert.Throws<CustomFunctionArgumentException>(() => EquationResolver.Process(equation, inputs));
		}

		[Test]
		public void Process_MissingFunc_Throws()
		{
			var equation = "foobar(1,2,3)";
			var inputs = new Dictionary<string, string>()
			{
			};

			Assert.Throws<EquationResolverException>(() => EquationResolver.Process(equation, inputs));
		}

		[Test]
		[Repeat(1000)]
		public void Process_RandomFunction_InsertsRandomValue()
		{
			var equation = "rnd(1,1,6)";
			var inputs = new Dictionary<string, string>()
			{
			};

			var result = EquationResolver.Process(equation, inputs);

			TestContext.WriteLine(result);
			Assert.That(result, Is.GreaterThanOrEqualTo(1).And.LessThanOrEqualTo(6));
		}

		[Test]
		[Repeat(1000)]
		public void Process_TossFunction_Inserts0or1()
		{
			var equation = "toss()";
			var inputs = new Dictionary<string, string>()
			{
			};

			var result = EquationResolver.Process(equation, inputs);

			TestContext.WriteLine(result);
			Assert.That(result, Is.EqualTo(0).Or.EqualTo(1));
		}

		[Test]
		[Repeat(10000)]
		public void Process_RandomFunctionWithVars_InsertsRandomValue()
		{
			var equation = "rnd(count,minRange,maxRange)";
			var inputs = new Dictionary<string, string>()
			{
				{ "count", "1" },
				{ "minRange", "1" },
				{ "maxRange", "6" }
			};

			var result = EquationResolver.Process(equation, inputs);

			Assert.That(result, Is.GreaterThanOrEqualTo(1).And.LessThanOrEqualTo(6));
		}

		[Test]
		[Repeat(1000)]
		public void Process_UseExpressionString_SucceedsAndFast()
		{
			var equation = "a + [[b]]";
			var inputs = new Dictionary<string, string>()
			{
				{ "a", "1" },
				{ "[[b]]", "floor(1.5)" }
			};

			var result = EquationResolver.Process(equation, inputs);

			Assert.That(result, Is.EqualTo(2));
		}
	}
}
