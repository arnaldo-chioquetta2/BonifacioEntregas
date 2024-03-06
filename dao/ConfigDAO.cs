using System;
using System.Data;
using System.Data.OleDb;

namespace TeleBonifacio.dao
{
    public class ConfigDAO
    {
        private DataTable GetConfig()
        {
            string query = "Select * From Config";
            DataTable dt = ExecutarConsulta(query.ToString());
            return dt;
        }

        private DataTable ExecutarConsulta(string query)
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dataTable;
        }

        public decimal getPercentual()
        {
            DataTable tb = GetConfig();
            if (tb.Rows.Count > 0 && tb.Columns.Contains("UtComissoes"))
            {
                DataRow row = tb.Rows[0];
                if (row["UtComissoes"] != DBNull.Value)
                {
                    return Convert.ToDecimal(row["UtComissoes"]);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public void SetPerc(float perc)
        {
            string sql = "UPDATE Config SET UtComissoes = @perc";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@perc", perc);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
