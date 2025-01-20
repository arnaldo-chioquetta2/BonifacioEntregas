using System;
using System.Data;

namespace TeleBonifacio.dao
{
    public class ClausulasDAO
    {
        // Método para obter todas as cláusulas de um contrato
        public DataTable GetClausulasByContratoId(int contratoId)
        {
            string query = $"SELECT * FROM Clausulas WHERE idContrato = {contratoId} ORDER BY ID";
            return DB.ExecutarConsulta(query);
        }

        // Método para inserir uma nova cláusula
        public void InsertClausula(int contratoId, string descricao)
        {
            string query = $@"INSERT INTO Clausulas (idContrato, Descricao) 
                              VALUES ({contratoId}, '{descricao.Replace("'", "''")}')";
            DB.ExecutarComandoSQL(query);
        }

        // Método para atualizar uma cláusula existente
        public void UpdateClausula(int clausulaId, string descricao)
        {
            string query = $@"UPDATE Clausulas 
                              SET Descricao = '{descricao.Replace("'", "''")}' 
                              WHERE ID = {clausulaId}";
            DB.ExecutarComandoSQL(query);
        }

        // Método para deletar uma cláusula
        public void DeleteClausula(int clausulaId)
        {
            string query = $"DELETE FROM Clausulas WHERE ID = {clausulaId}";
            DB.ExecutarComandoSQL(query);
        }

        public void AdicionarClausula(int contratoId, string descricao)
        {
            string query = $@"INSERT INTO Clausulas (idContrato, Descricao) 
                      VALUES ({contratoId}, '{descricao.Replace("'", "''")}')";
            DB.ExecutarComandoSQL(query);
        }

        public void RemoverClausulasPorContrato(int contratoId)
        {
            string sql = "DELETE FROM Clausulas WHERE idContrato = " + contratoId.ToString();
            DB.ExecutarComandoSQL(sql);
        }


    }
}
