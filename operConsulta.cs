using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class Consultas : Form
    {

        private decimal totalGeral = 0;
        private DataTable Dados = null;

        public Consultas()
        {
            InitializeComponent();
            
        }

        private void operConsulta_Load(object sender, EventArgs e)
        {
            DateTime Agora = DateTime.Now;
            int Ano = Agora.Year;
            int Mes = Agora.Month;
            int Dia = Agora.Day+1;
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
            dtpDataFim.Value = DT2;
            CarregaGrid(DT1, DT2);
            ConfigurarGrid(); 
        }

        private void CarregaGrid(DateTime? DT1 = null, DateTime? DT2 = null)
        {
            EntregasDAO entregasDAO = new EntregasDAO();
            DataTable DadosTemp = entregasDAO.getDadosC(DT1, DT2);
            if (DadosTemp.Rows.Count == 0)
            {
                dataGrid1.DataSource = null;
            } else
            {
                Dados = OrganizarDadosEmDataTable(DadosTemp);
                DevAge.ComponentModel.BoundDataView boundDataView = new DevAge.ComponentModel.BoundDataView(Dados.DefaultView);
                dataGrid1.DataSource = boundDataView;
            }
        }

        private void ConfigurarGrid()
        {
            try
            {
                dataGrid1.Columns[0].Width = 100;
                for (int i = 1; i < dataGrid1.Columns.Count; i++)
                {
                    dataGrid1.Columns[i].Width = 90;
                }
                dataGrid1.Invalidate();
            }
            catch (Exception)
            {
                // Não faz nada
            }
        }

#region Grid

        private DataTable OrganizarDadosEmDataTable(DataTable dados)
        {
            var dadosOrganizados = InicializarDataTable();
            var formasPagamento = ObterFormasDePagamento();
            AdicionarFormasDePagamento(dadosOrganizados, formasPagamento);
            var motoBoys = ObterMotoBoys(dados);
            AdicionarColunasMotoBoys(dadosOrganizados, motoBoys);
            var totaisEntregadores = CalcularValoresPorEntregador(dados, dadosOrganizados, formasPagamento, motoBoys);
            AdicionarLinhaTotal(dadosOrganizados, totaisEntregadores);
            AtualizarValorTotalInterface(totaisEntregadores);
            return dadosOrganizados;
        }

        private DataTable InicializarDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Forma de Pagamento", typeof(string));
            return dataTable;
        }

        private Dictionary<int, string> ObterFormasDePagamento()
        {
            return new Dictionary<int, string>
            {
                { 0, "Anotado" },
                { 1, "Cartão" },
                { 2, "Dinheiro" },
                { 3, "Pix" },
                { 4, "Troca" }
            };
        }

        private void AdicionarFormasDePagamento(DataTable dataTable, Dictionary<int, string> formasPagamento)
        {
            foreach (var forma in formasPagamento)
            {
                dataTable.Rows.Add(forma.Value);
            }
        }

        private List<string> ObterMotoBoys(DataTable dados)
        {
            return dados.AsEnumerable()
                .Select(row => row.Field<string>("MotoBoy"))
                .Distinct()
                .Where(motoBoy => !string.IsNullOrEmpty(motoBoy))
                .ToList();
        }

        private void AdicionarColunasMotoBoys(DataTable dataTable, List<string> motoBoys)
        {
            foreach (var motoBoy in motoBoys)
            {
                dataTable.Columns.Add(motoBoy, typeof(string));
            }
        }

        private Dictionary<string, decimal> CalcularValoresPorEntregador(DataTable dados, DataTable dadosOrganizados, Dictionary<int, string> formasPagamento, List<string> motoBoys)
        {
            var totaisEntregadores = motoBoys.ToDictionary(motoBoy => motoBoy, motoBoy => 0M);
            foreach (DataRow row in dados.Rows)
            {
                string motoBoy = row.Field<string>("MotoBoy");
                int idForma = row.Field<int>("idForma");
                decimal valor = row.Field<decimal>("Valor");
                DataRow linhaForma = dadosOrganizados.AsEnumerable().FirstOrDefault(r => r.Field<string>("Forma de Pagamento") == formasPagamento[idForma]);
                if (linhaForma != null && dadosOrganizados.Columns.Contains(motoBoy))
                {
                    decimal valorAtual = linhaForma.IsNull(motoBoy) ? 0 : decimal.Parse(linhaForma[motoBoy].ToString());
                    decimal valorSomado = valorAtual + valor;
                    linhaForma[motoBoy] = valorSomado.ToString("N2");
                    totaisEntregadores[motoBoy] += valor;
                }
            }
            return totaisEntregadores;
        }

        private void AdicionarLinhaTotal(DataTable dataTable, Dictionary<string, decimal> totaisEntregadores)
        {
            DataRow linhaTotal = dataTable.NewRow();
            linhaTotal["Forma de Pagamento"] = "TOTAL";
            foreach (var motoBoy in totaisEntregadores.Keys)
            {
                linhaTotal[motoBoy] = totaisEntregadores[motoBoy].ToString("N2");
            }
            dataTable.Rows.Add(linhaTotal);
        }

        private void AtualizarValorTotalInterface(Dictionary<string, decimal> totaisEntregadores)
        {
            totalGeral = totaisEntregadores.Values.Sum();
            txtValor.Text = totalGeral.ToString("N2");
            ConfigDAO config = new ConfigDAO();
            decimal perc = config.getPercentual();
            AtualizaValores(perc);
        }

        #endregion

        private void AtualizaValores(decimal perc)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGrid1.DataSource = null;
            dataGrid1.Columns.Clear();            
            CarregaGrid(dtpDataIniicio.Value, dtpDataFim.Value);
            ConfigurarGrid();
        }

        private void txPerc_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txPerc_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}

