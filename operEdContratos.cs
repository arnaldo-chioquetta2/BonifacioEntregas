using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class operEdContratos : Form
    {
        public int contratoId;
        private ContratosDAO contratosDAO; 
        private Contrato contratoExistente; 
        private ClausulasDAO clausulasDAO = new ClausulasDAO();        
        private bool modoEdicao;        
        private int clausulaSelecionadaId = 0; 
        private bool Carregando = false;
        private int idTipoContrato = -1;
        private int idCadastroAssociado = 0;

        private void operEdContratos_Load(object sender, EventArgs e)
        {
            InicializarTela();
            InicializarComboTiposContrato();
            if (modoEdicao && contratoExistente != null)
            {
                CarregarContratoParaEdicao(contratoExistente);
            }
        }

        private void CarregarContratoParaEdicao(Contrato contrato)
        {
            if (!Carregando)
            {
                Carregando = true;

                // Preencher os campos do contrato
                txtDescricao.Text = contrato.Descricao;
                txtValor.Text = contrato.Valor.ToString("N2");
                cmbStatus.SelectedItem = contrato.Status;
                dtpInicio.Value = contrato.DataInicio;
                dtpFim.Value = contrato.DataTermino;
                txtObservacoes.Text = contrato.Observacoes;

                // Verificar se há um nome de empresa ou nome associado ao contrato
                string nomePessoa = contrato.NomeEmpresa;

                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(int)); // Coluna para ID
                dt.Columns.Add("Nome", typeof(string)); // Coluna para Nome

                // Adiciona o registro com o ID fixo e o nome
                dt.Rows.Add(1, nomePessoa);

                // Define o DataSource do ComboBox
                cmbMotoboy.DataSource = dt;
                cmbMotoboy.DisplayMember = "Nome"; // Nome visível no ComboBox
                cmbMotoboy.ValueMember = "id"; // Valor associado ao ComboBox
                cmbMotoboy.SelectedValue = 1; 

                // Selecionar o tipo de contrato no combo
                cmbTipoContrato.SelectedValue = contrato.Id;

                // Habilitar os campos
                ToggleCampos(true);

                Carregando = false;
            }
        }

        public operEdContratos(int nrContrato)
        {
            InitializeComponent();
            contratosDAO = new ContratosDAO();
            clausulasDAO = new ClausulasDAO();

            // Carrega os motoboys no ComboBox
            //CarregarMotoboys();

            if (nrContrato > 0) // Edição de contrato
            {
                modoEdicao = true;
                contratoExistente = contratosDAO.GetContratoById(nrContrato);
                idTipoContrato = contratoExistente.Id;
                CarregarDadosAssociados(idTipoContrato);

                if (contratoExistente != null)
                {
                    // Seleciona o motoboy no ComboBox
                    cmbMotoboy.SelectedValue = contratoExistente.IdEntregador;
                    cmbMotoboy.Enabled = false; // Desativa edição do Motoboy
                    //btnNovoMotoboy.Enabled = false;
                }
            }
            else // Novo contrato
            {
                modoEdicao = false;
                contratoExistente = null;
            }
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
                    float fValor = glo.LeValor(txtValor.Text);
                    contratosDAO.UpdateContrato(
                        contratoExistente.Id,
                        txtDescricao.Text.Trim(),
                        (decimal)fValor,
                        cmbStatus.SelectedItem.ToString(),
                        dtpInicio.Value,
                        dtpFim.Value,
                        null, // PIX pode ser adicionado futuramente
                        txtObservacoes.Text.Trim(),
                        cmbMotoboy.Text // Inclui o texto do ComboBox como NomeEmpresa
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
            this.Carregando = true;
            AtualizarListaClausulas();

            // Limpar o ID da cláusula selecionada e o campo de edição
            clausulaSelecionadaId = 0;
            txtEditarAdicionar.Clear();
            this.Carregando = false;
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (lstClausulas.SelectedItem != null)
            {
                clausulasDAO.DeleteClausula(clausulaSelecionadaId);
                this.Carregando = true;
                AtualizarListaClausulas();
                txtEditarAdicionar.Text = "";
                this.Carregando = false;
            }
            else
            {
                MessageBox.Show("Selecione uma cláusula para remover.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }       

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
                    txtObservacoes.Text.Trim(),
                    cmbMotoboy.Text // Inclui o texto do ComboBox como NomeEmpresa
                );


                //contratosDAO.UpdateContrato(
                //    contratoExistente.Id,
                //    txtDescricao.Text.Trim(),
                //    (decimal)fValor,
                //    cmbStatus.SelectedItem.ToString(),
                //    dtpInicio.Value,
                //    dtpFim.Value,
                //    null, // PIX pode ser adicionado futuramente
                //    txtObservacoes.Text.Trim()
                //);
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
            foreach (var item in lstClausulas.Items)
            {
                if (item is DataRowView rowView && rowView.Row.Table.Columns.Contains("Texto")) // Verifica a existência da coluna
                {
                    string clausulaTexto = rowView["Texto"].ToString(); // Acessa a propriedade correta
                    clausulasDAO.AdicionarClausula(contratoId, clausulaTexto);
                }
                else
                {
                    // Tratar o caso onde a coluna não existe
                    MessageBox.Show("Erro: Coluna 'Texto' não encontrada na DataTable associada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                        txtEditarAdicionar.Text = clausulaSelecionada[2].ToString(); // Mostra a descrição no campo de edição
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
                contrato.Clausulas.ToArray(),
                contrato.DataInicio,
                contrato.DataTermino, 
                contrato.Observacoes
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

        private void InicializarComboTiposContrato()
        {
            Carregando = true;
            ContratosDAO contratosDAO = new ContratosDAO();
            DataTable tiposContrato = contratosDAO.GetTiposDeContrato();

            // Adiciona "Escolha o contrato" como item inicial
            DataRow linhaInicial = tiposContrato.NewRow();
            linhaInicial["Nome"] = "Escolha o tipo de contrato";
            linhaInicial["IdTipoContrato"] = 0;
            tiposContrato.Rows.InsertAt(linhaInicial, 0);

            cmbTipoContrato.DataSource = tiposContrato;
            cmbTipoContrato.DisplayMember = "Nome";
            cmbTipoContrato.ValueMember = "IdTipoContrato";
            cmbTipoContrato.SelectedIndex = 0;

            // Desabilita os campos até que um tipo seja selecionado
            ToggleCampos(false);
            Carregando = false;
        }

        private void ToggleCampos(bool habilitar)
        {
            txtDescricao.Enabled = habilitar;
            txtValor.Enabled = habilitar;
            cmbStatus.Enabled = habilitar;
            dtpInicio.Enabled = habilitar;
            dtpFim.Enabled = habilitar;
            txtObservacoes.Enabled = habilitar;
            lstClausulas.Enabled = habilitar;
            btnAddClausula.Enabled = habilitar;
            btnEditClausula.Enabled = habilitar;
            btnRemoveClausula.Enabled = habilitar;
            txtEditarAdicionar.Enabled = habilitar;
            btnSalvar.Enabled = habilitar;
            btnImprimir.Enabled = habilitar;
        }

        private void cmbTipoContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Carregando)
            {
                if (cmbTipoContrato.SelectedIndex > 0) // Um tipo de contrato válido foi selecionado
                {
                    Carregando = true;

                    // Obtém o texto do tipo de contrato selecionado
                    string tipoContratoTexto = cmbTipoContrato.Text;

                    // Busca o ID do tipo de contrato com base no texto selecionado
                    idCadastroAssociado = contratosDAO.ObterIdCadastroAssociado(tipoContratoTexto);

                    if (idCadastroAssociado > 0) // ID encontrado
                    {
                        // Carrega os dados associados ao tipo de contrato
                        CarregarDadosAssociados(idCadastroAssociado);

                        // Carrega as cláusulas associadas ao tipo de contrato
                        DataTable clausulas = clausulasDAO.GetClausulasByTipoContrato(contratosDAO.IdTipoContrato);

                        lstClausulas.DataSource = clausulas;
                        lstClausulas.DisplayMember = "Texto"; // Coluna para exibir no DataGridView
                        lstClausulas.ValueMember = "Ordem";  // Coluna que identifica os registros

                        // Habilita os campos para edição
                        ToggleCampos(true);
                    }
                    else
                    {
                        MessageBox.Show("Erro ao encontrar o tipo de contrato selecionado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    Carregando = false;
                }
                else
                {
                    // Caso "Escolha o contrato" seja selecionado, desabilita os campos
                    ToggleCampos(false);
                    lstClausulas.DataSource = null;
                }
            }
        }

        private void CarregarDadosAssociados(int cadastroAssociado)
        {
            switch (cadastroAssociado)
            {
                case 1:
                    CarregarFuncion();
                    break;
                case 2: // Clientes
                    CarregarClientes();
                    break;
                case 3: // Entregadores
                    CarregarMotoboys();
                    break;
                case 4: // Fornecedores
                    CarregarFornecedores();
                    break;
                default:
                    cmbMotoboy.DataSource = null;
                    return;
            }
        }

        #region Carregantos

        private void CarregarMotoboys()
        {
            EntregadorDAO entregadorDAO = new EntregadorDAO();
            DataTable motoboys = entregadorDAO.getDados();
            if (motoboys.Rows.Count > 0)
            {
                cmbMotoboy.DataSource = motoboys;
                cmbMotoboy.DisplayMember = "Nome";
                cmbMotoboy.ValueMember = "id";
            }
            else
            {
                cmbMotoboy.DataSource = null;
            }
        }

        private void CarregarFornecedores()
        {
            FornecedorDao entregadorDAO = new FornecedorDao();
            DataTable motoboys = entregadorDAO.getDados();
            if (motoboys.Rows.Count > 0)
            {
                cmbMotoboy.DataSource = motoboys;
                cmbMotoboy.DisplayMember = "Nome";
                cmbMotoboy.ValueMember = "IdForn";
            }
            else
            {
                cmbMotoboy.DataSource = null;
            }
        }

        private void CarregarFuncion()
        {
            VendedoresDAO entregadorDAO = new VendedoresDAO();
            DataTable motoboys = entregadorDAO.getDados();
            if (motoboys.Rows.Count > 0)
            {
                cmbMotoboy.DataSource = motoboys;
                cmbMotoboy.DisplayMember = "Nome";
                cmbMotoboy.ValueMember = "id";
            }
            else
            {
                cmbMotoboy.DataSource = null;
            }
        }
        
        private void CarregarClientes()
        {
            ClienteDAO entregadorDAO = new ClienteDAO();
            DataTable motoboys = entregadorDAO.getDados();
            if (motoboys.Rows.Count > 0)
            {
                cmbMotoboy.DataSource = motoboys;
                cmbMotoboy.DisplayMember = "Nome";
                cmbMotoboy.ValueMember = "id";
            }
            else
            {
                cmbMotoboy.DataSource = null;
            }
        }

        #endregion

    }

}
