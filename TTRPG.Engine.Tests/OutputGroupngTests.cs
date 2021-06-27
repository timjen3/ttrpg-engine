using NUnit.Framework;
using System.Collections.Generic;
using TTRPG.Engine.Exceptions;
using TTRPG.Engine.OutputGroupings;

namespace TTRPG.Engine.Tests
{
	[TestFixture]
	[TestOf(typeof(OutputGrouping<string>))]
	class OutputGroupngTests
	{
		OutputGrouping<string> OutputGrouping = new OutputGrouping<string>()
		{
			Name = ""
		};

		[Test]
		public void GetResult_MappingFound_ResultContainsItem()
		{
			OutputGrouping.Items = new List<OutputGroupingItem>
			{
				new OutputGroupingItem("a", "as")
			};
			var inputs = new Dictionary<string, string>
			{
				{ "as", "1" }
			};

			var result = OutputGrouping.GetResult(inputs);

			Assert.That(result.Results.ContainsKey("a"));
			Assert.That(result.Results["a"], Is.EqualTo("1"));
		}

		[Test]
		public void GetResult_MappingsFound_ResultContainsItems()
		{
			OutputGrouping.Items = new List<OutputGroupingItem>
			{
				new OutputGroupingItem("a", "as"),
				new OutputGroupingItem("b", "bs")
			};
			var inputs = new Dictionary<string, string>
			{
				{ "as", "1" },
				{ "bs", "1" },
			};

			var result = OutputGrouping.GetResult(inputs);

			Assert.That(result.Results.ContainsKey("a"));
			Assert.That(result.Results["a"], Is.EqualTo("1"));
			Assert.That(result.Results.ContainsKey("b"));
			Assert.That(result.Results["b"], Is.EqualTo("1"));
		}

		[Test]
		public void GetResult_MappingNotFound_ResultContainsNullItem()
		{
			OutputGrouping.Items = new List<OutputGroupingItem>
			{
				new OutputGroupingItem("a", "as"),
			};
			var inputs = new Dictionary<string, string>
			{
			};

			var result = OutputGrouping.GetResult(inputs);

			Assert.That(result.Results.ContainsKey("a"));
			Assert.That(result.Results["a"], Is.Null);
		}

		[Test]
		public void GetResult_MappingsNotFound_ResultContainsNullItems()
		{
			OutputGrouping.Items = new List<OutputGroupingItem>
			{
				new OutputGroupingItem("a", "as"),
				new OutputGroupingItem("b", "bs")
			};
			var inputs = new Dictionary<string, string>
			{
			};

			var result = OutputGrouping.GetResult(inputs);

			Assert.That(result.Results.ContainsKey("a"));
			Assert.That(result.Results["a"], Is.Null);
			Assert.That(result.Results.ContainsKey("b"));
			Assert.That(result.Results["b"], Is.Null);
		}

		[Test]
		public void GetResult_MappingNotFoundThrow_Throws()
		{
			OutputGrouping.Items = new List<OutputGroupingItem>
			{
				new OutputGroupingItem("a", "as", true),
			};
			var inputs = new Dictionary<string, string>
			{
			};

			Assert.Throws<MissingOutputGroupingItemException>(() => OutputGrouping.GetResult(inputs));
		}

		[Test]
		public void GetResult_HasPayload_ResultHasPayload()
		{
			OutputGrouping.Payload = "this is a test";

			var result = OutputGrouping.GetResult(null);

			Assert.That(result, Is.TypeOf<OutputGroupingResult<string>>());
			Assert.That(((OutputGroupingResult<string>)result).Payload, Is.EqualTo("this is a test"));
		}
	}
}
