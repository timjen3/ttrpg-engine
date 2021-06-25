using DieEngine.Conditions;
using DieEngine.Equations;
using DieEngine.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(Condition))]
	public class ConditionTests
	{
		private IEquationResolver MockEquationResolver(int processResult)
		{
			var resolver = new Mock<IEquationResolver>();
			resolver.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Returns(processResult);

			return resolver.Object;
		}

		[Test]
		public void Check_MatchAndResolverReturnsTrue_ReturnsTrue()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "a";
			int resolverResult = 1;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition(conditionItemName, equation);

			bool result = condition.Check(itemName, resolver, null, null);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_MatchAndResolverReturnsFalse_ReturnsFalse()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "a";
			int resolverResult = 0;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition(conditionItemName, equation);
			
			bool result = condition.Check(itemName, resolver, null, null);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_NotMatchAndResolverReturnsFalse_ReturnsTrue()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "b";
			int resolverResult = 0;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition(conditionItemName, equation);

			bool result = condition.Check(itemName, resolver, null, null);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_NotMatchAndResolverReturnsTrue_ReturnsTrue()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "b";
			int resolverResult = 0;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition(conditionItemName, equation);

			bool result = condition.Check(itemName, resolver, null, null);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_MatchAndResolverReturnsFalseThrowIsTrueNoExMsg_ThrowsExWithDefaultMessage()
		{
			var itemName = "a";
			var equation = "anything";
			var conditionItemName = "a";
			int resolverResult = 0;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition(conditionItemName, equation, throwOnFail: true);

			var ex = Assert.Throws<ConditionFailedException>(() => condition.Check(itemName, resolver, null, null));

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
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition(conditionItemName, equation, throwOnFail: true, failureMessage: customMessage);

			var ex = Assert.Throws<ConditionFailedException>(() => condition.Check(itemName, resolver, null, null));

			Assert.That(ex.Message, Is.EqualTo(customMessage));
		}

		[Test]
		public void Check_ConditionCreatedWithoutEquationOrDependentItem_ThrowsArgumentNullException()
		{
			var conditionItemName = "a";
			int resolverResult = 0;
			var resolver = MockEquationResolver(resolverResult);

			var ex = Assert.Throws<ArgumentNullException>(() => new Condition(conditionItemName));
		}
	}
}
