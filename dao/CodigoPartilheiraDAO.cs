using System.Data;
using System.Text;
using TeleBonifacio.tb;
using System.Collections.Generic;
using System;

namespace TeleBonifacio.dao
{
    public class CodigoPartilheiraDAO
    {
        public List<CodigoPartilheira> ListarTodos()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT Id, Codigo FROM CodigosPartilheira ");

            DataTable dt = glo.getDados(query.ToString());

            List<CodigoPartilheira> lista = new List<CodigoPartilheira>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new CodigoPartilheira
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Codigo = row["Codigo"].ToString()
                });
            }

            return lista;
        }

        public void Inserir(string codigo)
        {
            string query = $"INSERT INTO CodigosPartilheira (Codigo) VALUES ('{codigo.Replace("'", "''")}')";
            glo.getDados(query);
        }

        public void Atualizar(int id, string codigo)
        {
            string query = $"UPDATE CodigosPartilheira SET Codigo = '{codigo.Replace("'", "''")}' WHERE Id = {id}";
            glo.getDados(query);
        }

        public void Excluir(int id)
        {
            string query = $"DELETE FROM CodigosPartilheira WHERE Id = {id}";
            glo.getDados(query);
        }

        public void LimparTodos()
        {
            string query = "DELETE FROM CodigosPartilheira";
            glo.getDados(query);
        }
    }
}