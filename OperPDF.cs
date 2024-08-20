using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class OperPDF : Form
    {
        private FornecedorDao Forn;
        private ContasAPagarDao contasAPagarDao;
        private FoldersDao FDao;
        private INI cINI;
        private string UID = "";
        private bool carregando = true;
        private string CaminhoPDF = "";
        
        private string sourceDirectory = "";
        private string CaminhoBasePDF = "";

        private int iIDe = 0;
        private int iID
        {
            get
            {
                return iIDe;
            }
            set
            {
                iIDe = value;
            }
        }

        #region Inicializacao

        public OperPDF()
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
            glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO", filtro: "EhForn = 1 ");
            FDao = new FoldersDao();
            InicializaIconesView();
            LoadTreeView();
            //CarregaGridGenerico(dataGrid1, 0, false);
            //CaminhoBasePDF = Path.GetDirectoryName(glo.CaminhoBase) + "\\Docs";
            //cINI = new INI();
            //sourceDirectory = cINI.ReadString("Config", "Docs", "");
            //rt.AdjustFormComponents(this);
            //carregando = false;            
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
            btMudar.Enabled = false;
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
            btPDF.Enabled = btEmail.Enabled = false;            
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
                //DataGridView targetGrid = tabControl1.SelectedIndex == 0 ? dataGrid1 : dataGrid2;
                //EditarRegistro(targetGrid, idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, pago, dataPagamento, observacoes);
                //CarregaGridGenerico(targetGrid, 0, (tabControl1.SelectedIndex == 1));
            }
        }

        private void EditarRegistro(DataGridView dataGridView, int idFornecedor, DateTime dataEmissao, DateTime dataVencimento, float valorTotal, string chaveNotaFiscal, string descricao, bool pago, DateTime? dataPagamento, string observacoes)
        {
            int selectedId = Convert.ToInt32(dataGridView.CurrentRow.Cells["ID"].Value);
            int scrollPosition = dataGridView.FirstDisplayedScrollingRowIndex;
            glo.Loga($@"PE,{idFornecedor}, {dataEmissao:yyyy-MM-dd}, {dataVencimento:yyyy-MM-dd}, {valorTotal}, {chaveNotaFiscal}, {descricao}, {pago}, {dataPagamento:yyyy-MM-dd}, {observacoes}, {UID}");
            contasAPagarDao.Edita(this.iID, idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, pago, dataPagamento, observacoes);
            Limpar();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (Convert.ToInt32(row.Cells["ID"].Value) == selectedId)
                {
                    dataGridView.CurrentCell = row.Cells[0];
                    row.Selected = true;
                    break;
                }
            }
            if (scrollPosition > -1 && scrollPosition < dataGridView.RowCount)
                dataGridView.FirstDisplayedScrollingRowIndex = scrollPosition;
        }

        #endregion

        #region PDF

        private void btPDF_Click(object sender, EventArgs e)
        {
            //DataGridView targetGrid = tabControl1.SelectedIndex == 0 ? dataGrid1 : dataGrid2;
            //foreach (DataGridViewRow row in targetGrid.SelectedRows)
            //{
            //    int idArquivo = (int)row.Cells["ID"].Value;
            //    string ArquivoOrig = (string)row.Cells["Arquivo"].Value; 
            //    AbreArquivo(idArquivo, ArquivoOrig);
            //    contasAPagarDao.Imprimiu(idArquivo);
            //    row.DefaultCellStyle.BackColor = Color.LightGreen;
            //}
            //targetGrid.Refresh();
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
            Grid.Columns[0].Width = 200;            // Arquivo
            Grid.Columns[1].Visible = false;        // ID
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
            if (rt.IsLargeScreen())
            {
                for (int i = 0; i < 12; i++)
                {
                    Grid.Columns[i].Width = (int)(Grid.Columns[i].Width * rt.scaleFactor);
                }
            }
            Grid.Invalidate();
        }

        private void dataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            Mostra(ref grid, ref e);
            btnAdicionar.Enabled = true;
            btLimparFiltro.Enabled = true;
            btExcluir.Enabled = true;
            btMudar.Enabled = true;            
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

                // btPDF.Enabled = true;
                btPDF.Enabled = btEmail.Enabled = true;

                if (selectedRow.Cells["idFornecedor"].Value != DBNull.Value)
                {
                    cmbForn.SelectedValue = Convert.ToInt32(selectedRow.Cells["idFornecedor"].Value);
                }
                if (selectedRow.Cells["DataVencimento"].Value != DBNull.Value)
                {
                    dtpDataVencimento.Value = Convert.ToDateTime(selectedRow.Cells["DataVencimento"].Value);
                }
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
                carregando = false;
            }
        }

        private void AtualizaGrids()
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    if (tabPage1.Tag == "A")
            //    {
            //        CarregaGridGenerico(dataGrid1, 0, false);
            //        tabPage1.Tag = "";
            //    }
            //}
            //else
            //{
            //    if (tabPage2.Tag == "A")
            //    {
            //        CarregaGridGenerico(dataGrid2, 0, true);
            //        tabPage2.Tag = "";
            //    }
            //}           
        }

        #endregion

        private void CarregaGridGenerico(DataGridView dataGrid, int filter, bool Perm)
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
            DataTable dados = contasAPagarDao.GetDados(Perm, idForne, dataPagamento, dataVencimento, dataEmissao, valorTotal, descricao, observacoes, pago);
            dataGrid.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (!row.Cells["idArquivo"].Value.Equals(DBNull.Value))
                {
                    if (row.Cells["idArquivo"].Value.ToString()=="1")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                }
            }
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

        private void ExcluirRegistros(DataGridView dataGridView, int tabIndex)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                this.iID = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["ID"].Value);
                string arquivo = Convert.ToString(dataGridView.SelectedRows[0].Cells["Arquivo"].Value);
                ApagaRegistro();  
                ApagarArquivo(this.iID, arquivo);
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView.SelectedRows)
                {
                    this.iID = Convert.ToInt32(row.Cells["ID"].Value);
                    this.UID = Convert.ToString(row.Cells["UID"].Value);
                    string arquivo = Convert.ToString(row.Cells["Arquivo"].Value);
                    ApagaRegistro();
                    ApagarArquivo(this.iID, arquivo);
                }
            }
            CarregaGridGenerico(dataGridView, -1, tabIndex == 1);
        }


        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir os registros selecionados?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                //DataGridView targetGrid = tabControl1.SelectedIndex == 0 ? dataGrid1 : dataGrid2;
                //ExcluirRegistros(targetGrid, tabControl1.SelectedIndex);
                //CarregaGridGenerico(dataGrid1, -1, (tabControl1.SelectedIndex == 1));
                //Limpar();
                //btLimparFiltro.Enabled = false;
                //cmbForn.SelectedIndex = 0;
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
                AdicionarAoArquivoINI(destinationFilePath);
            }
        }

        #endregion

        private void btObter_Click(object sender, EventArgs e)
        {
            btObter.Enabled = false;
            if (!Directory.Exists(sourceDirectory))
            {
                MessageBox.Show("A pasta não existe ou não é acessível no momento.");
            } else
            {
                bool Perm = false;
                //if (tabControl1.SelectedIndex == 0)
                //{
                //    tabPage1.Tag = "A";
                //}
                //else
                //{
                //    tabPage2.Tag = "A";
                //    Perm = true;
                //}
                foreach (string filePath in Directory.GetFiles(sourceDirectory))
                {
                    try
                    {
                        string UID = glo.GenerateUID();
                        string fileName = Path.GetFileName(filePath);
                        DateTime dataEmissao = DateTime.Now;
                        int idAdic = contasAPagarDao.AdicObter(Perm, DateTime.Now, fileName, UID);
                        string fileExtension = Path.GetExtension(fileName).TrimStart('.');
                        string destinationFileName = $"Doc{idAdic}.{fileExtension}";
                        string destinationFilePath = Path.Combine(CaminhoBasePDF, destinationFileName);
                        File.Move(filePath, destinationFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao processar o arquivo {filePath}: {ex.Message}");
                    }
                }
                AtualizaGrids();
            }
            btObter.Enabled = true;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex==0)
            //{
            //    if (tabPage1.Tag=="A")
            //    {
            //        CarregaGridGenerico(dataGrid1, 0, false);
            //        tabPage1.Tag = "";
            //    }
            //} else
            //{
            //    if (tabPage2.Tag == "A")
            //    {
            //        CarregaGridGenerico(dataGrid2, 0, true);
            //        tabPage2.Tag = "";
            //    }
            //}
        }

        private void btMudar_Click(object sender, EventArgs e)
        {
            //contasAPagarDao.Muda(this.iID, (tabControl1.SelectedIndex == 0));            
            //CarregaGridGenerico((tabControl1.SelectedIndex == 0 ? dataGrid1 : dataGrid2), 0, (tabControl1.SelectedIndex==1));
            //Limpar();
            //if (tabControl1.SelectedIndex==0)
            //{
            //    tabPage2.Tag = "A";
            //} else
            //{
            //    tabPage1.Tag = "A";
            //}
        }

        private void btEmail_Click(object sender, EventArgs e)
        {
            pesEmails OForm = new pesEmails();                      
            //int idArquivo = Convert.ToInt32(dataGrid1.SelectedRows[0].Cells["ID"].Value.ToString());
            //string ArquivoOrig = dataGrid1.SelectedRows[0].Cells["Arquivo"].Value.ToString();

            //// AbreArquivo(idArquivo, ArquivoOrig);
            //string fileNamePrefix = "Doc";
            //string fileExtension = Path.GetExtension(ArquivoOrig).TrimStart('.');
            //string sourceFilePath = Path.Combine(CaminhoBasePDF, $"{fileNamePrefix}{idArquivo}.{fileExtension}");

            //// OForm.Arquivo = Arquivo;
            //OForm.Arquivo = sourceFilePath;
            //OForm.Show();
            //AdicionarAoArquivoINI(sourceFilePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OperPagar fPagar = new OperPagar();
            fPagar.Show();
            Close();
        }

        #region TreeView

        private void InicializaIconesView()
        {
            // Define o ImageList para o TreeView
            treeView1.ImageList = imageList2; // Referencia o ImageList configurado no designer

            // Suponha que você tenha os IDs das pastas "Permanentes" e "Temporários"
            int permanentesFolderId = 1;
            int temporariosFolderId = 2;

            // Adicionar pastas ao TreeView
            TreeNode permanentesNode = new TreeNode("Permanentes")
            {
                ImageKey = "generico", // "FolderFechado64",     // Nome da imagem de pasta fechada no ImageList
                SelectedImageKey = "generico", // "FolderAberto64", // Nome da imagem de pasta aberta no ImageList
                Tag = permanentesFolderId          // Armazena o FolderID no Tag
            };

            TreeNode temporariosNode = new TreeNode("Temporários")
            {
                ImageKey = "generico", // "FolderFechado64",
                SelectedImageKey = "generico", // "FolderAberto64",
                Tag = temporariosFolderId          // Armazena o FolderID no Tag
            };

            // Adicionar os nós ao TreeView
            treeView1.Nodes.Add(permanentesNode);
            treeView1.Nodes.Add(temporariosNode);

            // Carregar documentos para cada pasta
            List<tb.Document> permanentesDocuments = FDao.GetDocuments((int)permanentesNode.Tag);
            AddDocumentNodes(permanentesNode, permanentesDocuments);

            List<tb.Document> temporariosDocuments = FDao.GetDocuments((int)temporariosNode.Tag);
            AddDocumentNodes(temporariosNode, temporariosDocuments);

            // Evento de expansão para trocar ícones ao abrir/fechar a pasta
            treeView1.BeforeExpand += (sender, e) =>
            {
                e.Node.ImageKey = "generico"; //  "FolderAberto64"; // Ícone de pasta aberta quando expandida
            };

            treeView1.BeforeCollapse += (sender, e) =>
            {
                e.Node.ImageKey = "generico";  //"FolderFechado64"; // Ícone de pasta fechada quando colapsada
            };
        }

        private void AddDocumentNodes(TreeNode parentNode, List<tb.Document> documents)
        {
            foreach (var doc in documents)
            {
                string extension = Path.GetExtension(doc.DocumentName);
                string iconKey = GetIconKeyByExtension(extension);

                TreeNode docNode = new TreeNode(doc.DocumentName)
                {
                    ImageIndex = imageList1.Images.IndexOfKey(iconKey),
                    SelectedImageIndex = imageList1.Images.IndexOfKey(iconKey),
                    StateImageIndex = imageList1.Images.IndexOfKey(iconKey),
                    Tag = doc.DocumentID
                };
                parentNode.Nodes.Add(docNode);
            }
        }        

        private string GetIconKeyByExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".pdf":
                    return "pdf64";
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".bmp":
                case ".gif":
                    return "imagens64";
                case ".doc":
                case ".docx":
                    return "word64";
                case ".txt":
                case ".csv":
                case ".log":
                    return "arquivo64";
                default:
                    return "arquivo64"; // Usar o ícone genérico para outros tipos de arquivo
            }
        }        

        private void LoadTreeView()
        {
            treeView1.Nodes.Clear();
            var rootFolders = FDao.GetRootFolders(); // Método para obter pastas raiz
            foreach (var folder in rootFolders)
            {
                var rootNode = new TreeNode(folder.FolderName) { Tag = folder.FolderID };
                LoadSubFoldersAndDocuments(rootNode, folder.FolderID);
                treeView1.Nodes.Add(rootNode);
            }
        }

        private void LoadSubFoldersAndDocuments(TreeNode parentNode, int folderId)
        {
            var subFolders = FDao.GetSubFolders(folderId);
            foreach (var subFolder in subFolders)
            {
                var childNode = new TreeNode(subFolder.FolderName)
                {
                    Tag = subFolder.FolderID,
                    ImageIndex = imageList1.Images.IndexOfKey("FolderFechado64"),
                    SelectedImageIndex = imageList1.Images.IndexOfKey("FolderAberto64")
                };
                LoadSubFoldersAndDocuments(childNode, subFolder.FolderID);
                parentNode.Nodes.Add(childNode);
            }

            var documents = FDao.GetDocuments(folderId);
            foreach (var doc in documents)
            {
                string extension = Path.GetExtension(doc.DocumentName);
                string iconKey = GetIconKeyByExtension(extension);
                var docNode = new TreeNode(doc.DocumentName)
                {
                    Tag = doc.DocumentID,
                    ImageIndex = imageList1.Images.IndexOfKey(iconKey),
                    SelectedImageIndex = imageList1.Images.IndexOfKey(iconKey)
                };
                parentNode.Nodes.Add(docNode);
            }
        }        

        #endregion

    }

}

