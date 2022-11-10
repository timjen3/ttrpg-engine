using System.Collections.Generic;
using NUnit.Framework;

namespace TTRPG.Engine.Tests
{
	[TestFixture]
	[TestOf(typeof(Entity))]
	public class RoleTests
	{
		[Test]
		public void CloneAs_CreatesADistinctClone()
		{
			var role = new Entity();
			role.Name = "a";

			var clone = role.CloneAs("b");

			Assert.False(Is.ReferenceEquals(role, clone));
			Assert.That(role.Name, Is.EqualTo(clone.Name));
			Assert.That(role.Name, Is.EqualTo("a"));
			Assert.That(clone.Name, Is.EqualTo("a"));
			Assert.That(role.Alias, Is.EqualTo("a"));
			Assert.That(clone.Alias, Is.EqualTo("b"));
			Assert.False(Is.ReferenceEquals(role.Attributes, clone.Attributes));
			Assert.False(Is.ReferenceEquals(role.Categories, clone.Categories));
		}

		[Test]
		public void AttributesAreCaseInsensitive()
		{
			var role = new Entity();

			role.Attributes["TEST"] = "1";

			Assert.That(role.Attributes, Contains.Key("test"));
			Assert.That(role.Attributes["test"], Is.EqualTo("1"));
		}

		[Test]
		public void AttributesAreCaseInsensitiveWhenPassedWithout()
		{
			var dictionary = new Dictionary<string, string>
			{
				{ "TEST", "1" },
			};
			var role = new Entity("a", dictionary, null, null);

			Assert.That(role.Attributes, Contains.Key("test"));
			Assert.That(role.Attributes["test"], Is.EqualTo("1"));
		}

		[Test]
		public void AttributesAreCaseInsensitiveWhenPassedNullAttributes()
		{
			var role = new Entity("a", null, null, null);

			role.Attributes["TEST"] = "1";

			Assert.That(role.Attributes, Contains.Key("test"));
			Assert.That(role.Attributes["test"], Is.EqualTo("1"));
		}

		[Test]
		public void InventoryItemsAreCaseInsensitive()
		{
			var role = new Entity();

			role.InventoryItems["TEST"] = new Entity("a", null, null, null);

			Assert.That(role.InventoryItems, Contains.Key("test"));
			Assert.That(role.InventoryItems["test"].Name, Is.EqualTo("a"));
		}

		[Test]
		public void InventoryItemsAreCaseInsensitiveWhenPassedWithout()
		{
			var dictionary = new Dictionary<string, Entity>
			{
				{ "TEST", new Entity("a", null, null, null) },
			};

			var role = new Entity("a", null, null, dictionary);

			Assert.That(role.InventoryItems, Contains.Key("test"));
			Assert.That(role.InventoryItems["test"].Name, Is.EqualTo("a"));
		}

		[Test]
		public void InventoryItemsAreCaseInsensitiveWhenPassedNullAttributes()
		{
			var role = new Entity("a", null, null, null);

			role.InventoryItems["TEST"] = new Entity("a", null, null, null);

			Assert.That(role.InventoryItems, Contains.Key("test"));
			Assert.That(role.InventoryItems["test"].Name, Is.EqualTo("a"));
		}
	}
}
