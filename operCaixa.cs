using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class operCaixa : Form
    {

        private CaixaDao Caixa;
        private int iID = 0;
        private string UID = "";

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
            if ((txCompra.Text == "") && (txDesc.Text == ""))
            {
                OK = false;
            }
            btnLimpar.Enabled = OK;
            btDespeza.Enabled = OK;
            btPix.Enabled = OK;
            btDinheiro.Enabled = OK;
            btCartao.Enabled = OK;
            btItau.Enabled = OK;
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
            glo.CarregarComboBox<Cliente>(cmbCliente, Cliente, "NÃO IDENTIFICADO");
            glo.CarregarComboBox<Vendedor>(cmbVendedor, Vendedor, "", " Where Vendedores.Atende = -1 or Vendedores.Atende = 1 ", " desc ");
            cmbCliente.SelectedIndex = 0;
            cmbVendedor.SelectedIndex = 0;
            DateTime ontem = DateTime.Today.AddDays(-1);
            dtpDataIN.Value = ontem;
            CarregaGrid();
            ConfigurarGrid();
        }

        private void txDesc_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab();
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
            if (this.iID == 0)
            {
                string UID = glo.GenerateUID();
                glo.Loga($@"CA,{idForma},{compra},{idCliente}, {obs}, {desc}, {idVend}, {UID}");
                Caixa.Adiciona(idForma, compra, idCliente, obs, desc, idVend, UID);
            }
            else
            {
                glo.Loga($@"CE,{this.iID},{idForma},{compra},{idCliente}, {obs}, {desc}, {idVend}, {this.UID}");
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

        private void btItau_Click(object sender, EventArgs e)
        {
            Registra(6);
        }

        #endregion

        #region Botões

        private void BotoesNormais()
        {
            this.btDinheiro.Enabled = true;
            this.btDespeza.Enabled = true;
            this.btPix.Enabled = true;
            this.btCartao.Enabled = true;
            this.btItau.Enabled = true;
            this.btDinheiro.BackColor = SystemColors.Control;
            this.btDespeza.BackColor = SystemColors.Control;
            this.btPix.BackColor = SystemColors.Control;
            this.btCartao.BackColor = SystemColors.Control;
            this.btItau.BackColor = SystemColors.Control;
            btExcluir.Visible = false;
            btEditar.Visible = false;
        }

        private void btnNovoCliente_Click(object sender, EventArgs e)
        {
            glo.IdAdicionado = -1;
            fCadClientes Cad = new fCadClientes();
            Cad.ShowDialog();
            if (glo.IdAdicionado > 0)
            {
                ClienteDAO Cliente = new ClienteDAO();
                glo.CarregarComboBox<tb.Cliente>(cmbCliente, Cliente);
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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                glo.Loga($@"CD,{this.iID}, {this.UID}");
                Caixa.Exclui(this.iID);
                CarregaGrid();
                Limpar();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rel.Caixa fRel = new rel.Caixa();
            fRel.DT1 = dtpDataIN.Value;
            fRel.DT2 = dtnDtFim.Value;
            fRel.Show();
        }

        private void btEditar_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Digite a data (DD/MM/AAAA):", "Digite a Data");
            if (input != "")
            {
                DateTime data;
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
                {
                    string Lista = "";
                    foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                    {
                        int id;
                        if (int.TryParse(row.Cells[0].Value.ToString(), out id))
                        {
                            Lista += id.ToString() + ",";
                        }
                    }
                    Lista = Lista.Remove(Lista.Length - 1);
                    Caixa.MudaData(data, Lista);
                    CarregaGrid();
                    Limpar();
                }
                else
                {
                    MessageBox.Show("Data inválida. Por favor, digite uma data no formato DD/MM/AAAA.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Grid

        private void dataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                int nrCli = 0;
                string snrCli = Convert.ToString(selectedRow.Cells["NrCli"].Value);
                if (snrCli.Length > 0)
                {
                    nrCli = Convert.ToInt32(selectedRow.Cells["NrCli"].Value);
                }
                decimal valor = Convert.ToDecimal(selectedRow.Cells["Valor"].Value);
                decimal desconto = Convert.ToDecimal(selectedRow.Cells["Desconto"].Value);
                int idVend = Convert.ToInt32(selectedRow.Cells["idVend"].Value);
                int idForma = Convert.ToInt32(selectedRow.Cells["idForma"].Value);
                this.iID = id;
                this.UID = Convert.ToString(selectedRow.Cells["UID"].Value);
                cmbCliente.SelectedValue = nrCli;
                txCompra.Text = valor.ToString();
                txDesc.Text = desconto.ToString();
                txObs.Text = selectedRow.Cells["Obs"].Value.ToString();
                cmbVendedor.SelectedValue = idVend;
                MostraTotal();
                btnLimpar.Enabled = true;
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
                    case 3:
                        this.btPix.BackColor = cor;
                        break;
                    case 5:
                        this.btDespeza.BackColor = cor;
                        break;
                    case 6:
                        this.btItau.BackColor = cor;
                        break;

                }
                btExcluir.Visible = true;
                btEditar.Visible = true;
            }
        }

        private void CarregaGrid()
        {
            DataTable dados = Caixa.getDados(dtpDataIN.Value, dtnDtFim.Value);
            dataGrid1.DataSource = dados;
        }

        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Visible = false; // Tornar a primeira coluna invisível (largura = 0)
            dataGrid1.Columns[1].Width = 170;   // Cliente
            dataGrid1.Columns[2].Width = 70;    // Valor
            dataGrid1.Columns[3].Width = 70;    // Desconto
            dataGrid1.Columns[4].Width = 70;    // VlNota            
            dataGrid1.Columns[5].Width = 170;   // Vendedor
            dataGrid1.Columns[6].Width = 100;   // Data
            dataGrid1.Columns[7].Width = 70;    // Forma de pagamento
            dataGrid1.Columns[8].Width = 200;    // Obs
            dataGrid1.Columns[9].Visible = false;
            dataGrid1.Columns[10].Visible = false;
            dataGrid1.Columns[11].Visible = false;
            dataGrid1.Columns[12].Visible = false;
        }

        #endregion

    }
}
