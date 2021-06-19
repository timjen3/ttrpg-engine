﻿using DieEngine.Exceptions;
using DieEngine.Mappings;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture(Category = "Unit"]
	[TestOf(typeof(RoleMapping))]
	internal class RoleMappingTests
	{
		RoleMapping mapping;
		List<Role> roles;
		Role role;
		Dictionary<string, double> inputs;

		[SetUp]
		public void SetupTests()
		{
			mapping = new RoleMapping();
			mapping.RoleName = "r1";
			inputs = new Dictionary<string, double>();
			roles = new List<Role>();
			role = new Role("r1", new Dictionary<string, double>());
			roles.Add(role);
		}

		[Test]
		public void KeyIsMappedTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			role.Attributes["a"] = 1;

			mapping.Apply(0, ref inputs, roles);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo(1));
		}

		[Test]
		public void MappingIsPerformedWhenOrderMatches()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.Order = 0;
			role.Attributes["a"] = 1;

			mapping.Apply(0, ref inputs, roles);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo(1));
		}

		[Test]
		public void MappingIsNotPerformedWhenOrderDoesNotMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.Order = 1;
			role.Attributes["a"] = 1;

			mapping.Apply(0, ref inputs, roles);

			Assert.True(!inputs.ContainsKey("b"));
		}

		[Test]
		public void MissingMappingThrow()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.Order = 0;
			mapping.ThrowOnFailure = true;

			var ex = Assert.Throws<MappingFailedException>(() => mapping.Apply(0, ref inputs, roles));
			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to missing key: '{mapping.From}'."));
		}

		[Test]
		public void MissingRoleThrow()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.Order = 0;
			mapping.ThrowOnFailure = true;
			roles.Clear();

			var ex = Assert.Throws<MissingRoleException>(() => mapping.Apply(0, ref inputs, roles));
			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to missing role: '{mapping.RoleName}'."));
		}
	}
}
