using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class OperPagar : Form
    {
        private FornecedorDao Forn;
        private bool carregando = true;
        private int BakidForn=0;        

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
        private void btnAdicionar_Click(object sender, EventArgs e)
        {

        }

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
        }

        private void cmbVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                VeSeHab();
                //cmbVendedor.FlatStyle = FlatStyle.Flat;
                //button2.Enabled = true;
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        #endregion

        #region Grid

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
            decimal valorTotal = Convert.ToDecimal(txValorTotal.Text);
            string chaveNotaFiscal = txChaveNotaFiscal.Text;
            string descricao = txDescricao.Text;
            bool pago = ckPago.Checked;
            DateTime? dataPagamento = null;
            if (pago)
            {
                dataPagamento = dtpDataPagamento.Value;
            }
            string observacoes = txObservacoes.Text;
            string caminhoPDF = "";
            glo.Loga($@"PA,{idFornecedor}, {dataEmissao}, {dataVencimento}, {valorTotal}, {chaveNotaFiscal}, {descricao}, {caminhoPDF}, {pago}, {dataPagamento}, {observacoes} ");
            ContasAPagarDao contasAPagarDao = new ContasAPagarDao();
            contasAPagarDao.Adiciona(idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, caminhoPDF, pago, dataPagamento, observacoes);
            CarregaGrid();
            Limpar();
        }

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
            dataGrid1.Columns[2].Width = 120; // Forn
            dataGrid1.Columns[3].Width = 75;
            dataGrid1.Columns[4].Width = 75;
            dataGrid1.Columns[5].Width = 75;   // Valor tota
            dataGrid1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGrid1.Columns[6].Visible = false; // chave
            dataGrid1.Columns[7].Width = 150;
            dataGrid1.Columns[8].Visible = false; // PDF
            dataGrid1.Columns[9].Visible = false;    // Flag Pago
            dataGrid1.Columns[10].Width = 75; // Data Pagamento
            dataGrid1.Columns[11].Width = 100; // Obs             
            dataGrid1.Invalidate();
        }

        private void ckPago_CheckedChanged(object sender, EventArgs e)
        {
            dtpDataPagamento.Enabled = ckPago.Checked;
        }

        private void txValorTotal_TextChanged(object sender, EventArgs e)
        {
            
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
    }
}