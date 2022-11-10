using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(EntityCondition))]
	public class RoleConditionTests
	{
		private EquationService MockEquationService(int processResult)
		{
			var mockResolver = new Mock<IEquationResolver>();
			mockResolver.Setup(x => x.Process(It.IsAny<string>(), It.IsAny<IDictionary<string, string>>())).Returns(processResult);
			var resolver = mockResolver.Object;

			return new EquationService(resolver);
		}

		[Test]
		public void Check_RoleHasRequiredCondition_ReturnsTrue()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var role = new Entity();
			role.Name = "a";
			role.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, role);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_RoleWithDifferentCaseHasRequiredCondition_ReturnsTrue()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var role = new Entity();
			role.Name = "A";
			role.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, role);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_RoleWithAliasHasRequiredCondition_ReturnsTrue()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var role = new Entity();
			role.Name = "a";
			role.Alias = "b";
			role.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "b",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, role);

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_RoleDoesNotHaveRequiredCondition_ReturnsFalse()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var role = new Entity();
			role.Name = "a";
			role.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c2"
				}
			});

			bool result = resolver.Check(sequence, role);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_RoleDoesNotMatch_ReturnsFalse()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			var role = new Entity();
			role.Name = "b";
			role.Categories.Add("c1");
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, role);

			Assert.IsFalse(result);
		}

		[Test]
		public void Check_NullRole_ReturnsFalse()
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
		public void Check_MultipleRolesWithValidOne_ReturnsTrue()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var role1 = new Entity();
			role1.Name = "a";
			role1.Categories.Add("c1");
			var role2 = new Entity();
			role2.Name = "b";
			role2.Categories.Add("c2");
			var sequence = new Sequence();
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, null, new List<Entity> { role1, role2 });

			Assert.IsTrue(result);
		}

		[Test]
		public void Check_MultipleRolesNoneValid_ReturnsFalse()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var role1 = new Entity();
			role1.Name = "b";
			role1.Categories.Add("c1");
			var role2 = new Entity();
			role2.Name = "c";
			role2.Categories.Add("c2");
			var sequence = new Sequence();
			sequence.EntityConditions.Add(new EntityCondition
			{
				EntityName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, null, new List<Entity> { role1, role2 });

			Assert.IsFalse(result);
		}

		[Test]
		public void Process_MissingRoleRequirement_ThrowsException()
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
