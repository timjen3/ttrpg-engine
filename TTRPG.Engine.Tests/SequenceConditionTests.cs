using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(Condition))]
	public class SequenceConditionTests
	{
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

			bool result = resolver.Check(condition, inputs: null);

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

			bool result = resolver.Check(condition, itemName, null, null);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_True_ReturnsTrueForSequence()
		{
			var equation = "anything";
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var condition = new Condition(equation);

			bool result = resolver.Check(condition, inputs: null);

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

			bool result = resolver.Check(condition, itemName, null, null);

			Assert.IsTrue(result);
		}
	}
}
