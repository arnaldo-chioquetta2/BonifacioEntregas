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
            DateTime DT2 = DT1.AddDays(-30);
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
            dadosOrganizados.Columns.Add("Forma de Pagto", typeof(string));
            var entregadores = dados.AsEnumerable().Select(row => row.Field<string>("MotoBoy")).Distinct().ToList();
            foreach (var entregador in entregadores)
            {
                dadosOrganizados.Columns.Add(entregador, typeof(string));
            }
            var gruposFormaPagamento = dados.AsEnumerable()
                .GroupBy(row => row.Field<string>("Pagamento"))
                .Select(group => new
                {
                    FormaPagamento = group.Key,
                    EntregadoresTotais = entregadores.ToDictionary(entregador => entregador, entregador => group.Where(row => row.Field<string>("MotoBoy") == entregador).Sum(row => Convert.ToDecimal(row["Valor"])))
                }).ToList();
            foreach (var grupo in gruposFormaPagamento)
            {
                DataRow newRow = dadosOrganizados.NewRow();
                newRow["Forma de Pagto"] = grupo.FormaPagamento;
                foreach (var entregador in entregadores)
                {
                    newRow[entregador] = grupo.EntregadoresTotais.ContainsKey(entregador) ? gen.ConvObjStrFormatado(grupo.EntregadoresTotais[entregador]) : "0";
                }
                dadosOrganizados.Rows.Add(newRow);
            }
            DataRow totalRow = dadosOrganizados.NewRow();
            totalRow["Forma de Pagto"] = "Total";
            foreach (var entregador in entregadores)
            {
                totalRow[entregador] = gen.ConvObjStrFormatado(gruposFormaPagamento.Sum(g => g.EntregadoresTotais.ContainsKey(entregador) ? g.EntregadoresTotais[entregador] : 0M));
            }
            dadosOrganizados.Rows.Add(totalRow);
            return dadosOrganizados;
        }

    }
}
