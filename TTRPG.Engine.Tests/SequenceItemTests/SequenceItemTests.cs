using Moq;
using NUnit.Framework;
using System.Collections.Generic;
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
		Role Role;

		[SetUp]
		public void SetupTest()
		{
			GlobalInputs = new Dictionary<string, string>();
			MappedInputs = new Dictionary<string, string>();
			Role = new Role("a");
		}

		/// Test that a sequence item resolves a basic algorithm
		[Test]
		public void SequenceItemResolvesAlgorithmTest()
		{
			int equationResult = 1;
			int order = 0;
			var item = new SequenceItem("a", "1", "ar", SequenceItemEquationType.Algorithm);
			var resolver = MockEquationService(equationResult);

			var result = resolver.GetResult(item, order, ref GlobalInputs);

			Assert.That(result.Result, Is.EqualTo(equationResult.ToString()));
			Assert.That(result.Order, Is.EqualTo(order));
		}

		/// Test that a sequence item resolves a basic message
		[Test]
		public void SequenceItemResolvesMessageTest()
		{
			string messageResult = "Hello a.";
			int order = 0;
			var item = new SequenceItem("a", "Hello {name}.", "ar", SequenceItemEquationType.Message);
			var resolver = MockEquationService(0);
			MappedInputs["name"] = "a";

			var result = resolver.GetResult(item, order, ref GlobalInputs, MappedInputs);

			Assert.That(result.Result, Is.EqualTo(messageResult));
			Assert.That(result.Order, Is.EqualTo(order));
		}

		/// Test that a sequence item can be processed individually
		[Test]
		public void SequenceItemProcessAlgorithmTest()
		{
			int equationResult = 1;
			int order = 0;
			var item = new SequenceItem("a", "1", "ar", SequenceItemEquationType.Algorithm);
			var resolver = MockEquationService(equationResult);

			var result = resolver.Process(item, Role, MappedInputs);

			Assert.That(result.Result, Is.EqualTo(equationResult.ToString()));
			Assert.That(result.Order, Is.EqualTo(order));
		}

		/// Test that a sequence item can be processed individually
		[Test]
		public void SequenceItemProcessMessageTest()
		{
			string messageResult = "Hello a.";
			int order = 0;
			var item = new SequenceItem("a", "Hello {name}.", "ar", SequenceItemEquationType.Message);
			var resolver = MockEquationService(0);
			MappedInputs["name"] = "a";

			var result = resolver.Process(item, Role, MappedInputs);

			Assert.That(result.Result, Is.EqualTo(messageResult));
			Assert.That(result.Order, Is.EqualTo(order));
		}

		/// Test that a sequence item can be processed individually
		[Test]
		public void SequenceItemProcessMessageWithRoleTest()
		{
			string messageResult = "Hello a.";
			int order = 0;
			var item = new SequenceItem("a", "Hello {name}.", "ar", SequenceItemEquationType.Message);
			var resolver = MockEquationService(0);
			Role.Attributes["name"] = "a";

			var result = resolver.Process(item, Role, MappedInputs);

			Assert.That(result.Result, Is.EqualTo(messageResult));
			Assert.That(result.Order, Is.EqualTo(order));
		}
	}
}