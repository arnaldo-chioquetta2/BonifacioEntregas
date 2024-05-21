using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TeleBonifacio.dao;
using static System.Windows.Forms.DataGrid;

namespace TeleBonifacio
{
    public partial class OperFalta : Form
    {
        private FaltasDAO faltasDAO;
        private TpoFaltaDAO TpoFalta;
        private bool carregando = true;
        private string UID = "";
        private int iID = 0;

        public OperFalta()
        {
            InitializeComponent();
        }

        private void OperFalta_Load(object sender, EventArgs e)
        {
            VendedoresDAO Vendedor = new VendedoresDAO();
            glo.CarregarComboBox<tb.Vendedor>(cmbVendedor, Vendedor, "", " Where Vendedores.Atende = -1 or Vendedores.Atende = 1 ", " desc ");
            cmbVendedor.SelectedIndex = -1;
            faltasDAO = new FaltasDAO();
            TpoFalta = new TpoFaltaDAO();
            CarregaGrid(0);
            ConfigurarGrid();            
            MostraTipos();
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
            int idBalconista = Convert.ToInt32(cmbVendedor.SelectedValue); // Assumindo que cmbVendedor agora representa o balconista
            float quantidade;
            if (!float.TryParse(txQuantidade.Text, out quantidade))
            {
                quantidade = 0;
            }
            string codigo = txtCodigo.Text;
            string Marca = txMarca.Text;

            if (btnAdicionar.Text == "Limpar")
            {
                Limpar();
            }
            else
            {
                string UID = glo.GenerateUID();
                glo.Loga($@"FA,{idBalconista}, {quantidade}, {codigo}, {Marca}, {UID}");
                faltasDAO.Adiciona(idBalconista, quantidade, codigo, Marca, UID);
            }
            CarregaGrid(0);
            Limpar();
        }

        private void Limpar()
        {
            cmbVendedor.SelectedIndex = -1;
            txQuantidade.Text = "";
            txMarca.Text = "";
            txtCodigo.Text = "";
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
            if (txQuantidade.Text == "")
            {
                ok = false;
            }
            if (txMarca.Text == "")
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

        #endregion

        #region Grid

        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Visible = false;
            dataGrid1.Columns[1].Visible = false;
            dataGrid1.Columns[2].Width = 100; // Data
            dataGrid1.Columns[3].Width = 100; // Código
            dataGrid1.Columns[4].Width = 50; // Quantidade
            dataGrid1.Columns[5].Width = 100; // Marca
            dataGrid1.Columns[6].Width = 200; // Vendedor
            dataGrid1.Columns[7].Visible = false; // UID
            dataGrid1.Columns[8].Width = 100; // Tipo
            dataGrid1.Invalidate();
        }

        private void CarregaGrid(int tipo)
        {
            FaltasDAO faltasDAO = new FaltasDAO();
            DataTable dados = faltasDAO.getDados(tipo);
            List<tb.TpoFalta> tipos = TpoFalta.getTipos();

            // List<tb.TpoFalta> Tipos = TpoFalta.getTipos(); // erro de sintaxe aqui
            foreach (DataRow row in dados.Rows)
            {
                if (!row.IsNull("Tipo") && !string.IsNullOrEmpty(row["Tipo"].ToString()) && int.TryParse(row["Tipo"].ToString(), out int tipoId))
                {
                    var tipoEncontrado = tipos.Find(t => t.Id == tipoId);
                    if (tipoEncontrado != null)
                    {
                        row["Tipo"] = tipoEncontrado.Nome;
                    }
                    else
                    {
                        row["Tipo"] = DBNull.Value; 
                    }
                }
            }
            DevAge.ComponentModel.BoundDataView boundDataView = new DevAge.ComponentModel.BoundDataView(dados.DefaultView);
            dataGrid1.DataSource = boundDataView;
        }

        private void dataGrid1_DoubleClick(object sender, EventArgs e)
        {
            SourceGrid.DataGrid grid = (SourceGrid.DataGrid)sender;
            if (grid != null && grid.Rows.Count > 0)
            {
                SourceGrid.Position position = grid.Selection.ActivePosition;
                if (position != SourceGrid.Position.Empty)
                {
                    this.iID = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["ID"]);
                    txQuantidade.Text = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["Quant"]);
                    txtCodigo.Text = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["Codigo"]);
                    txMarca.Text = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["Marca"]);
                    cmbVendedor.SelectedValue = glo.ConvOjbInt(((DataRowView)grid.SelectedDataRows[0]).Row["IDBalconista"]);
                    this.UID = glo.ConvOjbStr(((DataRowView)grid.SelectedDataRows[0]).Row["UID"]);
                    ReadlyOnly(true);
                    btnAdicionar.Text = "Limpar";
                    btnExcluir.Enabled = true;
                }
            }

        }

        #endregion

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                glo.Loga($@"FD,{this.iID}, {this.UID}");
                faltasDAO.Exclui(this.iID);
                CarregaGrid(0);
                Limpar();
            }
        }

        #region Tipos

        private void MostraTipos()
        {
            glo.CarregarComboBox<tb.TpoFalta>(cmbTipos, TpoFalta, "ESCOLHA",ItemFinal:"ADICIONE");
            glo.CarregarComboBox<tb.TpoFalta>(cmbTiposFiltro, TpoFalta, "TODOS");
        }

        private void btAdicTpo_Click(object sender, EventArgs e)
        {
            if (btAdicTpo.Text == "Adicionar")
            {
                TpoFalta.Adiciona(txNvTipo.Text);
                txNvTipo.Text = "";
                txNvTipo.Visible = false;
                cmbTipos.Visible = true;
                cmbTipos.DataSource = null;
                MostraTipos();
                btAdicTpo.Text = "Indicar tipo";
                btAdicTpo.Enabled = false;
            } else
            {
                int iTpo = cmbTipos.SelectedIndex;
                int idTipo = ((tb.ComboBoxItem)cmbTipos.Items[iTpo]).Id;
                faltasDAO.SetaTipo(iID, idTipo);
                CarregaGrid(0);
                cmbTipos.SelectedIndex = 0;
                btAdicTpo.Enabled = false;
            }
        }

        private void cmbTipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
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
                        txNvTipo.Focus();
                    } else
                    {
                        btAdicTpo.Enabled = true;
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
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int iTpo = cmbTiposFiltro.SelectedIndex;
            int idTipo = ((tb.ComboBoxItem)cmbTiposFiltro.Items[iTpo]).Id;
            CarregaGrid(idTipo);
        }

        #endregion
    }
}