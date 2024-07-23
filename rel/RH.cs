using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
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
            rt.AdjustFormComponents(this);
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

            TimeSpan totalzao = TimeSpan.Zero;

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
                    InCafeMan = row["InCafeMan"].ToString(),
                    FmCafeMan = row["FmCafeMan"].ToString(),
                    InCafeTrd = row["InCafeTrd"].ToString(),
                    FmCafeTrd = row["FmCafeTrd"].ToString(),
                    FuncID = Convert.ToInt32(row["FuncID"])
                };

                TimeSpan inMan = ProcHora(lancto.InMan);
                TimeSpan fmMan = ProcHora(lancto.FmMan);
                TimeSpan inCafeMan = ProcHora(lancto.InCafeMan);
                TimeSpan fmCafeMan = ProcHora(lancto.FmCafeMan);
                TimeSpan inTrd = ProcHora(lancto.InTrd);
                TimeSpan fnTrd = ProcHora(lancto.FnTrd);
                TimeSpan inCafeTrd = ProcHora(lancto.InCafeTrd);
                TimeSpan fmCafeTrd = ProcHora(lancto.FmCafeTrd);

                TimeSpan totalDia = TimeSpan.Zero;

                // 1) Inicio da manhã até o inicio do café da manhã (só se tiver inicio do café da manhã)
                if (inMan != TimeSpan.Zero && (inCafeMan != TimeSpan.Zero || fmMan != TimeSpan.Zero))
                {
                    if (inCafeMan != TimeSpan.Zero)
                        totalDia += inCafeMan - inMan;
                    else
                        totalDia += fmMan - inMan;
                }

                // 2) Fim do café da manhã até a saída da manhã (só se tiver saída da manhã)
                if (fmCafeMan != TimeSpan.Zero && fmMan != TimeSpan.Zero)
                {
                    totalDia += fmMan - fmCafeMan;
                }
                else if (fmMan != TimeSpan.Zero && inCafeMan != TimeSpan.Zero)
                {
                    totalDia += fmMan - inCafeMan;
                }

                // 3) Inicio da tarde até o inicio do café da tarde (só se tiver inicio de café da tarde)
                if (inTrd != TimeSpan.Zero && (inCafeTrd != TimeSpan.Zero || fnTrd != TimeSpan.Zero))
                {
                    if (inCafeTrd != TimeSpan.Zero)
                        totalDia += inCafeTrd - inTrd;
                    else
                        totalDia += fnTrd - inTrd;
                }

                // 4) Fim do café da tarde até fim do turno (só se tiver fim do turno)
                if (fmCafeTrd != TimeSpan.Zero && fnTrd != TimeSpan.Zero)
                {
                    totalDia += fnTrd - fmCafeTrd;
                }
                else if (fnTrd != TimeSpan.Zero && inCafeTrd != TimeSpan.Zero)
                {
                    totalDia += fnTrd - inCafeTrd;
                }

                totalzao += totalDia;

                // Calcula o total de horas e minutos corretamente, mesmo para valores acima de 24 horas
                int totalHoras = (int)totalDia.TotalHours; // TotalHours inclui horas completas de todos os minutos
                int totalMinutos = totalDia.Minutes;

                lancto.Total = $"{totalHoras:D2}:{totalMinutos:D2}";
                relcaixa.Add(lancto);
            }

            //if (idFunc > 0)
            //{
            //    string[] totalRowData = new string[dados.Columns.Count + 1];
            //    for (int i = 0; i < dados.Columns.Count; i++)
            //    {
            //        totalRowData[i] = "";
            //    }
            //    totalRowData[12] = $"{(int)totalzao.TotalHours:D2}:{totalzao.Minutes:D2}";
            //    dataGrid1.Rows.Add(totalRowData);
            //}

            GerarRelCaixa();
            this.carregando = false;
        }

        public void GerarRelCaixa()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(glo.ComplStr("Relatório de Horas Trabalhadas", 57, 1));
            sb.AppendLine(glo.ComplStr(cmbVendedor.Text, 57, 1));
            string dDatas = $"Período: {dtpDataIN.Value.ToString("dd/MM/yyyy")} a {dtnDtFim.Value.ToString("dd/MM/yyyy")}";
            sb.AppendLine(glo.ComplStr(dDatas, 57, 1));
            sb.AppendLine();
            sb.AppendLine("Data       Nome            InMan InCafe FmCafe FmMan InTrd InCafe FmCafe FnTrd  Total");
            TimeSpan totalzao = TimeSpan.Zero;
            TimeSpan totalMensal = TimeSpan.Zero;
            int currentMonth = -1;
            bool firstEntry = true;
            bool isFirstMonth = true;

            foreach (Lanctos lancto in relcaixa)
            {
                if (lancto.Data.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (currentMonth != lancto.Data.Month)
                    {
                        if (!firstEntry)
                        {
                            sb.AppendLine($"Total do mês: {totalMensal.TotalHours:n0} horas {totalMensal.Minutes} minutos");
                            totalMensal = TimeSpan.Zero;
                        }
                        currentMonth = lancto.Data.Month;
                        if (!isFirstMonth)
                        {
                            sb.AppendLine();
                            sb.AppendLine($"-- {lancto.Data.ToString("MMMM yyyy")} --");
                        }
                        isFirstMonth = false;
                        firstEntry = false;
                    }
                    AdicionarLinhaLancamento(sb, lancto);
                    TimeSpan totalDoDia = CalcularTotalDoDia(lancto);
                    totalMensal += totalDoDia;
                    totalzao += totalDoDia;
                }
            }
            if (currentMonth != -1)
            {
                sb.AppendLine($"Total do mês: {totalMensal.TotalHours:n0} horas {totalMensal.Minutes} minutos");
            }
            sb.AppendLine();
            sb.AppendLine($"Total de horas: {totalzao.TotalHours:n0} horas {totalzao.Minutes} minutos");
            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        // 12/07/2024         SHIRLEI 08:01 09:44   10:01 12:02 13:03  16:33 16:45  18:00  08:29

        private TimeSpan CalcularTotalDoDia(Lanctos lancto)
        {
            TimeSpan inMan = ProcHora(lancto.InMan);
            TimeSpan fmMan = ProcHora(lancto.FmMan);
            TimeSpan inCafeMan = ProcHora(lancto.InCafeMan);
            TimeSpan fmCafeMan = ProcHora(lancto.FmCafeMan);
            TimeSpan inTrd = ProcHora(lancto.InTrd);
            TimeSpan fnTrd = ProcHora(lancto.FnTrd);
            TimeSpan inCafeTrd = ProcHora(lancto.InCafeTrd);
            TimeSpan fmCafeTrd = ProcHora(lancto.FmCafeTrd);

            TimeSpan totalDia = TimeSpan.Zero;

            // 1) Inicio da manhã até o inicio do café da manhã (só se tiver inicio do café da manhã)
            if (inMan != TimeSpan.Zero && (inCafeMan != TimeSpan.Zero || fmMan != TimeSpan.Zero))
            {
                if (inCafeMan != TimeSpan.Zero)
                    totalDia += inCafeMan - inMan;
                else
                    totalDia += fmMan - inMan;
            }

            // 2) Fim do café da manhã até a saída da manhã (só se tiver saída da manhã)
            if (fmCafeMan != TimeSpan.Zero && fmMan != TimeSpan.Zero)
            {
                totalDia += fmMan - fmCafeMan;
            }
            else if (fmMan != TimeSpan.Zero && inCafeMan != TimeSpan.Zero)
            {
                totalDia += fmMan - inCafeMan;
            }

            // 3) Inicio da tarde até o inicio do café da tarde (só se tiver inicio de café da tarde)
            if (inTrd != TimeSpan.Zero && (inCafeTrd != TimeSpan.Zero || fnTrd != TimeSpan.Zero))
            {
                if (inCafeTrd != TimeSpan.Zero)
                    totalDia += inCafeTrd - inTrd;
                else
                    totalDia += fnTrd - inTrd;
            }

            // 4) Fim do café da tarde até fim do turno (só se tiver fim do turno)
            if (fmCafeTrd != TimeSpan.Zero && fnTrd != TimeSpan.Zero)
            {
                totalDia += fnTrd - fmCafeTrd;
            }
            else if (fnTrd != TimeSpan.Zero && inCafeTrd != TimeSpan.Zero)
            {
                totalDia += fnTrd - inCafeTrd;
            }

            return totalDia;
        }

        private TimeSpan ParseTimeString(string timeString)
        {
            return TimeSpan.TryParse(timeString, out TimeSpan result) ? result : TimeSpan.Zero;
        }

        private void AdicionarLinhaLancamento(StringBuilder sb, Lanctos lancto)
        {
            string Data = glo.ComplStr(lancto.Data.ToString("dd/MM/yyyy"), 10, 2);
            string Nome = glo.ComplStr(lancto.Nome, 15, 2);
            string InMan = glo.ComplStr(lancto.InMan, 5, 2);
            string InCafeMan = glo.ComplStr(lancto.InCafeMan, 5, 2);
            string FmCafeMan = glo.ComplStr(lancto.FmCafeMan, 5, 2);
            string FmMan = glo.ComplStr(lancto.FmMan, 5, 2);
            string InTrd = glo.ComplStr(lancto.InTrd, 5, 2);
            string InCafeTrd = glo.ComplStr(lancto.InCafeTrd, 5, 2);
            string FmCafeTrd = glo.ComplStr(lancto.FmCafeTrd, 5, 2);
            string FnTrd = glo.ComplStr(lancto.FnTrd, 5, 2);
            string Total = glo.ComplStr(lancto.Total, 5, 2);

            if (string.IsNullOrEmpty(lancto.InMan) && string.IsNullOrEmpty(lancto.InTrd))
            {
                sb.AppendLine($"{Data} {Nome}                      F A L T A");
            }
            else if (string.IsNullOrEmpty(lancto.InMan))
            {
                sb.AppendLine($"{Data} {Nome}        F A L T A          {InTrd} {InCafeTrd} {FmCafeTrd} {FnTrd}    {Total}");
            }
            else if (string.IsNullOrEmpty(lancto.InTrd))
            {
                sb.AppendLine($"{Data} {Nome} {InMan} {InCafeMan} {FmCafeMan}   {FmMan}          F A L T A         {Total}");
            }
            else
            {
                sb.AppendLine($"{Data} {Nome} {InMan} {InCafeMan} {FmCafeMan}   {FmMan} {InTrd} {InCafeTrd} {FmCafeTrd} {FnTrd}    {Total}");
            }
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

        public class Lanctos
        {
            public int ID { get; set; }
            public string UID { get; set; }
            public DateTime Data { get; set; }
            public string Nome { get; set; }
            public string InMan { get; set; }
            public string FmMan { get; set; }
            public string InTrd { get; set; }
            public string FnTrd { get; set; }
            public string InCafeMan { get; set; }
            public string FmCafeMan { get; set; }
            public string InCafeTrd { get; set; }
            public string FmCafeTrd { get; set; }
            public int FuncID { get; set; }
            public string Total { get; set; }
        }

    }
}