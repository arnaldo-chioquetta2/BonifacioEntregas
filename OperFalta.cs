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

        public OperFalta()
        {
            InitializeComponent();
            SetStartPosition();
        }

        private void OperFalta_Load(object sender, EventArgs e)
        {
            VendedoresDAO Vendedor = new VendedoresDAO();
            INI2 cINI2 = new INI2();
            string iUser = cINI2.ReadString("Usuario", "User", "");
            CarregarComboBoxV<tb.Vendedor>(cmbVendedor, Vendedor, iUser);
            faltasDAO = new FaltasDAO();
            TpoFalta = new TpoFaltaDAO();
            Forn = new FornecedorDao();
            CarregaGrid(0,0,0);
            MostraTipos();            
            //if (iUser.Length==0)
            //{
            //    groupBox2.Visible = false;
            //}
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
            if (btnAdicionar.Text == "Limpar")
            {
                Limpar();
            }
            else
            {
                string codigo = txtCodigo.Text;
                string ret = faltasDAO.VeSeJaTemAFalta(codigo);
                if (ret.Length>0)
                {
                    MessageBox.Show(ret, "Já foi lançada a falta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    glo.Loga($@"FA,{idBalconista}, {quantidade}, {codigo}, {Marca},{Descr} ,{Obs} , {UID}");
                    faltasDAO.Adiciona(idBalconista, quantidade, codigo, Marca, Descr, Obs, UID);
                }
            }
            CarregaGrid(0,0,0);
            Limpar();
        }

        private void Limpar()
        {
            cmbVendedor.SelectedIndex = -1;
            txQuantidade.Text = "0";
            txMarca.Text = "";
            txtCodigo.Text = "";
            txDescr.Text = "";
            txObs.Text = "";
            ReadlyOnly(false);
            btnAdicionar.Text = "Adicionar";
            btnExcluir.Enabled = false;
            txtCodigo.Focus();
        }

        private void txtCodigo_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab();
        }

        private void VeSeHab()
        {
            bool ok = true;
            if (cmbVendedor.SelectedIndex == -1)
            {
                ok = false;
            }
            if (txQuantidade.Text.Length == 0)
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
            VeSeHab();
        }

        private void txMarca_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab();
        }

        private void cmbVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                VeSeHab();
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
                foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                {
                    int gID = Convert.ToInt32(row.Cells["ID"].Value);
                    string UID = Convert.ToString(row.Cells["UID"].Value);                    
                    glo.Loga($@"FD,{gID}, {UID}");
                    faltasDAO.Exclui(gID);
                }
                CarregaGrid(0,0,0);
                Limpar();
            }
        }

        private void txDescr_KeyUp(object sender, KeyEventArgs e)
        {
            VeSeHab();
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

        private void CarregaGrid(int tipo, int idForn, int Comprado)
        {
            FaltasDAO faltasDAO = new FaltasDAO();
            DataTable dados = faltasDAO.getDados(tipo, idForn, Comprado);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();
            List<tb.Fornecedor> Fornecs = Forn.getForns();
            dataGrid1.DataSource = dados;
            foreach (DataGridViewRow row in dataGrid1.Rows)
            {
                if (row.Cells["Compra"].Value.ToString()!="")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White; 
                }
                if (!row.Cells["Tipo"].Value.Equals(DBNull.Value))
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
            ConfigurarGrid();
        }

        private void dataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {
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
            }
        }

        #endregion

        #region Tipos

        private void MostraTipos()
        {
            glo.CarregarComboBox<tb.TpoFalta>(cmbTipos, TpoFalta, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
            glo.CarregarComboBox<tb.TpoFalta>(cmbTiposFiltro, TpoFalta, "TODOS");
            glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn, "ESCOLHA", ItemFinal: "ADICIONE", ItemFinal2: "EDIÇÃO");
            glo.CarregarComboBox<tb.Fornecedor>(cmbFornFiltro, Forn, "TODOS");
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
                int iForn = cmbForn.SelectedIndex;
                int idForn = 0;
                if (iForn > -1)
                {
                    idForn = ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
                }
                int iTpo = cmbTipos.SelectedIndex;
                int idTipo = 0;
                if (iTpo > -1)
                {
                    idTipo = ((tb.ComboBoxItem)cmbTipos.Items[iTpo]).Id;
                }
                foreach (DataGridViewRow row in dataGrid1.SelectedRows)
                {
                    int gID = Convert.ToInt32(row.Cells["ID"].Value);
                    string UID = Convert.ToString(row.Cells["UID"].Value);
                    glo.Loga($@"FA,{gID}, {idTipo}, {idForn} ,{UID}");
                    faltasDAO.Atualiza(gID, idTipo, idForn);
                }
                AtualizouEmBaixo();
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
            CarregaGrid(0,0,0);
            cmbTipos.SelectedIndex = 0;
            btAdicTpo.Enabled = false;
            btComprei.Enabled = false;
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
                AtualizouEmBaixo();
            }
            else
            {
                RetCmboTpo();
            }
        }

        #endregion

        #region Filtro
        private void cmbTiposFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int iTpo = cmbTiposFiltro.SelectedIndex;
            int idTipo = 0;
            if (iTpo>-1)
            {
                idTipo = ((tb.ComboBoxItem)cmbTiposFiltro.Items[iTpo]).Id;
            }
            int iForn = cmbFornFiltro.SelectedIndex;
            int idForn = 0;
            if (iForn > -1)
            {
                idForn = ((tb.ComboBoxItem)cmbFornFiltro.Items[iForn]).Id;
            }
            int Comprado = (ckComprado.Checked) ? 1 : 0;
            CarregaGrid(idTipo, idForn, Comprado);
        }

        private void ckComprado_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        #endregion

    }
}