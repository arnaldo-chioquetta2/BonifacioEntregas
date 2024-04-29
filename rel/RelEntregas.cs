using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace TeleBonifacio.rel
{
    public partial class RelEntegas : Form
    {

        private bool ativou = false;
        private List<Lanctos> rel { get; set; }
        public DateTime Data { get; internal set; }

        public RelEntegas()
        {
            InitializeComponent();
            SetStartPosition();
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private List<Lanctos> CarregaRelEntregas()
        {
            DateTime? dataInicio = dtpDataIN.Value;
            DateTime dataFim = dtnDtFim.Value;
            string SQL = $@"SELECT DateValue(Data) AS Data,
                            SUM(VlNota - Desconto) AS Vendas,
                            SUM(Valor) AS Entregas,
                            COUNT(*) AS Qtd
                        FROM Entregas
                        Where Data Between #{ dataInicio:dd/MM/yyyy 00:00}# and #{ dataFim:dd/MM/yyyy 23:59}#
                        GROUP BY DateValue(Data)";
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
                                lancamento.Data = (DateTime)reader["Data"];
                                lancamento.Vendas = (decimal)reader["Vendas"];
                                lancamento.Entregas = (decimal) reader["Entregas"];
                                lancamento.Qtd = (int)reader["Qtd"];
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

        public void GerarRelEntregas()
        {
            var rel = CarregaRelEntregas();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Relatório de Entregas");
            sb.AppendLine();
            sb.AppendLine($"Período: {dtpDataIN.Value.ToString("dd/MM/yyyy")} a {dtnDtFim.Value.ToString("dd/MM/yyyy")}");
            sb.AppendLine();
            sb.AppendLine("Data       |   Vendas  |  Entregas  |  Quantidade");
            decimal TVendas = 0;
            decimal TEntregas = 0;
            int TQuantidade = 0;
            foreach (var lancos in rel)
            {
                string Data = glo.ComplStr(lancos.Data.ToString("dd/MM/yyyy"), 10, 2);
                string Vendas = glo.ComplStr(lancos.Vendas.ToString("N2"), 9, 2);
                string Entregas = glo.ComplStr(lancos.Entregas.ToString("N2"), 10, 2);
                string Quantidade = glo.ComplStr(lancos.Qtd.ToString(), 11, 2);
                sb.AppendLine($"{Data} | {Vendas} | {Entregas} | {Quantidade}");
                TVendas += lancos.Vendas;
                TEntregas += lancos.Entregas;
                TQuantidade += lancos.Qtd;
            }
            sb.AppendLine();
            string stVendas = glo.ComplStr(TVendas.ToString("N2"), 9, 2);
            string stEntregas = glo.ComplStr(TEntregas.ToString("N2"), 10, 2);
            string stQuantidade = glo.ComplStr(TQuantidade.ToString(), 11, 2);
            sb.AppendLine($"             {stVendas} | {stEntregas} | {stQuantidade}");
            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void Extrato_Activated(object sender, EventArgs e)
        {
            if (!ativou)
            {                
                ativou = true;
                dtnDtFim.Value = Data;
                dtpDataIN.Value = Data.AddDays(-30);
                GerarRelEntregas();
            }                            
        }
        
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            GerarRelEntregas();
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintPageHandler);
            printDocument.Print();
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Courier New", 12);
            float yPos = 0;
            int count = 0;
            string[] lines = textBox1.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                yPos = count * font.GetHeight(e.Graphics);
                e.Graphics.DrawString(line, font, Brushes.Black, new PointF(10, yPos));
                count++;
            }
        }
        
        #region Classes

        private class Lanctos
        {
            public DateTime Data;
            public decimal Vendas;
            public decimal Entregas;
            public int Qtd;            
        }

        #endregion

    }

}