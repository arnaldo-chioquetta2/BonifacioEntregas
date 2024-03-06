using System;
using System.Data;
using System.Data.OleDb;

namespace TeleBonifacio.dao
{
    public class ReciboDAO
    {

        public DataTable ValoresAPagar()
        {
            string query = @"Select Entregas.idVend, Vendedores.Nome, Sum(Entregas.VlNota) / 100 as Valor  
                        From Entregas
                        Inner Join Vendedores on Vendedores.ID = Entregas.idVend
                        Where Entregas.Pago is Null
                        and Entregas.idVend > 0
                        Group by Entregas.idVend, Vendedores.Nome
                        Order by Entregas.idVend, Vendedores.Nome ";
            DataTable dataTable = glo.getDados(query);
            return dataTable;
        }

        public decimal VlrPend(int id)
        {
            string query = @"Select Sum(Entregas.VlNota) / 100 as Valor  
                From Entregas 
                Where Entregas.Pago is Null 
                and Entregas.idVend = " + id.ToString();
            DataTable dataTable = glo.getDados(query);
            double doubleValue = Convert.ToDouble(dataTable.Rows[0]["Valor"]);
            decimal ret = Convert.ToDecimal(doubleValue);
            return ret;
        }

        public DateTime Pagar(int id)
        {
            string SQLPriComiss = "SELECT Data FROM Entregas WHERE Pago IS NULL AND idVend = " + id.ToString() + " ORDER BY Data ASC";
            DataTable dataTable = glo.getDados(SQLPriComiss);
            DateTime Dia = Convert.ToDateTime(dataTable.Rows[0]["Data"]);
            string query = "UPDATE Entregas SET Pago = VlNota / 100 WHERE Pago IS NULL AND idVend = " + id.ToString();
            glo.ExecutarComandoSQL(query, null);
            return Dia;
        }

    }
}
