using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class OperPagar : Form
    {
        private FornecedorDao Forn;
        private string UID = "";
        private bool carregando = true;
        private int BakidForn=0;
        private string CaminhoPDF = "";
        private int iID = 0;
        private Color cor = Color.FromArgb(255, 192, 192); 

        #region Inicializacao

        public OperPagar()
        {
            InitializeComponent();
            SetStartPosition();
        }

        private void OperFalta_Load(object sender, EventArgs e)
        {

        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void OperFalta_Activated(object sender, EventArgs e)
        {
            //if (carregando)
            //{
            //    carregando = false;
            //}
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
        }

        private void VeSeHab(TextBox obj = null)
        {
            bool ok = true;
            if (cmbForn.SelectedIndex == -1)
            {
                ok = false;
            }
            if (txValorTotal.Text.Length<2)
            {
                ok = false;
            }
            btnAdicionar.Enabled = ok;
            btPDF.Enabled = ok;
            chPermanente.Enabled = ok;
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
            Limpar();
        }

        #endregion

        #region Grid

        private void CarregaGrid()
        {
            ContasAPagarDao contasAPagarDao = new ContasAPagarDao();
            DataTable dados = contasAPagarDao.GetDados(BakidForn);
            dataGrid1.DataSource = dados;
            if (dados != null)
            {
                ConfigurarGrid();
            }
        }

        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Visible = false;
            dataGrid1.Columns[1].Visible = false;
            dataGrid1.Columns[2].Width = 120;           // Forn
            dataGrid1.Columns[3].Width = 75;
            dataGrid1.Columns[4].Width = 75;
            dataGrid1.Columns[5].Width = 75;            // Valor tota
            dataGrid1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGrid1.Columns[6].Visible = false;       // chave
            dataGrid1.Columns[7].Width = 150;
            dataGrid1.Columns[8].Visible = false;       // PDF
            dataGrid1.Columns[9].Visible = false;       // Flag Pago
            dataGrid1.Columns[10].Width = 75;           // Data Pagamento
            dataGrid1.Columns[11].Width = 100;          // Obs
            dataGrid1.Columns[12].Visible = false;      // PermUID
            dataGrid1.Columns[13].Visible = false;      // ContasAPagar
            dataGrid1.Invalidate();
        }

        private void dataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
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
                } else
                {                    
                    dtpDataPagamento.Enabled = false;
                }
                this.CaminhoPDF = Convert.ToString(selectedRow.Cells["CaminhoPDF"].Value);
                if (this.CaminhoPDF.Length>0)
                {                    
                    btPDF.BackColor = cor; 
                } else
                {
                    btPDF.BackColor = SystemColors.Control;
                }
                txObservacoes.Text = Convert.ToString(selectedRow.Cells["Observacoes"].Value);
                chPermanente.Checked = Convert.ToBoolean(selectedRow.Cells["Perm"].Value);
                this.UID = Convert.ToString(selectedRow.Cells["UID"].Value);
                btnAdicionar.Text = "Alterar";
                btnAdicionar.Enabled = true;
                carregando = false;
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

        private void OperPagar_Load(object sender, EventArgs e)
        {
            Forn = new FornecedorDao();
            glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
            CarregaGrid();
        }

        private void btnAdicionar_Click_1(object sender, EventArgs e)
        {
            int idFornecedor = 0;
            int iForn = cmbForn.SelectedIndex;
            if (iForn > -1)
            {
                idFornecedor = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
            }
            DateTime dataEmissao = dtpDataEmissao.Value;
            DateTime dataVencimento = dtpDataVencimento.Value;            
            float valorTotal = (float)Convert.ToDouble(txValorTotal.Text);
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
            ContasAPagarDao contasAPagarDao = new ContasAPagarDao();
            if (btnAdicionar.Text == "Alterar")
            {                
                glo.Loga($@"PE,{idFornecedor}, {dataEmissao}, {dataVencimento}, {valorTotal}, {chaveNotaFiscal}, {descricao}, {this.CaminhoPDF}, {pago}, {dataPagamento}, {observacoes}, {perm}, {UID} ");                                
                contasAPagarDao.Edita(this.iID, idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, this.CaminhoPDF, pago, dataPagamento, observacoes, perm);
                btnAdicionar.Text = "Adicionar";
            }
            else
            {
                string UID = glo.GenerateUID();
                glo.Loga($@"PA,{idFornecedor}, {dataEmissao}, {dataVencimento}, {valorTotal}, {chaveNotaFiscal}, {descricao}, {this.CaminhoPDF}, {pago}, {dataPagamento}, {observacoes}, {perm}, {UID} ");
                contasAPagarDao.Adiciona(idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, this.CaminhoPDF, pago, dataPagamento, observacoes, perm, UID);
            }            
            CarregaGrid();
            Limpar();
            btPDF.BackColor = SystemColors.Control;
            this.CaminhoPDF = "";
        }

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
            VeSeHab();
        }

        private void btLimparFiltro_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btPDF_Click(object sender, EventArgs e)
        {
            if (btPDF.BackColor == SystemColors.Control)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.CaminhoPDF = openFileDialog.FileName;
                    btPDF.BackColor = cor;
                }
            }
            else
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
        }
    }
}