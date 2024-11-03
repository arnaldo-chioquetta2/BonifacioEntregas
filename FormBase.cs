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
using System.Drawing.Printing;

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
        private System.Windows.Forms.DataGrid dataGrid = null;
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
                check.Checked = (valor == "True" || valor == "1");
               //  check.Checked = (valor == "True");
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
            } else
            {
                textBox.Text = "";
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
                if (!data.HasValue || (data.Value.Date == glo.D0) || (data.Value.Date == glo.D1))
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

        private void MapearCamposParaModelo(dao.BaseDAO reg, Control container = null)
        {
            Control[] controls = container == null ? this.Controls.Cast<Control>().ToArray() : container.Controls.Cast<Control>().ToArray();

            foreach (Control control in controls)
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
                    }
                    else if (control is CheckBox check)
                    {
                        MapearCheckParaModelo(check, reg);
                    }
                    else if (control is ComboBox cmb)
                    {
                        MapearComboParaModelo(cmb, reg);
                    }
                    else if (control is GroupBox groupBox)
                    {
                        // Chamada recursiva para mapear controles dentro do GroupBox
                        MapearCamposParaModelo(reg, groupBox);
                    }
                    else if (control.HasChildren)
                    {
                        // Chamada recursiva para outros tipos de contêineres
                        MapearCamposParaModelo(reg, control);
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
                int Vlr = check.Checked ? 1 : 0;
                propertyInfo.SetValue(reg, Vlr, null);
            }
        }

        private void MapearTextBoxParaModelo(TextBox textBox, dao.BaseDAO reg)
        {
            string propertyName = textBox.Name.Substring(3);
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);

            if (propertyInfo != null)
            {
                if (!TrySetPropertyValue(propertyInfo, reg, textBox.Text))
                {
                    Console.WriteLine($"Falha na conversão para {propertyInfo.PropertyType}: {textBox.Text}");
                }
            }
            else
            {
                Console.WriteLine($"Propriedade '{propertyName}' não encontrada");
            }
        }

        private bool TrySetPropertyValue(PropertyInfo propertyInfo, dao.BaseDAO reg, string text)
        {
            if (propertyInfo.PropertyType == typeof(int))
            {
                return TrySetIntProperty(propertyInfo, reg, text);
            }
            else if (propertyInfo.PropertyType == typeof(decimal))
            {
                return TrySetDecimalProperty(propertyInfo, reg, text);
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(reg, text, null);
                return true;
            }
            else if (propertyInfo.PropertyType == typeof(TimeSpan))
            {
                return TrySetTimeSpanProperty(propertyInfo, reg, text);
            }
            else
            {
                Console.WriteLine("Tipo de propriedade não suportado");
                return false;
            }
        }

        private bool TrySetIntProperty(PropertyInfo propertyInfo, dao.BaseDAO reg, string text)
        {
            if (int.TryParse(text, out int value))
            {
                propertyInfo.SetValue(reg, value, null);
                return true;
            }
            return false;
        }

        private bool TrySetDecimalProperty(PropertyInfo propertyInfo, dao.BaseDAO reg, string text)
        {
            if (decimal.TryParse(text, out decimal value))
            {
                propertyInfo.SetValue(reg, value, null);
                return true;
            }
            return false;
        }

        private bool TrySetTimeSpanProperty(PropertyInfo propertyInfo, dao.BaseDAO reg, string text)
        {
            if (TimeSpan.TryParse(text, out TimeSpan value))
            {
                propertyInfo.SetValue(reg, value, null);
                return true;
            }
            return false;
        }        

        private void MapearDateTimePickerParaModelo(DateTimePicker dtpControl, dao.BaseDAO reg)
        {
            string propertyName = dtpControl.Name.Substring(3);
            PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);

            if (propertyInfo != null)
            {
                if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                {
                    if (dtpControl.Format != DateTimePickerFormat.Custom)
                    {
                        propertyInfo.SetValue(reg, dtpControl.Value, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(reg, null, null);
                    }
                }
                else if (propertyInfo.PropertyType == typeof(TimeSpan) || propertyInfo.PropertyType == typeof(TimeSpan?))
                {
                    TimeSpan timeSpanValue = dtpControl.Value.TimeOfDay;
                    propertyInfo.SetValue(reg, timeSpanValue, null);
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

        // Refatorado em 03/08/24 Original 62 linhas, resultado 32 linhas
        protected void cntrole1_AcaoRealizada(object sender, AcaoEventArgs e, tb.IDataEntity entidade)
        {
            switch (e.Acao)
            {
                case "Adicionar":
                    HandleAdicionar();
                    break;
                case "Delete":
                    HandleDelete(entidade);
                    break;
                case "ParaTras":
                    HandleParaTras();
                    break;
                case "ParaFrente":
                    HandleParaFrente();
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

        private void HandleAdicionar()
        {
            LimparCampos();
            EmAdicao = true;
        }

        private void HandleDelete(tb.IDataEntity entidade)
        {
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
            if (GridCarregada)
            {
                DescarrregaGrid();
            }
        }

        private void DescarrregaGrid()
        {
            this.Controls.Remove(dataGrid);
            dataGrid.Dispose();
            dataGrid = null;
            GridCarregada = false;
        }

        private void HandleParaTras()
        {
            Direcao = -1;
            reg = DAO.ParaTraz();
            if (!Mostra())
            {
                cntrole1.Ultimo = true;
            }
        }

        private void HandleParaFrente()
        {
            Direcao = 1;
            reg = DAO.ParaFrente();
            if (!Mostra())
            {
                cntrole1.Primeiro = true;
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
                    if (GridCarregada)
                    {
                        DescarrregaGrid();
                    }
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
                        // OBRIGATÓRIO
                        object valor = propriedade.GetValue(objeto);
                        if (valor == null || (valor is string && string.IsNullOrEmpty((string)valor)))
                        {
                            criticas.Add($"O campo {propriedade.Name} é obrigatório.");
                        }
                    }
                    else if (campoTag.Tag == "H" && propriedade.PropertyType == typeof(DateTime))
                    {
                        // NÃO PODE SER SUPERIOR A HOJE
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
                    try
                    {
                        tableStyle.GridColumnStyles[0].Width = 30;
                        tableStyle.GridColumnStyles[1].Width = 100;

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

        #region Impressão        
        protected void Imprimir()
        {
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.Landscape = true;
            pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
            pd.PrintPage += (sender, e) =>
            {
                Graphics g = e.Graphics;
                float scale = e.MarginBounds.Width / (float)this.Width;
                PrintControls(this.Controls, g, e.MarginBounds, scale, 0, 0);
            };

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }
        private void PrintControls(Control.ControlCollection controls, Graphics g, Rectangle bounds, float scale, float parentX, float parentY)
        {
            foreach (Control ctrl in controls.Cast<Control>().OrderBy(c => c.TabIndex))
            {
                float x = parentX + ctrl.Left * scale;
                float y = parentY + ctrl.Top * scale;

                if (ctrl is GroupBox || ctrl.Controls.Count > 0)
                {
                    // Recursivamente imprimir controles dentro de contêineres
                    PrintControls(ctrl.Controls, g, bounds, scale, x, y);
                }

                string text = GetControlText(ctrl);
                if (!string.IsNullOrEmpty(text))
                {
                    using (Font scaledFont = new Font(ctrl.Font.FontFamily, ctrl.Font.Size * scale, ctrl.Font.Style))
                    {
                        SizeF size = g.MeasureString(text, scaledFont);
                        g.DrawString(text, scaledFont, Brushes.Black, bounds.Left + x, bounds.Top + y);
                    }
                }
            }
        }
        private string GetControlText(Control ctrl)
        {
            if (ctrl is TextBox textBox)
            {
                return textBox.Name.StartsWith("txtSenha") ? "******" : textBox.Text;
            }
            else if (ctrl is DateTimePicker dtp)
            {
                if (dtp.Format == DateTimePickerFormat.Custom)
                {
                    if (dtp.CustomFormat == "HH:mm")
                    {
                        return dtp.Value.ToString("HH:mm");
                    }
                    else if (dtp.CustomFormat == " ")
                    {
                        return ""; // Retorna vazio se o DateTimePicker estiver vazio
                    }
                    return dtp.Text; // Para outros formatos customizados
                }
                else if (dtp.ShowUpDown) // Isso geralmente indica um seletor de hora
                {
                    return dtp.Value.ToString("HH:mm");
                }
                else
                {
                    return dtp.Value.ToShortDateString();
                }
            }
            else if (ctrl is CheckBox checkBox)
            {
                return $"{checkBox.Text}: {(checkBox.Checked ? "Sim" : "Não")}";
            }
            else if (ctrl is Label label)
            {
                return label.Text;
            }
            return ctrl.Text;
        }

        #endregion

    }
}
