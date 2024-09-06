using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeleBonifacio.dao
{
    public class DynamicGridDao
    {

        private DataGridView griTaxas;

        public DynamicGridDao(DataGridView griTaxas)
        {
            this.griTaxas = griTaxas;
        }

        public int LoadDataFromDatabase()
        {
            string query = "SELECT RowIndex, ColumnIndex, CellValue FROM DynamicGrid ORDER BY RowIndex, ColumnIndex";
            DataTable dataTable = DB.ExecutarConsulta(query);
            int c = 0;

            // Limpar os dados existentes na grid
            griTaxas.Rows.Clear();

            foreach (DataRow row in dataTable.Rows)
            {
                int rowIndex = Convert.ToInt32(row["RowIndex"]);
                int columnIndex = Convert.ToInt32(row["ColumnIndex"]);
                string cellValue = row["CellValue"].ToString();

                while (griTaxas.ColumnCount <= columnIndex)
                {
                    griTaxas.Columns.Add($"Column{griTaxas.ColumnCount}", $"Column {griTaxas.ColumnCount}");
                }

                // Garantir que existem linhas suficientes
                while (griTaxas.RowCount <= rowIndex)
                {
                    griTaxas.Rows.Add();
                }

                // Definir o valor da célula
                griTaxas.Rows[rowIndex].Cells[columnIndex].Value = cellValue;
                c++;
            }

            return c;
        }

    }
}
