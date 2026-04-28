using System;
using System.Data;
using System.Text;
using TeleBonifacio.tb;
using System.Collections.Generic;
using System.Data.OleDb; // Necessário para os parâmetros

namespace TeleBonifacio.dao
{
    public class CodigoPartilheiraDAO
    {
        // O ListarTodos continua usando glo.getDados pois é um SELECT
        public List<CodigoPartilheira> ListarTodos()
        {
            string query = "SELECT Id, Codigo, Endereco FROM CodigosPartilheira";
            DataTable dt = glo.getDados(query);
            List<CodigoPartilheira> lista = new List<CodigoPartilheira>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new CodigoPartilheira
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Codigo = row["Codigo"]?.ToString() ?? "",
                    Endereco = row["Endereco"]?.ToString() ?? ""
                });
            }
            return lista;
        }

        public void Inserir(string codigo, string endereco)
        {
            string query = "INSERT INTO CodigosPartilheira (Codigo, Endereco) VALUES (?, ?)";

            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("?", codigo),
                new OleDbParameter("?", endereco)
            };

            DB.ExecutarComandoSQL(query, parametros);
        }

        public void Atualizar(int id, string codigo)
        {
            string query = "UPDATE CodigosPartilheira SET Codigo = ? WHERE Id = ?";

            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("?", codigo),
                new OleDbParameter("?", id)
            };

            DB.ExecutarComandoSQL(query, parametros);
        }

        public void AtualizarEndereco(int id, string novoEndereco)
        {
            string query = "UPDATE CodigosPartilheira SET Endereco = ? WHERE Id = ?";

            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("?", novoEndereco),
                new OleDbParameter("?", id)
            };

            DB.ExecutarComandoSQL(query, parametros);
        }

        public void Excluir(int id)
        {
            string query = "DELETE FROM CodigosPartilheira WHERE Id = ?";

            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("?", id)
            };

            DB.ExecutarComandoSQL(query, parametros);
        }

        public void LimparTodos()
        {
            string query = "DELETE FROM CodigosPartilheira";
            DB.ExecutarComandoSQL(query);
        }
    }
}