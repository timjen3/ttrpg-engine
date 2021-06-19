using DieEngine.CustomFunctions;
using DieEngine.SequencesItems;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture]
	[TestOf(typeof(DataSequenceItem<string>))]
	public class DataSequenceItemTests
	{
		IEquationResolver EquationResolver;
		Dictionary<string, double> Inputs = new Dictionary<string, double>();

		[SetUp]
		public void Setup()
		{
			var services = new ServiceCollection();
			services.AddDieEngineServices();
			var provider = services.BuildServiceProvider();
			EquationResolver = provider.GetRequiredService<IEquationResolver>();
		}

		/// Test that a data sequence item resolves a basic equation
		[Test]
		public void DataSequenceItemResolvesTest()
		{
			var item = new DataSequenceItem<string>("a", "1", "");

			var result = item.GetResult(EquationResolver, ref Inputs);

			Assert.That(result.Result, Is.EqualTo(1));
		}

		/// Test that a data sequence item resolves a basic equation
		[Test]
		public void DataSequenceItemResultTest()
		{
			string customData = "some instruction";
			var item = new DataSequenceItem<string>("a", "1", customData);

			var result = item.GetResult(EquationResolver, ref Inputs);

			Assert.That(result.ResolvedItem, Is.TypeOf<DataSequenceItem<string>>());
			Assert.That(((DataSequenceItem<string>)result.ResolvedItem).Data, Is.EqualTo(customData));
		}
	}
}
