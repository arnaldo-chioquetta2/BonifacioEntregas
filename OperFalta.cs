using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.gen;
using static System.Windows.Forms.DataGrid;

namespace TeleBonifacio
{
    public partial class OperFalta : Form
    {
        private FaltasDAO faltasDAO;
        private TpoFaltaDAO TpoFalta;
        private FornecedorDao Forn;
        private EncomendasDao EncoDao;
        private ProdutosDao cDaoP;
        private bool carregando = true;
        private string UID = "";
        private int iID = 0;
        private bool Restrito = false;
        private Color originalBackgroundColor;

        private int BakidTipo = 0;
        private int BakidForn = 0;
        private int bakComprado =  0;
        private string Bakcodigo = "";
        private int Bakquantidade = -1;
        private string Bakmarca = "";
        private string BakObs = "";
        private string iUser = "";
        private bool AtualizarGridP = true;
        private bool AtualizarGridE = true;
        private string BakCodigoLost = "";
        private int BakidVendedor = 0;
        private int bakEmFalta = 0;
        private pesCliente FpesCliente;
        private int idCliente = 0;

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
                if (codigo.Length>0)
                {
                    ret = faltasDAO.VeSeJaTem(codigo);
                }                
                if (ret.Length>0)
                {
                    MessageBox.Show(ret, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } else
                {
                    int idBalconista = Convert.ToInt32(cmbVendedor.SelectedValue); // Assumindo que cmbVendedor agora representa o balconista
                    float quantidade;
                    if (!float.TryParse(txQuantidade.Text, out quantidade))
                    {
                        quantidade = 0;
                    }
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
            cmbTipos.SelectedIndex = 0;
            cmbTipos.FlatStyle = FlatStyle.System;
            cmbTipos.Tag = "";
            cmbForn.FlatStyle = FlatStyle.System;
            cmbForn.SelectedIndex = 0;
            cmbForn.Tag = "";
            Normaliza(txtCodigo,1);
            Normaliza(txMarca,1);
            Normaliza(txDescr,1);
            Normaliza(txObs,1);
            Normaliza(txQuantidade,1);
            btnAdicionar.Text = "Adicionar";
            btnExcluir.Enabled = false;
            btAdicTpo.Enabled = false;
            BakCodigoLost = "";
            BakidTipo = 0;
            BakidForn = 0;
            bakComprado = 0;
            Bakcodigo = "";
            Bakquantidade = -1;
            Bakmarca = "";
            BakObs = "";
            txtCodigo.Focus();
        }

        private void Normaliza(TextBox obj, int tipo)
        {
            if (tipo==1) {
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

        private void VeSeHab(TextBox obj=null)
        {
            if (obj!=null)
            {
                if (!Restrito)
                {
                    if (!obj.ReadOnly)
                    {
                        if (obj.Text.Length>0)
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

        #region Grid
        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Width = 100;       // Compra
            dataGrid1.Columns[1].Width = 130;       // Forn
            dataGrid1.Columns[2].Visible = false;
            dataGrid1.Columns[3].Visible = false;
            dataGrid1.Columns[4].Width = 75;       // Data
            dataGrid1.Columns[5].Width = 80;        // Código
            dataGrid1.Columns[6].Width = 50;        // Quantidade
            dataGrid1.Columns[7].Width = 80;        // Marca
            dataGrid1.Columns[8].Width = 160;       // Descrição
            dataGrid1.Columns[9].Width = 130;       // Balconista
            dataGrid1.Columns[10].Visible = false;  // UID
            dataGrid1.Columns[11].Width = 130;      // Tipo - colocado o texto
            dataGrid1.Columns[12].Visible = false;  // Tipo valor original
            dataGrid1.Columns[13].Visible = false;  // idForn
            dataGrid1.Columns[14].Width = 100;      // Obs
            dataGrid1.Invalidate();
        }

        private void CarregaGrid()
        {
            FaltasDAO faltasDAO = new FaltasDAO();
            DataTable dados = faltasDAO.getDados(BakidTipo, BakidForn, bakComprado, Bakcodigo, Bakquantidade, Bakmarca, BakObs, BakidVendedor, bakEmFalta);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid1.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid1.Rows)
            {
                AtualizarLinha(row, tipos, "Tipo", "Tipo");
                AtualizarLinha(row, Fornecs, "idForn", "Forn");
            }
            if (dados != null)
            {
                ConfigurarGrid();
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
                cmbVendedor.SelectedValue = Convert.ToInt32(selectedRow.Cells["IDBalconista"].Value);
                this.UID = Convert.ToString(selectedRow.Cells["UID"].Value);
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
                    int itemId = Convert.ToInt32(row.Cells[idColumnName].Value);
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

        #endregion

        #region Tipos

        private void MostraTipos()
        {
            if (glo.ODBC)
            {
                glo.CarregarComboBox<tb.TpoFalta>(cmbTipos, TpoFalta, "ESCOLHA");
                glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA");
            } else
            {
                glo.CarregarComboBox<tb.TpoFalta>(cmbTipos, TpoFalta, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
                glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
            }
        }

        private void btAdicTpo_Click(object sender, EventArgs e)
        {
            if (btAdicTpo.Text == "Adicionar")
            {
                if ((txNvTipo.Text.Length == 0) && (txNvForn.Text.Length==0))
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
                    } else
                    {
                        Forn.Adiciona(txNvForn.Text);
                    }                    
                    RetCmboTpo();
                }
            }
            else
            {
                // ATUALIZAÇÃO
                string codigo = "";
                if (txtCodigo.Tag=="M")
                {
                    codigo = txtCodigo.Text;
                }
                int idForn = 0;
                if (cmbForn.Tag == "M")
                {
                    int iForn = cmbForn.SelectedIndex;
                    if (iForn > -1)
                    {
                        idForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
                    }                
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
                int quantidade = -1;
                if (txQuantidade.Tag == "M")
                {
                    if (!int.TryParse(txQuantidade.Text, out quantidade))
                    {
                        quantidade = -1;
                    }
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
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        AtualizaItensSelecionados(0, dataGrid1, idTipo, idForn, codigo, quantidade, marca, Obs, Descr);
                        break;
                    case 1:
                        AtualizaItensSelecionados(1, dataGrid2, idTipo, idForn, codigo, quantidade, marca, Obs, Descr);
                        break;
                    case 2:
                        AtualizaItensSelecionados(2, dataGrid3, idTipo, idForn, codigo, quantidade, marca, Obs, Descr);
                        break;
                }
            }
        }

        private void AtualizaItensSelecionados(int nrGrid, DataGridView grid, int idTipo, int idForn, string codigo, int quantidade, string marca, string obs, string descr)
        {
            HashSet<string> selectedCodes = new HashSet<string>();
            int scrollPosition = grid.FirstDisplayedScrollingRowIndex;

            foreach (DataGridViewRow row in grid.SelectedRows)
            {
                string sID = Convert.ToString(row.Cells["ID"].Value);
                selectedCodes.Add(sID);
                int gID = Convert.ToInt32(row.Cells["ID"].Value);
                string UID = Convert.ToString(row.Cells["UID"].Value);
                glo.Loga($@"FA,{gID}, {idTipo}, {idForn}, {codigo}, {quantidade}, {marca}, {obs}, {descr}, {UID}");
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        faltasDAO.Atualiza(gID, idTipo, idForn, codigo, quantidade, marca, obs, descr);
                        break;
                    case 1:
                        cDaoP.Atualiza(gID, idTipo, idForn, codigo, quantidade, marca, obs, descr);
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
                foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                {
                    int gID = Convert.ToInt32(row.Cells["ID"].Value);
                    faltasDAO.Comprou(gID);
                }
                AtualizarGridP = true;
                CarregaGrid();
                AtualizouEmBaixo();
            }
            else
            {
                RetCmboTpo();
                Limpar();
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
                    }
                    if (ret.Length > 0)
                    {
                        txtCodigo.BackColor = Color.Orange;
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
            int idTipo = cmbTipos.SelectedIndex;
            BakidTipo = 0;
            if (idTipo > 0)
            {
                BakidTipo = ((tb.ComboBoxItem)cmbTipos.Items[idTipo]).Id;
            }
            int iForn = cmbForn.SelectedIndex;
            BakidForn = 0;
            if (iForn > 0)
            {
                BakidForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
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
            Bakquantidade = -1;
            if (txQuantidade.Tag == "M")
            {
                if (!int.TryParse(txQuantidade.Text, out Bakquantidade))
                {
                    Bakquantidade = -1;
                }
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

            switch (tbFaltas.SelectedIndex)
            {
                case 0:
                    bakEmFalta = (ckEmFalta.Checked == true) ? 1 : 0;
                    CarregaGrid(); break;
                case 1:
                    CarregaGridP();
                    break;
                case 2:
                    CarregaGridE();
                    break;
            }
            //if (tbFaltas.SelectedIndex == 1)
            //{
            //    CarregaGridP();
            //}
            //else
            //{
            //    bakEmFalta = (ckEmFalta.Checked == true) ? 1 : 0;
            //    CarregaGrid();
            //}

            btnAdicionar.Text = "Limpar";
            btAdicTpo.Enabled = false;
        }

        private void ckComprado_Click(object sender, EventArgs e)
        {
            button2.Enabled = btLmpFiltro.Enabled = true;
        }

        private void btLmpFiltro_Click(object sender, EventArgs e)
        {
            Limpar();
            BakidTipo = 0;
            BakidForn = 0;
            bakComprado = 0;
            Bakcodigo = "";
            Bakquantidade = -1;
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
            }
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
            dataGrid2.Columns[5].Width = 50;        // Quantidade
            dataGrid2.Columns[6].Width = 80;        // Marca
            dataGrid2.Columns[7].Width = 150;       // Descrição
            dataGrid2.Columns[8].Visible = false;  // UID
            dataGrid2.Columns[9].Width = 130;      // Tipo - colocado o texto
            dataGrid2.Columns[10].Visible = false;  // Tipo valor original
            dataGrid2.Columns[11].Visible = false;  // idForn
            dataGrid2.Columns[12].Width = 100;      // Obs
            dataGrid2.Invalidate();
        }

        private void CarregaGridP()
        {
            cDaoP = new ProdutosDao();
            DataTable dados = cDaoP.getDados(BakidTipo, BakidForn, Bakcodigo, Bakquantidade, Bakmarca, BakObs);
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
            }
        }

        private void tbFaltas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                switch (tbFaltas.SelectedIndex)
                {
                    case 0:
                        btComprei.Visible = true;
                        ckEmFalta.Visible = true;
                         if (iUser.Length > 0)
                        {
                            cmbVendedor.SelectedItem = Convert.ToInt16(iUser);
                        }
                        else
                        {
                            cmbVendedor.Enabled = true;
                        }
                        break;
                    case 1:
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
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
                    case 2:
                        btComprei.Visible = false;
                        ckEmFalta.Visible = false;
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
                this.UID = Convert.ToString(selectedRow.Cells["UID"].Value);
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

        #endregion

        private void ckEmFalta_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        #region Encomendas

        private void btEncomenda_Click(object sender, EventArgs e)
        {
            glo.IdAdicionado = 0;
            string Descricao = "";
            bool ProdNovo = false;
            if (dataGrid1.SelectedRows.Count==1)
            {
                if (dataGrid1.SelectedRows.Count == 1)
                {

                    if (dataGrid1.Rows.Count == 1 || dataGrid1.SelectedRows[0].Index != 0)
                    {
                        Descricao = dataGrid1.SelectedRows[0].Cells["Descricao"].Value.ToString();
                    }
                    else
                    {
                        ProdNovo = true;
                    }
                }
            }
            btEncomenda.Enabled = false; 
            if (FpesCliente == null)
            {
                FpesCliente = new pesCliente();
                FpesCliente.SetDescricao(Descricao, ProdNovo);
                FpesCliente.ShowDialog();
            }
            else
            {                
                try
                {
                    FpesCliente.SetDescricao(Descricao, ProdNovo);
                    FpesCliente.Visible = true;
                }
                catch (Exception)
                {
                    FpesCliente = new pesCliente();
                    FpesCliente.SetDescricao(Descricao, ProdNovo);
                    FpesCliente.ShowDialog();
                }                
            }
            if (!glo.ODBC)
            {
                btEncomenda.Enabled = true;
            }            
            if (FpesCliente.OK)
            {
                string Nome = "";
                string Fone = "";
                string NovaDesc = "";
                int ClienteAdicioado = FpesCliente.ClienteLocalizado;
                if (ClienteAdicioado == 0)
                {
                    Nome = FpesCliente.Nome;
                    Fone = FpesCliente.Fone;
                }
                if (ProdNovo)
                {
                    NovaDesc = FpesCliente.getDescricao();
                }
                DateTime DtAgora = FpesCliente.getDtAgora();
                DateTime DtEnc = FpesCliente.getDtEnc();
                foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                {
                    int gID = Convert.ToInt16(row.Cells["ID"].Value);
                    faltasDAO.ConfirmaEncomenda(gID, Nome, Fone, NovaDesc, DtAgora, DtEnc);
                }
                AtualizarGridE = true;
                if (ProdNovo==false)
                {
                    if (tbFaltas.SelectedIndex==0)
                    {
                        CarregaGrid();
                        AtualizouEmBaixo();
                    }
                }
            }
        }

        private void ConfigurarGridE()
        {
            dataGrid3.Columns[0].Visible = false;   // ID
            dataGrid3.Columns[1].Width = 130;       // Cliente
            dataGrid3.Columns[2].Width = 100;       // Data
            dataGrid3.Columns[3].Width = 100;       // Código
            dataGrid3.Columns[4].Width = 40;        // Quant
            dataGrid3.Columns[5].Width = 50;        // Marca
            dataGrid3.Columns[6].Width = 190;       // Descrição
            dataGrid3.Columns[7].Visible = false;   // UID
            dataGrid3.Columns[8].Width = 100;       // Tipo
            dataGrid3.Columns[9].Width = 130;       // Compra
            dataGrid3.Columns[10].Width = 100;      // Forn
            dataGrid3.Columns[11].Visible = false;  // IdForn
            dataGrid3.Columns[12].Width = 100;      // Obs
            dataGrid3.Columns[13].Visible = false;  // idCliente 
            dataGrid3.Invalidate();
        }

        private void CarregaGridE()
        {
            EncoDao = new EncomendasDao();
            DataTable dados = EncoDao.getDados(BakidTipo, BakidForn, Bakcodigo, Bakquantidade, Bakmarca, BakObs);
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
            }
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
                this.UID = Convert.ToString(selectedRow.Cells["UID"].Value);
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
            if (texto=="0")
            {
                return "";
            } else
            {
                return texto;
            }
        }

    }
}