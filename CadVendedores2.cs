using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class CadVendedores2 : TeleBonifacio.FormBase
    {
        private bool Carregando = true;
        private int ID = 0;
        private bool Adicionando = false;
        private string Nome = "";
        private INI cINI;

        #region Inicializaçao        

        public CadVendedores2()
        {
            InitializeComponent();
            base.DAO = new dao.VendedoresDAO();
            base.reg = getUlt();
            ID = base.reg.Id;
            Nome = base.reg.Nome;
            base.Mostra();
            base.LerTagsDosCamposDeTexto();
            List<string> lista = new List<string>();
            lista.Add("Balconísta");
            lista.Add("Caixa");
            lista.Add("Escritório");
            base.setListCombo(lista);
            rt.AdjustFormComponents(this);
            cINI = new INI();
            VerificaHorarios(base.reg);
            cntrole1.MostraImpressao();
            Carregando = false;
        }

        private void CadVendedores2_Activated(object sender, EventArgs e)
        {
            if (glo.IdAdicionado == -1)
            {
                Adicionando = true;
            }
        }

        #endregion

        #region Metodos de Base de Dados        

        private tb.Vendedor getUlt()
        {
            string query = "SELECT TOP 1 * FROM Vendedores ORDER BY ID Desc";
            return ExecutarConsulta(query); 
        }

        private Vendedor ExecutarConsulta(string query)
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
                        Vendedor ret = new Vendedor();
                        if (reader.Read())
                        {
                            ret.Id = (int)reader["ID"];
                            ret.Nome = reader["Nome"].ToString();
                            ret.Loja = reader["Loja"].ToString();
                            ret.Atende = reader["Atende"] != DBNull.Value && (int)reader["Atende"] == 1;
                            ret.Nro = reader["Nro"].ToString();
                            ret.Usuario = reader["Usuario"].ToString();
                            ret.Senha = reader["Senha"].ToString();
                            ret.Nivel = reader["Nivel"] != DBNull.Value ? (int)reader["Nivel"] : 0;
                            ret.DataNascimento = reader["DataNascimento"] != DBNull.Value ? (DateTime)reader["DataNascimento"] : DateTime.MinValue;
                            ret.DataAdmissao = reader["DataAdmissao"] != DBNull.Value ? (DateTime)reader["DataAdmissao"] : DateTime.MinValue;
                            ret.Salario = reader["Salario"] != DBNull.Value ? (decimal)reader["Salario"] : 0;
                            ret.HorarioSemanaInicio = ParseTimeFromDateTime(reader, "HorarioSemanaInicio");
                            ret.HorarioSemanaFim = ParseTimeFromDateTime(reader, "HorarioSemanaFim");
                            ret.HorarioSabadoInicio = ParseTimeFromDateTime(reader, "HorarioSabadoInicio");
                            ret.HorarioSabadoFim = ParseTimeFromDateTime(reader, "HorarioSabadoFim");
                            ret.FormaPagamento = reader["FormaPagamento"].ToString();
                            ret.ValeAlimentacao = reader["ValeAlimentacao"] != DBNull.Value && (int)reader["ValeAlimentacao"] == 1;
                            ret.ValeTransporte = reader["ValeTransporte"] != DBNull.Value && (int)reader["ValeTransporte"] == 1;
                            ret.LinhaOnibus = reader["LinhaOnibus"].ToString();
                            ret.DataDemissao = reader["DataDemissao"] != DBNull.Value ? (DateTime)reader["DataDemissao"] : DateTime.MinValue;
                            ret.MotivoDemissao = reader["MotivoDemissao"].ToString();
                            ret.RG = reader["RG"].ToString();
                            ret.CPF = reader["CPF"].ToString();
                            ret.Cargo = reader["Cargo"].ToString();
                            ret.FoneEmergencia = reader["FoneEmergencia"].ToString();
                            ret.QtdFilhosMenor14 = reader["QtdFilhosMenor14"] != DBNull.Value ? (int)reader["QtdFilhosMenor14"] : 0;
                            ret.FilhoComDeficiencia = reader["FilhoComDeficiencia"] != DBNull.Value && (int)reader["FilhoComDeficiencia"] == 1;
                            ret.CTPS = reader["CTPS"].ToString();
                            ret.Fone = reader["Fone"].ToString();
                            ret.Amigo = reader["Amigo"].ToString();
                        }
                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during database operation: " + ex.Message);
                throw;
            }
        }
    }

        private void ConfigurarDAO()
        {
            if (!Adicionando)
            {
                base.DAO.SetId(ID);
                base.DAO.SetNome(Nome);
            }
        }

        #endregion

        #region Funções auxiliares        

        private TimeSpan ParseTimeFromDateTime(OleDbDataReader reader, string columnName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                DateTime tempDate = (DateTime)reader[columnName];
                return tempDate.TimeOfDay;
            }
            return TimeSpan.Zero;
        }

        private void VerificaHorarios(tb.IDataEntity modelo)
        {
            AjustarDateTimePicker(dtpHorarioSemanaInicio, "Turnos", "ManIni", "08:00", modelo);
            AjustarDateTimePicker(dtpHorarioSemanaFim, "Turnos", "ManFim", "12:00", modelo);
            AjustarDateTimePicker(dtpHorarioSabadoInicio, "Turnos", "TarIni", "13:00", modelo);
            AjustarDateTimePicker(dtpHorarioSabadoFim, "Turnos", "TarFim", "18:30", modelo);

            AjustarDateTimePicker(dtpHSSaiMan, "Turnos", "HSSaiMan", "12:00", modelo);
            AjustarDateTimePicker(dtpHSIniTrd, "Turnos", "HSIniTrd", "13:00", modelo);
            AjustarDateTimePicker(dtpHFSaiMan, "Turnos", "HFSaiMan", "12:00", modelo);
            AjustarDateTimePicker(dtpHFIniTrd, "Turnos", "HFIniTrd", "13:00", modelo);

        }

        private void AjustarDateTimePicker(DateTimePicker dtp, string section, string key, string defaultValue, object modelo)
        {
            string fieldName = dtp.Name.Substring(3);
            PropertyInfo propertyInfo = modelo.GetType().GetProperty(fieldName);
            if (propertyInfo != null)
            {
                object fieldValue = propertyInfo.GetValue(modelo);
                if (fieldValue == null || (fieldValue is DateTime dateTimeValue && dateTimeValue.TimeOfDay == TimeSpan.Zero))
                {
                    TimeSpan time = GetIniTime(section, key, defaultValue);
                    dtp.Value = DateTime.Today + time;
                }
                else if (fieldValue is TimeSpan timeSpanValue && timeSpanValue == TimeSpan.Zero)
                {
                    TimeSpan time = GetIniTime(section, key, defaultValue);
                    dtp.Value = DateTime.Today + time;
                }
                else
                {
                    dtp.Value = DateTime.Today + (TimeSpan)fieldValue;
                }

                // Configurações comuns para todos os casos
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.ShowUpDown = true;
                dtp.CustomFormat = "HH:mm";
            }
        }

        private TimeSpan GetIniTime(string section, string key, string defaultValue)
        {
            string timeStr = cINI.ReadString(section, key, defaultValue);
            return TimeSpan.TryParse(timeStr, out TimeSpan result) ? result : TimeSpan.Parse(defaultValue);
        }

        private bool ValidarHorariosECPF()
        {
            if (!SaoHorariosValidos())
            {
                ExibirMensagemErro("Os horários de trabalho são inválidos. Por favor, verifique e corrija.");
                Carregando = false;
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtCPF.Text) && !IsCPFValido(txtCPF.Text))
            {
                ExibirMensagemErro("O CPF informado é inválido. Por favor, verifique e corrija.");
                Carregando = false;
                return false;
            }
            return true;
        }

        private void ExibirMensagemErro(string mensagem)
        {
            MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool IsCPFValido(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11)
                return false;
            if (cpf.Distinct().Count() == 1)
                return false;
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;
            if (int.Parse(cpf[9].ToString()) != digito1)
                return false;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;
            return int.Parse(cpf[10].ToString()) == digito2;
        }

        private bool SaoHorariosValidos()
        {
            TimeSpan semanaInicio = dtpHorarioSemanaInicio.Value.TimeOfDay;
            TimeSpan semanaFim = dtpHorarioSemanaFim.Value.TimeOfDay;
            TimeSpan sabadoInicio = dtpHorarioSabadoInicio.Value.TimeOfDay;
            TimeSpan sabadoFim = dtpHorarioSabadoFim.Value.TimeOfDay;
            if (semanaFim <= semanaInicio || sabadoFim <= sabadoInicio)
            {
                return false;
            }
            return true;
        }

        #endregion

        private void cntrole1_AcaoRealizada(object sender, AcaoEventArgs e)
        {
            // Refatorado em 02/08/24
            Carregando = true;
            if (e.Acao == "Adicionar" || e.Acao == "OK")
            {
                if (!ValidarHorariosECPF()) return;
                if (e.Acao == "Adicionar") Adicionando = true;
            }
            else
            {
                ConfigurarDAO();
            }
            base.cntrole1_AcaoRealizada(sender, e, base.reg);
            if (base.reg != null)
            {
                ID = base.reg.Id;
                Nome = base.reg.Nome;
                if ((e.Acao == "ParaFrente") || (e.Acao == "ParaTras"))
                {
                    VerificaHorarios(base.reg);
                }
            }
            Carregando = false;
        }        
        
        #region Eventos


        private void CadVendedor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && !(e.Control && e.KeyCode == Keys.C) && !(e.Control && e.KeyCode == Keys.A))
            {
                if (e.KeyCode == Keys.Escape)
                {
                    base.Cancela();
                }
                else
                {
                    if (!base.Pesquisando)
                    {
                        base.cntrole1.EmEdicao = true;
                    }
                }

            }
        }

        private void chkAtende_CheckStateChanged(object sender, EventArgs e)
        {
            if (Carregando==false)
            {
                base.cntrole1.EmEdicao = true;
            }            
        }

        private void cnbNivel_Click(object sender, EventArgs e)
        {
            base.cntrole1.EmEdicao = true;
        }

        private void dtpDataAdmissao_ValueChanged(object sender, EventArgs e)
        {
            if (!Carregando)
            {
                DateTimePicker dtp = sender as DateTimePicker;
                if (dtp.Value != DateTime.MinValue)
                {
                    dtp.Format = DateTimePickerFormat.Short;
                    base.cntrole1.EmEdicao = true;
                }
                else if (dtp.Value == DateTime.MinValue)
                {
                    dtp.CustomFormat = " ";
                }
            }
        }

        private void dtpDataNascimento_ValueChanged(object sender, EventArgs e)
        {
            if (!Carregando)
            {
                DateTimePicker dtp = sender as DateTimePicker;
                if (dtp.Value != DateTime.MinValue)
                {
                    dtp.Format = DateTimePickerFormat.Short;
                    base.cntrole1.EmEdicao = true;
                }
                else if (dtp.Value == DateTime.MinValue)
                {
                    dtp.CustomFormat = " ";
                }
            }
        }

        private void dtpDataAdmissao_Enter(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            if (dtp.Format == DateTimePickerFormat.Custom)
            {
                dtp.Value = DateTime.Today;  // Ajusta para a data atual quando o controle é ativado.
                dtp.Format = DateTimePickerFormat.Short; // Muda para o formato curto para mostrar a data.
            }
        }

        private void dtpDataDemissao_Enter(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            if (dtp.Format == DateTimePickerFormat.Custom)
            {
                dtp.Value = DateTime.Today;
                dtp.Format = DateTimePickerFormat.Short;
            }
        }

        private void dtpHorarioSemanaInicio_ValueChanged(object sender, EventArgs e)
        {
            if (!Carregando)
            {
                base.cntrole1.EmEdicao = true;
            }
        }

        private void txtFilhoComDeficiencia_CheckedChanged(object sender, EventArgs e)
        {
            if (!Carregando)
            {
                base.cntrole1.EmEdicao = true;
            }
        }

        #endregion

        private void cntrole1_SolicitacaoImpressao(object sender, EventArgs e)
        {
            base.Imprimir();
        }

    }
}
