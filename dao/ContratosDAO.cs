using System;
using System.Data;
using System.Collections.Generic;

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

        // Método para obter um contrato específico pelo ID
        public DataTable GetContratoById(int idContrato)
        {
            string sql = $"SELECT * FROM Contratos WHERE ID = {idContrato}";
            return DB.ExecutarConsulta(sql);
        }
    }
}
