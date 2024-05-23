using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleBonifacio.tb;

namespace TeleBonifacio.dao
{
    public class FornecedorDao : BaseDAO
    {
        public int Id { get; set; }

        public string Nome { get; set; }


        public void Adiciona(string descricao)
        {
            string sql = $"INSERT INTO Fornecedores (Nome) VALUES ('{descricao}')";
            glo.ExecutarComandoSQL(sql);
        }

        public override IDataEntity Apagar(int direcao, IDataEntity entidade)
        {
            glo.ExecutarComandoSQL("DELETE FROM Fornecedores WHERE IdForn = " + Id.ToString(), null);
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
            string query = @"SELECT IdForn AS id, Nome  
                FROM Fornecedores 
                Where Nome > '' 
                Order By Nome ";
            DataTable dt = glo.ExecutarConsulta(query);
            return dt;
        }

        public override IDataEntity GetEsse()
        {
            return (tb.Fornecedor)new tb.Fornecedor
            {
                Id = Id,
                Nome = Nome
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
            }            
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
                            while (reader.Read())
                            {
                                DataRow row = dataTable.NewRow();
                                row["ID"] = reader["IdForn"];
                                row["Nome"] = reader["Nome"];
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

        private List<OleDbParameter> ConstruirParametro(FornecedorDao Fornecedor, bool inserindo)
        {
            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@Nome", Fornecedor.Nome)
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
                query = "INSERT INTO Fornecedores (Nome) VALUES (?)";
                parameters = ConstruirParametro(Fornecedor, true);
            }
            else
            {
                query = "UPDATE Fornecedores SET Nome = ? WHERE IdForn = ?";
                parameters = ConstruirParametro(Fornecedor, false);
            }

            try
            {
                glo.ExecutarComandoSQL(query, parameters);
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
                                Id = Convert.ToInt32(reader["IdForn"]);
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
            int countNome = glo.ExecutarConsultaCount(queryNome);
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
            DataTable dt = glo.ExecutarConsulta(query.ToString());
            List<tb.Fornecedor> tipos = new List<tb.Fornecedor>();
            foreach (DataRow row in dt.Rows)
            {
                tipos.Add(new Fornecedor { Id = Convert.ToInt32(row["IdForn"]), Nome = row["Nome"].ToString() });
            }
            return tipos;
        }


    }
}