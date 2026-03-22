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
            // Adicionamos o Endereco no SELECT e já ordenamos por ele!
            query.Append("SELECT Id, Codigo, Endereco FROM CodigosPartilheira ORDER BY Endereco, Codigo");

            DataTable dt = glo.getDados(query.ToString());
            List<CodigoPartilheira> lista = new List<CodigoPartilheira>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new CodigoPartilheira
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Codigo = row["Codigo"].ToString(),
                    // Lê o endereço do banco. Se for nulo, joga 0.
                    Endereco = row["Endereco"] != DBNull.Value ? Convert.ToInt32(row["Endereco"]) : 0
                });
            }
            return lista;
        }

        // Atualize o Inserir para jogar o endereço no final da fila
        public void Inserir(string codigo, int proximoEndereco)
        {
            string query = $"INSERT INTO CodigosPartilheira (Codigo, Endereco) VALUES ('{codigo.Replace("'", "''")}', {proximoEndereco})";
            glo.getDados(query);
        }

        // Precisaremos de um método para atualizar apenas o Endereço quando ele editar na Grid
        public void AtualizarEndereco(int id, int novoEndereco)
        {
            string query = $"UPDATE CodigosPartilheira SET Endereco = {novoEndereco} WHERE Id = {id}";
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