
namespace TTRPG.Engine.Demo
{
	partial class AttributesForm
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
			this.grid_Attributes = new System.Windows.Forms.DataGridView();
			this.btn_Close = new System.Windows.Forms.Button();
			this.btn_Save = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.grid_Attributes)).BeginInit();
			this.SuspendLayout();
			// 
			// grid_Attributes
			// 
			this.grid_Attributes.AllowUserToAddRows = false;
			this.grid_Attributes.AllowUserToDeleteRows = false;
			this.grid_Attributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid_Attributes.Location = new System.Drawing.Point(30, 29);
			this.grid_Attributes.Name = "grid_Attributes";
			this.grid_Attributes.RowHeadersWidth = 51;
			this.grid_Attributes.RowTemplate.Height = 29;
			this.grid_Attributes.Size = new System.Drawing.Size(734, 356);
			this.grid_Attributes.TabIndex = 0;
			// 
			// btn_Close
			// 
			this.btn_Close.Location = new System.Drawing.Point(670, 400);
			this.btn_Close.Name = "btn_Close";
			this.btn_Close.Size = new System.Drawing.Size(94, 29);
			this.btn_Close.TabIndex = 1;
			this.btn_Close.Text = "Close";
			this.btn_Close.UseVisualStyleBackColor = true;
			this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
			// 
			// btn_Save
			// 
			this.btn_Save.Location = new System.Drawing.Point(340, 400);
			this.btn_Save.Name = "btn_Save";
			this.btn_Save.Size = new System.Drawing.Size(94, 29);
			this.btn_Save.TabIndex = 2;
			this.btn_Save.Text = "Save";
			this.btn_Save.UseVisualStyleBackColor = true;
			this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
			// 
			// AttributesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.btn_Save);
			this.Controls.Add(this.btn_Close);
			this.Controls.Add(this.grid_Attributes);
			this.Name = "AttributesForm";
			this.Text = "AttributesForm";
			this.Load += new System.EventHandler(this.AttributesForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.grid_Attributes)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView grid_Attributes;
		private System.Windows.Forms.Button btn_Close;
		private System.Windows.Forms.Button btn_Save;
	}
}