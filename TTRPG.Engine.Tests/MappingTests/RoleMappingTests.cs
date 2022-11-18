using System.Collections.Generic;
using NUnit.Framework;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(Mapping))]
	internal class EntityMappingTests
	{
		Mapping mapping;
		List<Entity> entities;
		Entity entity;
		Dictionary<string, string> inputs;
		EquationService service;

		[SetUp]
		public void SetupTests()
		{
			mapping = new Mapping();
			mapping.MappingType = MappingType.Entity;
			mapping.EntityName = "r1";
			inputs = new Dictionary<string, string>();
			entities = new List<Entity>();
			entity = new Entity("r1");
			entities.Add(entity);
			service = new EquationService(null);
		}

		[Test]
		public void Apply_KeyIsMappedTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			entity.Attributes["a"] = "1";

			service.Apply(mapping, "a", ref inputs, entities);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_MappingIsPerformedWhenMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			entity.Attributes["a"] = "1";

			service.Apply(mapping, "a", ref inputs, entities);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_MappingIsNotPerformedWhenNotMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "b";
			entity.Attributes["a"] = "1";

			service.Apply(mapping, "a", ref inputs, entities);

			Assert.True(!inputs.ContainsKey("b"));
		}

		[Test]
		public void Apply_DoesNotThrowWhenSucceeds()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			entity.Attributes["a"] = "1";
			mapping.ThrowOnFailure = true;

			service.Apply(mapping, "a", ref inputs, entities);
		}

		[Test]
		public void Apply_MissingMappingThrow()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			mapping.ThrowOnFailure = true;

			var ex = Assert.Throws<MappingFailedException>(() => service.Apply(mapping, "a", ref inputs, entities));
			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to missing key: '{mapping.From}'."));
		}

		[Test]
		public void Apply_MissingEntityThrow()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			mapping.ThrowOnFailure = true;
			entities.Clear();

			var ex = Assert.Throws<MissingEntityException>(() => service.Apply(mapping, "a", ref inputs, entities));
			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to missing entity: '{mapping.EntityName}'."));
		}

		[Test]
		public void Apply_MultipleEntitiesChoosesCorrectOne()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			mapping.ThrowOnFailure = true;
			entity.Attributes["a"] = "1";
			var entity2 = new Entity("r2", new Dictionary<string, string>(), new List<string>());
			entity2.Attributes["a"] = "2";
			entities.Add(entity2);

			service.Apply(mapping, "a", ref inputs, entities);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_EntityNameNotSetChooseFirst()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.EntityName = null;
			mapping.ItemName = "a";
			entity.Attributes["a"] = "1";
			mapping.ThrowOnFailure = true;

			service.Apply(mapping, "a", ref inputs, entities);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_EntityNameNotSetNoEntitiesPassedThrow()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.EntityName = null;
			mapping.ItemName = "a";
			mapping.ThrowOnFailure = true;
			entities.Clear();

			var ex = Assert.Throws<MissingEntityException>(() => service.Apply(mapping, "a", ref inputs, entities));
			Assert.That(ex.Message, Is.EqualTo("Mapping failed due to no entities being passed."));
		}

		[Test]
		public void Apply_FromFormatStringFromEntity()
		{
			mapping.From = "{rename}";
			mapping.To = "b";
			mapping.ItemName = null;
			mapping.ThrowOnFailure = true;
			entity.Attributes["rename"] = "a";
			entity.Attributes["a"] = "1";

			service.Apply(mapping, "anything", ref inputs, entities);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_ToFormatStringFromEntity()
		{
			mapping.From = "a";
			mapping.To = "{rename}";
			mapping.ItemName = null;
			mapping.ThrowOnFailure = true;
			entity.Attributes["rename"] = "b";
			entity.Attributes["a"] = "1";

			service.Apply(mapping, "anything", ref inputs, entities);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test(Description = "Results are available to the mapping formatter. If a value is mapped from a entity and exists in the results then the value mapped from the entity should take priority.")]
		public void Apply_EnsureSourceTakesPriorityOverInputs()
		{
			mapping.From = "a";
			mapping.To = "{rename}";
			mapping.ItemName = null;
			mapping.ThrowOnFailure = true;
			entity.Attributes["rename"] = "b";
			entity.Attributes["a"] = "1";
			inputs["rename"] = "c";

			service.Apply(mapping, null, ref inputs, entities);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}
	}
}
