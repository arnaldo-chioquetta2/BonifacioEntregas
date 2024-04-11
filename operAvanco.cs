using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class operAvanco : Form
    {

        private Chart chartLucratividade;
        private Chart chartVendas;
        private EntregasDAO entregasDAO;
        private DataGridView dataGridView1;
        private int indiceSelecionado = -1;
        private INI cINI;

        public operAvanco()
        {
            InitializeComponent();
            entregasDAO = new EntregasDAO();
            dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(dataGridView1);
            InicializarComponentesDeGrafico();
        }

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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Checked)
            {
                foreach (Control control in radioButton.Parent.Controls)
                {
                    if (control is RadioButton otherRadioButton && otherRadioButton != radioButton)
                    {
                        otherRadioButton.Checked = false;
                    }
                }
            }
        }

        private void operAvanco_Load(object sender, EventArgs e)
        {
            cINI = new INI();
            indiceSelecionado = cINI.ReadInt("Opcoes", "ModoSelAvanco", 0);
            switch (indiceSelecionado)
            {
                case 0:
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    break;
                case 1:
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                    radioButton3.Checked = false;
                    break;
                case 2:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;
                    break;

            }
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
            CarregaGrafEntregas(DT1, DT2, indiceSelecionado);
            CarregaGrafClientes(DT1, DT2, indiceSelecionado);
        }

        private void VeIndice()
        {
            if (radioButton1.Checked)
            {
                indiceSelecionado = 0;
            }
            else if (radioButton2.Checked)
            {
                indiceSelecionado = 1;
            }
            else if (radioButton3.Checked)
            {
                indiceSelecionado = 2;
            }
            cINI.WriteInt("Opcoes", "ModoSelAvanco", indiceSelecionado);
        }

        private void CarregaGrafEntregas(DateTime DT1, DateTime DT2, int indiceSelecionado)
        {
            chartVendas.Series.Clear();
            chartVendas.ChartAreas.Clear();
            ChartArea chartArea = new ChartArea();
            chartVendas.ChartAreas.Add(chartArea);
            Series seriesVendas = new Series("Entregas")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Green
            };
            chartVendas.Series.Add(seriesVendas);
            DataTable dados = entregasDAO.getEntregas(DT1, DT2, indiceSelecionado);
            int QtdTotal = 0;
            if (dados.Rows.Count > 0)
            {
                foreach (DataRow row in dados.Rows)
                {
                    DateTime data = Convert.ToDateTime(row["Dado"]);
                    int quantidadeEntregas = Convert.ToInt32(row["QTD"]);
                    QtdTotal += quantidadeEntregas;
                    chartVendas.Series["Entregas"].Points.AddXY(data.ToShortDateString(), quantidadeEntregas);
                }
                if (indiceSelecionado == 1)
                {
                    chartVendas.ChartAreas[0].AxisX.Interval = 1;
                    chartVendas.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Weeks;
                    chartVendas.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
                }
                else
                {
                    chartVendas.ChartAreas[0].AxisX.Interval = 1;
                    chartVendas.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    chartVendas.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
                }
                chartVendas.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
                chartVendas.Invalidate();
            }
            txtTotalEntregadores.Text = QtdTotal.ToString();
        }

        private void CarregaGrafClientes(DateTime dT1, DateTime dT2, int indiceSelecionado)
        {
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VeIndice();
            CarregaGrafEntregas(dtpDataIniicio.Value, dtpDataFim.Value, indiceSelecionado);
        }
    }
}

