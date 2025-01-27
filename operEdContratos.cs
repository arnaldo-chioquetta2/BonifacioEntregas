using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class operEdContratos : Form
    {
        private ContratosDAO contratosDAO; // Objeto DAO para manipular os contratos
        private Contrato contratoExistente; // Contrato a ser editado (se for o ca
        private ClausulasDAO clausulasDAO = new ClausulasDAO();
        public int contratoId;
        private bool modoEdicao;
        private int clausulaSelecionadaId = 0; // ID da cláusula atualmente selecionada
        private bool Carregando = false;

        private void operEdContratos_Load(object sender, EventArgs e)
        {
            InicializarTela();
        }

        private void InicializarTela()
        {
            Carregando = true;
            if (modoEdicao && contratoExistente != null) // Edição de contrato existente
            {
                contratoId = contratoExistente.Id;
                txtDescricao.Text = contratoExistente.Descricao;
                txtValor.Text = contratoExistente.Valor.ToString("C");
                cmbStatus.SelectedItem = contratoExistente.Status;
                dtpInicio.Value = contratoExistente.DataInicio;
                dtpFim.Value = contratoExistente.DataTermino;
                txtObservacoes.Text = contratoExistente.Observacoes;

                // Carregar as cláusulas relacionadas ao contrato
                CarregarClausulas();
            }
            else // Adição de novo contrato
            {
                contratoId = contratosDAO.GetNextContratoId(); // Gera um novo ID temporário
                txtDescricao.Clear();
                txtValor.Clear();
                cmbStatus.SelectedIndex = 0; // Valor padrão, como "Ativo"
                dtpInicio.Value = DateTime.Today;
                dtpFim.Value = DateTime.Today;
                txtObservacoes.Clear();
                lstClausulas.DataSource = null;                
            }
            lblContratoId.Text = "Contrato: " + contratoId.ToString();
            Carregando = false;
        }

        public operEdContratos(int nrContrato)
        {
            InitializeComponent();
            contratosDAO = new ContratosDAO();
            clausulasDAO = new ClausulasDAO();

            // Carrega os motoboys no ComboBox
            CarregarMotoboys();

            if (nrContrato > 0) // Edição de contrato
            {
                modoEdicao = true;
                contratoExistente = contratosDAO.GetContratoById(nrContrato);

                if (contratoExistente != null)
                {
                    // Seleciona o motoboy no ComboBox
                    cmbMotoboy.SelectedValue = contratoExistente.IdEntregador;
                    cmbMotoboy.Enabled = false; // Desativa edição do Motoboy
                    btnNovoMotoboy.Enabled = false;
                }
            }
            else // Novo contrato
            {
                modoEdicao = false;
                contratoExistente = null;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CarregarClausulas()
        {
            if (contratoId > 0)
            {
                var clausulas = clausulasDAO.GetClausulasByContratoId(contratoId);
                lstClausulas.DataSource = clausulas;
                lstClausulas.DisplayMember = "Descricao";
                lstClausulas.ValueMember = "Id";
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                if (modoEdicao)
                {
                    // Atualiza os dados do contrato
                    contratosDAO.UpdateContrato(
                        contratoId,
                        txtDescricao.Text,
                        decimal.Parse(txtValor.Text),
                        cmbStatus.SelectedItem.ToString(),
                        dtpInicio.Value,
                        dtpFim.Value,
                        "PIX", // Placeholder para PIX
                        txtObservacoes.Text
                    );
                    // MessageBox.Show("Contrato atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Insere um novo contrato
                    contratosDAO.InsertContrato(
                        txtDescricao.Text,
                        0, // Placeholder para idEntregador
                        decimal.Parse(txtValor.Text),
                        cmbStatus.SelectedItem.ToString(),
                        dtpInicio.Value,
                        dtpFim.Value,
                        "PIX", // Placeholder para PIX
                        txtObservacoes.Text
                    );
                    // MessageBox.Show("Contrato criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Fecha a tela após salvar
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidarCampos()
        {
                // Verificar se o campo Descrição está preenchido
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                MessageBox.Show("A descrição do contrato é obrigatória.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescricao.Focus();
                return false;
            }

            // Validar o valor            
            float fValor = glo.LeValor(txtValor.Text);
            if (fValor <= 0.0)
            {
                MessageBox.Show("O valor do contrato deve ser maior que zero.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtValor.Focus();
                return false;
            }

            // Verificar se as datas de início e término são válidas
            if (dtpInicio.Value >= dtpFim.Value)
            {
                MessageBox.Show("A data de término deve ser posterior à data de início.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpInicio.Focus();
                return false;
            }

            // Verificar se há pelo menos uma cláusula adicionada
            if (lstClausulas.Items.Count == 0)
            {
                MessageBox.Show("Adicione pelo menos uma cláusula ao contrato.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEditarAdicionar.Focus();
                return false;
            }

            return true; // Todas as validações passaram

        }

        private void btnAdicionarClausula_Click(object sender, EventArgs e)
        {
            string novaClausula = txtEditarAdicionar.Text.Trim();
            if (!string.IsNullOrWhiteSpace(novaClausula))
            {
                ClausulasDAO clausulasDAO = new ClausulasDAO();
                clausulasDAO.AdicionarClausula(contratoId, novaClausula);

                // Atualiza a lista de cláusulas na tela
                AtualizarListaClausulas();
                txtEditarAdicionar.Clear();
            }
            else
            {
                MessageBox.Show("Por favor, insira o texto da cláusula.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AtualizarListaClausulas()
        {
            if (contratoId > 0)
            {
                var clausulas = clausulasDAO.GetClausulasByContratoId(contratoId);
                lstClausulas.DataSource = clausulas;
                lstClausulas.DisplayMember = "Descricao";
                lstClausulas.ValueMember = "ID";
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            GravarContrato();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Verificar se há uma cláusula selecionada
            if (clausulaSelecionadaId == 0)
            {
                MessageBox.Show("Selecione uma cláusula para editar antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar se o campo de edição não está vazio
            if (string.IsNullOrWhiteSpace(txtEditarAdicionar.Text))
            {
                MessageBox.Show("Informe uma nova descrição para a cláusula.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Atualizar a cláusula no banco de dados
            clausulasDAO.UpdateClausula(clausulaSelecionadaId, txtEditarAdicionar.Text.Trim());

            // Recarregar a lista de cláusulas
            AtualizarListaClausulas();

            // Limpar o ID da cláusula selecionada e o campo de edição
            clausulaSelecionadaId = 0;
            txtEditarAdicionar.Clear();
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (lstClausulas.SelectedItem != null)
            {
                int clausulaId = Convert.ToInt32(lstClausulas.SelectedValue);
                clausulasDAO.DeleteClausula(clausulaId);
                AtualizarListaClausulas();
            }
            else
            {
                MessageBox.Show("Selecione uma cláusula para remover.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private bool ValidarContrato()
        //{
        //    // Validação: Verificar se a descrição está preenchida
        //    if (string.IsNullOrWhiteSpace(txtDescricao.Text))
        //    {
        //        MessageBox.Show("A descrição do contrato é obrigatória.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        txtDescricao.Focus();
        //        return false;
        //    }

        //    // Validação: Verificar se o valor é válido
        //    if (!decimal.TryParse(txtValor.Text, out decimal valor) || valor <= 0)
        //    {
        //        MessageBox.Show("O valor do contrato deve ser maior que zero.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        txtValor.Focus();
        //        return false;
        //    }

        //    // Validação: Verificar se as datas são consistentes
        //    if (dtpInicio.Value >= dtpFim.Value)
        //    {
        //        MessageBox.Show("A data de início deve ser anterior à data de término.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        dtpInicio.Focus();
        //        return false;
        //    }

        //    // Validação: Verificar se há observações
        //    //if (string.IsNullOrWhiteSpace(txtObservacoes.Text))
        //    //{
        //    //    MessageBox.Show("As observações são obrigatórias.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //    txtObservacoes.Focus();
        //    //    return false;
        //    //}

        //    // Validação: Verificar se há cláusulas no contrato
        //    if (lstClausulas.Items.Count == 0)
        //    {
        //        MessageBox.Show("O contrato deve ter pelo menos uma cláusula.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    return true;
        //}

        private void GravarContrato()
        {
            // Validações
            if (!ValidarCampos())
            {
                return; // Sai do método se as validações falharem
            }
            ContratosDAO contratosDAO = new ContratosDAO();
            if (modoEdicao)
            {
                float fValor = glo.LeValor(txtValor.Text);
                contratosDAO.UpdateContrato(
                    contratoExistente.Id,
                    txtDescricao.Text.Trim(),
                    (decimal)fValor,
                    cmbStatus.SelectedItem.ToString(),
                    dtpInicio.Value,
                    dtpFim.Value,
                    null, // PIX pode ser adicionado futuramente
                    txtObservacoes.Text.Trim()
                );
                AtualizarClausulas(contratoExistente.Id);
            }
            else
            {
                int novoContratoId = contratosDAO.InsertContrato(
                    txtDescricao.Text.Trim(),
                    (int)cmbMotoboy.SelectedValue,
                    decimal.Parse(txtValor.Text),
                    cmbStatus.SelectedItem.ToString(),
                    dtpInicio.Value,
                    dtpFim.Value,
                    null, // PIX pode ser adicionado futuramente
                    txtObservacoes.Text.Trim()
                );

                // Salvar as cláusulas do novo contrato
                AtualizarClausulas(novoContratoId);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AtualizarClausulas(int contratoId)
        {
            // Instância do DAO para manipular cláusulas
            clausulasDAO.RemoverClausulasPorContrato(contratoId);

            // Adiciona as cláusulas atuais da lista
            foreach (DataRowView item in lstClausulas.Items) // Alterado para DataRowView
            {
                string clausulaTexto = item["Descricao"].ToString(); // Acessa a propriedade correta
                clausulasDAO.AdicionarClausula(contratoId, clausulaTexto);
            }

        }

        private void CarregarMotoboys()
        {
            // Instancia o DAO de Entregadores
            EntregadorDAO entregadorDAO = new EntregadorDAO();

            // Obtém os dados dos entregadores no formato DataTable
            DataTable motoboys = entregadorDAO.getDados();

            if (motoboys.Rows.Count > 0)
            {
                // Preenche o ComboBox com os dados dos motoboys
                cmbMotoboy.DataSource = motoboys;
                cmbMotoboy.DisplayMember = "Nome"; // Nome do motoboy exibido no ComboBox
                cmbMotoboy.ValueMember = "id";    // Valor associado ao item (ID do motoboy)
            }
            else
            {
                cmbMotoboy.DataSource = null;
            }
        }

        private void btnNovoMotoboy_Click(object sender, EventArgs e)
        {
            fCadEntregadores CadEntregadores = new fCadEntregadores();
            CadEntregadores.Adicao();
            if (CadEntregadores.ShowDialog() == DialogResult.OK)
            {
                // Atualiza o ComboBox com o novo Motoboy
                CarregarMotoboys();

                // Seleciona o novo Motoboy automaticamente
                cmbMotoboy.SelectedValue = glo.IdAdicionado;
            }
        }

        private void lstClausulas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.Carregando)
            {
                if (lstClausulas.SelectedItem != null)
                {
                    DataRowView clausulaSelecionada = lstClausulas.SelectedItem as DataRowView;
                    if (clausulaSelecionada != null)
                    {
                        clausulaSelecionadaId = Convert.ToInt32(clausulaSelecionada[0]); // Armazena o ID da cláusula
                        txtEditarAdicionar.Text = clausulaSelecionada["Descricao"].ToString(); // Mostra a descrição no campo de edição
                    }
                }
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            tb.Contrato contrato = contratosDAO.GetContratoCompleto(contratoId);
            rel.ContratoPrinter printer = new rel.ContratoPrinter(
                contrato.Contratante,
                contrato.ContratanteCNPJ,
                contrato.ContratanteEndereco,
                contrato.Contratada,
                contrato.ContratadaCNPJ,
                contrato.ContratadaEndereco,
                contrato.NomeEmpresa, 
                contrato.CNPJEmpresa, 
                contrato.Valor,
                contrato.Descricao,
                contrato.Clausulas.ToArray() 
            );
            printer.Imprimir();
        }

        public void UpdateClausula(int clausulaId, string descricao)
        {
            string query = $@"UPDATE Clausulas 
                              SET Descricao = '{descricao.Replace("'", "''")}' 
                              WHERE ID = {clausulaId}";
            DB.ExecutarComandoSQL(query);
        }

    }

}
