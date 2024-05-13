using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.OleDb;
using System.Text;

namespace TeleBonifacio.dao
{
    public class ReciboDAO
    {

        public DataTable ValoresAPagar(DateTime? DT1, DateTime? DT2)
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"Select Entregas.idVend, Vendedores.Nome, Sum(Entregas.VlNota) / 100 as Valor  
                        From Entregas
                        Inner Join Vendedores on Vendedores.ID = Entregas.idVend
                        Where Entregas.Pago is Null
                        and Entregas.idVend > 0 ");
            DateTime dataInicio = DT1.Value.Date;
            DateTime dataFim = DT2.Value.Date;
            string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");
            query.AppendFormat(" and Entregas.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            query.Append(" Group by Entregas.idVend, Vendedores.Nome ");
            query.Append("Order by Entregas.idVend, Vendedores.Nome");
            DataTable dataTable = glo.getDados(query.ToString());
            return dataTable;
        }

        public decimal VlrPend(int id, DateTime DT1, DateTime DT2)
        {
            DateTime dataInicio = DT1.Date;
            DateTime dataFim = DT2.Date;
            string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");
            StringBuilder query = new StringBuilder();

            query.Append($@"Select Sum(Entregas.VlNota) / 100 as Valor  
                From Entregas 
                Where Entregas.Pago is Null 
                    and Entregas.idVend = " + id.ToString());
            query.AppendFormat(" and Entregas.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            DataTable dataTable = glo.getDados(query.ToString());
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

        //public DateTime DtInicial(int id)
        //{
        //    string SQLPriComiss = "SELECT Data FROM Entregas WHERE Pago IS NULL AND idVend = " + id.ToString() + " ORDER BY Data ASC";
        //    DataTable dtData = glo.getDados(SQLPriComiss);
        //    DateTime Dia = Convert.ToDateTime(dtData.Rows[0]["Data"]);
        //    return Dia;
        //}

        public void Pagar(int id, string Valor, string dataPagamento, DateTime DT1, DateTime DT2)
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

            DateTime dataInicio = DT1.Date;
            DateTime dataFim = DT2.Date;
            string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");

            StringBuilder queryUpd = new StringBuilder();
            queryUpd.Append($@"UPDATE Entregas SET Pago = VlNota / 100, idPagto = " + idRecibo);
            queryUpd.Append(" WHERE Pago IS NULL AND idVend = " + id.ToString());
            queryUpd.AppendFormat(" and Entregas.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            glo.ExecutarComandoSQL(queryUpd.ToString(), null);

        }

        private int VeUltReg()
        {
            string SQL = "SELECT Max(ID) as ID FROM Vales";
            DataTable dtData = glo.getDados(SQL);
            return Convert.ToInt16(dtData.Rows[0]["ID"]);
        }

    }
}
