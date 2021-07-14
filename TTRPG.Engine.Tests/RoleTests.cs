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
			role.Attributes = new Dictionary<string, string>();
			role.Categories = new List<string>();

			var clone = role.CloneAs("b");

			Assert.False(Is.ReferenceEquals(role, clone));
			Assert.That(role.Name, Is.EqualTo(clone.Name));
			Assert.That(role.Name, Is.EqualTo("a"));
			Assert.That(clone.Name, Is.EqualTo("a"));
			Assert.That(role.Alias, Is.Null);
			Assert.That(clone.Alias, Is.EqualTo("b"));
			Assert.False(Is.ReferenceEquals(role.Attributes, clone.Attributes));
			Assert.False(Is.ReferenceEquals(role.Categories, clone.Categories));
		}
	}
}
