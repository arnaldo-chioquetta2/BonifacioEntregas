using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

// 3.9.9 Operador Caixa não vê lançamentos de outros

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

        public DataTable getDados(DateTime DT1, DateTime DT2, int idForma, string sObs, string sCliente, string sVendedor, string sValor, string sValorDebito, string sDesconto, int NrVend)
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
                if (!string.IsNullOrEmpty(sValorDebito))
                {
                    query.Append(", ca.VlDebito");
                }
                query.Append(@" FROM ((Caixa ca
                    LEFT JOIN Clientes c ON c.NrCli = ca.idCliente)
                    LEFT JOIN Vendedores v ON v.ID = ca.idVend)
                    LEFT JOIN Formas f ON f.ID = (ca.idForma + 1)");
                query.AppendFormat(" WHERE ca.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
                if (idForma > 0)
                {
                    query.AppendFormat(" AND ca.idForma = {0}", idForma - 1);
                }
                if (!string.IsNullOrEmpty(sObs))
                {
                    query.AppendFormat(" AND ca.Obs LIKE '%{0}%'", sObs);
                }
                if (!string.IsNullOrEmpty(sCliente))
                {
                    query.AppendFormat(" AND c.Nome LIKE '%{0}%'", sCliente);
                }
                if (!string.IsNullOrEmpty(sVendedor))
                {
                    query.AppendFormat(" AND v.Nome LIKE '%{0}%'", sVendedor);
                }
                if (!string.IsNullOrEmpty(sValor) && decimal.TryParse(sValor, out decimal valor))
                {
                    query.AppendFormat(" AND ca.Valor = {0}", valor.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                if (!string.IsNullOrEmpty(sValorDebito) && decimal.TryParse(sValorDebito, out decimal vlDebito))
                {
                    query.AppendFormat(" AND ca.VlDebito = {0}", vlDebito.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                if (!string.IsNullOrEmpty(sDesconto) && decimal.TryParse(sDesconto, out decimal desconto))
                {
                    query.AppendFormat(" AND ca.Desconto = {0}", desconto.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
                if (NrVend > 0)
                {
                    query.AppendFormat(" AND ca.idVend = {0}", NrVend);
                }
                query.Append(" ORDER BY ca.ID DESC");
                dt = DB.ExecutarConsulta(query.ToString());
                if (dt.Rows.Count == 0)
                {
                    StringBuilder q2 = new StringBuilder();
                    q2.Append("SELECT Data FROM Caixa ");
                    if (NrVend > 0)
                    {
                        q2.AppendFormat(" Where idVend = {0}", NrVend);
                    }
                    q2.AppendFormat(" ORDER BY Data DESC");
                    DataTable dt2 = DB.ExecutarConsulta(q2.ToString());

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

    public void MudaData(DateTime data, string listaIds)
    {
        // 1. Validar e converter a lista de strings para inteiros
        var ids = new List<int>();
        if (string.IsNullOrWhiteSpace(listaIds))
        {
            // Não há IDs para atualizar, talvez logar um aviso ou lançar uma exceção
            glo.Loga("Aviso em MudaData: Lista de IDs está vazia.");
            return; // Ou lançar exceção: throw new ArgumentException("Lista de IDs não pode estar vazia.");
        }

        string[] partes = listaIds.Split(',');
        foreach (var parte in partes)
        {
            if (int.TryParse(parte.Trim(), out int id))
            {
                ids.Add(id);
            }
            else
            {
                // Logar erro ou lançar exceção se um ID for inválido
                glo.Loga($"Erro em MudaData: ID inválido na lista: '{parte}'");
                // throw new ArgumentException($"ID inválido na lista: '{parte}'");
                // Ou simplesmente ignore partes inválidas e continue
            }
        }

        if (ids.Count == 0)
        {
            glo.Loga("Aviso em MudaData: Nenhum ID válido encontrado na lista.");
            return; // Ou lançar exceção
        }

        // 2. Construir a query com placeholders para parâmetros
        // Um placeholder (?) para a data e um (?) para cada ID
        var parametros = new List<OleDbParameter>();
        var inClausePlaceholders = new List<string>();

        // Adiciona o parâmetro para a data
        // Use o formato ODBC canônico {d 'yyyy-mm-dd'} para datas com OleDb, ou um parâmetro DateTime
        // Vamos usar um parâmetro DateTime para maior clareza e evitar problemas com #data#
        parametros.Add(new OleDbParameter("@Data", OleDbType.Date) { Value = data });
        // parametros.Add(new OleDbParameter("@Data", OleDbType.VarChar) { Value = data.ToString("yyyy-MM-dd") }); // Alternativa

        // Adiciona um parâmetro para cada ID
        for (int i = 0; i < ids.Count; i++)
        {
            string paramName = $"@Id{i}";
            inClausePlaceholders.Add($"?"); // Ou use o nome: $"@{paramName}"
                                            // Note: OleDb usa ? como placeholder. Nomear o parâmetro aqui pode não ser necessário,
                                            // mas é bom para clareza. O OleDb associa os parâmetros pela ordem.
            parametros.Add(new OleDbParameter(/*paramName*/ $"@Id{i}", OleDbType.Integer) { Value = ids[i] });
            // Se usar ? no placeholder, o nome do OleDbParameter é irrelevante, mas o tipo e valor são importantes.
            // parametros.Add(new OleDbParameter($"?{i}", OleDbType.Integer) { Value = ids[i] }); // Outra forma
        }

        // Monta a query final
        // Usando o formato ODBC canônico para data: {d 'yyyy-mm-dd'}
        // Mas como estamos usando um parâmetro DateTime, podemos usar ? diretamente
        string sql = $"UPDATE Caixa SET Data = ? WHERE ID IN ({string.Join(", ", inClausePlaceholders)})";
        // string sql = $"UPDATE Caixa SET Data = @{parametros[0].ParameterName} WHERE ID IN ({string.Join(", ", inClausePlaceholders)})"; // Se usar nomes

        // 3. Executar a query com os parâmetros
        try
        {
            DB.ExecutarComandoSQL(sql, parametros);
        }
        catch (Exception ex) // Captura específica pode ser OleDbException
        {
            // Loga o erro com mais detalhes
            glo.Loga($"Erro em MudaData ao executar SQL: {ex.Message} | Query: {sql} | IDs: {listaIds}");
            // Relança a exceção para que o código chamador (btEditar_Click) possa tratá-la
            throw;
        }
    }

    //public void MudaData(DateTime data, string lista)
    //{           
    //    string sData = data.ToString("MM/dd/yyyy");
    //    String sql = $@"Update Caixa Set Data = #{sData}# WHERE ID in ({lista}) ";
    //    DB.ExecutarComandoSQL(sql);
    //}

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
