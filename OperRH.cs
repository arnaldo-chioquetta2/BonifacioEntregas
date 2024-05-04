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

        private List<Lanctos> CarregaCaixa()
        {
            DateTime? dataInicio = dtpDataIN.Value;
            DateTime dataFim = dtnDtFim.Value;
            int SelTipo = cmbVendedor.SelectedIndex;
            string sFiltro = "";
            if (SelTipo > 0)
            {
                sFiltro = " and C.idForma = " + retIdForma(cmbVendedor.Text);
            }
            string SQL = $@"SELECT C.ID, C.Data, C.Valor, C.Desconto, 
                            C.idForma AS FormaPagto, Obs 
                            FROM Caixa C
                            Where Data Between #{ dataInicio:dd/MM/yyyy HH:ss}# and #{ dataFim:dd/MM/yyyy HH:ss}# {sFiltro} ";
            List<Lanctos> lancamentos = new List<Lanctos>();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(SQL, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Lanctos lancamento = new Lanctos();
                                lancamento.ID = (int)reader["ID"];
                                lancamento.DataPagamento = (DateTime)reader["Data"];
                                lancamento.Desconto = (decimal)reader["Desconto"];
                                lancamento.idFormaPagto = (int)reader["FormaPagto"];
                                if (lancamento.idFormaPagto == 5)
                                {
                                    lancamento.Entrada = 0;
                                    lancamento.Saida = (decimal)reader["Valor"];
                                }
                                else
                                {
                                    lancamento.Entrada = (decimal)reader["Valor"];
                                    lancamento.Saida = 0;
                                }
                                lancamento.Forma = retFormaId(lancamento.idFormaPagto);
                                lancamento.Saldo = lancamento.Entrada - lancamento.Desconto - lancamento.Saida;
                                lancamento.Obs = (string)reader["Obs"];
                                lancamentos.Add(lancamento);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            return lancamentos;
        }

        private int retIdForma(string Forma)
        {
            int ret = 5;
            switch (Forma)
            {
                case "Dinheiro":
                    ret = 0;
                    break;
                case "Cartão":
                    ret = 1;
                    break;
                case "Anotado":
                    ret = 2;
                    break;
                case "Pix":
                    ret = 3;
                    break;
                case "Despesa":
                    ret = 5;
                    break;
            }
            return ret;
        }

        private string retFormaId(int idForma)
        {
            string ret = "";
            switch (idForma)
            {
                case 0:
                    ret = "Dinheiro";
                    break;
                case 1:
                    ret = "Cartão";
                    break;
                case 2:
                    ret = "Anotado";
                    break;
                case 3:
                    ret = "Pix";
                    break;
                case 5:
                    ret = "Despesa";
                    break;
            }
            return ret;
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
            dataGrid1.Columns[0].Width = 0;
            dataGrid1.Columns[1].Width = 75;
            dataGrid1.Columns[2].Width = 110;
            dataGrid1.Columns[3].Width = 70; 
            dataGrid1.Columns[4].Width = 70; 
            dataGrid1.Columns[5].Width = 70;
            dataGrid1.Columns[6].Width = 70;
            dataGrid1.Invalidate();
        }

        private void Mostra()
        {
            DateTime DT1 = dtpDataIN.Value.Date;
            DateTime DT2 = dtnDtFim.Value.Date;
            DataTable dados = cRHDAO.getDados(DT1, DT2);
            DevAge.ComponentModel.BoundDataView boundDataView = new DevAge.ComponentModel.BoundDataView(dados.DefaultView);
            dataGrid1.DataSource = boundDataView;
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
            panel1.Height = 91;
            lbColaborador.Text = cmbVendedor.Text;
            btGravar.Enabled = false;
            txInMan.Focus();
        }

        private void cmbVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            btLancar.Enabled = (cmbVendedor.SelectedIndex > 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idFunc = Convert.ToInt32(cmbVendedor.SelectedValue.ToString());
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
            cRHDAO.AddHorario(idFunc, dInMan, dFmMan, dInTrd, dFnTrd, UID, dtHorario);
            txInMan.Text = "";
            txFmMan.Text = "";
            txInTrd.Text = "";
            txFnTrd.Text = "";
            if (dtpHorario.Value>DateTime.Now.AddDays(-1))
            {
                dtpHorario.Value = dtHorario.AddDays(1);
            }
            Mostra();
            txInMan.Focus();
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
    }

}