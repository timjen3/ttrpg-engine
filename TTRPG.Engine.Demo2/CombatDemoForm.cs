using System;
using System.Windows.Forms;
using TTRPG.Engine.Demo.Engine;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.Demo2
{
	public partial class CombatDemoForm : Form
	{
		CombatDemoService _demo;
		private readonly IEquationService _equationService;
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

		public CombatDemoForm(IEquationService equationService, GameObject gameObject)
		{
			_equationService = equationService;
			_demo = new CombatDemoService(WriteMessage, gameObject);
			InitializeComponent();
			UpdateTargets();
			SetHelpText();
		}

		private void ProcessCommand()
		{
			txtBox_MessageLog.Clear();
			var command = txt_Command.Text;
			var equationParts = _demo.GetEquationPartsFromCommand(command);
			if (!equationParts.IsValid())
			{
				WriteMessage("Invalid Command.");
				txt_Command.Clear();
				return;
			}

			try
			{
				var result = _equationService.Process(equationParts.Sequence, equationParts.Inputs, equationParts.Roles);
				_demo.HandleResultItems(result);
				UpdateTargets();
				var statusParts = _demo.GetEquationPartsFromCommand("Status [miner:miner]");
				var status = _equationService.Process(statusParts.Sequence, statusParts.Inputs, statusParts.Roles);
				txt_Status.Text = status.Results[0].Result;
			}
			catch (Exception ex)
			{
				WriteMessage($"Failed to process command: '{command}'\n");
				WriteMessage(ex.Message);
				WriteMessage(ex.StackTrace);
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
			txt_Status.Lines = new string[]
			{
				"Try one of these commands:",
				"status [miner:miner]",
				"MineTerrain [miner:miner,dirtblock1:terrain]",
				"Rest [miner:miner]"
			};
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
