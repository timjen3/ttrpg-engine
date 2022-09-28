using NUnit.Framework;
using System.Collections.Generic;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Exceptions;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(Mapping))]
	internal class MappingTests
	{
		Mapping mapping;
		Dictionary<string, string> inputs;
		EquationService equationService;

		[SetUp]
		public void SetupTests()
		{
			mapping = new Mapping();
			mapping.MappingType = MappingType.Input;
			inputs = new Dictionary<string, string>();
			equationService = new EquationService(null);
		}

		[Test]
		public void Apply_KeyIsMappedTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			inputs["a"] = "1";

			equationService.Apply(mapping, "a", ref inputs, null);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_FromKeyIsKeptTest()
		{
			mapping.From = "a";
			mapping.To = "b";
			inputs["a"] = "1";

			equationService.Apply(mapping, "a", ref inputs, null);

			Assert.That(inputs, Contains.Key("a"));
			Assert.That(inputs["a"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_MappingIsPerformedWhenMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "a";
			inputs["a"] = "1";

			equationService.Apply(mapping, "a", ref inputs, null);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_MappingIsNotPerformedWhenNotMatch()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "b";
			inputs["a"] = "1";

			equationService.Apply(mapping, "a", ref inputs, null);

			Assert.False(inputs.ContainsKey("b"));
		}

		[Test]
		public void Apply_DoesNotThrowWhenMappingSucceeds()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "b";
			mapping.ThrowOnFailure = true;
			inputs["a"] = "1";

			equationService.Apply(mapping, "b", ref inputs, null);
		}

		[Test]
		public void Apply_ThrowsWhenMissingKey()
		{
			mapping.From = "a";
			mapping.To = "b";
			mapping.ItemName = "b";
			mapping.ThrowOnFailure = true;

			var ex = Assert.Throws<MappingFailedException>(() => equationService.Apply(mapping, "b", ref inputs, null));

			Assert.That(ex.Message, Is.EqualTo($"Mapping failed due to missing key: '{mapping.From}'."));
		}

		[Test]
		public void Apply_MappingFromSupportsFormatString()
		{
			mapping.From = "{rename}";
			mapping.To = "b";
			mapping.ItemName = "a";
			inputs["rename"] = "a";
			inputs["a"] = "1";

			equationService.Apply(mapping, "a", ref inputs, null);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}

		[Test]
		public void Apply_MappingToSupportsFormatString()
		{
			mapping.From = "a";
			mapping.To = "{rename}";
			mapping.ItemName = "a";
			inputs["rename"] = "b";
			inputs["a"] = "1";

			equationService.Apply(mapping, "a", ref inputs, null);

			Assert.That(inputs, Contains.Key("b"));
			Assert.That(inputs["b"], Is.EqualTo("1"));
		}
	}
}
