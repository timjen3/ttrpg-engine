using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(Condition))]
	public class ConditionTests
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
		public void Check_MatchAndResolverReturnsTrue_ReturnsTrue()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "a";
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(conditionItemName, equation);

			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_MatchAndResolverReturnsFalse_ReturnsFalse()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "a";
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(conditionItemName, equation);
			
			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_NotMatchAndResolverReturnsFalse_ReturnsTrue()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "b";
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(conditionItemName, equation);

			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_NotMatchAndResolverReturnsTrue_ReturnsTrue()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "b";
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(conditionItemName, equation);

			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_MatchAndResolverReturnsFalseThrowIsTrueNoExMsg_ThrowsExWithDefaultMessage()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "a";
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(conditionItemName, equation, throwOnFail: true);

			var ex = Assert.Throws<ConditionFailedException>(() => resolver.Check(condition, itemName, null, null, ref _failureMessages));

			Assert.That(ex.Message, Is.EqualTo(Condition.DEFAULT_FAILURE_MESSAGE));
		}

		[Test]
		public void Check_MatchAndResolverReturnsFalseThrowIsTrueExMsg_ThrowsExWithCustomMessage()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "a";
			int resolverResult = 0;
			string customMessage = "custom ex message";
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(conditionItemName, equation, throwOnFail: true, failureMessage: customMessage);

			var ex = Assert.Throws<ConditionFailedException>(() => resolver.Check(condition, itemName, null, null, ref _failureMessages));

			Assert.That(ex.Message, Is.EqualTo(customMessage));
		}

		[Test]
		public void Check_ConditionCreatedWithoutEquationOrDependentItem_ThrowsArgumentNullException()
		{
			var conditionItemName = "a";
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);

			var ex = Assert.Throws<ArgumentNullException>(() => new Condition(conditionItemName, equation: null, dependentOnItem: null));
		}

		[Test]
		public void Check_MultipleItemsAndMatchResolvesFalse_ReturnsFalse()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemNames = new string[] { "a", "b" };
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(conditionItemNames, equation);

			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_MultipleItemsNoMatchResolvesFalse_ReturnsTrue()
		{
			var itemName = "c";
			var equation = "anything";
			var conditionItemNames = new string[] { "a", "b" };
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(conditionItemNames, equation);

			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_ItemConditionHasErrors_WriteErrors()
		{
			var itemName = "c";
			var equation = "anything";
			var conditionItemName = "c";
			var failureMessage = "error";
			int resolverResult = 0;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(conditionItemName, equation, failureMessage: failureMessage);

			bool result = resolver.Check(condition, itemName, null, null, ref _failureMessages);

			Assert.IsFalse(result);
			Assert.Contains(failureMessage, _failureMessages.ToArray());
		}
	}
}
