using System;
using System.Windows.Forms;

namespace TTRPG.Engine.Demo
{
	public partial class AttributesForm : Form
	{
		private readonly Role _target;

		public AttributesForm(Role target)
		{
			InitializeComponent();
			_target = target;
		}

		private void AttributesForm_Load(object sender, EventArgs e)
		{
			grid_Attributes.Columns.Add("Name", "Name");
			grid_Attributes.Columns.Add("Value", "Value");
			foreach (var attribute in _target.Attributes)
			{
				grid_Attributes.Rows.Add(attribute.Key, attribute.Value);
			}
		}

		private void btn_Close_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btn_Save_Click(object sender, EventArgs e)
		{
			_target.Attributes.Clear();
			foreach (DataGridViewRow row in grid_Attributes.Rows)
			{
				_target.Attributes[row.Cells[0].Value.ToString()] = row.Cells[1].Value.ToString();
			}
		}
	}
}
