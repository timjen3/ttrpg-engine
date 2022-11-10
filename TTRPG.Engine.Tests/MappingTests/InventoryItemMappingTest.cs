using System;
using System.Collections.Generic;
using NUnit.Framework;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(Mapping))]
	internal class InventoryItemMappingTests
	{
		Mapping mapping;
		List<Entity> roles;
		Entity role;
		Entity item;
		Dictionary<string, string> inputs;
		EquationService service;

		[SetUp]
		public void SetupTests()
		{
			mapping = new Mapping();
			mapping.MappingType = MappingType.InventoryItem;
			mapping.EntityName = "r1";
			mapping.InventoryItemName = "i1";
			inputs = new Dictionary<string, string>();
			roles = new List<Entity>();
			role = new Entity("r1");
			item = new Entity("i1");
			role.InventoryItems["i1"] = item;
			roles.Add(role);
			service = new EquationService(null);
		}

		[Test]
		public void Apply_KeyIsMappedTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			item.Attributes["a"] = "1";

			service.Apply(mapping, "a", ref inputs, roles);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_ItemNameSetKeyIsMappedTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			item.Attributes["a"] = "1";

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
			item.Attributes["a"] = "1";

			service.Apply(mapping, "a", ref inputs, roles);

			Assert.True(!inputs.ContainsKey("b"));
		}

		[Test]
		public void Apply_DoesNotThrowWhenSucceeds()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			item.Attributes["a"] = "1";
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
		public void Apply_MissingItemThrow()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			mapping.ThrowOnFailure = true;
			role.InventoryItems.Clear();

			var ex = Assert.Throws<MissingEntityException>(() => service.Apply(mapping, "a", ref inputs, roles));
			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to entity not having item: '{mapping.InventoryItemName}'."));
		}

		[Test]
		public void Apply_MultipleItemsChoosesCorrectOne()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			mapping.ThrowOnFailure = true;
			item.Attributes["a"] = "1";
			var role2 = new Entity("r2");
			role2.InventoryItems["i1"] = new Entity("i1");
			role2.InventoryItems["i1"].Attributes["a"] = "2";
			roles.Add(role2);

			service.Apply(mapping, "a", ref inputs, roles);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Ctor_ItemNameNullThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new Mapping("a", "b", null, null, null));
		}
	}
}
