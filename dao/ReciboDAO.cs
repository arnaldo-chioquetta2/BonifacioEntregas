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
            DataTable dataTable = gen.getDados(query);
            return dataTable;
        }

        public decimal VlrPend(int id)
        {
            string query = @"Select Sum(Entregas.VlNota) / 100 as Valor  
                From Entregas 
                Where Entregas.Pago is Null 
                and Entregas.idVend = " + id.ToString();
            DataTable dataTable = gen.getDados(query);
            double doubleValue = Convert.ToDouble(dataTable.Rows[0]["Valor"]);
            decimal ret = Convert.ToDecimal(doubleValue);
            return ret;
        }

        internal void Pagar(int id)
        {
            string query = "UPDATE Entregas SET Pago = VlNota/ 100 WHERE Pago is Null and idVend = " + id.ToString();
            gen.ExecutarComandoSQL(query, null);
        }
    }
}
