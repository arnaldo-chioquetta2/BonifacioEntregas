using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
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
        private Color Verde = Color.FromArgb(215, 255, 215);
        
        private string sourceDirectory = "";

        private string caminhoBasePDF = "";

        public string CaminhoBasePDF
        {
            get { return caminhoBasePDF; }
            set
            {
                // Verifica se a pasta existe, se não, cria a pasta
                if (!Directory.Exists(value))
                {
                    Directory.CreateDirectory(value);
                }
                caminhoBasePDF = value;
            }
        }

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
            CaminhoBasePDF = Path.GetDirectoryName(glo.CaminhoBase) + "\\Docs";
            cINI = new INI();
            sourceDirectory = cINI.ReadString("Config", "Docs", "");
            rt.AdjustFormComponents(this);
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

        #region Filtro

        private void btFiltrar_Click(object sender, EventArgs e)
        {
            // AtualizaGrids();
        }

        #endregion

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
            //if (dataGridView.SelectedRows.Count == 1)
            //{
            //    this.iID = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["ID"].Value);
            //    string arquivo = Convert.ToString(dataGridView.SelectedRows[0].Cells["Arquivo"].Value);
            //    ApagaRegistro();  
            //    ApagarArquivo(this.iID, arquivo);
            //}
            //else
            //{
            //    foreach (DataGridViewRow row in dataGridView.SelectedRows)
            //    {
            //        this.iID = Convert.ToInt32(row.Cells["ID"].Value);
            //        this.UID = Convert.ToString(row.Cells["UID"].Value);
            //        string arquivo = Convert.ToString(row.Cells["Arquivo"].Value);
            //        ApagaRegistro();
            //        ApagarArquivo(this.iID, arquivo);
            //    }
            //}
            //CarregaGridGenerico(dataGridView, -1, tabIndex == 1);
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

        #region Eventos

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

        //private void btObter_Click(object sender, EventArgs e)
        //{
        //    btStripObter.Enabled = false;

        //    try
        //    {
        //        if (!Directory.Exists(sourceDirectory))
        //        {
        //            MessageBox.Show("A pasta não existe ou não é acessível no momento.");
        //            return;
        //        }

        //        // Verificar se existe uma pasta chamada "Nova" na TreeView
        //        TreeNode novaNode = null;
        //        foreach (TreeNode node in treeView1.Nodes)
        //        {
        //            if (node.Text == "Nova")
        //            {
        //                novaNode = node;
        //                break;
        //            }
        //        }

        //        // Se a pasta "Nova" não existir, criar e adicioná-la como a primeira
        //        if (novaNode == null)
        //        {
        //            novaNode = new TreeNode("Nova");
        //            treeView1.Nodes.Insert(0, novaNode); // Insere na primeira posição
        //        }

        //        // Processar os documentos na pasta monitorada
        //        foreach (string filePath in Directory.GetFiles(sourceDirectory))
        //        {
        //            try
        //            {
        //                string UID = glo.GenerateUID();
        //                string fileName = Path.GetFileName(filePath);
        //                DateTime dataEmissao = DateTime.Now;
        //                int idAdic = contasAPagarDao.AdicObter(false, DateTime.Now, fileName, UID);
        //                string fileExtension = Path.GetExtension(fileName).TrimStart('.');
        //                string destinationFileName = $"Doc{idAdic}.{fileExtension}";
        //                string destinationFilePath = Path.Combine(CaminhoBasePDF, destinationFileName);
        //                File.Move(filePath, destinationFilePath);
        //                TreeNode docNode = new TreeNode(destinationFileName);
        //                novaNode.Nodes.Add(docNode);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"Erro ao processar o arquivo {filePath}: {ex.Message}");
        //            }
        //        }

        //        // Expandir a pasta "Nova" para mostrar os novos documentos
        //        novaNode.Expand();
        //        treeView1.SelectedNode = novaNode; // Selecionar o nó "Nova"

        //        // Atualizar a TreeView
        //        treeView1.Refresh();
        //    }
        //    finally
        //    {
        //        btStripObter.Enabled = true;
        //    }
        //}

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

        #endregion

        #region TreeView

        private void InicializaIconesView()
        {
            // Remova a referência ao ImageList
            // treeView1.ImageList = null;
            if (treeView1.ImageList == null)
            {
                treeView1.ImageList = new ImageList();
                treeView1.ImageList.ColorDepth = ColorDepth.Depth32Bit;
                treeView1.ImageList.ImageSize = new Size(32, 32);
            }

            int permanentesFolderId = 1;
            int temporariosFolderId = 2;

            TreeNode permanentesNode = new TreeNode("Permanentes")
            {
                Tag = permanentesFolderId
            };

            TreeNode temporariosNode = new TreeNode("Temporários")
            {
                Tag = temporariosFolderId
            };

            treeView1.Nodes.Add(permanentesNode);
            treeView1.Nodes.Add(temporariosNode);

            List<tb.Document> permanentesDocuments = FDao.GetDocuments((int)permanentesNode.Tag);
            AddDocumentNodes(permanentesNode, permanentesDocuments);

            List<tb.Document> temporariosDocuments = FDao.GetDocuments((int)temporariosNode.Tag);
            AddDocumentNodes(temporariosNode, temporariosDocuments);

            // Não é mais necessário definir eventos BeforeExpand e BeforeCollapse para trocar ícones
            treeView1.ImageList.Images.Add(GetFolderIcon());
        }

        private void AddDocumentNodes(TreeNode parentNode, List<tb.Document> documents)
        {
            foreach (var doc in documents)
            {
                // Aplicar a cor verde claro aos documentos já existentes
                AddNodeWithIcon(parentNode, doc.DocumentName, doc.DocumentID.ToString(), isBold: false, addAsFirstNode: false, backColor: Verde);
            }
        }
        //private void AddDocumentNodes(TreeNode parentNode, List<tb.Document> documents)
        //{
        //    foreach (var doc in documents)
        //    {
        //        AddNodeWithIcon(parentNode, doc.DocumentName, doc.DocumentID.ToString(), isBold: false, addAsFirstNode: false);
        //    }
        //}

        private void AddNodeWithIcon(TreeNode parentNode, string nodeName, string filePath, object tag = null, bool isBold = false, bool addAsFirstNode = false, Color? backColor = null)
        {
            string extension = Path.GetExtension(nodeName);
            Icon icon = GetIconByExtension(extension);

            TreeNode newNode = new TreeNode(nodeName)
            {
                Tag = tag,
                BackColor = backColor ?? Color.Transparent // Aplica a cor de fundo, se fornecida
            };

            if (isBold)
            {
                newNode.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
            }

            if (icon != null)
            {
                // Converter o Icon para Bitmap
                using (Bitmap bmp = icon.ToBitmap())
                {
                    // Adicionar o Bitmap ao ImageList
                    int imageIndex = treeView1.ImageList.Images.Add(bmp, Color.Transparent);
                    newNode.ImageIndex = imageIndex;
                    newNode.SelectedImageIndex = imageIndex;
                }
            }

            if (addAsFirstNode)
            {
                parentNode.Nodes.Insert(0, newNode); // Adiciona como o primeiro nó
            }
            else
            {
                parentNode.Nodes.Add(newNode); // Adiciona como o último nó
            }
        }
        //private void AddNodeWithIcon(TreeNode parentNode, string nodeName, string filePath, object tag = null, bool isBold = false, bool addAsFirstNode = false)
        //{
        //    string extension = Path.GetExtension(nodeName);
        //    Icon icon = GetIconByExtension(extension);

        //    TreeNode newNode = new TreeNode(nodeName)
        //    {
        //        Tag = tag
        //    };

        //    if (isBold)
        //    {
        //        newNode.Text = nodeName + "  "; // Add a space at the end
        //        newNode.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
        //    }

        //    if (icon != null)
        //    {
        //        // Converter o Icon para Bitmap
        //        using (Bitmap bmp = icon.ToBitmap())
        //        {
        //            // Adicionar o Bitmap ao ImageList
        //            int imageIndex = treeView1.ImageList.Images.Add(bmp, Color.Transparent);
        //            newNode.ImageIndex = imageIndex;
        //            newNode.SelectedImageIndex = imageIndex;
        //        }
        //    }

        //    if (addAsFirstNode)
        //    {
        //        parentNode.Nodes.Insert(0, newNode); // Adiciona como o primeiro nó
        //    }
        //    else
        //    {
        //        parentNode.Nodes.Add(newNode); // Adiciona como o último nó
        //    }
        //}        

        private Icon GetIconByExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension)) // Se for pasta (não tem extensão)
            {
                return GetFolderIcon();
            }
            return GetFileIcon(extension);
        }

        private Icon GetFileIcon(string extension)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hImgLarge = Win32.SHGetFileInfo(extension, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_USEFILEATTRIBUTES | Win32.SHGFI_LARGEICON);

            if (shinfo.hIcon == IntPtr.Zero)
            {
                return System.Drawing.SystemIcons.Application;
            }

            Icon icon = Icon.FromHandle(shinfo.hIcon);
            try
            {
                return (Icon)icon.Clone();
            }
            finally
            {
                Win32.DestroyIcon(shinfo.hIcon);
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
                // Criar um nó de pasta personalizado
                var childNode = new FolderNode(subFolder.FolderName)
                {
                    Tag = subFolder.FolderID
                };

                LoadSubFoldersAndDocuments(childNode, subFolder.FolderID);
                parentNode.Nodes.Add(childNode);
            }

            var documents = FDao.GetDocuments(folderId);
            foreach (var doc in documents)
            {
                string extension = Path.GetExtension(doc.DocumentName);
                using (Icon icon = GetIconByExtension(extension))
                {
                    var docNode = new TreeNode(doc.DocumentName)
                    {
                        Tag = doc.DocumentID
                    };

                    using (Bitmap bmp = icon.ToBitmap())
                    {
                        int imageIndex = treeView1.ImageList.Images.Add(bmp, Color.Transparent);
                        docNode.ImageIndex = imageIndex;
                        docNode.SelectedImageIndex = imageIndex;
                    }

                    parentNode.Nodes.Add(docNode);
                }
            }
        }

        private Icon GetFolderIcon()
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hImgLarge = Win32.SHGetFileInfo(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                Win32.FILE_ATTRIBUTE_DIRECTORY,
                ref shinfo,
                (uint)Marshal.SizeOf(shinfo),
                Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON | Win32.SHGFI_USEFILEATTRIBUTES);

            if (shinfo.hIcon == IntPtr.Zero)
            {
                // Caso falhe, use um ícone de diretório padrão
                return Icon.ExtractAssociatedIcon(Environment.GetFolderPath(Environment.SpecialFolder.System));
            }

            Icon icon = Icon.FromHandle(shinfo.hIcon);
            try
            {
                return (Icon)icon.Clone();
            }
            finally
            {
                Win32.DestroyIcon(shinfo.hIcon);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        static class Win32
        {
            public const uint SHGFI_ICON = 0x000000100;
            public const uint SHGFI_LARGEICON = 0x000000000;
            public const uint SHGFI_SMALLICON = 0x000000001;
            public const uint SHGFI_SYSICONINDEX = 0x000004000;
            public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

            public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;

            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

            [DllImport("user32.dll")]
            public static extern bool DestroyIcon(IntPtr hIcon);
        }

        public class FolderNode : TreeNode
        {
            public FolderNode(string name) : base(name)
            {
                // Definir o ícone de pasta para o nó
                ImageIndex = 0;
                SelectedImageIndex = 0;
            }
        }

        #endregion

        #region Obtenção        

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = e.Node;
            }
        }

        private void btStripObter_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                // Obtém o nó selecionado na TreeView
                TreeNode selectedNode = treeView1.SelectedNode;

                // Chama o método para processar a obtenção dos arquivos
                btObter_Click(selectedNode);
            }
        }

        private void btObter_Click(TreeNode selectedNode)
        {

            try
            {
                if (!Directory.Exists(sourceDirectory))
                {
                    MessageBox.Show("A pasta não existe ou não é acessível no momento.");
                    return;
                }
                int c = 0;
                // Processar os documentos na pasta monitorada
                foreach (string filePath in Directory.GetFiles(sourceDirectory))
                {
                    try
                    {
                        string UID = glo.GenerateUID();
                        string fileName = Path.GetFileName(filePath);
                        DateTime dataEmissao = DateTime.Now;

                        // Inserir na base de dados associando à pasta correta
                        int idAdic = contasAPagarDao.AdicObterComPasta(dataEmissao, fileName, UID, selectedNode.Tag);

                        string fileExtension = Path.GetExtension(fileName).TrimStart('.');
                        string destinationFileName = $"Doc{idAdic}.{fileExtension}";
                        string destinationFilePath = Path.Combine(CaminhoBasePDF, destinationFileName);
                        File.Move(filePath, destinationFilePath);

                        AddNodeWithIcon(selectedNode, fileName, destinationFilePath, idAdic, true, true);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao processar o arquivo {filePath}: {ex.Message}");
                    }
                }

                // Expandir a pasta selecionada para mostrar os novos documentos
                selectedNode.Expand();
                treeView1.SelectedNode = selectedNode; // Selecionar o nó da pasta

                // Atualizar a TreeView
                treeView1.Refresh();
            }
            finally
            {
                
            }
        }

        #endregion

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {            
            TreeNode selectedNode = e.Node;
            if (selectedNode != null && selectedNode.Tag is int idArquivo)
            {
                string fileName = selectedNode.Text;
                AbreArquivo(idArquivo, fileName);
                selectedNode.BackColor = Verde;
                selectedNode.NodeFont = new Font(treeView1.Font, FontStyle.Regular);
                contasAPagarDao.Imprimiu(idArquivo);
            }           
        }
    }

}

