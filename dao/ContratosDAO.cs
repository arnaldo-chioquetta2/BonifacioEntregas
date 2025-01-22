using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace TeleBonifacio.dao
{
    public class ContratosDAO
    {
        // Método para obter todos os contratos
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
        public void UpdateContrato(int idContrato, string descricao, decimal valor, string status, DateTime dataInicio, DateTime dataTermino, string pix, string observacoes)
        {
            string sql = $@"UPDATE Contratos SET 
                            Descricao = '{descricao}', 
                            Valor = {glo.sv(valor)}, 
                            Status = '{status}', 
                            DataInicio = '{dataInicio:yyyy-MM-dd HH:mm:ss}', 
                            DataTermino = '{dataTermino:yyyy-MM-dd HH:mm:ss}', 
                            PIX = '{pix}', 
                            Observacoes = '{observacoes}'
                            WHERE ID = {idContrato}";
            DB.ExecutarComandoSQL(sql);
        }

        // Método para deletar um contrato
        public void DeleteContrato(int idContrato)
        {
            string sql = $"DELETE FROM Contratos WHERE ID = {idContrato}";
            DB.ExecutarComandoSQL(sql);
        }

        // Método corrigido em ContratosDAO
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
                    // Pix = row["PIX"]?.ToString(),
                    Observacoes = row["Observacoes"]?.ToString()
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

                tb.Contrato contrato = new tb.Contrato
                {
                    Id = Convert.ToInt32(dtContrato.Rows[0]["ID"]),
                    Contratante = config.GetEmpresa(),
                    ContratanteCNPJ = config.GetCNPJ(),
                    ContratanteEndereco = config.GetEndereco(),
                    Contratada = dtContrato.Rows[0]["Descricao"].ToString()
                };

                if (entregador != null)
                {
                    contrato.ContratadaCNPJ = entregador.CPF; // Assume que o CPF está armazenado no campo ContratadaCNPJ
                    contrato.ContratadaEndereco = entregador.Endereco; // Endereço do entregador

                    // Adicionando campos NomeEmpresa e CNPJEmpresa
                    contrato.NomeEmpresa = entregador.NomeEmpresa; // Nome da empresa do entregador
                    contrato.CNPJEmpresa = entregador.CNPJ; // CNPJ da empresa do entregador
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
                return contrato;
            }

            return null;
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


    }
}
