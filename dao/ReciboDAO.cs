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
            query.Append(@"SELECT Entregas.idVend, Vendedores.Nome, SUM(Entregas.VlNota) as TotalVendas, 0 as Valor 
                   FROM Entregas
                   INNER JOIN Vendedores ON Vendedores.ID = Entregas.idVend
                   WHERE Entregas.Pago IS NULL AND Entregas.idVend > 0 ");

            DateTime dataInicio = DT1.Value.Date;
            DateTime dataFim = DT2.Value.Date;
            string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");
            query.AppendFormat(" AND Entregas.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            query.Append(" GROUP BY Entregas.idVend, Vendedores.Nome ");
            query.Append("ORDER BY Entregas.idVend, Vendedores.Nome");

            DataTable dataTable = glo.getDados(query.ToString());

            // Calcular o valor da comissão usando o percentual variável
            foreach (DataRow row in dataTable.Rows)
            {
                decimal totalVendas = Convert.ToDecimal(row["TotalVendas"]);
                decimal percentual = glo.ObterPercentualVariavel(totalVendas);

                decimal valorComissao = Math.Round(totalVendas * (percentual / 100m), 2);
                row["Valor"] = valorComissao;

            }

            return dataTable;
        }


        public decimal VlrPend(int id, DateTime DT1, DateTime DT2)
        {
            DateTime dataInicio = DT1.Date;
            DateTime dataFim = DT2.Date;
            string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");

            StringBuilder query = new StringBuilder();
            query.Append($@"SELECT SUM(Entregas.VlNota) as TotalVendas  
                            FROM Entregas 
                            WHERE Entregas.Pago IS NULL 
                                AND Entregas.idVend = " + id.ToString());
            query.AppendFormat(" AND Entregas.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);

            DataTable dataTable = glo.getDados(query.ToString());
            decimal ret = 0;

            try
            {
                if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["TotalVendas"] != DBNull.Value)
                {
                    decimal totalVendas = Convert.ToDecimal(dataTable.Rows[0]["TotalVendas"]);
                    decimal percentual = glo.ObterPercentualVariavel(totalVendas);
                    ret = totalVendas * (percentual / 100m);
                }
            }
            catch (Exception ex)
            {
                // Considere logar a exceção aqui
                // Log.Error("Erro ao calcular VlrPend", ex);
            }

            return ret;
        }

        public void Pagar(int id, string Valor, string dataPagamento, DateTime DT1, DateTime DT2)
        {
            // Inserção na tabela Vales permanece a mesma
            string query = "INSERT INTO Vales (IdOperador, Data, Valor, Pago, Tipo, Periodo) " +
                           "VALUES ('" +
                           id.ToString() + "', '" +
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
                           Valor + "', '" +
                           DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 1, '" +
                           dataPagamento + "')";
            DB.ExecutarComandoSQL(query);

            int idRecibo = VeUltReg();

            DateTime dataInicio = DT1.Date;
            DateTime dataFim = DT2.Date;
            string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");

            // Obter o total de vendas para calcular o percentual
            string queryTotal = $@"SELECT SUM(VlNota) FROM Entregas 
                           WHERE Pago IS NULL AND idVend = {id} 
                           AND Data BETWEEN #{dataInicioStr}# AND #{dataFimStr}#";

            // Executa a consulta e obtém o resultado
            DataTable result = DB.ExecutarConsulta(queryTotal);

            // Verifica se a consulta retornou algum resultado e obtém o valor
            decimal totalVendas = 0;
            if (result.Rows.Count > 0 && result.Rows[0][0] != DBNull.Value)
            {
                totalVendas = Convert.ToDecimal(result.Rows[0][0]);
            }

            // Calcular o percentual
            decimal percentual = glo.ObterPercentualVariavel(totalVendas)/100;
            string sPerc = glo.sv(percentual);
            // Atualizar as entregas com o novo cálculo de comissão
            StringBuilder queryUpd = new StringBuilder();
            queryUpd.Append($@"UPDATE Entregas SET Pago = VlNota * {sPerc}, idPagto = {idRecibo}");
            queryUpd.Append($" WHERE Pago IS NULL AND idVend = {id}");
            queryUpd.AppendFormat(" AND Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            DB.ExecutarComandoSQL(queryUpd.ToString(), null);
        }

        //public void Pagar(int id, string Valor, string dataPagamento, DateTime DT1, DateTime DT2)
        //{
        //    // , decimal VlrComiss
        //    // Inserção na tabela Vales permanece a mesma
        //    string query = "INSERT INTO Vales (IdOperador, Data, Valor, Pago, Tipo, Periodo) " +
        //                   "VALUES ('" +
        //                   id.ToString() + "', '" +
        //                   DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
        //                   Valor + "', '" +
        //                   DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 1, '" +
        //                   dataPagamento + "')";
        //    DB.ExecutarComandoSQL(query);

        //    int idRecibo = VeUltReg();

        //    DateTime dataInicio = DT1.Date;
        //    DateTime dataFim = DT2.Date;
        //    string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
        //    string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");

        //    // Obter o total de vendas para calcular o percentual
        //    string queryTotal = $@"SELECT SUM(VlNota) FROM Entregas 
        //                   WHERE Pago IS NULL AND idVend = {id} 
        //                   AND Data BETWEEN #{dataInicioStr}# AND #{dataFimStr}#";
        //    decimal totalVendas = Convert.ToDecimal(DB.ExecutarConsulta(queryTotal));

        //    // Calcular o percentual
        //    decimal percentual = glo.ObterPercentualVariavel(totalVendas);

        //    // Atualizar as entregas com o novo cálculo de comissão
        //    StringBuilder queryUpd = new StringBuilder();
        //    queryUpd.Append($@"UPDATE Entregas SET Pago = VlNota * {percentual / 100m}, idPagto = {idRecibo}");
        //    queryUpd.Append($" WHERE Pago IS NULL AND idVend = {id}");
        //    queryUpd.AppendFormat(" AND Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
        //    DB.ExecutarComandoSQL(queryUpd.ToString(), null);
        //}

        private int VeUltReg()
        {
            string SQL = "SELECT Max(ID) as ID FROM Vales";
            DataTable dtData = glo.getDados(SQL);
            return Convert.ToInt16(dtData.Rows[0]["ID"]);
        }

    }
}
