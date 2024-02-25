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

        private ReciboDAO Recibo;

        public opRecibos()
        {
            InitializeComponent();            
        }

        private void opRecibos_Load(object sender, EventArgs e)
        {
            PopulaVendedores();
            Recibo = new ReciboDAO();
            CarregaGrid();
            ConfigurarGrid();
        }

        private void PopulaVendedores()
        {
            VendedoresDAO Vendedor = new VendedoresDAO();
            DataTable dados = Vendedor.GetDadosOrdenados();
            List<ComboBoxItem> lista = new List<ComboBoxItem>();
            ComboBoxItem item0 = new ComboBoxItem(0, "SELECIONE");
            lista.Add(item0);
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

        #region Grid
        private void CarregaGrid()
        {
            Recibo = new ReciboDAO();
            DataTable Dados = Recibo.ValoresAPagar();
            if (Dados.Rows.Count == 0)
            {
                dataGrid1.DataSource = null;
            }
            else
            {
                dataGrid1.Rows.SetHeight(0, 0);
                DataTable DadosInvertidos = InverteLinhasColunas(Dados);
                for (int i = 0; i < DadosInvertidos.Columns.Count; i++)
                {
                    string info = (string)DadosInvertidos.Rows[1][i];
                    double vlr = Convert.ToDouble(info);
                    DadosInvertidos.Rows[1][i] = vlr.ToString("0.00");
                }
                DevAge.ComponentModel.BoundDataView boundDataView = new DevAge.ComponentModel.BoundDataView(DadosInvertidos.DefaultView);
                dataGrid1.DataSource = boundDataView;
                for (int i = 0; i < dataGrid1.Columns.Count; i++)
                {
                    if (dataGrid1.GetCell(1, i) != null)
                    {
                        dataGrid1.GetCell(1, i).View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                    }
                }
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

        private void ConfigurarGrid()
        {
            SourceGrid.Cells.Views.Cell fonte = new SourceGrid.Cells.Views.Cell();
            fonte.Font = new Font("Arial", 12, FontStyle.Regular);
            for (int i = 0; i < dataGrid1.Columns.Count; i++)
            {
                dataGrid1.Columns[i].Width = 160;
            }
            for (int i = 0; i < dataGrid1.Columns.Count; i++)
            {
                for (int j = 0; j < dataGrid1.Rows.Count; j++)
                {
                    dataGrid1.GetCell(j, i).View = fonte;
                    dataGrid1.GetCell(j, i).View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                }
            }
            dataGrid1.Invalidate();
        }

        #endregion

        private void cmbVendedor_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (dataGrid1.DataSource != null)
            {
                int id = Convert.ToInt32(cmbVendedor.SelectedValue);
                decimal ret = Recibo.VlrPend(id);
                ltVlr.Text = ret.ToString("C");
            }
        }
    }
}
