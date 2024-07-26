using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.gen;

namespace TeleBonifacio.dao
{
    public class VendedoresDAO : BaseDAO
    {

        public int Id { get; set; }

        protected string _Nome;
        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }

        public string Loja { get; set; }

        public bool Atende { get; set; }

        public string Nro { get; set; }

        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int Nivel { get; set; }

        public DateTime DataNascimento { get; set; }
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
        public TimeSpan HorarioSemanaInicio { get; set; }
        public TimeSpan HorarioSemanaFim { get; set; }
        public TimeSpan HorarioSabadoInicio { get; set; }
        public TimeSpan HorarioSabadoFim { get; set; }
        public string FormaPagamento { get; set; }
        public bool ValeAlimentacao { get; set; }
        public bool ValeTransporte { get; set; }
        public string LinhaOnibus { get; set; }
        public DateTime DataDemissao { get; set; }
        public string MotivoDemissao { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public string Cargo { get; set; }
        public string FoneEmergencia { get; set; }
        public int QtdFilhosMenor14 { get; set; }
        public string CTPS { get; set; }

        public bool FilhoComDeficiencia { get; set; }

        public string Amigo { get; set; }

        public string Fone { get; set; }

        public VendedoresDAO()
        {
            
        }

        public override void Grava(object obj)
        {
            VendedoresDAO vendedor = (VendedoresDAO)obj;
            string query;
            List<OleDbParameter> parameters = null;
            int iAtende = vendedor.Atende ? 1 : 0;
            string sCript = "";
            if (vendedor.Senha.Length > 0)
            {
                sCript = Cripto.Encrypt(vendedor.Senha);
            }
            string formattedSalary = vendedor.Salario.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            int valeAlimentacao = vendedor.ValeAlimentacao ? 1 : 0;
            int valeTransporte = vendedor.ValeTransporte ? 1 : 0;
            int filhoComDeficiencia = vendedor.FilhoComDeficiencia ? 1 : 0;
            if (vendedor.Adicao)
            {
                query = $"INSERT INTO Vendedores (Nome, Loja, Atende, Nro, Usuario, Senha, Nivel, DataNascimento, DataAdmissao, Salario, HorarioSemanaInicio, HorarioSemanaFim, HorarioSabadoInicio, HorarioSabadoFim, FormaPagamento, ValeAlimentacao, ValeTransporte, LinhaOnibus, DataDemissao, MotivoDemissao, RG, CPF, Cargo, FoneEmergencia, QtdFilhosMenor14, FilhoComDeficiencia, CTPS, Fone, Amigo) " +
                        $"VALUES ('{vendedor.Nome}', '{vendedor.Loja}', {iAtende}, '{vendedor.Nro}', '{vendedor.Usuario}', '{sCript}', {vendedor.Nivel}, '{vendedor.DataNascimento:yyyy-MM-dd}', '{vendedor.DataAdmissao:yyyy-MM-dd}', {formattedSalary}, '{vendedor.HorarioSemanaInicio}', '{vendedor.HorarioSemanaFim}', '{vendedor.HorarioSabadoInicio}', '{vendedor.HorarioSabadoFim}', '{vendedor.FormaPagamento}', {valeAlimentacao}, {valeTransporte}, '{vendedor.LinhaOnibus}', '{vendedor.DataDemissao:yyyy-MM-dd}', '{vendedor.MotivoDemissao}', '{vendedor.RG}', '{vendedor.CPF}', '{vendedor.Cargo}', '{vendedor.FoneEmergencia}', {vendedor.QtdFilhosMenor14}, {filhoComDeficiencia}, '{vendedor.CTPS}', '{vendedor.Fone}', '{vendedor.Amigo}')";
            }
            else
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append($"UPDATE Vendedores SET ");
                sqlBuilder.Append($"Nome = '{vendedor.Nome}', ");
                sqlBuilder.Append($"Loja = '{vendedor.Loja}', ");
                sqlBuilder.Append($"Atende = {iAtende}, ");
                sqlBuilder.Append($"Nro = '{vendedor.Nro}', ");
                sqlBuilder.Append($"Usuario = '{vendedor.Usuario}', ");
                sqlBuilder.Append($"Senha = '{sCript}', ");
                sqlBuilder.Append($"Nivel = {vendedor.Nivel}, ");
                sqlBuilder.Append($"Fone = '{vendedor.Fone}', ");
                sqlBuilder.Append($"Amigo = '{vendedor.Amigo}', ");
                sqlBuilder.Append(GetDataSqlPart(vendedor.DataNascimento, "DataNascimento"));
                sqlBuilder.Append(GetDataSqlPart(vendedor.DataAdmissao, "DataAdmissao"));
                sqlBuilder.Append(GetDataSqlPart(vendedor.DataDemissao, "DataDemissao"));
                if (vendedor.HorarioSemanaInicio != TimeSpan.Zero)
                    sqlBuilder.Append($"HorarioSemanaInicio = '#{vendedor.HorarioSemanaInicio:hh\\:mm}#', ");
                if (vendedor.HorarioSemanaFim != TimeSpan.Zero)
                    sqlBuilder.Append($"HorarioSemanaFim = '#{vendedor.HorarioSemanaFim:hh\\:mm}#', ");
                if (vendedor.HorarioSabadoInicio != TimeSpan.Zero)
                    sqlBuilder.Append($"HorarioSabadoInicio = '#{vendedor.HorarioSabadoInicio:hh\\:mm}#', ");
                if (vendedor.HorarioSabadoFim != TimeSpan.Zero)
                    sqlBuilder.Append($"HorarioSabadoFim = '#{vendedor.HorarioSabadoFim:hh\\:mm}#', ");
                sqlBuilder.Append($"Salario = {vendedor.Salario.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}, ");
                sqlBuilder.Append($"FormaPagamento = '{vendedor.FormaPagamento}', ");
                sqlBuilder.Append($"ValeAlimentacao = {valeAlimentacao}, ");
                sqlBuilder.Append($"ValeTransporte = {valeTransporte}, "); 
                sqlBuilder.Append($"LinhaOnibus = '{vendedor.LinhaOnibus}', ");
                sqlBuilder.Append($"MotivoDemissao = '{vendedor.MotivoDemissao}', ");
                sqlBuilder.Append($"RG = '{vendedor.RG}', ");
                sqlBuilder.Append($"CPF = '{vendedor.CPF}', ");
                sqlBuilder.Append($"Cargo = '{vendedor.Cargo}', ");
                sqlBuilder.Append($"FoneEmergencia = '{vendedor.FoneEmergencia}', ");
                sqlBuilder.Append($"QtdFilhosMenor14 = {vendedor.QtdFilhosMenor14}, ");
                sqlBuilder.Append($"FilhoComDeficiencia = {filhoComDeficiencia}, "); 
                sqlBuilder.Append($"CTPS = '{vendedor.CTPS}' ");
                sqlBuilder.Append($"WHERE ID = {vendedor.Id};");
                query = sqlBuilder.ToString();
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

        public string GetDataSqlPart(DateTime? data, string fieldName)
        {            
            if (data.Value.Date != glo.D1)
            {
                return $"{fieldName} = '{data.Value:yyyy-MM-dd}', ";
            }
            else
            {
                return "";
            }
        }

        private DataTable ExecutarConsultaVendedorODBC(string query)
        {
            using (OdbcConnection connection = new OdbcConnection(glo.connectionString))
            {
                DataTable dataTable = new DataTable();
                try
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    {
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ODBC execution error: " + ex.Message);
                }
                return dataTable;
            }
        }

        private DataTable ExecutarConsultaVendedorADO(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                DataTable dataTable = new DataTable();
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ADO execution error: " + ex.Message);
                }
                return dataTable;
            }
        }

