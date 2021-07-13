using NUnit.Framework;
using System.Collections.Generic;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(Mapping))]
	internal class RoleMappingTests
	{
		Mapping mapping;
		List<Role> roles;
		Role role;
		Dictionary<string, string> inputs;
		EquationService service;

		[SetUp]
		public void SetupTests()
		{
			mapping = new Mapping();
			mapping.MappingType = MappingType.Role;
			mapping.RoleName = "r1";
			inputs = new Dictionary<string, string>();
			roles = new List<Role>();
			role = new Role("r1", new Dictionary<string, string>(), new List<string>());
			roles.Add(role);
			service = new EquationService(null);
		}

		[Test]
		public void Apply_KeyIsMappedTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			role.Attributes["a"] = "1";

			service.Apply(mapping, "a", ref inputs, roles);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_MappingIsPerformedWhenMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			role.Attributes["a"] = "1";

			service.Apply(mapping, "a", ref inputs, roles);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_MappingIsNotPerformedWhenNotMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "b";
			role.Attributes["a"] = "1";

			service.Apply(mapping, "a", ref inputs, roles);

			Assert.True(!inputs.ContainsKey("b"));
		}

		[Test]
		public void Apply_DoesNotThrowWhenSucceeds()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			role.Attributes["a"] = "1";
			mapping.ThrowOnFailure = true;

			service.Apply(mapping, "a", ref inputs, roles);
		}

		[Test]
		public void Apply_MissingMappingThrow()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			mapping.ThrowOnFailure = true;

			var ex = Assert.Throws<MappingFailedException>(() => service.Apply(mapping, "a", ref inputs, roles));
			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to missing key: '{mapping.From}'."));
		}

		[Test]
		public void Apply_MissingRoleThrow()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			mapping.ThrowOnFailure = true;
			roles.Clear();

			var ex = Assert.Throws<MissingRoleException>(() => service.Apply(mapping, "a", ref inputs, roles));
			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to missing role: '{mapping.RoleName}'."));
		}
	}
}
