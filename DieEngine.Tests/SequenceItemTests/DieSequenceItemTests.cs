using DieEngine.Equations;
using DieEngine.SequencesItems;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(DieSequenceItem))]
	public class DieSequenceItemTests
	{
		private IEquationResolver MockEquationResolver(int processResult)
		{
			var resolver = new Mock<IEquationResolver>();
			resolver.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IDictionary<string, double>>())).Returns(processResult);

			return resolver.Object;
		}

		Dictionary<string, double> Inputs = new Dictionary<string, double>();

		/// Test that a data sequence item resolves a basic equation
		[Test]
		public void DieSequenceItemResolvesTest()
		{
			int equationResult = 1;
			var item = new DataSequenceItem<string>("a", "1", "");
			var resolver = MockEquationResolver(equationResult);


			var result = item.GetResult(resolver, ref Inputs);

			Assert.That(result.Result, Is.EqualTo(equationResult));
		}
	}
}