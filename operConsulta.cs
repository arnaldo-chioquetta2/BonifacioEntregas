using SourceGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class Consultas : Form
    {
        public Consultas()
        {
            InitializeComponent();
        }

        private void operConsulta_Load(object sender, EventArgs e)
        {            
            DateTime DT1 = DateTime.Now;
            DateTime DT2 = DT1.AddMonths(-1);

            DT1 = new DateTime(2024, 1, 1);
            DT2 = new DateTime(2024,12, 31);

            CarregaGrid(DT1, DT2);
            ConfigurarGrid();
        }

        private void CarregaGrid(DateTime? DT1 = null, DateTime? DT2 = null)
        {
            EntregasDAO entregasDAO = new EntregasDAO();
            DataTable dados = entregasDAO.getDadosC(DT1, DT2);
            DataTable dados2 = OrganizarDadosEmDataTable(dados);
            DevAge.ComponentModel.BoundDataView boundDataView = new DevAge.ComponentModel.BoundDataView(dados2.DefaultView);
            dataGrid1.DataSource = boundDataView;
        }

        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Width = 100;
            for (int i = 1; i < dataGrid1.Columns.Count; i++)
            {
                dataGrid1.Columns[i].Width = 90;
            }
            dataGrid1.Invalidate();
        }

        private DataTable OrganizarDadosEmDataTable(DataTable dados)
        {
            DataTable dadosOrganizados = new DataTable();
            dadosOrganizados.Columns.Add("Forma de Pagamento", typeof(string));
            var formasPagamento = new Dictionary<int, string>
                {
                    { 0, "Anotado" },
                    { 1, "Cartão" },
                    { 2, "Dinheiro" },
                    { 3, "Pix" },
                    { 4, "Troca" }
                };
            foreach (var forma in formasPagamento)
            {
                dadosOrganizados.Rows.Add(forma.Value);
            }
            var motoBoys = dados.AsEnumerable()
                .Select(row => row.Field<string>("MotoBoy"))
                .Distinct()
                .Where(motoBoy => !string.IsNullOrEmpty(motoBoy)) 
                .ToList();
            foreach (var motoBoy in motoBoys)
            {
                dadosOrganizados.Columns.Add(motoBoy, typeof(string)); 
            }
            foreach (DataRow row in dados.Rows)
            {
                string motoBoy = row.Field<string>("MotoBoy");
                int idForma = row.Field<int>("idForma");
                decimal valor = row.Field<decimal>("Valor");
                DataRow linhaForma = dadosOrganizados.AsEnumerable().FirstOrDefault(r => r.Field<string>("Forma de Pagamento") == formasPagamento[idForma]);
                if (linhaForma != null && dadosOrganizados.Columns.Contains(motoBoy))
                {
                    decimal valorAtual = linhaForma.IsNull(motoBoy) ? 0 : decimal.Parse(linhaForma[motoBoy].ToString());
                    linhaForma[motoBoy] = (valorAtual + valor).ToString("N2"); // Formatação como número com duas casas decimais
                }
            }
            return dadosOrganizados;
        }

    }
}
