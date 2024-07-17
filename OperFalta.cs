using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.gen;

namespace TeleBonifacio
{
    public partial class OperFalta : Form
    {
        private FaltasDAO faltasDAO;
        private TpoFaltaDAO TpoFalta;
        private FornecedorDao Forn;
        private EncomendasDao EncoDao;
        private ProdutosDao cDaoP;      
        private GarantiasDao cDaoG;
        private pesCliente FpesCliente;
        private DataTable dadosCli;
        private bool carregando = true;
        private bool Restrito = false;
        private Color originalBackgroundColor;
        private int BakidTipo = 0;
        private int BakidForn = 0;
        private int bakComprado = 0;
        private string Bakcodigo = "";
        private string Bakquantidade = "";
        private string Bakmarca = "";
        private string BakObs = "";
        private string BakDescr = "";
        private string iUser = "";
        private string BakCodigoLost = "";
        private int BakidVendedor = 0;
        private int bakEmFalta = 0;
        private int idCliente = 0;
        private bool AtualizarGridP = true;
        private bool AtualizarGridE = true;
        private bool AtualizarGridG = true;
        private int iID = 0;
        private string caminhoDoArquivo = "";
        private bool Instanciar = true;

        #region Inicializacao

        public OperFalta()
        {
            InitializeComponent();
            SetStartPosition();
        }

        private void OperFalta_Load(object sender, EventArgs e)
        {
            VendedoresDAO Vendedor = new VendedoresDAO();
            iUser = glo.iUsuario.ToString();
            CarregarComboBoxV<tb.Vendedor>(cmbVendedor, Vendedor, iUser);
            faltasDAO = new FaltasDAO();
            TpoFalta = new TpoFaltaDAO();
            Forn = new FornecedorDao();
            CarregaGrid();
            MostraTipos();
            if (iUser.Length == 0)
            {
                btAdicTpo.Visible = false;
                btComprei.Visible = false;
                btnExcluir.Visible = false;
                Restrito = true;
            }
            originalBackgroundColor = txtCodigo.BackColor;
            if (glo.ODBC)
            {
                btEncomenda.Enabled = false;
            }
            if (glo.Nivel==2)
            {
                lbVlor.Visible = true;
                txValor.Visible = true;                
            } else
            {
                tbFaltas.TabPages.Remove(tabPage2);
                tbFaltas.TabPages.Remove(tabPage3);
                tbFaltas.TabPages.Remove(tabPage4);
                tbFaltas.TabPages.Remove(tabPage5);
            }
            ConfigureDataGridView(this.dataGrid1);
            ConfigureDataGridView(this.dataGrid2);
            ConfigureDataGridView(this.dataGrid3);
            ConfigureDataGridView(this.dataGrid4);
        }

        private void ConfigureDataGridView(DataGridView grid)
        {
            grid.Font = new System.Drawing.Font("Segoe UI", 12);
            grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grid.ColumnHeadersHeight = 30; 
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void CarregarComboBoxV<T>(ComboBox comboBox, VendedoresDAO vendedor, string iUser)
        {
            glo.Loga("Dentro de CarregarComboBoxV");
            DataTable dados = vendedor.getBalconistas();
            List<tb.ComboBoxItem> lista = new List<tb.ComboBoxItem>();
            int NrLista = 0;
            int NrItem = 0;
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string Nro = row["Nro"].ToString();
                string nome = Nro + " - " + row["Nome"].ToString();
                tb.ComboBoxItem item = new tb.ComboBoxItem(id, nome);
                lista.Add(item);
                NrItem++;
                if (iUser == Nro)
                {
                    NrLista = NrItem;
                }
            }
            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Nome";
            comboBox.ValueMember = "Id";
            if (NrLista > 0)
            {
                comboBox.SelectedValue = NrLista;
            }
            else
            {
                comboBox.SelectedIndex = -1;
            }
        }

        private void OperFalta_Activated(object sender, EventArgs e)
        {
            if (carregando)
            {
                carregando = false;
            }
        }

        #endregion

        #region Adição
        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (btnAdicionar.Text != "Limpar")
            {
                string codigo = txtCodigo.Text;
                string ret = "";
                if (codigo.Length > 0)
                {
                    ret = faltasDAO.VeSeJaTem(codigo);
                }
                if (ret.Length > 0)
                {
                    MessageBox.Show(ret, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    int idBalconista = Convert.ToInt32(cmbVendedor.SelectedValue);
                    string quantidade = txQuantidade.Text;
                    string Marca = txMarca.Text;
                    string Descr = txDescr.Text;
                    string Obs = txObs.Text;
                    string UID = glo.GenerateUID();
                    int idForn = 0;
                    if (!Restrito)
                    {
                        if (cmbForn.FlatStyle == FlatStyle.Flat)
                        {
                            int iForn = cmbForn.SelectedIndex;
                            if (iForn > -1)
                            {
                                idForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
                            }
                        }
                    }
                    int idTipo = 0;
                    if (!Restrito)
                    {
                        if (cmbTipos.FlatStyle == FlatStyle.Flat)
                        {
                            int iTpo = cmbTipos.SelectedIndex;
                            if (iTpo > -1)
                            {
                                idTipo = ((tb.ComboBoxItem)cmbTipos.Items[iTpo]).Id;
                            }
                        }
                    }
                    glo.Loga($@"FA,{idBalconista}, {quantidade}, {codigo}, {Marca}, {Descr}, {Obs} , {idForn}, {idTipo}, {UID}");
                    faltasDAO.Adiciona(idBalconista, quantidade, codigo, Marca, Descr, Obs, idForn, idTipo, UID);
                    cmbVendedor.FlatStyle = FlatStyle.System;
                }
            }
            CarregaGrid();
            Limpar();
            cmbVendedor.Enabled = true;
        }

        private void Limpar()
        {            
            cmbTipos.FlatStyle = FlatStyle.System;
            cmbForn.FlatStyle = FlatStyle.System;
            cmbTipos.Tag = "";
            cmbForn.Tag = "";
            cmbTipos.SelectedIndex = 0;
            cmbForn.SelectedIndex = 0;            
            btnAdicionar.Text = "Adicionar";
            btnExcluir.Enabled = false;
            btAdicTpo.Enabled = false;
            BakCodigoLost = "";
            BakidTipo = 0;
            BakidForn = 0;
            bakComprado = 0;
            Bakcodigo = "";
            Bakquantidade = "";
            Bakmarca = "";
            BakObs = "";
            Normaliza(txtCodigo, 1);
            Normaliza(txMarca, 1);
            Normaliza(txDescr, 1);
            Normaliza(txObs, 1);
            Normaliza(txQuantidade, 1);
            Normaliza(txValor, 1);
            txtCodigo.Focus();
        }

