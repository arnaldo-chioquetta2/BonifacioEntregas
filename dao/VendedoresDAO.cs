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
            if (vendedor.Adicao)
            {
                query = $"INSERT INTO Vendedores (Nome, Loja, Atende, Nro, Usuario, Senha, Nivel, DataNascimento, DataAdmissao, Salario, HorarioSemanaInicio, HorarioSemanaFim, HorarioSabadoInicio, HorarioSabadoFim, FormaPagamento, ValeAlimentacao, ValeTransporte, LinhaOnibus, DataDemissao, MotivoDemissao, RG, CPF, Cargo, FoneEmergencia, QtdFilhosMenor14, FilhoComDeficiencia, QtdFilhosMenor14, CTPS) " +
                        $"VALUES ('{vendedor.Nome}', '{vendedor.Loja}', {iAtende}, '{vendedor.Nro}', '{vendedor.Usuario}', '{sCript}', {vendedor.Nivel}, '{vendedor.DataNascimento:yyyy-MM-dd}', '{vendedor.DataAdmissao:yyyy-MM-dd}', {vendedor.Salario:F2}, '{vendedor.HorarioSemanaInicio}', '{vendedor.HorarioSemanaFim}', '{vendedor.HorarioSabadoInicio}', '{vendedor.HorarioSabadoFim}', '{vendedor.FormaPagamento}', {vendedor.ValeAlimentacao}, {vendedor.ValeTransporte}, '{vendedor.LinhaOnibus}', '{vendedor.DataDemissao:yyyy-MM-dd}', '{vendedor.MotivoDemissao}', '{vendedor.RG}', '{vendedor.CPF}', '{vendedor.Cargo}', '{vendedor.FoneEmergencia}', {vendedor.QtdFilhosMenor14}, {vendedor.FilhoComDeficiencia}, '{vendedor.CTPS}')";
            }
            else
            {
                string formattedSalary = vendedor.Salario.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                int valeAlimentacao = vendedor.ValeAlimentacao ? 1 : 0;
                int valeTransporte = vendedor.ValeTransporte ? 1 : 0;
                int filhoComDeficiencia = vendedor.FilhoComDeficiencia ? 1 : 0;
                // query = $"UPDATE Vendedores SET Nome = '{vendedor.Nome}', Loja = '{vendedor.Loja}', Atende = {iAtende}, Nro = '{vendedor.Nro}', Usuario = '{vendedor.Usuario}', Senha = '{sCript}', Nivel = {vendedor.Nivel}, DataNascimento = '#{vendedor.DataNascimento:yyyy-MM-dd}#', DataAdmissao = '#{vendedor.DataAdmissao:yyyy-MM-dd}#', Salario = {vendedor.Salario.ToString(System.Globalization.CultureInfo.InvariantCulture)}, HorarioSemanaInicio = '#{vendedor.HorarioSemanaInicio:hh\\:mm}#', HorarioSemanaFim = '#{vendedor.HorarioSemanaFim:hh\\:mm}#', HorarioSabadoInicio = '#{vendedor.HorarioSabadoInicio:hh\\:mm}#', HorarioSabadoFim = '{vendedor.HorarioSabadoFim:hh\\:mm}', FormaPagamento = '{vendedor.FormaPagamento}', ValeAlimentacao = {valeAlimentacao}, ValeTransporte = {valeTransporte}, LinhaOnibus = '{vendedor.LinhaOnibus}', DataDemissao = '#{vendedor.DataDemissao:yyyy-MM-dd}#', MotivoDemissao = '{vendedor.MotivoDemissao}', RG = '{vendedor.RG}', CPF = '{vendedor.CPF}', Cargo = '{vendedor.Cargo}', FoneEmergencia = '{vendedor.FoneEmergencia}', QtdFilhosMenor14 = {vendedor.QtdFilhosMenor14}, FilhoComDeficiencia = {filhoComDeficiencia}, CTPS = '{vendedor.CTPS}' WHERE ID = {vendedor.Id}";
                // query = $"UPDATE Vendedores SET Nome = '{vendedor.Nome}', Loja = '{vendedor.Loja}', Atende = {iAtende}, Nro = '{vendedor.Nro}', Usuario = '{vendedor.Usuario}', Senha = '{sCript}', Nivel = {vendedor.Nivel}, DataNascimento = '#{vendedor.DataNascimento:yyyy-MM-dd}#', DataAdmissao = '#{vendedor.DataAdmissao:yyyy-MM-dd}#', Salario = {formattedSalary}, HorarioSemanaInicio = '#{vendedor.HorarioSemanaInicio:hh\\:mm}#', HorarioSemanaFim = '#{vendedor.HorarioSemanaFim:hh\\:mm}#', HorarioSabadoInicio = '#{vendedor.HorarioSabadoInicio:hh\\:mm}#', HorarioSabadoFim = '#{vendedor.HorarioSabadoFim:hh\\:mm}#', FormaPagamento = '{vendedor.FormaPagamento}', ValeAlimentacao = {valeAlimentacao}, ValeTransporte = {valeTransporte}, LinhaOnibus = '{vendedor.LinhaOnibus}', DataDemissao = '#{vendedor.DataDemissao:yyyy-MM-dd}#', MotivoDemissao = '{vendedor.MotivoDemissao}', RG = '{vendedor.RG}', CPF = '{vendedor.CPF}', Cargo = '{vendedor.Cargo}', FoneEmergencia = '{vendedor.FoneEmergencia}', QtdFilhosMenor14 = {vendedor.QtdFilhosMenor14}, FilhoComDeficiencia = {filhoComDeficiencia}, CTPS = '{vendedor.CTPS}' WHERE ID = {vendedor.Id}";

                // Criando a base do comando SQL
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append($"UPDATE Vendedores SET Nome = '{vendedor.Nome}', Loja = '{vendedor.Loja}', Atende = {iAtende}, Nro = '{vendedor.Nro}', Usuario = '{vendedor.Usuario}', Senha = '{sCript}', Nivel = {vendedor.Nivel}, ");

                // Adicionando campos condicionalmente
                if (vendedor.DataNascimento > new DateTime(0001, 01, 01))
                    sqlBuilder.Append($"DataNascimento = '#{vendedor.DataNascimento:yyyy-MM-dd}#', ");

                if (vendedor.DataAdmissao > new DateTime(0001, 01, 01))
                    sqlBuilder.Append($"DataAdmissao = '#{vendedor.DataAdmissao:yyyy-MM-dd}#', ");

                sqlBuilder.Append($"Salario = {vendedor.Salario.ToString(System.Globalization.CultureInfo.InvariantCulture)}, ");

                if (vendedor.HorarioSemanaInicio != TimeSpan.Zero)
                    sqlBuilder.Append($"HorarioSemanaInicio = '#{vendedor.HorarioSemanaInicio:hh\\:mm}#', ");

                if (vendedor.HorarioSemanaFim != TimeSpan.Zero)
                    sqlBuilder.Append($"HorarioSemanaFim = '#{vendedor.HorarioSemanaFim:hh\\:mm}#', ");

                if (vendedor.HorarioSabadoInicio != TimeSpan.Zero)
                    sqlBuilder.Append($"HorarioSabadoInicio = '#{vendedor.HorarioSabadoInicio:hh\\:mm}#', ");

                if (vendedor.HorarioSabadoFim != TimeSpan.Zero)
                    sqlBuilder.Append($"HorarioSabadoFim = '#{vendedor.HorarioSabadoFim:hh\\:mm}#', ");

                sqlBuilder.Append($"FormaPagamento = '{vendedor.FormaPagamento}', ValeAlimentacao = {vendedor.ValeAlimentacao}, ValeTransporte = {vendedor.ValeTransporte}, LinhaOnibus = '{vendedor.LinhaOnibus}', ");

                if (vendedor.DataDemissao > new DateTime(0001, 01, 01))
                    sqlBuilder.Append($"DataDemissao = '#{vendedor.DataDemissao:yyyy-MM-dd}#', ");

                sqlBuilder.Append($"MotivoDemissao = '{vendedor.MotivoDemissao}', RG = '{vendedor.RG}', CPF = '{vendedor.CPF}', Cargo = '{vendedor.Cargo}', FoneEmergencia = '{vendedor.FoneEmergencia}', QtdFilhosMenor14 = {vendedor.QtdFilhosMenor14}, FilhoComDeficiencia = {vendedor.FilhoComDeficiencia}, CTPS = '{vendedor.CTPS}' ");

                // Finalizando comando com WHERE
                sqlBuilder.Append($"WHERE ID = {vendedor.Id}");

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

        private DataTable ExecutarConsultaVendedorODBC(string query)
        {
            using (OdbcConnection connection = new OdbcConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand(query, connection))
                    {
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("ID", typeof(int));
                            dataTable.Columns.Add("Nome", typeof(string));
                            dataTable.Columns.Add("Loja", typeof(string));
                            dataTable.Columns.Add("Atende", typeof(string));
                            dataTable.Columns.Add("Nro", typeof(string));
                            dataTable.Columns.Add("Usuario", typeof(string));
                            dataTable.Columns.Add("Senha", typeof(string));
                            dataTable.Columns.Add("Nivel", typeof(int));
                            dataTable.Columns.Add("DataNascimento", typeof(DateTime));
                            dataTable.Columns.Add("DataAdmissao", typeof(DateTime));
                            dataTable.Columns.Add("Salario", typeof(decimal));
                            dataTable.Columns.Add("HorarioSemanaInicio", typeof(TimeSpan));
                            dataTable.Columns.Add("HorarioSemanaFim", typeof(TimeSpan));
                            dataTable.Columns.Add("HorarioSabadoInicio", typeof(TimeSpan));
                            dataTable.Columns.Add("HorarioSabadoFim", typeof(TimeSpan));
                            dataTable.Columns.Add("FormaPagamento", typeof(string));
                            dataTable.Columns.Add("ValeAlimentacao", typeof(bool));
                            dataTable.Columns.Add("ValeTransporte", typeof(bool));
                            dataTable.Columns.Add("LinhaOnibus", typeof(string));
                            dataTable.Columns.Add("DataDemissao", typeof(DateTime));
                            dataTable.Columns.Add("MotivoDemissao", typeof(string));
                            dataTable.Columns.Add("RG", typeof(string));
                            dataTable.Columns.Add("CPF", typeof(string));
                            dataTable.Columns.Add("Cargo", typeof(string));
                            dataTable.Columns.Add("FoneEmergencia", typeof(string));
                            dataTable.Columns.Add("QtdFilhosMenor14", typeof(int));
                            dataTable.Columns.Add("FilhoComDeficiencia", typeof(bool));
                            dataTable.Columns.Add("CTPS", typeof(string));

                            while (reader.Read())
                            {
                                DataRow row = dataTable.NewRow();
                                row["ID"] = reader.GetInt32(reader.GetOrdinal("ID"));
                                row["Nome"] = reader.GetString(reader.GetOrdinal("Nome"));
                                row["Loja"] = reader.GetString(reader.GetOrdinal("Loja"));
                                row["Atende"] = reader.GetString(reader.GetOrdinal("Atende"));
                                row["Nro"] = reader.GetString(reader.GetOrdinal("Nro"));
                                row["Usuario"] = reader.GetString(reader.GetOrdinal("Usuario"));
                                row["Senha"] = reader.GetString(reader.GetOrdinal("Senha"));
                                row["Nivel"] = reader.GetInt32(reader.GetOrdinal("Nivel"));
                                row["DataNascimento"] = reader.GetDateTime(reader.GetOrdinal("DataNascimento"));
                                row["DataAdmissao"] = reader.GetDateTime(reader.GetOrdinal("DataAdmissao"));
                                row["Salario"] = reader.GetDecimal(reader.GetOrdinal("Salario"));
                                row["HorarioSemanaInicio"] = TimeSpan.Parse(reader.GetString(reader.GetOrdinal("HorarioSemanaInicio")));
                                row["HorarioSemanaFim"] = TimeSpan.Parse(reader.GetString(reader.GetOrdinal("HorarioSemanaFim")));
                                row["HorarioSabadoInicio"] = TimeSpan.Parse(reader.GetString(reader.GetOrdinal("HorarioSabadoInicio")));
                                row["HorarioSabadoFim"] = TimeSpan.Parse(reader.GetString(reader.GetOrdinal("HorarioSabadoFim")));
                                row["FormaPagamento"] = reader.GetString(reader.GetOrdinal("FormaPagamento"));
                                row["ValeAlimentacao"] = reader.GetBoolean(reader.GetOrdinal("ValeAlimentacao"));
                                row["ValeTransporte"] = reader.GetBoolean(reader.GetOrdinal("ValeTransporte"));
                                row["LinhaOnibus"] = reader.GetString(reader.GetOrdinal("LinhaOnibus"));
                                row["DataDemissao"] = reader.GetDateTime(reader.GetOrdinal("DataDemissao"));
                                row["MotivoDemissao"] = reader.GetString(reader.GetOrdinal("MotivoDemissao"));
                                row["RG"] = reader.GetString(reader.GetOrdinal("RG"));
                                row["CPF"] = reader.GetString(reader.GetOrdinal("CPF"));
                                row["Cargo"] = reader.GetString(reader.GetOrdinal("Cargo"));
                                row["FoneEmergencia"] = reader.GetString(reader.GetOrdinal("FoneEmergencia"));
                                row["QtdFilhosMenor14"] = reader.GetInt32(reader.GetOrdinal("QtdFilhosMenor14"));
                                row["FilhoComDeficiencia"] = reader.GetBoolean(reader.GetOrdinal("FilhoComDeficiencia"));
                                row["CTPS"] = reader.GetString(reader.GetOrdinal("CTPS"));
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

        private DataTable ExecutarConsultaVendedorADO(string query)
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
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("ID", typeof(int));
                            dataTable.Columns.Add("Nome", typeof(string));
                            dataTable.Columns.Add("Loja", typeof(string));
                            dataTable.Columns.Add("Atende", typeof(string));
                            dataTable.Columns.Add("Nro", typeof(string));
                            dataTable.Columns.Add("Usuario", typeof(string));
                            dataTable.Columns.Add("Senha", typeof(string));
                            dataTable.Columns.Add("Nivel", typeof(int));
                            dataTable.Columns.Add("DataNascimento", typeof(DateTime));
                            dataTable.Columns.Add("DataAdmissao", typeof(DateTime));
                            dataTable.Columns.Add("Salario", typeof(decimal));
                            dataTable.Columns.Add("HorarioSemanaInicio", typeof(TimeSpan));
                            dataTable.Columns.Add("HorarioSemanaFim", typeof(TimeSpan));
                            dataTable.Columns.Add("HorarioSabadoInicio", typeof(TimeSpan));
                            dataTable.Columns.Add("HorarioSabadoFim", typeof(TimeSpan));
                            dataTable.Columns.Add("FormaPagamento", typeof(string));
                            dataTable.Columns.Add("ValeAlimentacao", typeof(bool));
                            dataTable.Columns.Add("ValeTransporte", typeof(bool));
                            dataTable.Columns.Add("LinhaOnibus", typeof(string));
                            dataTable.Columns.Add("DataDemissao", typeof(DateTime));
                            dataTable.Columns.Add("MotivoDemissao", typeof(string));
                            dataTable.Columns.Add("RG", typeof(string));
                            dataTable.Columns.Add("CPF", typeof(string));
                            dataTable.Columns.Add("Cargo", typeof(string));
                            dataTable.Columns.Add("FoneEmergencia", typeof(string));
                            dataTable.Columns.Add("QtdFilhosMenor14", typeof(int));
                            dataTable.Columns.Add("FilhoComDeficiencia", typeof(bool));
                            dataTable.Columns.Add("CTPS", typeof(string));

                            while (reader.Read())
                            {
                                DataRow row = dataTable.NewRow();
                                row["ID"] = reader["ID"];
                                row["Nome"] = reader["Nome"];
                                row["Loja"] = reader["Loja"];
                                row["Atende"] = reader["Atende"];
                                row["Nro"] = reader["Nro"];
                                row["Usuario"] = reader["Usuario"];
                                row["Senha"] = reader["Senha"];
                                row["Nivel"] = reader["Nivel"];
                                row["DataNascimento"] = reader["DataNascimento"];
                                row["DataAdmissao"] = reader["DataAdmissao"];
                                row["Salario"] = reader["Salario"];

                                row["HorarioSemanaInicio"] = ParseNullableTimeSpan(reader, "HorarioSemanaInicio");
                                row["HorarioSemanaFim"] = ParseNullableTimeSpan(reader, "HorarioSemanaFim");
                                row["HorarioSabadoInicio"] = ParseNullableTimeSpan(reader, "HorarioSabadoInicio");
                                row["HorarioSabadoFim"] = ParseNullableTimeSpan(reader, "HorarioSabadoFim");

                                // Verifica se o valor é DBNull antes de tentar converter para TimeSpan
                                //if (!reader.IsDBNull(reader.GetOrdinal("HorarioSemanaInicio")))
                                //{
                                //    row["HorarioSemanaInicio"] = TimeSpan.Parse(reader["HorarioSemanaInicio"].ToString());
                                //}
                                //else
                                //{
                                //    // Trata o valor nulo como necessário, por exemplo, definindo um valor padrão ou deixando-o vazio
                                //    row["HorarioSemanaInicio"] = default(TimeSpan); // Valor padrão para TimeSpan é Zero
                                //}

                                //// Repete o processo para outros campos de TimeSpan
                                //if (!reader.IsDBNull(reader.GetOrdinal("HorarioSemanaFim")))
                                //{
                                //    row["HorarioSemanaFim"] = TimeSpan.Parse(reader["HorarioSemanaFim"].ToString());
                                //}
                                //else
                                //{
                                //    row["HorarioSemanaFim"] = default(TimeSpan);
                                //}

                                //if (!reader.IsDBNull(reader.GetOrdinal("HorarioSabadoInicio")))
                                //{
                                //    row["HorarioSabadoInicio"] = TimeSpan.Parse(reader["HorarioSabadoInicio"].ToString());
                                //}
                                //else
                                //{
                                //    row["HorarioSabadoInicio"] = default(TimeSpan);
                                //}

                                //if (!reader.IsDBNull(reader.GetOrdinal("HorarioSabadoFim")))
                                //{
                                //    row["HorarioSabadoFim"] = TimeSpan.Parse(reader["HorarioSabadoFim"].ToString());
                                //}
                                //else
                                //{
                                //    row["HorarioSabadoFim"] = default(TimeSpan);
                                //}

                                // Continua preenchendo outras colunas...
                                row["FormaPagamento"] = reader["FormaPagamento"];
                                row["ValeAlimentacao"] = reader["ValeAlimentacao"];
                                row["ValeTransporte"] = reader["ValeTransporte"];
                                row["LinhaOnibus"] = reader["LinhaOnibus"];
                                row["DataDemissao"] = reader["DataDemissao"];
                                row["MotivoDemissao"] = reader["MotivoDemissao"];
                                row["RG"] = reader["RG"];
                                row["CPF"] = reader["CPF"];
                                row["Cargo"] = reader["Cargo"];
                                row["FoneEmergencia"] = reader["FoneEmergencia"];
                                row["QtdFilhosMenor14"] = reader["QtdFilhosMenor14"];
                                row["FilhoComDeficiencia"] = reader["FilhoComDeficiencia"];
                                row["CTPS"] = reader["CTPS"];

                                dataTable.Rows.Add(row);
                            }
                            return dataTable;
                            //while (reader.Read())
                            //{
                            //    DataRow row = dataTable.NewRow();
                            //    row["ID"] = reader["ID"];
                            //    row["Nome"] = reader["Nome"];
                            //    row["Loja"] = reader["Loja"];
                            //    row["Atende"] = reader["Atende"];
                            //    row["Nro"] = reader["Nro"];
                            //    row["Usuario"] = reader["Usuario"];
                            //    row["Senha"] = reader["Senha"];
                            //    row["Nivel"] = reader["Nivel"];
                            //    row["DataNascimento"] = reader["DataNascimento"];
                            //    row["DataAdmissao"] = reader["DataAdmissao"];
                            //    row["Salario"] = reader["Salario"];
                            //    row["HorarioSemanaInicio"] = TimeSpan.Parse(reader["HorarioSemanaInicio"].ToString());
                            //    row["HorarioSemanaFim"] = TimeSpan.Parse(reader["HorarioSemanaFim"].ToString());
                            //    row["HorarioSabadoInicio"] = TimeSpan.Parse(reader["HorarioSabadoInicio"].ToString());
                            //    row["HorarioSabadoFim"] = TimeSpan.Parse(reader["HorarioSabadoFim"].ToString());
                            //    row["FormaPagamento"] = reader["FormaPagamento"];
                            //    row["ValeAlimentacao"] = reader["ValeAlimentacao"];
                            //    row["ValeTransporte"] = reader["ValeTransporte"];
                            //    row["LinhaOnibus"] = reader["LinhaOnibus"];
                            //    row["DataDemissao"] = reader["DataDemissao"];
                            //    row["MotivoDemissao"] = reader["MotivoDemissao"];
                            //    row["RG"] = reader["RG"];
                            //    row["CPF"] = reader["CPF"];
                            //    row["Cargo"] = reader["Cargo"];
                            //    row["FoneEmergencia"] = reader["FoneEmergencia"];
                            //    row["QtdFilhosMenor14"] = reader["QtdFilhosMenor14"];
                            //    row["FilhoComDeficiencia"] = reader["FilhoComDeficiencia"];
                            //    row["CTPS"] = reader["CTPS"];
                            //    dataTable.Rows.Add(row);
                            //}

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

        private TimeSpan ParseNullableTimeSpan(OleDbDataReader reader, string columnName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return TimeSpan.Parse(reader[columnName].ToString());
            }
            else
            {
                // Retorna o valor padrão para TimeSpan, que é Zero
                return default(TimeSpan);
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
                CTPS = CTPS
            };
        }

        public DataTable getBalconistas()
        {
            glo.Loga("Dentro de getBalconistas");
            string query = $"SELECT * FROM Vendedores Where Nro > '0' Order By Nome ";
            return ExecutarConsultaVendedor(query);
        }

        public override object GetUltimo()
        {
            string query = "SELECT TOP 1 * FROM Vendedores ORDER BY ID Desc";
            return ExecutarConsultaVendedor(query); ;
        }

        //public override void Grava(object obj)
        //{
        //    VendedoresDAO vendedor = (VendedoresDAO)obj;
        //    string query;
        //    List<OleDbParameter> parameters=null;
        //    int iAtende = vendedor.Atende ? 1 : 0;
        //    string sCript = "";
        //    if (vendedor.Senha.Length>0)
        //    {
        //        sCript = Cripto.Encrypt(vendedor.Senha);
        //    }
        //    if (vendedor.Adicao)
        //    {
        //        query = $"INSERT INTO Vendedores (Nome, Loja, Atende, Nro, Usuario, Senha, Nivel) " +
        //            $"VALUES ('{vendedor.Nome}', '{vendedor.Loja}', {iAtende}, '{vendedor.Nro}','{vendedor.Usuario}' ,'{sCript}' ,{vendedor.Nivel} )";
        //    }
        //    else
        //    {                
        //        query = $"UPDATE Vendedores SET Nome = '{vendedor.Nome}', Loja = '{vendedor.Loja}', Atende = {iAtende}, Nro = '{vendedor.Nro}', Usuario = '{vendedor.Usuario}', Senha = '{sCript}', Nivel = {vendedor.Nivel} WHERE ID = {vendedor.Id} ";
        //    }
        //    try
        //    {
        //        DB.ExecutarComandoSQL(query, parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        string x = ex.ToString();
        //        MessageBox.Show(x, "Erro na operação do banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

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

        //private DataTable ExecutarConsultaVendedorODBC(string query)
        //{            
        //    using (OdbcConnection connection = new OdbcConnection(glo.connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            using (OdbcCommand command = new OdbcCommand(query, connection))
        //            {
        //                using (OdbcDataReader reader = command.ExecuteReader())
        //                {
        //                    DataTable dataTable = new DataTable();
        //                    dataTable.Columns.Add("ID", typeof(int));
        //                    dataTable.Columns.Add("Nome", typeof(string));
        //                    dataTable.Columns.Add("Loja", typeof(string));
        //                    dataTable.Columns.Add("Atende", typeof(string));
        //                    dataTable.Columns.Add("Nro", typeof(string));
        //                    dataTable.Columns.Add("Usuario", typeof(string));
        //                    dataTable.Columns.Add("Senha", typeof(string));
        //                    dataTable.Columns.Add("Nivel", typeof(int));

        //                    while (reader.Read())
        //                    {
        //                        DataRow row = dataTable.NewRow();
        //                        row["ID"] = reader.GetInt32(reader.GetOrdinal("ID"));
        //                        row["Nome"] = reader.GetString(reader.GetOrdinal("Nome"));
        //                        row["Loja"] = reader.GetString(reader.GetOrdinal("Loja"));
        //                        row["Atende"] = reader.GetString(reader.GetOrdinal("Atende"));
        //                        row["Nro"] = reader.GetString(reader.GetOrdinal("Nro"));
        //                        row["Usuario"] = reader.GetString(reader.GetOrdinal("Usuario"));
        //                        row["Senha"] = reader.GetString(reader.GetOrdinal("Senha"));
        //                        row["Nivel"] = reader.GetInt32(reader.GetOrdinal("Nivel"));
        //                        dataTable.Rows.Add(row);
        //                    }
        //                    return dataTable;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //            return null;
        //        }
        //    }
        //}

        //private DataTable ExecutarConsultaVendedorADO(string query)
        //{
        //    using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            using (OleDbCommand command = new OleDbCommand(query, connection))
        //            {
        //                using (OleDbDataReader reader = command.ExecuteReader())
        //                {
        //                    DataTable dataTable = new DataTable();
        //                    dataTable.Columns.Add("ID", typeof(int));
        //                    dataTable.Columns.Add("Nome", typeof(string));
        //                    dataTable.Columns.Add("Loja", typeof(string));
        //                    dataTable.Columns.Add("Atende", typeof(string));
        //                    dataTable.Columns.Add("Nro", typeof(string));
        //                    dataTable.Columns.Add("Usuario", typeof(string));
        //                    dataTable.Columns.Add("Senha", typeof(string));
        //                    dataTable.Columns.Add("Nivel", typeof(int));

        //                    while (reader.Read())
        //                    {
        //                        DataRow row = dataTable.NewRow();
        //                        row["ID"] = reader["ID"];
        //                        row["Nome"] = reader["Nome"];
        //                        row["Loja"] = reader["Loja"];
        //                        row["Atende"] = reader["Atende"];
        //                        row["Nro"] = reader["Nro"];
        //                        row["Usuario"] = reader["Usuario"];
        //                        row["Senha"] = reader["Senha"];
        //                        row["Nivel"] = reader["Nivel"];
        //                        dataTable.Rows.Add(row);
        //                    }
        //                    return dataTable;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //            return null;
        //        }
        //    }
        //}

        //public override tb.IDataEntity GetEsse()
        //{
        //    return (tb.Vendedor)new tb.Vendedor
        //    {
        //        Id = Id,
        //        Nome = Nome,
        //        Loja = Loja,
        //        Atende = Atende,
        //        Nro = Nro,

        //        Usuario = Usuario,
        //        Senha = Senha,
        //        Nivel = Nivel
        //    };
        //}

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

        //private tb.Vendedor ExecutarConsultaVendedor2(string query)
        //{            
        //    using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            using (OleDbCommand command = new OleDbCommand(query, connection))
        //            {
        //                using (OleDbDataReader reader = command.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        Nome = reader["Nome"].ToString();
        //                        Id = Convert.ToInt32(reader["ID"]);
        //                        Loja = reader["Loja"].ToString();
        //                        object oAt = reader["Atende"];
        //                        if (oAt == DBNull.Value)
        //                        {
        //                            Atende = false;
        //                        } else
        //                        {
        //                            int iAt = Convert.ToInt32(oAt);
        //                            Atende = !(iAt == 0);
        //                        }     
        //                        Nro = reader["Nro"].ToString();

        //                        Usuario = reader["Usuario"].ToString();
        //                        Senha = reader["Senha"].ToString();

        //                        object oNivel = reader["Nivel"];
        //                        if (oNivel == DBNull.Value)
        //                        {
        //                            Nivel = 0;
        //                        } else
        //                        {
        //                            Nivel = Convert.ToInt32(oNivel);
        //                        }                                    

        //                        return (tb.Vendedor)GetEsse();
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {                    
        //            return null;
        //        }
        //    }
        //    return null;
        //}

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
