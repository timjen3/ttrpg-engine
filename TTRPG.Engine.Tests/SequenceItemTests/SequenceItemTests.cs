using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TTRPG.Engine.Equations;
using TTRPG.Engine.SequenceItems;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(SequenceItem))]
	public class SequenceItemTests
	{
		private EquationService MockEquationService(int processResult)
		{
			var resolverMock = new Mock<IEquationResolver>();
			resolverMock.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Returns(processResult);
			var resolver = resolverMock.Object;

			return new EquationService(resolver);
		}

		Dictionary<string, string> GlobalInputs;
		Dictionary<string, string> MappedInputs;
		Entity Entity;

		[SetUp]
		public void SetupTest()
		{
			GlobalInputs = new Dictionary<string, string>();
			MappedInputs = new Dictionary<string, string>();
			Entity = new Entity("a");
		}

		/// Test that a sequence item resolves a basic algorithm
		[Test]
		public void SequenceItemResolvesAlgorithmTest()
		{
			int equationResult = 1;
			int order = 0;
			var item = new SequenceItem("a", "1", "ar");
			var resolver = MockEquationService(equationResult);

			var result = resolver.GetResult(item, order, ref GlobalInputs);

			Assert.That(result.Result, Is.EqualTo(equationResult.ToString()));
			Assert.That(result.Order, Is.EqualTo(order));
		}

		/// Test that a sequence item can be processed individually
		[Test]
		public void SequenceItemProcessAlgorithmTest()
		{
			int equationResult = 1;
			int order = 0;
			var item = new SequenceItem("a", "1", "ar");
			var resolver = MockEquationService(equationResult);

			var result = resolver.Process(item, Entity, MappedInputs);

			Assert.That(result.Result, Is.EqualTo(equationResult.ToString()));
			Assert.That(result.Order, Is.EqualTo(order));
		}
	}
}
