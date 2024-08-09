using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.tb;

namespace TeleBonifacio.dao
{
    public class FornecedorDao : BaseDAO
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public bool EhForn { get; set; }

        public string email { get; set; }

        public void Adiciona(string descricao)
        {
            string sql = $"INSERT INTO Fornecedores (Nome, EhForn) VALUES ('{descricao}', 1)";
            DB.ExecutarComandoSQL(sql);
        }

        public override IDataEntity Apagar(int direcao, IDataEntity entidade)
        {
            DB.ExecutarComandoSQL("DELETE FROM Fornecedores WHERE IdForn = " + Id.ToString(), null);
            tb.Fornecedor proximocliente = direcao > -1 ? ParaFrente() as tb.Fornecedor : ParaTraz() as tb.Fornecedor;
            if (proximocliente == null || proximocliente.Id == 0)
            {
                proximocliente = direcao > -1 ? ParaTraz() as tb.Fornecedor : ParaFrente() as tb.Fornecedor;
            }
            return proximocliente ?? new tb.Fornecedor();
        }

        public override DataTable getDados()
        {
            string query = $"SELECT * FROM Fornecedores Where Nome > '' ";
            return ExecutarConsulta(query);
        }

        public override DataTable GetDadosOrdenados(string filtro = "", string ordem = "")
        {
            if (filtro.Length > 0)
            {
                filtro = " and " + filtro;
            }            
            string query = $@"SELECT IdForn AS id, Nome  
                FROM Fornecedores 
                Where Nome > '' {filtro}
                Order By Nome ";
            DataTable dt = DB.ExecutarConsulta(query);
            return dt;
        }

        public override IDataEntity GetEsse()
        {
            return (tb.Fornecedor)new tb.Fornecedor
            {
                Id = Id,
                Nome = Nome,
                EhForn = EhForn,
                email = email
            };
        }

        public override IDataEntity GetPeloID(string id)
        {
            string query = $"SELECT * FROM Fornecedores Where IdForn = {id} ";
            return ExecutarConsulta2(query);
        }

        public override object GetUltimo()
        {
            string query = "SELECT TOP 1 * FROM Fornecedores Where Nome > '' ORDER BY IdForn Desc";
            DataTable ret = ExecutarConsulta(query);
            if (ret.Rows.Count>0)
            {
                Nome = ret.Rows[0]["Nome"].ToString();
                EhForn = (ret.Rows[0]["EhForn"].ToString() == "1");
                email = ret.Rows[0]["email"].ToString();
            }
            return ret;
        }

        public override void SetId(int iD)
        {
            this.Id = iD;
        }

        private DataTable ExecutarConsulta(string query)
        {
            DataTable dataTable = new DataTable();

            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dataTable); // Preenche o DataTable automaticamente
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }

            return dataTable;
        }

        private List<OleDbParameter> ConstruirParametro(FornecedorDao Fornecedor, bool inserindo)
        {
            int EhForn = Fornecedor.EhForn ? 1 : 0;
            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@Nome", Fornecedor.Nome),
                new OleDbParameter("@EhForn", EhForn),
                new OleDbParameter("@email", Fornecedor.email)
        };
            if (!inserindo)
            {
                parametros.Add(new OleDbParameter("@ID", Fornecedor.Id));
            }
            return parametros;
        }

        public override void Grava(object obj)
        {
            FornecedorDao Fornecedor = (FornecedorDao)obj;
            string query;
            List<OleDbParameter> parameters;
            if (Fornecedor.Adicao)
            {
                query = "INSERT INTO Fornecedores (Nome, EhForn, email) VALUES (?, ?, ?)";
                parameters = ConstruirParametro(Fornecedor, true);
            }
            else
            {
                query = "UPDATE Fornecedores SET Nome = ?, EhForn = ?, email = ? WHERE IdForn = ?";
                parameters = ConstruirParametro(Fornecedor, false);
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

        public override tb.IDataEntity ParaFrente()
        {
            string query = $"SELECT TOP 1 * FROM Fornecedores Where Nome > '{Nome}' ORDER BY Nome Desc ";
            return ExecutarConsulta2(query);
        }

        public override IDataEntity ParaTraz()
        {
            string query = $"SELECT TOP 1 * FROM Fornecedores Where Nome < '{Nome}' ORDER BY Nome Desc";
            return ExecutarConsulta2(query);
        }

        private tb.Fornecedor ExecutarConsulta2(string query)
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
                                Id = Convert.ToInt32(reader["IdForn"]);
                                EhForn = (((int)reader["EhForn"]) == 1);
                                if (reader["email"] != DBNull.Value)
                                {
                                    email = (string)reader["email"];
                                }
                                else
                                {
                                    email = "";
                                }
                                return (tb.Fornecedor)GetEsse();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        public override string VeSeJaTem(object obj)
        {
            FornecedorDao Fornecedor = (FornecedorDao)obj;
            string wre = "";
            if (!Fornecedor.Adicao)
            {
                wre = " and ID <> " + Fornecedor.Id.ToString();
            }
            string queryNome = $"SELECT COUNT(*) FROM Fornecedores WHERE Nome = '{Fornecedor.Nome}' " + wre;
            int countNome = DB.ExecutarConsultaCount(queryNome);
            if (countNome > 0)
            {
                return "Já existe um tipo cadastrado com esse nome.";
            }
            return "";
        }

        public List<tb.Fornecedor> getForns()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT IdForn, Nome FROM Fornecedores");
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            List<tb.Fornecedor> tipos = new List<tb.Fornecedor>();
            foreach (DataRow row in dt.Rows)
            {
                tipos.Add(new Fornecedor { Id = Convert.ToInt32(row["IdForn"]), Nome = row["Nome"].ToString() });
            }
            return tipos;
        }


    }
}