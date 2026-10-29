using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace btl.Classes
{
	public class DatabaseProcessor
	{
		string strConnect = "Data Source=localhost; Database=QuanLySuaXe; User ID=sa; Password=1234$; Integrated Security=false;";
		SqlConnection sqlConnect = null;
		void OpenConnection()
		{
			sqlConnect = new SqlConnection(strConnect);
			if(sqlConnect.State != ConnectionState.Open)
			{
				sqlConnect.Open();
			}
		}
		void CloseConnection()
		{
			if(sqlConnect.State != ConnectionState.Closed)
			{
				sqlConnect.Close();
				sqlConnect.Dispose();
			}
		}
		public DataTable ReadData(string sql)
		{
			OpenConnection();
			DataTable data = new DataTable();
			SqlDataAdapter sqlData = new SqlDataAdapter(sql, sqlConnect);
			sqlData.Fill(data);
			CloseConnection();
			return data;
		}
		public void ChangeData(string sql)
		{
			OpenConnection();
			SqlCommand sqlComma = new SqlCommand();
			sqlComma.Connection = sqlConnect;
			sqlComma.CommandText = sql;
			sqlComma.ExecuteNonQuery();
			CloseConnection();
		}
		// Chạy nhiều câu lệnh trong 1 transaction: lỗi ở bất kỳ câu nào thì hoàn tác tất cả, lỗi được ném ra ngoài
		public void ChangeDataTransaction(string sql)
		{
			ChangeData("SET XACT_ABORT ON;\n" +
				"BEGIN TRY\n" +
				"BEGIN TRANSACTION;\n" +
				sql + "\n" +
				"COMMIT TRANSACTION;\n" +
				"END TRY\n" +
				"BEGIN CATCH\n" +
				"IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;\n" +
				"THROW;\n" +
				"END CATCH;");
		}
		public bool KiemTraTonTai(string tenBang, string tenCot, string giaTri)
		{
			string sql = "Select 1 From " + tenBang + " Where " + tenCot + " = N'" + giaTri.Replace("'", "''") + "'";
			return ReadData(sql).Rows.Count > 0;
		}
		// Số lớn nhất phía sau tiền tố của cột mã, VD: PN025 => 25; chưa có mã nào thì trả về 0
		public int LaySoLonNhat(string tenBang, string tenCot, string tienTo)
		{
			string sql = "Select Max(Try_Cast(Substring(" + tenCot + ", " + (tienTo.Length + 1) + ", Len(" + tenCot + ")) As int)) " +
				"From " + tenBang + " " +
				"Where " + tenCot + " Like '" + tienTo + "%'";
			DataTable data = ReadData(sql);
			if (data.Rows.Count > 0 && data.Rows[0][0] != DBNull.Value)
			{
				return Convert.ToInt32(data.Rows[0][0]);
			}
			return 0;
		}
		// Mã tiếp theo, VD: PN025 => PN026, bảng chưa có dòng nào => PN001
		public string GenerateMa(string tenBang, string tenCot, string tienTo)
		{
			return tienTo + (LaySoLonNhat(tenBang, tenCot, tienTo) + 1).ToString("D3");
		}
	}
}
