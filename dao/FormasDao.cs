using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.tb;

namespace TeleBonifacio.dao
{
    public class FormasDAO : BaseDAO
    {
        public int Id { get; set; }

        public int Tipo { get; set; }

        public int Ativo { get; set; }
        public string Nome { get; private set; }

        public void Adiciona(string descricao, int tipo)
        {
            string sql = $"INSERT INTO Formas (Nome, Tipo) VALUES ('{descricao}', {tipo})";
            DB.ExecutarComandoSQL(sql);
        }

        public override IDataEntity Apagar(int direcao, IDataEntity entidade)
        {
            DB.ExecutarComandoSQL("DELETE FROM Formas WHERE ID = " + Id.ToString(), null);
            tb.Forma proximaForma = direcao > -1 ? ParaFrente() as tb.Forma : ParaTraz() as tb.Forma;
            if (proximaForma == null || proximaForma.Id == 0)
            {
                proximaForma = direcao > -1 ? ParaTraz() as tb.Forma : ParaFrente() as tb.Forma;
            }
            return proximaForma ?? new tb.Forma();
        }

        public override DataTable getDados()
        {
            string query = "SELECT * FROM Formas WHERE Nome > ''";
            return ExecutarConsulta(query);
        }

        public override DataTable GetDadosOrdenados(string filtro = "", string ordem = "")
        {
            string query = @"SELECT ID, Nome, Tipo, Ativo   
                             FROM Formas 
                             WHERE Ativo = 1 
                             ORDER BY Nome";
            return DB.ExecutarConsulta(query);
        }

        public override IDataEntity GetEsse()
        {
            return new tb.Forma
            {
                Id = Id,
                Nome = Nome,
                Tipo = Tipo,
                Ativo = Ativo
            };
        }

        public override IDataEntity GetPeloID(string id)
        {
            string query = $"SELECT * FROM Formas WHERE ID = {id}";
            return ExecutarConsulta2(query);
        }

        public override object GetUltimo()
        {
            string query = "SELECT TOP 1 * FROM Formas WHERE Nome > '' ORDER BY ID DESC";
            DataTable ret = ExecutarConsulta(query);
            Nome = ret.Rows[0]["Nome"].ToString();
            return ret;
        }

        public override void SetId(int iD)
        {
            this.Id = iD;
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
                            dataTable.Columns.Add("ID", typeof(int));
                            dataTable.Columns.Add("Nome", typeof(string));
                            dataTable.Columns.Add("Tipo", typeof(int));
                            dataTable.Columns.Add("Ativo", typeof(int));
                            while (reader.Read())
                            {
                                DataRow row = dataTable.NewRow();
                                row["ID"] = reader["ID"];
                                row["Nome"] = reader["Nome"];
                                row["Tipo"] = reader["Tipo"];
                                row["Ativo"] = reader["Ativo"];
                                dataTable.Rows.Add(row);
                            }
                            return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
        }

        public int getDefCred()
        {
            // DefCred
            string query = "SELECT * FROM Config";
            DataTable ret = ExecutarConsulta(query);            
            string sID = ret.Rows[0]["DefCred"].ToString();
            int ID = Convert.ToInt32(sID);
            return ID;
        }

        private tb.Forma ExecutarConsulta2(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
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
                                Id = Convert.ToInt32(reader["ID"]);
                                Tipo = Convert.ToInt32(reader["Tipo"]);
                                Ativo = Convert.ToInt32(reader["Ativo"]);
                                return (tb.Forma)GetEsse();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            return null;
        }

        private List<OleDbParameter> ConstruirParametro(FormasDAO forma, bool inserindo)
        {
            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@Nome", forma.Nome),
                new OleDbParameter("@Tipo", forma.Tipo),
                new OleDbParameter("@Ativo", forma.Ativo)
            };
            if (!inserindo)
            {
                parametros.Add(new OleDbParameter("@ID", forma.Id));
            }
            return parametros;
        }

        public override void Grava(object obj)
        {
            FormasDAO forma = (FormasDAO)obj;
            string query;
            List<OleDbParameter> parameters;
            if (forma.Adicao)
            {
                query = "INSERT INTO Formas (Nome, Tipo) VALUES (?, ?)";
                parameters = ConstruirParametro(forma, true);
            }
            else
            {
                query = "UPDATE Formas SET Nome = ?, Tipo = ?, Ativo = ? WHERE ID = ?";
                parameters = ConstruirParametro(forma, false);
            }

            try
            {
                DB.ExecutarComandoSQL(query, parameters);
            }
            catch (Exception ex)
            {
                string x = ex.ToString();
                MessageBox.Show(x, "Erro na operação do banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        public override IDataEntity ParaFrente()
        {
            string query = $"SELECT TOP 1 * FROM Formas WHERE Nome > '{Nome}' ORDER BY Nome DESC";
            return ExecutarConsulta2(query);
        }

        public override IDataEntity ParaTraz()
        {
            string query = $"SELECT TOP 1 * FROM Formas WHERE Nome < '{Nome}' ORDER BY Nome DESC";
            return ExecutarConsulta2(query);
        }

        public override string VeSeJaTem(object obj)
        {
            FormasDAO forma = (FormasDAO)obj;
            string wre = "";
            if (!forma.Adicao)
            {
                wre = " AND ID <> " + forma.Id.ToString();
            }
            string queryNome = $"SELECT COUNT(*) FROM Formas WHERE Nome = '{forma.Nome}'" + wre;
            int countNome = DB.ExecutarConsultaCount(queryNome);
            if (countNome > 0)
            {
                return "Já existe uma forma cadastrada com essa descrição.";
            }
            return "";
        }

        public List<tb.Forma> getFormas(int Tipo)
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"SELECT ID, Nome FROM Formas Where Ativo = 1 And Tipo = {Tipo} ");
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            List<tb.Forma> formas = new List<tb.Forma>();
            foreach (DataRow row in dt.Rows)
            {
                formas.Add(new tb.Forma { 
                    Id = Convert.ToInt32(row["ID"]), 
                    Nome = row["Nome"].ToString(), 
                });
            }
            return formas;
        }
    }
}
