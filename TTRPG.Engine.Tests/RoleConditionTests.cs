using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.Roles;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(EntityCondition))]
	public class EntityConditionTests
	{
		private EquationService MockEquationService(int processResult)
		{
			var mockResolver = new Mock<IEquationResolver>();
			mockResolver.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Returns(processResult);
			var resolver = mockResolver.Object;

			return new EquationService(resolver);
		}

		[Test]
		public void Check_EntityHasRequiredCondition_ReturnsTrue()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var entity = new Entity();
			entity.Name = "a";
			entity.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, entity);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_EntityWithDifferentCaseHasRequiredCondition_ReturnsTrue()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var entity = new Entity();
			entity.Name = "A";
			entity.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, entity);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_EntityWithAliasHasRequiredCondition_ReturnsTrue()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var entity = new Entity();
			entity.Name = "a";
			entity.Alias = "b";
			entity.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "b",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, entity);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_EntityDoesNotHaveRequiredCondition_ReturnsFalse()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var entity = new Entity();
			entity.Name = "a";
			entity.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c2"
				}
			});

			bool result = resolver.Check(sequence, entity);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_EntityDoesNotMatch_ReturnsFalse()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var entity = new Entity();
			entity.Name = "b";
			entity.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, entity);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_NullEntity_ReturnsFalse()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, (Entity)null);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_MultipleEntitiesWithValidOne_ReturnsTrue()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var entity1 = new Entity();
			entity1.Name = "a";
			entity1.Categories.Add("c1");
			var entity2 = new Entity();
			entity2.Name = "b";
			entity2.Categories.Add("c2");
			var sequence = new Sequence();
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, null, new List<Entity> { entity1, entity2 });

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_MultipleEntitiesNoneValid_ReturnsFalse()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var entity1 = new Entity();
			entity1.Name = "b";
			entity1.Categories.Add("c1");
			var entity2 = new Entity();
			entity2.Name = "c";
			entity2.Categories.Add("c2");
			var sequence = new Sequence();
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, null, new List<Entity> { entity1, entity2 });

			Assert.IsFalse(result);
		}

		[Test]
		public void Process_MissingEntityRequirement_ThrowsException()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			Assert.Throws<EntityConditionFailedException>(() => resolver.Process(sequence, inputs: null, entities: null));
		}
	}
}
