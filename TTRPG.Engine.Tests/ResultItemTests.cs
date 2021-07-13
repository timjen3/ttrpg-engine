using NUnit.Framework;
using System.Collections.Generic;
using TTRPG.Engine.Equations;
using TTRPG.Engine.SequenceItems;

namespace TTRPG.Engine.Tests
{
	[TestFixture]
	[TestOf(typeof(ResultItem))]
	public class ResultItemTests
	{
		EquationService EquationService;
		Dictionary<string, string> Inputs;
		List<Role> Roles;

		[SetUp]
		public void SetupTest()
		{
			EquationService = new EquationService(null);
			Inputs = new Dictionary<string, string>();
			Roles = new List<Role>();
		}

		[Test]
		public void ProcessResults_KeyFoundInResults_Added()
		{
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";
			Inputs["c"] = "1";

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Roles);

			Assert.That(resultItems, Has.Count.EqualTo(1));
			Assert.That(resultItems[0].Result, Is.EqualTo("1"));
		}

		[Test]
		public void ProcessResults_KeyMissingFromResults_NotAdded()
		{
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Roles);

			Assert.That(resultItems, Is.Empty);
		}

		[Test]
		public void ProcessResults_KeyFoundInResultsWithRole_AddedRoleSet()
		{
			var item = new ResultItem();
			item.Name = "a";
			item.Category = "b";
			item.Source = "c";
			Inputs["c"] = "1";
			item.RoleName = "d";
			Roles.Add(new Role("d", null, null));

			var resultItems = EquationService.ProcessResults(new ResultItem[] { item }, Inputs, Roles);

			Assert.That(resultItems, Has.Count.EqualTo(1));
			Assert.AreSame(Roles[0], resultItems[0].Role);
		}
	}
}
