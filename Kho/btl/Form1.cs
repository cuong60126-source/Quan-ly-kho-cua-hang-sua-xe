using System.Globalization;
using System.Data;
using System.Linq;

namespace btl
{
	public partial class frmNhapPhieuNhap : Form
	{
		Classes.DatabaseProcessor dtbase = new Classes.DatabaseProcessor();
		Classes.Function function = new Classes.Function();

		int selectedRowIndex = -1;

		public frmNhapPhieuNhap()
		{
			InitializeComponent();
		}
		public frmNhapPhieuNhap(string maPhieuNhap)
		{
			InitializeComponent();
			txtMaPhieuNhap.Text = maPhieuNhap;
			txtMaPhieuNhap.Enabled = false;
		}

		private void CheckBtnSua()
		{
			string maPhieuNhap = txtMaPhieuNhap.Text;

			string sqlCheck = "Select * From PhieuNhap Where MaPhieuNhap = '" + maPhieuNhap + "'";

			DataTable dataCheck = dtbase.ReadData(sqlCheck);

			if(dataCheck.Rows.Count > 0)
			{
				// MaNCC và HanSuDung nằm ở bảng PhuTung, không phải ChiTietPhieuNhap
				string sqlDgvPhuTung = "Select pn.MaPhieuNhap, ncc.TenNCC, pn.NgayNhap, pt.MaPhuTung, pt.TenPhuTung, ct.SoLuong As SoLuongNhap, ct.DonGiaNhap, pt.DonGiaBan, ct.ThanhTien, pt.HanSuDung, pn.MaNhanVienNhap, pn.GhiChu " +
					"From PhuTung pt Join ChiTietPhieuNhap ct On pt.MaPhuTung = ct.MaPhuTung " +
					"Join PhieuNhap pn On pn.MaPhieuNhap = ct.MaPhieuNhap " +
					"Join NhaCungCap ncc On pt.MaNCC = ncc.MaNCC " +
					"Where pn.MaPhieuNhap = '" + maPhieuNhap + "' " +
					"Order By ct.MaChiTietPhieuNhap";

				// Thêm từng dòng thay vì gán DataSource, để nút Thêm phụ tùng (Rows.Add) vẫn dùng được
				foreach (DataRow data in dtbase.ReadData(sqlDgvPhuTung).Rows)
				{
					dgvPhuTung.Rows.Add(
						data["MaPhieuNhap"],
						data["TenNCC"],
						data["NgayNhap"],
						data["MaPhuTung"],
						data["TenPhuTung"],
						data["SoLuongNhap"],
						Convert.ToDecimal(data["DonGiaNhap"]).ToString("0.##"),
						Convert.ToDecimal(data["DonGiaBan"]).ToString("0.##"),
						Convert.ToDecimal(data["ThanhTien"]).ToString("0.##"),
						data["HanSuDung"],
						data["MaNhanVienNhap"],
						data["GhiChu"]
						);
				}
			}
		}

		private void frmKho_Load(object sender, EventArgs e)
		{
			function.NumberOnly(txtSoLuongNhap);
			function.NumberOnly(txtDonGiaNhap);

			txtThanhTien.Enabled = false;
			dtpNgayNhap.Enabled = false;

			function.AddComboBox(cbNhaCungCap, dtbase.ReadData("Select TenNCC, MaNCC From NhaCungCap"), "TenNCC", "MaNCC");
			cbNhaCungCap.SelectedIndex = -1;
			function.AddComboBox(cbMaPhuTung, dtbase.ReadData("Select MaPhuTung From PhuTung"), "MaPhuTung", "MaPhuTung");
			cbMaPhuTung.SelectedIndex = -1;

			dgvPhuTung.AutoGenerateColumns = false;
			dgvPhuTung.DataSource = null;
			dgvPhuTung.Rows.Clear();
			dgvPhuTung.ReadOnly = true;

			btnXoaNCC.Enabled = false;
			btnSuaPhuTung.Enabled = false;
			btnXoaPhuTung.Enabled = false;
			btnHuyPhuTung.Enabled = false;

			CheckBtnSua();
		}

