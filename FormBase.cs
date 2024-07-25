using TeleBonifacio.tb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.gen;

namespace TeleBonifacio
{
    public partial class FormBase : Form
    {

        protected int Direcao = 0;
        protected bool EmAdicao = false;
        protected bool Mostrando = false;
        protected bool Pesquisando = false;
        protected dao.BaseDAO _dao;
        public dao.BaseDAO DAO
        {
            get { return _dao; }
            set
            {
                _dao = value;
            }
        }
        protected tb.IDataEntity reg;
        private List<CampoTagInfo> tagsDosCampos;
        private System.Windows.Forms.DataGrid dataGrid;
        private bool GridCarregada = false;
        private bool AdicaoPorfora = false;
        private List<string> listCombo;

        public FormBase()
        {
            InitializeComponent();
            tagsDosCampos = new List<tb.CampoTagInfo>();
        }

        public void setListCombo(List<string> lista)
        {
            listCombo = lista;
        }

        private void InitializeDataGrid()
        {
            DataTable dataTable = DAO.CarregarDados();
        }

        #region Campos

        protected bool Mostra()
        {
            if (reg == null)
            {
                cntrole1.Vazio = true;
                return false;
            }
            else
            {
                if (reg.Id==0)
                {
                    cntrole1.Vazio = true;
                    return false;
                } else
                {
                    cntrole1.TemDados = true;
                    Pesquisando = false;
                    Mostrando = true;
                    foreach (Control control in this.Controls)
                    {
                        if (control is TextBox textBox)
                        {
                            ProcessarTextBox(textBox);
                        }
                        else if (control is DateTimePicker dateTimePicker)
                        {
                            ProcessarDateTimePicker(dateTimePicker);
                        } else if (control is CheckBox Check)
                        {
                            ProcessaCheck(Check);
                        } else if (control is ComboBox cmb)
                        {
                            ProcessaCombo(cmb);
                        }
                    }
                    Mostrando = false;
                    cntrole1.IDAtual = reg.Id;
                    return true;
                }
            }
        }

        private void ProcessaCombo(ComboBox cmb)
        {
            string propertyName = cmb.Name.Substring(3);
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                cmb.SelectedIndex = 0;
            } else
            {
                string valor = propertyInfo.GetValue(reg, null)?.ToString() ?? string.Empty;
                int iVlr = Convert.ToInt16(valor);
                cmb.SelectedIndex = iVlr;
            }
        }

