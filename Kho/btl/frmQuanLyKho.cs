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
using ClosedXML.Excel;

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
			function.NumberOnly(txtDonGiaBanMoi);

			LoadPhuTung();
			LoadPhieuNhap();
			LoadDieuChinh();
		}

		// Escape dấu ' để mã NV/lý do có dấu nháy không làm hỏng câu SQL
		private string EscapeSql(string value)
		{
			return value.Replace("'", "''");
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

		private void btnDieuChinhSoLuong_Click(object sender, EventArgs e)
		{
			if (dgvPhuTung.CurrentRow == null)
			{
				MessageBox.Show("Chưa chọn phụ tùng!", "Thông báo");
				return;
			}

			int soLuongDieuChinh = Convert.ToInt32(nudSoLuongDieuChinh.Value);

			if (soLuongDieuChinh == 0)
			{
				MessageBox.Show("Số lượng điều chỉnh phải khác 0!", "Thông báo");
				return;
			}

			string maPhuTung = dgvPhuTung.CurrentRow.Cells["colMaPhuTung"].Value.ToString();
			int soLuongTon = Convert.ToInt32(dgvPhuTung.CurrentRow.Cells["colSoLuongTon"].Value);
			int soLuongTonMoi = soLuongTon + soLuongDieuChinh;

			if (soLuongTonMoi < 0)
			{
				MessageBox.Show("Số lượng tồn sau điều chỉnh không được âm!", "Thông báo");
				return;
			}

			if (!KiemTraThongTinDieuChinh())
			{
				return;
			}

			decimal donGiaNhap = Convert.ToDecimal(dgvPhuTung.CurrentRow.Cells["colDonGiaNhap"].Value);
			decimal tienLech = soLuongDieuChinh * donGiaNhap;

			if (MessageBox.Show("Điều chỉnh tồn " + maPhuTung + ": " + soLuongTon + " → " + soLuongTonMoi + " (tiền lệch " + tienLech.ToString("N0") + ")?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{
					string maDieuChinh = GenerateMaDieuChinh("DieuChinhTonKho", "MaDieuChinh", "DC");
					string maNV = EscapeSql(txtMaNVDieuChinh.Text.Trim());
					string lyDo = EscapeSql(txtLyDoDieuChinh.Text.Trim());

					// Ghi lịch sử và cập nhật tồn trong 1 transaction, số lượng cũ lấy từ database
					string sqlDieuChinhSoLuong = $"""
					SET XACT_ABORT ON;

					BEGIN TRY
					    BEGIN TRANSACTION;

					    INSERT INTO dbo.DieuChinhTonKho (MaDieuChinh, MaPhuTung, MaNhanVien, NgayDieuChinh, SoLuongCu, SoLuongMoi, DonGiaNhap, LyDo)
					    SELECT '{maDieuChinh}', MaPhuTung, '{maNV}', CAST(GETDATE() AS DATE), SoLuongTon, SoLuongTon + ({soLuongDieuChinh}), DonGiaNhap, N'{lyDo}'
					    FROM dbo.PhuTung
					    WHERE MaPhuTung = '{maPhuTung}';

					    UPDATE dbo.PhuTung
					    SET SoLuongTon = SoLuongTon + ({soLuongDieuChinh})
					    WHERE MaPhuTung = '{maPhuTung}';

					    COMMIT TRANSACTION;
					END TRY
					BEGIN CATCH
					    IF @@TRANCOUNT > 0
					        ROLLBACK TRANSACTION;

					    THROW;
					END CATCH;
					""";

					dtbase.ChangeData(sqlDieuChinhSoLuong);

					nudSoLuongDieuChinh.Value = 0;
					txtLyDoDieuChinh.Clear();

					LoadPhuTung();
					LoadDieuChinh();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Lỗi khi điều chỉnh tồn: " + ex.Message, "Thông báo");
				}
			}
		}

		// Chọn phụ tùng thì điền sẵn giá bán hiện tại để sửa
		private void dgvPhuTung_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}

			txtDonGiaBanMoi.Text = Convert.ToDecimal(dgvPhuTung.Rows[e.RowIndex].Cells["colDonGiaBan"].Value).ToString("0.##");
		}

		private void btnDieuChinhGiaBan_Click(object sender, EventArgs e)
		{
			if (dgvPhuTung.CurrentRow == null)
			{
				MessageBox.Show("Chưa chọn phụ tùng!", "Thông báo");
				return;
			}

			if (txtDonGiaBanMoi.Text.Trim() == "")
			{
				MessageBox.Show("Chưa nhập giá bán mới!", "Thông báo");
				return;
			}

			string maPhuTung = dgvPhuTung.CurrentRow.Cells["colMaPhuTung"].Value.ToString();
			decimal donGiaNhap = Convert.ToDecimal(dgvPhuTung.CurrentRow.Cells["colDonGiaNhap"].Value);
			decimal donGiaBan = Convert.ToDecimal(dgvPhuTung.CurrentRow.Cells["colDonGiaBan"].Value);
			decimal donGiaBanMoi = Convert.ToDecimal(txtDonGiaBanMoi.Text);

			if (donGiaBanMoi == donGiaBan)
			{
				MessageBox.Show("Giá bán mới phải khác giá bán hiện tại!", "Thông báo");
				return;
			}

			if (donGiaBanMoi <= 0)
			{
				MessageBox.Show("Giá bán không hợp lệ!", "Thông báo");
				return;
			}

			if (!KiemTraThongTinDieuChinh())
			{
				return;
			}

			string xacNhan = "Điều chỉnh giá bán " + maPhuTung + ": " + donGiaBan.ToString("N0") + " → " + donGiaBanMoi.ToString("N0") + "?";

			// Giá thị trường giảm thì được bán thấp hơn giá nhập, chỉ cảnh báo để tránh gõ nhầm
			if (donGiaBanMoi < donGiaNhap)
			{
				xacNhan = "Giá bán thấp hơn đơn giá nhập (" + donGiaNhap.ToString("N0") + ")!\n" + xacNhan;
			}

			if (MessageBox.Show(xacNhan, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{
					string maDieuChinhGia = GenerateMaDieuChinh("DieuChinhGiaBan", "MaDieuChinhGia", "DG");
					string maNV = EscapeSql(txtMaNVDieuChinh.Text.Trim());
					string lyDo = EscapeSql(txtLyDoDieuChinh.Text.Trim());

					// Ghi lịch sử và cập nhật giá bán trong 1 transaction, giá cũ lấy từ database
					string sqlDieuChinhGiaBan = $"""
					SET XACT_ABORT ON;

					BEGIN TRY
					    BEGIN TRANSACTION;

					    INSERT INTO dbo.DieuChinhGiaBan (MaDieuChinhGia, MaPhuTung, MaNhanVien, NgayDieuChinh, GiaBanCu, GiaBanMoi, LyDo)
					    SELECT '{maDieuChinhGia}', MaPhuTung, '{maNV}', CAST(GETDATE() AS DATE), DonGiaBan, {donGiaBanMoi.ToString("0")}, N'{lyDo}'
					    FROM dbo.PhuTung
					    WHERE MaPhuTung = '{maPhuTung}';

					    UPDATE dbo.PhuTung
					    SET DonGiaBan = {donGiaBanMoi.ToString("0")}
					    WHERE MaPhuTung = '{maPhuTung}';

					    COMMIT TRANSACTION;
					END TRY
					BEGIN CATCH
					    IF @@TRANCOUNT > 0
					        ROLLBACK TRANSACTION;

					    THROW;
					END CATCH;
					""";

					dtbase.ChangeData(sqlDieuChinhGiaBan);

					txtDonGiaBanMoi.Clear();
					txtLyDoDieuChinh.Clear();

					LoadPhuTung();
					LoadDieuChinh();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Lỗi khi điều chỉnh giá bán: " + ex.Message, "Thông báo");
				}
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

		// Mã NV và lý do bắt buộc cho mọi lần điều chỉnh tồn/giá bán
		private bool KiemTraThongTinDieuChinh()
		{
			if (txtMaNVDieuChinh.Text.Trim() == "" || txtLyDoDieuChinh.Text.Trim() == "")
			{
				MessageBox.Show("Nhập mã NV và lý do điều chỉnh!", "Thông báo");
				return false;
			}

			string sqlNhanVien = "Select * " +
				"From NhanVien " +
				"Where MaNhanVien = '" + EscapeSql(txtMaNVDieuChinh.Text.Trim()) + "'";

			if (dtbase.ReadData(sqlNhanVien).Rows.Count == 0)
			{
				MessageBox.Show("Mã nhân viên không tồn tại!", "Thông báo");
				txtMaNVDieuChinh.Focus();
				return false;
			}

			return true;
		}

		private string GenerateMaDieuChinh(string bang, string cot, string tienTo)
		{
			string sql = "Select Max(Cast(Substring(" + cot + ", 3, Len(" + cot + ")) As int)) From " + bang;

			DataTable data = dtbase.ReadData(sql);

			int so = 1;
			if (data.Rows.Count > 0 && data.Rows[0][0] != DBNull.Value)
			{
				so = Convert.ToInt32(data.Rows[0][0]) + 1;
			}

			return tienTo + so.ToString("D3");
		}

		// ===== Lịch sử điều chỉnh =====

		private void LoadDieuChinh()
		{
			string timPhuTung = EscapeSql(txtTimDieuChinh.Text);

			string sqlDieuChinhTonKho = "Select dc.MaDieuChinh, dc.NgayDieuChinh, dc.MaPhuTung, pt.TenPhuTung, dc.SoLuongCu, dc.SoLuongMoi, dc.DonGiaNhap, dc.GiaTriChenhLech, dc.MaNhanVien, dc.LyDo " +
				"From DieuChinhTonKho dc Join PhuTung pt On dc.MaPhuTung = pt.MaPhuTung " +
				"Where dc.MaPhuTung Like '%" + timPhuTung + "%' " +
				"Order By dc.MaDieuChinh Desc";

			DataTable dataTonKho = dtbase.ReadData(sqlDieuChinhTonKho);

			dgvDieuChinhTonKho.AutoGenerateColumns = false;
			dgvDieuChinhTonKho.DataSource = dataTonKho;
			dgvDieuChinhTonKho.ReadOnly = true;

			TinhTongTienLech(dataTonKho, out decimal tongThua, out decimal tongThieu);

			lblTongTienLech.Text = "Tổng thừa: " + tongThua.ToString("N0") + "    Tổng thiếu: " + tongThieu.ToString("N0") + "    Tổng tiền lệch: " + (tongThua + tongThieu).ToString("N0");

			string sqlDieuChinhGiaBan = "Select dg.MaDieuChinhGia, dg.NgayDieuChinh, dg.MaPhuTung, pt.TenPhuTung, dg.GiaBanCu, dg.GiaBanMoi, dg.MaNhanVien, dg.LyDo " +
				"From DieuChinhGiaBan dg Join PhuTung pt On dg.MaPhuTung = pt.MaPhuTung " +
				"Where dg.MaPhuTung Like '%" + timPhuTung + "%' " +
				"Order By dg.MaDieuChinhGia Desc";

			dgvDieuChinhGiaBan.AutoGenerateColumns = false;
			dgvDieuChinhGiaBan.DataSource = dtbase.ReadData(sqlDieuChinhGiaBan);
			dgvDieuChinhGiaBan.ReadOnly = true;
		}

		// Tiền lệch dương là thừa hàng, âm là thiếu hàng
		private void TinhTongTienLech(DataTable dataTonKho, out decimal tongThua, out decimal tongThieu)
		{
			tongThua = 0;
			tongThieu = 0;
			foreach (DataRow row in dataTonKho.Rows)
			{
				decimal tienLech = Convert.ToDecimal(row["GiaTriChenhLech"]);
				if (tienLech > 0)
				{
					tongThua += tienLech;
				}
				else
				{
					tongThieu += tienLech;
				}
			}
		}

		private void txtTimDieuChinh_TextChanged(object sender, EventArgs e)
		{
			LoadDieuChinh();
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

				CapNhatNutPhieuNhap(maPhieuNhap);
			}
			else
			{
				CapNhatNutPhieuNhap("");
			}
		}

		private void dgvPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			string maPhieuNhap = dgvPhieuNhap.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString();

			string sqlDgvChiTietPhieuNhap = "Select ct.MaPhieuNhap, ct.MaPhuTung, ct.SoLuongNhap, pt.DonGiaNhap, ct.ThanhTien, pt.HanSuDung " +
				"From PhuTung pt Join ChiTietPhieuNhap ct On pt.MaPhuTung = ct.MaPhuTung " +
				"Where ct.MaPhieuNhap = '" + maPhieuNhap + "'";

			dgvChiTietPhieuNhap.DataSource = dtbase.ReadData(sqlDgvChiTietPhieuNhap);

			CapNhatNutPhieuNhap(maPhieuNhap);
		}

		// Phiếu có phụ tùng đã được đưa vào dịch vụ (ChiTietDichVu) thì không được sửa/xóa
		private bool PhieuNhapDaDungTrongDichVu(string maPhieuNhap)
		{
			string sqlCheck = "Select ct.MaPhuTung " +
				"From ChiTietPhieuNhap ct Join ChiTietDichVu ctdv On ct.MaPhuTung = ctdv.MaPhuTung " +
				"Where ct.MaPhieuNhap = '" + maPhieuNhap + "'";

			return dtbase.ReadData(sqlCheck).Rows.Count > 0;
		}

		private void CapNhatNutPhieuNhap(string maPhieuNhap)
		{
			bool choPhep = maPhieuNhap != "" && !PhieuNhapDaDungTrongDichVu(maPhieuNhap);

			btnSuaPhieuNhap.Enabled = choPhep;
			btnXoaPhieuNhap.Enabled = choPhep;
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

			if (PhieuNhapDaDungTrongDichVu(maPhieuNhap))
			{
				MessageBox.Show("Phiếu nhập có phụ tùng đã được dùng trong dịch vụ, không thể sửa!", "Thông báo");
				return;
			}

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

			string maPhieuNhap = dgvPhieuNhap.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString();

			if (PhieuNhapDaDungTrongDichVu(maPhieuNhap))
			{
				MessageBox.Show("Phiếu nhập có phụ tùng đã được dùng trong dịch vụ, không thể xóa!", "Thông báo");
				return;
			}

			if (MessageBox.Show("Chắc chắn muốn xóa phiếu nhập?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{
					// Cả phiếu được xóa trong 1 transaction: lỗi ở bất kỳ bước nào thì không xóa gì cả.
					string sqlXoaPhieuNhap = $"""
					SET XACT_ABORT ON;

					BEGIN TRY
					    BEGIN TRANSACTION;

					    DECLARE @MaPhieuNhap VARCHAR(10) = '{maPhieuNhap}';
					    DECLARE @Loi NVARCHAR(400);

					    -- 1. Phụ tùng đã được đưa vào dịch vụ thì không được xóa phiếu
					    IF EXISTS
					    (
					        SELECT 1
					        FROM dbo.ChiTietPhieuNhap CT
					        JOIN dbo.ChiTietDichVu CTDV ON CTDV.MaPhuTung = CT.MaPhuTung
					        WHERE CT.MaPhieuNhap = @MaPhieuNhap
					    )
					        THROW 50001, N'Phiếu nhập có phụ tùng đã được dùng trong dịch vụ, không thể xóa!', 1;

					    -- 2. Phụ tùng đã có lịch sử điều chỉnh tồn/giá bán thì không được xóa phiếu
					    SELECT TOP 1 @Loi = N'Phụ tùng ' + CT.MaPhuTung + N' đã có điều chỉnh tồn hoặc giá bán, không thể xóa phiếu!'
					    FROM dbo.ChiTietPhieuNhap CT
					    WHERE CT.MaPhieuNhap = @MaPhieuNhap
					      AND (EXISTS (SELECT 1 FROM dbo.DieuChinhTonKho DC WHERE DC.MaPhuTung = CT.MaPhuTung)
					        OR EXISTS (SELECT 1 FROM dbo.DieuChinhGiaBan DG WHERE DG.MaPhuTung = CT.MaPhuTung));

					    IF @Loi IS NOT NULL
					        THROW 50002, @Loi, 1;

					    -- 3. Số lượng tồn phải đủ để trừ
					    SELECT TOP 1 @Loi = N'Số lượng tồn của phụ tùng ' + PT.MaPhuTung + N' không đủ để trừ!'
					    FROM dbo.ChiTietPhieuNhap CT
					    JOIN dbo.PhuTung PT ON PT.MaPhuTung = CT.MaPhuTung
					    WHERE CT.MaPhieuNhap = @MaPhieuNhap
					      AND PT.SoLuongTon < CT.SoLuongNhap;

					    IF @Loi IS NOT NULL
					        THROW 50003, @Loi, 1;

					    -- 4. Trừ số lượng tồn
					    UPDATE PT
					    SET PT.SoLuongTon = PT.SoLuongTon - CT.SoLuongNhap
					    FROM dbo.PhuTung PT
					    JOIN dbo.ChiTietPhieuNhap CT ON CT.MaPhuTung = PT.MaPhuTung
					    WHERE CT.MaPhieuNhap = @MaPhieuNhap;

					    -- 5. Xóa chi tiết và phiếu nhập (giữ lại danh sách phụ tùng để dọn ở bước 6)
					    DECLARE @PhuTung TABLE (MaPhuTung VARCHAR(10) NOT NULL PRIMARY KEY);

					    INSERT INTO @PhuTung (MaPhuTung)
					    SELECT MaPhuTung
					    FROM dbo.ChiTietPhieuNhap
					    WHERE MaPhieuNhap = @MaPhieuNhap;

					    DELETE FROM dbo.ChiTietPhieuNhap
					    WHERE MaPhieuNhap = @MaPhieuNhap;

					    DELETE FROM dbo.PhieuNhap
					    WHERE MaPhieuNhap = @MaPhieuNhap;

					    -- 6. Phụ tùng không còn thuộc phiếu nhập nào thì xóa
					    DELETE PT
					    FROM dbo.PhuTung PT
					    JOIN @PhuTung X ON X.MaPhuTung = PT.MaPhuTung
					    WHERE NOT EXISTS (SELECT 1 FROM dbo.ChiTietPhieuNhap CT WHERE CT.MaPhuTung = PT.MaPhuTung);

					    COMMIT TRANSACTION;
					END TRY
					BEGIN CATCH
					    IF @@TRANCOUNT > 0
					        ROLLBACK TRANSACTION;

					    THROW;
					END CATCH;
					""";

					dtbase.ChangeData(sqlXoaPhieuNhap);

					MessageBox.Show("Xóa thành công!", "Thông báo");

					LoadPhieuNhap();
					LoadPhuTung();
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

				CapNhatNutPhieuNhap(maPhieuNhap);
			}
			else
			{
				CapNhatNutPhieuNhap("");
			}
		}

		// ===== Xuất Excel =====

		private string ChonFileLuu(string tenFile)
		{
			using (SaveFileDialog dialog = new SaveFileDialog())
			{
				dialog.Filter = "Excel (*.xlsx)|*.xlsx";
				// Thêm giờ phút để mỗi lần xuất ra 1 file riêng, không ghi đè lần xuất trước
				dialog.FileName = tenFile + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".xlsx";

				return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "";
			}
		}

		// Ghi 1 bảng ra 1 sheet: tiền định dạng #,##0, ngày định dạng dd/MM/yyyy
		private IXLWorksheet ThemSheet(XLWorkbook wb, string tenSheet, DataTable data, string[] cot, string[] tieuDe)
		{
			IXLWorksheet ws = wb.Worksheets.Add(tenSheet);

			for (int j = 0; j < tieuDe.Length; j++)
			{
				ws.Cell(1, j + 1).Value = tieuDe[j];
			}

			for (int i = 0; i < data.Rows.Count; i++)
			{
				for (int j = 0; j < cot.Length; j++)
				{
					object giaTri = data.Rows[i][cot[j]];
					IXLCell cell = ws.Cell(i + 2, j + 1);

					if (giaTri is DateTime ngay)
					{
						cell.Value = ngay;
						cell.Style.NumberFormat.Format = "dd/MM/yyyy";
					}
					else if (giaTri is decimal tien)
					{
						cell.Value = (double)tien;
						cell.Style.NumberFormat.Format = "#,##0";
					}
					else if (giaTri is int so)
					{
						cell.Value = so;
					}
					else if (giaTri != DBNull.Value)
					{
						cell.Value = giaTri.ToString();
					}
				}
			}

			// Kẻ bảng có lọc; bảng trống vẫn giữ 1 dòng để Excel tạo được bảng
			ws.Range(1, 1, Math.Max(data.Rows.Count, 1) + 1, cot.Length).CreateTable();
			ws.Columns().AdjustToContents();

			return ws;
		}

		// Cột và tiêu đề lấy theo lưới đang hiện, dữ liệu theo ô tìm
		private IXLWorksheet ThemSheet(XLWorkbook wb, string tenSheet, DataGridView dgv)
		{
			List<DataGridViewColumn> cot = dgv.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();

			return ThemSheet(wb, tenSheet, (DataTable)dgv.DataSource, cot.Select(c => c.DataPropertyName).ToArray(), cot.Select(c => c.HeaderText).ToArray());
		}

		private void LuuExcel(XLWorkbook wb, string duongDan)
		{
			try
			{
				wb.SaveAs(duongDan);

				MessageBox.Show("Xuất file thành công!", "Thông báo");
			}
			catch (IOException)
			{
				MessageBox.Show("File đang được mở, hãy đóng file rồi thử lại!", "Thông báo");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Thông báo");
			}
		}

		// 1 file gồm 3 sheet: phụ tùng, lịch sử điều chỉnh tồn (kèm dòng tổng), lịch sử điều chỉnh giá bán
		private void btnXuatExcelPhuTung_Click(object sender, EventArgs e)
		{
			string duongDan = ChonFileLuu("PhuTung");

			if (duongDan == "")
			{
				return;
			}

			using (XLWorkbook wb = new XLWorkbook())
			{
				ThemSheet(wb, "Phụ tùng", dgvPhuTung);

				DataTable dataTonKho = (DataTable)dgvDieuChinhTonKho.DataSource;
				IXLWorksheet wsTonKho = ThemSheet(wb, "Điều chỉnh tồn", dgvDieuChinhTonKho);

				// Dòng tổng nằm dưới bảng, số tiền thẳng cột "Tiền lệch"
				TinhTongTienLech(dataTonKho, out decimal tongThua, out decimal tongThieu);
				int dong = Math.Max(dataTonKho.Rows.Count, 1) + 3;
				int cotTienLech = colDieuChinhGiaTriChenhLech.DisplayIndex + 1;
				string[] nhan = { "Tổng thừa:", "Tổng thiếu:", "Tổng tiền lệch:" };
				decimal[] tong = { tongThua, tongThieu, tongThua + tongThieu };

				for (int i = 0; i < nhan.Length; i++)
				{
					wsTonKho.Cell(dong + i, cotTienLech - 1).Value = nhan[i];
					wsTonKho.Cell(dong + i, cotTienLech).Value = (double)tong[i];
					wsTonKho.Cell(dong + i, cotTienLech).Style.NumberFormat.Format = "#,##0";
					wsTonKho.Range(dong + i, cotTienLech - 1, dong + i, cotTienLech).Style.Font.Bold = true;
				}
				wsTonKho.Columns().AdjustToContents();

				ThemSheet(wb, "Điều chỉnh giá bán", dgvDieuChinhGiaBan);

				LuuExcel(wb, duongDan);
			}
		}

		// 1 file gồm 2 sheet: phiếu nhập và chi tiết của các phiếu đang hiện trên lưới
		private void btnXuatExcelPhieuNhap_Click(object sender, EventArgs e)
		{
			string duongDan = ChonFileLuu("PhieuNhap");

			if (duongDan == "")
			{
				return;
			}

			DataTable dataPhieuNhap = (DataTable)dgvPhieuNhap.DataSource;
			List<string> dsMaPhieuNhap = dataPhieuNhap.AsEnumerable().Select(r => "'" + r["MaPhieuNhap"].ToString() + "'").ToList();

			string sqlChiTiet = "Select ct.MaPhieuNhap, ct.MaPhuTung, pt.TenPhuTung, ncc.TenNhaCungCap, ct.SoLuongNhap, pt.DonGiaNhap, ct.ThanhTien, pt.HanSuDung " +
				"From ChiTietPhieuNhap ct Join PhuTung pt On ct.MaPhuTung = pt.MaPhuTung " +
				"Join NhaCungCap ncc On pt.MaNhaCungCap = ncc.MaNhaCungCap " +
				"Where " + (dsMaPhieuNhap.Count > 0 ? "ct.MaPhieuNhap In (" + string.Join(", ", dsMaPhieuNhap) + ")" : "1 = 0") + " " +
				"Order By ct.MaPhieuNhap, ct.MaPhuTung";

			using (XLWorkbook wb = new XLWorkbook())
			{
				ThemSheet(wb, "Phiếu nhập", dgvPhieuNhap);

				ThemSheet(wb, "Chi tiết phiếu nhập", dtbase.ReadData(sqlChiTiet),
					new string[] { "MaPhieuNhap", "MaPhuTung", "TenPhuTung", "TenNhaCungCap", "SoLuongNhap", "DonGiaNhap", "ThanhTien", "HanSuDung" },
					new string[] { "Mã phiếu nhập", "Mã phụ tùng", "Tên phụ tùng", "Nhà cung cấp", "Số lượng nhập", "Đơn giá nhập", "Thành tiền", "Hạn sử dụng" });

				LuuExcel(wb, duongDan);
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
