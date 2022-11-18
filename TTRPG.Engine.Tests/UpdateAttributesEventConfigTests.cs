using System.Collections.Generic;
using NUnit.Framework;
using TTRPG.Engine.Engine.Events;
using TTRPG.Engine.Equations;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.Tests
{
	[TestFixture]
	[TestOf(typeof(UpdateAttributesEventConfig))]
	public class UpdateAttributesEventConfigTests
	{
		EquationService EquationService;
		Dictionary<string, string> Inputs;
		List<Entity> Entities;

		[SetUp]
		public void SetupTest()
		{
			EquationService = new EquationService(null);
			Inputs = new Dictionary<string, string>();
			Entities = new List<Entity>();
			Entities.Add(new Entity
			{
				Name = "name1",
				Alias = "alias1"
			});
			Entities[0].Attributes["a"] = "0";
		}

		[Test]
		public void ProcessResults_KeyFoundInResults_EventAdded()
		{
			var item = new UpdateAttributesEventConfig();
			item.AttributeName = "a";
			item.EventType = TTRPGEventType.UpdateAttributes;
			item.Source = "new_a";
			item.EntityAlias = "alias1";
			Inputs["new_a"] = "1";

			var resultItems = EquationService.ProcessResults(new EventConfig[] { item }, Inputs, Entities);

			Assert.That(resultItems, Has.Count.EqualTo(1));
			Assert.That(resultItems[0], Is.TypeOf<UpdateAttributesEvent>());
			var updateAttributesResult = (UpdateAttributesEvent)resultItems[0];
			Assert.That(updateAttributesResult.Category, Is.EqualTo(TTRPGEventType.UpdateAttributes));
			Assert.That(updateAttributesResult.OldValue, Is.EqualTo("0"));
			Assert.That(updateAttributesResult.NewValue, Is.EqualTo("1"));
			Assert.That(updateAttributesResult.AttributeToUpdate, Is.EqualTo("a"));
		}

		[Test]
		public void ProcessResults_KeyNotFoundInResults_EventNotAdded()
		{
			var item = new UpdateAttributesEventConfig();
			item.AttributeName = "a";
			item.EventType = TTRPGEventType.UpdateAttributes;
			item.Source = "new_a";
			item.EntityAlias = "alias1";
			// input not added

			var resultItems = EquationService.ProcessResults(new EventConfig[] { item }, Inputs, Entities);

			Assert.That(resultItems, Has.Count.EqualTo(0));
		}
	}
}
