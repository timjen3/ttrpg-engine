using DieEngine.Equations;
using DieEngine.Exceptions;
using Moq;
using NUnit.Framework;
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
			resolver.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IDictionary<string, double>>())).Returns(processResult);

			return resolver.Object;
		}

		[Test]
		public void Check_OrderMatchesAndResolverReturnsTrue_ReturnsTrue()
		{
			int conditionOrder = 0;
			int itemOrder = 0;
			int resolverResult = 1;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition("", conditionOrder);

			bool result = condition.Check(itemOrder, resolver, null);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_OrderMatchesAndResolverReturnsFalse_ReturnsFalse()
		{
			int conditionOrder = 0;
			int itemOrder = 0;
			int resolverResult = 0;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition("", conditionOrder, false, null);
			
			bool result = condition.Check(itemOrder, resolver, null);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_OrderNotMatchesAndResolverReturnsFalse_ReturnsTrue()
		{
			int conditionOrder = 0;
			int itemOrder = 1;
			int resolverResult = 0;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition("", conditionOrder, false, null);

			bool result = condition.Check(itemOrder, resolver, null);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_OrderNotMatchesAndResolverReturnsTrue_ReturnsTrue()
		{
			int conditionOrder = 0;
			int itemOrder = 0;
			int resolverResult = 1;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition("", conditionOrder, false, null);

			bool result = condition.Check(itemOrder, resolver, null);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_OrderMatchesAndResolverReturnsFalseThrowIsTrueNoExMsg_ThrowsExWithDefaultMessage()
		{
			int conditionOrder = 0;
			int itemOrder = 0;
			int resolverResult = 0;
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition("", conditionOrder, true, null);

			var ex = Assert.Throws<ConditionFailedException>(() => condition.Check(itemOrder, resolver, null));

			Assert.That(ex.Message, Is.EqualTo(Condition.DEFAULT_FAILURE_MESSAGE));
		}

		[Test]
		public void Check_OrderMatchesAndResolverReturnsFalseThrowIsTrueExMsg_ThrowsExWithCustomMessage()
		{
			int conditionOrder = 0;
			int itemOrder = 0;
			int resolverResult = 0;
			string customMessage = "custom ex message";
			var resolver = MockEquationResolver(resolverResult);
			var condition = new Condition("", conditionOrder, true, customMessage);

			var ex = Assert.Throws<ConditionFailedException>(() => condition.Check(itemOrder, resolver, null));

			Assert.That(ex.Message, Is.EqualTo(customMessage));
		}
	}
}
