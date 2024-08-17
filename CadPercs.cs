using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class CadPercs : Form
    {

        private int iID = 0;
        private PercentsDAO Percen;

        public CadPercs()
        {
            InitializeComponent();
        }

        private void CadPercs_Load(object sender, EventArgs e)
        {
            dataGridView1.Font = new System.Drawing.Font("Segoe UI", 12);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ColumnHeadersHeight = 30;
            Percen = new PercentsDAO();
            AtualizaGrid();
        }

        private void AtualizaGrid()
        {
            DataTable dados = Percen.getDados();

            // Verifica se há linhas no DataTable
            if (dados.Rows.Count > 0)
            {
                // Alterar o valor do último item para "acima disso"
                dados.Rows[dados.Rows.Count - 1]["Expr1002"] = "acima disso";
            }

            dataGridView1.DataSource = dados;

            // ID - Ocultar
            dataGridView1.Columns[0].Visible = false;

            // Perc - Mostrar como percentual
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[1].HeaderText = "Perc";
            dataGridView1.Columns[1].DefaultCellStyle.Format = "0'%'";

            // Até ou "acima disso" - Sem título e centralizado
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[2].HeaderText = "";
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Valor - Mostrar como valor monetário
            dataGridView1.Columns[3].Width = 120;
            dataGridView1.Columns[3].HeaderText = "Valor";
            dataGridView1.Columns[3].DefaultCellStyle.Format = "C2";

            if (rt.IsLargeScreen())
            {
                for (int i = 0; i < 3; i++)
                {
                    dataGridView1.Columns[i].Width = (int)(dataGridView1.Columns[i].Width * rt.scaleFactor);
                }
            }

            dataGridView1.Invalidate();
            btAdic.Enabled = false;
            btnExcluir.Enabled = false;
            txPerc.Text = "";
            txValor.Text = "";

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (grid != null && e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count)
            {

                DataGridViewRow selectedRow = grid.Rows[e.RowIndex];
                this.iID = Convert.ToInt32(selectedRow.Cells["ID"].Value); // Armazena o ID selecionado, se necessário

                // Preenche os campos de texto com os valores selecionados da célula
                txPerc.Text = Convert.ToString(selectedRow.Cells["Perc"].Value) + "%"; // Exibe o percentual com o símbolo de porcentagem
                txValor.Text = glo.fmtVlr(Convert.ToString(selectedRow.Cells["Valor"].Value)); // Formata o valor como monetário

                // Configura o botão de adicionar e excluir
                btAdic.Text = "Salvar";
                btAdic.Enabled = true;
                btnExcluir.Enabled = true;

            }
        }

        private void btAdic_Click(object sender, EventArgs e)
        {
            float perc = glo.LeValor(txPerc.Text);
            float valor = glo.LeValor(txValor.Text);
            if (btAdic.Text == "Salvar")
            {
                Percen.Edita(this.iID, perc, valor);
            } else
            {
                Percen.InsertPercent(perc, valor);
            }                        
            AtualizaGrid();
        }

        private void txPerc_KeyUp(object sender, KeyEventArgs e)
        {
            if ((txPerc.Text.Length>0) && (txValor.Text.Length>0))
            {
                btAdic.Enabled = true;
            } else
            {
                btAdic.Enabled = false;
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
                Percen.DeletePercentById(this.iID);
                AtualizaGrid();
            }                            
        }
    }

}