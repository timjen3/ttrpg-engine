using System.Collections.Generic;
using NUnit.Framework;

namespace TTRPG.Engine.Tests
{
	[TestFixture]
	[TestOf(typeof(Entity))]
	public class EntityTests
	{
		[Test]
		public void CloneAs_CreatesADistinctClone()
		{
			var entity = new Entity();
			entity.Name = "a";

			var clone = entity.CloneAs("b");

			Assert.False(Is.ReferenceEquals(entity, clone));
			Assert.That(entity.Name, Is.EqualTo(clone.Name));
			Assert.That(entity.Name, Is.EqualTo("a"));
			Assert.That(clone.Name, Is.EqualTo("a"));
			Assert.That(entity.Alias, Is.EqualTo("a"));
			Assert.That(clone.Alias, Is.EqualTo("b"));
			Assert.False(Is.ReferenceEquals(entity.Attributes, clone.Attributes));
			Assert.False(Is.ReferenceEquals(entity.Categories, clone.Categories));
		}

		[Test]
		public void AttributesAreCaseInsensitive()
		{
			var entity = new Entity();

			entity.Attributes["TEST"] = "1";

			Assert.That(entity.Attributes, Contains.Key("test"));
			Assert.That(entity.Attributes["test"], Is.EqualTo("1"));
		}

		[Test]
		public void AttributesAreCaseInsensitiveWhenPassedWithout()
		{
			var dictionary = new Dictionary<string, string>
			{
				{ "TEST", "1" },
			};
			var entity = new Entity("a", dictionary, null, null);

			Assert.That(entity.Attributes, Contains.Key("test"));
			Assert.That(entity.Attributes["test"], Is.EqualTo("1"));
		}

		[Test]
		public void AttributesAreCaseInsensitiveWhenPassedNullAttributes()
		{
			var entity = new Entity("a", null, null, null);

			entity.Attributes["TEST"] = "1";

			Assert.That(entity.Attributes, Contains.Key("test"));
			Assert.That(entity.Attributes["test"], Is.EqualTo("1"));
		}

		[Test]
		public void InventoryItemsAreCaseInsensitive()
		{
			var entity = new Entity();

			entity.InventoryItems["TEST"] = new Entity("a", null, null, null);

			Assert.That(entity.InventoryItems, Contains.Key("test"));
			Assert.That(entity.InventoryItems["test"].Name, Is.EqualTo("a"));
		}

		[Test]
		public void InventoryItemsAreCaseInsensitiveWhenPassedWithout()
		{
			var dictionary = new Dictionary<string, Entity>
			{
				{ "TEST", new Entity("a", null, null, null) },
			};

			var entity = new Entity("a", null, null, dictionary);

			Assert.That(entity.InventoryItems, Contains.Key("test"));
			Assert.That(entity.InventoryItems["test"].Name, Is.EqualTo("a"));
		}

		[Test]
		public void InventoryItemsAreCaseInsensitiveWhenPassedNullAttributes()
		{
			var entity = new Entity("a", null, null, null);

			entity.InventoryItems["TEST"] = new Entity("a", null, null, null);

			Assert.That(entity.InventoryItems, Contains.Key("test"));
			Assert.That(entity.InventoryItems["test"].Name, Is.EqualTo("a"));
		}
	}
}
