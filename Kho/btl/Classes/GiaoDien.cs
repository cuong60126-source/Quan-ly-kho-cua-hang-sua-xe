using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace btl.Classes
{
	// Giao diện chung cho các form: nền sáng, tiêu đề xanh, khung trắng, nút phẳng, bảng gọn
	public static class GiaoDien
	{
		public static readonly Color MauNen = Color.FromArgb(244, 247, 251);
		public static readonly Color MauKhung = Color.White;
		public static readonly Color MauChinh = Color.FromArgb(30, 99, 181);
		public static readonly Color MauChinhDam = Color.FromArgb(22, 80, 150);
		public static readonly Color MauChu = Color.FromArgb(33, 37, 41);
		public static readonly Color MauChuPhu = Color.FromArgb(96, 104, 116);
		public static readonly Color MauVien = Color.FromArgb(196, 210, 228);
		public static readonly Color MauLuoi = Color.FromArgb(226, 232, 240);
		public static readonly Color MauTieuDeBang = Color.FromArgb(238, 243, 250);
		public static readonly Color MauDongChon = Color.FromArgb(212, 229, 249);
		public static readonly Color MauDongXen = Color.FromArgb(250, 252, 254);
		public static readonly Color MauRe = Color.FromArgb(234, 242, 252);

		static readonly Font FontThuong = new Font("Segoe UI", 9F);
		static readonly Font FontDam = new Font("Segoe UI", 9F, FontStyle.Bold);
		static readonly Font FontTieuDe = new Font("Segoe UI", 18F, FontStyle.Bold);

		// Gọi sau InitializeComponent(); nutChinh là các nút hành động chính (nền xanh, chữ trắng)
		public static void ApDung(Form form, params Button[] nutChinh)
		{
			form.BackColor = MauNen;
			form.ForeColor = MauChu;
			form.StartPosition = FormStartPosition.CenterScreen;

			ApDungControl(form.Controls, nutChinh);
		}

		// Tiêu đề lớn màu xanh và dòng mô tả xám, căn giữa
		public static void TieuDe(Label lblTieuDe, Label lblMoTa)
		{
			lblTieuDe.Font = FontTieuDe;
			lblTieuDe.ForeColor = MauChinh;
			lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;

			lblMoTa.Font = FontThuong;
			lblMoTa.ForeColor = MauChuPhu;
			lblMoTa.TextAlign = ContentAlignment.MiddleCenter;
		}

		// Nhãn cần nhấn mạnh (tổng tiền...): chữ đậm màu xanh
		public static void NhanManh(Label lbl)
		{
			lbl.Font = FontDam;
			lbl.ForeColor = MauChinh;
		}

		static void ApDungControl(Control.ControlCollection controls, Button[] nutChinh)
		{
			foreach (Control c in controls)
			{
				if (c is GroupBox)
				{
					// Tiêu đề khung đậm màu xanh; control bên trong được đặt lại font thường ở dưới
					c.BackColor = MauKhung;
					c.ForeColor = MauChinh;
					c.Font = FontDam;
				}
				else if (c is TabControl tab)
				{
					KieuTab(tab);
				}
				else if (c is TabPage trang)
				{
					trang.UseVisualStyleBackColor = false;
					trang.BackColor = MauNen;
				}
				else if (c is Button nut)
				{
					KieuNut(nut, Array.IndexOf(nutChinh, nut) >= 0);
				}
				else if (c is DataGridView dgv)
				{
					KieuBang(dgv);
				}
				else if (c is TextBoxBase txt)
				{
					txt.BorderStyle = BorderStyle.FixedSingle;
					txt.Font = FontThuong;
					txt.ForeColor = MauChu;
				}
				else if (c is DateTimePicker dtp)
				{
					dtp.Font = FontThuong;
					dtp.Format = DateTimePickerFormat.Custom;
					dtp.CustomFormat = "dd/MM/yyyy";
				}
				else if (c is Label || c is ComboBox || c is NumericUpDown || c is CheckBox || c is RadioButton)
				{
					c.Font = FontThuong;
					c.ForeColor = MauChu;
				}

				// Chỉ đi vào các khung chứa, không đi vào bên trong NumericUpDown, DateTimePicker...
				if (c is GroupBox || c is Panel || c is TabControl)
				{
					ApDungControl(c.Controls, nutChinh);
				}
			}
		}

		static void KieuNut(Button nut, bool laNutChinh)
		{
			nut.UseVisualStyleBackColor = false;
			nut.FlatStyle = FlatStyle.Flat;
			nut.FlatAppearance.BorderSize = 1;
			nut.FlatAppearance.BorderColor = laNutChinh ? MauChinh : MauVien;
			nut.FlatAppearance.MouseOverBackColor = laNutChinh ? MauChinhDam : MauRe;
			nut.BackColor = laNutChinh ? MauChinh : MauKhung;
			nut.ForeColor = laNutChinh ? Color.White : MauChinh;
			nut.Font = FontDam;
			nut.Cursor = Cursors.Hand;
		}

		static void KieuBang(DataGridView dgv)
		{
			dgv.BackgroundColor = MauKhung;
			dgv.BorderStyle = BorderStyle.FixedSingle;
			dgv.GridColor = MauLuoi;
			dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
			dgv.RowHeadersVisible = false;
			dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgv.RowTemplate.Height = 32;

			dgv.EnableHeadersVisualStyles = false;
			dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dgv.ColumnHeadersDefaultCellStyle.BackColor = MauTieuDeBang;
			dgv.ColumnHeadersDefaultCellStyle.ForeColor = MauChu;
			dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = MauTieuDeBang;
			dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = MauChu;
			dgv.ColumnHeadersDefaultCellStyle.Font = FontDam;
			dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 6, 4, 6);

			dgv.DefaultCellStyle.Font = FontThuong;
			dgv.DefaultCellStyle.ForeColor = MauChu;
			dgv.DefaultCellStyle.SelectionBackColor = MauDongChon;
			dgv.DefaultCellStyle.SelectionForeColor = MauChu;
			dgv.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
			dgv.AlternatingRowsDefaultCellStyle.BackColor = MauDongXen;

			// Tiền hiện dạng 1,200,000, ngày hiện dạng dd/MM/yyyy; chỉ đổi cách hiển thị, giá trị trong ô giữ nguyên
			dgv.CellFormatting += (sender, e) =>
			{
				if (e.Value is DateTime ngay)
				{
					e.Value = ngay.ToString("dd/MM/yyyy");
					e.FormattingApplied = true;
				}
				else if (e.Value is decimal tien)
				{
					e.Value = tien.ToString("N0");
					e.FormattingApplied = true;
				}
			};
		}

		// Tab vẽ lại: tab đang chọn nền xanh chữ trắng, tab còn lại chữ xám
		static void KieuTab(TabControl tab)
		{
			tab.DrawMode = TabDrawMode.OwnerDrawFixed;
			tab.SizeMode = TabSizeMode.Fixed;
			tab.ItemSize = new Size(170, 34);

			tab.DrawItem += (sender, e) =>
			{
				bool dangChon = e.Index == tab.SelectedIndex;
				Rectangle khung = tab.GetTabRect(e.Index);

				using (SolidBrush nen = new SolidBrush(dangChon ? MauChinh : MauNen))
				{
					e.Graphics.FillRectangle(nen, khung);
				}

				TextRenderer.DrawText(e.Graphics, tab.TabPages[e.Index].Text, FontDam, khung, dangChon ? Color.White : MauChuPhu, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
			};
		}
	}
}
