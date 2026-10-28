namespace btl
{
	partial class frmQuanLyKho
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
			pnlTimPhuTung = new Panel();
			txtTimPhuTung = new TextBox();
			lblTimPhuTung = new Label();
			grpPhuTung = new GroupBox();
			lblSoLuongDieuChinh = new Label();
			nudSoLuongDieuChinh = new NumericUpDown();
			btnDieuChinhSoLuong = new Button();
			lblDonGiaBanMoi = new Label();
			txtDonGiaBanMoi = new TextBox();
			btnDieuChinhGiaBan = new Button();
			btnNgungSuDung = new Button();
			dgvPhuTung = new DataGridView();
			colMaPhuTung = new DataGridViewTextBoxColumn();
			colTenPhuTung = new DataGridViewTextBoxColumn();
			colTenNCC = new DataGridViewTextBoxColumn();
			colSoLuongTon = new DataGridViewTextBoxColumn();
			colDonGiaBan = new DataGridViewTextBoxColumn();
			colDonGiaNhap = new DataGridViewTextBoxColumn();
			colHanSuDung = new DataGridViewTextBoxColumn();
			pnlTimPhieuNhap = new Panel();
			txtTimPhieuNhap = new TextBox();
			lblTimPhieuNhap = new Label();
			grpPhieuNhap = new GroupBox();
			dgvPhieuNhap = new DataGridView();
			colMaPhieuNhap = new DataGridViewTextBoxColumn();
			colNgayNhap = new DataGridViewTextBoxColumn();
			colTongSoLuongNhap = new DataGridViewTextBoxColumn();
			colThanhTienNhap = new DataGridViewTextBoxColumn();
			colGhiChu = new DataGridViewTextBoxColumn();
			colMaNhanVienNhap = new DataGridViewTextBoxColumn();
			grpChiTietPhieuNhap = new GroupBox();
			dgvChiTietPhieuNhap = new DataGridView();
			colChiTietMaPhieuNhap = new DataGridViewTextBoxColumn();
			colChiTietMaPhuTung = new DataGridViewTextBoxColumn();
			colChiTietSoLuongNhap = new DataGridViewTextBoxColumn();
			colChiTietDonGiaNhap = new DataGridViewTextBoxColumn();
			colChiTietThanhTien = new DataGridViewTextBoxColumn();
			colChiTietHanSuDung = new DataGridViewTextBoxColumn();
			pnlChucNangPhieuNhap = new Panel();
			btnInPhieuNhap = new Button();
			btnThoat = new Button();
			btnXoaPhieuNhap = new Button();
			btnSuaPhieuNhap = new Button();
			btnThemPhieuNhap = new Button();
			pnlTimPhuTung.SuspendLayout();
			grpPhuTung.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)nudSoLuongDieuChinh).BeginInit();
			((System.ComponentModel.ISupportInitialize)dgvPhuTung).BeginInit();
			pnlTimPhieuNhap.SuspendLayout();
			grpPhieuNhap.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvPhieuNhap).BeginInit();
			grpChiTietPhieuNhap.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvChiTietPhieuNhap).BeginInit();
			pnlChucNangPhieuNhap.SuspendLayout();
			SuspendLayout();
			//
			// pnlTimPhuTung
			//
			pnlTimPhuTung.Controls.Add(txtTimPhuTung);
			pnlTimPhuTung.Controls.Add(lblTimPhuTung);
			pnlTimPhuTung.Location = new Point(12, 12);
			pnlTimPhuTung.Name = "pnlTimPhuTung";
			pnlTimPhuTung.Size = new Size(860, 58);
			pnlTimPhuTung.TabIndex = 0;
			//
			// txtTimPhuTung
			//
			txtTimPhuTung.Location = new Point(107, 17);
			txtTimPhuTung.Name = "txtTimPhuTung";
			txtTimPhuTung.Size = new Size(278, 27);
			txtTimPhuTung.TabIndex = 1;
			txtTimPhuTung.TextChanged += txtTimPhuTung_TextChanged;
			//
			// lblTimPhuTung
			//
			lblTimPhuTung.AutoSize = true;
			lblTimPhuTung.Location = new Point(55, 20);
			lblTimPhuTung.Name = "lblTimPhuTung";
			lblTimPhuTung.Size = new Size(37, 20);
			lblTimPhuTung.TabIndex = 0;
			lblTimPhuTung.Text = "Tìm:";
			//
			// grpPhuTung
			//
			grpPhuTung.Controls.Add(lblSoLuongDieuChinh);
			grpPhuTung.Controls.Add(nudSoLuongDieuChinh);
			grpPhuTung.Controls.Add(btnDieuChinhSoLuong);
			grpPhuTung.Controls.Add(lblDonGiaBanMoi);
			grpPhuTung.Controls.Add(txtDonGiaBanMoi);
			grpPhuTung.Controls.Add(btnDieuChinhGiaBan);
			grpPhuTung.Controls.Add(btnNgungSuDung);
			grpPhuTung.Controls.Add(dgvPhuTung);
			grpPhuTung.Location = new Point(12, 76);
			grpPhuTung.Name = "grpPhuTung";
			grpPhuTung.Size = new Size(860, 728);
			grpPhuTung.TabIndex = 1;
			grpPhuTung.TabStop = false;
			grpPhuTung.Text = "Danh sách phụ tùng";
			//
			// lblSoLuongDieuChinh
			//
			lblSoLuongDieuChinh.AutoSize = true;
			lblSoLuongDieuChinh.Location = new Point(16, 630);
			lblSoLuongDieuChinh.Name = "lblSoLuongDieuChinh";
			lblSoLuongDieuChinh.Size = new Size(145, 20);
			lblSoLuongDieuChinh.TabIndex = 17;
			lblSoLuongDieuChinh.Text = "Số lượng điều chỉnh:";
			//
			// nudSoLuongDieuChinh
			//
			nudSoLuongDieuChinh.Location = new Point(167, 628);
			nudSoLuongDieuChinh.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
			nudSoLuongDieuChinh.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
			nudSoLuongDieuChinh.Name = "nudSoLuongDieuChinh";
			nudSoLuongDieuChinh.Size = new Size(100, 27);
			nudSoLuongDieuChinh.TabIndex = 18;
			//
			// btnDieuChinhSoLuong
			//
			btnDieuChinhSoLuong.Location = new Point(280, 615);
			btnDieuChinhSoLuong.Name = "btnDieuChinhSoLuong";
			btnDieuChinhSoLuong.Size = new Size(127, 49);
			btnDieuChinhSoLuong.TabIndex = 19;
			btnDieuChinhSoLuong.Text = "Điều chỉnh tồn";
			btnDieuChinhSoLuong.UseVisualStyleBackColor = true;
			btnDieuChinhSoLuong.Click += btnDieuChinhSoLuong_Click;
			//
			// lblDonGiaBanMoi
			//
			lblDonGiaBanMoi.AutoSize = true;
			lblDonGiaBanMoi.Location = new Point(16, 685);
			lblDonGiaBanMoi.Name = "lblDonGiaBanMoi";
			lblDonGiaBanMoi.Size = new Size(95, 20);
			lblDonGiaBanMoi.TabIndex = 20;
			lblDonGiaBanMoi.Text = "Giá bán mới:";
			//
			// txtDonGiaBanMoi
			//
			txtDonGiaBanMoi.Location = new Point(167, 683);
			txtDonGiaBanMoi.Name = "txtDonGiaBanMoi";
			txtDonGiaBanMoi.Size = new Size(100, 27);
			txtDonGiaBanMoi.TabIndex = 21;
			//
			// btnDieuChinhGiaBan
			//
			btnDieuChinhGiaBan.Location = new Point(280, 670);
			btnDieuChinhGiaBan.Name = "btnDieuChinhGiaBan";
			btnDieuChinhGiaBan.Size = new Size(127, 49);
			btnDieuChinhGiaBan.TabIndex = 22;
			btnDieuChinhGiaBan.Text = "Điều chỉnh giá bán";
			btnDieuChinhGiaBan.UseVisualStyleBackColor = true;
			btnDieuChinhGiaBan.Click += btnDieuChinhGiaBan_Click;
			//
			// btnNgungSuDung
			//
			btnNgungSuDung.Location = new Point(727, 670);
			btnNgungSuDung.Name = "btnNgungSuDung";
			btnNgungSuDung.RightToLeft = RightToLeft.No;
			btnNgungSuDung.Size = new Size(127, 49);
			btnNgungSuDung.TabIndex = 16;
			btnNgungSuDung.Text = "Ngưng sử dụng";
			btnNgungSuDung.UseVisualStyleBackColor = true;
			btnNgungSuDung.Click += btnNgungSuDung_Click;
			//
			// dgvPhuTung
			//
			dgvPhuTung.AllowUserToAddRows = false;
			dgvPhuTung.AllowUserToDeleteRows = false;
			dgvPhuTung.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvPhuTung.Columns.AddRange(new DataGridViewColumn[] { colMaPhuTung, colTenPhuTung, colTenNCC, colSoLuongTon, colDonGiaBan, colDonGiaNhap, colHanSuDung });
			dgvPhuTung.Location = new Point(6, 26);
			dgvPhuTung.Name = "dgvPhuTung";
			dgvPhuTung.RowHeadersWidth = 51;
			dgvPhuTung.Size = new Size(848, 583);
			dgvPhuTung.TabIndex = 0;
			dgvPhuTung.CellClick += dgvPhuTung_CellClick;
			//
			// colMaPhuTung
			//
			colMaPhuTung.DataPropertyName = "MaPhuTung";
			colMaPhuTung.HeaderText = "Mã phụ tùng";
			colMaPhuTung.MinimumWidth = 6;
			colMaPhuTung.Name = "colMaPhuTung";
			colMaPhuTung.Width = 125;
			//
			// colTenPhuTung
			//
			colTenPhuTung.DataPropertyName = "TenPhuTung";
			colTenPhuTung.HeaderText = "Tên phụ tùng";
			colTenPhuTung.MinimumWidth = 6;
			colTenPhuTung.Name = "colTenPhuTung";
			colTenPhuTung.Width = 250;
			//
			// colTenNCC
			//
			colTenNCC.DataPropertyName = "TenNCC";
			colTenNCC.HeaderText = "Nhà cung cấp";
			colTenNCC.MinimumWidth = 6;
			colTenNCC.Name = "colTenNCC";
			colTenNCC.Width = 250;
			//
			// colSoLuongTon
			//
			colSoLuongTon.DataPropertyName = "SoLuongTon";
			colSoLuongTon.HeaderText = "Số lượng tồn";
			colSoLuongTon.MinimumWidth = 6;
			colSoLuongTon.Name = "colSoLuongTon";
			colSoLuongTon.Width = 125;
			//
			// colDonGiaBan
			//
			colDonGiaBan.DataPropertyName = "DonGiaBan";
			colDonGiaBan.HeaderText = "Đơn giá bán";
			colDonGiaBan.MinimumWidth = 6;
			colDonGiaBan.Name = "colDonGiaBan";
			colDonGiaBan.Width = 125;
			//
			// colDonGiaNhap
			//
			colDonGiaNhap.DataPropertyName = "DonGiaNhap";
			colDonGiaNhap.HeaderText = "Đơn giá nhập";
			colDonGiaNhap.MinimumWidth = 6;
			colDonGiaNhap.Name = "colDonGiaNhap";
			colDonGiaNhap.Width = 125;
			//
			// colHanSuDung
			//
			colHanSuDung.DataPropertyName = "HanSuDung";
			colHanSuDung.HeaderText = "Hạn sử dụng";
			colHanSuDung.MinimumWidth = 6;
			colHanSuDung.Name = "colHanSuDung";
			colHanSuDung.Width = 178;
			//
			// pnlTimPhieuNhap
			//
			pnlTimPhieuNhap.Controls.Add(txtTimPhieuNhap);
			pnlTimPhieuNhap.Controls.Add(lblTimPhieuNhap);
			pnlTimPhieuNhap.Location = new Point(884, 12);
			pnlTimPhieuNhap.Name = "pnlTimPhieuNhap";
			pnlTimPhieuNhap.Size = new Size(858, 58);
			pnlTimPhieuNhap.TabIndex = 2;
			//
			// txtTimPhieuNhap
			//
			txtTimPhieuNhap.Location = new Point(103, 17);
			txtTimPhieuNhap.Name = "txtTimPhieuNhap";
			txtTimPhieuNhap.Size = new Size(239, 27);
			txtTimPhieuNhap.TabIndex = 1;
			txtTimPhieuNhap.TextChanged += txtTimPhieuNhap_TextChanged;
			//
			// lblTimPhieuNhap
			//
			lblTimPhieuNhap.AutoSize = true;
			lblTimPhieuNhap.Location = new Point(41, 20);
			lblTimPhieuNhap.Name = "lblTimPhieuNhap";
			lblTimPhieuNhap.Size = new Size(37, 20);
			lblTimPhieuNhap.TabIndex = 0;
			lblTimPhieuNhap.Text = "Tìm:";
			//
			// grpPhieuNhap
			//
			grpPhieuNhap.Controls.Add(dgvPhieuNhap);
			grpPhieuNhap.Location = new Point(884, 76);
			grpPhieuNhap.Name = "grpPhieuNhap";
			grpPhieuNhap.Size = new Size(858, 296);
			grpPhieuNhap.TabIndex = 3;
			grpPhieuNhap.TabStop = false;
			grpPhieuNhap.Text = "Danh sách phiếu nhập";
			//
			// dgvPhieuNhap
			//
			dgvPhieuNhap.AllowUserToAddRows = false;
			dgvPhieuNhap.AllowUserToDeleteRows = false;
			dgvPhieuNhap.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvPhieuNhap.Columns.AddRange(new DataGridViewColumn[] { colMaPhieuNhap, colNgayNhap, colTongSoLuongNhap, colThanhTienNhap, colGhiChu, colMaNhanVienNhap });
			dgvPhieuNhap.Location = new Point(6, 26);
			dgvPhieuNhap.Name = "dgvPhieuNhap";
			dgvPhieuNhap.RowHeadersWidth = 51;
			dgvPhieuNhap.Size = new Size(846, 264);
			dgvPhieuNhap.TabIndex = 0;
			dgvPhieuNhap.CellClick += dgvPhieuNhap_CellClick;
			//
			// colMaPhieuNhap
			//
			colMaPhieuNhap.DataPropertyName = "MaPhieuNhap";
			colMaPhieuNhap.HeaderText = "Mã phiếu nhập";
			colMaPhieuNhap.MinimumWidth = 6;
			colMaPhieuNhap.Name = "colMaPhieuNhap";
			colMaPhieuNhap.Width = 140;
			//
			// colNgayNhap
			//
			colNgayNhap.DataPropertyName = "NgayNhap";
			colNgayNhap.HeaderText = "Ngày nhập";
			colNgayNhap.MinimumWidth = 6;
			colNgayNhap.Name = "colNgayNhap";
			colNgayNhap.Width = 175;
			//
			// colTongSoLuongNhap
			//
			colTongSoLuongNhap.DataPropertyName = "TongSoLuongNhap";
			colTongSoLuongNhap.HeaderText = "Tổng số lượng nhập";
			colTongSoLuongNhap.MinimumWidth = 6;
			colTongSoLuongNhap.Name = "colTongSoLuongNhap";
			colTongSoLuongNhap.Width = 125;
			//
			// colThanhTienNhap
			//
			colThanhTienNhap.DataPropertyName = "ThanhTien";
			colThanhTienNhap.HeaderText = "Thành tiền";
			colThanhTienNhap.MinimumWidth = 6;
			colThanhTienNhap.Name = "colThanhTienNhap";
			colThanhTienNhap.Width = 175;
			//
			// colGhiChu
			//
			colGhiChu.DataPropertyName = "GhiChu";
			colGhiChu.HeaderText = "Ghi chú";
			colGhiChu.MinimumWidth = 6;
			colGhiChu.Name = "colGhiChu";
			colGhiChu.Width = 125;
			//
			// colMaNhanVienNhap
			//
			colMaNhanVienNhap.DataPropertyName = "MaNhanVienNhap";
			colMaNhanVienNhap.HeaderText = "Mã NV nhập";
			colMaNhanVienNhap.MinimumWidth = 6;
			colMaNhanVienNhap.Name = "colMaNhanVienNhap";
			colMaNhanVienNhap.Width = 200;
			//
			// grpChiTietPhieuNhap
			//
			grpChiTietPhieuNhap.Controls.Add(dgvChiTietPhieuNhap);
			grpChiTietPhieuNhap.Location = new Point(884, 378);
			grpChiTietPhieuNhap.Name = "grpChiTietPhieuNhap";
			grpChiTietPhieuNhap.Size = new Size(858, 299);
			grpChiTietPhieuNhap.TabIndex = 4;
			grpChiTietPhieuNhap.TabStop = false;
			grpChiTietPhieuNhap.Text = "Thông tin phiếu nhập";
			//
			// dgvChiTietPhieuNhap
			//
			dgvChiTietPhieuNhap.AllowUserToAddRows = false;
			dgvChiTietPhieuNhap.AllowUserToDeleteRows = false;
			dgvChiTietPhieuNhap.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvChiTietPhieuNhap.Columns.AddRange(new DataGridViewColumn[] { colChiTietMaPhieuNhap, colChiTietMaPhuTung, colChiTietSoLuongNhap, colChiTietDonGiaNhap, colChiTietThanhTien, colChiTietHanSuDung });
			dgvChiTietPhieuNhap.Location = new Point(6, 26);
			dgvChiTietPhieuNhap.Name = "dgvChiTietPhieuNhap";
			dgvChiTietPhieuNhap.RowHeadersWidth = 51;
			dgvChiTietPhieuNhap.Size = new Size(846, 267);
			dgvChiTietPhieuNhap.TabIndex = 0;
			//
			// colChiTietMaPhieuNhap
			//
			colChiTietMaPhieuNhap.DataPropertyName = "MaPhieuNhap";
			colChiTietMaPhieuNhap.HeaderText = "Mã phiếu nhập";
			colChiTietMaPhieuNhap.MinimumWidth = 6;
			colChiTietMaPhieuNhap.Name = "colChiTietMaPhieuNhap";
			colChiTietMaPhieuNhap.Width = 125;
			//
			// colChiTietMaPhuTung
			//
			colChiTietMaPhuTung.DataPropertyName = "MaPhuTung";
			colChiTietMaPhuTung.HeaderText = "Mã phụ tùng";
			colChiTietMaPhuTung.MinimumWidth = 6;
			colChiTietMaPhuTung.Name = "colChiTietMaPhuTung";
			colChiTietMaPhuTung.Width = 125;
			//
			// colChiTietSoLuongNhap
			//
			colChiTietSoLuongNhap.DataPropertyName = "SoLuongNhap";
			colChiTietSoLuongNhap.HeaderText = "Số lượng nhập";
			colChiTietSoLuongNhap.MinimumWidth = 6;
			colChiTietSoLuongNhap.Name = "colChiTietSoLuongNhap";
			colChiTietSoLuongNhap.Width = 125;
			//
			// colChiTietDonGiaNhap
			//
			colChiTietDonGiaNhap.DataPropertyName = "DonGiaNhap";
			colChiTietDonGiaNhap.HeaderText = "Đơn giá nhập";
			colChiTietDonGiaNhap.MinimumWidth = 6;
			colChiTietDonGiaNhap.Name = "colChiTietDonGiaNhap";
			colChiTietDonGiaNhap.Width = 125;
			//
			// colChiTietThanhTien
			//
			colChiTietThanhTien.DataPropertyName = "ThanhTien";
			colChiTietThanhTien.HeaderText = "Thành Tiền";
			colChiTietThanhTien.MinimumWidth = 6;
			colChiTietThanhTien.Name = "colChiTietThanhTien";
			colChiTietThanhTien.Width = 125;
			//
			// colChiTietHanSuDung
			//
			colChiTietHanSuDung.DataPropertyName = "HanSuDung";
			colChiTietHanSuDung.HeaderText = "Hạn sử dụng";
			colChiTietHanSuDung.MinimumWidth = 6;
			colChiTietHanSuDung.Name = "colChiTietHanSuDung";
			colChiTietHanSuDung.Width = 125;
			//
			// pnlChucNangPhieuNhap
			//
			pnlChucNangPhieuNhap.Controls.Add(btnInPhieuNhap);
			pnlChucNangPhieuNhap.Controls.Add(btnThoat);
			pnlChucNangPhieuNhap.Controls.Add(btnXoaPhieuNhap);
			pnlChucNangPhieuNhap.Controls.Add(btnSuaPhieuNhap);
			pnlChucNangPhieuNhap.Controls.Add(btnThemPhieuNhap);
			pnlChucNangPhieuNhap.Location = new Point(884, 683);
			pnlChucNangPhieuNhap.Name = "pnlChucNangPhieuNhap";
			pnlChucNangPhieuNhap.Size = new Size(858, 121);
			pnlChucNangPhieuNhap.TabIndex = 5;
			//
			// btnInPhieuNhap
			//
			btnInPhieuNhap.Location = new Point(525, 34);
			btnInPhieuNhap.Name = "btnInPhieuNhap";
			btnInPhieuNhap.Size = new Size(139, 53);
			btnInPhieuNhap.TabIndex = 11;
			btnInPhieuNhap.Text = "In phiếu nhập";
			btnInPhieuNhap.UseVisualStyleBackColor = true;
			btnInPhieuNhap.Click += btnInPhieuNhap_Click;
			//
			// btnThoat
			//
			btnThoat.Location = new Point(690, 34);
			btnThoat.Name = "btnThoat";
			btnThoat.Size = new Size(139, 53);
			btnThoat.TabIndex = 9;
			btnThoat.Text = "Thoát";
			btnThoat.UseVisualStyleBackColor = true;
			btnThoat.Click += btnThoat_Click;
			//
			// btnXoaPhieuNhap
			//
			btnXoaPhieuNhap.Location = new Point(354, 34);
			btnXoaPhieuNhap.Name = "btnXoaPhieuNhap";
			btnXoaPhieuNhap.Size = new Size(139, 53);
			btnXoaPhieuNhap.TabIndex = 7;
			btnXoaPhieuNhap.Text = "Xóa phiếu nhập";
			btnXoaPhieuNhap.UseVisualStyleBackColor = true;
			btnXoaPhieuNhap.Click += btnXoaPhieuNhap_Click;
			//
			// btnSuaPhieuNhap
			//
			btnSuaPhieuNhap.Location = new Point(191, 34);
			btnSuaPhieuNhap.Name = "btnSuaPhieuNhap";
			btnSuaPhieuNhap.Size = new Size(139, 53);
			btnSuaPhieuNhap.TabIndex = 6;
			btnSuaPhieuNhap.Text = "Sửa phiếu nhập";
			btnSuaPhieuNhap.UseVisualStyleBackColor = true;
			btnSuaPhieuNhap.Click += btnSuaPhieuNhap_Click;
			//
			// btnThemPhieuNhap
			//
			btnThemPhieuNhap.Location = new Point(29, 34);
			btnThemPhieuNhap.Name = "btnThemPhieuNhap";
			btnThemPhieuNhap.Size = new Size(139, 53);
			btnThemPhieuNhap.TabIndex = 5;
			btnThemPhieuNhap.Text = "Thêm phiếu nhập";
			btnThemPhieuNhap.UseVisualStyleBackColor = true;
			btnThemPhieuNhap.Click += btnThemPhieuNhap_Click;
			//
			// frmQuanLyKho
			//
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1754, 816);
			Controls.Add(pnlChucNangPhieuNhap);
			Controls.Add(grpChiTietPhieuNhap);
			Controls.Add(grpPhieuNhap);
			Controls.Add(pnlTimPhieuNhap);
			Controls.Add(grpPhuTung);
			Controls.Add(pnlTimPhuTung);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			Name = "frmQuanLyKho";
			Text = "frmQuanLyKho";
			Load += frmQuanLyKho_Load;
			pnlTimPhuTung.ResumeLayout(false);
			pnlTimPhuTung.PerformLayout();
			grpPhuTung.ResumeLayout(false);
			grpPhuTung.PerformLayout();
			((System.ComponentModel.ISupportInitialize)nudSoLuongDieuChinh).EndInit();
			((System.ComponentModel.ISupportInitialize)dgvPhuTung).EndInit();
			pnlTimPhieuNhap.ResumeLayout(false);
			pnlTimPhieuNhap.PerformLayout();
			grpPhieuNhap.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvPhieuNhap).EndInit();
			grpChiTietPhieuNhap.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvChiTietPhieuNhap).EndInit();
			pnlChucNangPhieuNhap.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Panel pnlTimPhuTung;
		private TextBox txtTimPhuTung;
		private Label lblTimPhuTung;
		private GroupBox grpPhuTung;
		private Label lblSoLuongDieuChinh;
		private NumericUpDown nudSoLuongDieuChinh;
		private Button btnDieuChinhSoLuong;
		private Label lblDonGiaBanMoi;
		private TextBox txtDonGiaBanMoi;
		private Button btnDieuChinhGiaBan;
		private Button btnNgungSuDung;
		private DataGridView dgvPhuTung;
		private DataGridViewTextBoxColumn colMaPhuTung;
		private DataGridViewTextBoxColumn colTenPhuTung;
		private DataGridViewTextBoxColumn colTenNCC;
		private DataGridViewTextBoxColumn colSoLuongTon;
		private DataGridViewTextBoxColumn colDonGiaBan;
		private DataGridViewTextBoxColumn colDonGiaNhap;
		private DataGridViewTextBoxColumn colHanSuDung;
		private Panel pnlTimPhieuNhap;
		private TextBox txtTimPhieuNhap;
		private Label lblTimPhieuNhap;
		private GroupBox grpPhieuNhap;
		private DataGridView dgvPhieuNhap;
		private DataGridViewTextBoxColumn colMaPhieuNhap;
		private DataGridViewTextBoxColumn colNgayNhap;
		private DataGridViewTextBoxColumn colTongSoLuongNhap;
		private DataGridViewTextBoxColumn colThanhTienNhap;
		private DataGridViewTextBoxColumn colGhiChu;
		private DataGridViewTextBoxColumn colMaNhanVienNhap;
		private GroupBox grpChiTietPhieuNhap;
		private DataGridView dgvChiTietPhieuNhap;
		private DataGridViewTextBoxColumn colChiTietMaPhieuNhap;
		private DataGridViewTextBoxColumn colChiTietMaPhuTung;
		private DataGridViewTextBoxColumn colChiTietSoLuongNhap;
		private DataGridViewTextBoxColumn colChiTietDonGiaNhap;
		private DataGridViewTextBoxColumn colChiTietThanhTien;
		private DataGridViewTextBoxColumn colChiTietHanSuDung;
		private Panel pnlChucNangPhieuNhap;
		private Button btnInPhieuNhap;
		private Button btnThoat;
		private Button btnXoaPhieuNhap;
		private Button btnSuaPhieuNhap;
		private Button btnThemPhieuNhap;
	}
}
