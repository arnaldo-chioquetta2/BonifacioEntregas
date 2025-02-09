using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.tb;

namespace TeleBonifacio.dao
{
    public class TpoFaltaDAO : BaseDAO
    { 
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Cor { get; set; }

        public int Adiciona(string descricao, string cor)
        {
            string qry = $"SELECT TOP 1 * FROM TpoFalta Where Nome > '' ORDER BY IdFalta Desc ";
            DataTable ret = ExecutarConsulta(qry);
            string sId = ret.Rows[0][0].ToString();
            int reti = Convert.ToInt32(sId) + 1;

            // 🔹 Agora inclui a cor na inserção
            string sql = $"INSERT INTO TpoFalta (Nome, Cor) VALUES ('{descricao}', '{cor}')";
            DB.ExecutarComandoSQL(sql);
            return reti;
        }

        public override IDataEntity Apagar(int direcao, IDataEntity entidade)
        {
            DB.ExecutarComandoSQL("DELETE FROM TpoFalta WHERE IdFalta = " + Id.ToString(), null);
            tb.TpoFalta proximocliente = direcao > -1 ? ParaFrente() as tb.TpoFalta : ParaTraz() as tb.TpoFalta;
            if (proximocliente == null || proximocliente.Id == 0)
            {
                proximocliente = direcao > -1 ? ParaTraz() as tb.TpoFalta : ParaFrente() as tb.TpoFalta;
            }
            return proximocliente ?? new tb.TpoFalta();
        }

        public override DataTable getDados()
        {
            string query = $"SELECT * FROM TpoFalta Where Nome > '' ";
            return ExecutarConsulta(query);
        }

        public override DataTable GetDadosOrdenados(string filtro = "", string ordem = "")
        {
            string query = @"SELECT IdFalta AS id, Nome, Cor   
                FROM TpoFalta 
                Where Nome > '' 
                Order By Nome ";
            DataTable dt = DB.ExecutarConsulta(query);
            return dt;
        }

        public override IDataEntity GetEsse()
        {
            return (tb.TpoFalta)new tb.TpoFalta
            {
                Id = Id,
                Nome = Nome,
                Cor = Cor // 🔹 Inclui a cor na recuperação do objeto
            };
        }

        public override IDataEntity GetPeloID(string id)
        {
            string query = $"SELECT * FROM TpoFalta Where idFalta = {id} ";
            return ExecutarConsulta2(query);
        }

        public override object GetUltimo()
        {
            string query = "SELECT TOP 1 * FROM TpoFalta Where Nome > '' ORDER BY idFalta Desc";
            DataTable ret = ExecutarConsulta(query);
            Nome = ret.Rows[0]["nome"].ToString();
            Cor = ret.Rows[0]["Cor"].ToString();
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
                            dataTable.Columns.Add("Cor", typeof(string));
                            while (reader.Read())
                            {
                                DataRow row = dataTable.NewRow();
                                row["ID"] = reader["IdFalta"];
                                row["Nome"] = reader["Nome"];
                                row["Cor"] = reader["Cor"];
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

        private List<OleDbParameter> ConstruirParametro(TpoFaltaDAO tpoFalta, bool inserindo)
        {
            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@Nome", tpoFalta.Nome),
                new OleDbParameter("@Cor", tpoFalta.Cor) // 🔹 Adicionamos a cor aqui
            };
            if (!inserindo)
            {
                parametros.Add(new OleDbParameter("@ID", tpoFalta.Id));
            }
            return parametros;
        }


        public override void Grava(object obj)
        {
            TpoFaltaDAO tpoFalta = (TpoFaltaDAO)obj;
            string query;
            List<OleDbParameter> parameters;
            if (tpoFalta.Adicao)
            {
                query = "INSERT INTO TpoFalta (Nome, Cor) VALUES (?, ?)";
                parameters = ConstruirParametro(tpoFalta, true);
            }
            else
            {
                query = $"UPDATE TpoFalta SET Nome = '{tpoFalta.Nome}', Cor = '{tpoFalta.Cor}' WHERE IdFalta = {tpoFalta.Id}";
            }

            try
            {
                DB.ExecutarComandoSQL(query);
            }
            catch (Exception ex)
            {
                string x = ex.ToString();
                MessageBox.Show(x, "Erro na operação do banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public override tb.IDataEntity ParaFrente()
        {
            string query = $"SELECT TOP 1 * FROM TpoFalta Where Nome > '{Nome}' ORDER BY Nome Desc ";
            return ExecutarConsulta2(query);
        }

        public override IDataEntity ParaTraz()
        {
            string query = $"SELECT TOP 1 * FROM TpoFalta Where Nome < '{Nome}' ORDER BY Nome Desc";
            return ExecutarConsulta2(query);
        }

        private tb.TpoFalta ExecutarConsulta2(string query)
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
                                Id = Convert.ToInt32(reader["idFalta"]);
                                Cor = reader["Cor"].ToString(); // 🔹 Inclui a cor
                                return (tb.TpoFalta)GetEsse();
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
            TpoFaltaDAO tpoFalta = (TpoFaltaDAO)obj;
            string wre = "";
            if (!tpoFalta.Adicao)
            {
                wre = " and ID <> " + tpoFalta.Id.ToString();
            }
            string queryNome = $"SELECT COUNT(*) FROM TpoFalta WHERE Nome = '{tpoFalta.Nome}' " + wre;
            int countNome = DB.ExecutarConsultaCount(queryNome);
            if (countNome > 0)
            {
                return "Já existe um tipo cadastrado com esse nome.";
            }
            return "";
        }

        public List<tb.TpoFalta> getTipos()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT IdFalta, Nome FROM TpoFalta");
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            List<tb.TpoFalta> tipos = new List<tb.TpoFalta>();
            foreach (DataRow row in dt.Rows)
            {
                tipos.Add(new TpoFalta { Id = Convert.ToInt32(row["IdFalta"]), Nome = row["Nome"].ToString() });
            }
            return tipos;
        }


    }
}