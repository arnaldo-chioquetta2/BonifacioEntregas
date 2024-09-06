using System;
using System.Data;

namespace TeleBonifacio.dao
{
    public class ConfigDAO
    {
        private DataTable GetConfig()
        {
            string query = "Select * From Config";
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            return dt;
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

    }
}
