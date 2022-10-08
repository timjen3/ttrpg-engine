using System;
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
		CombatDemoService _demo;
		private readonly IEquationService _equationService;
		private readonly IInventoryService _inventoryService;
		private string _targetFilter;

		private void WriteMessage(string message) => txtBox_MessageLog.Text += $"{message}\r\n";

		private void UpdateTargets()
		{
			var targetFilter = cmb_TargetFilter.Text.Trim();
			list_Targets.Items.Clear();
			foreach (var target in _demo.ListTargetNames(targetFilter))
			{
				list_Targets.Items.Add(target);
			}
		}

		public CombatDemoForm(IEquationService equationService, IInventoryService inventoryService, GameObject gameObject)
		{
			_equationService = equationService;
			_inventoryService = inventoryService;
			_demo = new CombatDemoService(WriteMessage, gameObject);
			InitializeComponent();
			UpdateTargets();
			SetHelpText();
		}

		private void ProcessCommand()
		{
			txtBox_MessageLog.Clear();
			var command = txt_Command.Text;
			var parsedCommand = _demo.ParseMainCommand(command);
			if (parsedCommand.CommandType == TTRPGCommandType.Equation)
			{
				var equationParts = _demo.GetEquationPartsFromCommand(parsedCommand);
				if (!equationParts.IsValid())
				{
					WriteMessage("Invalid Command.");
					txt_Command.Clear();
					return;
				}
				try
				{
					var result = equationParts.Process(_equationService);
					_demo.HandleResultItems(result);
					UpdateTargets();
					var statusParts = _demo.ParseMainCommand("Status [miner:target]");
					var statusEquationParts = _demo.GetEquationPartsFromCommand(statusParts);
					var status = _equationService.Process(statusEquationParts.Sequence, statusEquationParts.Inputs, statusEquationParts.Roles);
					txt_Status.Text = status.Results[0].Result;
				}
				catch (Exception ex)
				{
					WriteMessage($"Failed to process command: '{command}'\n");
					WriteMessage(ex.Message);
					WriteMessage(ex.StackTrace);
				}
			}
			else if (parsedCommand.CommandType == TTRPGCommandType.Inventory)
			{
				var inventoryParts = _demo.GetInventoryPartsFromCommand(parsedCommand);
				var message = inventoryParts.Process(_inventoryService);
				WriteMessage(message);
				txt_Command.Clear();
			}
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
			var examples = _demo.Data.Sequences.Select(s => s.Example);
			txt_Status.Lines = new string[] { "Help:" }
				.Union(examples)
				.Union(InventoryParts.GetInventoryCommandExamples())
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
