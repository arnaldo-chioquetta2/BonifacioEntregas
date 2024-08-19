using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class opRecibos : Form
    {

        private ReciboDAO Recibo;
        private VendedoresDAO Vendedor;
        private DataTable dataTableInvertido;
        private decimal VlrComiss = 0;

        public opRecibos()
        {
            InitializeComponent();            
        }

        private void opRecibos_Load(object sender, EventArgs e)
        {
            PopulaVendedores();
            Recibo = new ReciboDAO();
            dtnDtFim.Value = DateTime.Today;
            dtpDataIN.Value = dtnDtFim.Value.AddDays(-7);
            CarregaGrid();
            ConfigurarGrid();
            rt.AdjustFormComponents(this);
        }

        private void PopulaVendedores()
        {
            Vendedor = new VendedoresDAO();
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
            DateTime DT1 = dtpDataIN.Value.Date;
            DateTime DT2 = dtnDtFim.Value.Date;
            Recibo = new ReciboDAO();
            DataTable Dados = Recibo.ValoresAPagar(DT1, DT2);
            if (Dados.Rows.Count == 0)
            {
                dataGrid1.DataSource = null;
            }
            else
            {
                DataTable DadosFormatados = new DataTable();

                // Adicionar colunas
                DadosFormatados.Columns.Add("Descrição");
                foreach (DataRow row in Dados.Rows)
                {
                    DadosFormatados.Columns.Add(row["Nome"].ToString());
                }

                // Adicionar linhas
                DataRow vendasRow = DadosFormatados.NewRow();
                vendasRow[0] = "vendas";
                DataRow comissoesRow = DadosFormatados.NewRow();
                comissoesRow[0] = "comissões";

                for (int i = 0; i < Dados.Rows.Count; i++)
                {
                    vendasRow[i + 1] = Convert.ToDouble(Dados.Rows[i]["TotalVendas"]).ToString("0.00");
                    comissoesRow[i + 1] = Convert.ToDouble(Dados.Rows[i]["Valor"]).ToString("0.00");
                }

                DadosFormatados.Rows.Add(vendasRow);
                DadosFormatados.Rows.Add(comissoesRow);

                dataGrid1.DataSource = new DevAge.ComponentModel.BoundDataView(DadosFormatados.DefaultView);

                // Configurar a aparência da grid
                ConfigurarGrid();

                // Ajustar o alinhamento
                for (int i = 0; i < dataGrid1.Columns.Count; i++)
                {
                    for (int j = 0; j < dataGrid1.Rows.Count; j++)
                    {
                        if (i == 0) // Primeira coluna (descrições)
                        {
                            dataGrid1.GetCell(j, i).View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft;
                        }
                        else // Colunas de dados
                        {
                            dataGrid1.GetCell(j, i).View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight;
                        }
                    }
                }
            }
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

        private void dataGrid1_MouseDown(object sender, MouseEventArgs e)
        {
            SourceGrid.DataGrid grid = (SourceGrid.DataGrid)sender;
            SourceGrid.Position position = grid.PositionAtPoint(new System.Drawing.Point(e.X, e.Y));
            try
            {
                SourceGrid.Cells.ICellVirtual cell = grid.GetCell(position.Row, position.Column);
                int colunaClicada = position.Column;
                if (cell != null)
                {
                    string nome = dataTableInvertido.Rows[0][colunaClicada].ToString();
                    foreach (var item in cmbVendedor.Items)
                    {
                        if (item.ToString() == nome)
                        {
                            cmbVendedor.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // throw;
            }
        }

        #endregion

        private void cmbVendedor_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(cmbVendedor.SelectedValue);
                if (Recibo != null)
                {
                    DateTime DT1 = dtpDataIN.Value.Date;
                    DateTime DT2 = dtnDtFim.Value.Date;
                    decimal ret = Recibo.VlrPend(id, DT1, DT2);
                    ; ;  decimal roundedRet = Math.Round(ret, 0);
                    this.VlrComiss = Math.Round(ret, 0);
                    ltVlr.Text = this.VlrComiss.ToString("C");
                    btPagar.Enabled = (roundedRet > 0);
                    btExtrato.Enabled = true;
                }
            }
            catch (Exception)
            {
                // throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(cmbVendedor.SelectedValue);
            DateTime DataIni = dtpDataIN.Value;
            string dataPagamento = "";
            if (DataIni.Date == DateTime.Now.Date)
            {
                dataPagamento = "do dia " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                dataPagamento = "de " + DataIni.ToString("dd/MM/yyyy") + " a " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            Recibo.Pagar(id, ltVlr.Text, dataPagamento, dtpDataIN.Value, dtnDtFim.Value);
            // , this.VlrComiss
            INI MeuIni = new INI();
            using (var receipt = new rel.Receipt())
            {
                string storeName = MeuIni.ReadString("Identidade", "Nome", "");
                string storeAddress = MeuIni.ReadString("Identidade", "Endereco", "");
                string storePhone = MeuIni.ReadString("Identidade", "Fone", "");
                string customerName = cmbVendedor.Text;
                receipt.Print(storeName, storeAddress, storePhone, customerName, ltVlr.Text, DataIni, dataPagamento);
            }
            this.Close();
        }

        private void btExtrato_Click(object sender, EventArgs e)
        {
            rel.Extrato fExtr = new rel.Extrato();
            fExtr.SetNome(cmbVendedor.Text);
            int id = Convert.ToInt32(cmbVendedor.SelectedValue);
            fExtr.SetId(id);
            fExtr.Show();
        }

        private void btAtu_Click(object sender, EventArgs e)
        {
            CarregaGrid();
        }
    }
}
