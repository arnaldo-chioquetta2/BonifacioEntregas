using System;
using System.Collections.Generic;
using System.Data;
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
        private string nmNo = "";
        private TreeNode selectedNodeToMove;
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
            btFiltrar.Enabled = true;
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
                EditarRegistro(idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, pago, dataPagamento, observacoes);
            }
        }

        private void EditarRegistro(int idFornecedor, DateTime dataEmissao, DateTime dataVencimento, float valorTotal, string chaveNotaFiscal, string descricao, bool pago, DateTime? dataPagamento, string observacoes)
        {
            int selectedId = this.iID;
            glo.Loga($@"PE,{idFornecedor}, {dataEmissao:yyyy-MM-dd}, {dataVencimento:yyyy-MM-dd}, {valorTotal}, {chaveNotaFiscal}, {descricao}, {pago}, {dataPagamento:yyyy-MM-dd}, {observacoes}, {UID}");
            contasAPagarDao.Edita(this.iID, idFornecedor, dataEmissao, dataVencimento, valorTotal, chaveNotaFiscal, descricao, pago, dataPagamento, observacoes);
            Limpar();
        }

        #endregion

        #region PDF

        private void btPDF_Click(object sender, EventArgs e)
        {
            AbreArquivo();
            contasAPagarDao.Imprimiu(this.iID);

            // Procurar nó baseado no texto
            TreeNode nodeToColor = FindNodeByText(treeView1.Nodes, this.nmNo);
            if (nodeToColor != null)
            {
                nodeToColor.BackColor = Verde;
                nodeToColor.NodeFont = new Font(treeView1.Font, FontStyle.Regular);
            }
        }

        // Método para encontrar um nó por seu texto
        private TreeNode FindNodeByText(TreeNodeCollection nodes, string searchText)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text == searchText)
                {
                    return node;
                }
                TreeNode foundNode = FindNodeByText(node.Nodes, searchText);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }
            return null;
        }


        private void AbreArquivo()
        {
            string fileNamePrefix = "Doc";
            string fileExtension = Path.GetExtension(this.nmNo).TrimStart('.');
            string sourceFilePath = Path.Combine(CaminhoBasePDF, $"{fileNamePrefix}{this.iID}.{fileExtension}");
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
            // Parâmetros de filtragem
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

            // Limpar a TreeView antes de aplicar o filtro
            treeView1.Nodes.Clear();

            // Recarregar a TreeView com base nos critérios de filtro
            var rootFolders = FDao.GetRootFolders(); // Método para obter pastas raiz
            foreach (var folder in rootFolders)
            {
                var rootNode = new TreeNode(folder.FolderName) { Tag = folder.FolderID };
                LoadFilteredSubFoldersAndDocuments(rootNode, folder.FolderID, idForne, dataPagamento, dataVencimento, dataEmissao, valorTotal, descricao, observacoes, pago);

                // Adicionar o nó raiz apenas se ele tiver subnós (documentos ou pastas)
                if (rootNode.Nodes.Count > 0)
                {
                    treeView1.Nodes.Add(rootNode);
                    rootNode.Expand(); // Expandir a raiz se tiver documentos
                }
            }
        }

        private void LoadFilteredSubFoldersAndDocuments(TreeNode parentNode, int folderId, int idForne, DateTime? dataPagamento, DateTime? dataVencimento, DateTime? dataEmissao, string valorTotal, string descricao, string observacoes, bool? pago)
        {
            var subFolders = FDao.GetSubFolders(folderId);
            bool folderHasDocuments = false;

            foreach (var subFolder in subFolders)
            {
                var childNode = new FolderNode(subFolder.FolderName)
                {
                    Tag = subFolder.FolderID
                };

                LoadFilteredSubFoldersAndDocuments(childNode, subFolder.FolderID, idForne, dataPagamento, dataVencimento, dataEmissao, valorTotal, descricao, observacoes, pago);

                // Só adicionar o nó se ele tiver documentos ou subpastas com documentos
                if (childNode.Nodes.Count > 0)
                {
                    parentNode.Nodes.Add(childNode);
                    folderHasDocuments = true;
                }
            }

            var documents = FDao.GetFilteredDocuments(folderId, idForne, dataPagamento, dataVencimento, dataEmissao, valorTotal, descricao, observacoes, pago);
            foreach (var doc in documents)
            {
                string extension = Path.GetExtension(doc.DocumentName);
                using (Icon icon = GetIconByExtension(extension))
                {
                    var docNode = new TreeNode(doc.DocumentName)
                    {
                        Tag = doc.DocumentID
                    };

                    if (doc.idArquivo == 1)
                    {
                        docNode.BackColor = Verde;
                    }

                    using (Bitmap bmp = icon.ToBitmap())
                    {
                        int imageIndex = treeView1.ImageList.Images.Add(bmp, Color.Transparent);
                        docNode.ImageIndex = imageIndex;
                        docNode.SelectedImageIndex = imageIndex;
                    }

                    parentNode.Nodes.Add(docNode);
                    folderHasDocuments = true;
                }
            }

            // Expandir o nó se tiver documentos
            if (folderHasDocuments)
            {
                parentNode.Expand();
            }
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
            contasAPagarDao.Exclui(this.iID.ToString());
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir os registros selecionados?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ApagaRegistro();
                ApagarArquivo(this.iID, this.nmNo);
                Limpar();
                btLimparFiltro.Enabled = false;
                cmbForn.SelectedIndex = 0;
                if (treeView1.SelectedNode != null)
                {
                    treeView1.SelectedNode.Remove();
                }
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
            LoadTreeView();
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
            // Torna o painel visível
            panel2.Visible = true;
            selectedNodeToMove = treeView1.SelectedNode;

            // Limpa o TreeView2 antes de preenchê-lo
            treeView2.Nodes.Clear();

            // Define o tamanho do ícone como metade do tamanho dos ícones no TreeView principal
            float iz = treeView1.ImageList.ImageSize.Width * 2 / 3;

            int iconSize = Convert.ToInt32(iz);
            // int iconSize = treeView1.ImageList.ImageSize.Width / 2;

            treeView2.ImageList = new ImageList();
            treeView2.ImageList.ImageSize = new Size(iconSize, iconSize);
            treeView2.ImageList.ColorDepth = ColorDepth.Depth32Bit;

            // Adicionar o ícone de pasta ao ImageList
            Icon folderIcon = GetFolderIcon();
            using (Bitmap bmp = folderIcon.ToBitmap())
            {
                treeView2.ImageList.Images.Add(bmp);
            }

            // Carregar apenas as pastas do TreeView1 no TreeView2
            TreeNode currentParentNode = treeView1.SelectedNode?.Parent;
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (IsFolderNode(node) && node != currentParentNode) // Verifica se o nó é uma pasta
                {
                    TreeNode newFolderNode = new TreeNode(node.Text)
                    {
                        Tag = node.Tag,
                        ImageIndex = 0, // Usar o índice do ícone de pasta no ImageList
                        SelectedImageIndex = 0
                    };
                    treeView2.Nodes.Add(newFolderNode);
                    CopySubFolders(node, newFolderNode);
                }
            }
        }

        private void CopySubFolders(TreeNode sourceNode, TreeNode targetNode)
        {
            foreach (TreeNode subNode in sourceNode.Nodes)
            {
                if (IsFolderNode(subNode)) // Verifica se o subnó é uma pasta
                {
                    TreeNode newSubFolderNode = new TreeNode(subNode.Text)
                    {
                        Tag = subNode.Tag,
                        ImageIndex = 0, // Usar o índice do ícone de pasta no ImageList
                        SelectedImageIndex = 0
                    };
                    targetNode.Nodes.Add(newSubFolderNode);
                    CopySubFolders(subNode, newSubFolderNode);
                }
            }
        }

        // Método para determinar se o nó é uma pasta
        private bool IsFolderNode(TreeNode node)
        {
            return node.Nodes.Count > 0 || node.Tag is int;
            // return node.Nodes.Count > 0; // Se o nó tem subnós, consideramos que é uma pasta
        }


        private void btEmail_Click(object sender, EventArgs e)
        {
            pesEmails OForm = new pesEmails();
            string fileNamePrefix = "Doc";
            string fileExtension = Path.GetExtension(this.nmNo.TrimStart('.'));
            string sourceFilePath = Path.Combine(CaminhoBasePDF, $"{fileNamePrefix}{this.iID}.{fileExtension}");
            OForm.Arquivo = sourceFilePath;
            OForm.Show();
            AdicionarAoArquivoINI(sourceFilePath);
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
                AddNodeWithIcon(parentNode, doc.DocumentName, doc.DocumentID.ToString());
            }
        }

        private void AddNodeWithIcon(TreeNode parentNode, string nodeName, string filePath, object tag = null, bool isBold = false, bool addAsFirstNode = false, Color? backColor = null)
        {
            string extension = Path.GetExtension(nodeName);
            Icon icon = GetIconByExtension(extension);

            TreeNode newNode = new TreeNode(nodeName)
            {
                Tag = tag
            };

            if (isBold)
            {
                newNode.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
            }
            if (backColor!=null)
            {
                newNode.BackColor = Verde;
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

                    if (doc.idArquivo == 1)
                    {
                        // Definir a cor de fundo como verde se idArquivo for 1
                        docNode.BackColor = Verde;
                    }

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
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show("Por favor, selecione uma pasta onde a nova subpasta será criada.");
                return;
            }

            // Verificar se o nó está no primeiro nível
            bool isFirstLevel = selectedNode.Parent == null;

            // Se for o primeiro nível, perguntar se a nova pasta deve ser criada no mesmo nível ou como subpasta
            if (isFirstLevel)
            {
                DialogResult result = MessageBox.Show("Deseja criar a nova pasta no mesmo nível ou como subpasta?",
                                                      "Criar Nova Pasta",
                                                      MessageBoxButtons.YesNoCancel,
                                                      MessageBoxIcon.Question,
                                                      MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    // Criar a pasta no mesmo nível (como irmã)
                    CriarPastaNoMesmoNivel(selectedNode);
                }
                else if (result == DialogResult.No)
                {
                    // Criar a pasta como subpasta (comportamento atual)
                    CriarSubPasta(selectedNode);
                }
            }
            else
            {
                // Para outros níveis, criar como subpasta (comportamento atual)
                CriarSubPasta(selectedNode);
            }
        }

        private void CriarPastaNoMesmoNivel(TreeNode selectedNode)
        {
            // Pedir o nome da nova pasta
            string novaPastaNome = glo.ShowDialog("Informe o nome da nova pasta:", "Nova Pasta");

            if (string.IsNullOrWhiteSpace(novaPastaNome))
            {
                MessageBox.Show("Nome da pasta não pode ser vazio.");
                return;
            }

            // Se o nó selecionado não tem pai, então está no nível raiz
            int? parentFolderId = selectedNode.Parent != null ? (int?)selectedNode.Parent.Tag : null;

            // Criar a nova pasta no banco de dados
            int novaPastaId = contasAPagarDao.CriarNovaPasta(novaPastaNome, parentFolderId);

            // Criar o nó na TreeView no mesmo nível
            TreeNode novaPastaNode = new TreeNode(novaPastaNome)
            {
                Tag = novaPastaId,
                ImageIndex = 0,  // Ícone de pasta
                SelectedImageIndex = 0
            };

            if (selectedNode.Parent != null)
            {
                selectedNode.Parent.Nodes.Add(novaPastaNode);
                selectedNode.Parent.Expand();
            }
            else
            {
                treeView1.Nodes.Add(novaPastaNode);
            }
        }

        private void CriarSubPasta(TreeNode selectedNode)
        {
            // Pedir o nome da nova subpasta
            string novaPastaNome = glo.ShowDialog("Informe o nome da nova subpasta:", "Nova Subpasta");

            if (string.IsNullOrWhiteSpace(novaPastaNome))
            {
                MessageBox.Show("Nome da pasta não pode ser vazio.");
                return;
            }

            // Criar a nova subpasta no banco de dados
            int parentFolderId = (int)selectedNode.Tag;
            int novaPastaId = contasAPagarDao.CriarNovaPasta(novaPastaNome, parentFolderId);

            // Criar o nó na TreeView como subpasta
            TreeNode novaPastaNode = new TreeNode(novaPastaNome)
            {
                Tag = novaPastaId,
                ImageIndex = 0,  // Ícone de pasta
                SelectedImageIndex = 0
            };

            selectedNode.Nodes.Add(novaPastaNode);
            selectedNode.Expand();
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
                this.iID = idArquivo;
                this.nmNo = selectedNode.Text;
                AbreArquivo();
                contasAPagarDao.Imprimiu(this.iID);
                selectedNode.BackColor = Verde;
                selectedNode.NodeFont = new Font(treeView1.Font, FontStyle.Regular);
            }           
        }

        private void btObter_Click_1(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                // Obtém o nó selecionado na TreeView
                TreeNode selectedNode = treeView1.SelectedNode;

                // Chama o método para processar a obtenção dos arquivos
                btObter_Click(selectedNode);
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;

            if (selectedNode != null)
            {
                // Verifica se o nó é uma pasta
                if (selectedNode.Nodes.Count > 0 || selectedNode.Tag is int == false)
                {
                    // Se o nó tem subnós ou a Tag não é um int, é uma pasta
                    // Você pode expandir a pasta, se necessário, ou simplesmente sair da função
                    selectedNode.Expand();
                    return;
                }

                // Se chegar aqui, significa que é um documento
                if (selectedNode.Tag is int documentID)
                {
                    this.iID = documentID;
                    this.nmNo = selectedNode.Text;
                    Mostra(); // Função que exibe os dados do documento
                }
                else
                {
                    // MessageBox.Show("Nenhum dado encontrado para o ID especificado.");
                }
                btMudar.Enabled = true;
            }
        }

        private void Mostra()
        {
            carregando = true;
            DataTable dados = contasAPagarDao.getPeloID(this.iID);

            if (dados != null && dados.Rows.Count > 0)
            {
                DataRow row = dados.Rows[0];

                this.UID = Convert.ToString(row["UID"]);
                dtpDataEmissao.Value = Convert.ToDateTime(row["DataEmissao"]);

                btPDF.Enabled = btEmail.Enabled = true;

                if (row["idFornecedor"] != DBNull.Value)
                {
                    cmbForn.SelectedValue = Convert.ToInt32(row["idFornecedor"]);
                }

                if (row["DataVencimento"] != DBNull.Value)
                {
                    dtpDataVencimento.Value = Convert.ToDateTime(row["DataVencimento"]);
                }

                txValorTotal.Text = Convert.ToString(row["ValorTotal"]);
                txChaveNotaFiscal.Text = Convert.ToString(row["ChaveNotaFiscal"]);
                txDescricao.Text = Convert.ToString(row["Descricao"]);
                ckPago.Checked = Convert.ToBoolean(row["Pago"]);

                if (ckPago.Checked)
                {
                    dtpDataPagamento.Enabled = true;
                    dtpDataPagamento.Value = Convert.ToDateTime(row["DataPagamento"]);
                }
                else
                {
                    dtpDataPagamento.Enabled = false;
                }

                this.CaminhoPDF = Convert.ToString(row["CaminhoPDF"]);
                txObservacoes.Text = Convert.ToString(row["Observacoes"]);
                btExcluir.Enabled = true;
            }
            else
            {
                // Tratar o caso em que não há dados retornados
                // MessageBox.Show("Nenhum dado encontrado para o ID especificado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Limpar os campos ou realizar outras ações necessárias
            }

            carregando = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedNodeToMove == null)
            {
                MessageBox.Show("Nenhum nó foi selecionado para mover.");
                return;
            }

            // Verifica se uma pasta foi selecionada no TreeView2 (destino)
            TreeNode targetFolderNode = treeView2.SelectedNode;

            if (targetFolderNode == null)
            {
                MessageBox.Show("Nenhum nó foi selecionado na TreeView2.");
                return;
            }

            // Obter o ID do documento a ser movido
            int documentID = (int)selectedNodeToMove.Tag;

            // Obter o ID da nova pasta (pasta de destino)
            int newFolderID = (int)targetFolderNode.Tag;

            // Realizar o UPDATE na base de dados
            contasAPagarDao.MudaNo(newFolderID, documentID);

            // Remover o nó da localização atual
            selectedNodeToMove.Remove();

            // Recarregar a pasta de destino para garantir que o nó seja exibido
            ReloadFolder(targetFolderNode);

            // Expandir a pasta de destino para mostrar o nó movido
            targetFolderNode.Expand();

            // Fechar o painel de movimentação
            panel2.Visible = false;

            // Limpar a variável para a próxima operação
            selectedNodeToMove = null;
        }

        private void ReloadFolder(TreeNode folderNode)
        {
            // Limpa os nós existentes
            folderNode.Nodes.Clear();

            // Recarrega os documentos e subpastas da pasta
            List<tb.Document> documents = FDao.GetDocuments((int)folderNode.Tag);
            AddDocumentNodes(folderNode, documents);
            // Se houver subpastas, recarregue-as também
            List<tb.Folder> subFolders = FDao.GetSubFolders((int)folderNode.Tag);
            foreach (var subFolder in subFolders)
            {
                TreeNode subFolderNode = new TreeNode(subFolder.FolderName) { Tag = subFolder.FolderID };
                folderNode.Nodes.Add(subFolderNode);
                LoadSubFoldersAndDocuments(subFolderNode, subFolder.FolderID);
            }
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Obtém o nó que está sob o cursor do mouse
                TreeNode clickedNode = treeView1.GetNodeAt(e.X, e.Y);

                if (clickedNode != null)
                {
                    // Seleciona o nó clicado
                    treeView1.SelectedNode = clickedNode;

                    // Verifica se o nó tem uma extensão de 3 ou 4 caracteres
                    string nodeName = clickedNode.Text;
                    string extension = Path.GetExtension(nodeName).TrimStart('.');

                    if (string.IsNullOrEmpty(extension) || (extension.Length != 3 && extension.Length != 4))
                    {
                        // Exibe o menu de contexto se não for uma extensão de 3 ou 4 caracteres
                        contextMenuStrip1.Show(treeView1, e.Location);
                    }
                    else
                    {
                        // Não exibe o menu de contexto
                        contextMenuStrip1.Hide();
                    }
                }
                else
                {
                    // Oculta o menu de contexto se clicado fora de um nó
                    contextMenuStrip1.Hide();
                }
            }
        }

        private void renomearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Obter o nó selecionado
            TreeNode selectedNode = treeView1.SelectedNode;

            if (selectedNode == null || selectedNode.Tag == null)
            {
                MessageBox.Show("Nenhuma pasta foi selecionada para renomear.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verifica se o nó é uma pasta
            if (IsFolderNode(selectedNode))
            {
                // Mostrar o diálogo para o novo nome
                string currentName = selectedNode.Text;
                string newName = glo.ShowDialog("Renomear Pasta: Informe o novo nome para a pasta:", currentName);

                if (!string.IsNullOrWhiteSpace(newName) && newName != currentName)
                {
                    // Obter o ID da pasta a ser renomeada
                    int folderID = (int)selectedNode.Tag;

                    // Atualizar o nome no banco de dados
                    contasAPagarDao.RenomearPasta(folderID, newName);

                    // Atualizar o texto do nó na TreeView
                    selectedNode.Text = newName;
                }
                else if (newName == currentName)
                {
                    MessageBox.Show("O nome da pasta não foi alterado.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Nome inválido. Operação cancelada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma pasta para renomear.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void apagarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Definir o cursor para ampulheta
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // Obter o nó selecionado
                TreeNode selectedNode = treeView1.SelectedNode;

                if (selectedNode == null || selectedNode.Tag == null)
                {
                    MessageBox.Show("Nenhuma pasta foi selecionada para exclusão.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verifica se o nó é uma pasta
                if (IsFolderNode(selectedNode))
                {
                    int folderID = (int)selectedNode.Tag;

                    // Verificar se a pasta contém subpastas ou arquivos
                    var subFolders = FDao.GetSubFolders(folderID);
                    var documents = FDao.GetDocuments(folderID);

                    // Construir mensagem de confirmação
                    string message = $"Tem certeza que deseja excluir a pasta '{selectedNode.Text}'?";
                    if (subFolders.Count > 0 || documents.Count > 0)
                    {
                        message += "\nEsta pasta contém subpastas e/ou arquivos que também serão excluídos.";
                    }

                    DialogResult result = MessageBox.Show(message, "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Excluir todos os arquivos e registros da pasta
                        foreach (var doc in documents)
                        {
                            this.iID = doc.DocumentID;
                            this.nmNo = doc.DocumentName;

                            // Apagar registro no banco e arquivo fisicamente
                            ApagaRegistro();
                            ApagarArquivo(this.iID, this.nmNo);
                        }

                        // Excluir a pasta e todas as subpastas e seus conteúdos do banco de dados
                        contasAPagarDao.ExcluiPastaEConteudos(folderID);

                        // Remover o nó da TreeView
                        selectedNode.Remove();

                        MessageBox.Show("Pasta e seus conteúdos excluídos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, selecione uma pasta para excluir.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                // Restaurar o cursor para o padrão
                Cursor.Current = Cursors.Default;
            }
        }


    }

}

