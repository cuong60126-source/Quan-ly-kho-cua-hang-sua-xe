using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using ClosedXML.Excel;

namespace btl
{
	public partial class frmQuanLyKho : Form
	{
		Classes.DatabaseProcessor dtbase = new Classes.DatabaseProcessor();
		Classes.Function function = new Classes.Function();
		private PrintDocument printDocument = new PrintDocument();
		private DataTable dataIn;
		private int dongDangIn;    // dòng chi tiết sẽ in tiếp, dùng khi phiếu dài phải in nhiều trang
		private int trangDangIn;

		#region Khởi tạo

		public frmQuanLyKho()
		{
			InitializeComponent();

			Classes.GiaoDien.ApDung(this, btnThemPhieuNhap);
			Classes.GiaoDien.TieuDe(lblTieuDe, lblMoTa);
			Classes.GiaoDien.NhanManh(lblTongTienLech);
		}

		private void frmQuanLyKho_Load(object sender, EventArgs e)
		{
			function.NumberOnly(txtDonGiaBanMoi);

			LoadPhuTung();
			LoadPhieuNhap();
			LoadDieuChinh();
		}

		private void btnThoat_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Chắc chắn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				this.Close();
			}
		}

		#endregion

		#region Phụ tùng

		private void LoadPhuTung(string timPhuTung = "")
		{
			timPhuTung = function.EscapeSql(timPhuTung);

			string sqlDgvPhuTung = "Select pt.MaPhuTung, pt.TenPhuTung, ncc.TenNhaCungCap As TenNCC, pt.SoLuongTon, pt.DonGiaBan, pt.DonGiaNhap, pt.HanSuDung " +
				"From PhuTung pt Join NhaCungCap ncc On pt.MaNhaCungCap = ncc.MaNhaCungCap " +
				"Where pt.TrangThai = 1 And (pt.MaPhuTung Like '%" + timPhuTung + "%' Or pt.TenPhuTung Like N'%" + timPhuTung + "%') " +
				"Order By pt.MaPhuTung Asc";

			dgvPhuTung.AutoGenerateColumns = false;
			dgvPhuTung.DataSource = dtbase.ReadData(sqlDgvPhuTung);
			dgvPhuTung.ReadOnly = true;
		}

		private void txtTimPhuTung_TextChanged(object sender, EventArgs e)
		{
			LoadPhuTung(txtTimPhuTung.Text);
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

		private bool KiemTraChonPhuTung()
		{
			if (dgvPhuTung.CurrentRow == null)
			{
				MessageBox.Show("Chưa chọn phụ tùng!", "Thông báo");
				return false;
			}

			return true;
		}

		private void btnNgungSuDung_Click(object sender, EventArgs e)
		{
			if (!KiemTraChonPhuTung())
			{
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

		#endregion

		#region Điều chỉnh tồn và giá bán

		// Mã NV và lý do bắt buộc cho mọi lần điều chỉnh tồn/giá bán
		private bool KiemTraThongTinDieuChinh()
		{
			if (txtMaNVDieuChinh.Text.Trim() == "" || txtLyDoDieuChinh.Text.Trim() == "")
			{
				MessageBox.Show("Nhập mã NV và lý do điều chỉnh!", "Thông báo");
				return false;
			}

			if (!dtbase.KiemTraTonTai("NhanVien", "MaNhanVien", txtMaNVDieuChinh.Text.Trim()))
			{
				MessageBox.Show("Mã nhân viên không tồn tại!", "Thông báo");
				txtMaNVDieuChinh.Focus();
				return false;
			}

			return true;
		}

		// Ghi lịch sử và cập nhật phụ tùng trong 1 transaction, xong thì load lại phụ tùng và lịch sử
		private bool LuuDieuChinh(string sqlDieuChinh, string thongBaoLoi)
		{
			try
			{
				dtbase.ChangeDataTransaction(sqlDieuChinh);

				txtLyDoDieuChinh.Clear();

				LoadPhuTung();
				LoadDieuChinh();
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(thongBaoLoi + ex.Message, "Thông báo");
				return false;
			}
		}

		private void btnDieuChinhSoLuong_Click(object sender, EventArgs e)
		{
			if (!KiemTraChonPhuTung())
			{
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

			if (MessageBox.Show("Điều chỉnh tồn " + maPhuTung + ": " + soLuongTon + " -> " + soLuongTonMoi + " (tiền lệch " + tienLech.ToString("N0") + ")?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				string maDieuChinh = dtbase.GenerateMa("DieuChinhTonKho", "MaDieuChinh", "DC");
				string maNV = function.EscapeSql(txtMaNVDieuChinh.Text.Trim());
				string lyDo = function.EscapeSql(txtLyDoDieuChinh.Text.Trim());

				// Số lượng cũ lấy từ database
				string sqlDieuChinhSoLuong = $"""
					INSERT INTO dbo.DieuChinhTonKho (MaDieuChinh, MaPhuTung, MaNhanVien, NgayDieuChinh, SoLuongCu, SoLuongMoi, DonGiaNhap, LyDo)
					SELECT '{maDieuChinh}', MaPhuTung, '{maNV}', CAST(GETDATE() AS DATE), SoLuongTon, SoLuongTon + ({soLuongDieuChinh}), DonGiaNhap, N'{lyDo}'
					FROM dbo.PhuTung
					WHERE MaPhuTung = '{maPhuTung}';

					UPDATE dbo.PhuTung
					SET SoLuongTon = SoLuongTon + ({soLuongDieuChinh})
					WHERE MaPhuTung = '{maPhuTung}';
					""";

				if (LuuDieuChinh(sqlDieuChinhSoLuong, "Lỗi khi điều chỉnh tồn: "))
				{
					nudSoLuongDieuChinh.Value = 0;
				}
			}
		}

		private void btnDieuChinhGiaBan_Click(object sender, EventArgs e)
		{
			if (!KiemTraChonPhuTung())
			{
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

			// NumberOnly chỉ chặn lúc gõ phím, dán (Ctrl+V) vẫn đưa được chữ vào ô giá
			if (!decimal.TryParse(txtDonGiaBanMoi.Text, out decimal donGiaBanMoi))
			{
				MessageBox.Show("Giá bán không hợp lệ!", "Thông báo");
				return;
			}

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

			string xacNhan = "Điều chỉnh giá bán " + maPhuTung + ": " + donGiaBan.ToString("N0") + " -> " + donGiaBanMoi.ToString("N0") + "?";

			// Giá thị trường giảm thì được bán thấp hơn giá nhập, chỉ cảnh báo để tránh gõ nhầm
			if (donGiaBanMoi < donGiaNhap)
			{
				xacNhan = "Giá bán thấp hơn đơn giá nhập (" + donGiaNhap.ToString("N0") + ")!\n" + xacNhan;
			}

			if (MessageBox.Show(xacNhan, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				string maDieuChinhGia = dtbase.GenerateMa("DieuChinhGiaBan", "MaDieuChinhGia", "DG");
				string maNV = function.EscapeSql(txtMaNVDieuChinh.Text.Trim());
				string lyDo = function.EscapeSql(txtLyDoDieuChinh.Text.Trim());

				// Giá cũ lấy từ database
				string sqlDieuChinhGiaBan = $"""
					INSERT INTO dbo.DieuChinhGiaBan (MaDieuChinhGia, MaPhuTung, MaNhanVien, NgayDieuChinh, GiaBanCu, GiaBanMoi, LyDo)
					SELECT '{maDieuChinhGia}', MaPhuTung, '{maNV}', CAST(GETDATE() AS DATE), DonGiaBan, {donGiaBanMoi.ToString("0")}, N'{lyDo}'
					FROM dbo.PhuTung
					WHERE MaPhuTung = '{maPhuTung}';

					UPDATE dbo.PhuTung
					SET DonGiaBan = {donGiaBanMoi.ToString("0")}
					WHERE MaPhuTung = '{maPhuTung}';
					""";

				if (LuuDieuChinh(sqlDieuChinhGiaBan, "Lỗi khi điều chỉnh giá bán: "))
				{
					txtDonGiaBanMoi.Clear();
				}
			}
		}

		#endregion

		#region Lịch sử điều chỉnh

		private void LoadDieuChinh()
		{
			string timPhuTung = function.EscapeSql(txtTimDieuChinh.Text);

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

		private void txtTimDieuChinh_TextChanged(object sender, EventArgs e)
		{
			LoadDieuChinh();
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

		#endregion

		#region Phiếu nhập

		private void LoadPhieuNhap(string timPhieuNhap = "")
		{
			timPhieuNhap = function.EscapeSql(timPhieuNhap);

			string sqlPhieuNhap = "Select pn.MaPhieuNhap, pn.NgayNhap, Sum(ct.SoLuongNhap) As TongSoLuongNhap, Sum(ct.ThanhTien) As ThanhTien, pn.GhiChu, pn.MaNhanVienNhap " +
				"From PhieuNhap pn Join ChiTietPhieuNhap ct On pn.MaPhieuNhap = ct.MaPhieuNhap " +
				"Where pn.MaPhieuNhap Like '%" + timPhieuNhap + "%' " +
				"Group By pn.MaPhieuNhap, pn.NgayNhap, pn.GhiChu, pn.MaNhanVienNhap " +
				"Order By pn.MaPhieuNhap Asc";

			dgvPhieuNhap.DataSource = dtbase.ReadData(sqlPhieuNhap);

			dgvPhieuNhap.ReadOnly = true;
			dgvChiTietPhieuNhap.ReadOnly = true;

			if (dgvPhieuNhap.Rows.Count > 0)
			{
				LoadChiTietPhieuNhap(dgvPhieuNhap.Rows[0].Cells["colMaPhieuNhap"].Value.ToString());
			}
			else
			{
				// Không có phiếu nào thì xóa luôn chi tiết của phiếu đang hiện trước đó
				dgvChiTietPhieuNhap.DataSource = null;
				CapNhatNutPhieuNhap("");
			}
		}

		private void LoadChiTietPhieuNhap(string maPhieuNhap)
		{
			string sqlDgvChiTietPhieuNhap = "Select ct.MaPhieuNhap, ct.MaPhuTung, ct.SoLuongNhap, pt.DonGiaNhap, ct.ThanhTien, pt.HanSuDung " +
				"From PhuTung pt Join ChiTietPhieuNhap ct On pt.MaPhuTung = ct.MaPhuTung " +
				"Where ct.MaPhieuNhap = '" + maPhieuNhap + "'";

			dgvChiTietPhieuNhap.DataSource = dtbase.ReadData(sqlDgvChiTietPhieuNhap);

			CapNhatNutPhieuNhap(maPhieuNhap);
		}

		private void txtTimPhieuNhap_TextChanged(object sender, EventArgs e)
		{
			LoadPhieuNhap(txtTimPhieuNhap.Text);
		}

		private void dgvPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}

			LoadChiTietPhieuNhap(dgvPhieuNhap.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString());
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

		private bool KiemTraChonPhieuNhap(string thongBao)
		{
			if (dgvChiTietPhieuNhap.Rows.Count == 0 || dgvPhieuNhap.CurrentRow.IsNewRow == true)
			{
				MessageBox.Show(thongBao, "Thông báo");
				return false;
			}

			return true;
		}

		// Sửa/xóa được khi đã chọn phiếu và phụ tùng trong phiếu chưa vào dịch vụ; hanhDong là "sửa" hoặc "xóa"
		private bool KiemTraSuaXoaPhieuNhap(string hanhDong)
		{
			if (!KiemTraChonPhieuNhap("Chọn phiếu cần " + hanhDong + "!"))
			{
				return false;
			}

			if (PhieuNhapDaDungTrongDichVu(dgvPhieuNhap.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString()))
			{
				MessageBox.Show("Phiếu nhập có phụ tùng đã được dùng trong dịch vụ, không thể " + hanhDong + "!", "Thông báo");
				return false;
			}

			return true;
		}

		// Thêm và sửa phiếu đều mở form nhập phiếu, đóng form thì load lại phiếu nhập và tồn kho
		private void MoFormNhapPhieuNhap(string maPhieuNhap)
		{
			frmNhapPhieuNhap formNhapPhieuNhap = new frmNhapPhieuNhap(maPhieuNhap);
			formNhapPhieuNhap.ShowDialog();

			LoadPhieuNhap();
			LoadPhuTung();
		}

		private void btnThemPhieuNhap_Click(object sender, EventArgs e)
		{
			MoFormNhapPhieuNhap(dtbase.GenerateMa("PhieuNhap", "MaPhieuNhap", "PN"));
		}

		private void btnSuaPhieuNhap_Click(object sender, EventArgs e)
		{
			if (!KiemTraSuaXoaPhieuNhap("sửa"))
			{
				return;
			}

			MoFormNhapPhieuNhap(dgvPhieuNhap.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString());
		}

		private void btnXoaPhieuNhap_Click(object sender, EventArgs e)
		{
			if (!KiemTraSuaXoaPhieuNhap("xóa"))
			{
				return;
			}

			if (MessageBox.Show("Chắc chắn muốn xóa phiếu nhập?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				string maPhieuNhap = dgvPhieuNhap.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString();

				try
				{
					string sqlXoaPhieuNhap = $"""
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
						""";

					dtbase.ChangeDataTransaction(sqlXoaPhieuNhap);

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

		#endregion

		#region In phiếu nhập

		private void btnInPhieuNhap_Click(object sender, EventArgs e)
		{
			if (!KiemTraChonPhieuNhap("Chọn phiếu in!"))
			{
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

			printDocument.BeginPrint -= printDocument_BeginPrint;
			printDocument.BeginPrint += printDocument_BeginPrint;
			printDocument.PrintPage -= printDocument_PrintPage;
			printDocument.PrintPage += printDocument_PrintPage;

			PrintPreviewDialog preview = new PrintPreviewDialog();
			preview.Document = printDocument;
			preview.Width = 1000;
			preview.Height = 700;
			preview.ShowDialog();
		}

		// Mỗi lần bắt đầu in (xem trước hoặc in thật) thì in lại từ dòng đầu, trang 1
		private void printDocument_BeginPrint(object sender, PrintEventArgs e)
		{
			dongDangIn = 0;
			trangDangIn = 1;
		}

		private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
		{
			Graphics g = e.Graphics;

			Font fontTitle = new Font("Arial", 18, FontStyle.Bold);
			Font fontHeader = new Font("Arial", 10, FontStyle.Bold);
			Font font = new Font("Arial", 10);

			// Vùng in nằm trong lề trang (đơn vị 1/100 inch), bảng giãn theo bề ngang trang
			float x = e.MarginBounds.Left, y = e.MarginBounds.Top;
			float rongTrang = e.MarginBounds.Width;
			float cuoiTrang = e.MarginBounds.Bottom;
			float lineHeight = font.GetHeight(g) + 12;

			StringFormat canGiua = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			StringFormat canTrai = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
			StringFormat canPhai = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

			void VeSoTrang()
			{
				g.DrawString("Trang " + trangDangIn, font, Brushes.Gray, new RectangleF(x, cuoiTrang + 10, rongTrang, lineHeight), canGiua);
			}

			// Tiêu đề và thông tin chung chỉ in ở trang đầu
			if (dongDangIn == 0)
			{
				string maPhieu = dataIn.Rows[0]["MaPhieuNhap"].ToString();
				string nguoiNhap = dataIn.Rows[0]["MaNhanVienNhap"].ToString();
				DateTime ngayNhap = Convert.ToDateTime(dataIn.Rows[0]["NgayNhap"]);

				g.DrawString("PHIẾU NHẬP KHO", fontTitle, Brushes.Black, new RectangleF(x, y, rongTrang, 40), canGiua);
				y += 55;

				g.DrawString("Mã phiếu: " + maPhieu, font, Brushes.Black, new RectangleF(x, y, rongTrang / 2, lineHeight), canTrai);
				g.DrawString("Ngày nhập: " + ngayNhap.ToString("dd/MM/yyyy"), font, Brushes.Black, new RectangleF(x + rongTrang / 2, y, rongTrang / 2, lineHeight), canPhai);
				y += lineHeight;

				g.DrawString("Người nhập: " + nguoiNhap, font, Brushes.Black, new RectangleF(x, y, rongTrang, lineHeight), canTrai);
				y += lineHeight;

				g.DrawString("Nhà cung cấp:", fontHeader, Brushes.Black, new RectangleF(x, y, rongTrang, lineHeight), canTrai);
				y += lineHeight;

				// Tên nhà cung cấp dài thì tự xuống dòng trong bề ngang trang
				var nhaCungCap = dataIn.AsEnumerable().Select(r => r["TenNCC"].ToString()).Distinct();
				foreach (string tenNCC in nhaCungCap)
				{
					float caoNCC = g.MeasureString("- " + tenNCC, font, (int)rongTrang).Height;
					g.DrawString("- " + tenNCC, font, Brushes.Black, new RectangleF(x, y, rongTrang, caoNCC));
					y += caoNCC + 6;
				}

				y += 14;
			}

			// Độ rộng cột theo tỉ lệ bề ngang trang; cột Tên PT rộng nhất. Số tiền căn phải, STT và SL căn giữa
			float[] tiLe = { 0.06f, 0.13f, 0.39f, 0.08f, 0.16f, 0.18f };
			string[] headers = { "STT", "Mã PT", "Tên PT", "SL", "Đơn giá", "Thành tiền" };
			StringFormat[] canCot = { canGiua, canGiua, canTrai, canGiua, canPhai, canPhai };
			float[] widths = tiLe.Select(t => rongTrang * t).ToArray();

			// Dòng tiêu đề bảng in lại ở đầu mỗi trang
			float tableX = x;
			for (int i = 0; i < headers.Length; i++)
			{
				g.FillRectangle(Brushes.Gainsboro, tableX, y, widths[i], lineHeight);
				g.DrawRectangle(Pens.Black, tableX, y, widths[i], lineHeight);
				g.DrawString(headers[i], fontHeader, Brushes.Black, new RectangleF(tableX, y, widths[i], lineHeight), canGiua);
				tableX += widths[i];
			}
			y += lineHeight;

			bool daInDong = false;
			while (dongDangIn < dataIn.Rows.Count)
			{
				DataRow row = dataIn.Rows[dongDangIn];

				string[] values = { (dongDangIn + 1).ToString(), row["MaPhuTung"].ToString(), row["TenPhuTung"].ToString(), row["SoLuong"].ToString(), Convert.ToDecimal(row["DonGiaNhap"]).ToString("N0"), Convert.ToDecimal(row["ThanhTien"]).ToString("N0") };

				// Chiều cao dòng theo ô cao nhất: chữ dài tự xuống dòng trong ô, không đè lên ô khác
				float rowHeight = lineHeight;
				for (int j = 0; j < values.Length; j++)
				{
					rowHeight = Math.Max(rowHeight, g.MeasureString(values[j], font, (int)(widths[j] - 10)).Height + 12);
				}

				// Hết chỗ thì sang trang mới; dòng cuối phải chừa chỗ cho dòng tổng tiền
				bool dongCuoi = dongDangIn == dataIn.Rows.Count - 1;
				if (daInDong && y + rowHeight + (dongCuoi ? lineHeight * 2 : 0) > cuoiTrang)
				{
					VeSoTrang();
					trangDangIn++;
					e.HasMorePages = true;
					return;
				}

				tableX = x;
				for (int j = 0; j < values.Length; j++)
				{
					g.DrawRectangle(Pens.Black, tableX, y, widths[j], rowHeight);
					g.DrawString(values[j], font, Brushes.Black, new RectangleF(tableX + 5, y, widths[j] - 10, rowHeight), canCot[j]);
					tableX += widths[j];
				}

				y += rowHeight;
				dongDangIn++;
				daInDong = true;
			}

			decimal tongTien = dataIn.AsEnumerable().Sum(r => Convert.ToDecimal(r["ThanhTien"]));

			y += 15;
			g.DrawString("Tổng tiền: " + tongTien.ToString("N0") + " VNĐ", fontHeader, Brushes.Black, new RectangleF(x, y, rongTrang, lineHeight), canPhai);

			VeSoTrang();
			e.HasMorePages = false;
		}

		#endregion

		#region Xuất Excel

		// 1 file gồm 3 sheet: phụ tùng, lịch sử điều chỉnh tồn (kèm dòng tổng), lịch sử điều chỉnh giá bán
		private void btnXuatExcelPhuTung_Click(object sender, EventArgs e)
		{
			string duongDan = Classes.XuatExcel.ChonFileLuu("PhuTung");

			if (duongDan == "")
			{
				return;
			}

			using (XLWorkbook wb = new XLWorkbook())
			{
				Classes.XuatExcel.ThemSheet(wb, "Phụ tùng", dgvPhuTung);

				DataTable dataTonKho = (DataTable)dgvDieuChinhTonKho.DataSource;
				IXLWorksheet wsTonKho = Classes.XuatExcel.ThemSheet(wb, "Điều chỉnh tồn", dgvDieuChinhTonKho);

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

				Classes.XuatExcel.ThemSheet(wb, "Điều chỉnh giá bán", dgvDieuChinhGiaBan);

				Classes.XuatExcel.Luu(wb, duongDan);
			}
		}

		// 1 file gồm 2 sheet: phiếu nhập và chi tiết của các phiếu đang hiện trên lưới
		private void btnXuatExcelPhieuNhap_Click(object sender, EventArgs e)
		{
			string duongDan = Classes.XuatExcel.ChonFileLuu("PhieuNhap");

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
				Classes.XuatExcel.ThemSheet(wb, "Phiếu nhập", dgvPhieuNhap);

				Classes.XuatExcel.ThemSheet(wb, "Chi tiết phiếu nhập", dtbase.ReadData(sqlChiTiet),
					new string[] { "MaPhieuNhap", "MaPhuTung", "TenPhuTung", "TenNhaCungCap", "SoLuongNhap", "DonGiaNhap", "ThanhTien", "HanSuDung" },
					new string[] { "Mã phiếu nhập", "Mã phụ tùng", "Tên phụ tùng", "Nhà cung cấp", "Số lượng nhập", "Đơn giá nhập", "Thành tiền", "Hạn sử dụng" });

				Classes.XuatExcel.Luu(wb, duongDan);
			}
		}

		#endregion
	}
}