        private void ProcessaCheck(CheckBox check)
        {
            string propertyName = check.Name.Substring(3); 
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                string valor = propertyInfo.GetValue(reg, null)?.ToString() ?? string.Empty;
                check.Checked = (valor == "True");
            }
        }

        protected void ProcessarTextBox(TextBox textBox)
        {
            string propertyName = textBox.Name.Substring(3); 
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                string valor = propertyInfo.GetValue(reg, null)?.ToString() ?? string.Empty;
                if (textBox.Name== "txtSenha")
                {
                    valor = Cripto.Decrypt(valor);
                }
                textBox.Text = valor;
            }
        }

        public void Adicionar()
        {
            AdicaoPorfora = true;
            LimparCampos();
            EmAdicao = true;
            cntrole1.MostraEmEstadodeEdicao();
        }

        private void ProcessarDateTimePicker(DateTimePicker dtpControl)
        {
            string propertyName = dtpControl.Name.Substring(3);
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
            {
                DateTime? data = propertyInfo.GetValue(reg, null) as DateTime?;
                if (!data.HasValue || data.Value == DateTime.MinValue)
                {
                    dtpControl.CustomFormat = " "; 
                    dtpControl.Format = DateTimePickerFormat.Custom;
                }
                else
                {
                    dtpControl.Value = data.Value;
                    dtpControl.Format = DateTimePickerFormat.Short;
                }
            }
        }


        private void MapearCamposParaModelo(dao.BaseDAO reg)
        {
            foreach (Control control in this.Controls)
            {
                try
                {
                    if (control is TextBox textBox)
                    {
                        MapearTextBoxParaModelo(textBox, reg);
                    }
                    else if (control is DateTimePicker dateTimePicker)
                    {
                        MapearDateTimePickerParaModelo(dateTimePicker, reg);
                    } else if (control is CheckBox Check)
                    {
                        MapearCheckParaModelo(Check, reg);
                    }
                    else if (control is ComboBox cmb)
                    {
                        MapearComboParaModelo(cmb, reg);
                    }

                }
                catch (Exception ex)
                {
                    string x = ex.ToString();
                    // Tratamento adequado de exceções
                }
            }
        }

        private void MapearComboParaModelo(ComboBox cmb, BaseDAO reg)
        {
            string propertyName = cmb.Name.Substring(3);
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                propertyInfo.SetValue(reg, null, null);
            }
            else
            {
                int c = 0;
                foreach (string item in listCombo)
                {
                    if (item == cmb.Text)
                    {                        
                        break;
                    } else
                    {
                        c++;
                    }
                }
                propertyInfo.SetValue(reg, c, null);
            }
        }

        private void MapearCheckParaModelo(CheckBox check, BaseDAO reg)
        {
            string propertyName = check.Name.Substring(3); 
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                propertyInfo.SetValue(reg, null, null);
            }
            else
            {
                // Verifica se o CheckBox está marcado e define o valor da propriedade correspondente
                propertyInfo.SetValue(reg, check.Checked, null);
            }
        }

        private void MapearTextBoxParaModelo(TextBox textBox, dao.BaseDAO reg)
        {
            string propertyName = textBox.Name.Substring(3);
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);

            if (propertyInfo != null)
            {
                if (propertyInfo.PropertyType == typeof(int))
                {
                    if (int.TryParse(textBox.Text, out int value))
                    {
                        propertyInfo.SetValue(reg, value, null);
                    }
                    else
                    {
                        Console.WriteLine($"Falha na conversão para inteiro: {textBox.Text}");
                    }
                }
                else if (propertyInfo.PropertyType == typeof(decimal))
                {
                    if (decimal.TryParse(textBox.Text, out decimal value))
                    {
                        propertyInfo.SetValue(reg, value, null);
                    }
                    else
                    {
                        Console.WriteLine($"Falha na conversão para decimal: {textBox.Text}");
                    }
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(reg, textBox.Text, null);
                }
                else if (propertyInfo.PropertyType == typeof(TimeSpan))
                {
                    if (TimeSpan.TryParse(textBox.Text, out TimeSpan timeSpanValue))
                    {
                        propertyInfo.SetValue(reg, timeSpanValue, null);
                    }
                    else
                    {
                        Console.WriteLine($"Falha na conversão para TimeSpan: {textBox.Text}");
                    }
                }
                else
                {
                    Console.WriteLine("Tipo de propriedade não suportado");
                }
            }
            else
            {
                Console.WriteLine($"Propriedade '{propertyName}' não encontrada");
            }
        }

        private void MapearDateTimePickerParaModelo(DateTimePicker dtpControl, dao.BaseDAO reg)
        {
            string propertyName = dtpControl.Name.Substring(3); // Remove o prefixo 'dtp'
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);
            if (propertyInfo != null && (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?)))
            {
                if (dtpControl.Format != DateTimePickerFormat.Custom)
                {
                    propertyInfo.SetValue(reg, dtpControl.Value, null);
                }
                else
                {
                    propertyInfo.SetValue(reg, null, null); // ou DateTime.MinValue
                }
            }
        }

        #endregion

        #region TratamentoDeTela
        protected void LimparCampos()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = string.Empty;
                }
            }
        }

        public void ResetarAparenciaControles()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.BackColor = SystemColors.Window; // Cor normal
                }
            }
        }

        protected void cntrole1_AcaoRealizada(object sender, AcaoEventArgs e, tb.IDataEntity entidade)
        {
            switch (e.Acao)
            {
                case "Adicionar":
                    LimparCampos();
                    EmAdicao = true;
                    break;
                case "Delete":
                    reg = DAO.Apagar(Direcao, entidade);
                    if (!Mostra())
                    {
                        if (Direcao == 1)
                        {
                            cntrole1.Ultimo = true;
                        }
                        else
                        {
                            cntrole1.Primeiro = true;
                        }
                    }
                    break;
                case "ParaTras":
                    Direcao = -1;
                    reg = DAO.ParaTraz();
                    if (!Mostra())
                    {
                        cntrole1.Ultimo = true;
                    }
                    break;
                case "ParaFrente":
                    Direcao = 1; ;
                    reg = DAO.ParaFrente();
                    
                    if (!Mostra())
                    {
                        cntrole1.Primeiro = true;
                    }
                    break;
                case "Editar":
                    // this.Text = "clicou";
                    break;
                case "CANC":
                    Cancela();
                    break;
                case "OK":
                    Grava();
                    break;
                case "PesqON":
                    LigaGrid();
                    break;
                case "PesqAcionar":
                    PesqAcionar();
                    break;
                case "PesqOFF":
                    PesqOFF();
                    break;
                case "Pesquisar":
                    Pesquisar();
                    break;
            }
        }

        private void PesqOFF()
        {
            AlterarVisibilidadeControles(true);
            dataGrid.Visible = false;            
            Pesquisando = false;
        }

        private void Pesquisar()
        {
            string Pesquisar = cntrole1.Pesquisa;
            if (Pesquisar.Length > 1)
            {
                System.Data.DataTable Dados = DAO.Fitrar(Pesquisar);
                dataGrid.DataSource = Dados;
            }
        }

        private System.Data.DataTable Fitrar(string pesquisar)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Crud
        protected void Grava()
        {
            MapearCamposParaModelo(DAO);
            List<string> criticas = FazerCriticas(DAO);
            bool ok = false;
            string mensagemCritica = "";
            if (criticas.Count == 0)
            {
                DAO.Adicao = EmAdicao;
                string mensJaTem = "";
                if (EmAdicao)
                {
                    //DAO.SetId(0);
                    mensJaTem = DAO.VeSeJaTem(DAO);                    
                } else
                {
                    DAO.SetId(cntrole1.IDAtual);
                }                
                if (mensJaTem.Length>0)
                {
                    mensagemCritica = mensJaTem;
                } else
                {
                    if (cntrole1.Vazio)
                    {
                        DAO.Adicao = true;
                    }
                    DAO.Grava(DAO);
                    EmAdicao = false;
                    cntrole1.ModoNormal();
                    cntrole1.Vazio = false;
                    ok = true;
                    if (AdicaoPorfora)
                    {
                        this.Close();
                    }
                }
            } else
            {
                mensagemCritica = string.Join("\n", criticas);
            }
            if (!ok)
            {                
                MessageBox.Show(mensagemCritica, "Críticas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Criticas

        public List<string> FazerCriticas<T>(T objeto) where T : class
        {
            List<string> criticas = new List<string>();
            PropertyInfo[] propriedades = objeto.GetType().GetProperties();
            foreach (CampoTagInfo campoTag in tagsDosCampos)
            {
                PropertyInfo propriedade = propriedades.FirstOrDefault(p => p.Name.Equals(campoTag.Nome, StringComparison.OrdinalIgnoreCase));
                if (propriedade != null)
                {
                    if (campoTag.Tag == "O")
                    {
                        object valor = propriedade.GetValue(objeto);
                        if (valor == null || (valor is string && string.IsNullOrEmpty((string)valor)))
                        {
                            criticas.Add($"O campo {propriedade.Name} é obrigatório.");
                        }
                    }
                    else if (campoTag.Tag == "H" && propriedade.PropertyType == typeof(DateTime))
                    {
                        DateTime dataValor = (DateTime)propriedade.GetValue(objeto);
                        if (dataValor > DateTime.Today)
                        {
                            criticas.Add($"A data no campo {propriedade.Name} não pode ser posterior a hoje.");
                        }
                    }
                }
            }
            MarcarControlesComErro(criticas);
            return criticas;
        }

        public void LerTagsDosCamposDeTexto()
        {
            tagsDosCampos.Clear();
            foreach (Control control in Controls)
            {
                if (control is TextBox textBox)
                {
                    if (textBox.Tag != null)
                    {
                        tagsDosCampos.Add(new CampoTagInfo
                        {
                            Nome = textBox.Name.Substring(3),
                            Tag = textBox.Tag.ToString()
                        });
                    }
                }
                else if (control is DateTimePicker dtp)
                {
                    if (dtp.Tag != null)
                    {
                        tagsDosCampos.Add(new CampoTagInfo
                        {
                            Nome = dtp.Name.Substring(3),
                            Tag = dtp.Tag.ToString()
                        });
                    }
                }
            }
        }

        private void MarcarControlesComErro(List<string> criticas)
        {
            HashSet<string> camposComErro = new HashSet<string>();
            foreach (string critica in criticas)
            {
                string[] palavras = critica.Split(' ');
                int indiceCampo = Array.IndexOf(palavras, "campo");
                if (indiceCampo != -1 && indiceCampo + 1 < palavras.Length)
                {
                    string nomeCampo = palavras[indiceCampo + 1];
                    camposComErro.Add(nomeCampo);
                }
            }
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox textBox)
                {
                    string nomeCampo = textBox.Name.Substring(3);
                    if (camposComErro.Contains(nomeCampo))
                    {
                        textBox.BackColor = Color.LightPink;
                    }
                    else
                    {
                        textBox.BackColor = SystemColors.Window;
                    }
                }
                else if (ctrl is DateTimePicker dtp)
                {
                    string nomeCampo = dtp.Name.Substring(3);
                    if (camposComErro.Contains(nomeCampo))
                    {
                        dtp.Font = new Font(dtp.Font.FontFamily, dtp.Font.Size, FontStyle.Bold);
                    }
                    else
                    {
                        dtp.Font = new Font(dtp.Font.FontFamily, dtp.Font.Size, FontStyle.Regular);
                    }
                }
            }
        }

        #endregion

        #region Pesquisa

        public void NrLinhas(int v)
        {
            DAO.SetarLinhas(v);
        }

        private System.Data.DataTable getDados()
        {
            return DAO.getDados();
        }
        private void PesqAcionar()
        {
            AlterarVisibilidadeControles(true);
        }
        public void LigaGrid()
        {
            Pesquisando = true;
            AlterarVisibilidadeControles(false);
            if (!GridCarregada)
            {
                CriaGrid();                
                int IndCtrGrid = this.Controls.Count - 1;
                var grid = this.Controls[IndCtrGrid] as DataGrid;
                if (grid != null)
                {
                    if (grid.TableStyles.Count == 0)
                    {
                        grid.TableStyles.Add(new DataGridTableStyle());
                    }
                    DataGridTableStyle tableStyle = grid.TableStyles[0];
                    tableStyle.GridColumnStyles[0].Width = 30;
                    tableStyle.GridColumnStyles[1].Width = 100;
                    try
                    {
                        tableStyle.GridColumnStyles[2].Width = 100;
                    }
                    catch (Exception)
                    {

                    }                    
                }
                GridCarregada = true;
            }
        }

        private void CriaGrid()
        {
            DataTable Dados = getDados();
            dataGrid = new DataGrid();
            this.Controls.Add(dataGrid);
            dataGrid.DataSource = Dados;
            dataGrid.Name = "GRID";
            dataGrid.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            dataGrid.ReadOnly = true;
            dataGrid.DoubleClick += new EventHandler(dataGrid_DoubleClick);
            int posY = cntrole1.Location.Y + cntrole1.Height;
            posY -= 20;
            int alturaDataGrid = this.ClientSize.Height - posY;
            dataGrid.SetBounds(0, posY, this.ClientSize.Width, alturaDataGrid);
            dataGrid.ColumnHeadersVisible = false;
        }

        private void dataGrid_DoubleClick(object sender, EventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            if (grid.CurrentRowIndex >= 0)
            {
                int rowIndex = grid.CurrentRowIndex;
                DataRowView selectedRowView = (DataRowView)grid.BindingContext[grid.DataSource].Current;
                object idValue = selectedRowView.Row["id"];
                CarregaRegistro(idValue.ToString());
            }
        }

        private void CarregaRegistro(string v)
        {
            reg = DAO.GetPeloID(v);
            Mostra();
            PesqAcionar();
            cntrole1.ControlesNormais();
        }

        protected void Cancela()
        {
            reg = DAO.GetEsse();
            ResetarAparenciaControles();
            Mostra();
        }

        private void AlterarVisibilidadeControles(bool visivel)
        {
            foreach (Control control in this.Controls)
            {
                switch (control.Name)
                {
                    case "cntrole1":
                        break;
                    case "GRID":
                        control.Visible = !visivel;
                        break;
                    default:
                        control.Visible = visivel;
                        break;
                }
            }
        }

        #endregion

    }
}