		private void ResetForm()
		{
			txtMaNVNhap.Clear();
			cbNhaCungCap.SelectedIndex = -1;
			cbNhaCungCap.Text = "";
			cbMaPhuTung.SelectedIndex = -1;
			cbMaPhuTung.Text = "";
			txtTenPhuTung.Clear();
			dtpHanSuDung.Value = DateTime.Now;
			txtSoLuongNhap.Clear();
			txtDonGiaBan.Clear();
			txtDonGiaNhap.Clear();
			txtThanhTien.Clear();
		}

		private void btnThemNCC_Click(object sender, EventArgs e)
		{
			frmThemNCC form = new frmThemNCC();
			form.ShowDialog();

			if (form.DialogResult == DialogResult.OK)
			{
				function.AddComboBox(cbNhaCungCap, dtbase.ReadData("Select TenNCC, MaNCC From NhaCungCap"), "TenNCC", "MaNCC");
				cbNhaCungCap.SelectedIndex = -1;
			}
		}

		private void btnXoaNCC_Click(object sender, EventArgs e)
		{
			if (cbNhaCungCap.SelectedValue == null || cbNhaCungCap.Text.Trim() == "")
			{
				MessageBox.Show("Chọn nhà cung cấp muốn xóa!", "Thông báo");
				return;
			}

			if (MessageBox.Show("Chắc chắn muốn xóa?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				string tenNCC = cbNhaCungCap.SelectedValue.ToString();
				string sql = "Delete From NhaCungCap Where MaNCC = '" + tenNCC + "'";

				dtbase.ChangeData(sql);

				MessageBox.Show("Xóa thành công!", "Thông báo");

				function.AddComboBox(cbNhaCungCap, dtbase.ReadData("Select TenNCC, MaNCC From NhaCungCap"), "TenNCC", "MaNCC");
				cbNhaCungCap.SelectedIndex = -1;
			}
		}

		private string GenerateMaPhuTung()
		{
			string sql = "Select Max(Try_Cast(Substring(MaPhuTung, 3, Len(MaPhuTung)) As int)) From PhuTung Where MaPhuTung Like 'PT%'";

			DataTable data = dtbase.ReadData(sql);

			int so = 0;
			if (data.Rows.Count > 0 && data.Rows[0][0] != DBNull.Value)
			{
				so = Convert.ToInt32(data.Rows[0][0]);
			}

			// Mã mới trên lưới chưa được lưu vào database nên phải xét thêm
			foreach (DataGridViewRow row in dgvPhuTung.Rows)
			{
				string maPhuTung = row.Cells["colMaPhuTung"].Value?.ToString() ?? "";
				if (maPhuTung.StartsWith("PT") && int.TryParse(maPhuTung.Substring(2), out int soTrenLuoi) && soTrenLuoi > so)
				{
					so = soTrenLuoi;
				}
			}

			return "PT" + (so + 1).ToString("D3");
		}

		private void btnMoiPhuTung_Click(object sender, EventArgs e)
		{
			cbMaPhuTung.SelectedIndex = -1;
			cbMaPhuTung.Text = GenerateMaPhuTung();
			txtTenPhuTung.Clear();
			txtTenPhuTung.Focus();
		}

		private void btnThemPhuTung_Click(object sender, EventArgs e)
		{
			if (cbMaPhuTung.Text.Trim() == "" || txtTenPhuTung.Text.Trim() == "" || txtSoLuongNhap.Text.Trim() == "" || txtDonGiaNhap.Text.Trim() == "" || txtDonGiaBan.Text.Trim() == "" || txtMaPhieuNhap.Text.Trim() == "" || txtMaNVNhap.Text.Trim() == "" || cbNhaCungCap.SelectedValue == null)
			{
				MessageBox.Show("Không được để trống!", "Thông báo");
				return;
			}

			if(Convert.ToDecimal(txtDonGiaNhap.Text) >= Convert.ToDecimal(txtDonGiaBan.Text))
			{
				MessageBox.Show("Giá bán không hợp lệ!", "Thông báo");
					return;
			}

			if (dtpHanSuDung.Value.Date < DateTime.Today)
			{
				MessageBox.Show("Giá trị hạn sử dụng không hợp lệ", "Thông báo");
				dtpHanSuDung.Focus();
				return;
			}

			if (Convert.ToInt32(txtSoLuongNhap.Text) <= 0 || Double.Parse(txtDonGiaNhap.Text) <= 0)
			{
				MessageBox.Show("Giá trị số lượng hoặc đơn giá nhập không hợp lệ!", "Thông báo");
				return;
			}

			dgvPhuTung.Rows.Add(
				txtMaPhieuNhap.Text,
				cbNhaCungCap.Text,
				dtpNgayNhap.Value.Date,
				cbMaPhuTung.Text,
				txtTenPhuTung.Text,
				txtSoLuongNhap.Text,
				txtDonGiaNhap.Text,
				txtDonGiaBan.Text,
				txtThanhTien.Text,
				dtpHanSuDung.Value.Date,
				txtMaNVNhap.Text,
				txtGhiChu.Text
				);

			ResetForm();
			txtMaPhieuNhap.Enabled = false;
			btnXoaNCC.Enabled = false;
		}

		private void CalThanhTien()
		{
			if (double.TryParse(txtSoLuongNhap.Text, out double soLuong) && double.TryParse(txtDonGiaNhap.Text, out double donGia))
			{
				txtThanhTien.Text = (soLuong * donGia).ToString();
			}
			else
			{
				txtThanhTien.Clear();
			}
		}

		private void txtSoLuongNhap_TextChanged(object sender, EventArgs e)
		{
			if (txtDonGiaNhap.Text.Trim() == "")
			{
				return;
			}
			CalThanhTien();
		}

		private void txtDonGiaNhap_TextChanged(object sender, EventArgs e)
		{
			if (txtSoLuongNhap.Text.Trim() == "")
			{
				return;
			}
			CalThanhTien();
		}

		private void cbNhaCungCap_TextChanged(object sender, EventArgs e)
		{
			btnXoaNCC.Enabled = true;
		}

		private void dgvPhuTung_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			selectedRowIndex = e.RowIndex;

			btnSuaPhuTung.Enabled = true;
			btnXoaPhuTung.Enabled = true;
			btnHuyPhuTung.Enabled = true;
			btnThemPhuTung.Enabled = false;
			btnXoaNCC.Enabled = false;

			txtMaPhieuNhap.Text = dgvPhuTung.CurrentRow.Cells["colMaPhieuNhap"].Value.ToString();
			dtpNgayNhap.Value = Convert.ToDateTime(dgvPhuTung.CurrentRow.Cells["colNgayNhap"].Value);
			txtMaNVNhap.Text = dgvPhuTung.CurrentRow.Cells["colMaNhanVienNhap"].Value.ToString();
			cbNhaCungCap.Text = dgvPhuTung.CurrentRow.Cells["colTenNCC"].Value.ToString();
			cbMaPhuTung.Text = dgvPhuTung.CurrentRow.Cells["colMaPhuTung"].Value.ToString();
			txtTenPhuTung.Text = dgvPhuTung.CurrentRow.Cells["colTenPhuTung"].Value.ToString();
			dtpHanSuDung.Value = Convert.ToDateTime(dgvPhuTung.CurrentRow.Cells["colHanSuDung"].Value);
			txtSoLuongNhap.Text = dgvPhuTung.CurrentRow.Cells["colSoLuongNhap"].Value.ToString();
			txtDonGiaNhap.Text = dgvPhuTung.CurrentRow.Cells["colDonGiaNhap"].Value.ToString();
			txtDonGiaBan.Text = dgvPhuTung.CurrentRow.Cells["colDonGiaBan"].Value.ToString();
			txtThanhTien.Text = dgvPhuTung.CurrentRow.Cells["colThanhTien"].Value.ToString();
			txtGhiChu.Text = dgvPhuTung.CurrentRow.Cells["colGhiChu"].Value.ToString();
		}

