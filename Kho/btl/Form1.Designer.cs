namespace btl
{
	partial class frmNhapPhieuNhap
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			groupBox1 = new GroupBox();
			btnXoaNCC = new Button();
			btnThemNCC = new Button();
			txtMaNVNhap = new TextBox();
			label4 = new Label();
			dtpNgayNhap = new DateTimePicker();
			label3 = new Label();
			cbNhaCungCap = new ComboBox();
			label2 = new Label();
			txtMaPhieuNhap = new TextBox();
			label1 = new Label();
			panel1 = new Panel();
			groupBox2 = new GroupBox();
			txtGhiChu = new RichTextBox();
			label12 = new Label();
			txtDonGiaBan = new TextBox();
			label11 = new Label();
			btnHuyPhuTung = new Button();
			btnLamMoiPhuTung = new Button();
			btnXoaPhuTung = new Button();
			btnSuaPhuTung = new Button();
			btnThemPhuTung = new Button();
			txtThanhTien = new TextBox();
			label10 = new Label();
			dtpHanSuDung = new DateTimePicker();
			label9 = new Label();
			txtDonGiaNhap = new TextBox();
			label8 = new Label();
			txtSoLuongNhap = new TextBox();
			label7 = new Label();
			txtTenPhuTung = new TextBox();
			label6 = new Label();
			cbMaPhuTung = new ComboBox();
			btnMoiPhuTung = new Button();
			label5 = new Label();
			dgvPhuTung = new DataGridView();
			panel2 = new Panel();
			btnThoat = new Button();
			btnLuuPhieu = new Button();
			colMaPhieuNhap = new DataGridViewTextBoxColumn();
			colTenNCC = new DataGridViewTextBoxColumn();
			colNgayNhap = new DataGridViewTextBoxColumn();
			colMaPhuTung = new DataGridViewTextBoxColumn();
			colTenPhuTung = new DataGridViewTextBoxColumn();
			colSoLuongNhap = new DataGridViewTextBoxColumn();
			colDonGiaNhap = new DataGridViewTextBoxColumn();
			colDonGiaBan = new DataGridViewTextBoxColumn();
			colThanhTien = new DataGridViewTextBoxColumn();
			colHanSuDung = new DataGridViewTextBoxColumn();
			colMaNhanVienNhap = new DataGridViewTextBoxColumn();
			colGhiChu = new DataGridViewTextBoxColumn();
			groupBox1.SuspendLayout();
			panel1.SuspendLayout();
			groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvPhuTung).BeginInit();
			panel2.SuspendLayout();
			SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(btnXoaNCC);
			groupBox1.Controls.Add(btnThemNCC);
			groupBox1.Controls.Add(txtMaNVNhap);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(dtpNgayNhap);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(cbNhaCungCap);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(txtMaPhieuNhap);
			groupBox1.Controls.Add(label1);
			groupBox1.Dock = DockStyle.Top;
			groupBox1.Location = new Point(0, 0);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(1295, 154);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "Thông tin phiếu nhập";
			// 
			// btnXoaNCC
			// 
			btnXoaNCC.Location = new Point(645, 80);
			btnXoaNCC.Name = "btnXoaNCC";
			btnXoaNCC.Size = new Size(142, 46);
			btnXoaNCC.TabIndex = 20;
			btnXoaNCC.Text = "Xóa NCC";
			btnXoaNCC.UseVisualStyleBackColor = true;
			btnXoaNCC.Click += btnXoaNCC_Click;
			// 
			// btnThemNCC
			// 
			btnThemNCC.Location = new Point(454, 80);
			btnThemNCC.Name = "btnThemNCC";
			btnThemNCC.Size = new Size(142, 46);
			btnThemNCC.TabIndex = 19;
			btnThemNCC.Text = "Thêm NCC";
			btnThemNCC.UseVisualStyleBackColor = true;
			btnThemNCC.Click += btnThemNCC_Click;
			// 
			// txtMaNVNhap
			// 
			txtMaNVNhap.Location = new Point(1020, 30);
			txtMaNVNhap.Name = "txtMaNVNhap";
			txtMaNVNhap.Size = new Size(250, 27);
			txtMaNVNhap.TabIndex = 7;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(894, 32);
			label4.Name = "label4";
			label4.Size = new Size(94, 20);
			label4.TabIndex = 6;
			label4.Text = "Mã NV nhập:";
			// 
			// dtpNgayNhap
			// 
			dtpNgayNhap.Location = new Point(580, 29);
			dtpNgayNhap.Name = "dtpNgayNhap";
			dtpNgayNhap.Size = new Size(250, 27);
			dtpNgayNhap.TabIndex = 5;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(454, 32);
			label3.Name = "label3";
			label3.Size = new Size(84, 20);
			label3.TabIndex = 4;
			label3.Text = "Ngày nhập:";
			// 
			// cbNhaCungCap
			// 
			cbNhaCungCap.FormattingEnabled = true;
			cbNhaCungCap.Location = new Point(149, 90);
			cbNhaCungCap.Name = "cbNhaCungCap";
			cbNhaCungCap.Size = new Size(246, 28);
			cbNhaCungCap.TabIndex = 3;
			cbNhaCungCap.TextChanged += cbNhaCungCap_TextChanged;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(20, 97);
			label2.Name = "label2";
			label2.Size = new Size(103, 20);
			label2.TabIndex = 2;
			label2.Text = "Nhà cung cấp:";
			// 
			// txtMaPhieuNhap
			// 
			txtMaPhieuNhap.Location = new Point(149, 28);
			txtMaPhieuNhap.Name = "txtMaPhieuNhap";
			txtMaPhieuNhap.Size = new Size(246, 27);
			txtMaPhieuNhap.TabIndex = 1;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(20, 33);
			label1.Name = "label1";
			label1.Size = new Size(111, 20);
			label1.TabIndex = 0;
			label1.Text = "Mã phiếu nhập:";
			// 
			// panel1
			// 
			panel1.Controls.Add(groupBox2);
			panel1.Controls.Add(dgvPhuTung);
			panel1.Dock = DockStyle.Fill;
			panel1.Location = new Point(0, 154);
			panel1.Name = "panel1";
			panel1.Size = new Size(1295, 647);
			panel1.TabIndex = 2;
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(txtGhiChu);
			groupBox2.Controls.Add(label12);
			groupBox2.Controls.Add(txtDonGiaBan);
			groupBox2.Controls.Add(label11);
			groupBox2.Controls.Add(btnHuyPhuTung);
			groupBox2.Controls.Add(btnLamMoiPhuTung);
			groupBox2.Controls.Add(btnXoaPhuTung);
			groupBox2.Controls.Add(btnSuaPhuTung);
			groupBox2.Controls.Add(btnThemPhuTung);
			groupBox2.Controls.Add(txtThanhTien);
			groupBox2.Controls.Add(label10);
			groupBox2.Controls.Add(dtpHanSuDung);
			groupBox2.Controls.Add(label9);
			groupBox2.Controls.Add(txtDonGiaNhap);
			groupBox2.Controls.Add(label8);
			groupBox2.Controls.Add(txtSoLuongNhap);
			groupBox2.Controls.Add(label7);
			groupBox2.Controls.Add(txtTenPhuTung);
			groupBox2.Controls.Add(label6);
			groupBox2.Controls.Add(cbMaPhuTung);
			groupBox2.Controls.Add(btnMoiPhuTung);
			groupBox2.Controls.Add(label5);
			groupBox2.Dock = DockStyle.Top;
			groupBox2.Location = new Point(0, 0);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new Size(1295, 248);
			groupBox2.TabIndex = 2;
			groupBox2.TabStop = false;
			groupBox2.Text = "Thông tin phụ tùng";
			// 
			// txtGhiChu
			// 
			txtGhiChu.Location = new Point(1020, 165);
			txtGhiChu.Name = "txtGhiChu";
			txtGhiChu.Size = new Size(250, 77);
			txtGhiChu.TabIndex = 24;
			txtGhiChu.Text = "";
			// 
			// label12
			// 
			label12.AutoSize = true;
			label12.Location = new Point(941, 165);
			label12.Name = "label12";
			label12.Size = new Size(61, 20);
			label12.TabIndex = 23;
			label12.Text = "Ghi chú:";
			// 
			// txtDonGiaBan
			// 
			txtDonGiaBan.Location = new Point(703, 111);
			txtDonGiaBan.Name = "txtDonGiaBan";
			txtDonGiaBan.Size = new Size(172, 27);
			txtDonGiaBan.TabIndex = 22;
			// 
			// label11
			// 
			label11.AutoSize = true;
			label11.Location = new Point(595, 114);
			label11.Name = "label11";
			label11.Size = new Size(94, 20);
			label11.TabIndex = 21;
			label11.Text = "Đơn giá bán:";
			// 
			// btnHuyPhuTung
			// 
			btnHuyPhuTung.Location = new Point(589, 165);
			btnHuyPhuTung.Name = "btnHuyPhuTung";
			btnHuyPhuTung.Size = new Size(141, 46);
			btnHuyPhuTung.TabIndex = 20;
			btnHuyPhuTung.Text = "Hủy";
			btnHuyPhuTung.UseVisualStyleBackColor = true;
			btnHuyPhuTung.Click += btnHuyPhuTung_Click;
			// 
			// btnLamMoiPhuTung
			// 
			btnLamMoiPhuTung.Location = new Point(774, 165);
			btnLamMoiPhuTung.Name = "btnLamMoiPhuTung";
			btnLamMoiPhuTung.Size = new Size(141, 46);
			btnLamMoiPhuTung.TabIndex = 19;
			btnLamMoiPhuTung.Text = "Làm mới";
			btnLamMoiPhuTung.UseVisualStyleBackColor = true;
			btnLamMoiPhuTung.Click += btnLamMoiPhuTung_Click;
			// 
			// btnXoaPhuTung
			// 
			btnXoaPhuTung.Location = new Point(397, 165);
			btnXoaPhuTung.Name = "btnXoaPhuTung";
			btnXoaPhuTung.Size = new Size(141, 46);
			btnXoaPhuTung.TabIndex = 18;
			btnXoaPhuTung.Text = "Xóa thông tin";
			btnXoaPhuTung.UseVisualStyleBackColor = true;
			btnXoaPhuTung.Click += btnXoaPhuTung_Click;
			// 
			// btnSuaPhuTung
			// 
			btnSuaPhuTung.Location = new Point(205, 165);
			btnSuaPhuTung.Name = "btnSuaPhuTung";
			btnSuaPhuTung.Size = new Size(141, 46);
			btnSuaPhuTung.TabIndex = 17;
			btnSuaPhuTung.Text = "Sửa thông tin";
			btnSuaPhuTung.UseVisualStyleBackColor = true;
			btnSuaPhuTung.Click += btnSuaPhuTung_Click;
			// 
			// btnThemPhuTung
			// 
			btnThemPhuTung.Location = new Point(20, 165);
			btnThemPhuTung.Name = "btnThemPhuTung";
			btnThemPhuTung.Size = new Size(141, 46);
			btnThemPhuTung.TabIndex = 16;
			btnThemPhuTung.Text = "Thêm phụ tùng";
			btnThemPhuTung.UseVisualStyleBackColor = true;
			btnThemPhuTung.Click += btnThemPhuTung_Click;
			// 
			// txtThanhTien
			// 
			txtThanhTien.Location = new Point(1020, 107);
			txtThanhTien.Name = "txtThanhTien";
			txtThanhTien.Size = new Size(250, 27);
			txtThanhTien.TabIndex = 15;
			// 
			// label10
			// 
			label10.AutoSize = true;
			label10.Location = new Point(897, 110);
			label10.Name = "label10";
			label10.Size = new Size(81, 20);
			label10.TabIndex = 14;
			label10.Text = "Thành tiền:";
			// 
			// dtpHanSuDung
			// 
			dtpHanSuDung.Location = new Point(1020, 50);
			dtpHanSuDung.Name = "dtpHanSuDung";
			dtpHanSuDung.Size = new Size(250, 27);
			dtpHanSuDung.TabIndex = 9;
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Location = new Point(894, 52);
			label9.Name = "label9";
			label9.Size = new Size(96, 20);
			label9.TabIndex = 8;
			label9.Text = "Hạn sử dụng:";
			// 
			// txtDonGiaNhap
			// 
			txtDonGiaNhap.Location = new Point(401, 111);
			txtDonGiaNhap.Name = "txtDonGiaNhap";
			txtDonGiaNhap.Size = new Size(172, 27);
			txtDonGiaNhap.TabIndex = 13;
			txtDonGiaNhap.TextChanged += txtDonGiaNhap_TextChanged;
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new Point(293, 114);
			label8.Name = "label8";
			label8.Size = new Size(102, 20);
			label8.TabIndex = 12;
			label8.Text = "Đơn giá nhập:";
			// 
			// txtSoLuongNhap
			// 
			txtSoLuongNhap.Location = new Point(149, 107);
			txtSoLuongNhap.Name = "txtSoLuongNhap";
			txtSoLuongNhap.Size = new Size(69, 27);
			txtSoLuongNhap.TabIndex = 11;
			txtSoLuongNhap.TextChanged += txtSoLuongNhap_TextChanged;
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new Point(18, 110);
			label7.Name = "label7";
			label7.Size = new Size(109, 20);
			label7.TabIndex = 10;
			label7.Text = "Số lượng nhập:";
			// 
			// txtTenPhuTung
			// 
			txtTenPhuTung.Location = new Point(580, 49);
			txtTenPhuTung.Name = "txtTenPhuTung";
			txtTenPhuTung.Size = new Size(250, 27);
			txtTenPhuTung.TabIndex = 9;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new Point(454, 51);
			label6.Name = "label6";
			label6.Size = new Size(98, 20);
			label6.TabIndex = 8;
			label6.Text = "Tên phụ tùng:";
			// 
			// cbMaPhuTung
			// 
			cbMaPhuTung.FormattingEnabled = true;
			cbMaPhuTung.Location = new Point(149, 49);
			cbMaPhuTung.Name = "cbMaPhuTung";
			cbMaPhuTung.Size = new Size(186, 28);
			cbMaPhuTung.TabIndex = 1;
			//
			// btnMoiPhuTung
			//
			btnMoiPhuTung.Location = new Point(339, 48);
			btnMoiPhuTung.Name = "btnMoiPhuTung";
			btnMoiPhuTung.Size = new Size(56, 30);
			btnMoiPhuTung.TabIndex = 2;
			btnMoiPhuTung.Text = "Mới";
			btnMoiPhuTung.UseVisualStyleBackColor = true;
			btnMoiPhuTung.Click += btnMoiPhuTung_Click;
			//
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(20, 52);
			label5.Name = "label5";
			label5.Size = new Size(96, 20);
			label5.TabIndex = 0;
			label5.Text = "Mã phụ tùng:";
			// 
			// dgvPhuTung
			// 
			dgvPhuTung.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvPhuTung.Columns.AddRange(new DataGridViewColumn[] { colMaPhieuNhap, colTenNCC, colNgayNhap, colMaPhuTung, colTenPhuTung, colSoLuongNhap, colDonGiaNhap, colDonGiaBan, colThanhTien, colHanSuDung, colMaNhanVienNhap, colGhiChu });
			dgvPhuTung.Location = new Point(3, 254);
			dgvPhuTung.Name = "dgvPhuTung";
			dgvPhuTung.RowHeadersWidth = 51;
			dgvPhuTung.Size = new Size(1289, 287);
			dgvPhuTung.TabIndex = 0;
			dgvPhuTung.CellClick += dgvPhuTung_CellClick;
			// 
			// panel2
			// 
			panel2.Controls.Add(btnThoat);
			panel2.Controls.Add(btnLuuPhieu);
			panel2.Dock = DockStyle.Bottom;
			panel2.Location = new Point(0, 701);
			panel2.Name = "panel2";
			panel2.Size = new Size(1295, 100);
			panel2.TabIndex = 3;
			// 
			// btnThoat
			// 
			btnThoat.Location = new Point(192, 24);
			btnThoat.Name = "btnThoat";
			btnThoat.Size = new Size(123, 57);
			btnThoat.TabIndex = 3;
			btnThoat.Text = "Thoát";
			btnThoat.UseVisualStyleBackColor = true;
			btnThoat.Click += btnThoat_Click;
			// 
			// btnLuuPhieu
			// 
			btnLuuPhieu.Location = new Point(31, 24);
			btnLuuPhieu.Name = "btnLuuPhieu";
			btnLuuPhieu.Size = new Size(123, 57);
			btnLuuPhieu.TabIndex = 1;
			btnLuuPhieu.Text = "Lưu phiếu";
			btnLuuPhieu.UseVisualStyleBackColor = true;
			btnLuuPhieu.Click += btnLuuPhieu_Click;
			// 
			// colMaPhieuNhap
			// 
			colMaPhieuNhap.DataPropertyName = "MaPhieuNhap";
			colMaPhieuNhap.HeaderText = "Mã phiếu nhập";
			colMaPhieuNhap.MinimumWidth = 6;
			colMaPhieuNhap.Name = "colMaPhieuNhap";
			colMaPhieuNhap.Width = 125;
			// 
			// colTenNCC
			// 
			colTenNCC.DataPropertyName = "TenNCC";
			colTenNCC.HeaderText = "Nhà cung cấp";
			colTenNCC.MinimumWidth = 6;
			colTenNCC.Name = "colTenNCC";
			colTenNCC.Width = 125;
			// 
			// colNgayNhap
			// 
			colNgayNhap.DataPropertyName = "NgayNhap";
			colNgayNhap.HeaderText = "Ngày nhập";
			colNgayNhap.MinimumWidth = 6;
			colNgayNhap.Name = "colNgayNhap";
			colNgayNhap.Width = 125;
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
			colTenPhuTung.Width = 125;
			// 
			// colSoLuongNhap
			// 
			colSoLuongNhap.DataPropertyName = "SoLuongNhap";
			colSoLuongNhap.HeaderText = "Số lượng nhập";
			colSoLuongNhap.MinimumWidth = 6;
			colSoLuongNhap.Name = "colSoLuongNhap";
			colSoLuongNhap.Width = 125;
			// 
			// colDonGiaNhap
			// 
			colDonGiaNhap.DataPropertyName = "DonGiaNhap";
			colDonGiaNhap.HeaderText = "Đơn giá nhập";
			colDonGiaNhap.MinimumWidth = 6;
			colDonGiaNhap.Name = "colDonGiaNhap";
			colDonGiaNhap.Width = 125;
			// 
			// colDonGiaBan
			// 
			colDonGiaBan.DataPropertyName = "DonGiaBan";
			colDonGiaBan.HeaderText = "Đơn giá bán";
			colDonGiaBan.MinimumWidth = 6;
			colDonGiaBan.Name = "colDonGiaBan";
			colDonGiaBan.Width = 125;
			// 
			// colThanhTien
			// 
			colThanhTien.DataPropertyName = "ThanhTien";
			colThanhTien.HeaderText = "Thành tiền";
			colThanhTien.MinimumWidth = 6;
			colThanhTien.Name = "colThanhTien";
			colThanhTien.Width = 125;
			// 
			// colHanSuDung
			// 
			colHanSuDung.DataPropertyName = "HanSuDung";
			colHanSuDung.HeaderText = "Hạn sử dụng";
			colHanSuDung.MinimumWidth = 6;
			colHanSuDung.Name = "colHanSuDung";
			colHanSuDung.Width = 125;
			// 
			// colMaNhanVienNhap
			// 
			colMaNhanVienNhap.DataPropertyName = "MaNhanVienNhap";
			colMaNhanVienNhap.HeaderText = "Mã nhân viên nhập";
			colMaNhanVienNhap.MinimumWidth = 6;
			colMaNhanVienNhap.Name = "colMaNhanVienNhap";
			colMaNhanVienNhap.Width = 125;
			// 
			// colGhiChu
			// 
			colGhiChu.DataPropertyName = "GhiChu";
			colGhiChu.HeaderText = "Ghi chú";
			colGhiChu.MinimumWidth = 6;
			colGhiChu.Name = "colGhiChu";
			colGhiChu.Width = 125;
			// 
			// frmNhapPhieuNhap
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1295, 801);
			Controls.Add(panel2);
			Controls.Add(panel1);
			Controls.Add(groupBox1);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			Name = "frmNhapPhieuNhap";
			Text = "Form nhập phiếu";
			Load += frmKho_Load;
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			panel1.ResumeLayout(false);
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dgvPhuTung).EndInit();
			panel2.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private GroupBox groupBox1;
		private TextBox txtMaNVNhap;
		private Label label4;
		private DateTimePicker dtpNgayNhap;
		private Label label3;
		private ComboBox cbNhaCungCap;
		private Label label2;
		private TextBox txtMaPhieuNhap;
		private Label label1;
		private Panel panel1;
		private DataGridView dgvPhuTung;
		private Panel panel2;
		private Button btnThoat;
		private Button btnLuuPhieu;
		private Button btnXoaNCC;
		private Button btnThemNCC;
		private GroupBox groupBox2;
		private Button btnXoaPhuTung;
		private Button btnSuaPhuTung;
		private Button btnThemPhuTung;
		private TextBox txtThanhTien;
		private Label label10;
		private DateTimePicker dtpHanSuDung;
		private Label label9;
		private TextBox txtDonGiaNhap;
		private Label label8;
		private TextBox txtSoLuongNhap;
		private Label label7;
		private TextBox txtTenPhuTung;
		private Label label6;
		private ComboBox cbMaPhuTung;
		private Button btnMoiPhuTung;
		private Label label5;
		private Button btnLamMoiPhuTung;
		private Button btnHuyPhuTung;
		private TextBox txtDonGiaBan;
		private Label label11;
		private RichTextBox txtGhiChu;
		private Label label12;
		private DataGridViewTextBoxColumn colMaPhieuNhap;
		private DataGridViewTextBoxColumn colTenNCC;
		private DataGridViewTextBoxColumn colNgayNhap;
		private DataGridViewTextBoxColumn colMaPhuTung;
		private DataGridViewTextBoxColumn colTenPhuTung;
		private DataGridViewTextBoxColumn colSoLuongNhap;
		private DataGridViewTextBoxColumn colDonGiaNhap;
		private DataGridViewTextBoxColumn colDonGiaBan;
		private DataGridViewTextBoxColumn colThanhTien;
		private DataGridViewTextBoxColumn colHanSuDung;
		private DataGridViewTextBoxColumn colMaNhanVienNhap;
		private DataGridViewTextBoxColumn colGhiChu;
	}
}
