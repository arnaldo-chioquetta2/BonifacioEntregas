using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OleDb;

namespace TeleBonifacio.dao
{
    public class ClausulasDAO
    {
        // Método para obter todas as cláusulas de um tipo de contrato
        public DataTable GetClausulasByTipoContrato(int idTipoContrato)
        {
            string query = "SELECT Ordem, Texto FROM ClausulasContrato WHERE IdTipoContrato = @IdTipoContrato ORDER BY Ordem";
            List<OleDbParameter> parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@IdTipoContrato", idTipoContrato)
            };
            return DB.ExecutarConsulta(query, parametros);
        }

        // Método para inserir uma nova cláusula
        public void InsertClausula(int idTipoContrato, int ordem, string texto)
        {
            string query = "INSERT INTO ClausulasContrato (IdTipoContrato, Ordem, Texto) VALUES (@idTipoContrato, @ordem, @texto)";
            DB.ExecutarComandoSQL(query, new List<OleDbParameter>
            {
                new OleDbParameter("@idTipoContrato", idTipoContrato),
                new OleDbParameter("@ordem", ordem),
                new OleDbParameter("@texto", texto)
            });
        }

        // Método para obter a próxima ordem para uma cláusula
        public int GetProximaOrdemClausula(int idTipoContrato)
        {
            string query = "SELECT MAX(Ordem) FROM ClausulasContrato WHERE IdTipoContrato = @idTipoContrato";
            DataTable dt = DB.ExecutarConsulta(query, new List<OleDbParameter>
            {
                new OleDbParameter("@idTipoContrato", idTipoContrato)
            });
            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                return Convert.ToInt32(dt.Rows[0][0]) + 1;
            }
            return 1; // Caso seja a primeira cláusula
        }

        // Método para atualizar uma cláusula pelo texto original
        public void UpdateClausulaByTexto(string textoOriginal, string novoTexto)
        {
            string sql = "UPDATE ClausulasContrato SET Texto = @novoTexto WHERE Texto = @textoOriginal";
            List<OleDbParameter> parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@novoTexto", novoTexto),
                new OleDbParameter("@textoOriginal", textoOriginal)
            };
            DB.ExecutarComandoSQL(sql, parametros);
        }

        // Método para deletar uma cláusula pelo texto
        public void DeleteClausulaByTexto(string texto)
        {
            string sql = "DELETE FROM ClausulasContrato WHERE Texto = @texto";
            List<OleDbParameter> parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@texto", texto)
            };
            DB.ExecutarComandoSQL(sql, parametros);
        }

        // Método para deletar uma cláusula pelo ID
        public void DeleteClausulaById(int idClausula)
        {
            string sql = "DELETE FROM ClausulasContrato WHERE IdClausula = @idClausula";
            List<OleDbParameter> parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@idClausula", idClausula)
            };
            DB.ExecutarComandoSQL(sql, parametros);
        }

        // Método para atualizar a ordem de uma cláusula
        public void UpdateOrdemClausula(int idClausula, int novaOrdem)
        {
            string sql = "UPDATE ClausulasContrato SET Ordem = @novaOrdem WHERE IdClausula = @idClausula";
            List<OleDbParameter> parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@novaOrdem", novaOrdem),
                new OleDbParameter("@idClausula", idClausula)
            };
            DB.ExecutarComandoSQL(sql, parametros);
        }

        public DataTable GetClausulasByContratoId(int contratoId)
        {
            string query = $"SELECT * FROM Clausulas WHERE Descricao> '' and idContrato = {contratoId} ORDER BY ID";
            return DB.ExecutarConsulta(query);
        }

        public void AdicionarClausula(int contratoId, string descricao)
        {
            string query = $@"INSERT INTO Clausulas (idContrato, Descricao) 
                      VALUES ({contratoId}, '{descricao.Replace("'", "''")}')";
            DB.ExecutarComandoSQL(query);
        }

        public void UpdateClausula(int clausulaId, string descricao)
        {
            string query = $@"UPDATE Clausulas 
                              SET Descricao = '{descricao.Replace("'", "''")}' 
                              WHERE ID = {clausulaId}";
            DB.ExecutarComandoSQL(query);
        }

        public void DeleteClausula(int clausulaId)
        {
            string query = $"DELETE FROM Clausulas WHERE ID = {clausulaId}";
            DB.ExecutarComandoSQL(query);
        }

        public void RemoverClausulasPorContrato(int contratoId)
        {
            string sql = "DELETE FROM Clausulas WHERE idContrato = " + contratoId.ToString();
            DB.ExecutarComandoSQL(sql);
        }

    }
}