        private void NormalizaCampos()
        {
            Normaliza(txtCodigo, 0);
            Normaliza(txMarca, 0);
            Normaliza(txDescr, 0);
            Normaliza(txObs, 0);
            Normaliza(txQuantidade, 0);
            Normaliza(txValor, 0);
            cmbTipos.FlatStyle = FlatStyle.System;
            cmbForn.FlatStyle = FlatStyle.System;
            cmbTipos.Tag = "";
            cmbForn.Tag = "";

        }

        private void Normaliza(TextBox obj, int tipo)
        {
            if (tipo == 1)
            {
                obj.Text = "";
            }
            obj.BackColor = originalBackgroundColor;
            obj.ReadOnly = false;
            obj.Tag = "";
        }

        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txtCodigo);
            if (tbFaltas.SelectedIndex == 0)
            {
                btnAdicionar.Enabled = true;
            }
        }

        private void VeSeHab(TextBox obj = null)
        {
            if (obj != null)
            {
                if (!Restrito)
                {
                    if (!obj.ReadOnly)
                    {
                        if (obj.Text.Length > 0)
                        {
                            if (tbFaltas.SelectedIndex == 0)
                            {
                                obj.BackColor = Color.Yellow;

                            }
                            button2.Enabled = btLmpFiltro.Enabled = true;
                            btAdicTpo.Enabled = true;
                            obj.Tag = "M";
                        } 
                    }
                }
            }
            bool ok = true;
            if (cmbVendedor.SelectedIndex == -1)
            {
                ok = false;
            }
            if (txMarca.Text.Length == 0)
            {
                ok = false;
            }
            if (txDescr.Text.Length == 0)
            {
                ok = false;
            }
            btnAdicionar.Enabled = ok;
        }

        private void txQuantidade_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txQuantidade);
        }

        private void txMarca_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txMarca);
        }

        private void txObs_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txObs);
        }

        private void cmbVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                VeSeHab();
                cmbVendedor.FlatStyle = FlatStyle.Flat;
                button2.Enabled = true;
            }
        }

        private void ReadlyOnly(bool v)
        {
            cmbVendedor.Enabled = !v;
            txQuantidade.ReadOnly = v;
            txMarca.ReadOnly = v;
            txtCodigo.ReadOnly = v;
        }

        private void ExcluirRegistrosDaGrid(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.SelectedRows)
            {
                int gID = Convert.ToInt32(row.Cells["ID"].Value);
                string UID = Convert.ToString(row.Cells["UID"].Value);
                glo.Loga($@"FD,{gID}, {UID}");
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        faltasDAO.Exclui(gID);
                        break;
                    case 1:
                        cDaoP.Exclui(gID);
                        break;
                    case 2:
                        EncoDao.Exclui(gID);
                        break;
                    case 3:
                        cDaoG.Exclui(gID);
                        break;
                    default:
                        return;
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        ExcluirRegistrosDaGrid(dataGrid1);
                        CarregaGrid();
                        break;
                    case 1:
                        ExcluirRegistrosDaGrid(dataGrid2);
                        CarregaGridP();
                        break;
                    case 2:
                        ExcluirRegistrosDaGrid(dataGrid3);
                        CarregaGridE();
                        break;
                    case 3:
                        ExcluirRegistrosDaGrid(dataGrid4);
                        CarregaGridG();
                        break;
                    default:
                        return;
                }
                Limpar();
            }
        }

        private void txDescr_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txDescr);
        }

        #endregion

        #region Faltas
        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Width = 100;       // Compra
            dataGrid1.Columns[1].Width = 130;       // Forn
            dataGrid1.Columns[2].Visible = false;    // ID false;
            dataGrid1.Columns[3].Visible = false;
            dataGrid1.Columns[4].Width = 75;       // Data
            dataGrid1.Columns[5].Width = 80;        // Código
            dataGrid1.Columns[6].Width = 50;        // Quantidade
            dataGrid1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGrid1.Columns[7].Width = 80;        // Marca
            dataGrid1.Columns[8].Width = 160;       // Descrição
            dataGrid1.Columns[9].Width = 130;       // Balconista
            dataGrid1.Columns[10].Visible = false;  // UID
            dataGrid1.Columns[11].Width = 130;      // Tipo - colocado o texto
            dataGrid1.Columns[12].Visible = false;  // Tipo valor original
            dataGrid1.Columns[13].Visible = false;  // idForn

            if (glo.Nivel == 2)
            {
                dataGrid1.Columns[14].Visible = true;   // Valor
                dataGrid1.Columns[14].Width = 50; 
                dataGrid1.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else
            {
                dataGrid1.Columns[14].Visible = false;  // Valor
            }            
            dataGrid1.Columns[15].Width = 100;      // Obs
            dataGrid1.Invalidate();
        }

        private void CarregaGrid()
        {
            if (!carregando)
            {

            }
            int scrollPosition = dataGrid1.FirstDisplayedScrollingRowIndex;            
            FaltasDAO faltasDAO = new FaltasDAO();
            DataTable dados = faltasDAO.getDados(BakidTipo, BakidForn, bakComprado, Bakcodigo, Bakquantidade, Bakmarca, BakObs, BakidVendedor, bakEmFalta, BakDescr);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid1.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid1.Rows)
            {
                if (!row.Cells["Tipo"].Value.Equals(DBNull.Value))
                {
                    int tipoId = Convert.ToInt32(row.Cells["Tipo"].Value);
                    if (tipoId == 8)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        if (tipoId == 26)
                        {
                            row.DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                }
                AtualizarLinha(row, tipos, "Tipo", "Tipo");
                AtualizarLinha(row, Fornecs, "idForn", "Forn");
            }
            if (dados != null)
            {
                ConfigurarGrid();
                if (scrollPosition>0)
                    dataGrid1.FirstDisplayedScrollingRowIndex = scrollPosition;
            }            
        }

        private void dataGrid1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                txQuantidade.Text = FiltraOZero(selectedRow.Cells["Quant"].Value);
                txtCodigo.Text = FiltraOZero(selectedRow.Cells["Codigo"].Value);
                txMarca.Text = FiltraOZero(selectedRow.Cells["Marca"].Value);
                txObs.Text = FiltraOZero(selectedRow.Cells["Obs"].Value);
                txDescr.Text = Convert.ToString(selectedRow.Cells["Descricao"].Value);
                txValor.Text = glo.fmtVlr(Convert.ToString(selectedRow.Cells["Valor"].Value));
                cmbVendedor.SelectedValue = Convert.ToInt32(selectedRow.Cells["IDBalconista"].Value);
                try
                {
                    cmbForn.SelectedValue = Convert.ToInt32(selectedRow.Cells["idForn"].Value);
                }
                catch (Exception)
                {
                    cmbForn.SelectedValue = -1;
                }
                try
                {
                    cmbTipos.SelectedValue = Convert.ToInt32(selectedRow.Cells["TipoOrig"].Value);
                }
                catch (Exception)
                {
                    cmbTipos.SelectedValue = -1;
                }
                ReadlyOnly(true);
                btnAdicionar.Text = "Limpar";
                btnAdicionar.Enabled = true;
                btnExcluir.Enabled = true;
                btComprei.Enabled = true;
                txQuantidade.ReadOnly = false;
                txMarca.ReadOnly = false;
                if (dataGrid1.SelectedRows.Count == 1)
                {
                    txtCodigo.ReadOnly = false;
                }
                else
                {
                    txtCodigo.ReadOnly = true;
                }
                txQuantidade.BackColor = originalBackgroundColor;
                txMarca.BackColor = originalBackgroundColor;
                txtCodigo.BackColor = originalBackgroundColor;
                txDescr.BackColor = originalBackgroundColor;
                txObs.BackColor = originalBackgroundColor;
                carregando = false;
            }
        }

        private void AtualizarLinha<T>(DataGridViewRow row, List<T> items, string idColumnName, string displayColumnName)
            where T : tb.IDataEntity
        {
            if (!row.Cells[idColumnName].Value.Equals(DBNull.Value))
            {
                if (row.Cells[idColumnName].Value.ToString().Equals("0"))
                {
                    row.Cells[displayColumnName].Value = "";
                }
                else
                {
                    object oItem = row.Cells[idColumnName].Value;
                    int itemId = 0;
                    if (oItem != null)
                    {
                        if (oItem is int directInt)
                        {
                            itemId = Convert.ToInt32(oItem.ToString());
                        }
                        else
                        {
                            if (oItem is string itemAsString && int.TryParse(itemAsString, out int convertedInt))
                                itemId = convertedInt;
                        }
                    }
                    var itemEncontrado = items.Find(i => i.Id == itemId);
                    if (itemEncontrado != null)
                    {
                        row.Cells[displayColumnName].Value = itemEncontrado.Nome;
                    }
                    else
                    {
                        row.Cells[displayColumnName].Value = "";
                    }
                }
            }
        }
        
        private void ckEmFalta_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        #endregion

        #region Tipos

        private void MostraTipos()
        {
            if (glo.ODBC)
            {
                glo.CarregarComboBox<tb.TpoFalta>(cmbTipos, TpoFalta, "ESCOLHA");
                glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA");
            }
            else
            {
                glo.CarregarComboBox<tb.TpoFalta>(cmbTipos, TpoFalta, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
                glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
            }
        }

        private void btAdicTpo_Click(object sender, EventArgs e)
        {
            if (btAdicTpo.Text == "Adicionar")
            {
                if ((txNvTipo.Text.Length == 0) && (txNvForn.Text.Length == 0))
                {
                    MessageBox.Show("Só é possível adicionar se dizer qual ele é",
                                                  "Faltou dizer",
                                                  MessageBoxButtons.OK);
                    txNvTipo.Focus();
                }
                else
                {
                    if (txNvTipo.Visible)
                    {
                        TpoFalta.Adiciona(txNvTipo.Text);
                    }
                    else
                    {
                        Forn.Adiciona(txNvForn.Text);
                    }
                    RetCmboTpo();
                }
            }
            else
            {
                // ATUALIZAÇÃO
                int idForn = 0;
                if (cmbForn.Tag == "M")
                {
                    int iForn = cmbForn.SelectedIndex;
                    if (iForn > -1)
                    {
                        idForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
                    }
                }
                if (tbFaltas.SelectedIndex == 3)
                {
                    string sID = dataGrid4.SelectedRows[0].Cells[0].Value.ToString();
                    string UID = dataGrid4.SelectedRows[0].Cells[5].Value.ToString();
                    int gID = Convert.ToInt32(sID);
                    glo.Loga($@"FG,{gID}, {idForn}, {UID}");
                    cDaoG.MudaForn(gID, idForn);
                    CarregaGridG();
                    Limpar();
                }
                else
                {
                    string codigo = "";
                    if (txtCodigo.Tag == "M")
                    {
                        codigo = txtCodigo.Text;
                    }
                    int idTipo = 0;
                    if (cmbTipos.Tag == "M")
                    {
                        int iTpo = cmbTipos.SelectedIndex;
                        if (iTpo > -1)
                        {
                            idTipo = ((tb.ComboBoxItem)cmbTipos.Items[iTpo]).Id;
                        }
                    }
                    string quantidade = "";
                    if (txQuantidade.Tag == "M")
                    {
                        quantidade = txQuantidade.Text;
                    }
                    string marca = "";
                    if (txMarca.Tag == "M")
                    {
                        marca = txMarca.Text;
                    }
                    string Obs = "";
                    if (txObs.Tag == "M")
                    {
                        Obs = txObs.Text;
                    }
                    string Descr = "";
                    if (txDescr.Tag == "M")
                    {
                        Descr = txDescr.Text;
                    }
                    float Vlr = 0;
                    if (txValor.Tag == "M")
                    {
                        Vlr = glo.LeValor(txValor.Text);
                    }
                    switch (tbFaltas.SelectedIndex)
                    {
                        case 0:
                            AtualizaItensSelecionados(0, dataGrid1, idTipo, idForn, codigo, quantidade, marca, Obs, Descr, Vlr);
                            break;
                        case 1:
                            AtualizaItensSelecionados(1, dataGrid2, idTipo, idForn, codigo, quantidade, marca, Obs, Descr, Vlr);
                            break;
                        case 2:
                            AtualizaItensSelecionados(2, dataGrid3, idTipo, idForn, codigo, quantidade, marca, Obs, Descr, Vlr);
                            break;
                    }
                }

            }
        }

        private void AtualizaItensSelecionados(int nrGrid, DataGridView grid, int idTipo, int idForn, string codigo, string quantidade, string marca, string obs, string descr, float Vlr)
        {
            HashSet<string> selectedCodes = new HashSet<string>();
            int scrollPosition = grid.FirstDisplayedScrollingRowIndex;
            if (grid.SelectedRows.Count > 1)
            {
                codigo = "";
            }
            foreach (DataGridViewRow row in grid.SelectedRows)
            {
                string sID = Convert.ToString(row.Cells["ID"].Value);
                selectedCodes.Add(sID);
                int gID = Convert.ToInt32(row.Cells["ID"].Value);
                string UID = Convert.ToString(row.Cells["UID"].Value);
                glo.Loga($@"FA,{gID}, {idTipo}, {idForn}, {codigo}, {quantidade}, {marca}, {obs}, {descr}, {Vlr}, {UID}");
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        faltasDAO.Atualiza(gID, idTipo, idForn, codigo, quantidade, marca, obs, descr, Vlr);
                        break;
                    case 1:
                        cDaoP.Atualiza(gID, idTipo, idForn, codigo, quantidade, marca, obs, descr, Vlr);
                        break;
                    case 2:
                        EncoDao.Atualiza(gID, this.idCliente, idForn, codigo, quantidade, marca, obs, descr);
                        break;
                }
            }
            switch (tbFaltas.SelectedIndex)
            {
                case 0:
                    CarregaGrid();
                    break;
                case 1:
                    CarregaGridP();
                    break;
                case 2:
                    CarregaGridE();
                    break;
            }
            if (grid.Rows.Count > 0)
            {
                grid.FirstDisplayedScrollingRowIndex = scrollPosition;
                foreach (string code in selectedCodes)
                {
                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        string cod = row.Cells["ID"].Value.ToString();
                        if (cod == code)
                        {
                            row.Selected = true;
                            break;
                        }
                    }
                }
            }
            if (grid.SelectedRows.Count > 1)
            {                
                NormalizaCampos();
            }
        }

        private void RetCmboTpo()
        {
            txNvTipo.Text = "";
            txNvForn.Text = "";
            txNvTipo.Visible = false;
            txNvForn.Visible = false;
            cmbTipos.Visible = true;
            cmbForn.Visible = true;
            cmbTipos.DataSource = null;
            cmbForn.DataSource = null;
            MostraTipos();
            btAdicTpo.Text = "Atualizar";
            btAdicTpo.Enabled = false;
            btComprei.Text = "Comprei";
            btComprei.Enabled = false;
        }

        private void AtualizouEmBaixo()
        {
            cmbTipos.SelectedIndex = 0;
            cmbTipos.FlatStyle = FlatStyle.System;
            cmbForn.SelectedIndex = 0;
            cmbForn.FlatStyle = FlatStyle.System;
            btAdicTpo.Enabled = false;
            btComprei.Enabled = false;
            Normaliza(txtCodigo, 0);
            Normaliza(txMarca, 0);
            Normaliza(txDescr, 0);
            Normaliza(txObs, 0);
            Normaliza(txQuantidade, 0);
        }

        private void cmbTipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                if (cmbTipos.SelectedItem != null)
                {
                    string ItemCombo = cmbTipos.SelectedItem.ToString();
                    if (ItemCombo != "ESCOLHA")
                    {
                        if (ItemCombo == "ADICIONE")
                        {
                            btAdicTpo.Text = "Adicionar";
                            btAdicTpo.Enabled = true;
                            txNvTipo.Visible = true;
                            cmbTipos.Visible = false;
                            btComprei.Text = "Cancelar";
                            btComprei.Enabled = true;
                            txNvTipo.Focus();
                        }
                        else
                        {
                            if (ItemCombo == "EDIÇÃO")
                            {
                                fCadTiposFaltas novoForm = new fCadTiposFaltas();
                                novoForm.ShowDialog();
                                MostraTipos();
                            }
                            else
                            {
                                btAdicTpo.Enabled = true;
                                cmbTipos.FlatStyle = FlatStyle.Flat;
                                cmbTipos.Tag = "M";
                                button2.Enabled = true;
                                btLmpFiltro.Enabled = true;
                                btAdicTpo.Enabled = true;
                            }
                        }
                    }
                }
            }
        }

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
                            btAdicTpo.Text = "Adicionar";
                            btAdicTpo.Enabled = true;
                            txNvForn.Visible = true;
                            cmbForn.Visible = false;
                            btComprei.Text = "Cancelar";
                            btComprei.Enabled = true;
                            txNvForn.Focus();
                        }
                        else
                        {
                            if (ItemCombo == "EDIÇÃO")
                            {
                                CadFornec novoForm = new CadFornec();
                                novoForm.ShowDialog();
                                MostraTipos();
                            }
                            else
                            {
                                btAdicTpo.Enabled = true;
                                cmbForn.FlatStyle = FlatStyle.Flat;
                                cmbForn.Tag = "M";
                                button2.Enabled = true;
                                btLmpFiltro.Enabled = true;
                                btAdicTpo.Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void txForn_KeyUp(object sender, KeyEventArgs e)
        {
            btAdicTpo.Enabled = true;
        }

        private void btComprei_Click(object sender, EventArgs e)
        {
            if (btComprei.Text == "Comprei")
            {
                //  COMPROU A MERCADORIA QUE ESTAVA EM FALTA
                int scrollPosition = dataGrid1.FirstDisplayedScrollingRowIndex;
                if (dataGrid1.SelectedRows.Count==1)
                {
                    string sID = dataGrid1.SelectedRows[0].Cells[2].Value.ToString();
                    int gID = Convert.ToInt32(sID);
                    string sVlr = dataGrid1.SelectedRows[0].Cells[14].Value.ToString();
                    if (sVlr.Length==0)
                    {
                        sVlr = "0";
                    }
                    string inputValue = Microsoft.VisualBasic.Interaction.InputBox("Valor de compra", "Input Value", sVlr, -1, -1);
                    if (inputValue.Length>0)
                    {
                        float valor = glo.LeValor(inputValue);
                        faltasDAO.Comprou(gID, valor);
                    }
                } else
                {
                    foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                    {
                        int gID = Convert.ToInt32(row.Cells["ID"].Value);
                        faltasDAO.Comprou(gID, 0);
                    }
                }
                AtualizarGridP = true;
                CarregaGrid();
                if (scrollPosition>0)  
                    dataGrid1.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
            else
            {
                if (btComprei.Text == "Em Falta")
                {
                    // RETORNOU PARA EM FALTA A MERCADORIA QUE ESTAVA COMO COMPRADA
                    DialogResult result = MessageBox.Show("Tem certeza que deseja retornar para faltas?",
                                                          "Esta em falta",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in dataGrid2.SelectedRows)
                        {
                            int gID = Convert.ToInt32(row.Cells["ID"].Value);
                            cDaoP.EmFalta(gID);
                        }
                        CarregaGridP();
                        CarregaGrid();
                        AtualizouEmBaixo();
                    }
                }
                else
                {
                    RetCmboTpo();
                    Limpar();
                }
            }
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (tbFaltas.SelectedIndex == 0)
            {
                string codigo = txtCodigo.Text;
                if (BakCodigoLost != codigo)
                {
                    BakCodigoLost = codigo;
                    string ret = "";
                    if (codigo.Length > 0)
                    {
                        ret = faltasDAO.VeSeJaTem(codigo);
                        if (ret.Length > 0)
                        {
                            if (ret.Contains("comprado"))
                            {
                                txtCodigo.BackColor = Color.Red;
                            }
                            else
                            {
                                txtCodigo.BackColor = Color.Orange;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Filtro
        private void cmbTiposFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                button2.Enabled = btLmpFiltro.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Tag = "F";
            glo.Loga("Filtro");
            int iForn = cmbForn.SelectedIndex;
            BakidForn = 0;
            if (iForn > 0)
            {
                BakidForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;

            }
            if (tbFaltas.SelectedIndex == 3)
            {
                CarregaGridG();
            }
            else
            {
                int idTipo = cmbTipos.SelectedIndex;
                BakidTipo = 0;
                if (idTipo > 0)
                {
                    BakidTipo = ((tb.ComboBoxItem)cmbTipos.Items[idTipo]).Id;
                }
                if (cmbVendedor.FlatStyle == FlatStyle.Flat)
                {
                    int idVendedor = cmbVendedor.SelectedIndex;
                    BakidVendedor = ((tb.ComboBoxItem)cmbVendedor.Items[idVendedor]).Id;
                }
                Bakcodigo = "";
                if (txtCodigo.Tag == "M")
                {
                    Bakcodigo = txtCodigo.Text;
                }
                Bakquantidade = "";
                if (txQuantidade.Tag == "M")
                {
                    Bakquantidade = txQuantidade.Text;
                }
                Bakmarca = "";
                if (txMarca.Tag == "M")
                {
                    Bakmarca = txMarca.Text;
                }
                BakObs = "";
                if (txObs.Tag == "M")
                {
                    BakObs = txObs.Text;
                }
                BakDescr = "";
                if (txDescr.Tag == "M")
                {
                    BakDescr = txDescr.Text;
                }
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        bakEmFalta = (ckEmFalta.Checked == true) ? 1 : 0;
                        CarregaGrid();
                        break;
                    case 1:
                        CarregaGridP();
                        break;
                    case 2:
                        CarregaGridE();
                        break;
                }
                btnAdicionar.Text = "Limpar";
                btAdicTpo.Enabled = false;
            }
        }

        private void ckComprado_Click(object sender, EventArgs e)
        {
            button2.Enabled = btLmpFiltro.Enabled = true;
        }

        private void btLmpFiltro_Click(object sender, EventArgs e)
        {
            button2.Tag = "";
            btLmpFiltro.Enabled = false; 
            Limpar();
            BakidTipo = 0;
            BakidForn = 0;
            bakComprado = 0;
            Bakcodigo = "";
            Bakquantidade = "";
            Bakmarca = "";
            BakObs = "";
            switch (tbFaltas.SelectedIndex)
            {
                case 0:
                    CarregaGrid();
                    break;
                case 1:
                    CarregaGridP();
                    break;
                case 2:
                    CarregaGridE();
                    break;
                case 3:
                    CarregaGridG();
                    break;
            }
            btLmpFiltro.Enabled = true;
        }

        #endregion

        #region Produtos

        private void ConfigurarGridP()
        {
            dataGrid2.Columns[0].Width = 100;       // Compra
            dataGrid2.Columns[1].Width = 130;       // Forn
            dataGrid2.Columns[2].Visible = false;   // ID
            dataGrid2.Columns[3].Width = 100;       // Data
            dataGrid2.Columns[4].Width = 80;        // Código
            if (glo.Nivel==2)
            {
                dataGrid2.Columns[5].Visible = true;
                dataGrid2.Columns[5].Width = 50;        // Valor
                dataGrid2.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            } else
            {
                dataGrid2.Columns[5].Visible = false;
            }
            dataGrid2.Columns[6].Width = 50;        // Quantidade
            dataGrid2.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGrid2.Columns[7].Width = 80;        // Marca
            dataGrid2.Columns[8].Width = 150;       // Descrição
            dataGrid2.Columns[9].Visible = false;  // UID
            dataGrid2.Columns[10].Width = 130;      // Tipo - colocado o texto
            dataGrid2.Columns[11].Visible = false;  // Tipo valor original
            dataGrid2.Columns[12].Visible = false;  // idForn
            dataGrid2.Columns[13].Width = 100;      // Obs
            dataGrid2.Invalidate();
        }

        private void CarregaGridP()
        {
            int scrollPosition = dataGrid2.FirstDisplayedScrollingRowIndex;
            cDaoP = new ProdutosDao();
            DataTable dados = cDaoP.getDados(BakidTipo, BakidForn, Bakcodigo, Bakquantidade, Bakmarca, BakObs, BakDescr);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid2.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid2.Rows)
            {
                AtualizarLinha(row, tipos, "Tipo", "Tipo");
                AtualizarLinha(row, Fornecs, "idForn", "Forn");
            }
            if (dados != null)
            {
                ConfigurarGridP();
                if (scrollPosition > 0)
                    dataGrid2.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
        }

        private void tbFaltas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                switch (tbFaltas.SelectedIndex)
                {
                    case 0: // Faltas
                        groupBox1.Enabled = true;
                        cmbTipos.Enabled = true;
                        btComprei.Visible = true;
                        btComprei.Text = "Comprei";
                        ckEmFalta.Visible = true;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        timer1.Enabled = false;
                        if (iUser.Length > 0)
                        {
                            cmbVendedor.SelectedItem = Convert.ToInt16(iUser);
                        }
                        else
                        {
                            cmbVendedor.Enabled = true;
                        }
                        break;
                    case 1: // Produtos
                        groupBox1.Enabled = true;
                        cmbTipos.Enabled = true;
                        btComprei.Visible = true;
                        btComprei.Text = "Em Falta";
                        ckEmFalta.Visible = false;
                        timer1.Enabled = false;
                        if (glo.Nivel==2)
                        {
                            lbVlor.Visible = true;
                            txValor.Visible = true;
                        }
                        if (AtualizarGridP)
                        {
                            carregando = true;
                            cmbVendedor.SelectedIndex = -1;
                            cmbVendedor.Enabled = false;
                            CarregaGridP();
                            AtualizarGridP = false;
                            carregando = false;
                        }
                        break;
                    case 2: //Encomendas
                        groupBox1.Enabled = true;
                        cmbTipos.Enabled = true;
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        timer1.Enabled = false;
                        if (AtualizarGridE)
                        {
                            carregando = true;
                            cmbVendedor.SelectedIndex = -1;
                            cmbVendedor.Enabled = false;
                            CarregaGridE();
                            AtualizarGridE = false;
                            carregando = false;
                        }
                        break;
                    case 3: // Garantias
                        groupBox1.Enabled = false;
                        cmbTipos.Enabled = false;
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        timer1.Enabled = false;
                        if (AtualizarGridG)
                        {
                            carregando = true;
                            cmbVendedor.SelectedIndex = -1;
                            cmbVendedor.Enabled = false;
                            CarregaGridG();
                            carregando = false;
                        }
                        break;
                    case 4: // Anotações
                        groupBox1.Enabled = false;
                        cmbTipos.Enabled = false;
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
                        lbVlor.Visible = false;
                        txValor.Visible = false;
                        if (rtfTexto.Text.Length == 0)
                        {
                            int padding = 5;
                            int toolbarHeight = toolStrip1.Height;
                            carregando = true;
                            rtfTexto.Location = new Point(0, toolbarHeight + padding);
                            rtfTexto.Size = new Size(tabPage5.Width, tabPage5.Height - toolbarHeight - padding);
                            caminhoDoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "anotacoes.rtf");
                            try
                            {
                                string Orig = File.ReadAllText(caminhoDoArquivo);
                                glo.Loga("Leu RTF criptografado, 10 primeiros caracteres" + Orig.Substring(0, 10));
                                string Conteudo = Cripto.Decrypt(Orig);
                                tsDescriptar.Visible = true;
                                tsEncriptar.Visible = false;
                                rtfTexto.Rtf = Conteudo;
                                glo.Loga("Mostrou RTF, 10 primeiros caracteres" + Conteudo.Substring(0, 10));
                            }
                            catch (Exception)
                            {
                                // 
                            }                            
                            carregando = false;
                        }
                        break;
                }
                Limpar();
            }
        }

        private void dataGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                txQuantidade.Text = FiltraOZero(selectedRow.Cells["Quant"].Value);
                txtCodigo.Text = FiltraOZero(selectedRow.Cells["Codigo"].Value);
                txMarca.Text = FiltraOZero(selectedRow.Cells["Marca"].Value);
                txObs.Text = FiltraOZero(selectedRow.Cells["Obs"].Value);
                txDescr.Text = Convert.ToString(selectedRow.Cells["Descricao"].Value);
                txValor.Text = glo.fmtVlr(Convert.ToString(selectedRow.Cells["Valor"].Value));
                try
                {
                    cmbForn.SelectedValue = Convert.ToInt32(selectedRow.Cells["idForn"].Value);
                }
                catch (Exception)
                {
                    cmbForn.SelectedValue = -1;
                }
                try
                {
                    cmbTipos.SelectedValue = Convert.ToInt32(selectedRow.Cells["TipoOrig"].Value);
                }
                catch (Exception)
                {
                    cmbTipos.SelectedValue = -1;
                }
                ReadlyOnly(true);
                btnExcluir.Enabled = true;
                btComprei.Enabled = true;
                txQuantidade.ReadOnly = false;
                txMarca.ReadOnly = false;
                if (dataGrid2.SelectedRows.Count == 1)
                {
                    txtCodigo.ReadOnly = false;
                }
                else
                {
                    txtCodigo.ReadOnly = true;
                }
                txQuantidade.BackColor = originalBackgroundColor;
                txMarca.BackColor = originalBackgroundColor;
                txtCodigo.BackColor = originalBackgroundColor;
                txDescr.BackColor = originalBackgroundColor;
                txObs.BackColor = originalBackgroundColor;
                carregando = false;
            }
        }

        private void txValor_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab(txValor);
        }

        #endregion

        #region Garantias

        private void btGarantia_Click(object sender, EventArgs e)
        {
            operGarantia FoperGarantia = new operGarantia();
            FoperGarantia.ShowDialog();
            if (tbFaltas.SelectedIndex == 3)
            {
                CarregaGridG();
            }
        }

        private void CarregaGridG()
        {
            int scrollPosition = dataGrid4.FirstDisplayedScrollingRowIndex;
            AtualizarGridG = false;
            cDaoG = new GarantiasDao();
            DataTable dados = cDaoG.getDados(BakidForn);
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid4.DataSource = dados;
            if (dados != null)
            {
                ConfigurarGridG();
                if (scrollPosition > 0)
                    dataGrid4.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
        }

        private void ConfigurarGridG()
        {
            dataGrid4.Columns[0].Visible = false;   // Id
            dataGrid4.Columns[1].Width = 80;        // Data
            dataGrid4.Columns[2].Visible = false;   // idForn

            // dataGrid4.Columns[3].Width = 50;        // Nota
            dataGrid4.Columns[3].Width = 70;        // Nota

            dataGrid4.Columns[4].Width = 80;        // Prometida
            dataGrid4.Columns[5].Width = 80;        // DataDoForn
            dataGrid4.Columns[6].Visible = false;   // UID
            try
            {
                dataGrid4.Columns[7].Width = 200;       // Fornecedor
            }
            catch (Exception)
            {
                // 
            }
            dataGrid4.Invalidate();
        }

        private void dataGrid4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                //this.UID = Convert.ToString(selectedRow.Cells["UID"].Value);
                try
                {
                    cmbForn.SelectedValue = Convert.ToInt32(selectedRow.Cells["idForn"].Value);
                }
                catch (Exception)
                {
                    cmbForn.SelectedValue = -1;
                }
                btnExcluir.Enabled = true;
                carregando = false;
            }
        }

        #endregion

        #region Encomendas

        private void btEncomenda_Click(object sender, EventArgs e)
        {
            glo.IdAdicionado = 0;
            string Descricao = "";
            string codigo = "";
            bool ProdNovo = false;
            switch (tbFaltas.SelectedIndex)
            {
                case 0: // Faltas                    
                    DefineCampos(dataGrid1, ref Descricao, ref ProdNovo, ref codigo);
                    break;
                case 1: // Produtos
                    DefineCampos(dataGrid2, ref Descricao, ref ProdNovo, ref codigo);
                    break;
                default:
                    ProdNovo = true;
                    break;
            }
            btEncomenda.Enabled = false;
            if (FpesCliente == null)
            {
                AcionaPesq(true, Descricao, ProdNovo, codigo);
            }
            else
            {
                try
                {
                    AcionaPesq(false, Descricao, ProdNovo, codigo);
                }
                catch (Exception)
                {
                    AcionaPesq(true, Descricao, ProdNovo, codigo);
                }
            }
            if (!glo.ODBC)
            {
                btEncomenda.Enabled = true;
            }
            if (FpesCliente.OK)
            {
                if (tbFaltas.SelectedIndex==2)
                {
                    CarregaGridE();
                } else
                {
                    AtualizarGridE = true;
                    if (ProdNovo == false)
                    {
                        if (tbFaltas.SelectedIndex == 0)
                        {
                            CarregaGrid();
                            AtualizouEmBaixo();
                        }
                    }
                }
            }
        }

        private void dataGrid3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView grid = sender as DataGridView;
                DataGridViewRow row = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(row.Cells["ID"].Value);
                string nome = row.Cells["Nome"].Value?.ToString() ?? "";
                string telefone = row.Cells["Telefone"].Value?.ToString() ?? "";
                DateTime data = Convert.ToDateTime(row.Cells["Data"].Value);
                DateTime dataPrometida = Convert.ToDateTime(row.Cells["DtPrometida"].Value);
                string codigo = row.Cells["Codigo"].Value?.ToString() ?? "";
                decimal valor = Convert.ToDecimal(row.Cells["Valor"].Value);
                string descricao = row.Cells["Descricao"].Value?.ToString() ?? "";
                PrepareAndShowFpesCliente(Instanciar, nome, telefone, data, dataPrometida, codigo, valor, descricao);
                if (FpesCliente.getAlterado())
                {
                    CarregaGridE();
                    Limpar();                                       
                }
            }
        }

        private void PrepareAndShowFpesCliente(bool instanciar, string nome, string telefone, DateTime data, DateTime dataPrometida, string codigo, decimal valor, string descricao)
        {
            bool sair = false;
            while (sair==false)
            {
                glo.Loga("Preparando FpesCliente");
                if (FpesCliente == null)
                {
                    glo.Loga("FpesCliente = null");
                    FpesCliente = new pesCliente();
                    if (dadosCli == null)
                    {
                        glo.Loga("dadosCli = null");
                        ClienteDAO Cliente = new ClienteDAO();
                        dadosCli = Cliente.GetDadosOrdenados();
                        FpesCliente.RecebeDadosCli(ref dadosCli, ref Forn, ref EncoDao, tbFaltas.SelectedIndex);
                    }
                }
                FpesCliente.setOperacao(2);
                FpesCliente.setId(this.iID);
                FpesCliente.CarregarDados(nome, telefone, data, dataPrometida, codigo, valor, descricao);
                if (instanciar)
                {
                    FpesCliente.Ativar();
                    FpesCliente.ShowDialog();
                    sair = true;
                }
                else
                {
                    try
                    {
                        FpesCliente.Visible = true;                        
                    }
                    catch (Exception)
                    {
                        FpesCliente = null;
                        instanciar = true;
                    }                    
                }
            }
        }

        private void DefineCampos(DataGridView Grid, ref string descricao, ref bool ProdNovo, ref string codigo)
        {
            if (Grid.SelectedRows.Count == 1)
            {
                if (Grid.SelectedRows.Count == 1)
                {
                    if (Grid.Rows.Count == 1 || Grid.SelectedRows[0].Index != 0)
                    {
                        descricao = Grid.SelectedRows[0].Cells["Descricao"].Value.ToString();
                        codigo = FiltraOZero(Grid.SelectedRows[0].Cells["Codigo"].Value.ToString());
                    }
                    else
                    {
                        ProdNovo = true;
                    }
                }
            }

        }

        private void AcionaPesq(bool Instanciar, string Descricao, bool ProdNovo, string codigo)
        {
            if (Instanciar)
            {
                FpesCliente = new pesCliente();
            }
            FpesCliente.SetDescricao(Descricao, ProdNovo, codigo);
            if (dadosCli == null)
            {
                ClienteDAO Cliente = new ClienteDAO();
                dadosCli = Cliente.GetDadosOrdenados();                
            }
            FpesCliente.setOperacao(1);
            if (Instanciar)
            {
                if (EncoDao == null)
                    EncoDao = new EncomendasDao();
                FpesCliente.RecebeDadosCli(ref dadosCli, ref Forn, ref EncoDao, tbFaltas.SelectedIndex);
                FpesCliente.Ativar();                
                FpesCliente.ShowDialog();
            }
            else
            {
                FpesCliente.ShowDialog();
            }
        }

        private void ConfigurarGridE()
        {
            dataGrid3.Columns[0].Visible = false;   // ID
            dataGrid3.Columns[1].Width = 130;       // Cliente
            dataGrid3.Columns[2].Width = 100;       // Data
            dataGrid3.Columns[3].Width = 100;       // Código
            dataGrid3.Columns[4].Width = 50;        // Valor
            dataGrid3.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGrid3.Columns[5].Width = 40;        // Quant
            dataGrid3.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGrid3.Columns[6].Width = 50;        // Marca
            dataGrid3.Columns[7].Width = 190;       // Descrição
            dataGrid3.Columns[8].Visible = false;   // UID
            dataGrid3.Columns[9].Width = 100;       // Tipo
            dataGrid3.Columns[10].Width = 130;      // Compra
            dataGrid3.Columns[11].Width = 100;      // Forn
            dataGrid3.Columns[12].Visible = false;  // IdForn
            dataGrid3.Columns[13].Width = 100;      // Obs
            dataGrid3.Columns[14].Visible = false;  // idCliente 
            dataGrid3.Columns[15].Visible = false;  // Telefone
            dataGrid3.Columns[16].Visible = false;  // DtPrometida
            dataGrid3.Columns[17].Visible = false;  // idCliente
            dataGrid3.Invalidate();
        }

        private void CarregaGridE()
        {
            int scrollPosition = dataGrid3.FirstDisplayedScrollingRowIndex;            
            if (EncoDao==null)
                EncoDao = new EncomendasDao();
            DataTable dados = EncoDao.getDados(BakidTipo, BakidForn, Bakcodigo, Bakquantidade, Bakmarca, BakObs, BakDescr);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid3.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid3.Rows)
            {
                AtualizarLinha(row, tipos, "Tipo", "Tipo");
                AtualizarLinha(row, Fornecs, "idForn", "Forn");
            }
            if (dados != null)
            {
                ConfigurarGridE();
            if (scrollPosition > 0)
                dataGrid3.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
            scrollPosition = dataGrid3.FirstDisplayedScrollingRowIndex;
        }

        private void dataGrid3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                txQuantidade.Text = FiltraOZero(selectedRow.Cells["Quant"].Value);
                txtCodigo.Text = FiltraOZero(selectedRow.Cells["Codigo"].Value);
                txMarca.Text = FiltraOZero(selectedRow.Cells["Marca"].Value);
                txObs.Text = FiltraOZero(selectedRow.Cells["Obs"].Value);
                txDescr.Text = Convert.ToString(selectedRow.Cells["Descricao"].Value);
                this.idCliente = Convert.ToInt16(selectedRow.Cells["idCliente"].Value);
                try
                {
                    cmbForn.SelectedValue = Convert.ToInt16(selectedRow.Cells["idForn"].Value);
                }
                catch (Exception)
                {
                    cmbForn.SelectedValue = -1;
                }
                ReadlyOnly(true);
                btnAdicionar.Text = "Limpar";
                btnAdicionar.Enabled = true;
                btnExcluir.Enabled = true;
                btComprei.Enabled = true;
                txQuantidade.ReadOnly = false;
                txMarca.ReadOnly = false;
                if (dataGrid3.SelectedRows.Count == 1)
                {
                    txtCodigo.ReadOnly = false;
                }
                else
                {
                    txtCodigo.ReadOnly = true;
                }
                txQuantidade.BackColor = originalBackgroundColor;
                txMarca.BackColor = originalBackgroundColor;
                txtCodigo.BackColor = originalBackgroundColor;
                txDescr.BackColor = originalBackgroundColor;
                txObs.BackColor = originalBackgroundColor;
                carregando = false;
            }
        }

        #endregion        
        private string FiltraOZero(object Value)
        {
            string texto = Convert.ToString(Value);
            if (texto == "0")
            {
                return "";
            }
            else
            {
                return texto;
            }
        }

        #region Anotações

        private void rtfTexto_TextChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                if (!timer1.Enabled)
                {
                    timer1.Enabled = true;
                }                
            }            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            SalvaRTF();
            if (timer1.Interval>500)
            {
                timer1.Interval -= 100;
            }
        }

        private void SalvaRTF()
        {
            string Orig = rtfTexto.Rtf;
            glo.Loga("Gravar RTF, 10 primeiros caracteres" + Orig.Substring(0, 10));
            string Conteudo = Cripto.Encrypt(Orig);
            File.WriteAllText(caminhoDoArquivo, Conteudo);
            glo.Loga("Gravou RTF, 10 primeiros caracteres" + Conteudo.Substring(0, 10));
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            if (rtfTexto.CanRedo)
            {
                rtfTexto.Redo();
            }
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            if (rtfTexto.CanUndo)
            {
                rtfTexto.Undo();
            }
        }

        private void toolStripButtonDecreaseFont_Click(object sender, EventArgs e)
        {
            float newSize = this.rtfTexto.SelectionFont.Size - 1;
            this.rtfTexto.SelectionFont = new Font(this.rtfTexto.SelectionFont.FontFamily, Math.Max(newSize, 1), this.rtfTexto.SelectionFont.Style); // Garante que o tamanho mínimo seja 1
        }

        private void toolStripButtonIncreaseFont_Click(object sender, EventArgs e)
        {
            float newSize = this.rtfTexto.SelectionFont.Size + 1;
            this.rtfTexto.SelectionFont = new Font(this.rtfTexto.SelectionFont.FontFamily, newSize, this.rtfTexto.SelectionFont.Style);
        }

        private void toolStripButtonUnderline_Click(object sender, EventArgs e)
        {
            if (rtfTexto.SelectionFont != null)
            {
                FontStyle currentStyle = rtfTexto.SelectionFont.Style;
                FontStyle newStyle;
                if (rtfTexto.SelectionFont.Underline)
                {
                    newStyle = currentStyle & ~FontStyle.Underline;
                }
                else
                {
                    newStyle = currentStyle | FontStyle.Underline;
                }
                rtfTexto.SelectionFont = new Font(rtfTexto.SelectionFont.FontFamily, rtfTexto.SelectionFont.Size, newStyle);
            }
        }

        private void toolStripButtonItalic_Click(object sender, EventArgs e)
        {
            if (rtfTexto.SelectionFont != null)
            {
                FontStyle currentStyle = rtfTexto.SelectionFont.Style;
                FontStyle newStyle;
                if (rtfTexto.SelectionFont.Italic)
                {
                    newStyle = currentStyle & ~FontStyle.Italic;
                }
                else
                {
                    newStyle = currentStyle | FontStyle.Italic;
                }
                rtfTexto.SelectionFont = new Font(rtfTexto.SelectionFont.FontFamily, rtfTexto.SelectionFont.Size, newStyle);
            }
        }

        private void toolStripButtonBold_Click(object sender, EventArgs e)
        {
            if (rtfTexto.SelectionFont != null)
            {
                FontStyle currentStyle = rtfTexto.SelectionFont.Style;
                FontStyle newStyle;
                if (rtfTexto.SelectionFont.Bold)
                {
                    newStyle = currentStyle & ~FontStyle.Bold;
                }
                else
                {
                    newStyle = currentStyle | FontStyle.Bold;
                }
                rtfTexto.SelectionFont = new Font(rtfTexto.SelectionFont.FontFamily, rtfTexto.SelectionFont.Size, newStyle);
            }
        }

        private void tsVermelho_Click(object sender, EventArgs e)
        {
            rtfTexto.SelectionColor = Color.Red;
        }

        private void tsAzul_Click(object sender, EventArgs e)
        {
            rtfTexto.SelectionColor = Color.Blue;
        }

        private void tsVerde_Click(object sender, EventArgs e)
        {
            rtfTexto.SelectionColor = Color.Green;
        }

        private void tsLaranja_Click(object sender, EventArgs e)
        {
            rtfTexto.SelectionColor = Color.Orange;
        }

        private void tsPreto_Click(object sender, EventArgs e)
        {
            rtfTexto.SelectionColor = Color.Black;
        }

        private void tsCinza_Click(object sender, EventArgs e)
        {
            rtfTexto.SelectionColor = Color.Gray;
        }

        private void toolStripButtonEncrypt_Click(object sender, EventArgs e)
        {
            //tsDescriptar.Visible = true;
            //tsEncriptar.Visible = false;
            //Criptografia = true;
            //SalvaRTF();
        }

        private void tsDescriptar_Click(object sender, EventArgs e)
        {
            //tsDescriptar.Visible = false;
            //tsEncriptar.Visible = true;
            //Criptografia = false;
            //SalvaRTF();
        }

        #endregion

    }
}