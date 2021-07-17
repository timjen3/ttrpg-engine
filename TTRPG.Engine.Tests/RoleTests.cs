using NUnit.Framework;
using System.Collections.Generic;

namespace TTRPG.Engine.Tests
{
	[TestFixture]
	[TestOf(typeof(Role))]
	public class RoleTests
	{
		[Test]
		public void CloneAs_CreatesADistinctClone()
		{
			var role = new Role();
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
			var role = new Role();

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
			var role = new Role("a", dictionary, null);

			Assert.That(role.Attributes, Contains.Key("test"));
			Assert.That(role.Attributes["test"], Is.EqualTo("1"));
		}

		[Test]
		public void AttributesAreCaseInsensitiveWhenPassedNullAttributes()
		{
			var role = new Role("a", null, new List<string>());

			role.Attributes["TEST"] = "1";

			Assert.That(role.Attributes, Contains.Key("test"));
			Assert.That(role.Attributes["test"], Is.EqualTo("1"));
		}
	}
}
