using System;
using System.Data;
using System.Text;
using System.Data.OleDb;
using System.Collections.Generic;

namespace TeleBonifacio.dao
{
    public class RelCaixaDAO
    {
        private readonly string connectionString = glo.connectionString;

        /// <summary>
        /// Retorna a soma dos valores por forma de pagamento no período informado.
        /// </summary>
        public List<(string FormaPagamento, decimal Total)> ObterTotaisPorForma(
            DateTime dataInicio,
            DateTime dataFim,
            string formaFiltro = null)
        {
            List<(string FormaPagamento, decimal Total)> totais = new List<(string, decimal)>();

            string filtroForma = "";
            if (!string.IsNullOrWhiteSpace(formaFiltro))
                filtroForma = $" AND FormaPagamento = '{formaFiltro.Replace("'", "''")}'";

            string sql = $@"
                SELECT FormaPagamento, SUM(Valor) AS Total
                FROM Lancamentos
                WHERE Data BETWEEN #{dataInicio:MM/dd/yyyy}# AND #{dataFim:MM/dd/yyyy}#
                {filtroForma}
                GROUP BY FormaPagamento
                ORDER BY FormaPagamento";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string forma = dr["FormaPagamento"]?.ToString() ?? "Indefinida";
                            decimal total = dr["Total"] != DBNull.Value ? Convert.ToDecimal(dr["Total"]) : 0m;
                            totais.Add((forma, total));
                        }
                    }
                }
                catch (Exception ex)
                {
                    glo.Loga($"Erro em RelCaixaDAO.ObterTotaisPorForma: {ex.Message}");
                }
            }

            return totais;
        }

        /// <summary>
        /// Retorna todos os lançamentos do período.
        /// </summary>
        public DataTable GetLancamentos(DateTime dataInicio, DateTime dataFim, string formaFiltro = null)
        {
            string filtroForma = "";
            if (!string.IsNullOrWhiteSpace(formaFiltro))
                filtroForma = $" AND FormaPagamento = '{formaFiltro.Replace("'", "''")}'";

            string sql = $@"
                SELECT Id, Data, Descricao, Valor, FormaPagamento, Tipo, Usuario
                FROM Lancamentos
                WHERE Data BETWEEN #{dataInicio:MM/dd/yyyy}# AND #{dataFim:MM/dd/yyyy}#
                {filtroForma}
                ORDER BY Data";

            DataTable tabela = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OleDbDataAdapter da = new OleDbDataAdapter(sql, conn))
                    {
                        da.Fill(tabela);
                    }
                }
                catch (Exception ex)
                {
                    glo.Loga($"Erro em RelCaixaDAO.GetLancamentos: {ex.Message}");
                }
            }

            return tabela;
        }

        // =========================
        // NOVO: GetLancamento(id)
        // =========================
        /// <summary>
        /// Retorna um único lançamento pelo Id (ou null se não encontrado).
        /// </summary>
        public tb.Lanctos GetLancamento(int id)
        {
            const string sql = @"
                SELECT TOP 1 Id, Data, Descricao, Valor, FormaPagamento, Tipo, Usuario
                FROM Lancamentos
                WHERE Id = ?";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = id;

                        using (OleDbDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                // Mapeia diretamente do DataReader
                                return MapearLancamento(dr);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    glo.Loga($"Erro em RelCaixaDAO.GetLancamento: {ex.Message}");
                }
            }

            return null;
        }

        /// <summary>
        /// Helper para mapear um registro (DataReader) para tb.Lancamento.
        /// Observação: ajusta Entrada/Saída com base no campo Tipo (E/S).
        /// </summary>
        private TeleBonifacio.tb.Lanctos MapearLancamento(OleDbDataReader dr)
        {
            // Campos esperados na tabela:
            // Id (int), Data (DateTime), Descricao (string),
            // Valor (decimal), FormaPagamento (string), Tipo (string 'E'/'S'), Usuario (string)

            var lanc = new TeleBonifacio.tb.Lanctos
            {
                ID = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                DataPagamento = dr["Data"] != DBNull.Value ? Convert.ToDateTime(dr["Data"]) : DateTime.MinValue,
                Obs = dr["Descricao"]?.ToString() ?? "",
                Forma = dr["FormaPagamento"]?.ToString() ?? "",

                // Compatível com código que lê FormaPagamento também:
                // (a classe tb.Lancamento que você criou tem a propriedade "FormaPagamento" espelhando "Forma")
                // Ex.: public string FormaPagamento { get => Forma; set => Forma = value; }

                Vendedor = dr["Usuario"]?.ToString() ?? ""
            };

            decimal valor = dr["Valor"] != DBNull.Value ? Convert.ToDecimal(dr["Valor"]) : 0m;
            string tipo = dr["Tipo"]?.ToString()?.Trim().ToUpperInvariant() ?? "E";

            if (tipo == "S")
            {
                lanc.Saida = valor;
                lanc.Entrada = 0m;
            }
            else
            {
                lanc.Entrada = valor;
                lanc.Saida = 0m;
            }

            // Desconto pode não existir nessa tabela; se existir, ajuste aqui:
            // lanc.Desconto = dr["Desconto"] != DBNull.Value ? Convert.ToDecimal(dr["Desconto"]) : 0m;

            // Valor total (se sua classe usa)
            lanc.Valor = valor;

            return lanc;
        }

        /// <summary>
        /// Retorna todas as formas de pagamento distintas da tabela de lançamentos.
        /// </summary>
        public List<string> GetFormasPagamento()
        {
            List<string> formas = new List<string>();

            string sql = "SELECT DISTINCT FormaPagamento FROM Lancamentos WHERE FormaPagamento IS NOT NULL AND TRIM(FormaPagamento) <> '' ORDER BY FormaPagamento";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string forma = dr["FormaPagamento"]?.ToString()?.Trim();
                            if (!string.IsNullOrEmpty(forma))
                                formas.Add(forma);
                        }
                    }
                }
                catch (Exception ex)
                {
                    glo.Loga($"Erro em RelCaixaDAO.GetFormasPagamento: {ex.Message}");
                }
            }

            // Garante que sempre existam opções padrão
            if (formas.Count == 0)
            {
                formas.AddRange(new[]
                {
                    "Dinheiro",
                    "Cartão",
                    "Pix",
                    "Boleto",
                    "Anotado",
                    "Troca",
                    "Vale"
                });
            }

            return formas;
        }

        public List<TeleBonifacio.tb.Lanctos> FiltrarLancamentos(DateTime inicio, DateTime fim, string tipo)
        {
            var lista = new List<TeleBonifacio.tb.Lanctos>();

            try
            {
                using (var con = new OleDbConnection(glo.connectionString))
                {
                    con.Open();

                    // Monta o filtro dinâmico
                    var sql = new StringBuilder();
                    sql.Append("SELECT Id, Data, Descricao, Valor, FormaPagamento, Tipo, Usuario ");
                    sql.Append("FROM Lancamentos ");
                    sql.Append("WHERE Data BETWEEN @Inicio AND @Fim ");

                    if (!string.IsNullOrEmpty(tipo) && tipo != "T")
                        sql.Append("AND Tipo = @Tipo ");

                    sql.Append("ORDER BY Data DESC");

                    using (var cmd = new OleDbCommand(sql.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue("@Inicio", inicio);
                        cmd.Parameters.AddWithValue("@Fim", fim);

                        if (!string.IsNullOrEmpty(tipo) && tipo != "T")
                            cmd.Parameters.AddWithValue("@Tipo", tipo);

                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(MapearLancamento(dr));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                glo.Loga($"Erro em RelCaixaDAO.FiltrarLancamentos: {ex.Message}");
            }

            return lista;
        }


    }
}
