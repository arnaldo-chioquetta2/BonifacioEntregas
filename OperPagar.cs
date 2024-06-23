using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class OperPagar : Form
    {
        private FornecedorDao Forn;
        private ContasAPagarDao contasAPagarDao;
        private string UID = "";
        private bool carregando = true;
        private int BakidForn=0;
        private string CaminhoPDF = "";        
        private int iID = 0;
        private bool CarregarT = true;
        private bool CarregarP = true;
        // private Color cor = Color.FromArgb(255, 192, 192); 

        #region Inicializacao

        public OperPagar()
        {
            InitializeComponent();
            SetStartPosition();
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void OperPagar_Load(object sender, EventArgs e)
        {
            Forn = new FornecedorDao();
            contasAPagarDao = new ContasAPagarDao();
            glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
            CarregaGrid();            
            carregando = false;
        }

        #endregion

        #region Adição

        private void Limpar()
        {
            cmbForn.SelectedIndex = 0;
            txValorTotal.Text = "";
            txChaveNotaFiscal.Text = "";
            txDescricao.Text = "";
            ckPago.Checked = false;
            txObservacoes.Text = "";
            chPermanente.Checked = false;
            chPermanente.Enabled = false;
            btLimparFiltro.Enabled = false;
            btExcluir.Enabled = false;
        }

        private void VeSeHab(TextBox obj = null)
        {
            bool ok = false;
            if (cmbForn.SelectedIndex != -1)
            {
                ok = true;
            } else
            {
                if (txDescricao.Text.Length > 0)
                {
                    ok = true;
                }
            }
            btnAdicionar.Enabled = ok;
            // btPDF.Enabled = ok;
            chPermanente.Enabled = ok;
            btLimparFiltro.Enabled = ok;
        }

        private void cmbVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                VeSeHab();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            string sID = "";
            string UID = "";
            if (tbContas.SelectedIndex == 0)
            {
                sID = dataGrid1.SelectedRows[0].Cells["ID"].Value.ToString();
                UID = dataGrid1.SelectedRows[0].Cells["UID"].Value.ToString();
            } else
            {
                sID = dataGrid2.SelectedRows[0].Cells["ID"].Value.ToString();
                UID = dataGrid2.SelectedRows[0].Cells["UID"].Value.ToString();
            }
            glo.Loga($@"PD,{sID}, {UID}");
            contasAPagarDao.Exclui(sID, this.CaminhoPDF);
            if (tbContas.SelectedIndex == 0)
            {
                CarregaGrid();
            } else
            {
                CarregaGridP();
            }
            Limpar();
        }

        private void btnAdicionar_Click_1(object sender, EventArgs e)
        {
            if (btnAdicionar.Text == "Adicionar")
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                this.CaminhoPDF = openFileDialog.FileName;
            }                        
            // btPDF.BackColor = cor;
            int idFornecedor = 0;
            int iForn = cmbForn.SelectedIndex;
            if (iForn > -1)
            {
                idFornecedor = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
            }
            DateTime dataEmissao = dtpDataEmissao.Value;
            DateTime dataVencimento = dtpDataVencimento.Value;
            float valorTotal = glo.LeValor(txValorTotal.Text);
            string chaveNotaFiscal = txChaveNotaFiscal.Text;
            string descricao = txDescricao.Text;
            bool pago = ckPago.Checked;
            DateTime? dataPagamento = null;
            if (pago)
            {
                dataPagamento = dtpDataPagamento.Value;
            }
            string observacoes = txObservacoes.Text;
            bool perm = chPermanente.Checked;
            if (btnAdicionar.Text == "Alterar")
            {
                glo.Loga($@"PE,{idFornecedor}, {dataEmissao}, {dataVencimento}, {valorTotal}, {chaveNotaFiscal}, {descricao}, {this.CaminhoPDF}, {pago}, {dataPagamento}, {observacoes}, {perm}, {UID} ");
                contasAPagarDao.Edita(this.iID, idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, this.CaminhoPDF, pago, dataPagamento, observacoes, perm);
                btnAdicionar.Text = "Adicionar";
                if (tbContas.SelectedIndex == 0)
                {
                    CarregaGrid();
                } else
                {
                    CarregaGridP();
                }
            }
            else
            {
                string UID = glo.GenerateUID();
                glo.Loga($@"PA,{idFornecedor}, {dataEmissao}, {dataVencimento}, {valorTotal}, {chaveNotaFiscal}, {descricao}, {this.CaminhoPDF}, {pago}, {dataPagamento}, {observacoes}, {perm}, {UID} ");
                contasAPagarDao.Adiciona(idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, this.CaminhoPDF, pago, dataPagamento, observacoes, perm, UID);
                if (tbContas.SelectedIndex == 0)
                {
                    if (!perm)
                    {
                        CarregaGrid();
                    }
                    else
                    {
                        CarregarP = true;
                    }
                }
                else
                {
                    if (perm)
                    {
                        CarregaGridP();
                    }
                    else
                    {
                        CarregarT = true;
                    }
                }

            }
            Limpar();
            //btPDF.BackColor = SystemColors.Control;
            this.CaminhoPDF = "";
        }

        #endregion

        #region Grid do Temporários

        private void CarregaGrid()
        {
            DataTable dados = contasAPagarDao.GetDados(BakidForn, 0);
            dataGrid1.DataSource = dados;
            if (dados != null)
            {
                ConfigurarGrid(ref dataGrid1);
            }
        }

        private void ConfigurarGrid(ref DataGridView Grid)
        {
            Grid.Columns[0].Visible = false;
            Grid.Columns[1].Visible = false;
            Grid.Columns[2].Width = 120;           // Forn
            Grid.Columns[3].Width = 75;
            Grid.Columns[4].Width = 75;
            Grid.Columns[5].Width = 75;            // Valor tota
            Grid.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Grid.Columns[6].Visible = false;       // chave
            Grid.Columns[7].Width = 150;
            Grid.Columns[8].Visible = false;       // PDF
            Grid.Columns[9].Visible = false;       // Flag Pago
            Grid.Columns[10].Width = 75;           // Data Pagamento
            Grid.Columns[11].Width = 100;          // Obs
            Grid.Columns[12].Visible = false;      // Perm
            Grid.Columns[13].Visible = false;      // ContasAPagar
            Grid.Invalidate();
        }

        private void dataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {            
            DataGridView grid = (DataGridView)sender;
            Mostra(ref grid, ref e);
        }

        private void Mostra(ref DataGridView grid, ref DataGridViewCellEventArgs e)
        {
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                this.UID = Convert.ToString(selectedRow.Cells["UID"].Value);
                cmbForn.SelectedValue = Convert.ToInt32(selectedRow.Cells["idFornecedor"].Value);
                dtpDataEmissao.Value = Convert.ToDateTime(selectedRow.Cells["DataEmissao"].Value);
                dtpDataVencimento.Value = Convert.ToDateTime(selectedRow.Cells["DataVencimento"].Value);
                txValorTotal.Text = Convert.ToString(selectedRow.Cells["ValorTotal"].Value);
                txChaveNotaFiscal.Text = Convert.ToString(selectedRow.Cells["ChaveNotaFiscal"].Value);
                txDescricao.Text = Convert.ToString(selectedRow.Cells["Descricao"].Value);
                ckPago.Checked = Convert.ToBoolean(selectedRow.Cells["Pago"].Value);
                btPDF.Enabled = true;
                if (ckPago.Checked)
                {
                    dtpDataPagamento.Enabled = true;
                    dtpDataPagamento.Value = Convert.ToDateTime(selectedRow.Cells["DataPagamento"].Value);
                }
                else
                {
                    dtpDataPagamento.Enabled = false;
                }
                this.CaminhoPDF = Convert.ToString(selectedRow.Cells["CaminhoPDF"].Value);
                //if (this.CaminhoPDF.Length>0)
                //{                    
                //    btPDF.BackColor = cor; 
                //} else
                //{
                //    btPDF.BackColor = SystemColors.Control;
                //}
                txObservacoes.Text = Convert.ToString(selectedRow.Cells["Observacoes"].Value);
                chPermanente.Checked = Convert.ToBoolean(selectedRow.Cells["Perm"].Value);                
                btnAdicionar.Text = "Alterar";
                btnAdicionar.Enabled = true;
                btLimparFiltro.Enabled = true;
                chPermanente.Enabled = true;
                btExcluir.Enabled = true;
                carregando = false;
            }
        }

        #endregion

        #region Grid dos Permanentes

        private void CarregaGridP()
        {
            DataTable dados = contasAPagarDao.GetDados(BakidForn, -1);
            dataGrid2.DataSource = dados;
            if (dados != null)
            {
                ConfigurarGrid(ref dataGrid2);
            }
        }

        #endregion

        #region Tipos

        private void btAdicTpo_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Filtro

        #endregion

        private void ckPago_CheckedChanged(object sender, EventArgs e)
        {
            dtpDataPagamento.Enabled = ckPago.Checked;
        }

        private void txValorTotal_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab();
        }

        private void cmbForn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                VeSeHab();
            }            
        }

        private void btLimparFiltro_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btPDF_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(this.CaminhoPDF);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao abrir o arquivo: {ex.Message}");
            }
        }

        private void txDescricao_KeyUp(object sender, KeyEventArgs e)
        {
            if (!carregando)
            {
                VeSeHab();
            }
        }

        private void dataGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            Mostra(ref grid, ref e);
        }

        private void tbContas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbContas.SelectedIndex==0)
            {
                if (CarregarT)
                {
                    CarregaGrid();
                }
            } else
            {
                if (CarregarP)
                {
                    CarregaGridP();
                }
            }
        }
    }
}