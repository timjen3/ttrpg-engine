using DieEngine.Equations;
using DieEngine.SequencesItems;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(DataSequenceItem<string>))]
	public class DataSequenceItemTests
	{
		private IEquationResolver MockEquationResolver(int processResult)
		{
			var resolver = new Mock<IEquationResolver>();
			resolver.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Returns(processResult);

			return resolver.Object;
		}

		Dictionary<string, string> Inputs = new Dictionary<string, string>();

		/// Test that a data sequence item resolves a basic equation
		[Test]
		public void DataSequenceItemResolvesTest()
		{
			int equationResult = 1;
			int order = 0;
			var item = new DataSequenceItem<string>("a", "1", "");
			var resolver = MockEquationResolver(equationResult);

			var result = item.GetResult(order, resolver, ref Inputs);

			Assert.That(result.Result, Is.EqualTo(equationResult.ToString()));
			Assert.That(result.Order, Is.EqualTo(order));
		}

		/// Test that a data sequence item resolves a basic equation
		[Test]
		public void DataSequenceItemResultTest()
		{
			int equationResult = 1;
			string customData = "some instruction";
			var item = new DataSequenceItem<string>("a", "1", customData);
			var resolver = MockEquationResolver(equationResult);

			var result = item.GetResult(0, resolver, ref Inputs);

			Assert.That(result.ResolvedItem, Is.TypeOf<DataSequenceItem<string>>());
			Assert.That(((DataSequenceItem<string>)result.ResolvedItem).Data, Is.EqualTo(customData));
		}
	}
}
