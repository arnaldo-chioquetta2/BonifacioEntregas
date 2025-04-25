using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Windows.Forms;

namespace TeleBonifacio.dao
{
    public class ContratosDAO
    {

        // Método para obter todos os contratos
        public int IdTipoContrato = 0;

        public DataTable GetAllContratos()
        {
            string sql = "SELECT * FROM Contratos ORDER BY ID DESC";
            return DB.ExecutarConsulta(sql);
        }

        // Método para obter contratos de um entregador específico
        public DataTable GetContratosByEntregadorId(int idEntregador)
        {
            string sql = $"SELECT * FROM Contratos WHERE idEntregador = {idEntregador} ORDER BY ID DESC";
            return DB.ExecutarConsulta(sql);
        }

        // Método para inserir um novo contrato
        public int InsertContrato(string descricao, int idEntregador, decimal valor, string status, DateTime dataInicio, DateTime dataTermino, string pix, string observacoes)
        {
            string sql = $@"INSERT INTO Contratos 
                            (Descricao, idEntregador, Valor, Status, DataInicio, DataTermino, PIX, Observacoes) 
                            VALUES 
                            ('{descricao}', {idEntregador}, {glo.sv(valor)}, '{status}', 
                            '{dataInicio:yyyy-MM-dd HH:mm:ss}', '{dataTermino:yyyy-MM-dd HH:mm:ss}', 
                            '{pix}', '{observacoes}')";
            DB.ExecutarComandoSQL(sql);

            string query = "SELECT MAX(ID) FROM Contratos";
            return DB.ExecutarConsultaCount(query);
        }

        // Método para atualizar um contrato existente
        public void UpdateContrato(int idContrato, string descricao, decimal valor, string status, DateTime dataInicio, DateTime dataTermino, string pix, string observacoes, string nomeEmpresa)
        {
            string sql = $@"UPDATE Contratos SET 
                    Descricao = '{descricao}', 
                    Valor = {glo.sv(valor)}, 
                    Status = '{status}', 
                    DataInicio = '{dataInicio:yyyy-MM-dd HH:mm:ss}', 
                    DataTermino = '{dataTermino:yyyy-MM-dd HH:mm:ss}', 
                    PIX = '{pix}', 
                    Observacoes = '{observacoes}', 
                    NomeEmpresa = '{nomeEmpresa.Replace("'", "''")}' 
                    WHERE ID = {idContrato}";
            DB.ExecutarComandoSQL(sql);
        }

        // Método para deletar um contrato
        public void DeleteContrato(int idContrato)
        {
            string sql = $"DELETE FROM Contratos WHERE ID = {idContrato}";
            DB.ExecutarComandoSQL(sql);
        }

        public tb.Contrato GetContratoById(int idContrato)
        {
            string sql = $"SELECT * FROM Contratos WHERE ID = {idContrato}";
            DataTable dt = DB.ExecutarConsulta(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new tb.Contrato
                {
                    Id = Convert.ToInt32(row["ID"]),
                    Descricao = row["Descricao"]?.ToString(),
                    IdEntregador = row["idEntregador"] != DBNull.Value ? Convert.ToInt32(row["idEntregador"]) : 0,
                    Valor = row["Valor"] != DBNull.Value ? Convert.ToDecimal(row["Valor"]) : 0,
                    Status = row["Status"]?.ToString(),
                    DataInicio = row["DataInicio"] != DBNull.Value ? Convert.ToDateTime(row["DataInicio"]) : DateTime.MinValue,
                    DataTermino = row["DataTermino"] != DBNull.Value ? Convert.ToDateTime(row["DataTermino"]) : DateTime.MinValue,
                    Observacoes = row["Observacoes"]?.ToString(),
                    NomeEmpresa = row["NomeEmpresa"]?.ToString(),
                    tpContrato = row["tpContrato"] != DBNull.Value ? Convert.ToInt32(row["tpContrato"]) : 0
                };
            }

            return null; // Contrato não encontrado
        }

        public int GetNextContratoId()
        {
            string query = "SELECT MAX(ID) AS MaxId FROM Contratos";
            DataTable dt = DB.ExecutarConsulta(query);

            if (dt.Rows.Count > 0 && dt.Rows[0]["MaxId"] != DBNull.Value)
            {
                return Convert.ToInt32(dt.Rows[0]["MaxId"]) + 1;
            }
            return 1; // Caso não haja registros, retorna 1 como primeiro ID
        }

