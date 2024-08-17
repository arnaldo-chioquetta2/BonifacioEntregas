using System;
using System.Data;
using System.Data.OleDb;

namespace TeleBonifacio.dao
{
    public class PercentsDAO
    {
        // Método para obter todos os registros da tabela Percents
        private DataTable GetPercents()
        {
            string query = "SELECT * FROM Percents";
            DataTable dt = ExecutarConsulta(query);
            return dt;
        }

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

        internal DataTable getDados()
        {
            string sql = "SELECT ID, Perc, 'até', Valor FROM Percents ORDER BY Perc ";
            DataTable dt = DB.ExecutarConsulta(sql);
            return dt;
        }

        // Método para obter o valor percentual com base no ID
        public double? GetPercById(int id)
        {
            string query = "SELECT Perc FROM Percents WHERE ID = @id";
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
                            return Convert.ToDouble(result);
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

        public void Edita(int id, float perc, float valor)
        {
            string sPerc = glo.sv(perc);
            string sValor = glo.sv(valor);
            string sql = $@"UPDATE Percents SET Perc = {sPerc}, Valor = {sValor} WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        // Método para inserir um novo percentual na tabela
        public void InsertPercent(double perc, double valor)
        {
            string sql = "INSERT INTO Percents (Perc, Valor) VALUES (@perc, @valor)";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@perc", perc);
                    command.Parameters.AddWithValue("@valor", valor);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Método para deletar um percentual da tabela
        public void DeletePercentById(int id)
        {
            string sql = "DELETE FROM Percents WHERE ID = @id";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