        private tb.Vendedor ExecutarConsultaVendedor2(string query)
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
                                Loja = reader["Loja"].ToString();
                                object oAt = reader["Atende"];
                                if (oAt == DBNull.Value)
                                {
                                    Atende = false;
                                }
                                else
                                {
                                    int iAt = Convert.ToInt32(oAt);
                                    Atende = !(iAt == 0);
                                }
                                Nro = reader["Nro"].ToString();
                                Usuario = reader["Usuario"].ToString();
                                Senha = reader["Senha"].ToString();
                                Nivel = reader["Nivel"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Nivel"]);
                                DataNascimento = reader["DataNascimento"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataNascimento"]);
                                DataAdmissao = reader["DataAdmissao"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataAdmissao"]);
                                Salario = reader["Salario"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Salario"]);
                                HorarioSemanaInicio = reader["HorarioSemanaInicio"] == DBNull.Value ? TimeSpan.Zero : TimeSpan.Parse(reader["HorarioSemanaInicio"].ToString());
                                HorarioSemanaFim = reader["HorarioSemanaFim"] == DBNull.Value ? TimeSpan.Zero : TimeSpan.Parse(reader["HorarioSemanaFim"].ToString());
                                HorarioSabadoInicio = reader["HorarioSabadoInicio"] == DBNull.Value ? TimeSpan.Zero : TimeSpan.Parse(reader["HorarioSabadoInicio"].ToString());
                                HorarioSabadoFim = reader["HorarioSabadoFim"] == DBNull.Value ? TimeSpan.Zero : TimeSpan.Parse(reader["HorarioSabadoFim"].ToString());
                                FormaPagamento = reader["FormaPagamento"].ToString();
                                ValeAlimentacao = reader["ValeAlimentacao"] == DBNull.Value ? false : Convert.ToBoolean(reader["ValeAlimentacao"]);
                                ValeTransporte = reader["ValeTransporte"] == DBNull.Value ? false : Convert.ToBoolean(reader["ValeTransporte"]);
                                LinhaOnibus = reader["LinhaOnibus"].ToString();
                                DataDemissao = reader["DataDemissao"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DataDemissao"]);
                                MotivoDemissao = reader["MotivoDemissao"].ToString();
                                RG = reader["RG"].ToString();
                                CPF = reader["CPF"].ToString();
                                Cargo = reader["Cargo"].ToString();
                                FoneEmergencia = reader["FoneEmergencia"].ToString();
                                QtdFilhosMenor14 = reader["QtdFilhosMenor14"] == DBNull.Value ? 0 : Convert.ToInt32(reader["QtdFilhosMenor14"]);
                                FilhoComDeficiencia = reader["FilhoComDeficiencia"] == DBNull.Value ? false : Convert.ToBoolean(reader["FilhoComDeficiencia"]);
                                CTPS = reader["CTPS"].ToString();
                                Fone = reader["Fone"].ToString();
                                Amigo = reader["Amigo"].ToString();
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

        public override tb.IDataEntity GetEsse()
        {
            return new tb.Vendedor
            {
                Id = Id,
                Nome = Nome,
                Loja = Loja,
                Atende = Atende,
                Nro = Nro,
                Usuario = Usuario,
                Senha = Senha,
                Nivel = Nivel,
                DataNascimento = DataNascimento,
                DataAdmissao = DataAdmissao,
                Salario = Salario,
                HorarioSemanaInicio = HorarioSemanaInicio,
                HorarioSemanaFim = HorarioSemanaFim,
                HorarioSabadoInicio = HorarioSabadoInicio,
                HorarioSabadoFim = HorarioSabadoFim,
                FormaPagamento = FormaPagamento,
                ValeAlimentacao = ValeAlimentacao,
                ValeTransporte = ValeTransporte,
                LinhaOnibus = LinhaOnibus,
                DataDemissao = DataDemissao,
                MotivoDemissao = MotivoDemissao,
                RG = RG,
                CPF = CPF,
                Cargo = Cargo,
                FoneEmergencia = FoneEmergencia,
                QtdFilhosMenor14 = QtdFilhosMenor14,
                FilhoComDeficiencia = FilhoComDeficiencia,
                CTPS = CTPS,
                Fone = Fone,
                Amigo = Amigo
            };
        }

        public DataTable getBalconistas()
        {
            string query = "SELECT ID, Nro, Nome FROM Vendedores WHERE Nro > '0' ORDER BY Nome";
            if (glo.ODBC)
            {
                return ExecutarConsultaVendedorODBC(query);
            }
            else
            {
                return ExecutarConsultaVendedorADO(query);
            }
        }

        public override object GetUltimo()
        {
            string query = "SELECT TOP 1 * FROM Vendedores ORDER BY ID Desc";
            return ExecutarConsultaVendedor(query); ;
        }

        public DataTable ExecutarConsultaVendedor(string query)
        {
            if (glo.ODBC)
            {
                glo.Loga("ODBC");
                return ExecutarConsultaVendedorODBC(query);
            } else
            {
                glo.Loga("Não ODBC");
                return ExecutarConsultaVendedorADO(query);
            }
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

        public override tb.IDataEntity Apagar(int direcao, tb.IDataEntity entidade)
        {
            DB.ExecutarComandoSQL("DELETE FROM Vendedores WHERE ID = " + Id.ToString(), null);
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
            int countNome = DB.ExecutarConsultaCount(queryNome);
            if (countNome > 0)
            {
                return "Já existe um vendedor cadastrado com esse nome.";
            }
            return "";
        }

        public override void SetId(int iD)
        {
            this.Id = iD;
        }
        public override void SetNome(string nome)
        {
            this.Nome = nome;
        }

    }
}
