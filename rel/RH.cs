using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio.rel
{
    public partial class RH : Form
    {

        private RHDAO cRHDAO;
        private bool ativou = false;
        private bool carregando = true;
        private int idFunc = 0;
        internal DateTime setID1;

        private List<Lanctos> relcaixa { get; set; }
        public DateTime DT1 { get; set; }
        public DateTime DT2 { get; set; }

        public RH()
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

        private void Mostra()
        {
            this.carregando = true;
            DateTime DT1 = dtpDataIN.Value.Date;
            DateTime DT2 = dtnDtFim.Value.Date;
            int idFunc = Convert.ToInt32(cmbVendedor.SelectedValue.ToString());
            DataTable dados = cRHDAO.getDados(DT1, DT2, idFunc);
            relcaixa = new List<Lanctos>();
            foreach (DataRow row in dados.Rows)
            {
                Lanctos lancto = new Lanctos
                {
                    ID = Convert.ToInt32(row["ID"]),
                    UID = row["uid"].ToString(),
                    Data = Convert.ToDateTime(row["Data"]),
                    Nome = row["Nome"].ToString(),
                    InMan = row["InMan"].ToString(),
                    FmMan = row["FmMan"].ToString(),
                    InTrd = row["InTrd"].ToString(),
                    FnTrd = row["FnTrd"].ToString(),
                    FuncID = Convert.ToInt32(row["FuncID"])
                };

                TimeSpan inMan = ProcHora(lancto.InMan);
                TimeSpan fmMan = ProcHora(lancto.FmMan);
                TimeSpan inTrd = ProcHora(lancto.InTrd);
                TimeSpan fnTrd = ProcHora(lancto.FnTrd);
                TimeSpan total = (fmMan - inMan) + (fnTrd - inTrd);
                lancto.Total = total.ToString(@"hh\:mm");
                relcaixa.Add(lancto);
            }

            GerarRelCaixa();
            this.carregando = false;
        }

        public void GerarRelCaixa()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Relatório de Horas Trabalhadas");
            sb.AppendLine();
            sb.AppendLine($"Período: {dtpDataIN.Value.ToString("dd/MM/yyyy")} a {dtnDtFim.Value.ToString("dd/MM/yyyy")}");
            sb.AppendLine();
            sb.AppendLine("ID    Data       Nome           InMan  FmMan  InTrd  FnTrd  Total");
            TimeSpan totalzao = TimeSpan.Zero;
            foreach (var lancto in relcaixa)
            {
                string ID = glo.ComplStr(lancto.ID.ToString(), 4, 2);
                string Data = glo.ComplStr(lancto.Data.ToString("dd/MM/yyyy"), 10, 2);
                string Nome = glo.ComplStr(lancto.Nome, 15, 2);
                string InMan = glo.ComplStr(lancto.InMan, 5, 2);
                string FmMan = glo.ComplStr(lancto.FmMan, 5, 2);
                string InTrd = glo.ComplStr(lancto.InTrd, 5, 2);
                string FnTrd = glo.ComplStr(lancto.FnTrd, 5, 2);
                string Total = glo.ComplStr(lancto.Total, 5, 2);

                sb.AppendLine($"{ID} {Data} {Nome} {InMan} {FmMan} {InTrd} {FnTrd} {Total}");
                totalzao += TimeSpan.Parse(lancto.Total);
            }

            sb.AppendLine();
            sb.AppendLine("Total de horas: " + totalzao.ToString(@"hh\:mm"));
            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private TimeSpan ProcHora(string horaStr)
        {
            if (string.IsNullOrEmpty(horaStr))
                return TimeSpan.Zero;
            return TimeSpan.Parse(horaStr);
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
            Font font = new Font("Courier New", 10);
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

        public void SetDados(int selectedIndex, DateTime value1, DateTime value2)
        {
            dtpDataIN.Value = value1;
            dtnDtFim.Value = value2;            
            VendedoresDAO Vendedor = new VendedoresDAO();
            glo.CarregarComboBox<tb.Vendedor>(cmbVendedor, Vendedor, "Selecione");
            cmbVendedor.SelectedIndex = selectedIndex;
            cRHDAO = new RHDAO();
        }

        private void RH_Activated_1(object sender, EventArgs e)
        {
            if (!ativou)
            {
                ativou = true;
                Mostra();                
            }
        }

        private class Lanctos
        {
            public int ID { get; set; }
            public string UID { get; set; }
            public DateTime Data { get; set; }
            public string Nome { get; set; }
            public string InMan { get; set; }
            public string FmMan { get; set; }
            public string InTrd { get; set; }
            public string FnTrd { get; set; }
            public int FuncID { get; set; }
            public string Total { get; set; }
        }

    }
}