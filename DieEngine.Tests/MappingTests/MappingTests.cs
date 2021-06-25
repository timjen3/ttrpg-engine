using DieEngine.Exceptions;
using DieEngine.Mappings;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(Mapping))]
	internal class MappingTests
	{
		Mapping mapping;
		Dictionary<string, string> inputs;

		[SetUp]
		public void SetupTests()
		{
			mapping = new Mapping();
			inputs = new Dictionary<string, string>();
		}

		[Test]
		public void KeyIsMappedTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			inputs["a"] = "1";

			mapping.Apply("a", ref inputs, null);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void FromKeyIsKeptTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			inputs["a"] = "1";

			mapping.Apply("a", ref inputs, null);

			Assert.That(inputs, Contains.Key("a"));
			Assert.That(inputs["a"], Is.EqualTo("1"));
		}

		[Test]
		public void MappingIsPerformedWhenMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			inputs["a"] = "1";

			mapping.Apply("a", ref inputs, null);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void MappingIsNotPerformedWhenNotMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "b";
			inputs["a"] = "1";

			mapping.Apply("a", ref inputs, null);

			Assert.True(!inputs.ContainsKey("b"));
		}

		[Test]
		public void ThrowsWhenMissingKey()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "b";
			mapping.ThrowOnFailure = true;

			var ex = Assert.Throws<MappingFailedException>(() => mapping.Apply("b", ref inputs, null));

			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to missing key: '{mapping.From}'."));
		}
	}
}
