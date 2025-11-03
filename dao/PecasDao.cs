using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace TeleBonifacio.dao
{
    public class PecasDAO
    {
        // Método para obter todos os registros da tabela Pecas
        public DataTable GetAllPecas()
        {
            string query = "SELECT * FROM Pecas";
            return ExecutarConsulta(query);
        }

        // Método para obter todas as peças de um carro específico
        public DataTable GetPecasByCarroId(int idCarro)
        {
            string query = "";
            if (idCarro>0)
            {
                query = "SELECT * FROM Pecas WHERE IdCarro = @idCarro";
            } else
            {
                query = "SELECT * FROM Pecas";
            }
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idCarro", idCarro);
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dataTable;
        }

        // Método para inserir uma nova peça
        public void InsertPeca(string nome, int idCarro)
        {
            string query = "INSERT INTO Pecas (Nome, IdCarro) VALUES (@nome, @idCarro)";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nome", nome);
                        command.Parameters.AddWithValue("@idCarro", idCarro);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // Método para atualizar uma peça
        public void UpdatePeca(int idPeca, string nome)
        {
            string query = "UPDATE Pecas SET Nome = @nome WHERE IdPeca = @idPeca";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nome", nome);
                        command.Parameters.AddWithValue("@idPeca", idPeca);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // Método para deletar uma peça
        public void DeletePecaById(int idPeca)
        {
            string query = "DELETE FROM Pecas WHERE IdPeca = @idPeca";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idPeca", idPeca);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // Método genérico para executar consultas e retornar um DataTable
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

        public void InsertCarro(string nomeCarro)
        {
            string query = "INSERT INTO Carros (Nome) VALUES (@nome)";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nome", nomeCarro);
                    command.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetAllCarros()
        {
            string query = "SELECT * FROM Carros";
            return ExecutarConsulta(query);
        }        

        public void InsertCaracteristica(string descricao, int idPeca)
        {
            string query = "INSERT INTO Caracteristicas (Descricao, IdPeca) VALUES (@descricao, @idPeca)";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@descricao", descricao);
                    command.Parameters.AddWithValue("@idPeca", idPeca);
                    command.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetCaracteristicasByPecaId(int idPeca)
        {
            string query = "SELECT * FROM Caracteristicas WHERE IdPeca = @idPeca";
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idPeca", idPeca);
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dataTable;
        }

        public DataTable SearchCarros(string termoPesquisa)
        {
            string query = "SELECT * FROM Carros WHERE Nome LIKE LIKE '%" + termoPesquisa + "%'";
            DataTable dt = DB.ExecutarConsulta(query);
            return dt;
        }

        public void UpdateCarro(int idCarro, string nome)
        {
            string query = "UPDATE Carros SET Nome = @nome WHERE IdCarro = @idCarro";
            List<OleDbParameter> parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@nome", nome),
                new OleDbParameter("@idCarro", idCarro)
            };
            DB.ExecutarComandoSQL(query, parametros);
        }

        public void UpdateCaracteristica(int idCaracteristica, string descricao)
        {
            string query = "UPDATE Caracteristicas SET Descricao = @descricao WHERE IdCaracteristica = @idCaracteristica";
            List<OleDbParameter> parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@descricao", descricao),
                new OleDbParameter("@idCaracteristica", idCaracteristica)
            };
            DB.ExecutarComandoSQL(query, parametros);
        }

        public DataTable GetCodigosByPecaId(int idPeca)
        {
            string query = "SELECT * FROM Codigos WHERE IdPeca = @idPeca";
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idPeca", idPeca);
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dataTable;
        }

        public void InsertCodigo(int idPeca, string codigo)
        {
            string query = "INSERT INTO Codigos (IdPeca, Codigo) VALUES (@idPeca, @codigo)";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idPeca", idPeca);
                        command.Parameters.AddWithValue("@codigo", codigo);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void UpdateCodigo(int idCodigo, string novoCodigo)
        {
            string query = "UPDATE Codigos SET Codigo = @codigo WHERE IdCodigo = @idCodigo";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@codigo", novoCodigo);
                        command.Parameters.AddWithValue("@idCodigo", idCodigo);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void DeleteCodigoById(int idCodigo)
        {
            string query = "DELETE FROM Codigos WHERE IdCodigo = @idCodigo";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idCodigo", idCodigo);
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
