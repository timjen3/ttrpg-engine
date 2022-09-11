
namespace TTRPG.Engine.Demo2
{
	partial class CombatDemoForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtBox_MessageLog = new System.Windows.Forms.TextBox();
			this.btn_Attack = new System.Windows.Forms.Button();
			this.btn_UsePotion = new System.Windows.Forms.Button();
			this.pnl_Stats = new System.Windows.Forms.Panel();
			this.lbl_PlayerName = new System.Windows.Forms.Label();
			this.lbl_ComputerName = new System.Windows.Forms.Label();
			this.lbl_PlayerPotions = new System.Windows.Forms.Label();
			this.lbl_PlayerHP = new System.Windows.Forms.Label();
			this.lbl_ComputerPotions = new System.Windows.Forms.Label();
			this.lbl_ComputerHP = new System.Windows.Forms.Label();
			this.txt_PlayerPotions = new System.Windows.Forms.TextBox();
			this.txt_PlayerHP = new System.Windows.Forms.TextBox();
			this.txt_ComputerPotions = new System.Windows.Forms.TextBox();
			this.txt_ComputerHP = new System.Windows.Forms.TextBox();
			this.btn_NewGame = new System.Windows.Forms.Button();
			this.list_Targets = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.pnl_Stats.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtBox_MessageLog
			// 
			this.txtBox_MessageLog.Location = new System.Drawing.Point(31, 186);
			this.txtBox_MessageLog.Multiline = true;
			this.txtBox_MessageLog.Name = "txtBox_MessageLog";
			this.txtBox_MessageLog.ReadOnly = true;
			this.txtBox_MessageLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtBox_MessageLog.Size = new System.Drawing.Size(414, 283);
			this.txtBox_MessageLog.TabIndex = 0;
			// 
			// btn_Attack
			// 
			this.btn_Attack.Location = new System.Drawing.Point(461, 340);
			this.btn_Attack.Name = "btn_Attack";
			this.btn_Attack.Size = new System.Drawing.Size(316, 39);
			this.btn_Attack.TabIndex = 1;
			this.btn_Attack.Text = "Attack";
			this.btn_Attack.UseVisualStyleBackColor = true;
			this.btn_Attack.Click += new System.EventHandler(this.btn_Attack_Click);
			// 
			// btn_UsePotion
			// 
			this.btn_UsePotion.Location = new System.Drawing.Point(461, 385);
			this.btn_UsePotion.Name = "btn_UsePotion";
			this.btn_UsePotion.Size = new System.Drawing.Size(316, 38);
			this.btn_UsePotion.TabIndex = 2;
			this.btn_UsePotion.Text = "Use Potion";
			this.btn_UsePotion.UseVisualStyleBackColor = true;
			this.btn_UsePotion.Click += new System.EventHandler(this.btn_UsePotion_Click);
			// 
			// pnl_Stats
			// 
			this.pnl_Stats.Controls.Add(this.lbl_PlayerName);
			this.pnl_Stats.Controls.Add(this.lbl_ComputerName);
			this.pnl_Stats.Controls.Add(this.lbl_PlayerPotions);
			this.pnl_Stats.Controls.Add(this.lbl_PlayerHP);
			this.pnl_Stats.Controls.Add(this.lbl_ComputerPotions);
			this.pnl_Stats.Controls.Add(this.lbl_ComputerHP);
			this.pnl_Stats.Controls.Add(this.txt_PlayerPotions);
			this.pnl_Stats.Controls.Add(this.txt_PlayerHP);
			this.pnl_Stats.Controls.Add(this.txt_ComputerPotions);
			this.pnl_Stats.Controls.Add(this.txt_ComputerHP);
			this.pnl_Stats.Location = new System.Drawing.Point(461, 22);
			this.pnl_Stats.Name = "pnl_Stats";
			this.pnl_Stats.Size = new System.Drawing.Size(316, 312);
			this.pnl_Stats.TabIndex = 4;
			// 
			// lbl_PlayerName
			// 
			this.lbl_PlayerName.AutoSize = true;
			this.lbl_PlayerName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.lbl_PlayerName.Location = new System.Drawing.Point(246, 164);
			this.lbl_PlayerName.Name = "lbl_PlayerName";
			this.lbl_PlayerName.Size = new System.Drawing.Size(52, 20);
			this.lbl_PlayerName.TabIndex = 9;
			this.lbl_PlayerName.Text = "Player";
			this.lbl_PlayerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbl_ComputerName
			// 
			this.lbl_ComputerName.AutoSize = true;
			this.lbl_ComputerName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.lbl_ComputerName.Location = new System.Drawing.Point(241, 28);
			this.lbl_ComputerName.Name = "lbl_ComputerName";
			this.lbl_ComputerName.Size = new System.Drawing.Size(54, 20);
			this.lbl_ComputerName.TabIndex = 8;
			this.lbl_ComputerName.Text = "Target";
			this.lbl_ComputerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbl_PlayerPotions
			// 
			this.lbl_PlayerPotions.AutoSize = true;
			this.lbl_PlayerPotions.Location = new System.Drawing.Point(17, 224);
			this.lbl_PlayerPotions.Name = "lbl_PlayerPotions";
			this.lbl_PlayerPotions.Size = new System.Drawing.Size(57, 20);
			this.lbl_PlayerPotions.TabIndex = 7;
			this.lbl_PlayerPotions.Text = "Potions";
			// 
			// lbl_PlayerHP
			// 
			this.lbl_PlayerHP.AutoSize = true;
			this.lbl_PlayerHP.Location = new System.Drawing.Point(17, 164);
			this.lbl_PlayerHP.Name = "lbl_PlayerHP";
			this.lbl_PlayerHP.Size = new System.Drawing.Size(28, 20);
			this.lbl_PlayerHP.TabIndex = 6;
			this.lbl_PlayerHP.Text = "HP";
			// 
			// lbl_ComputerPotions
			// 
			this.lbl_ComputerPotions.AutoSize = true;
			this.lbl_ComputerPotions.Location = new System.Drawing.Point(17, 83);
			this.lbl_ComputerPotions.Name = "lbl_ComputerPotions";
			this.lbl_ComputerPotions.Size = new System.Drawing.Size(57, 20);
			this.lbl_ComputerPotions.TabIndex = 5;
			this.lbl_ComputerPotions.Text = "Potions";
			// 
			// lbl_ComputerHP
			// 
			this.lbl_ComputerHP.AutoSize = true;
			this.lbl_ComputerHP.Location = new System.Drawing.Point(17, 28);
			this.lbl_ComputerHP.Name = "lbl_ComputerHP";
			this.lbl_ComputerHP.Size = new System.Drawing.Size(28, 20);
			this.lbl_ComputerHP.TabIndex = 4;
			this.lbl_ComputerHP.Text = "HP";
			// 
			// txt_PlayerPotions
			// 
			this.txt_PlayerPotions.Location = new System.Drawing.Point(17, 247);
			this.txt_PlayerPotions.Name = "txt_PlayerPotions";
			this.txt_PlayerPotions.Size = new System.Drawing.Size(278, 27);
			this.txt_PlayerPotions.TabIndex = 3;
			this.txt_PlayerPotions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txt_PlayerHP
			// 
			this.txt_PlayerHP.Location = new System.Drawing.Point(17, 187);
			this.txt_PlayerHP.Name = "txt_PlayerHP";
			this.txt_PlayerHP.Size = new System.Drawing.Size(278, 27);
			this.txt_PlayerHP.TabIndex = 2;
			this.txt_PlayerHP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txt_ComputerPotions
			// 
			this.txt_ComputerPotions.Location = new System.Drawing.Point(17, 106);
			this.txt_ComputerPotions.Name = "txt_ComputerPotions";
			this.txt_ComputerPotions.Size = new System.Drawing.Size(278, 27);
			this.txt_ComputerPotions.TabIndex = 1;
			this.txt_ComputerPotions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txt_ComputerHP
			// 
			this.txt_ComputerHP.Location = new System.Drawing.Point(17, 51);
			this.txt_ComputerHP.Name = "txt_ComputerHP";
			this.txt_ComputerHP.Size = new System.Drawing.Size(278, 27);
			this.txt_ComputerHP.TabIndex = 0;
			this.txt_ComputerHP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btn_NewGame
			// 
			this.btn_NewGame.Location = new System.Drawing.Point(461, 430);
			this.btn_NewGame.Name = "btn_NewGame";
			this.btn_NewGame.Size = new System.Drawing.Size(316, 39);
			this.btn_NewGame.TabIndex = 5;
			this.btn_NewGame.Text = "New Game";
			this.btn_NewGame.UseVisualStyleBackColor = true;
			this.btn_NewGame.Visible = false;
			this.btn_NewGame.Click += new System.EventHandler(this.btn_NewGame_Click);
			// 
			// list_Targets
			// 
			this.list_Targets.FormattingEnabled = true;
			this.list_Targets.ItemHeight = 20;
			this.list_Targets.Location = new System.Drawing.Point(31, 45);
			this.list_Targets.Name = "list_Targets";
			this.list_Targets.Size = new System.Drawing.Size(414, 124);
			this.list_Targets.TabIndex = 6;
			this.list_Targets.SelectedIndexChanged += new System.EventHandler(this.list_Targets_SelectedIndexChanged);
			this.list_Targets.DoubleClick += new System.EventHandler(this.list_Targets_DoubleClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(202, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 20);
			this.label1.TabIndex = 7;
			this.label1.Text = "Enemies";
			// 
			// CombatDemoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(797, 486);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.list_Targets);
			this.Controls.Add(this.btn_NewGame);
			this.Controls.Add(this.pnl_Stats);
			this.Controls.Add(this.btn_UsePotion);
			this.Controls.Add(this.btn_Attack);
			this.Controls.Add(this.txtBox_MessageLog);
			this.Name = "CombatDemoForm";
			this.Text = "CombatDemo";
			this.pnl_Stats.ResumeLayout(false);
			this.pnl_Stats.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtBox_MessageLog;
		private System.Windows.Forms.Button btn_Attack;
		private System.Windows.Forms.Button btn_UsePotion;
		private System.Windows.Forms.Panel pnl_Stats;
		private System.Windows.Forms.Label lbl_PlayerName;
		private System.Windows.Forms.Label lbl_ComputerName;
		private System.Windows.Forms.Label lbl_PlayerPotions;
		private System.Windows.Forms.Label lbl_PlayerHP;
		private System.Windows.Forms.Label lbl_ComputerPotions;
		private System.Windows.Forms.Label lbl_ComputerHP;
		private System.Windows.Forms.TextBox txt_PlayerPotions;
		private System.Windows.Forms.TextBox txt_PlayerHP;
		private System.Windows.Forms.TextBox txt_ComputerPotions;
		private System.Windows.Forms.TextBox txt_ComputerHP;
		private System.Windows.Forms.Button btn_NewGame;
		private System.Windows.Forms.ListBox list_Targets;
		private System.Windows.Forms.Label label1;
	}
}