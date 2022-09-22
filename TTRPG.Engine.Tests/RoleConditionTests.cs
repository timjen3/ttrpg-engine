using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(RoleCondition))]
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
			var role = new Role();
			role.Name = "a";
			role.Categories.Add("c1");
			sequence.RoleConditions.Add(new RoleCondition
			{
				RoleName = "a",
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
			var role = new Role();
			role.Name = "a";
			role.Categories.Add("c1");
			sequence.RoleConditions.Add(new RoleCondition
			{
				RoleName = "a",
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
			var role = new Role();
			role.Name = "b";
			role.Categories.Add("c1");
			sequence.RoleConditions.Add(new RoleCondition
			{
				RoleName = "a",
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
			sequence.RoleConditions.Add(new RoleCondition
			{
				RoleName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			bool result = resolver.Check(sequence, (Role) null);

			Assert.IsFalse(result);
		}

		[Test]
		public void Process_MissingRoleRequirement_ThrowsException()
		{
			int resolverResult = 1;
			var resolver = MockEquationService(resolverResult);
			var sequence = new Sequence();
			sequence.RoleConditions.Add(new RoleCondition
			{
				RoleName = "a",
				RequiredCategories = new List<string>
				{
					"c1"
				}
			});

			Assert.Throws<RoleConditionFailedException>(() => resolver.Process(sequence, inputs: null, roles: null));
		}
	}
}
