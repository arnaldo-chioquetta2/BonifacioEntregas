using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
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
            decimal ret = 0;
            try
            {
                double doubleValue = Convert.ToDouble(dataTable.Rows[0]["Valor"]);
                ret = Convert.ToDecimal(doubleValue);
            }
            catch (Exception)
            {
                // throw;
            }
            return ret;
        }

        public DateTime DtInicial(int id)
        {
            string SQLPriComiss = "SELECT Data FROM Entregas WHERE Pago IS NULL AND idVend = " + id.ToString() + " ORDER BY Data ASC";
            DataTable dtData = glo.getDados(SQLPriComiss);
            DateTime Dia = Convert.ToDateTime(dtData.Rows[0]["Data"]);
            return Dia;
        }

        public void Pagar(int id, string Valor, string dataPagamento)
        {

            string query = "INSERT INTO Vales (IdOperador, Data, Valor, Pago, Tipo, Periodo) " +
                            "VALUES ('" +
                            id.ToString() + "', '" +
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
                            Valor + "', '" +
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 1, '" +
                            dataPagamento + "')";
            glo.ExecutarComandoSQL(query);

            int idRecibo = VeUltReg();

            string queryUpd = "UPDATE Entregas SET Pago = VlNota / 100, idPagto = " + idRecibo + " WHERE Pago IS NULL AND idVend = " + id.ToString();
            glo.ExecutarComandoSQL(queryUpd, null);

        }

        private int VeUltReg()
        {
            string SQL = "SELECT Max(ID) as ID FROM Vales";
            DataTable dtData = glo.getDados(SQL);
            return Convert.ToInt16(dtData.Rows[0]["ID"]);
        }

    }
}
