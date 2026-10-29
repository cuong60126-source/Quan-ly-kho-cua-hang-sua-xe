using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace btl.Classes
{
	public class Function
	{
		public void AddComboBox(ComboBox cb, DataTable data, string name, string value)
		{
			cb.DataSource = data;
			cb.DisplayMember = name;
			cb.ValueMember = value;
		}
		public void NumberOnly(TextBox txt)
		{
			txt.KeyPress += txt_KeyPress;
		}
		private void txt_KeyPress(Object sender, KeyPressEventArgs e)
		{
			if(!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}
		// Escape dấu ' để tên/ghi chú/lý do có dấu nháy không làm hỏng câu SQL
		public string EscapeSql(object? value)
		{
			return (value?.ToString() ?? "").Replace("'", "''");
		}
	}
}
