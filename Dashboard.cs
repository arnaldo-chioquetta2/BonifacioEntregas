using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TeleBonifacio.dao;

namespace TeleBonifacio
{

    public partial class Dashboard : Form
    {
        private Chart chartLucratividade;
        private Chart chartVendas;
        private EntregasDAO entregasDAO;        
        private DataGridView dataGridView1;
        private bool MostrouMedias = false;

        private void InicializarComponentesDeGrafico()
        {
            chartLucratividade = new Chart();
            chartLucratividade.Dock = DockStyle.Fill;
            ChartArea chartArea = new ChartArea();
            chartLucratividade.ChartAreas.Add(chartArea);
            Series seriesLucro = new Series("Lucro")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Green
            };
            chartLucratividade.Series.Add(seriesLucro);
            tabPage1.Controls.Add(chartLucratividade);
            chartVendas = new Chart();
            chartVendas.Dock = DockStyle.Fill;
            ChartArea chartAreaVendas = new ChartArea();
            chartVendas.ChartAreas.Add(chartAreaVendas);
            Series seriesVendas = new Series("Vendas")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Blue
            };
            chartVendas.Series.Add(seriesVendas);
            tabPage2.Controls.Add(chartVendas);
        }
        
        public Dashboard()
        {
            InitializeComponent();
            InicializarComponentesDeGrafico();
            entregasDAO = new EntregasDAO();
            dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(dataGridView1);
            if (rt.IsLargeScreen())
                this.WindowState = FormWindowState.Maximized;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            DateTime Agora = DateTime.Now;
            int Ano = Agora.Year;
            int Mes = Agora.Month;
            int Dia = Agora.Day + 1;
            DateTime DT1 = new DateTime(Ano, 1, 1);
            DateTime DT2 = DateTime.Now;
            if (glo.IsDateTimeValid(Ano, Mes, Dia))
            {
                DT2 = new DateTime(Ano, Mes, Dia);
            }
            else
            {
                Mes++;
                if (glo.IsDateTimeValid(Ano, Mes, Dia))
                {
                    Ano++;
                    Mes = 1;
                    DT2 = new DateTime(Ano, Mes, 1);
                }
            }
            dtpDataIniicio.Value = DT1;
            dtpDataFim.Value = DT2.AddDays(-1);
            CarregaGrafEntregadores(DT1, DT2);
            CarregaGrafVendas(DT1, DT2);            
        }

        // Refatorado em 09/09/24 Original 34 linhas, resultado 15 linhas
        private void CarregaGrafVendas(DateTime DT1, DateTime DT2)
        {
            ConfiguraGrafico();
            DataTable dados = entregasDAO.GraficVendas(DT1, DT2);
            if (dados.Rows.Count > 0)
            {
                AdicionaPontosGrafico(dados);
                ConfiguraEixosGrafico();
                chartVendas.Invalidate();
            }
            else
            {
                MessageBox.Show("Não há dados para o período selecionado.");
            }
        }

        private void ConfiguraGrafico()
        {
            chartVendas.Series.Clear();
            chartVendas.ChartAreas.Clear();
            ChartArea chartArea = new ChartArea();
            chartVendas.ChartAreas.Add(chartArea);
            Series seriesVendas = new Series("Vendas")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Blue
            };
            chartVendas.Series.Add(seriesVendas);
        }

        private void AdicionaPontosGrafico(DataTable dados)
        {
            foreach (DataRow row in dados.Rows)
            {
                DateTime data = Convert.ToDateTime(row["DataFormatada"]);
                decimal totalVendas = Convert.ToDecimal(row["TotalVendas"]);
                chartVendas.Series["Vendas"].Points.AddXY(data.ToShortDateString(), totalVendas);
                int pointIndex = chartVendas.Series["Vendas"].Points.Count - 1;
                chartVendas.Series["Vendas"].Points[pointIndex].Color = totalVendas >= 0 ? Color.Blue : Color.Red;
            }
        }

        private void ConfiguraEixosGrafico()
        {
            chartVendas.ChartAreas[0].AxisX.Interval = 1;
            chartVendas.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chartVendas.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
            chartVendas.ChartAreas[0].AxisY.LabelStyle.Format = "C2";
        }

        private void CarregaGrafEntregadores(DateTime DT1, DateTime DT2)
        {
            chartLucratividade.Series.Clear();
            chartLucratividade.ChartAreas.Clear();
            ChartArea chartArea = new ChartArea();
            chartLucratividade.ChartAreas.Add(chartArea);
            Series seriesLucro = new Series("Lucro")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Green 
            };
            chartLucratividade.Series.Add(seriesLucro);
            DataTable dados = entregasDAO.GraficEntregadores(DT1, DT2);
            if (dados.Rows.Count > 0)
            {
                foreach (DataRow row in dados.Rows)
                {
                    DateTime data = Convert.ToDateTime(row["DataTruncada"]);
                    decimal lucroTeleentrega = Convert.ToDecimal(row["LucroTeleentrega"]);
                    chartLucratividade.Series["Lucro"].Points.AddXY(data.ToShortDateString(), lucroTeleentrega);
                    int pointIndex = chartLucratividade.Series["Lucro"].Points.Count - 1;
                    chartLucratividade.Series["Lucro"].Points[pointIndex].Color = lucroTeleentrega >= 0 ? Color.Green : Color.Red;
                }
                chartLucratividade.ChartAreas[0].AxisX.Interval = 1;
                chartLucratividade.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                chartLucratividade.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
                chartLucratividade.ChartAreas[0].AxisY.LabelStyle.Format = "C2";
                chartLucratividade.Invalidate();
            }
            else
            {
                MessageBox.Show("Não há dados para o período selecionado.");
            }
            AtualizarCamposDeTextoComValoresTotais(dados);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CarregaGrafEntregadores(dtpDataIniicio.Value, dtpDataFim.Value);
            CarregaGrafVendas(dtpDataIniicio.Value, dtpDataFim.Value);
        }

        private void AtualizarCamposDeTextoComValoresTotais(DataTable dados)
        {
            decimal totalVendas = 0m;
            decimal totalEntregadores = 0m;
            decimal lucroTotal = 0m;
            decimal comiss = 0m;
            if (dados.Rows.Count > 0)
            {
                foreach (DataRow row in dados.Rows)
                {
                    decimal VlNota = Convert.ToDecimal(row["VlNota"]);
                    totalVendas += VlNota;
                    totalEntregadores += row.IsNull("ValorTotalEntrega") ? 0m : Convert.ToDecimal(row["ValorTotalEntrega"]);
                    lucroTotal += row.IsNull("LucroTeleentrega") ? 0m : Convert.ToDecimal(row["LucroTeleentrega"]);
                    comiss += VlNota / 100;
                }
            }
            txtTotalVendas.Text = totalVendas.ToString("C");
            txtTotalEntregadores.Text = totalEntregadores.ToString("C");
            txtLucroTotal.Text = lucroTotal.ToString("C");
            txtComiss.Text = comiss.ToString("C");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex==2)
            {
                if (!MostrouMedias)
                {
                    MostrouMedias = true;
                    CarragaMedias();
                }                
            }
        }

        #region Medias

        private void CarragaMedias()
        {
            this.dtgMedias.Rows.Add(3); // Adiciona 3 linhas
            this.dtgMedias.Columns[0].Width = 400; // Largura da primeira coluna
            this.dtgMedias.Columns[1].Width = 400; // Largura da segunda coluna
            this.dtgMedias.RowTemplate.Height = 100; // Altura das linhas

            // Definindo valores para a primeira coluna
            this.dtgMedias.Rows[0].Cells[0].Value = "1 ano ";
            this.dtgMedias.Rows[1].Cells[0].Value = "6 meses ";
            this.dtgMedias.Rows[2].Cells[0].Value = "3 meses ";

            this.dtgMedias.Rows[0].Cells[1].Value = Cons1ano();
            this.dtgMedias.Rows[1].Cells[1].Value = Cons6meses();
            this.dtgMedias.Rows[2].Cells[1].Value = Cons3meses();

            // Estilo da fonte para ambas as colunas
            DataGridViewCellStyle leftColumnStyle = new DataGridViewCellStyle();
            leftColumnStyle.Font = new Font("Arial", 36F, GraphicsUnit.Pixel); // Define uma fonte maior para a primeira coluna
            leftColumnStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // Alinhamento à esquerda para a primeira coluna

            DataGridViewCellStyle rightColumnStyle = new DataGridViewCellStyle();
            rightColumnStyle.Font = new Font("Arial", 36F, GraphicsUnit.Pixel); // Define o mesmo tamanho de fonte para a segunda coluna
            rightColumnStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Alinhamento à direita para a segunda coluna

            // Aplicando o estilo nas colunas
            this.dtgMedias.Columns[0].DefaultCellStyle = rightColumnStyle;
            this.dtgMedias.Columns[1].DefaultCellStyle = leftColumnStyle; 
        }

        private object Cons3meses()
        {
            string sql  = "SELECT AVG(CDbl(VlNota)) AS Media FROM Entregas WHERE Data >= DateAdd('m', -3, Date())";
            return cons(sql);
        }

        private object Cons6meses()
        {
            string sql = "SELECT AVG(CDbl(VlNota)) AS Media FROM Entregas WHERE Data >= DateAdd('m', -6, Date())";
            return cons(sql);
        }

        private string Cons1ano()
        {
            string sql = "SELECT AVG(CDbl(VlNota)) AS Media FROM Entregas WHERE Data >= DateAdd('yyyy', -1, Date())";
            return cons(sql);
        }

        private string cons(string sql)
        {
            DataTable dt = DB.ExecutarConsulta(sql);
            double vlr = (double)dt.Rows[0]["Media"];
            string sVlr = " R$ " + vlr.ToString("N2");
            return sVlr;
        }

        #endregion

    }
}

/*
1) Primeiramente, o programa busca informações sobre as entregas feitas dentro de um período de tempo específico. 
2) Ele procura saber a data de cada entrega e o lucro que foi gerado com ela.

Sendo que o lucro considerado é o valor da venda, menos desconto e menos valor pago pela entrega.

3) Depois de coletar esses dados, o programa organiza as informações por dia. 
Isso significa que ele junta todas as entregas que foram feitas em um mesmo dia e calcula o lucro total daquele dia.
4) Com essas informações organizadas, o programa cria um gráfico. 
Cada barra no gráfico representa um dia, e a altura da barra mostra o lucro gerado naquele dia. 
As barras verdes representam dias em que houve lucro, enquanto as barras vermelhas representam dias com prejuízo.
 */ 