        public tb.Contrato GetContratoCompleto(int idContrato)
        {
            string sqlContrato = $"SELECT * FROM Contratos WHERE ID = {idContrato}";
            DataTable dtContrato = DB.ExecutarConsulta(sqlContrato);

            if (dtContrato.Rows.Count > 0)
            {
                ConfigDAO config = new ConfigDAO();
                config.CarregarDados();

                int idEntregador = Convert.ToInt32(dtContrato.Rows[0]["idEntregador"]);
                EntregadorDAO entregadorDAO = new EntregadorDAO();
                tb.Entregador entregador = (tb.Entregador)entregadorDAO.GetPeloID(idEntregador.ToString());

                tb.Contrato contrato = new tb.Contrato();
                contrato.Id = Convert.ToInt32(dtContrato.Rows[0]["ID"]);
                contrato.Contratante = config.GetEmpresa();
                contrato.ContratanteCNPJ = config.GetCNPJ();
                contrato.ContratanteEndereco = config.GetEndereco();
                contrato.Descricao = dtContrato.Rows[0]["Descricao"].ToString();
                string sValor = dtContrato.Rows[0]["Valor"].ToString();
                contrato.Valor = Convert.ToDecimal(sValor);

                // Adicionando a leitura das datas de início e término do contrato
                if (dtContrato.Rows[0]["DataInicio"] != DBNull.Value)
                    contrato.DataInicio = Convert.ToDateTime(dtContrato.Rows[0]["DataInicio"]);
                else
                    contrato.DataInicio = DateTime.MinValue; // Caso a data não esteja definida

                if (dtContrato.Rows[0]["DataTermino"] != DBNull.Value)
                    contrato.DataTermino = Convert.ToDateTime(dtContrato.Rows[0]["DataTermino"]);
                else
                    contrato.DataTermino = DateTime.MinValue; // Caso a data não esteja definida

                if (entregador != null)
                {
                    contrato.ContratadaCNPJ = entregador.CPF; // Assume que o CPF está armazenado no campo ContratadaCNPJ
                    contrato.ContratadaEndereco = entregador.Endereco; // Endereço do entregador

                    // Adicionando campos NomeEmpresa e CNPJEmpresa
                    contrato.NomeEmpresa = entregador.NomeEmpresa ?? "Não informado"; // Nome da empresa do entregador
                    contrato.CNPJEmpresa = entregador.CNPJ ?? "Não informado"; // CNPJ da empresa do entregador
                }
                else
                {
                    contrato.ContratadaCNPJ = "Não informado";
                    contrato.ContratadaEndereco = "Não informado";
                    contrato.NomeEmpresa = "Não informado";
                    contrato.CNPJEmpresa = "Não informado";
                }

                // Buscar cláusulas associadas ao contrato
                string sqlClausulas = $"SELECT Descricao FROM Clausulas WHERE idContrato = {idContrato}";
                DataTable dtClausulas = DB.ExecutarConsulta(sqlClausulas);
                contrato.Clausulas = dtClausulas.AsEnumerable().Select(r => r["Descricao"].ToString()).ToList();

                contrato.Observacoes = dtContrato.Rows[0]["Observacoes"].ToString();

                return contrato;
            }

            return null;
        }


        public int InsertTipoContrato(string nome, int cadastroAssociado)
        {
            string query = "INSERT INTO TiposContrato (Nome, CadastroAssociado) VALUES (@nome, @cadastroAssociado)";
            List<OleDbParameter> parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@nome", nome),
                new OleDbParameter("@cadastroAssociado", cadastroAssociado)
            };

            try
            {
                // Executa a inserção
                DB.ExecutarComandoSQL(query, parametros);

                // Recupera o último ID gerado
                string queryId = "SELECT @@IDENTITY";
                DataTable result = DB.ExecutarConsulta(queryId);

                if (result.Rows.Count > 0 && result.Rows[0][0] != DBNull.Value)
                {
                    return Convert.ToInt32(result.Rows[0][0]);
                }
                else
                {
                    throw new Exception("Não foi possível recuperar o ID do novo tipo de contrato.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir tipo de contrato: {ex.Message}");
            }
        }

        public DataTable GetFilteredContratos(string filtroDescricao = "", string filtroStatus = "Todos", DateTime? inicio = null, DateTime? fim = null)
        {
            string sql = "SELECT * FROM Contratos WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(filtroDescricao))
            {
                sql += $" AND Descricao LIKE '%{filtroDescricao}%'";
            }

            if (filtroStatus != "Todos")
            {
                sql += $" AND Status = '{filtroStatus}'";
            }

            if (inicio.HasValue && fim.HasValue)
            {
                sql += $" AND DataInicio <= '{fim.Value:yyyy-MM-dd}' AND DataTermino >= '{inicio.Value:yyyy-MM-dd}'";
            }

            sql += " ORDER BY ID DESC";

            return DB.ExecutarConsulta(sql);
        }

        public DataTable GetTiposDeContrato()
        {
            string query = "SELECT IdTipoContrato, Nome FROM TiposContrato ORDER BY Nome ASC";
            return DB.ExecutarConsulta(query);
        }

        public int ObterIdCadastroAssociado(string textoTipoContrato)
        {
            try
            {
                string query = "SELECT IdTipoContrato, CadastroAssociado FROM TiposContrato WHERE Nome = @Nome";
                List<OleDbParameter> parametros = new List<OleDbParameter>
                {
                    new OleDbParameter("@Nome", textoTipoContrato)
                };

                DataTable resultado = DB.ExecutarConsulta(query, parametros);

                if (resultado.Rows.Count > 0)
                {
                    IdTipoContrato = Convert.ToInt32(resultado.Rows[0]["IdTipoContrato"]);
                    return Convert.ToInt32(resultado.Rows[0]["CadastroAssociado"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar o ID do tipo de contrato: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return -1; // Retorna -1 em caso de erro ou ID não encontrado
        }

    }
}
