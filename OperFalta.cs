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
            if (NrLista>0)
            {
                comboBox.SelectedValue = NrLista;
            } else
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
            cmbForn.FlatStyle = FlatStyle.System;
            cmbForn.SelectedIndex = 0;
            Normaliza(txtCodigo,1);
            Normaliza(txMarca,1);
            Normaliza(txDescr,1);
            Normaliza(txObs,1);
            Normaliza(txQuantidade,1);
            btnAdicionar.Text = "Adicionar";
            btnExcluir.Enabled = false;
            btAdicTpo.Enabled = false;
            btEncomenda.Enabled = false;
            BakCodigoLost = "";
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
                                btAdicTpo.Enabled = true;
                            }                                                             
                            button2.Enabled = btLmpFiltro.Enabled = true;
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

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (tbFaltas.SelectedIndex == 0)
                {
                    foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                    {
                        int gID = Convert.ToInt32(row.Cells["ID"].Value);
                        string UID = Convert.ToString(row.Cells["UID"].Value);
                        glo.Loga($@"FD,{gID}, {UID}");
                        faltasDAO.Exclui(gID);
                    }
                    CarregaGrid();
                }
                else
                {
                    ProdutosDao cDao = new ProdutosDao();
                    foreach (DataGridViewRow row in dataGrid2.SelectedRows)
                    {
                        int gID = Convert.ToInt32(row.Cells["ID"].Value);
                        string UID = Convert.ToString(row.Cells["UID"].Value);
                        glo.Loga($@"FD,{gID}, {UID}");
                        cDao.Exclui(gID);
                    }
                    CarregaGridP();
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
            dataGrid1.Columns[4].Width = 100;       // Data
            dataGrid1.Columns[5].Width = 80;        // Código
            dataGrid1.Columns[6].Width = 50;        // Quantidade
            dataGrid1.Columns[7].Width = 80;        // Marca
            dataGrid1.Columns[8].Width = 150;       // Descrição
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
                if (!row.Cells["Tipo"].Value.Equals(DBNull.Value))
                {
                    int tipoId = Convert.ToInt32(row.Cells["Tipo"].Value);
                    var tipoEncontrado = tipos.Find(t => t.Id == tipoId);
                    if (tipoEncontrado != null)
                    {
                        row.Cells["Tipo"].Value = tipoEncontrado.Nome;
                        if (tipoId == 8)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                    }
                }
                if (!row.Cells["idForn"].Value.Equals(DBNull.Value))
                {
                    int FornId = Convert.ToInt32(row.Cells["idForn"].Value);
                    var fornEncontrado = Fornecs.Find(f => f.Id == FornId);
                    if (fornEncontrado != null)
                    {
                        row.Cells["Forn"].Value = fornEncontrado.Nome;                        
                    }
                }
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
                txQuantidade.Text = Convert.ToString(selectedRow.Cells["Quant"].Value);
                txtCodigo.Text = Convert.ToString(selectedRow.Cells["Codigo"].Value);
                txMarca.Text = Convert.ToString(selectedRow.Cells["Marca"].Value);
                cmbVendedor.SelectedValue = Convert.ToInt32(selectedRow.Cells["IDBalconista"].Value);
                txObs.Text = Convert.ToString(selectedRow.Cells["Obs"].Value);
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
                btnAdicionar.Text = "Limpar";
                btnAdicionar.Enabled = true;
                btnExcluir.Enabled = true;
                btComprei.Enabled = true;
                btEncomenda.Enabled = true;
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

        #endregion

        #region Tipos

        private void MostraTipos()
        {
            glo.CarregarComboBox<tb.TpoFalta>(cmbTipos, TpoFalta, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
            glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
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
                int idForn = 0;
                if (cmbForn.FlatStyle == FlatStyle.Flat)
                {
                    int iForn = cmbForn.SelectedIndex;
                    if (iForn > -1)
                    {
                        idForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
                    }                
                }
                int idTipo = 0;
                if (cmbTipos.FlatStyle == FlatStyle.Flat)
                {
                    int iTpo = cmbTipos.SelectedIndex;
                    if (iTpo > -1)
                    {
                        idTipo = ((tb.ComboBoxItem)cmbTipos.Items[iTpo]).Id;
                    }
                }
                string codigo = "";
                if (txtCodigo.BackColor == Color.Yellow)
                {
                    codigo = txtCodigo.Text;
                }
                int quantidade = -1;
                if (txQuantidade.BackColor == Color.Yellow)
                {
                    if (!int.TryParse(txQuantidade.Text, out quantidade))
                    {
                        quantidade = -1;
                    }
                }
                string marca = "";
                if (txMarca.BackColor == Color.Yellow)
                {
                    marca = txMarca.Text;
                }
                string Obs = "";
                if (txObs.BackColor == Color.Yellow)
                {
                    Obs = txObs.Text;
                }
                string Descr = "";
                if (txDescr.BackColor == Color.Yellow)
                {
                    Descr = txDescr.Text;
                }
                HashSet<string> selectedCodes = new HashSet<string>();
                int scrollPosition = dataGrid1.FirstDisplayedScrollingRowIndex;
                foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                {
                    selectedCodes.Add((string)row.Cells["Codigo"].Value);
                    int gID = Convert.ToInt32(row.Cells["ID"].Value);
                    string UID = Convert.ToString(row.Cells["UID"].Value);
                    glo.Loga($@"FA,{gID}, {idTipo}, {idForn} ,{codigo}, {quantidade},{marca}, {Obs}, {Descr}, {UID}");
                    faltasDAO.Atualiza(gID, idTipo, idForn, codigo, quantidade, marca, Obs, Descr);
                }
                CarregaGrid();
                if (dataGrid1.Rows.Count>0)
                {
                    dataGrid1.FirstDisplayedScrollingRowIndex = scrollPosition;
                    foreach (string code in selectedCodes)
                    {
                        foreach (DataGridViewRow row in dataGrid1.Rows)
                        {
                            string cod = row.Cells["Codigo"].Value.ToString();
                            if (cod == code)
                            {
                                row.Selected = true;
                                break;
                            }
                        }
                    }
                    AtualizouEmBaixo();
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
                        MessageBox.Show(ret, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    txtCodigo.Focus();
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
            if (tbFaltas.SelectedIndex == 1)
            {
                CarregaGridP();
            }
            else
            {
                bakEmFalta = (ckEmFalta.Checked == true) ? 1 : 0;
                CarregaGrid();
            }
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
            if (tbFaltas.SelectedIndex == 1)
            {
                CarregaGridP();
            }
            else
            {
                CarregaGrid();
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
            ProdutosDao cDao = new ProdutosDao();
            DataTable dados = cDao.getDados(BakidTipo, BakidForn, Bakcodigo, Bakquantidade, Bakmarca, BakObs);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid2.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid2.Rows)
            {
                if ((!row.Cells["Tipo"].Value.Equals(DBNull.Value)) && (row.Cells["Tipo"].Value.ToString().Length > 0))
                {
                    int tipoId = Convert.ToInt32(row.Cells["Tipo"].Value);
                    var tipoEncontrado = tipos.Find(t => t.Id == tipoId);
                    if (tipoEncontrado != null)
                    {
                        row.Cells["Tipo"].Value = tipoEncontrado.Nome;
                    }
                }
                if (!row.Cells["idForn"].Value.Equals(DBNull.Value))
                {
                    int FornId = Convert.ToInt32(row.Cells["idForn"].Value);
                    var fornEncontrado = Fornecs.Find(f => f.Id == FornId);
                    if (fornEncontrado != null)
                    {
                        row.Cells["Forn"].Value = fornEncontrado.Nome;
                    }
                }
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
                        btEncomenda.Visible = true;
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
                        btEncomenda.Visible = false;
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
                        btEncomenda.Visible = false;
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

        private void dataGrid2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
                carregando = true;
                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                txQuantidade.Text = Convert.ToString(selectedRow.Cells["Quant"].Value);
                txtCodigo.Text = Convert.ToString(selectedRow.Cells["Codigo"].Value);
                txMarca.Text = Convert.ToString(selectedRow.Cells["Marca"].Value);
                txObs.Text = Convert.ToString(selectedRow.Cells["Obs"].Value);
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
            if (FpesCliente == null)
            {
                FpesCliente = new pesCliente();
                FpesCliente.ShowDialog();
            }
            else
            {
                FpesCliente.Visible = true;
            }
            if (glo.IdAdicionado > 0)
            {
                foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                {
                    int gID = Convert.ToInt32(row.Cells["ID"].Value);
                    faltasDAO.ConfirmaEncomenda(gID);
                }
                AtualizarGridE = true;
                CarregaGrid();
                AtualizouEmBaixo();
            }
        }

        private void ConfigurarGridE()
        {
            dataGrid3.Columns[0].Visible = false;   // ID
            dataGrid3.Columns[1].Width = 130;       // idCliente
            dataGrid3.Columns[2].Width = 100;       // Data
            dataGrid3.Columns[3].Width = 100;       // Código
            dataGrid3.Columns[4].Width = 80;        // Quant
            dataGrid3.Columns[5].Width = 50;        // Marca
            dataGrid3.Columns[6].Width = 190;       // Descrição
            dataGrid3.Columns[7].Visible = false;   // UID
            dataGrid3.Columns[8].Width = 100;       // Tipo
            dataGrid3.Columns[9].Width = 130;       // Compra
            dataGrid3.Columns[10].Visible = false;  // IdForn
            dataGrid3.Columns[11].Width = 190;      // Obs
            dataGrid3.Invalidate();

        }

        private void CarregaGridE()
        {
            EncomendasDao cDao = new EncomendasDao();
            DataTable dados = cDao.getDados(BakidTipo, BakidForn, Bakcodigo, Bakquantidade, Bakmarca, BakObs);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid3.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid3.Rows)
            {
                if ((!row.Cells["Tipo"].Value.Equals(DBNull.Value)) && (row.Cells["Tipo"].Value.ToString().Length > 0))
                {
                    int tipoId = Convert.ToInt32(row.Cells["Tipo"].Value);
                    var tipoEncontrado = tipos.Find(t => t.Id == tipoId);
                    if (tipoEncontrado != null)
                    {
                        row.Cells["Tipo"].Value = tipoEncontrado.Nome;
                    }
                }
                //if (!row.Cells["idForn"].Value.Equals(DBNull.Value))
                //{
                //    int FornId = Convert.ToInt32(row.Cells["idForn"].Value);
                //    var fornEncontrado = Fornecs.Find(f => f.Id == FornId);
                //    if (fornEncontrado != null)
                //    {
                //        row.Cells["Forn"].Value = fornEncontrado.Nome;
                //    }
                //}
            }
            if (dados != null)
            {
                ConfigurarGridE();
            }
        }

        #endregion
    }
}