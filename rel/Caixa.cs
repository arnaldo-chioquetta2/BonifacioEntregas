using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio.rel
{
    public partial class Caixa : Form
    {

        private bool ativou = false;
        public string txtForma="";

        private List<Lanctos> relcaixa { get; set; }
        public DateTime DT1 { get; set; }
        public DateTime DT2 { get; set; }
        public int Forma { get; internal set; }

        public Caixa()
        {
            InitializeComponent();
            SetStartPosition();
            FormasDAO cFormas = new FormasDAO();
            glo.CarregarComboBox<tb.Forma>(cmbTipo, cFormas, " ");
            rt.AdjustFormComponents(this);
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        // Refatorado em 04/09/24 Original 71 linhas, resultado 12 linhas
        private List<Lanctos> CarregaCaixa()
        {
            DateTime dataInicio = this.DT1.Date;
            DateTime dataFim = this.DT2.Date.AddDays(1).AddSeconds(-1); 
            string filtroForma = ObterFiltroForma();
            string SQL = $@"SELECT C.ID, C.Data, C.Valor, C.Desconto, 
                C.idForma AS FormaPagto, C.Obs, F.Tipo AS FormaTipo
                FROM Caixa C                
                LEFT JOIN Formas f ON F.ID = (C.idForma + 1)
                WHERE C.Data BETWEEN #{dataInicio:yyyy-MM-dd HH:mm:ss}# AND #{dataFim:yyyy-MM-dd HH:mm:ss}#{filtroForma}";
            return ExecutarConsulta(SQL);
        }

        private string ObterFiltroForma()
        {
            if (cmbTipo.SelectedIndex > 0)
            {
                int idFormaSelecionada = ((tb.ComboBoxItem)cmbTipo.SelectedItem).Id;
                return $" AND C.idForma = {idFormaSelecionada - 1}";
            }
            return "";
        }

        private List<Lanctos> ExecutarConsulta(string SQL)
        {
            List<Lanctos> lancamentos = new List<Lanctos>();
            Dictionary<int, int> mapaFormas = CriarMapaFormas();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                //try
                //{
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(SQL, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var lancamento = CriarLancamento(reader, mapaFormas);
                                lancamentos.Add(lancamento);
                            }
                        }
                    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.ToString());
                //    return null;
                //}
            }
            return lancamentos;
        }

        private Lanctos CriarLancamento(OleDbDataReader reader, Dictionary<int, int> mapaFormas)
        {
            int idFormaCaixa = Convert.ToInt32(reader["FormaPagto"]);
            int idFormaReal = mapaFormas.ContainsKey(idFormaCaixa) ? mapaFormas[idFormaCaixa] : idFormaCaixa + 1;
            var lancamento = new Lanctos
            {
                ID = Convert.ToInt32(reader["ID"]),
                DataPagamento = Convert.ToDateTime(reader["Data"]),
                Desconto = Convert.ToDecimal(reader["Desconto"]),
                idFormaPagto = idFormaReal,
                Entrada = 0,
                Saida = 0,
                Obs = reader["Obs"].ToString()
            };
            decimal valor = Convert.ToDecimal(reader["Valor"]);
            int formaTipo = Convert.ToInt32(reader["FormaTipo"]);
            if (formaTipo == 1)
            {
                lancamento.Saida = valor;
            }
            else
            {
                lancamento.Entrada = valor;
            }
            lancamento.Saldo = lancamento.Entrada - lancamento.Desconto - lancamento.Saida;
            return lancamento;
        }        

        public void GerarRelCaixa()
        {
            int? formaPagamentoFiltro = null;
            if (cmbTipo.SelectedIndex > 0)
            {
                formaPagamentoFiltro = ((tb.ComboBoxItem)cmbTipo.SelectedItem).Id;
            }
            this.DT1 = dtpDataIN.Value.Date;
            this.DT2 = dtnDtFim.Value.Date.AddHours(23).AddMinutes(59);
            var relcaixa = CarregaCaixa();
            var formas = CarregaFormas();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Extrato de Movimentação do Caixa");
            sb.AppendLine();
            sb.AppendLine($"Período: {this.DT1:dd/MM/yyyy} a {this.DT2:dd/MM/yyyy}");

            var formaSelecionada = formaPagamentoFiltro.HasValue
                ? formas.FirstOrDefault(f => f.Id == formaPagamentoFiltro.Value)
                : null;

            if (formaSelecionada != null)
            {
                sb.AppendLine($"Forma de Pagamento: {formaSelecionada.Nome}");
            }

            sb.AppendLine();
            sb.AppendLine("ID       Data     |  Entrada | Desconto |  Saídas |  FormaPagto |   Valor   | Observação");

            decimal totalEntradas = 0m;
            decimal totalSaidas = 0m;

            foreach (var lancos in relcaixa)
            {
                var formaPagamento = formas.FirstOrDefault(f => f.Id == lancos.idFormaPagto);
                if (formaPagamento == null) continue;

                if (!formaPagamentoFiltro.HasValue || lancos.idFormaPagto == formaPagamentoFiltro.Value)
                {
                    string ID = glo.ComplStr(lancos.ID.ToString(), 5, 2);
                    string Data = glo.ComplStr(lancos.DataPagamento.ToString("dd/MM/yyyy"), 10, 2);
                    string Entrada = glo.ComplStr(lancos.Entrada.ToString("N2"), 8, 3);
                    string Desconto = glo.ComplStr(lancos.Desconto.ToString("N2"), 8, 3);
                    string Saidas = glo.ComplStr(lancos.Saida.ToString("N2"), 7, 3);
                    string Forma = glo.ComplStr(formaPagamento.Nome, 11, 2);
                    string Valor = glo.ComplStr((lancos.Entrada - lancos.Desconto - lancos.Saida).ToString("N2"), 9, 2);
                    string Obs = lancos.Obs.Substring(0, Math.Min(lancos.Obs.Length, 20));

                    sb.AppendLine($"{ID}   {Data}   {Entrada}   {Desconto}   {Saidas}   {Forma}   {Valor} {Obs}");

                    totalEntradas += lancos.Entrada;
                    totalSaidas += lancos.Saida;
                }
            }
            sb.AppendLine();

            decimal totalLiquido = totalEntradas - totalSaidas;

            if (formaSelecionada != null)
            {
                string nome = formaSelecionada.Nome.PadRight(11);
                string valorFormatado = Math.Abs(totalLiquido).ToString("N2").PadLeft(12);
                sb.AppendLine($"{nome}: {valorFormatado}");
            }
            else
            {
                GerarTotaisFormas(sb, relcaixa, formas);
            }

            sb.AppendLine("");
            sb.AppendLine("Total de entradas:" + glo.ComplStr(totalEntradas.ToString("N2"), 10, 2));
            sb.AppendLine("Total de saídas  :" + glo.ComplStr(totalSaidas.ToString("N2"), 10, 2));
            sb.AppendLine("Saldo            :" + glo.ComplStr(totalLiquido.ToString("N2"), 10, 2));

            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void GerarTotaisFormas(StringBuilder sb, List<Lanctos> relcaixa, List<tb.Forma> formas)
        {
            var totaisPorForma = formas.ToDictionary(f => f.Id, _ => 0m);

            foreach (var lancos in relcaixa)
            {
                var formaPagamento = formas.FirstOrDefault(f => f.Id == lancos.idFormaPagto);
                if (formaPagamento != null)
                {
                    if (formaPagamento.Tipo == 0) // Entrada
                    {
                        totaisPorForma[formaPagamento.Id] += lancos.Entrada - lancos.Desconto;
                    }
                    else if (formaPagamento.Tipo == 1) // Saída
                    {
                        totaisPorForma[formaPagamento.Id] += lancos.Saida;
                    }
                }
            }

            int maxNomeLength = formas.Max(f => f.Nome.Length);

            foreach (var forma in formas)
            {
                decimal total = totaisPorForma[forma.Id];
                string nome = forma.Nome.PadRight(maxNomeLength);
                string valorFormatado = total.ToString("N2").PadLeft(12);

                sb.AppendLine($"{nome}: {valorFormatado}");
            }
        }        

        private Dictionary<int, int> CriarMapaFormas()
        {
            Dictionary<int, int> mapa = new Dictionary<int, int>();
            string SQL = "SELECT ID FROM Formas ORDER BY ID";

            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(SQL, connection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        int index = 0;
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["ID"]);
                            mapa[index] = id;
                            index++;
                        }
                    }
                }
            }
            return mapa;
        }

        private List<tb.Forma> CarregaFormas()
        {
            string SQL = "SELECT ID, Nome, Tipo, Ativo FROM Formas WHERE Ativo = 1";
            List<tb.Forma> formas = new List<tb.Forma>();

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
                                formas.Add(new tb.Forma
                                {
                                    Id = (int)reader["ID"],
                                    Nome = (string)reader["Nome"],
                                    Tipo = (int)reader["Tipo"],
                                    Ativo = (int)reader["Ativo"]
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return new List<tb.Forma>();
                }
            }
            return formas;
        }        

        private void Extrato_Activated(object sender, EventArgs e)
        {
            if (!ativou)
            {
                ativou = true;
                dtpDataIN.Value = this.DT1;
                dtnDtFim.Value = this.DT2.Date.AddDays(1).AddMinutes(-1);
                if (this.Forma>-1)
                {
                    cmbTipo.Text = this.txtForma;
                } 
                GerarRelCaixa();
            }
        }
        
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            GerarRelCaixa();
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

    }

}