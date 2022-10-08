using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TTRPG.Engine.Demo.Engine;
using TTRPG.Engine.Demo.Engine.CommandParsing;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.Demo
{
	public partial class CombatDemoForm : Form
	{
		private readonly GameObject _data;
		private readonly IEquationService _equationService;
		private readonly IInventoryService _inventoryService;
		private string _targetFilter;

		private IEnumerable<string> ListTargetNames(string category) => _data.GetLiveTargets(category).Select(x => x.Name);

		private void WriteMessage(string message) => txtBox_MessageLog.Text += $"{message}\r\n";

		private void UpdateTargets()
		{
			var targetFilter = cmb_TargetFilter.Text.Trim();
			list_Targets.Items.Clear();
			foreach (var target in ListTargetNames(targetFilter))
			{
				list_Targets.Items.Add(target);
			}
		}

		private void DisplayStatus()
		{
			var miner = _data.Roles.Single(r => r.Name.Equals("Miner", StringComparison.OrdinalIgnoreCase));
			var statusSequence = _data.Sequences.Single(s => s.Name.Equals("Status", StringComparison.OrdinalIgnoreCase));
			var result = _equationService.Process(statusSequence, null, new Role[] { miner.CloneAs("target") });
			txt_Status.Text = result.Results[0].Result;
		}

		public CombatDemoForm(IEquationService equationService, IInventoryService inventoryService, GameObject gameObject)
		{
			_equationService = equationService;
			_inventoryService = inventoryService;
			_data = gameObject;
			InitializeComponent();
			UpdateTargets();
			SetHelpText();
		}

		private void ProcessCommand()
		{
			txtBox_MessageLog.Clear();
			var command = txt_Command.Text;
			var commandParser = new CommandParser(command, _data);
			var parsedCommand = commandParser.Build(_equationService, _inventoryService);
			if (!parsedCommand.IsValid())
			{
				WriteMessage("Invalid Command.");
				txt_Command.Clear();
				return;
			}
			parsedCommand.Process(WriteMessage, _data);
			UpdateTargets();
			DisplayStatus();
		}

		private void btn_Perform_Click(object sender, EventArgs e)
		{
			ProcessCommand();
		}

		private void txt_Command_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				ProcessCommand();
			}
		}

		private void SetHelpText()
		{
			var examples = _data.Sequences.Select(s => s.Example);
			txt_Status.Lines = new string[] { "Help:" }
				.Union(examples)
				.Union(InventoryProcessor.GetInventoryCommandExamples())
				.ToArray();
		}

		private void btn_Help_Click(object sender, EventArgs e)
		{
			SetHelpText();
		}

		private void cmb_TargetFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			var target = cmb_TargetFilter.Text.Trim();
			if (_targetFilter != target)
			{
				target = _targetFilter;
				UpdateTargets();
			}
		}
	}
}
