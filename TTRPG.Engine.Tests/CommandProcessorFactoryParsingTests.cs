using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.CommandParsing.Parsers;
using TTRPG.Engine.Data.TtrpgDataLoaders;
using TTRPG.Engine.SequenceItems;
using TTRPG.Engine.Sequences;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(CommandProcessorFactory))]
	public class CommandProcessorFactoryParsingTests
	{
		List<Sequence> Sequences;
		List<SequenceItem> SequenceItems;
		List<Entity> Entities;
		List<ICommandParser> Parsers;

		[SetUp]
		public void SetupTest()
		{
			Sequences = new List<Sequence>();
			SequenceItems = new List<SequenceItem>();
			Entities = new List<Entity>();
			Parsers = new List<ICommandParser>();
		}

		ICommandProcessorFactory BuildCommandProcessorFactory()
		{
			var mockLoader = new Mock<ITTRPGDataRepository>();
			mockLoader.Setup(x => x.GetSequencesAsync()).ReturnsAsync(Sequences);
			mockLoader.Setup(x => x.GetSequenceItemsAsync()).ReturnsAsync(SequenceItems);
			mockLoader.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(Entities);
			var gameObject = new GameObject(mockLoader.Object);

			return new CommandProcessorFactory(gameObject, Parsers);
		}

		[Test]
		public void ParseCommand_WithEmptyCommand_AllNull()
		{
			var processor = BuildCommandProcessorFactory();

			var parsed = processor.ParseCommand("");

			Assert.That(parsed.MainCommand, Is.Null);
			Assert.That(parsed.Inputs, Is.Empty);
			Assert.That(parsed.Entities, Is.Empty);
		}

		[Test]
		public void ParseCommand_WithCommand_SetsMainCommand()
		{
			var processor = BuildCommandProcessorFactory();

			var parsed = processor.ParseCommand("commandName");

			Assert.That(parsed.MainCommand, Is.EqualTo("commandName"));
		}

		[Test]
		public void ParseCommand_WithInputs_SetsInputs()
		{
			var processor = BuildCommandProcessorFactory();

			var parsed = processor.ParseCommand("{inputa:1,inputb:2}");

			Assert.That(parsed.Inputs, Is.EquivalentTo(new Dictionary<string, string>
			{
				{ "inputa", "1" },
				{ "inputb", "2" }
			}));
		}

		[Test]
		public void ParseCommand_WithEntities_SetsEntities()
		{
			var entity = new Entity
			{
				Name = "entitya"
			};
			Entities.Add(entity);
			var processor = BuildCommandProcessorFactory();

			var parsed = processor.ParseCommand("[entitya:aliasb]");

			Assert.That(parsed.Entities[0].Name, Is.EqualTo("entitya"));
			Assert.That(parsed.Entities[0].Alias, Is.EqualTo("aliasb"));
			Assert.That(parsed.Entities[0], Is.Not.SameAs(entity));
		}
	}
}
