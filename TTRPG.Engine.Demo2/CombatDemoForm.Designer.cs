
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
			this.list_Targets = new System.Windows.Forms.ListBox();
			this.txt_Command = new System.Windows.Forms.TextBox();
			this.btn_Perform = new System.Windows.Forms.Button();
			this.txt_Status = new System.Windows.Forms.TextBox();
			this.btn_Help = new System.Windows.Forms.Button();
			this.cmb_TargetFilter = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// txtBox_MessageLog
			// 
			this.txtBox_MessageLog.Location = new System.Drawing.Point(31, 282);
			this.txtBox_MessageLog.Multiline = true;
			this.txtBox_MessageLog.Name = "txtBox_MessageLog";
			this.txtBox_MessageLog.ReadOnly = true;
			this.txtBox_MessageLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtBox_MessageLog.Size = new System.Drawing.Size(720, 148);
			this.txtBox_MessageLog.TabIndex = 0;
			// 
			// list_Targets
			// 
			this.list_Targets.FormattingEnabled = true;
			this.list_Targets.ItemHeight = 20;
			this.list_Targets.Location = new System.Drawing.Point(31, 61);
			this.list_Targets.Name = "list_Targets";
			this.list_Targets.Size = new System.Drawing.Size(361, 204);
			this.list_Targets.TabIndex = 6;
			// 
			// txt_Command
			// 
			this.txt_Command.Location = new System.Drawing.Point(31, 449);
			this.txt_Command.Name = "txt_Command";
			this.txt_Command.Size = new System.Drawing.Size(619, 27);
			this.txt_Command.TabIndex = 8;
			this.txt_Command.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Command_KeyPress);
			// 
			// btn_Perform
			// 
			this.btn_Perform.Location = new System.Drawing.Point(657, 447);
			this.btn_Perform.Name = "btn_Perform";
			this.btn_Perform.Size = new System.Drawing.Size(94, 29);
			this.btn_Perform.TabIndex = 9;
			this.btn_Perform.Text = "Perform";
			this.btn_Perform.UseVisualStyleBackColor = true;
			this.btn_Perform.Click += new System.EventHandler(this.btn_Perform_Click);
			// 
			// txt_Status
			// 
			this.txt_Status.Location = new System.Drawing.Point(407, 61);
			this.txt_Status.Multiline = true;
			this.txt_Status.Name = "txt_Status";
			this.txt_Status.ReadOnly = true;
			this.txt_Status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txt_Status.Size = new System.Drawing.Size(344, 204);
			this.txt_Status.TabIndex = 10;
			// 
			// btn_Help
			// 
			this.btn_Help.Location = new System.Drawing.Point(527, 28);
			this.btn_Help.Name = "btn_Help";
			this.btn_Help.Size = new System.Drawing.Size(94, 29);
			this.btn_Help.TabIndex = 11;
			this.btn_Help.Text = "Help";
			this.btn_Help.UseVisualStyleBackColor = true;
			this.btn_Help.Click += new System.EventHandler(this.btn_Help_Click);
			// 
			// cmb_TargetFilter
			// 
			this.cmb_TargetFilter.FormattingEnabled = true;
			this.cmb_TargetFilter.Items.AddRange(new object[] {
            "Terrain",
            "Animal"});
			this.cmb_TargetFilter.Location = new System.Drawing.Point(31, 27);
			this.cmb_TargetFilter.Name = "cmb_TargetFilter";
			this.cmb_TargetFilter.Size = new System.Drawing.Size(361, 28);
			this.cmb_TargetFilter.TabIndex = 12;
			this.cmb_TargetFilter.Text = "Terrain";
			this.cmb_TargetFilter.SelectedIndexChanged += new System.EventHandler(this.cmb_TargetFilter_SelectedIndexChanged);
			// 
			// CombatDemoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(776, 497);
			this.Controls.Add(this.cmb_TargetFilter);
			this.Controls.Add(this.btn_Help);
			this.Controls.Add(this.txt_Status);
			this.Controls.Add(this.btn_Perform);
			this.Controls.Add(this.txt_Command);
			this.Controls.Add(this.list_Targets);
			this.Controls.Add(this.txtBox_MessageLog);
			this.Name = "CombatDemoForm";
			this.Text = "CombatDemo";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtBox_MessageLog;
		private System.Windows.Forms.ListBox list_Targets;
		private System.Windows.Forms.TextBox txt_Command;
		private System.Windows.Forms.Button btn_Perform;
		private System.Windows.Forms.TextBox txt_Status;
		private System.Windows.Forms.Button btn_Help;
		private System.Windows.Forms.ComboBox cmb_TargetFilter;
	}
}