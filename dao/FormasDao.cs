using System;
using System.Data;
using System.Data.OleDb;

namespace TeleBonifacio.dao
{
    public class FormasDAO
    {
        // Método para executar consultas e preencher um DataTable
        private DataTable ExecutarConsulta(string query)
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dataTable;
        }

        // Método para obter dados da tabela Formas filtrados pelo Tipo
        internal DataTable GetDadosPorTipo(int tipo)
        {
            string sql = $"SELECT ID, Descricao, Tipo FROM Formas WHERE Tipo = {tipo} ORDER BY Descricao";
            DataTable dt = ExecutarConsulta(sql);
            return dt;
        }

        // Método para obter a descrição de uma forma com base no ID
        public string GetDescricaoById(int id)
        {
            string query = "SELECT Descricao FROM Formas WHERE ID = @id";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            return result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null;
        }

        // Método para editar uma forma
        public void Edita(int id, string descricao, int tipo)
        {
            string sql = "UPDATE Formas SET Descricao = @descricao, Tipo = @tipo WHERE ID = @id";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@descricao", descricao);
                        command.Parameters.AddWithValue("@tipo", tipo);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // Método para inserir uma nova forma na tabela
        public void InsertForma(string descricao, int tipo)
        {
            string sql = "INSERT INTO Formas (Descricao, Tipo) VALUES (@descricao, @tipo)";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@descricao", descricao);
                        command.Parameters.AddWithValue("@tipo", tipo);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // Método para deletar uma forma da tabela
        public void DeleteFormaById(int id)
        {
            string sql = "DELETE FROM Formas WHERE ID = @id";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
