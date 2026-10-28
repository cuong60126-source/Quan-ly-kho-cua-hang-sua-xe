using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Windows.Markup;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace btl
{
	public partial class frmQuanLyKho : Form
	{
		Classes.DatabaseProcessor dtbase = new Classes.DatabaseProcessor();
		Classes.Function function = new Classes.Function();
		private PrintDocument printDocument = new PrintDocument();
		private DataTable dataIn;
		public frmQuanLyKho()
		{
			InitializeComponent();
		}

		private void frmQuanLyKho_Load(object sender, EventArgs e)
		{
			LoadPhuTung();
			LoadPhieuNhap();
		}

		// ===== Phụ tùng =====

		private void LoadPhuTung()
		{
			string sqlDgvPhuTung = "Select pt.MaPhuTung, pt.TenPhuTung, ncc.TenNhaCungCap As TenNCC, pt.SoLuongTon, pt.DonGiaBan, pt.DonGiaNhap, pt.HanSuDung " +
				"From PhuTung pt Join NhaCungCap ncc On pt.MaNhaCungCap = ncc.MaNhaCungCap " +
				"Where pt.TrangThai = 1 " +
				"Order By pt.MaPhuTung Asc";

			dgvPhuTung.AutoGenerateColumns = false;
			dgvPhuTung.DataSource = dtbase.ReadData(sqlDgvPhuTung);
			dgvPhuTung.ReadOnly = true;
		}

		private void btnNgungSuDung_Click(object sender, EventArgs e)
		{
			if (dgvPhuTung.CurrentRow == null)
			{
				MessageBox.Show("Chưa chọn phụ tùng!", "Thông báo");
				return;
			}

			if (MessageBox.Show("Chắc chắn muốn ngưng sử dụng?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				string maPhuTung = dgvPhuTung.CurrentRow.Cells["colMaPhuTung"].Value.ToString();

				string sqlUpdateTrangThai = "Update PhuTung Set TrangThai = 0 Where MaPhuTung = '" + maPhuTung + "'";

				dtbase.ChangeData(sqlUpdateTrangThai);

				LoadPhuTung();
			}
		}

		private void txtTimPhuTung_TextChanged(object sender, EventArgs e)
		{
			string timPhuTung = txtTimPhuTung.Text;

			string sqlTimPhuTung = "Select pt.MaPhuTung, pt.TenPhuTung, ncc.TenNhaCungCap As TenNCC, pt.SoLuongTon, pt.DonGiaBan, pt.DonGiaNhap, pt.HanSuDung " +
				"From PhuTung pt Join NhaCungCap ncc On pt.MaNhaCungCap = ncc.MaNhaCungCap " +
				"Where pt.TrangThai = 1 And (pt.MaPhuTung Like '%" + timPhuTung + "%' Or pt.TenPhuTung Like N'%" + timPhuTung + "%')" +
				"Order By pt.MaPhuTung Asc";

			dgvPhuTung.DataSource = dtbase.ReadData(sqlTimPhuTung);
		}

		// ===== Phiếu nhập =====

		private void LoadPhieuNhap()
		{
			string sqlPhieuNhap = "Select pn.MaPhieuNhap, pn.NgayNhap, Sum(ct.SoLuongNhap) As TongSoLuongNhap, Sum(ct.ThanhTien) As ThanhTien, pn.GhiChu, pn.MaNhanVienNhap " +
				"From PhieuNhap pn Join ChiTietPhieuNhap ct On pn.MaPhieuNhap = ct.MaPhieuNhap " +
				"Group By pn.MaPhieuNhap, pn.NgayNhap, pn.GhiChu, pn.MaNhanVienNhap " +
				"Order By pn.MaPhieuNhap Asc";

			dgvPhieuNhap.DataSource = dtbase.ReadData(sqlPhieuNhap);

			dgvPhieuNhap.ReadOnly = true;
			dgvChiTietPhieuNhap.ReadOnly = true;

			if (dgvPhieuNhap.Rows.Count > 0)
			{
				string maPhieuNhap = dgvPhieuNhap.Rows[0].Cells["colMaPhieuNhap"].Value.ToString();

				string sqlDgvChiTietPhieuNhap = "Select ct.MaPhieuNhap, ct.MaPhuTung, ct.SoLuongNhap, pt.DonGiaNhap, ct.ThanhTien, pt.HanSuDung " +
				"From PhuTung pt Join ChiTietPhieuNhap ct On pt.MaPhuTung = ct.MaPhuTung " +
				"Where ct.MaPhieuNhap = '" + maPhieuNhap + "'";

				dgvChiTietPhieuNhap.DataSource = dtbase.ReadData(sqlDgvChiTietPhieuNhap);
			}
		}

		private void dgvPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			string maPhieuNhap = dgvPhieuNhap.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString();

			string sqlDgvChiTietPhieuNhap = "Select ct.MaPhieuNhap, ct.MaPhuTung, ct.SoLuongNhap, pt.DonGiaNhap, ct.ThanhTien, pt.HanSuDung " +
				"From PhuTung pt Join ChiTietPhieuNhap ct On pt.MaPhuTung = ct.MaPhuTung " +
				"Where ct.MaPhieuNhap = '" + maPhieuNhap + "'";

			dgvChiTietPhieuNhap.DataSource = dtbase.ReadData(sqlDgvChiTietPhieuNhap);
		}

		private string GenerateMaPhieuNhap()
		{
			string sql = "Select Max(Cast(Substring(MaPhieuNhap, 3, Len(MaPhieuNhap)) As int)) From PhieuNhap";

			DataTable data = dtbase.ReadData(sql);

			int so = 1;
			if (data.Rows.Count > 0 && data.Rows[0][0] != DBNull.Value)
			{
				so = Convert.ToInt32(data.Rows[0][0]) + 1;
			}

			return "PN" + so.ToString("D3");
		}

		private void btnThemPhieuNhap_Click(object sender, EventArgs e)
		{
			frmNhapPhieuNhap formNhapPhieuNhap = new frmNhapPhieuNhap(GenerateMaPhieuNhap());
			formNhapPhieuNhap.ShowDialog();

			LoadPhieuNhap();
			LoadPhuTung();
		}

		private void btnSuaPhieuNhap_Click(object sender, EventArgs e)
		{
			if (dgvChiTietPhieuNhap.Rows.Count == 0 || dgvPhieuNhap.CurrentRow.IsNewRow == true)
			{
				MessageBox.Show("Chọn phiếu cần sửa!", "Thông báo");
				return;
			}

			string maPhieuNhap = dgvPhieuNhap.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString();

			frmNhapPhieuNhap formNhapPhieuNhap = new frmNhapPhieuNhap(maPhieuNhap);
			formNhapPhieuNhap.ShowDialog();

			LoadPhieuNhap();
			LoadPhuTung();
		}

		private void btnXoaPhieuNhap_Click(object sender, EventArgs e)
		{
			if (dgvChiTietPhieuNhap.Rows.Count == 0 || dgvPhieuNhap.CurrentRow.IsNewRow == true)
			{
				MessageBox.Show("Chọn phiếu cần xóa!", "Thông báo");
				return;
			}

			if (MessageBox.Show("Chắc chắn muốn xóa phiếu nhập?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{

				}
				catch (Exception ex)
				{
					MessageBox.Show("Lỗi khi xóa phiếu: " + ex.Message, "Thông báo");
				}
			}
		}

		private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
		{
			Graphics g = e.Graphics;

			Font fontTitle = new Font("Arial", 18, FontStyle.Bold);
			Font fontHeader = new Font("Arial", 11, FontStyle.Bold);
			Font font = new Font("Arial", 10);

			float x = 50, y = 50;
			float lineHeight = 30;

			string maPhieu = dataIn.Rows[0]["MaPhieuNhap"].ToString();
			string nguoiNhap = dataIn.Rows[0]["MaNhanVienNhap"].ToString();
			DateTime ngayNhap = Convert.ToDateTime(dataIn.Rows[0]["NgayNhap"]);

			g.DrawString("Phiếu Nhập Kho", fontTitle, Brushes.Black, x + 200, y);
			y += 50;

			g.DrawString("Mã phiếu: " + maPhieu, font, Brushes.Black, x, y);
			y += lineHeight;

			g.DrawString("Ngày nhập: " + ngayNhap.ToString("dd/MM/yyyy"), font, Brushes.Black, x, y);
			y += lineHeight;

			g.DrawString("Người nhập: " + nguoiNhap, font, Brushes.Black, x, y);
			y += lineHeight;

			g.DrawString("Nhà cung cấp:", fontHeader, Brushes.Black, x, y);
			y += lineHeight;

			var nhaCungCap = dataIn.AsEnumerable().Select(r => r["TenNCC"].ToString()).Distinct();
			foreach (string tenNCC in nhaCungCap)
			{
				g.DrawString("- " + tenNCC, font, Brushes.Black, x, y);
				y += lineHeight;
			}

			y += 10;

			float[] widths = { 45, 100, 200, 65, 90, 100 };
			string[] headers = { "STT", "Mã PT", "Tên PT", "SL", "Đơn giá", "Thành tiền" };

			float tableX = x;

			for (int i = 0; i < headers.Length; i++)
			{
				g.DrawRectangle(Pens.Black, tableX, y, widths[i], lineHeight);
				g.DrawString(headers[i], fontHeader, Brushes.Black, new RectangleF(tableX + 3, y + 5, widths[i] - 6, lineHeight));
				tableX += widths[i];
			}

			y += lineHeight;
			decimal tongTien = 0;

			for (int i = 0; i < dataIn.Rows.Count; i++)
			{
				DataRow row = dataIn.Rows[i];
				tableX = x;

				string[] values = { (i + 1).ToString(), row["MaPhuTung"].ToString(), row["TenPhuTung"].ToString(), row["SoLuong"].ToString(), Convert.ToDecimal(row["DonGiaNhap"]).ToString(), Convert.ToDecimal(row["ThanhTien"]).ToString() };

				for (int j = 0; j < values.Length; j++)
				{
					g.DrawRectangle(Pens.Black, tableX, y, widths[j], lineHeight);
					g.DrawString(values[j], font, Brushes.Black, new RectangleF(tableX + 3, y + 5, widths[j] - 6, lineHeight));
					tableX += widths[j];
				}

				tongTien += Convert.ToDecimal(row["ThanhTien"]);
				y += lineHeight;
			}

			y += 20;

			g.DrawString("Tổng tiền: " + tongTien.ToString("N0") + " VNĐ", fontHeader, Brushes.Black, x, y);

			e.HasMorePages = false;
		}

		private void btnInPhieuNhap_Click(object sender, EventArgs e)
		{
			if (dgvChiTietPhieuNhap.Rows.Count == 0 || dgvPhieuNhap.CurrentRow.IsNewRow == true)
			{
				MessageBox.Show("Chọn phiếu in!", "Thông báo");
				return;
			}

			string maPhieuNhap = dgvPhieuNhap.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString();

			string sql = "SELECT pn.MaPhieuNhap, pn.NgayNhap, pn.MaNhanVienNhap, " +
				 "ncc.TenNhaCungCap AS TenNCC, pt.MaPhuTung, pt.TenPhuTung, " +
				 "ct.SoLuongNhap AS SoLuong, pt.DonGiaNhap, ct.ThanhTien, pt.HanSuDung " +
				 "FROM PhieuNhap pn " +
				 "JOIN ChiTietPhieuNhap ct ON pn.MaPhieuNhap = ct.MaPhieuNhap " +
				 "JOIN PhuTung pt ON ct.MaPhuTung = pt.MaPhuTung " +
				 "JOIN NhaCungCap ncc ON pt.MaNhaCungCap = ncc.MaNhaCungCap " +
				 "WHERE pn.MaPhieuNhap = '" + maPhieuNhap + "' " +
				 "ORDER BY ct.MaPhuTung";

			dataIn = dtbase.ReadData(sql);

			if (dataIn.Rows.Count == 0)
			{
				MessageBox.Show("Phiếu nhập không có chi tiết!", "Thông báo");
				return;
			}

			printDocument.PrintPage -= printDocument_PrintPage;
			printDocument.PrintPage += printDocument_PrintPage;

			PrintPreviewDialog preview = new PrintPreviewDialog();
			preview.Document = printDocument;
			preview.Width = 1000;
			preview.Height = 700;
			preview.ShowDialog();
		}

		private void txtTimPhieuNhap_TextChanged(object sender, EventArgs e)
		{
			string timPhieuNhap = txtTimPhieuNhap.Text;

			string sqlTimPhieuNhap = "Select pn.MaPhieuNhap, pn.NgayNhap, Sum(ct.SoLuongNhap) As TongSoLuongNhap, Sum(ct.ThanhTien) As ThanhTien, pn.GhiChu, pn.MaNhanVienNhap " +
				"From PhieuNhap pn Join ChiTietPhieuNhap ct On pn.MaPhieuNhap = ct.MaPhieuNhap " +
				"Where pn.MaPhieuNhap Like '%" + timPhieuNhap + "%' " +
				"Group By pn.MaPhieuNhap, pn.NgayNhap, pn.GhiChu, pn.MaNhanVienNhap " +
				"Order By pn.MaPhieuNhap Asc";
			dgvPhieuNhap.DataSource = dtbase.ReadData(sqlTimPhieuNhap);

			if (dgvPhieuNhap.Rows.Count > 0)
			{
				string maPhieuNhap = dgvPhieuNhap.Rows[0].Cells["colMaPhieuNhap"].Value.ToString();

				string sqlChiTietPhieuNhap = "Select ct.MaPhieuNhap, ct.MaPhuTung, ct.SoLuongNhap, pt.DonGiaNhap, ct.ThanhTien, pt.HanSuDung " +
				"From PhuTung pt Join ChiTietPhieuNhap ct On pt.MaPhuTung = ct.MaPhuTung " +
				"Where ct.MaPhieuNhap = '" + maPhieuNhap + "'";

				dgvChiTietPhieuNhap.DataSource = dtbase.ReadData(sqlChiTietPhieuNhap);
			}
		}

		private void btnThoat_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Chắc chắn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				this.Close();
			}
		}
	}
}
