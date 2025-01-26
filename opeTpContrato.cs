using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{

    public partial class opeTpContrato : Form
    {

        private dao.ContratosDAO contratosDao = new dao.ContratosDAO();

        public opeTpContrato()
        {
            InitializeComponent();
            CarregarTiposDeContrato();
        }

        private void CarregarTiposDeContrato()
        {
            DataTable tiposContrato = contratosDao.GetTiposDeContrato();

            if (tiposContrato != null && tiposContrato.Rows.Count > 0)
            {
                // Cria uma nova linha para o item inicial
                DataRow novaLinha = tiposContrato.NewRow();
                novaLinha["IdTipoContrato"] = -1; // ID fictício
                novaLinha["Nome"] = "Escolha o Contrato";
                tiposContrato.Rows.InsertAt(novaLinha, 0); // Insere na primeira posição

                // Popula o ComboBox
                cmbTiposContrato.DataSource = tiposContrato;
                cmbTiposContrato.DisplayMember = "Nome"; // Nome a ser exibido
                cmbTiposContrato.ValueMember = "IdTipoContrato"; // Valor associado
                cmbTiposContrato.SelectedIndex = 0; // Seleciona o item inicial
            }
            else
            {
                MessageBox.Show("Nenhum tipo de contrato encontrado. Por favor, adicione um novo tipo de contrato.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MostrarDadosTipoContrato(int idTipoContrato)
        {
            // Limpa os dados do DataGridView
            lstClausulas.DataSource = null;
            lstClausulas.Rows.Clear();
            lstClausulas.Columns.Clear();

            // Adiciona a coluna de cláusulas ao DataGridView
            lstClausulas.Columns.Add("Texto", "Cláusula");
            lstClausulas.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            lstClausulas.RowHeadersVisible = false; // Remove a numeração da lateral

            // Consulta ao banco de dados para buscar as cláusulas
            ContratosDAO contratosDAO = new ContratosDAO();
            DataTable clausulas = contratosDAO.GetClausulasByTipoContrato(idTipoContrato);

            // Adiciona as cláusulas ao DataGridView
            foreach (DataRow row in clausulas.Rows)
            {
                string texto = row["Texto"].ToString();
                lstClausulas.Rows.Add(texto);
            }

            // Limpa o campo de nova cláusula e desabilita o botão de excluir
            txtNovaClausula.Clear();
            btnExcluirClausula.Enabled = false;
        }

        private void cmbTiposContrato_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmbTiposContrato.SelectedValue != null && int.TryParse(cmbTiposContrato.SelectedValue.ToString(), out int idTipoContrato))
            {
                MostrarDadosTipoContrato(idTipoContrato);
                HabilitaBotoes();
            }
        }

        private void HabilitaBotoes()
        {
            btnAdicionarClausula.Enabled = true;
            btnExcluirClausula.Enabled = true;
            btnSalvarClausula.Enabled = true;
            btSalvaContrato.Enabled = true;
        }

        private void lstClausulas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Garante que a linha clicada é válida
            {
                var row = lstClausulas.Rows[e.RowIndex];
                txtNovaClausula.Text = row.Cells[0].Value.ToString(); // Exibe o texto no campo de edição
                btnExcluirClausula.Enabled = true; // Habilita o botão de excluir
                lblNovaClausula.Text = "Editar Cláusula";
            }
        }

        private void btnExcluirClausula_Click(object sender, System.EventArgs e)
        {
            if (lstClausulas.CurrentRow != null)
            {
                // Confirmar exclusão
                DialogResult result = MessageBox.Show("Deseja realmente excluir esta cláusula?",
                                                      "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int rowIndex = lstClausulas.CurrentRow.Index;
                    string clausulaTexto = lstClausulas.CurrentRow.Cells[0].Value.ToString();

                    // Remover do DataGridView
                    lstClausulas.Rows.RemoveAt(rowIndex);

                    // Opcional: Atualizar o banco de dados
                    ContratosDAO contratosDAO = new ContratosDAO();
                    contratosDAO.DeleteClausulaByTexto(clausulaTexto); // Implemente este método para deletar no banco

                    txtNovaClausula.Clear();
                    btnExcluirClausula.Enabled = false;
                }
            }

        }

        private void btnAdicionarClausula_Click(object sender, EventArgs e)
        {
            // Verificar se há um tipo de contrato selecionado

            // Verificar se o campo da nova cláusula está preenchido
            if (string.IsNullOrWhiteSpace(txtNovaClausula.Text))
            {
                MessageBox.Show("Digite o texto da cláusula antes de adicioná-la.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obter o ID do tipo de contrato selecionado
            int idTipoContrato = Convert.ToInt32(cmbTiposContrato.SelectedValue);

            // Obter o texto da cláusula
            string textoClausula = txtNovaClausula.Text.Trim();

            try
            {
                // Instanciar o DAO
                ContratosDAO contratosDAO = new ContratosDAO();

                // Obter a próxima ordem para a cláusula
                int novaOrdem = contratosDAO.GetProximaOrdemClausula(idTipoContrato);

                // Inserir a nova cláusula no banco de dados
                contratosDAO.InsertClausula(idTipoContrato, novaOrdem, textoClausula);

                // Atualizar a lista de cláusulas exibida
                MostrarDadosTipoContrato(idTipoContrato);

                // Limpar o campo de texto
                txtNovaClausula.Clear();

                MessageBox.Show("Cláusula adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar a cláusula: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
