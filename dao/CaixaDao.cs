using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace TeleBonifacio.dao
{
    public class CaixaDao
    {
        public DateTime DT1 { get; private set; }

        public int Adiciona(int idForma, float compra, int idCliente, string obs, float desc, int idVend, string UID)
        {
            string sql = @"INSERT INTO Caixa (idCliente, idForma, Valor, VlNota, Obs, Desconto, idVend, UID, Data) VALUES ("
                        + idCliente.ToString() + ", "
                        + idForma.ToString() + ", "
                        + glo.sv(compra) + ", "
                        + glo.sv(compra - desc) + ", "
                        + glo.fa(obs) + ", "
                        + glo.sv(desc) + ", "
                        + idVend.ToString() + ", "
                        + glo.fa(UID)
                        + ", Now)";
            DB.ExecutarComandoSQL(sql);
            string queryUltimoId = "SELECT @@IDENTITY";
            DataTable dt = DB.ExecutarConsulta(queryUltimoId);
            return Convert.ToInt32(dt.Rows[0][0]); // Retorna o ID gerado
        }

        public DataTable getDados(DateTime DT1, DateTime DT2, int idForma, string sObs, string sCliente, string sVendedor, string sValor, string sValorDebito, string sDesconto)
        {
            bool Sair = false;
            DataTable dt = null;
            int qtD = 0;

            while (!Sair)
            {
                DateTime dataInicio = DT1.Date;
                DateTime dataFim = DT2.Date;
                string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
                string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");

                StringBuilder query = new StringBuilder();
                query.Append(@"SELECT ca.ID, c.Nome AS Cliente, ca.Valor, ca.Desconto, ca.VlNota, 
                    v.Nome AS Vendedor, ca.Data, f.Nome AS Pagamento, ca.Obs,
                    c.NrCli, ca.idVend, ca.idForma, ca.UID");

                // 🔹 Somente adiciona `ca.VlDebito` se a pesquisa foi feita pelo valor de débito
                if (!string.IsNullOrEmpty(sValorDebito))
                {
                    query.Append(", ca.VlDebito");
                }

                query.Append(@" FROM ((Caixa ca
                    LEFT JOIN Clientes c ON c.NrCli = ca.idCliente)
                    LEFT JOIN Vendedores v ON v.ID = ca.idVend)
                    LEFT JOIN Formas f ON f.ID = (ca.idForma + 1)");

                query.AppendFormat(" WHERE ca.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);

                bool hasCondition = false;

                if (idForma > 0)
                {
                    query.AppendFormat(" AND ca.idForma = {0}", idForma - 1);
                    hasCondition = true;
                }
                if (!string.IsNullOrEmpty(sObs))
                {
                    query.AppendFormat(" AND ca.Obs LIKE '%{0}%'", sObs);
                    hasCondition = true;
                }
                if (!string.IsNullOrEmpty(sCliente))
                {
                    query.AppendFormat(" AND c.Nome LIKE '%{0}%'", sCliente);
                    hasCondition = true;
                }
                if (!string.IsNullOrEmpty(sVendedor))
                {
                    query.AppendFormat(" AND v.Nome LIKE '%{0}%'", sVendedor);
                    hasCondition = true;
                }
                if (!string.IsNullOrEmpty(sValor) && decimal.TryParse(sValor, out decimal valor))
                {
                    query.AppendFormat(" AND ca.Valor = {0}", valor.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    hasCondition = true;
                }
                if (!string.IsNullOrEmpty(sValorDebito) && decimal.TryParse(sValorDebito, out decimal vlDebito))
                {
                    query.AppendFormat(" AND ca.VlDebito = {0}", vlDebito.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    hasCondition = true;
                }
                if (!string.IsNullOrEmpty(sDesconto) && decimal.TryParse(sDesconto, out decimal desconto))
                {
                    query.AppendFormat(" AND ca.Desconto = {0}", desconto.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    hasCondition = true;
                }

                query.Append(" ORDER BY ca.ID DESC");
                dt = DB.ExecutarConsulta(query.ToString());

                if (dt.Rows.Count == 0)
                {
                    string q2 = "SELECT Data FROM Caixa ORDER BY Data DESC";
                    DataTable dt2 = DB.ExecutarConsulta(q2);

                    if (dt2.Rows.Count == 0)
                    {
                        Sair = true;
                    }
                    else
                    {
                        DT1 = (DateTime)dt2.Rows[0]["Data"];
                        if (qtD < 10)
                        {
                            qtD++;
                            DT2 = DT2.AddDays(1);
                        }
                        else
                        {
                            Sair = true;
                        }
                    }
                }
                else
                {
                    Sair = true;
                }
            }
            this.DT1 = DT1;
            return dt;
        }

        public void Edita(int iID, int idForma, float compra, int idCliente, string obs, float desc, int idVend)
        {
            String sql = @"UPDATE Caixa SET 
                              idCliente = " + idCliente.ToString() +
                            ",idForma = " + idForma.ToString() +
                            ",Valor = " + glo.sv(compra) +
                            ",VlNota = " + glo.sv(compra) +
                            ",Obs = " + glo.fa(obs) +
                            ",Desconto = " + glo.sv(desc) +
                            ",idVend = " + idVend.ToString() +
                            " WHERE ID = " + iID.ToString();
            DB.ExecutarComandoSQL(sql);
        }

        public void Exclui(int iID)
        {
            String sql = @"Delete From Caixa WHERE ID = " + iID.ToString();
            DB.ExecutarComandoSQL(sql);
        }

        public void MudaData(DateTime data, string lista)
        {           
            string sData = data.ToString("MM/dd/yyyy");
            String sql = $@"Update Caixa Set Data = #{sData}# WHERE ID in ({lista}) ";
            DB.ExecutarComandoSQL(sql);
        }

        public void EditaFormaPagamento(int registroId, int novaFormaId)
        {
            string query = $@"UPDATE Caixa SET idForma = {novaFormaId} WHERE ID = {registroId} ";
            DB.ExecutarComandoSQL(query);
        }

        public void AtualizaForma(int iID, int idForma)
        {
            String sql = @"UPDATE Caixa SET 
                      idForma = " + idForma.ToString() +
                           " WHERE ID = " + iID.ToString();
            DB.ExecutarComandoSQL(sql);
        }
    }
}
