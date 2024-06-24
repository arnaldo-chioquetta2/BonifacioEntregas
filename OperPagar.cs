using System;
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
        private string UID = "";
        private bool carregando = true;
        private string CaminhoPDF = "";
        private int iID = 0;
        private int idArquivo = 0;
        private bool CarregarT = true;
        private bool CarregarP = true;
        private string Apagar = "";
        string CaminhoBasePDF = "";
        string connectionStringPDF = "";

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
            CaminhoBasePDF = Path.GetDirectoryName(glo.CaminhoBase) + "\\Docs.mdb";
            connectionStringPDF = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CaminhoBasePDF + ";";
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
            dtpDataPagamento.Tag = "";
            dtpDataVencimento.Tag = "";
            dtpDataEmissao.Tag = "";
            txValorTotal.Tag = "";
            txDescricao.Tag = "";
            txObservacoes.Tag = "";
            ckPago.Tag = "";
        }

        private void VeSeHab(TextBox obj)
        {
            if (obj.Text.Length > 0)
            {
                obj.Tag = "M";
            }
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
            chPermanente.Enabled = ok;
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
            } else
            {
                if (btnAdicionar.Text == "Adicionar")
                {
                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    this.CaminhoPDF = openFileDialog.FileName;
                }
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
                    AtualizaGrids();
                }
                else
                {
                    string UID = glo.GenerateUID();
                    glo.Loga($@"PA,{idFornecedor}, {dataEmissao}, {dataVencimento}, {valorTotal}, {chaveNotaFiscal}, {descricao}, {this.CaminhoPDF}, {pago}, {dataPagamento}, {observacoes}, {perm}, {UID} ");
                    int idArquivo = 0;
                    if (perm)
                    {
                        idArquivo = GravaPDF();
                    }                     
                    contasAPagarDao.Adiciona(idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, this.CaminhoPDF, pago, dataPagamento, observacoes, perm, UID, idArquivo);
                    if (perm)
                    {
                        //MandarApagar();
                    }                        
                    Limpar();
                    if (tbContas.SelectedIndex == 0)
                    {
                        if (!perm)
                        {
                            CarregaGridGenerico(dataGrid1, 0);
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
                            CarregaGridGenerico(dataGrid2, -1);
                        }
                        else
                        {
                            CarregarT = true;
                        }
                    }
                }                
            }
        }

        #endregion

        #region PDF

        private void btPDF_Click(object sender, EventArgs e)
        {
            if (chPermanente.Checked)
            {
                AbreArquivo();
            }
            else
            {

                DialogResult result = MessageBox.Show("Deseja excluir o arquivo?\nPara mante-lo escolha Não", "Impressão",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MandarApagar();
                }
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

        private int GravaPDF()
        {
            string fileExtension = Path.GetExtension(this.CaminhoPDF).TrimStart('.');
            int insertedId = -1;
            byte[] fileContent = File.ReadAllBytes(this.CaminhoPDF);  
            using (OleDbConnection connection = new OleDbConnection(this.connectionStringPDF))
            {
                connection.Open();
                using (OleDbCommand insertCommand = new OleDbCommand("INSERT INTO Docs (Conteudo, ext) VALUES (?, ?)", connection))
                {
                    insertCommand.Parameters.AddWithValue("@Conteudo", fileContent);
                    insertCommand.Parameters.AddWithValue("@ext", fileExtension);
                    insertCommand.ExecuteNonQuery();
                }
                using (OleDbCommand selectCommand = new OleDbCommand("SELECT @@IDENTITY", connection))
                {
                    insertedId = Convert.ToInt32(selectCommand.ExecuteScalar());
                }
            }
            return insertedId;
        }

        private void AbreArquivo()
        {
            string query = "SELECT * FROM Docs WHERE ID = ?";
            string tempFolderPath = @"C:\Temp";
            string fileName = "DenisTemp";
            string fileExtension;
            string tempFilePath = "";
            try
            {
                using (OleDbConnection connection = new OleDbConnection(this.connectionStringPDF))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", this.idArquivo);
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                fileExtension = reader["ext"].ToString().Trim();
                                tempFilePath = Path.Combine(tempFolderPath, $"{fileName}.{fileExtension}");
                                long contentLength = reader.GetBytes(reader.GetOrdinal("Conteudo"), 0, null, 0, 0);
                                byte[] content = new byte[contentLength];
                                reader.GetBytes(reader.GetOrdinal("Conteudo"), 0, content, 0, content.Length);
                                File.WriteAllBytes(tempFilePath, content);  
                            }
                        }
                    }
                }
                Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });
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
            Grid.Columns[14].Visible = false;       // idArquivo
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
                this.idArquivo = Convert.ToInt32(selectedRow.Cells["idArquivo"].Value);
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

        private void AtualizaGrids()
        {
            if (tbContas.SelectedIndex == 0)
            {
                CarregaGridGenerico(dataGrid1, 0);
            }
            else
            {
                CarregaGridGenerico(dataGrid2, -1);
            }
        }

        #endregion

        private void CarregaGridGenerico(DataGridView dataGrid, int filter)
        {
            int idForne = 0;
            if (cmbForn.Tag=="M")
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
            DataTable dados = contasAPagarDao.GetDados(idForne, filter, dataPagamento, dataVencimento, dataEmissao, valorTotal, descricao, observacoes, pago);
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
                            }
                        }
                    }
                }
            }
        }

        private void btLimparFiltro_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void dataGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            Mostra(ref grid, ref e);
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
            if (tbContas.SelectedIndex == 0)
            {
                if (CarregarT)
                {
                    CarregaGridGenerico(dataGrid1, 0);
                }
            }
            else
            {
                if (CarregarP)
                {
                    CarregaGridGenerico(dataGrid2, -1);
                }
            }
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

        private void chPermanente_CheckedChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                chPermanente.Tag = "M";
            }
        }


        #endregion

        #region Deleção

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            File.Delete(this.Apagar);
            this.Apagar = "";
        }

        private void MandarApagar()
        {
            this.Apagar = this.CaminhoPDF;
            timer1.Enabled = true;
        }
        private void ApagaRegistro()
        {
            string sID = "";
            string UID = "";
            if (tbContas.SelectedIndex == 0)
            {
                sID = dataGrid1.SelectedRows[0].Cells["ID"].Value.ToString();
                UID = dataGrid1.SelectedRows[0].Cells["UID"].Value.ToString();
            }
            else
            {
                sID = dataGrid2.SelectedRows[0].Cells["ID"].Value.ToString();
                UID = dataGrid2.SelectedRows[0].Cells["UID"].Value.ToString();
            }
            glo.Loga($@"PD,{sID}, {UID}");
            contasAPagarDao.Exclui(sID, this.CaminhoPDF);
            MandarApagar();
            Limpar();
            AtualizaGrids();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ApagaRegistro();
            }
        }

        #endregion

    }
}