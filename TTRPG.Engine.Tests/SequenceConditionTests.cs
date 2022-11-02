using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(Condition))]
	public class SequenceConditionTests
	{
		HashSet<string> _failureMessages;

		[SetUp]
		public void SetupTest()
		{
			_failureMessages = new HashSet<string>();
		}

		private EquationService MockEquationService(int processResult)
		{
			var mockResolver = new Mock<IEquationResolver>();
			mockResolver.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Returns(processResult);
			var resolver = mockResolver.Object;

			return new EquationService(resolver);
		}

		[Test]
		public void Check_False_ReturnsFalseForSequence()
		{
			var equation = "anything";
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(equation);

			bool result = resolver.Check(condition, inputs: null, ref _failureMessages);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_False_ReturnsFalseForSequenceItem()
		{
			var itemName = "a";
			var equation = "anything";
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(equation);

			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_True_ReturnsTrueForSequence()
		{
			var equation = "anything";
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(equation);

			bool result = resolver.Check(condition, inputs: null, ref _failureMessages);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_True_ReturnsTrueForSequenceItem()
		{
			var itemName = "a";
			var equation = "anything";
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(equation);

			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_FalseAndHasErrors_WritesErrors()
		{
			var itemName = "c";
			var equation = "anything";
			var failureMessage = "error";
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(equation);
			condition.FailureMessage = failureMessage;

			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsFalse(result);
			Assert.Contains(failureMessage, _failureMessages.ToArray());
		}
	}
}