		private void btnSuaPhuTung_Click(object sender, EventArgs e)
		{
			if (selectedRowIndex == -1)
			{
				return;
			}

			if (MessageBox.Show("Xác nhận sửa?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				DataGridViewRow row = dgvPhuTung.Rows[selectedRowIndex];
				row.Cells["colNgayNhap"].Value = dtpNgayNhap.Value;
				row.Cells["colMaNhanVienNhap"].Value = txtMaNVNhap.Text;
				row.Cells["colTenNCC"].Value = cbNhaCungCap.Text;
				row.Cells["colMaPhuTung"].Value = cbMaPhuTung.Text;
				row.Cells["colTenPhuTung"].Value = txtTenPhuTung.Text;
				row.Cells["colHanSuDung"].Value = dtpHanSuDung.Value;
				row.Cells["colSoLuongNhap"].Value = txtSoLuongNhap.Text;
				row.Cells["colDonGiaNhap"].Value = txtDonGiaNhap.Text;
				row.Cells["colDonGiaBan"].Value = txtDonGiaBan.Text;
				row.Cells["colThanhTien"].Value = txtThanhTien.Text;
				row.Cells["colGhiChu"].Value = txtGhiChu.Text;

				ResetForm();

				btnThemPhuTung.Enabled = true;
				btnSuaPhuTung.Enabled = false;
				btnXoaPhuTung.Enabled = false;
				btnHuyPhuTung.Enabled = false;
				btnXoaNCC.Enabled = false;
			}
			else
			{
				return;
			}
		}

		private void btnXoaPhuTung_Click(object sender, EventArgs e)
		{
			if (selectedRowIndex == -1)
			{
				return;
			}

			if (MessageBox.Show("Xác nhận xóa?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				dgvPhuTung.Rows.RemoveAt(selectedRowIndex);

				selectedRowIndex = -1;

				btnSuaPhuTung.Enabled = false;
				btnXoaPhuTung.Enabled = false;
				btnHuyPhuTung.Enabled = false;
				btnThemPhuTung.Enabled = true;
			}

			if (dgvPhuTung.Rows.Count == 0)
			{
				txtMaPhieuNhap.Enabled = true;
			}
		}

		private void btnHuyPhuTung_Click(object sender, EventArgs e)
		{
			selectedRowIndex = -1;

			btnSuaPhuTung.Enabled = false;
			btnXoaPhuTung.Enabled = false;
			btnHuyPhuTung.Enabled = false;
			btnThemPhuTung.Enabled = true;

			dgvPhuTung.ClearSelection();
		}

		private void btnLamMoiPhuTung_Click(object sender, EventArgs e)
		{
			cbMaPhuTung.SelectedIndex = -1;
			cbMaPhuTung.Text = "";
			txtTenPhuTung.Clear();
			dtpHanSuDung.Value = DateTime.Now;
			txtSoLuongNhap.Clear();
			txtDonGiaNhap.Clear();
			txtDonGiaBan.Clear();
			txtThanhTien.Clear();
		}

		// Escape dấu ' để tên/ghi chú có dấu nháy không làm hỏng câu SQL
		private string EscapeSql(object? value)
		{
			return (value?.ToString() ?? "").Replace("'", "''");
		}

		private void btnLuuPhieu_Click(object sender, EventArgs e)
		{
			var rows = dgvPhuTung.Rows.Cast<DataGridViewRow>().Where(row => !row.IsNewRow).ToList();

			if (rows.Count == 0)
			{
				MessageBox.Show("Thêm thông tin phiếu nhập trước!", "Thông báo");
				return;
			}

			string maPhieuNhap = txtMaPhieuNhap.Text.Trim();
			DateTime ngayNhap = Convert.ToDateTime(rows[0].Cells["colNgayNhap"].Value);
			string ngayNhapSql = ngayNhap.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
			string maNV = rows[0].Cells["colMaNhanVienNhap"].Value?.ToString()?.Trim() ?? "";
			string ghiChu = rows[0].Cells["colGhiChu"].Value?.ToString() ?? "";

			// Kiểm tra mã NV trên lưới, vì txtMaNVNhap bị xóa trắng sau mỗi lần thêm phụ tùng
			string sqlNhanVien = "Select * " +
				"From NhanVien " +
				"Where MaNhanVien = '" + EscapeSql(maNV) + "'";
			DataTable dataNhanVien = dtbase.ReadData(sqlNhanVien);

			if (dataNhanVien.Rows.Count == 0)
			{
				MessageBox.Show("Mã nhân viên không tồn tại!");
				txtMaNVNhap.Focus();
				return;
			}

			List<string> phuTungMoi = new List<string>();
			for (int i = 0; i < rows.Count; i++)
			{
				DataGridViewRow row = rows[i];
				int soLuongNhap = Convert.ToInt32(row.Cells["colSoLuongNhap"].Value);
				decimal donGiaNhap = Convert.ToDecimal(row.Cells["colDonGiaNhap"].Value);
				decimal donGiaBan = Convert.ToDecimal(row.Cells["colDonGiaBan"].Value);
				DateTime hanSuDung = Convert.ToDateTime(row.Cells["colHanSuDung"].Value);

				phuTungMoi.Add("(" +
					(i + 1) + ", " +
					"'" + EscapeSql(row.Cells["colMaPhuTung"].Value) + "', " +
					"N'" + EscapeSql(row.Cells["colTenPhuTung"].Value) + "', " +
					"N'" + EscapeSql(row.Cells["colTenNCC"].Value) + "', " +
					"'" + hanSuDung.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "', " +
					soLuongNhap + ", " +
					donGiaNhap.ToString(CultureInfo.InvariantCulture) + ", " +
					donGiaBan.ToString(CultureInfo.InvariantCulture) +
					")");
			}

			// Cả phiếu được lưu trong 1 transaction: lỗi ở bất kỳ bước nào thì không lưu gì cả.
			// Dùng chung cho thêm mới và sửa phiếu: phiếu mới thì "số lượng cũ" = 0.
			string sqlLuuPhieu = $"""
				SET XACT_ABORT ON;

				BEGIN TRY
				    BEGIN TRANSACTION;

				    DECLARE @MaPhieuNhap VARCHAR(10) = '{EscapeSql(maPhieuNhap)}';
				    DECLARE @Loi NVARCHAR(400);

				    -- Các dòng phụ tùng trên lưới
				    DECLARE @Moi TABLE (
				        STT INT NOT NULL,
				        MaPhuTung VARCHAR(10) NOT NULL,
				        TenPhuTung NVARCHAR(100) NOT NULL,
				        TenNCC NVARCHAR(100) NOT NULL,
				        HanSuDung DATE NOT NULL,
				        SoLuong INT NOT NULL,
				        DonGiaNhap DECIMAL(18,2) NOT NULL,
				        DonGiaBan DECIMAL(18,2) NOT NULL
				    );

				    INSERT INTO @Moi (STT, MaPhuTung, TenPhuTung, TenNCC, HanSuDung, SoLuong, DonGiaNhap, DonGiaBan)
				    VALUES
				        {string.Join(",\n        ", phuTungMoi)};

				    -- Số lượng cũ (đang lưu trong database) và mới (trên lưới) của từng phụ tùng
				    DECLARE @ChenhLech TABLE (
				        MaPhuTung VARCHAR(10) NOT NULL PRIMARY KEY,
				        SoLuongCu INT NOT NULL,
				        SoLuongMoi INT NOT NULL
				    );

				    INSERT INTO @ChenhLech (MaPhuTung, SoLuongCu, SoLuongMoi)
				    SELECT X.MaPhuTung, SUM(X.SoLuongCu), SUM(X.SoLuongMoi)
				    FROM
				    (
				        SELECT CT.MaPhuTung, CT.SoLuong AS SoLuongCu, 0 AS SoLuongMoi
				        FROM dbo.ChiTietPhieuNhap CT
				        WHERE CT.MaPhieuNhap = @MaPhieuNhap

				        UNION ALL

				        SELECT M.MaPhuTung, 0, M.SoLuong
				        FROM @Moi M
				    ) X
				    GROUP BY X.MaPhuTung;

				    -- 1. Không cho sửa số lượng phụ tùng đã dùng trong dịch vụ (đã có trong phiếu tiếp nhận)
				    SELECT TOP 1 @Loi = N'Phụ tùng ' + CL.MaPhuTung + N' đã được dùng trong dịch vụ, không thể sửa số lượng!'
				    FROM @ChenhLech CL
				    WHERE CL.SoLuongCu > 0
				      AND CL.SoLuongCu <> CL.SoLuongMoi
				      AND EXISTS
				      (
				          SELECT 1
				          FROM dbo.ChiTietPhieuTiepNhan CTTN
				          JOIN dbo.DichVu DV ON DV.MaDichVu = CTTN.MaDichVu
				          WHERE DV.MaPhuTung = CL.MaPhuTung
				      );

				    IF @Loi IS NOT NULL
				        THROW 50001, @Loi, 1;

				    -- 2. Không cho số lượng tồn bị âm khi giảm số lượng nhập
				    SELECT TOP 1 @Loi = N'Số lượng tồn của phụ tùng ' + PT.MaPhuTung + N' không đủ để giảm!'
				    FROM dbo.PhuTung PT
				    JOIN @ChenhLech CL ON CL.MaPhuTung = PT.MaPhuTung
				    WHERE PT.SoLuongTon + CL.SoLuongMoi - CL.SoLuongCu < 0;

				    IF @Loi IS NOT NULL
				        THROW 50002, @Loi, 1;

				    -- 3. Nhà cung cấp phải tồn tại
				    SELECT TOP 1 @Loi = N'Không tìm thấy nhà cung cấp ' + M.TenNCC + N'!'
				    FROM @Moi M
				    WHERE NOT EXISTS (SELECT 1 FROM dbo.NhaCungCap NCC WHERE NCC.TenNCC = M.TenNCC);

				    IF @Loi IS NOT NULL
				        THROW 50003, @Loi, 1;

				    -- 4. Phiếu nhập: sửa nếu đã có, thêm nếu chưa có
				    IF EXISTS (SELECT 1 FROM dbo.PhieuNhap WHERE MaPhieuNhap = @MaPhieuNhap)
				        UPDATE dbo.PhieuNhap
				        SET NgayNhap = '{ngayNhapSql}',
				            MaNhanVienNhap = '{EscapeSql(maNV)}',
				            GhiChu = N'{EscapeSql(ghiChu)}'
				        WHERE MaPhieuNhap = @MaPhieuNhap;
				    ELSE
				        INSERT INTO dbo.PhieuNhap (MaPhieuNhap, NgayNhap, MaNhanVienNhap, GhiChu)
				        VALUES (@MaPhieuNhap, '{ngayNhapSql}', '{EscapeSql(maNV)}', N'{EscapeSql(ghiChu)}');

				    -- 5. Phụ tùng chưa có thì tạo mới từ phiếu nhập (tồn = 0, cộng ở bước 6)
				    INSERT INTO dbo.PhuTung (MaPhuTung, TenPhuTung, MaNCC, HanSuDung, DonGiaNhap, DonGiaBan, SoLuongTon)
				    SELECT M.MaPhuTung, M.TenPhuTung, NCC.MaNCC, M.HanSuDung, M.DonGiaNhap, M.DonGiaBan, 0
				    FROM @Moi M
				    CROSS APPLY
				    (
				        SELECT TOP 1 N.MaNCC
				        FROM dbo.NhaCungCap N
				        WHERE N.TenNCC = M.TenNCC
				    ) NCC
				    WHERE M.STT = (SELECT MIN(M2.STT) FROM @Moi M2 WHERE M2.MaPhuTung = M.MaPhuTung)
				      AND NOT EXISTS (SELECT 1 FROM dbo.PhuTung PT WHERE PT.MaPhuTung = M.MaPhuTung);

				    -- 6. Cập nhật số lượng tồn theo chênh lệch mới - cũ
				    UPDATE PT
				    SET PT.SoLuongTon = PT.SoLuongTon + CL.SoLuongMoi - CL.SoLuongCu
				    FROM dbo.PhuTung PT
				    JOIN @ChenhLech CL ON CL.MaPhuTung = PT.MaPhuTung;

				    -- 7. Phụ tùng chỉ thuộc phiếu này thì cập nhật thông tin theo lưới
				    UPDATE PT
				    SET PT.TenPhuTung = M.TenPhuTung,
				        PT.MaNCC = NCC.MaNCC,
				        PT.HanSuDung = M.HanSuDung,
				        PT.DonGiaNhap = M.DonGiaNhap,
				        PT.DonGiaBan = M.DonGiaBan
				    FROM dbo.PhuTung PT
				    JOIN @Moi M ON M.MaPhuTung = PT.MaPhuTung
				    CROSS APPLY
				    (
				        SELECT TOP 1 N.MaNCC
				        FROM dbo.NhaCungCap N
				        WHERE N.TenNCC = M.TenNCC
				    ) NCC
				    WHERE M.STT = (SELECT MAX(M2.STT) FROM @Moi M2 WHERE M2.MaPhuTung = M.MaPhuTung)
				      AND NOT EXISTS
				      (
				          SELECT 1
				          FROM dbo.ChiTietPhieuNhap CT
				          WHERE CT.MaPhuTung = PT.MaPhuTung
				            AND CT.MaPhieuNhap <> @MaPhieuNhap
				      );

				    -- 8. Ghi lại chi tiết phiếu nhập
				    DELETE FROM dbo.ChiTietPhieuNhap
				    WHERE MaPhieuNhap = @MaPhieuNhap;

				    INSERT INTO dbo.ChiTietPhieuNhap (MaPhieuNhap, MaPhuTung, SoLuong, DonGiaNhap)
				    SELECT @MaPhieuNhap, M.MaPhuTung, M.SoLuong, M.DonGiaNhap
				    FROM @Moi M
				    ORDER BY M.STT;

				    -- 9. Phụ tùng bị bỏ khỏi phiếu, không còn phiếu nhập/dịch vụ nào dùng và hết tồn thì xóa
				    DELETE PT
				    FROM dbo.PhuTung PT
				    JOIN @ChenhLech CL ON CL.MaPhuTung = PT.MaPhuTung
				    WHERE CL.SoLuongMoi = 0
				      AND PT.SoLuongTon = 0
				      AND NOT EXISTS (SELECT 1 FROM dbo.ChiTietPhieuNhap CT WHERE CT.MaPhuTung = PT.MaPhuTung)
				      AND NOT EXISTS (SELECT 1 FROM dbo.DichVu DV WHERE DV.MaPhuTung = PT.MaPhuTung);

				    -- 10. Tính lại tổng tiền phiếu nhập
				    UPDATE PN
				    SET PN.TongTien = ISNULL(Tong.TongTien, 0)
				    FROM dbo.PhieuNhap PN
				    OUTER APPLY
				    (
				        SELECT SUM(CT.ThanhTien) AS TongTien
				        FROM dbo.ChiTietPhieuNhap CT
				        WHERE CT.MaPhieuNhap = PN.MaPhieuNhap
				    ) Tong
				    WHERE PN.MaPhieuNhap = @MaPhieuNhap;

				    COMMIT TRANSACTION;
				END TRY
				BEGIN CATCH
				    IF @@TRANCOUNT > 0
				        ROLLBACK TRANSACTION;

				    THROW;
				END CATCH;
				""";

			try
			{
				dtbase.ChangeData(sqlLuuPhieu);

				MessageBox.Show("Lưu phiếu nhập thành công!", "Thông báo");

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi lưu phiếu nhập: " + ex.Message, "Thông báo");
			}
		}

		private void btnThoat_Click(object sender, EventArgs e)
		{
			if(MessageBox.Show("Chắc chắn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				this.Close();
			}
		}
	}
}