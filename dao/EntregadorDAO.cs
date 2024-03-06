using TeleBonifacio.tb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace TeleBonifacio.dao
{
    public class EntregadorDAO : BaseDAO
    {
        protected int id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string CNH { get; set; }
        public DateTime DataValidadeCNH { get; set; }

        private int Linhas;

        public EntregadorDAO()
        {

        }

        public override void Grava(object obj)
        {
            EntregadorDAO entregador = (EntregadorDAO)obj;
            string query;
            List<OleDbParameter> parameters;
            int result = 0;
            if (entregador.Adicao)
            {
                query = "INSERT INTO Mecanicos (codi, Oper, Nome, Telefone, CNH, DataValidadeCNH) VALUES (?, ?, ?, ?, ?, ?)";
                parameters = ConstruirParametrosEntregador(entregador, true);
            }
            else
            {
                query = "UPDATE Mecanicos SET Nome = ?, Telefone = ?, CNH = ?, DataValidadeCNH = ? WHERE codi = ?";
                parameters = ConstruirParametrosEntregador(entregador, false);
            }

            try
            {
                glo.ExecutarComandoSQL(query, parameters);
            }
            catch (Exception ex)
            {
                string x = ex.ToString();
                // Considerar um melhor tratamento de exceções ou log
            }
        }

        internal static List<Entregador> ObterMotoBoys()
        {
            throw new NotImplementedException();
        }

        private List<OleDbParameter> ConstruirParametrosEntregador(EntregadorDAO entregador, bool inserindo)
        {
            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@Nome", entregador.Nome),
                new OleDbParameter("@Telefone", entregador.Telefone),
                new OleDbParameter("@CNH", entregador.CNH),
                new OleDbParameter("@DataValidadeCNH", entregador.DataValidadeCNH)
            };
            if (inserindo)
            {
                parametros.Insert(0, new OleDbParameter("@Oper", 3));
                parametros.Insert(0, new OleDbParameter("@codi", VeUltReg() + 1));
            }
            else
            {
                parametros.Add(new OleDbParameter("@codi", entregador.id));
            }
            return parametros;
        }

        private int VeUltReg()
        {
            string query = $"SELECT Max(codi) as codi FROM Mecanicos";
            using (OleDbConnection connection = new OleDbConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Convert.ToInt32(reader["codi"]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string x = ex.ToString();
                }
                return 0;
            }
        }

        public int ExecutarComandoSQL(string query, List<OleDbParameter> parameters)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.Add(param);
                        }
                    }
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public override tb.IDataEntity Apagar(int direcao, tb.IDataEntity entidade)
        {
            ExecutarComandoSQL("DELETE FROM Mecanicos WHERE codi = " + id.ToString(), null);
            tb.Entregador proximocliente = direcao > -1 ? ParaFrente() as tb.Entregador : ParaTraz() as tb.Entregador;
            if (proximocliente == null || proximocliente.Id == 0)
            {
                proximocliente = direcao > -1 ? ParaTraz() as tb.Entregador : ParaFrente() as tb.Entregador;
            }
            return proximocliente ?? new tb.Entregador();
        }

        public override tb.IDataEntity GetEsse()
        {
            return (tb.Entregador)new tb.Entregador
            {
                Id = id,
                Nome = Nome,
                Telefone = Telefone,
                CNH = CNH,
                DataValidadeCNH = DataValidadeCNH
            };

        }

        public override object GetUltimo()
        {
            string query = "SELECT TOP 1 * FROM Mecanicos Where Oper = 3 ORDER BY codi Desc";
            return ExecutarConsultaEntregador(query);
        }

        public override tb.IDataEntity ParaTraz()
        {
            string query = $"SELECT TOP 1 * FROM Mecanicos Where Oper = 3 and Nome < '{Nome}' ORDER BY Nome Desc";
            return ExecutarConsultaEntregador(query);
        }

        public override tb.IDataEntity ParaFrente()
        {
            string query = $"SELECT TOP 1 * FROM Mecanicos Where Oper = 3 and Nome > '{Nome}' ORDER BY Nome ";
            return ExecutarConsultaEntregador(query);
        }

        private tb.Entregador ExecutarConsultaEntregador(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Nome = reader["Nome"].ToString();
                                id = Convert.ToInt32(reader["codi"]);
                                Telefone = reader["Telefone"].ToString();
                                CNH = reader["CNH"].ToString();
                                if (reader["DataValidadeCNH"] != DBNull.Value)
                                {
                                    DataValidadeCNH = Convert.ToDateTime(reader["DataValidadeCNH"]);
                                }
                                else
                                {
                                    DataValidadeCNH = DateTime.MinValue;
                                }
                                return (tb.Entregador)GetEsse();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Tratamento de exceções adequado
                    throw;
                }
            }
            return null;
        }

        public override void SetarLinhas(int v)
        {
            this.Linhas = v;
        }

        public override tb.IDataEntity GetPeloID(string id)
        {
            string query = $"SELECT * FROM Mecanicos Where codi = {id} ";
            return ExecutarConsultaEntregador(query);
        }

        private DataTable ExecutarConsulta(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(this.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("id", typeof(int));
                            dataTable.Columns.Add("Nome", typeof(string));
                            dataTable.Columns.Add("Telefone", typeof(string));
                            dataTable.Columns.Add("CNH", typeof(string));
                            while (reader.Read())
                            {
                                DataRow row = dataTable.NewRow();
                                row["id"] = reader["codi"];
                                row["Nome"] = reader["Nome"];
                                row["Telefone"] = reader["Telefone"];
                                row["CNH"] = reader["CNH"];
                                dataTable.Rows.Add(row);
                            }
                            return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Aqui você pode decidir como lidar com a exceção
                    throw;
                }
            }
        }

        public override DataTable GetDadosOrdenados()
        {
            string query = "SELECT * FROM Mecanicos Where Oper = 3 and Nome > '' Order By Nome ";
            return ExecutarConsulta(query);
        }

        public override DataTable getDados()
        {
            string query = "SELECT * FROM Mecanicos Where Oper = 3 ";
            return ExecutarConsulta(query);
        }

        public override DataTable Fitrar(string pesquisar)
        {
            string query = $"SELECT * FROM Mecanicos Where Oper = 3 and Nome like '%{pesquisar}%' ";
            return ExecutarConsulta(query);
        }

        public override string VeSeJaTem(object obj)
        {
            EntregadorDAO entregador = (EntregadorDAO)obj;
            string wre = "";
            if (!entregador.Adicao)
            {
                wre = " and codi <> " + entregador.id.ToString();
            }
            string queryNome = $"SELECT COUNT(*) FROM Mecanicos WHERE Nome = '{entregador.Nome}' " + wre;
            int countNome = glo.ExecutarConsultaCount(queryNome);
            if (countNome > 0)
            {
                return "Já existe um cliente cadastrado com esse nome.";
            }
            return "";
        }


    }

}


