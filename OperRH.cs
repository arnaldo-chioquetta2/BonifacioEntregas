using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class OperRH : Form
    {

        private bool ativou = false;

        private RHDAO cRHDAO;
        private int iID = 0;
        private string UID = "";
        private bool carregando = true;
        private int idFunc = 0;

        public OperRH()
        {
            InitializeComponent();
            SetStartPosition();
            panel1.Height = 43;
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

       private void Extrato_Activated(object sender, EventArgs e)
        {
            if (!ativou)
            {
                ativou = true;
                VendedoresDAO Vendedor = new VendedoresDAO();
                CarregarComboBox<Vendedor>(cmbVendedor, Vendedor, "Selecione");
                dtnDtFim.Value = DateTime.Today;
                dtpDataIN.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                cRHDAO = new RHDAO();
                Mostra();                
                PreparaGrid();
            }
        }

        private void PreparaGrid()
        {
            dataGrid1.Columns[0].Visible = false;
            dataGrid1.Columns[1].Visible = false;
            dataGrid1.Columns[2].Width = 75;
            dataGrid1.Columns[3].Width = 110;
            dataGrid1.Columns[4].Width = 70; 
            dataGrid1.Columns[5].Width = 70; 
            dataGrid1.Columns[6].Width = 70;
            dataGrid1.Columns[7].Width = 70;
            dataGrid1.Columns[8].Width = 70;
            dataGrid1.Columns[9].Visible = false;
            dataGrid1.Invalidate();
        }

        private void Mostra()
        {
            this.carregando = true;
            DateTime DT1 = dtpDataIN.Value.Date;
            DateTime DT2 = dtnDtFim.Value.Date;
            int idFunc = Convert.ToInt32(cmbVendedor.SelectedValue.ToString());
            DataTable dados = cRHDAO.getDados(DT1, DT2, idFunc);
            dataGrid1.Rows.Clear();
            if (dataGrid1.Columns.Count == 0 && dados.Columns.Count > 0)
            {
                foreach (DataColumn column in dados.Columns)
                {
                    dataGrid1.Columns.Add(column.ColumnName, column.ColumnName);
                }
            }
            dataGrid1.Columns.Add("Total", "Total");
            foreach (DataRow row in dados.Rows)
            {
                string[] rowData = new string[dados.Columns.Count+1];
                for (int i = 0; i < dados.Columns.Count; i++)
                {
                    rowData[i] = row[i].ToString();
                }
                DateTime? inMan = ParseTime(row["InMan"].ToString());
                DateTime? fmMan = ParseTime(row["FmMan"].ToString());
                DateTime? inTrd = ParseTime(row["InTrd"].ToString());
                DateTime? fnTrd = ParseTime(row["FnTrd"].ToString());
                TimeSpan total = TimeSpan.Zero;
                if (inMan != null && fmMan != null)
                {
                    total += fmMan.Value - inMan.Value;
                }
                if (inTrd != null && fnTrd != null)
                {
                    total += fnTrd.Value - inTrd.Value;
                }
                rowData[9] = total.ToString(@"hh\:mm");
                dataGrid1.Rows.Add(rowData);
            }
            this.carregando = false;
        }

        private DateTime? ParseTime(string timeString)
        {
            if (DateTime.TryParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
            {
                return time;
            }
            return null;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            Mostra();
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintPageHandler);
            printDocument.Print();
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            //Font font = new Font("Courier New", 10);
            //float yPos = 0;
            //int count = 0;
            //string[] lines = textBox1.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            //foreach (string line in lines)
            //{
            //    yPos = count * font.GetHeight(e.Graphics);
            //    e.Graphics.DrawString(line, font, Brushes.Black, new PointF(10, yPos));
            //    count++;
            //}
        }

        private void CarregarComboBox<T>(ComboBox comboBox, BaseDAO classe, string ItemZero = "") where T : IDataEntity, new()
        {
            DataTable dados = classe.GetDadosOrdenados();
            List<ComboBoxItem> lista = new List<ComboBoxItem>();
            if (ItemZero.Length > 0)
            {
                ComboBoxItem item = new ComboBoxItem(0, ItemZero);
                lista.Add(item);
            }
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                ComboBoxItem item = new ComboBoxItem(id, nome);
                lista.Add(item);
            }
            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Nome";
            comboBox.ValueMember = "Id";
        }

        #region Classes

        private class Lanctos
        {
            public string Forma;
            public int ID;
            public DateTime DataPagamento;
            public decimal Entrada;
            public decimal Desconto;
            public decimal Saida;
            public int idFormaPagto;
            public decimal Saldo;
            public int Quantidade;
            public string Obs;

        }

        #endregion

        private void btLancar_Click(object sender, EventArgs e)
        {
            LancaRegistro();
        }

        private void LancaRegistro()
        {
            panel1.Height = 91;
            lbColaborador.Text = cmbVendedor.Text;
            btGravar.Enabled = false;
            txInMan.Text = "";
            txFmMan.Text = "";
            txInTrd.Text = "";
            txFnTrd.Text = "";
            txInMan.Focus();
        }

        private void cmbVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            btLancar.Enabled = (cmbVendedor.SelectedIndex > 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            DateTime dInMan, dFmMan, dInTrd, dFnTrd;
            string sInMan = txInMan.Text.Replace(";", ":");
            string sFmMan = txFmMan.Text.Replace(";", ":");
            string sInTrd = txInTrd.Text.Replace(";", ":");
            string sFnTrd = txFnTrd.Text.Replace(";", ":");
            sInMan = string.IsNullOrWhiteSpace(sInMan) ? "00:00" : sInMan;
            sFmMan = string.IsNullOrWhiteSpace(sFmMan) ? "00:00" : sFmMan;
            sInTrd = string.IsNullOrWhiteSpace(sInTrd) ? "00:00" : sInTrd;
            sFnTrd = string.IsNullOrWhiteSpace(sFnTrd) ? "00:00" : sFnTrd;            
            string UID = glo.GenerateUID();
            DateTime.TryParseExact(sInMan, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dInMan);
            DateTime.TryParseExact(sFmMan, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dFmMan);
            DateTime.TryParseExact(sInTrd, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dInTrd);
            DateTime.TryParseExact(sFnTrd, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dFnTrd);
            DateTime dtHorario = dtpHorario.Value.Date;
            if (this.iID==0)
            {
                int idFuncio = Convert.ToInt32(cmbVendedor.SelectedValue.ToString());
                glo.Loga($@"HA,{idFuncio}, {dInMan}, {dFmMan}, {dInTrd}, {dFnTrd}, {UID}, {dtHorario}");
                cRHDAO.AddHorario(idFuncio, dInMan, dFmMan, dInTrd, dFnTrd, UID, dtHorario);
                txInMan.Text = "";
                txFmMan.Text = "";
                txInTrd.Text = "";
                txFnTrd.Text = "";
                if (dtpHorario.Value < DateTime.Now.AddDays(-1))
                {
                    dtpHorario.Value = dtHorario.AddDays(1);
                }
                if (dtpDataIN.Value> dtpHorario.Value)
                {
                    dtpDataIN.Value = dtpHorario.Value;
                }
                txInMan.Focus();
            } else
            {
                glo.Loga($@"HE,{this.iID}, {this.idFunc}, {dInMan}, {dFmMan}, {dInTrd}, {dFnTrd}, {UID}, {dtHorario}");
                cRHDAO.EdHorario(this.iID, this.idFunc, dInMan, dFmMan, dInTrd, dFnTrd, dtHorario);
                DesfazEdicao();
            }
            Mostra();            
        }

        private void txInMan_KeyUp(object sender, KeyEventArgs e)
        {
            string sInMan = txInMan.Text.Replace(";", ":");
            string sFmMan = txFmMan.Text.Replace(";", ":");
            string sInTrd = txInTrd.Text.Replace(";", ":");
            string sFnTrd = txFnTrd.Text.Replace(";", ":");
            string timeFormat = "HH:mm";
            int temManha=0;
            int temTarde=0;
            bool isValid = false;
            if (sInMan.Length > 0)
            {
                temManha++;
                if ((sFmMan.Length > 0))
                {
                    isValid = true;
                    temManha++;
                }
            }
            if (sInTrd.Length > 0) 
            {
                temTarde++;
                if (sFnTrd.Length > 0)
                {
                    isValid = true;
                    temTarde++;
                }
            }
            if (temManha>0 && isValid)
            {
                DateTime inMan, fmMan;
                if (DateTime.TryParseExact(sInMan, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out inMan) &&
                    DateTime.TryParseExact(sFmMan, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out fmMan))
                {
                    if (inMan >= fmMan)
                    {
                        isValid = false;
                    }
                } else
                {
                    isValid = false;
                }
            }
            if (temTarde>0 && isValid)
            {
                DateTime inTar, fmTar;
                if (DateTime.TryParseExact(sInTrd, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out inTar) &&
                    DateTime.TryParseExact(sFnTrd, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out fmTar))
                {
                    if (inTar >= fmTar)
                    {
                        isValid = false;
                    }
                } else
                {
                    isValid = false;
                }
            }
            btGravar.Enabled = isValid;
        }

        private void btExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                glo.Loga($@"HD,{this.iID}, {this.UID}");
                cRHDAO.Exclui(this.iID);
                Mostra();
                DesfazEdicao();
            }
        }

        private void DesfazEdicao()
        {
            btExcluir.Visible = false;
            panel1.Height = 43;
            this.iID = 0;
        }

        private void dataGrid1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.carregando==false)
            {
                if (dataGrid1.SelectedRows.Count > 0)
                {
                    LancaRegistro();
                    btExcluir.Visible = true;
                    DataGridViewRow selectedRow = dataGrid1.SelectedRows[0];
                    this.iID = Convert.ToInt16(selectedRow.Cells["ID"].Value.ToString());
                    this.UID = selectedRow.Cells["UID"].Value.ToString();
                    this.idFunc = Convert.ToInt16(selectedRow.Cells["FuncID"].Value.ToString());
                    dtpHorario.Value = Convert.ToDateTime(selectedRow.Cells["Data"].Value);
                    txInMan.Text = selectedRow.Cells["InMan"].Value.ToString();
                    txFmMan.Text = selectedRow.Cells["FmMan"].Value.ToString();
                    txInTrd.Text = selectedRow.Cells["InTrd"].Value.ToString();
                    txFnTrd.Text = selectedRow.Cells["FnTrd"].Value.ToString();
                }

            }
        }
    }

}