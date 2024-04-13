using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TeleBonifacio.dao;

namespace TeleBonifacio
{

    public partial class operAvanc : Form
    {
        private Chart chartEntregas;
        private Chart chartClientes;
        EntregasDAO entregasDAO;        
        private DataGridView dataGridView1;
        private int indiceSelecionado = -1;
        private INI cINI;

        private void InicializarComponentesDeGrafico()
        {
            chartEntregas = new Chart();
            chartEntregas.Dock = DockStyle.Fill;
            ChartArea chartArea = new ChartArea();
            chartEntregas.ChartAreas.Add(chartArea);
            Series seriesLucro = new Series("Entregas")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Green
            };
            chartEntregas.Series.Add(seriesLucro);
            tabPage1.Controls.Add(chartEntregas);
            chartClientes = new Chart();
            chartClientes.Dock = DockStyle.Fill;
            ChartArea chartAreaVendas = new ChartArea();
            chartClientes.ChartAreas.Add(chartAreaVendas);
            Series seriesVendas = new Series("Clientes")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Blue
            };
            chartClientes.Series.Add(seriesVendas);
            tabPage2.Controls.Add(chartClientes);
        }

        public operAvanc()
        {
            InitializeComponent();
            InicializarComponentesDeGrafico();
            entregasDAO = new EntregasDAO();
            dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(dataGridView1);
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            InicializaChecks();
            ChamaGraficos();
        }

        private void CarregaGrafVendas()
        {
            DataTable dados = entregasDAO.getQuantidades(dtpDataIniicio.Value, dtpDataFim.Value, indiceSelecionado, "Clientes");
            int QtdTotal = 0;
            if (dados.Rows.Count > 0)
            {

                foreach (DataRow row in dados.Rows)
                {
                    Console.WriteLine("Dado: " + row["Dado"] + ", QTD: " + row["QTD"]);
                    DateTime data = Convert.ToDateTime(row["Dado"]);
                    int quantidades = (int)row["QTD"];
                    QtdTotal += quantidades;
                    chartClientes.Series["Clientes"].Points.AddXY(data.ToShortDateString(), quantidades);
                }

                if (indiceSelecionado == 1)
                {
                    chartClientes.ChartAreas[0].AxisX.Interval = 1;
                    chartClientes.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Weeks;
                    chartClientes.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
                }
                else
                {
                    chartClientes.ChartAreas[0].AxisX.Interval = 1;
                    chartClientes.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    chartClientes.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
                }
                chartClientes.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
                chartClientes.Invalidate();
            }
            lbClientes.Text = QtdTotal.ToString();
        }

        private void CarregaGrafEntregadores()
        {
            DataTable dados = entregasDAO.getQuantidades(dtpDataIniicio.Value, dtpDataFim.Value, indiceSelecionado, "Entregas");
            int QtdTotal = 0;
            if (dados.Rows.Count > 0)
            {
                foreach (DataRow row in dados.Rows)
                {
                    DateTime data = Convert.ToDateTime(row["Dado"]);
                    int quantidades = Convert.ToInt32(row["QTD"]);
                    QtdTotal += quantidades;
                    chartEntregas.Series["Entregas"].Points.AddXY(data.ToShortDateString(), quantidades);
                }
                if (indiceSelecionado == 1)
                {
                    chartEntregas.ChartAreas[0].AxisX.Interval = 1;
                    chartEntregas.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Weeks;
                    chartEntregas.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
                }
                else
                {
                    chartEntregas.ChartAreas[0].AxisX.Interval = 1;
                    chartEntregas.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                    chartEntregas.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM";
                }
                chartEntregas.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
                chartEntregas.Invalidate();
            }
            txtTotalEntregadores.Text = QtdTotal.ToString();
        }

        private void InicializaChecks()
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            VeIndice();
            chartEntregas.Series["Entregas"].Points.Clear();
            chartClientes.Series["Clientes"].Points.Clear();
            ChamaGraficos();
        }

        private void ChamaGraficos()
        {
            CarregaGrafEntregadores();
            CarregaGrafVendas();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
