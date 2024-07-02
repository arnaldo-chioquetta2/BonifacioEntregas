using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class OperPagar : Form
    {
        private FornecedorDao Forn;
        private ContasAPagarDao contasAPagarDao;
        private INI cINI;
        private string UID = "";
        private bool carregando = true;
        private string CaminhoPDF = "";
        private int iID = 0;
        private bool CarregarT = true;
        private bool CarregarP = true;
        private string destinationDirectory = "";
        private string sourceDirectory = "";
        private string CaminhoBasePDF = "";

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
            CarregaGridGenerico(dataGrid1, 0);
            CaminhoBasePDF = Path.GetDirectoryName(glo.CaminhoBase) + "\\Docs";
            INI cINI = new INI();
            sourceDirectory = cINI.ReadString("Config", "Docs", "");
            carregando = false;
        }

        #endregion

        #region Adição

        private void Limpar()
        {
            txValorTotal.Text = "";
            txChaveNotaFiscal.Text = "";
            txDescricao.Text = "";
            ckPago.Checked = false;
            txObservacoes.Text = "";
            btExcluir.Enabled = false;
            dtpDataPagamento.Tag = "";
            dtpDataVencimento.Tag = "";
            dtpDataEmissao.Tag = "";
            txValorTotal.Tag = "";
            txDescricao.Tag = "";
            txObservacoes.Tag = "";
            ckPago.Tag = "";
            cmbForn.Tag = "";
        }

        private void VeSeHab(TextBox obj = null)
        {
            if (obj != null)
            {
                if (obj.Text.Length > 0)
                {
                    obj.Tag = "M";
                }
            }
            bool ok = false;
            if (cmbForn.SelectedIndex != -1)
            {
                ok = true;
            }
            else
            {
                if (txDescricao.Text.Length > 0)
                {
                    ok = true;
                }
            }
            btnAdicionar.Enabled = ok;
            btPDF.Enabled = false;
            btLimparFiltro.Enabled = ok;
        }

        private void btnAdicionar_Click_1(object sender, EventArgs e)
        {
            if (txNvForn.Visible)
            {
                Forn.Adiciona(txNvForn.Text);
                txNvForn.Visible = false;
                cmbForn.Visible = true;
                glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
                cmbForn.Text = txNvForn.Text;
                VeSeHab();
            }
            else
            {
                int idFornecedor = 0;
                int iForn = cmbForn.SelectedIndex;
                if (iForn > -1)
                {
                    idFornecedor = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
                }
                DateTime dataEmissao = dtpDataEmissao.Value;
                DateTime dataVencimento = DateTime.MinValue;
                if (dtpDataVencimento.Tag=="M")
                {
                    dataVencimento = dtpDataVencimento.Value;
                }
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
                int selectedRowIndex = dataGrid1.CurrentRow != null ? dataGrid1.CurrentRow.Index : -1;
                int scrollPosition = dataGrid1.FirstDisplayedScrollingRowIndex;
                glo.Loga($@"PE,{idFornecedor}, {dataEmissao}, {dataVencimento}, {valorTotal}, {chaveNotaFiscal}, {descricao}, {pago}, {dataPagamento}, {observacoes}, {UID} ");
                contasAPagarDao.Edita(this.iID, idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, pago, dataPagamento, observacoes);
                Limpar();
                AtualizaGrids();
                dataGrid1.FirstDisplayedScrollingRowIndex = scrollPosition;
                if (selectedRowIndex >= 0 && selectedRowIndex < dataGrid1.RowCount)
                {
                    dataGrid1.Rows[selectedRowIndex].Selected = true;
                    dataGrid1.CurrentCell = dataGrid1.Rows[selectedRowIndex].Cells[0]; 
                }
            }
        }

        #endregion

        #region PDF

        private void btPDF_Click(object sender, EventArgs e)
        {
            if (dataGrid1.SelectedRows.Count == 1)
            {
                string ArquivoOrig = (string)dataGrid1.SelectedRows[0].Cells["Arquivo"].Value;
                AbreArquivo(this.iID, ArquivoOrig);
            }
            else
            {
                int i = 1;
                foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                {
                    int idArquivo = (int)row.Cells["ID"].Value;
                    string ArquivoOrig = (string)row.Cells["Arquivo"].Value; 
                    AbreArquivo(idArquivo, ArquivoOrig);
                    i++;
                }
            }            
        }

        private void GravaPDF(int numero)
        {
            string fileExtension = Path.GetExtension(this.CaminhoPDF).TrimStart('.');            
            string destinationFileName = $"Doc{numero}.{fileExtension}";
            string destinationFilePath = Path.Combine(CaminhoBasePDF, destinationFileName);
            try
            {
                if (!Directory.Exists(CaminhoBasePDF))
                {
                    Directory.CreateDirectory(CaminhoBasePDF);
                }
                File.Copy(this.CaminhoPDF, destinationFilePath, true);
                Console.WriteLine($"Arquivo copiado para: {destinationFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao copiar o arquivo: {ex.Message}");
            }
        }

        private void AbreArquivo(int Nro, string ArquivoOrig)
        {
            string fileNamePrefix = "Doc";
            string fileExtension = Path.GetExtension(ArquivoOrig).TrimStart('.');
            string sourceFilePath = Path.Combine(CaminhoBasePDF, $"{fileNamePrefix}{Nro}.{fileExtension}");
            try
            {
                if (File.Exists(sourceFilePath))
                {
                    Process.Start(new ProcessStartInfo(sourceFilePath) { UseShellExecute = true });
                }
                else
                {
                    Console.WriteLine($"Arquivo não encontrado: {sourceFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao abrir o arquivo: {ex.Message}");
            }
        }

        #endregion

        #region Grid 

        private void ConfigurarGrid(ref DataGridView Grid)
        {            
            Grid.Columns[0].Width = 120;            // Arquivo
            Grid.Columns[1].Visible = false;
            Grid.Columns[2].Visible = false;
            Grid.Columns[3].Width = 120;           // Forn
            Grid.Columns[4].Width = 75;
            Grid.Columns[5].Width = 75;

            Grid.Columns[6].Width = 75;            // Valor tota
            Grid.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            Grid.Columns[7].Visible = false;       // chave
            Grid.Columns[8].Width = 150;
            Grid.Columns[9].Visible = false;       // PDF
            Grid.Columns[10].Visible = false;       // Flag Pago
            Grid.Columns[11].Width = 75;           // Data Pagamento
            Grid.Columns[12].Width = 100;          // Obs
            Grid.Columns[13].Visible = false;      // Perm
            Grid.Columns[14].Visible = false;      // ContasAPagar
            Grid.Columns[15].Visible = false;      // idArquivo
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
                dtpDataEmissao.Value = Convert.ToDateTime(selectedRow.Cells["DataEmissao"].Value);
                btPDF.Enabled = true;
                if (selectedRow.Cells["idFornecedor"].Value != DBNull.Value)
                {
                    cmbForn.SelectedValue = Convert.ToInt32(selectedRow.Cells["idFornecedor"].Value);                    
                    dtpDataVencimento.Value = Convert.ToDateTime(selectedRow.Cells["DataVencimento"].Value);
                    txValorTotal.Text = Convert.ToString(selectedRow.Cells["ValorTotal"].Value);
                    txChaveNotaFiscal.Text = Convert.ToString(selectedRow.Cells["ChaveNotaFiscal"].Value);
                    txDescricao.Text = Convert.ToString(selectedRow.Cells["Descricao"].Value);
                    ckPago.Checked = Convert.ToBoolean(selectedRow.Cells["Pago"].Value);                    
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
                    txObservacoes.Text = Convert.ToString(selectedRow.Cells["Observacoes"].Value);
                }
                btnAdicionar.Enabled = true;
                btLimparFiltro.Enabled = true;
                btExcluir.Enabled = true;
                carregando = false;
            }
        }

        private void AtualizaGrids()
        {
            CarregaGridGenerico(dataGrid1, 0);
        }

        #endregion

        private void CarregaGridGenerico(DataGridView dataGrid, int filter)
        {
            int idForne = 0;
            if (cmbForn.Tag == "M")
            {
                int iForn = cmbForn.SelectedIndex;
                idForne = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
            }
            DateTime? dataPagamento = dtpDataPagamento.Tag?.ToString() == "M" && dtpDataPagamento.Checked ? (DateTime?)dtpDataPagamento.Value : null;
            DateTime? dataVencimento = dtpDataVencimento.Tag?.ToString() == "M" && dtpDataVencimento.Checked ? (DateTime?)dtpDataVencimento.Value : null;
            DateTime? dataEmissao = dtpDataEmissao.Tag?.ToString() == "M" && dtpDataEmissao.Checked ? (DateTime?)dtpDataEmissao.Value : null;
            string valorTotal = txValorTotal.Tag?.ToString() == "M" && !string.IsNullOrWhiteSpace(txValorTotal.Text) ? txValorTotal.Text : null;
            string descricao = txDescricao.Tag?.ToString() == "M" && !string.IsNullOrWhiteSpace(txDescricao.Text) ? txDescricao.Text : null;
            string observacoes = txObservacoes.Tag?.ToString() == "M" && !string.IsNullOrWhiteSpace(txObservacoes.Text) ? txObservacoes.Text : null;
            bool? pago = ckPago.Tag?.ToString() == "M" && ckPago.CheckState != CheckState.Indeterminate ? (bool?)ckPago.Checked : null;
            DataTable dados = contasAPagarDao.GetDados(idForne, dataPagamento, dataVencimento, dataEmissao, valorTotal, descricao, observacoes, pago);
            dataGrid.DataSource = dados;
            if (dados != null)
            {
                ConfigurarGrid(ref dataGrid);
            }
        }

        #region Filtro

        private void btFiltrar_Click(object sender, EventArgs e)
        {
            AtualizaGrids();
        }

        #endregion
        private void cmbForn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                if (cmbForn.SelectedItem != null)
                {
                    string ItemCombo = cmbForn.SelectedItem.ToString();
                    if (ItemCombo != "ESCOLHA")
                    {
                        if (ItemCombo == "ADICIONE")
                        {
                            txNvForn.Visible = true;
                            cmbForn.Visible = false;
                            btnAdicionar.Enabled = true;
                            txNvForn.Focus();
                        }
                        else
                        {
                            if (ItemCombo == "EDIÇÃO")
                            {
                                fCadTiposFaltas novoForm = new fCadTiposFaltas();
                                novoForm.ShowDialog();
                                glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
                            }
                            else
                            {
                                cmbForn.Tag = "M";
                                btLimparFiltro.Enabled = true;
                                btFiltrar.Enabled = true;
                                btnAdicionar.Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void btLimparFiltro_Click(object sender, EventArgs e)
        {
            Limpar();
            cmbForn.SelectedIndex = 0;
            btLimparFiltro.Enabled = false;
            btnAdicionar.Enabled = false;
        }

        #region Eventos de Teclado

        private void ckPago_CheckedChanged(object sender, EventArgs e)
        {
            dtpDataPagamento.Enabled = ckPago.Checked;
        }

        private void txValorTotal_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txValorTotal);
        }

        private void txDescricao_KeyUp(object sender, KeyEventArgs e)
        {
            if (!carregando)
            {
                VeSeHab(txDescricao);
            }
        }

        private void tbContas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregaGridGenerico(dataGrid1, 0);
        }
        private void txDescricao_TextChanged(object sender, EventArgs e)
        {
            VeSeHab(txDescricao);
        }

        private void dtpDataEmissao_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                VeSeHab(txObservacoes);
            }
        }

        private void txChaveNotaFiscal_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txChaveNotaFiscal);
        }

        private void txObservacoes_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txObservacoes);
        }

        private void dtpDataVencimento_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                dtpDataVencimento.Tag = "M";
            }
        }

        private void dtpDataPagamento_ValueChanged(object sender, EventArgs e)
        {
            dtpDataVencimento.Tag = "M";
        }

        #endregion

        #region Deleção

        private void AdicionarAoArquivoINI(string caminhoPDF)
        {            
            int nro = cINI.ReadInt("Apagar", "Nro", 0);
            nro++;
            cINI.WriteString("Apagar", "Arq" + nro, caminhoPDF);
            cINI.WriteInt("Apagar", "Nro", nro);
        }

        private void ApagaRegistro()
        {
            glo.Loga($@"PD,{this.iID}, {this.UID}");
            contasAPagarDao.Exclui(this.iID.ToString(), this.CaminhoPDF);
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir os registros selecionados?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (dataGrid1.SelectedRows.Count == 1)
                {
                    ApagaRegistro();
                    string Arquivo = Convert.ToString(dataGrid1.SelectedRows[0].Cells["Arquivo"].Value);
                    ApagarArquivo(this.iID, Arquivo);
                } else
                {
                    foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                    {
                        this.iID = Convert.ToInt32(row.Cells["ID"].Value);
                        this.UID = Convert.ToString(row.Cells["UID"].Value);
                        string Arquivo = Convert.ToString(row.Cells["Arquivo"].Value);
                        ApagaRegistro();
                        ApagarArquivo(this.iID, Arquivo);
                    }
                }
                CarregaGridGenerico(dataGrid1, -1);
                Limpar();
                btLimparFiltro.Enabled = false;
                cmbForn.SelectedIndex = 0;
            }
        }

        private void ApagarArquivo(int numero, string Arquivo)
        {
            string fileExtension = Path.GetExtension(Arquivo).TrimStart('.');
            string destinationFileName = $"Doc{numero}.{fileExtension}";
            string destinationFilePath = Path.Combine(CaminhoBasePDF, destinationFileName);
            try
            {
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                    Console.WriteLine($"Arquivo apagado: {destinationFilePath}");
                }
                else
                {
                    Console.WriteLine($"Arquivo não encontrado: {destinationFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao apagar o arquivo: {ex.Message}");
            }
        }

        #endregion

        private void btObter_Click(object sender, EventArgs e)
        {
            btObter.Enabled = false;
            if (!Directory.Exists(CaminhoBasePDF))
            {
                Directory.CreateDirectory(CaminhoBasePDF);
            }
            foreach (string filePath in Directory.GetFiles(sourceDirectory))
            {
                try
                {
                    string UID = glo.GenerateUID();
                    string fileName = Path.GetFileName(filePath);
                    DateTime dataEmissao = DateTime.Now; 
                    int idAdic = contasAPagarDao.AdicObter(DateTime.Now, fileName, UID);
                    string fileExtension = Path.GetExtension(fileName).TrimStart('.');
                    string destinationFileName = $"Doc{idAdic}.{fileExtension}";
                    string destinationFilePath = Path.Combine(CaminhoBasePDF, destinationFileName);
                    File.Move(filePath, destinationFilePath);
                    AtualizaGrids();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao processar o arquivo {filePath}: {ex.Message}");
                }
            }
            btObter.Enabled = true;
        }
    }
}