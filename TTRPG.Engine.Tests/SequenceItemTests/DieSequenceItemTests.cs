using TTRPG.Engine.Equations;
using TTRPG.Engine.SequenceItems;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(SequenceItem))]
	public class DieSequenceItemTests
	{
		private IEquationResolver MockEquationResolver(int processResult)
		{
			var resolver = new Mock<IEquationResolver>();
			resolver.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Returns(processResult);

			return resolver.Object;
		}

		Dictionary<string, string> Inputs = new Dictionary<string, string>();

		/// Test that a die sequence item resolves a basic equation
		[Test]
		public void DieSequenceItemResolvesTest()
		{
			int equationResult = 1;
			int order = 0;
			var item = new SequenceItem("a", "1", "ar", SequenceItemType.Algorithm);
			var resolver = MockEquationResolver(equationResult);

			var result = item.GetResult(order, resolver, ref Inputs);

			Assert.That(result.Result, Is.EqualTo(equationResult.ToString()));
			Assert.That(result.Order, Is.EqualTo(order));
		}
	}
}