using DieEngine.Exceptions;
using DieEngine.Mappings;
using NUnit.Framework;
using System.Collections.Generic;

namespace DieEngine.Tests
{
	[TestFixture]
	[TestOf(typeof(Mapping))]
	internal class MappingTests
	{
		Mapping mapping;
		Dictionary<string, double> inputs;

		[SetUp]
		public void SetupTests()
		{
			mapping = new Mapping();
			inputs = new Dictionary<string, double>();
		}

		[Test]
		public void KeyIsMappedTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			inputs["a"] = 1;

			mapping.Apply(0, ref inputs, null);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo(1));
		}

		[Test]
		public void FromKeyIsKeptTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			inputs["a"] = 1;

			mapping.Apply(0, ref inputs, null);

			Assert.That(inputs, Contains.Key("a"));
			Assert.That(inputs["a"], Is.EqualTo(1));
		}

		[Test]
		public void MappingIsPerformedWhenOrderMatches()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.Order = 0;
			inputs["a"] = 1;

			mapping.Apply(0, ref inputs, null);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo(1));
		}

		[Test]
		public void MappingIsNotPerformedWhenOrderDoesNotMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.Order = 1;
			inputs["a"] = 1;

			mapping.Apply(0, ref inputs, null);

			Assert.True(!inputs.ContainsKey("b"));
		}

		[Test]
		public void ThrowsWhenMissingKey()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.Order = 1;
			mapping.ThrowOnFailure = true;

			var ex = Assert.Throws<MappingFailedException>(() => mapping.Apply(1, ref inputs, null));

			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to missing key: '{mapping.From}'."));
		}
	}
}
