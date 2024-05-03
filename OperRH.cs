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
        private DateTime? DataInicio { get; set; }
        private DateTime DataFim { get; set; }
        private List<Lanctos> relcaixa { get; set; }
        public DateTime DT1 { get; set; }
        public DateTime DT2 { get; set; }

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
            if (SelTipo>0)
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

        public void GerarRelCaixa()
        {
            relcaixa = CarregaCaixa();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Extrato de Movimentação do Caixa");
            sb.AppendLine();
            sb.AppendLine($"Período: {dtpDataIN.Value.ToString("dd/MM/yyyy")} a {dtpDataIN.Value.ToString("dd/MM/yyyy")}");
            sb.AppendLine();
            sb.AppendLine("ID       Data     |  Entrada | Desconto |  Saídas |  FormaPagto |  Saldo  | Observação");
            // sb.AppendLine("ID       Data     |  Entrada | Desconto |  Saídas |  FormaPagto |  Saldo ");
            decimal TDinheiro = 0;
            decimal TCartao = 0;
            decimal TPix = 0;
            decimal TDespesa = 0;
            foreach (var lancos in relcaixa)
            {
                string ID = glo.ComplStr(lancos.ID.ToString(), 4, 2);
                string Data = glo.ComplStr(lancos.DataPagamento.ToString("dd/MM/yyyy"), 10, 2);
                string Entrada = glo.ComplStr(lancos.Entrada.ToString("N2"), 8, 3);
                string Desconto = glo.ComplStr(lancos.Desconto.ToString("N2"), 8, 3);
                string Saidas = glo.ComplStr(lancos.Saida.ToString("N2"), 7, 3);
                string Forma = glo.ComplStr(lancos.Forma, 11, 2);
                string Saldo = glo.ComplStr(lancos.Saldo.ToString("N2"), 7, 2);
                string Obs = lancos.Obs.Substring(0, Math.Min(lancos.Obs.Length, 20));
                sb.AppendLine($"{ID}   {Data}   {Entrada}   {Desconto}   {Saidas}   {Forma}   {Saldo} {Obs}");
                switch (lancos.idFormaPagto)
                {
                    case 0:
                        TDinheiro += lancos.Entrada - lancos.Desconto;
                    break;
                    case 1:
                        TCartao += lancos.Entrada - lancos.Desconto;
                    break;
                    case 3:
                        TPix += lancos.Entrada - lancos.Desconto;
                    break;
                    case 5:
                        TDespesa += lancos.Saida;
                    break;
                }
            }
            sb.AppendLine();
            sb.AppendLine($"Dinheiro: " + glo.ComplStr(TDinheiro.ToString("N2"), 9, 2));
            sb.AppendLine($"Cartão:   " + glo.ComplStr(TCartao.ToString("N2"), 9, 2));
            sb.AppendLine($"Pix:      " + glo.ComplStr(TPix.ToString("N2"), 9, 2));
            sb.AppendLine($"Despesas: " + glo.ComplStr(TDespesa.ToString("N2"), 9, 2));
            decimal total = TDinheiro + TCartao + TPix - TDespesa;
            string totalString = glo.ComplStr(total.ToString("N2"), 9, 2); 
            string EspacosAjustes = "";
            if (total > 0)
            {
                EspacosAjustes = new string(' ', 25); 
            }
            string EspacosIniciais = new string(' ', 34);
            string final = $"Saldo:{EspacosIniciais}{EspacosAjustes}{totalString}";
            sb.AppendLine();
            sb.AppendLine(final);
            //textBox1.Text = sb.ToString(); ;
            //textBox1.SelectionStart = textBox1.Text.Length;
            //textBox1.ScrollToCaret();
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
                //GerarRelCaixa();
            }
        }
        
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            //GerarRelCaixa();
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
            txInMan.Focus();
        }

        private void cmbVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            btLancar.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idFunc = 1;
            DateTime dInMan, dFmMan, dInTrd, dFnTrd;
            string sInMan = string.IsNullOrWhiteSpace(txInMan.Text) ? "00:00" : txInMan.Text;
            string sFmMan = string.IsNullOrWhiteSpace(txFmMan.Text) ? "00:00" : txFmMan.Text;
            string sInTrd = string.IsNullOrWhiteSpace(txInTrd.Text) ? "00:00" : txInTrd.Text;
            string sFnTrd = string.IsNullOrWhiteSpace(txFnTrd.Text) ? "00:00" : txInTrd.Text;
            RHDAO cRHDAO = new RHDAO();
            string UID = glo.GenerateUID();
            DateTime.TryParseExact(sInMan, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dInMan);
            DateTime.TryParseExact(sFmMan, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dFmMan);
            DateTime.TryParseExact(sInTrd, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dInTrd);
            DateTime.TryParseExact(sFnTrd, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dFnTrd);
            cRHDAO.AddHorario(idFunc, dInMan, dFmMan, dInTrd, dFnTrd, UID);
            txInMan.Text = "";
            txFmMan.Text = "";
            txInTrd.Text = "";
            txFnTrd.Text = "";
            txInMan.Focus();
        }

        private void txInMan_KeyUp(object sender, KeyEventArgs e)
        {
            string timeFormat = "HH:mm";
            bool isValid = true;
            if (!string.IsNullOrWhiteSpace(txFnTrd.Text) && !string.IsNullOrWhiteSpace(txInTrd.Text))
            {
                if (!DateTime.TryParseExact(txFnTrd.Text, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _) ||
                    !DateTime.TryParseExact(txInTrd.Text, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    isValid = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(txFmMan.Text) && !string.IsNullOrWhiteSpace(txInMan.Text))
            {
                if (!DateTime.TryParseExact(txFmMan.Text, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _) ||
                    !DateTime.TryParseExact(txInMan.Text, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    isValid = false;
                }
            }
            btGravar.Enabled = isValid;
        }
    }

}