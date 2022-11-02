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
        List<Role> Roles;
        List<ICommandParser> Parsers;

        [SetUp]
        public void SetupTest()
        {
            Sequences = new List<Sequence>();
            SequenceItems = new List<SequenceItem>();
            Roles = new List<Role>();
            Parsers = new List<ICommandParser>();
        }

        ICommandProcessorFactory BuildCommandProcessorFactory()
        {
            var mockLoader = new Mock<ITTRPGDataRepository>();
            mockLoader.Setup(x => x.GetSequencesAsync()).ReturnsAsync(Sequences);
            mockLoader.Setup(x => x.GetSequenceItemsAsync()).ReturnsAsync(SequenceItems);
            mockLoader.Setup(x => x.GetRolesAsync()).ReturnsAsync(Roles);
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
            Assert.That(parsed.Roles, Is.Empty);
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
        public void ParseCommand_WithRoles_SetsRoles()
        {
            var role = new Role
            {
                Name = "rolea"
            };
            Roles.Add(role);
            var processor = BuildCommandProcessorFactory();

            var parsed = processor.ParseCommand("[rolea:aliasb]");

            Assert.That(parsed.Roles[0].Name, Is.EqualTo("rolea"));
            Assert.That(parsed.Roles[0].Alias, Is.EqualTo("aliasb"));
            Assert.That(parsed.Roles[0], Is.Not.SameAs(role));
        }
    }
}
