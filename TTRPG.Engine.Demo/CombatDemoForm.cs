using System;
using System.Windows.Forms;
using TTRPG.Engine.Demo.Engine;
using TTRPG.Engine.Equations;

namespace TTRPG.Engine.Demo2
{
	public partial class CombatDemoForm : Form
	{
		CombatDemoService _demo;

		private void WriteMessage(string message) => txtBox_MessageLog.Text += $"{message}\r\n";

		private void EndGame()
		{
			WriteMessage("Game over.");
			btn_Attack.Enabled = false;
			btn_UsePotion.Enabled = false;
			btn_NewGame.Visible = true;
		}

		public CombatDemoForm(IEquationService equationService, GameObject gameObject)
		{
			_demo = new CombatDemoService(WriteMessage, equationService, gameObject);
			InitializeComponent();
			foreach (var target in _demo.ListTargetNames())
			{
				list_Targets.Items.Add(target);
			}
			UpdateState();
		}

		private void UpdateState()
		{
			txt_ComputerHP.Text = _demo.ComputerHPStatus;
			txt_ComputerPotions.Text = _demo.ComputerPotions;
			txt_PlayerHP.Text = _demo.PlayerHPStatus;
			txt_PlayerPotions.Text = _demo.PlayerPotions;
			btn_Attack.Enabled = !_demo.IsGameOver() && _demo.CheckPlayerAttack();
			btn_UsePotion.Enabled = !_demo.IsGameOver() && _demo.CheckPlayerUsePotion();
		}

		private void btn_Attack_Click(object sender, EventArgs e)
		{
			txtBox_MessageLog.Clear();
			_demo.PlayerAttack();
			UpdateState();
			if (_demo.IsGameOver()) EndGame();
		}

		private void btn_UsePotion_Click(object sender, EventArgs e)
		{
			txtBox_MessageLog.Clear();
			_demo.PlayerUsePotion();
			UpdateState();
			if (_demo.IsGameOver()) EndGame();
		}

		private void btn_NewGame_Click(object sender, EventArgs e)
		{
			txtBox_MessageLog.Clear();
			_demo.Data.NewGame();
			btn_NewGame.Visible = false;
			UpdateState();
		}

		private void list_Targets_SelectedIndexChanged(object sender, EventArgs e)
		{
			var targetName = list_Targets.SelectedItem?.ToString();
			if (!string.IsNullOrWhiteSpace(targetName))
			{
				_demo.SetTarget(targetName);
				UpdateState();
			}
		}
	}
}
