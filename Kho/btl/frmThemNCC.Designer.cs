namespace btl
{
	partial class frmThemNCC
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
			label1 = new Label();
			lblMoTa = new Label();
			label2 = new Label();
			txtMaNCC = new TextBox();
			txtSDT = new TextBox();
			label3 = new Label();
			txtTenNCC = new TextBox();
			label4 = new Label();
			label5 = new Label();
			btnXacNhan = new Button();
			btnHuy = new Button();
			txtDiaChi = new RichTextBox();
			SuspendLayout();
			// 
			// label1
			// 
			label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			label1.Location = new Point(0, 8);
			label1.Name = "label1";
			label1.Size = new Size(711, 42);
			label1.TabIndex = 0;
			label1.Text = "THÊM NHÀ CUNG CẤP";
			label1.TextAlign = ContentAlignment.MiddleCenter;
			//
			// lblMoTa
			//
			lblMoTa.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			lblMoTa.Location = new Point(0, 50);
			lblMoTa.Name = "lblMoTa";
			lblMoTa.Size = new Size(711, 26);
			lblMoTa.TabIndex = 11;
			lblMoTa.Text = "Nhập thông tin nhà cung cấp mới";
			lblMoTa.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(43, 93);
			label2.Name = "label2";
			label2.Size = new Size(66, 20);
			label2.TabIndex = 1;
			label2.Text = "Mã NCC:";
			// 
			// txtMaNCC
			// 
			txtMaNCC.Location = new Point(135, 90);
			txtMaNCC.Name = "txtMaNCC";
			txtMaNCC.Size = new Size(183, 27);
			txtMaNCC.TabIndex = 2;
			// 
			// txtSDT
			// 
			txtSDT.Location = new Point(475, 90);
			txtSDT.Name = "txtSDT";
			txtSDT.Size = new Size(179, 27);
			txtSDT.TabIndex = 4;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(388, 93);
			label3.Name = "label3";
			label3.Size = new Size(36, 20);
			label3.TabIndex = 3;
			label3.Text = "SĐT";
			// 
			// txtTenNCC
			// 
			txtTenNCC.Location = new Point(135, 162);
			txtTenNCC.Name = "txtTenNCC";
			txtTenNCC.Size = new Size(183, 27);
			txtTenNCC.TabIndex = 6;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(43, 165);
			label4.Name = "label4";
			label4.Size = new Size(68, 20);
			label4.TabIndex = 5;
			label4.Text = "Tên NCC:";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(388, 165);
			label5.Name = "label5";
			label5.Size = new Size(58, 20);
			label5.TabIndex = 7;
			label5.Text = "Địa chỉ:";
			// 
			// btnXacNhan
			// 
			btnXacNhan.Location = new Point(148, 260);
			btnXacNhan.Name = "btnXacNhan";
			btnXacNhan.Size = new Size(147, 48);
			btnXacNhan.TabIndex = 9;
			btnXacNhan.Text = "Xác nhận";
			btnXacNhan.UseVisualStyleBackColor = true;
			btnXacNhan.Click += btnXacNhan_Click;
			// 
			// btnHuy
			// 
			btnHuy.Location = new Point(387, 260);
			btnHuy.Name = "btnHuy";
			btnHuy.Size = new Size(147, 48);
			btnHuy.TabIndex = 10;
			btnHuy.Text = "Hủy";
			btnHuy.UseVisualStyleBackColor = true;
			btnHuy.Click += btnHuy_Click;
			// 
			// txtDiaChi
			// 
			txtDiaChi.Location = new Point(475, 165);
			txtDiaChi.Name = "txtDiaChi";
			txtDiaChi.Size = new Size(179, 76);
			txtDiaChi.TabIndex = 11;
			txtDiaChi.Text = "";
			// 
			// frmThemNCC
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(711, 339);
			Controls.Add(txtDiaChi);
			Controls.Add(btnHuy);
			Controls.Add(btnXacNhan);
			Controls.Add(label5);
			Controls.Add(txtTenNCC);
			Controls.Add(label4);
			Controls.Add(txtSDT);
			Controls.Add(label3);
			Controls.Add(txtMaNCC);
			Controls.Add(label2);
			Controls.Add(lblMoTa);
			Controls.Add(label1);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			Name = "frmThemNCC";
			Text = "Thêm nhà cung cấp";
			Load += frmThemNCC_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label label1;
		private Label lblMoTa;
		private Label label2;
		private TextBox txtMaNCC;
		private TextBox txtSDT;
		private Label label3;
		private TextBox txtTenNCC;
		private Label label4;
		private Label label5;
		private Button btnXacNhan;
		private Button btnHuy;
		private RichTextBox txtDiaChi;
	}
}