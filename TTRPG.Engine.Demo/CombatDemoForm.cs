using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TTRPG.Engine.Demo
{
	public partial class CombatDemoForm : Form
	{
		private readonly GameObject _data;
		private readonly TTRPGEngine _engine;
		private string _targetFilter;

		private IEnumerable<string> ListTargetNames(string category) => _data.GetLiveTargets(category).Select(x => x.Name);

		private void WriteMessage(object sender, string message) => txtBox_MessageLog.Text += $"{message}\r\n";

		private bool TargetsChanged()
		{
			var currentItems = list_Targets.Items.Cast<string>().ToArray();
			var updatedItems = ListTargetNames(cmb_TargetFilter.Text.Trim()).ToArray();
			if (currentItems.Length != updatedItems.Length)
				return true;

			for (int i = 0; i < updatedItems.Length; i++)
			{
				if (updatedItems[i] != currentItems[i])
					return true;
			}

			return false;
		}

		private void UpdateTargets()
		{
			if (!TargetsChanged()) return;

			list_Targets.BeginUpdate();
			var targetFilter = cmb_TargetFilter.Text.Trim();
			list_Targets.Items.Clear();
			foreach (var target in ListTargetNames(targetFilter))
			{
				list_Targets.Items.Add(target);
			}
			list_Targets.EndUpdate();
		}

		private void DisplayStatus()
		{
			txt_Status.SuspendLayout();
			var statusUpdate = _engine.Process("Status [miner:target]", false);
			txt_Status.Text = statusUpdate.FirstOrDefault();
			txt_Status.PerformLayout();
		}

		public CombatDemoForm(GameObject gameObject, TTRPGEngine engine)
		{
			_data = gameObject;
			_engine = engine;
			engine.MessageCreated += new EventHandler<string>(WriteMessage);
			InitializeComponent();
			UpdateTargets();
			SetHelpText();
		}

		private void ProcessCommand()
		{
			txtBox_MessageLog.SuspendLayout();
			txtBox_MessageLog.Clear();
			var command = txt_Command.Text;
			_engine.Process(command, true);
			UpdateTargets();
			DisplayStatus();
			txtBox_MessageLog.PerformLayout();
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
			txt_Status.SuspendLayout();
			txt_Status.Lines = new string[] { "Help:" }
				.Union(_engine.GetExampleCommands())
				.ToArray();
			txt_Status.ResumeLayout();
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
