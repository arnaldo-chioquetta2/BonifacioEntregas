﻿using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{

    public partial class opeTpContrato : Form
    {

        private dao.ContratosDAO contratosDao = new dao.ContratosDAO();
        private ClausulasDAO clausulasDAO = new ClausulasDAO();
        private bool carregando = false;
        private int cadastroAssociado = 0;

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
                novaLinha["Nome"] = "Escolha o tipo de Contrato";
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
            lstClausulas.DataSource = null;
            lstClausulas.Rows.Clear();
            lstClausulas.Columns.Clear();
            lstClausulas.Columns.Add("Texto", "Cláusula");
            lstClausulas.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            lstClausulas.RowHeadersVisible = false; // Remove a numeração da lateral
            ContratosDAO contratosDAO = new ContratosDAO();
            DataTable clausulas = clausulasDAO.GetClausulasByTipoContrato(idTipoContrato);
            foreach (DataRow row in clausulas.Rows)
            {
                string texto = row["Texto"].ToString();
                lstClausulas.Rows.Add(texto);
            }
            txtNovaClausula.Clear();
            btnExcluirClausula.Enabled = false;
        }

        private void MostraTipo(int idTipoContrato)
        {
            switch (idTipoContrato)
            {
                case 1:
                    lbTiContrato.Text = "Funcionários";
                    break;
                case 2: // Clientes
                    lbTiContrato.Text = "Clientes";
                    break;
                case 3: // Entregadores
                    lbTiContrato.Text = "Motoboys";
                    break;
                case 4: // Fornecedores
                    lbTiContrato.Text = "Fornecedores";
                    break;
                default:
                    lbTiContrato.Text = "?";
                    break;
            }
        }

        private void cmbTiposContrato_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (carregando==false)
            {
                if (cmbTiposContrato.SelectedValue != null && int.TryParse(cmbTiposContrato.SelectedValue.ToString(), out int idTipoContrato))
                {
                    MostrarDadosTipoContrato(idTipoContrato);
                    HabilitaBotoes();
                    MostraTipo(idTipoContrato);
                }
            }            
        }

        private void HabilitaBotoes()
        {
            btnAdicionarClausula.Enabled = true;
            btnExcluirClausula.Enabled = true;
            btnSalvarClausula.Enabled = true;
            // btSalvaContrato.Enabled = true;
        }

        private void lstClausulas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Garante que a linha clicada é válida
            {
                var row = lstClausulas.Rows[e.RowIndex];
                txtNovaClausula.Text = row.Cells[0].Value.ToString(); // Exibe o texto no campo de edição
                lblNovaClausula.Text = "Editar Cláusula";
                btnExcluirClausula.Enabled = true;
                btnSalvarClausula.Enabled = true;
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

                    clausulasDAO.DeleteClausulaByTexto(clausulaTexto); // Implemente este método para deletar no banco

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
                //ContratosDAO contratosDAO = new ContratosDAO();

                // Obter a próxima ordem para a cláusula
                int novaOrdem = clausulasDAO.GetProximaOrdemClausula(idTipoContrato);

                // Inserir a nova cláusula no banco de dados
                clausulasDAO.InsertClausula(idTipoContrato, novaOrdem, textoClausula);

                // Atualizar a lista de cláusulas exibida
                MostrarDadosTipoContrato(idTipoContrato);

                // Limpar o campo de texto
                txtNovaClausula.Clear();

                // MessageBox.Show("Cláusula adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar a cláusula: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalvarClausula_Click(object sender, EventArgs e)
        {
            // Verifica se há uma linha selecionada no DataGridView
            if (lstClausulas.CurrentRow != null)
            {
                // Obtém o texto original da cláusula a partir da célula correspondente
                string textoOriginal = lstClausulas.CurrentRow.Cells["Texto"].Value.ToString();
                string novoTexto = txtNovaClausula.Text.Trim();

                // Verifica se o campo de texto não está vazio
                if (string.IsNullOrWhiteSpace(novoTexto))
                {
                    MessageBox.Show("O texto da cláusula não pode estar vazio.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Atualiza a cláusula no banco de dados
                ClausulasDAO clausulasDAO = new ClausulasDAO();
                clausulasDAO.UpdateClausulaByTexto(textoOriginal, novoTexto);

                // Atualiza o texto da célula no DataGridView
                lstClausulas.CurrentRow.Cells["Texto"].Value = novoTexto;

                // Limpa o campo de texto após a atualização
                txtNovaClausula.Clear();

                MessageBox.Show("Cláusula atualizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma cláusula para atualizar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdicionarTipo_Click(object sender, EventArgs e)
        {
            carregando = true;
            string novoTipoContrato = txtNovoTipoContrato.Text.Trim();
            cadastroAssociado = cmbAssociarDados.SelectedIndex + 1; // Assume que os índices correspondem aos cadastros

            // Validações
            if (string.IsNullOrWhiteSpace(novoTipoContrato))
            {
                carregando = false;
                MessageBox.Show("Por favor, insira o nome do novo tipo de contrato.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cadastroAssociado < 1 || cadastroAssociado > 4)
            {
                carregando = false;
                MessageBox.Show("Por favor, selecione o cadastro associado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Adiciona o novo tipo de contrato ao banco de dados
            try
            {
                //ContratosDAO contratosDAO = new ContratosDAO();
                contratosDao.InsertTipoContrato(novoTipoContrato, cadastroAssociado);

                // Atualiza o combo de tipos de contrato
                CarregarTiposDeContrato();

                // Busca o índice do novo tipo de contrato pelo nome e o seleciona
                for (int i = 0; i < cmbTiposContrato.Items.Count; i++)
                {
                    if (cmbTiposContrato.GetItemText(cmbTiposContrato.Items[i]).Equals(novoTipoContrato, StringComparison.OrdinalIgnoreCase))
                    {
                        cmbTiposContrato.SelectedIndex = i;
                        break;
                    }
                }

                // Limpa os campos e reseta a lista de cláusulas
                txtNovoTipoContrato.Clear();
                cmbAssociarDados.SelectedIndex = -1;
                lstClausulas.DataSource = null; // Limpa a lista de cláusulas

                MostraTipo(cadastroAssociado);
                HabilitaBotoes();
                carregando = false;
            }
            catch (Exception ex)
            {
                carregando = false;
                MessageBox.Show($"Erro ao adicionar tipo de contrato: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
