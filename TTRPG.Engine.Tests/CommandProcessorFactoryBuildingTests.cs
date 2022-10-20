using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.CommandParsing.Parsers;
using TTRPG.Engine.CommandParsing.Processors;

namespace TTRPG.Engine.Tests
{
	[TestFixture(Category = "Unit")]
	[TestOf(typeof(CommandProcessorFactory))]
	public class CommandProcessorFactoryBuildingTests
	{
		Mock<ICommandParser> MatchParser;
		Mock<ICommandParser> DefaultParser;

		[SetUp]
		public void SetupTest()
		{
			MatchParser = null;
			DefaultParser = null;
		}

		ICommandProcessorFactory BuildCommandProcessorFactory()
		{
			var parsers = new List<ICommandParser>();
			if (MatchParser != null) parsers.Add(MatchParser.Object);
			if (DefaultParser != null) parsers.Add(DefaultParser.Object);
			return new CommandProcessorFactory(null, parsers);
		}

		Mock<ICommandParser> GetMockParser(bool isDefault, HashSet<string> supportedCommands)
		{
			var mockParser = new Mock<ICommandParser>();
			mockParser.Setup(x => x.IsDefault).Returns(isDefault);
			mockParser.Setup(x => x.CanProcess(It.Is<string>(s => supportedCommands.Contains(s)))).Returns(true);
			var mockProcessor = new Mock<ITTRPGCommandProcessor>();
			mockParser.Setup(x => x.GetProcessor(It.IsAny<ParsedCommand>())).Returns(mockProcessor.Object);

			return mockParser;
		}

		[Test]
		public void Build_ParserMatches_ParserIsChosen()
		{
			MatchParser = GetMockParser(false, new HashSet<string> { "action1" });
			var processor = BuildCommandProcessorFactory();
			var parsedCommand = new ParsedCommand();
			parsedCommand.MainCommand = "action1";

			var built = processor.Build(parsedCommand);

			Assert.That(built, Is.Not.Null);
			// command processor's GetProcessor command is called
			MatchParser.Verify(x => x.GetProcessor(It.Is<ParsedCommand>(parsedCommand => true)), Times.Once);
		}

		[Test]
		public void Build_NoMatchesWithDefault_DefaultIsChosen()
		{
			DefaultParser = GetMockParser(true, new HashSet<string>());
			var processor = BuildCommandProcessorFactory();
			var parsedCommand = new ParsedCommand();
			parsedCommand.MainCommand = "action1";

			var built = processor.Build(parsedCommand);

			Assert.That(built, Is.Not.Null);
			// command processor's GetProcessor command is called
			DefaultParser.Verify(x => x.GetProcessor(It.Is<ParsedCommand>(parsedCommand => true)), Times.Once);
		}

		[Test]
		public void Build_MatchesAndDefault_MatchChosen()
		{
			MatchParser = GetMockParser(false, new HashSet<string> { "action1" });
			DefaultParser = GetMockParser(true, new HashSet<string> { });
			var processor = BuildCommandProcessorFactory();
			var parsedCommand = new ParsedCommand();
			parsedCommand.MainCommand = "action1";

			var built = processor.Build(parsedCommand);

			Assert.That(built, Is.Not.Null);
			// ensure correct processor's GetProcessor command is called
			MatchParser.Verify(x => x.GetProcessor(It.Is<ParsedCommand>(parsedCommand => true)), Times.Once);
			DefaultParser.Verify(x => x.GetProcessor(It.Is<ParsedCommand>(parsedCommand => true)), Times.Never);
		}

		[Test]
		public void Build_NoMatchesNoDefault_Throws()
		{
			MatchParser = GetMockParser(false, new HashSet<string> { "action1" });
			var processor = BuildCommandProcessorFactory();
			var parsedCommand = new ParsedCommand();
			parsedCommand.MainCommand = "action2";

			Assert.Throws<Exception>(() => processor.Build(parsedCommand), "Unknown command.");
		}
	}
}
