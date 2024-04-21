using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class operCaixa : Form
    {

        private CaixaDao Caixa;
        private int iID = 0;

        public operCaixa()
        {
            InitializeComponent();
        }

        private void txCompra_KeyUp(object sender, KeyEventArgs e)
        {
            MostraTotal();
            VeSeHab();
        }

        private void VeSeHab()
        {
            bool OK = true;
            if (txCompra.Text == "")
            {
                OK = false;
            }
            btnLimpar.Enabled = OK;
            btDespeza.Enabled = OK;
            btPix.Enabled = OK;
            btDinheiro.Enabled = OK;
            btCartao.Enabled = OK;
        }

        private void MostraTotal()
        {
            float compra = glo.LeValor(txCompra.Text);
            float desc = glo.LeValor(txDesc.Text);
            float total = compra - desc;
            if (total > 0)
            {
                lbTotal.Text = total.ToString("C");
            }
            else
            {
                lbTotal.Text = "";
            }
        }

        private void operCaixa_Load(object sender, EventArgs e)
        {
            Caixa = new CaixaDao();
            ClienteDAO Cliente = new ClienteDAO();
            VendedoresDAO Vendedor = new VendedoresDAO();
            CarregarComboBox<Cliente>(cmbCliente, Cliente, "NÃO IDENTIFICADO");
            CarregarComboBox<Vendedor>(cmbVendedor, Vendedor);
            cmbCliente.SelectedIndex = 0;
            cmbVendedor.SelectedIndex = 0;            
            CarregaGrid();
            ConfigurarGrid();
        }

        private void CarregarComboBox<T>(ComboBox comboBox, BaseDAO classe, string ItemZero = "") where T : IDataEntity, new()
        {
            DataTable dados = classe.GetDadosOrdenados();
            List<ComboBoxItem> lista = new List<ComboBoxItem>();
            if (ItemZero.Length > 0)
            {
                ComboBoxItem item = new ComboBoxItem(0, ItemZero);
                lista.Add(item);
            }
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                ComboBoxItem item = new ComboBoxItem(id, nome);
                lista.Add(item);
            }
            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Nome";
            comboBox.ValueMember = "Id";
        }

        private void txDesc_KeyUp(object sender, KeyEventArgs e)
        {
            MostraTotal();
        }

        private void Registra(int idForma)
        {
            int idCliente = Convert.ToInt32(cmbCliente.SelectedValue);
            int idVend = Convert.ToInt32(cmbVendedor.SelectedValue);
            float compra;
            if (!float.TryParse(txCompra.Text, out compra))
            {
                compra = 0;
            }
            string obs = txObs.Text;
            float desc;
            if (!float.TryParse(txDesc.Text, out desc))
            {
                desc = 0;
            }
            if (this.iID==0)
            {
                Caixa.Adiciona(idForma, compra, idCliente, obs, desc, idVend);
            } else
            {
                Caixa.Edita(this.iID, idForma, compra, idCliente, obs, desc, idVend);
            }            
            CarregaGrid();
            Limpar();
        }

        private void Limpar()
        {
            txDesc.Text = "";
            txObs.Text = "";
            txCompra.Text = "";
            lbTotal.Text = "";
            cmbCliente.SelectedIndex = 0;
            cmbVendedor.SelectedIndex = 0;
            this.iID = 0;
            BotoesNormais();
        }

        private void BotoesNormais()
        {
            this.btDinheiro.Enabled = true;
            this.btDespeza.Enabled = true;
            this.btPix.Enabled = true;
            this.btCartao.Enabled = true;
            this.btDinheiro.BackColor = SystemColors.Control;
            this.btDespeza.BackColor = SystemColors.Control;
            this.btPix.BackColor = SystemColors.Control;
            this.btCartao.BackColor = SystemColors.Control;
            btExcluir.Visible = false;
        }

        private void CarregaGrid()
        {
            DataTable dados = Caixa.getDados(dtpData.Value);
            DevAge.ComponentModel.BoundDataView boundDataView = new DevAge.ComponentModel.BoundDataView(dados.DefaultView);
            dataGrid1.DataSource = boundDataView;
        }

        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Width = 0;
            dataGrid1.Columns[1].Width = 170;   // Cliente
            dataGrid1.Columns[2].Width = 70;    // Valor
            dataGrid1.Columns[3].Width = 70;    // Desconto
            dataGrid1.Columns[4].Width = 70;    // VlNota            
            dataGrid1.Columns[5].Width = 170;   // Vendedor
            dataGrid1.Columns[6].Width = 100;   // Data
            dataGrid1.Columns[7].Width = 70;    // Forma de pagamento
            dataGrid1.Columns[8].Width = 200;    // Obs
            dataGrid1.Columns[9].Width = 0;
            dataGrid1.Columns[10].Width = 0;
            dataGrid1.Columns[11].Width = 0;
            dataGrid1.Invalidate();
        }

        #region Formas

        private void btCartao_Click(object sender, EventArgs e)
        {
            Registra(1);
        }

        private void btPix_Click(object sender, EventArgs e)
        {
            Registra(3);
        }

        private void btTroco_Click(object sender, EventArgs e)
        {
            Registra(5);
        }

        private void btAnotado_Click(object sender, EventArgs e)
        {
            Registra(2);
        }

        private void btDinheiro_Click(object sender, EventArgs e)
        {
            Registra(0);
        }

        #endregion

        private void btnNovoCliente_Click(object sender, EventArgs e)
        {
            glo.IdAdicionado = -1;
            fCadClientes Cad = new fCadClientes();
            Cad.ShowDialog();
            if (glo.IdAdicionado > 0)
            {
                ClienteDAO Cliente = new ClienteDAO();
                CarregarComboBox<tb.Cliente>(cmbCliente, Cliente);
                cmbCliente.SelectedValue = glo.IdAdicionado;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregaGrid();
        }

        private void dataGrid1_Click(object sender, EventArgs e)
        {
            SourceGrid.DataGrid grid = (SourceGrid.DataGrid)sender;
            if (grid != null && grid.Rows.Count > 0)
            {
                SourceGrid.Position position = grid.Selection.ActivePosition;
                if (position != SourceGrid.Position.Empty)
                {
                    this.iID = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["Id"]);
                    cmbCliente.SelectedValue = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["NrCli"]);
                    txCompra.Text = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["Valor"]);
                    txDesc.Text = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["Desconto"]);
                    cmbVendedor.SelectedValue = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["idVend"]);
                    MostraTotal();
                    btnLimpar.Enabled = true;
                    int idForma= glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["idForma"]);
                    BotoesNormais();
                    Color cor = Color.FromArgb(128, 255, 128);
                    switch (idForma)
                    {
                        case 0:
                            this.btDinheiro.BackColor = cor;
                            break;
                        case 1:
                            this.btCartao.BackColor = cor;                            
                            break;
                        //case 2:
                        //    this.btAnotado.BackColor = cor;
                        //    break;
                        case 3:
                            this.btPix.BackColor = cor;
                            break;
                        case 5:
                            this.btDespeza.BackColor = cor;
                            break;
                    }
                    btExcluir.Visible = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Caixa.Exclui(this.iID);
                CarregaGrid();
                Limpar();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rel.Caixa fRel = new rel.Caixa();
            fRel.Show();
        }
    }
}
