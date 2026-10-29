using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ClosedXML.Excel;

namespace btl.Classes
{
	// Xuất DataTable / DataGridView ra file Excel (.xlsx) bằng ClosedXML
	public static class XuatExcel
	{
		// Hộp chọn chỗ lưu, tên gợi ý có ngày giờ để mỗi lần xuất ra 1 file riêng; bấm Hủy thì trả về ""
		public static string ChonFileLuu(string tenFile)
		{
			using (SaveFileDialog dialog = new SaveFileDialog())
			{
				dialog.Filter = "Excel (*.xlsx)|*.xlsx";
				dialog.FileName = tenFile + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".xlsx";

				return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "";
			}
		}

		// Ghi 1 bảng ra 1 sheet: tiền định dạng #,##0, ngày định dạng dd/MM/yyyy
		public static IXLWorksheet ThemSheet(XLWorkbook wb, string tenSheet, DataTable data, string[] cot, string[] tieuDe)
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
		public static IXLWorksheet ThemSheet(XLWorkbook wb, string tenSheet, DataGridView dgv)
		{
			List<DataGridViewColumn> cot = dgv.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();

			return ThemSheet(wb, tenSheet, (DataTable)dgv.DataSource, cot.Select(c => c.DataPropertyName).ToArray(), cot.Select(c => c.HeaderText).ToArray());
		}

		public static void Luu(XLWorkbook wb, string duongDan)
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
	}
}
