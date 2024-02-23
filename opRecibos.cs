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
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class opRecibos : Form
    {

        VendedoresDAO Vendedor;

        public opRecibos()
        {
            InitializeComponent();            
        }

        private void opRecibos_Load(object sender, EventArgs e)
        {
            Vendedor = new VendedoresDAO();
            PopulaVendedores();
            CarregaGrid();
        }

        private void CarregaGrid()
        {
            ReciboDAO Recibo = new ReciboDAO();
            DataTable Dados = Recibo.ValoresAPagar();
            if (Dados.Rows.Count == 0)
            {
                dataGrid1.DataSource = null;
            }
            else
            {
                DataTable DadosInvertidos = InverteLinhasColunas(Dados);
                DevAge.ComponentModel.BoundDataView boundDataView = new DevAge.ComponentModel.BoundDataView(DadosInvertidos.DefaultView);
                dataGrid1.DataSource = boundDataView;
            }
        }

        private DataTable InverteLinhasColunas(DataTable dataTableOriginal)
        {
            DataTable dataTableInvertido = new DataTable();
            foreach (DataRow row in dataTableOriginal.Rows)
            {
                dataTableInvertido.Columns.Add(row[0].ToString());
            }
            for (int i = 1; i < dataTableOriginal.Columns.Count; i++)
            {
                DataRow novaLinha = dataTableInvertido.NewRow();
                for (int j = 0; j < dataTableOriginal.Rows.Count; j++)
                {
                    novaLinha[j] = dataTableOriginal.Rows[j][i];
                }
                dataTableInvertido.Rows.Add(novaLinha);
            }
            return dataTableInvertido;
        }

        private void PopulaVendedores()
        {
            DataTable dados = Vendedor.GetDadosOrdenados();
            List<ComboBoxItem> lista = new List<ComboBoxItem>();
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                ComboBoxItem item = new ComboBoxItem(id, nome);
                lista.Add(item);
            }
            cmbVendedor.DataSource = lista;
            cmbVendedor.DisplayMember = "Nome";
            cmbVendedor.ValueMember = "Id";
        }
    }
}
