using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace btl
{
	public partial class frmThemNCC : Form
	{
		Classes.DatabaseProcessor dtbase = new Classes.DatabaseProcessor();
		Classes.Function function = new Classes.Function();
		public frmThemNCC()
		{
			InitializeComponent();

			Classes.GiaoDien.ApDung(this, btnXacNhan);
			Classes.GiaoDien.TieuDe(label1, lblMoTa);
		}

		private void frmThemNCC_Load(object sender, EventArgs e)
		{
			function.NumberOnly(txtSDT);
		}

		private void btnXacNhan_Click(object sender, EventArgs e)
		{
			if (txtMaNCC.Text.Trim() == "" || txtTenNCC.Text.Trim() == "" || txtSDT.Text.Trim() == "" || txtDiaChi.Text.Trim() == "")
			{
				MessageBox.Show("Không được để trống!", "Thông báo");
				return;
			}

			if (dtbase.KiemTraTonTai("NhaCungCap", "MaNhaCungCap", txtMaNCC.Text))
			{
				MessageBox.Show("Mã nhà cung cấp bị trùng!", "Thông báo");
				return;
			}

			// Escape dấu ' để tên/địa chỉ có dấu nháy (VD: O'Brien) không làm hỏng câu SQL
			string sql = "Insert Into NhaCungCap Values ('" + function.EscapeSql(txtMaNCC.Text) + "', N'" + function.EscapeSql(txtTenNCC.Text) + "', '" + function.EscapeSql(txtSDT.Text) + "', N'" + function.EscapeSql(txtDiaChi.Text) + "')";
			dtbase.ChangeData(sql);

			MessageBox.Show("Thêm thành công!", "Thông báo");
			this.DialogResult = DialogResult.OK;

			this.Close();
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			if(MessageBox.Show("Xác nhận hủy?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				this.Close();
			}
		}
	}
}
