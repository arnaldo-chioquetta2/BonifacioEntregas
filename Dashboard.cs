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

        public Dashboard()
        {
            InitializeComponent();
            InicializarComponentesDeGrafico();
        }

        private void InicializarComponentesDeGrafico()
        {
            chartLucratividade = new Chart();
            chartLucratividade.Dock = DockStyle.Fill;
            this.Controls.Add(chartLucratividade);

            ChartArea chartArea = new ChartArea();
            chartLucratividade.ChartAreas.Add(chartArea);
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            DateTime DT1 = DateTime.Now.AddDays(-30); // Exemplo: último mês
            DateTime DT2 = DateTime.Now;
            CarregarDadosDoGrafico(DT1, DT2);
        }

        private void CarregarDadosDoGrafico(DateTime DT1, DateTime DT2)
        {
            // Limpar séries e áreas de gráfico existentes
            chartLucratividade.Series.Clear();
            chartLucratividade.ChartAreas.Clear();

            // Adicionar uma nova área de gráfico
            ChartArea chartArea = new ChartArea();
            chartLucratividade.ChartAreas.Add(chartArea);

            // Adicionar a série "Lucro"
            Series seriesLucro = new Series("Lucro")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Green // Cor padrão para lucro positivo
            };
            chartLucratividade.Series.Add(seriesLucro);

            // Carregar os dados
            EntregasDAO entregasDAO = new EntregasDAO();
            DataTable dados = entregasDAO.Dasboard(DT1, DT2);

            // Verifica se há dados para mostrar
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

                // Configurar a área do gráfico
                chartLucratividade.ChartAreas[0].AxisX.Interval = 1;
                chartLucratividade.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                chartLucratividade.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
                chartLucratividade.ChartAreas[0].AxisY.LabelStyle.Format = "C2";

                // Atualizar o gráfico
                chartLucratividade.Invalidate();
            }
            else
            {
                MessageBox.Show("Não há dados para o período selecionado.");
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            DateTime Agora = DateTime.Now;
            int Ano = Agora.Year;
            int Mes = Agora.Month;
            int Dia = Agora.Day + 1;
            DateTime DT1 = new DateTime(Ano, 1, 1);
            DateTime DT2 = DateTime.Now;
            if (gen.IsDateTimeValid(Ano, Mes, Dia))
            {
                DT2 = new DateTime(Ano, Mes, Dia);
            }
            else
            {
                Mes++;
                if (gen.IsDateTimeValid(Ano, Mes, Dia))
                {
                    Ano++;
                    Mes = 1;
                    DT2 = new DateTime(Ano, Mes, 1);
                }
            }
            dtpDataIniicio.Value = DT1;
            dtpDataFim.Value = DT2.AddDays(-1);
            CarregarDadosDoGrafico(DT1, DT2);
        }
    }
}
