using TeleBonifacio.tb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace TeleBonifacio.dao
{
    public class VendedoresDAO : BaseDAO
    {        

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Loja { get; set; }

        public bool Atende { get; set; }

        public VendedoresDAO()
        {
            
        }

        public void AdicionaVendedor(string nome, string loja, bool Atende)
        {
            int iat = (Atende == true) ? 1: 0;
            string sat = iat.ToString();
            String sql = @"INSERT INTO Vendedores (Nome, Loja, Atende) VALUES ("
                + glo.fa(nome) + ", "
                + glo.fa(loja)  
                + sat + ")";
            glo.ExecutarComandoSQL(sql);
        }

        public void EditaVendedor(int id, string nome, string loja, bool Atende)
        {
            int iat = (Atende == true) ? 1 : 0;
            string sat = iat.ToString();
            String sql = @"UPDATE Vendedores SET 
                Nome = " + glo.fa(nome) +
                ", Loja = '" + glo.fa(loja) +
                ", Atende = " + sat + 
                " WHERE ID = " + id.ToString();
            glo.ExecutarComandoSQL(sql);
        }
        public override object GetUltimo()
        {
            string query = "SELECT TOP 1 * FROM Vendedores ORDER BY ID Desc";
            return ExecutarConsultaVendedor(query); ;
        }

        public override void Grava(object obj)
        {
            VendedoresDAO vendedor = (VendedoresDAO)obj;
            string query;
            List<OleDbParameter> parameters;
            if (vendedor.Adicao)
            {
                query = "INSERT INTO Vendedores (Nome, Lojam Atende) VALUES (?, ?, ?)";
                parameters = ConstruirParametro(vendedor, true);
            }
            else
            {
                query = "UPDATE Vendedores SET Nome = ?, Loja = ?, Atende = ? WHERE ID = ?";
                parameters = ConstruirParametro(vendedor, false);
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

        private List<OleDbParameter> ConstruirParametro(VendedoresDAO vendedor, bool inserindo)
        {
            int iAt = (vendedor.Atende) ? 1 : 0;
            var parametros = new List<OleDbParameter>
            {
                new OleDbParameter("@Nome", vendedor.Nome),
                new OleDbParameter("@Loja", vendedor.Loja),
                new OleDbParameter("@Atende", iAt) 
            };
            if (!inserindo)
            {
                parametros.Add(new OleDbParameter("@ID", vendedor.Id));
            }
            return parametros;
        }

        public DataTable ExecutarConsultaVendedor(string query)
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
                            dataTable.Columns.Add("Loja", typeof(string));
                            dataTable.Columns.Add("Atende", typeof(string));
                            while (reader.Read())
                            {
                                DataRow row = dataTable.NewRow();
                                row["ID"] = reader["ID"];
                                row["Nome"] = reader["Nome"];
                                row["Loja"] = reader["Loja"];
                                row["Atende"] = reader["Atende"];                                
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

        public override tb.IDataEntity GetEsse()
        {
            return (tb.Vendedor)new tb.Vendedor
            {
                Id = Id,
                Nome = Nome,
                Loja = Loja,
                Atende = Atende
            };
        }

        internal string VlrPend(object selectedValue)
        {
            throw new NotImplementedException();
        }

        public override DataTable getDados()
        {
            string query = $"SELECT * FROM Vendedores";
            return ExecutarConsultaVendedor(query);
        }

        public override tb.IDataEntity GetPeloID(string id)
        {
            string query = $"SELECT * FROM Vendedores Where ID = {id} ";
            return ExecutarConsultaVendedor2(query);
        }

        private tb.Vendedor ExecutarConsultaVendedor2(string query)
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
                                Id = Convert.ToInt32(reader["ID"]);
                                Loja = reader["Loja"].ToString();
                                object oAt = reader["Atende"];
                                if (oAt == DBNull.Value)
                                {
                                    Atende = false;
                                } else
                                {
                                    int iAt = Convert.ToInt32(oAt);
                                    Atende = !(iAt == 0);
                                }                                
                                return (tb.Vendedor)GetEsse();
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

        public override tb.IDataEntity Apagar(int direcao, tb.IDataEntity entidade)
        {
            glo.ExecutarComandoSQL("DELETE FROM Vendedores WHERE ID = " + Id.ToString(), null);
            tb.Vendedor proximocliente = direcao > -1 ? ParaFrente() as tb.Vendedor : ParaTraz() as tb.Vendedor;
            if (proximocliente == null || proximocliente.Id == 0)
            {
                proximocliente = direcao > -1 ? ParaTraz() as tb.Vendedor : ParaFrente() as tb.Vendedor;
            }
            return proximocliente ?? new tb.Vendedor();
        }

        public override tb.IDataEntity ParaFrente()
        {
            string query = $"SELECT TOP 1 * FROM Vendedores Where Nome > '{Nome}' ORDER BY Nome ";
            return ExecutarConsultaVendedor2(query);
        }

        public override tb.IDataEntity ParaTraz()
        {
            string query = $"SELECT TOP 1 * FROM Vendedores Where Nome < '{Nome}' ORDER BY Nome Desc";
            return ExecutarConsultaVendedor2(query);
        }

        public override DataTable GetDadosOrdenados(string filtro="", string ordem ="")   
        {
            string query = $"SELECT * FROM Vendedores {filtro} Order By Nome {ordem}";
            return ExecutarConsultaVendedor(query);
        }

        public override string VeSeJaTem(object obj)
        {
            VendedoresDAO vendedor = (VendedoresDAO)obj;
            string wre = "";
            if (!vendedor.Adicao)
            {
                wre = " and ID <> " + vendedor.Id.ToString();
            }
            string queryNome = $"SELECT COUNT(*) FROM Vendedores WHERE Nome = '{vendedor.Nome}' " + wre;
            int countNome = glo.ExecutarConsultaCount(queryNome);
            if (countNome > 0)
            {
                return "Já existe um vendedor cadastrado com esse nome.";
            }
            return "";
        }


    }
}
