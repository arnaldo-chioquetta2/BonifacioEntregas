using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class operPecas : Form
    {

        private PecasDAO pecasDao;
        private bool carregando = false;
        private bool carregandoCarro=false;
        private bool CarregandoPecas = false;

        // Variável para armazenar o item que está sendo editado
        private int idItemEditando = -1;

        // Para editar itens na lista de carros
        private void LstCarros_DoubleClick(object sender, EventArgs e)
        {
            if (lstCarros.SelectedItem != null)
            {
                DataRowView carroSelecionado = lstCarros.SelectedItem as DataRowView;
                txtCarro.Text = carroSelecionado["Nome"].ToString();
                idItemEditando = Convert.ToInt32(carroSelecionado["IdCarro"]);
                btnAddCarro.Text = "✓"; // Alterar o botão para "Salvar"
            }
        }

        // Para editar itens na lista de peças
        private void LstPecas_DoubleClick(object sender, EventArgs e)
        {
            if (lstPecas.SelectedItem != null)
            {
                DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                txtPeca.Text = pecaSelecionada["Nome"].ToString();
                idItemEditando = Convert.ToInt32(pecaSelecionada["IdPeca"]);
                btnAddPeca.Text = "✓"; // Alterar o botão para "Salvar"
            }
        }

        // Para editar itens na lista de características
        private void LstCaracteristicas_DoubleClick(object sender, EventArgs e)
        {
            if (lstCaracteristicas.SelectedItem != null)
            {
                DataRowView caracteristicaSelecionada = lstCaracteristicas.SelectedItem as DataRowView;
                txtCaracteristica.Text = caracteristicaSelecionada["Descricao"].ToString();
                idItemEditando = Convert.ToInt32(caracteristicaSelecionada["IdCaracteristica"]);
                btnAddCaracteristica.Text = "✓"; // Alterar o botão para "Salvar"
            }
        }


        public operPecas()
        {
            InitializeComponent();
            pecasDao = new PecasDAO();
            this.cmbFiltro.Items.AddRange(new string[] { "Todos Tipos", "Carros", "Peças", "Características" });
            this.cmbFiltro.SelectedIndex = 0;
        }

        private void btnAddCarro_Click(object sender, EventArgs e)
        {
            string novoCarro = txtCarro.Text.Trim();
            if (!string.IsNullOrWhiteSpace(novoCarro))
            {
                if (btnAddCarro.Text == "✓" && idItemEditando != -1)
                {
                    // Modo de edição
                    pecasDao.UpdateCarro(idItemEditando, novoCarro);

                    // Atualizar diretamente o item no ListBox
                    AtualizarItemNaLista(lstCarros, idItemEditando, novoCarro, "IdCarro", "Nome");

                    btnAddCarro.Text = "+"; // Voltar ao modo de adição
                    idItemEditando = -1;
                }
                else
                {
                    // Modo de adição
                    pecasDao.InsertCarro(novoCarro);
                    AtualizarListaCarros(); // Somente no caso de adição
                }
                txtCarro.Clear();
            }
        }

        private void AtualizarItemNaLista(ListBox listBox, int id, string novoTexto, string colunaId, string colunaTexto)
        {
            if (listBox.DataSource is DataTable dataTable)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (Convert.ToInt32(row[colunaId]) == id)
                    {
                        row[colunaTexto] = novoTexto; // Atualiza o texto no DataRow
                        break;
                    }
                }

                // Forçar atualização do ListBox
                listBox.Refresh();
            }
        }


        private void btnAddPeca_Click(object sender, EventArgs e)
        {
            string novaPeca = txtPeca.Text.Trim();
            if (!string.IsNullOrWhiteSpace(novaPeca) && lstCarros.SelectedItem != null)
            {
                if (btnAddPeca.Text == "✓" && idItemEditando != -1)
                {
                    // Modo de edição
                    pecasDao.UpdatePeca(idItemEditando, novaPeca);

                    // Atualizar diretamente o item no ListBox
                    AtualizarItemNaLista(lstPecas, idItemEditando, novaPeca, "IdPeca", "Nome");

                    btnAddPeca.Text = "+"; // Voltar ao modo de adição
                    idItemEditando = -1;
                }
                else
                {
                    // Modo de adição
                    DataRowView carroSelecionado = lstCarros.SelectedItem as DataRowView;
                    int idCarro = Convert.ToInt32(carroSelecionado["IdCarro"]);
                    pecasDao.InsertPeca(novaPeca, idCarro);
                    AtualizarListaPecas(idCarro); // Somente no caso de adição
                }
                txtPeca.Clear();
            }
        }

        private void btnAddCaracteristica_Click(object sender, EventArgs e)
        {
            string novaCaracteristica = txtCaracteristica.Text.Trim();
            if (!string.IsNullOrWhiteSpace(novaCaracteristica) && lstPecas.SelectedItem != null)
            {
                if (btnAddCaracteristica.Text == "✓" && idItemEditando != -1)
                {
                    // Modo de edição
                    pecasDao.UpdateCaracteristica(idItemEditando, novaCaracteristica);

                    // Atualizar diretamente o item no ListBox
                    AtualizarItemNaLista(lstCaracteristicas, idItemEditando, novaCaracteristica, "IdCaracteristica", "Descricao");

                    btnAddCaracteristica.Text = "+"; // Voltar ao modo de adição
                    idItemEditando = -1;
                }
                else
                {
                    // Modo de adição
                    DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                    int idPeca = Convert.ToInt32(pecaSelecionada["IdPeca"]);
                    pecasDao.InsertCaracteristica(novaCaracteristica, idPeca);
                    AtualizarListaCaracteristicas(idPeca); // Somente no caso de adição
                }
                txtCaracteristica.Clear();
            }
        }


        private void AtualizarListaCarros()
        {
            // Consulta todos os carros na tabela
            DataTable carros = pecasDao.GetAllCarros();

            if (carros != null && carros.Rows.Count > 0)
            {
                lstCarros.DataSource = null; // Limpa qualquer vínculo anterior
                lstCarros.Items.Clear();     // Garante que a lista seja reiniciada

                lstCarros.DataSource = carros;
                lstCarros.DisplayMember = "Nome"; // Nome do carro a ser exibido
                lstCarros.ValueMember = "IdCarro"; // Valor associado ao item
            }
            else
            {
                // Caso não haja carros, limpa a lista
                lstCarros.DataSource = null;
                lstCarros.Items.Clear();
            }
        }


        private void AtualizarListaCaracteristicas(int idPeca)
        {
            string query;

            if (idPeca == 0)
            {
                // Carrega todas as características
                query = "SELECT * FROM Caracteristicas";
            }
            else
            {
                // Carrega as características relacionadas à peça específica
                query = "SELECT * FROM Caracteristicas WHERE IdPeca = @idPeca";
            }

            List<OleDbParameter> parametros = new List<OleDbParameter>();
            if (idPeca != 0)
            {
                parametros.Add(new OleDbParameter("@idPeca", idPeca));
            }

            // Executa a consulta
            DataTable caracteristicas = DB.ExecutarConsulta(query, parametros);

            if (caracteristicas.Rows.Count > 0)
            {
                lstCaracteristicas.DataSource = caracteristicas;
                lstCaracteristicas.DisplayMember = "Descricao";
                lstCaracteristicas.ValueMember = "IdCaracteristica";
            }
            else
            {
                lstCaracteristicas.DataSource = null; // Nenhuma característica encontrada
            }
        }


        private void operPecas_Load(object sender, EventArgs e)
        {
            Inicializar();
            txtPesquisa.Focus();
        }

        private void Inicializar()
        {
            carregando = true;
            AtualizarListaCarros();
            AtualizarListaPecas(0);
            AtualizarListaCaracteristicas(0);

            // Limpar seleção inicial das listas
            lstCarros.ClearSelected();
            lstPecas.ClearSelected();
            lstCaracteristicas.ClearSelected();

            // Definir o foco no campo de pesquisa
            txtPesquisa.Focus();
            carregando = false;
        }

        private void AtualizarListaPecas(int idCarro)
        {
            DataTable pecas = pecasDao.GetPecasByCarroId(idCarro);
            if (pecas.Rows.Count > 0)
            {
                lstPecas.DataSource = pecas;
                lstPecas.DisplayMember = "Nome";
                lstPecas.ValueMember = "IdPeca";
            }
            else
            {
                lstPecas.DataSource = null; // Nenhuma peça para o carro selecionado
            }
        }

        private void lstCarros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                if (!CarregandoPecas)
                {
                    carregandoCarro = true;

                    if (lstCarros.SelectedItem != null)
                    {
                        int idCarro = 0;
                        DataRowView carroSelecionado = lstCarros.SelectedItem as DataRowView;

                        if (carroSelecionado != null)
                        {
                            // Caso seja um DataRowView, pega o ID diretamente
                            idCarro = Convert.ToInt32(carroSelecionado["IdCarro"]);
                        }
                        else
                        {
                            // Caso seja uma string, faz a busca no banco de dados
                            string nomeCarro = lstCarros.SelectedItem.ToString();
                            DataTable carros = pecasDao.GetAllCarros();

                            foreach (DataRow row in carros.Rows)
                            {
                                if (row["Nome"].ToString() == nomeCarro)
                                {
                                    idCarro = Convert.ToInt32(row["IdCarro"]);
                                    break;
                                }
                            }
                        }

                        if (idCarro > 0)
                        {
                            // Atualizar lista de peças relacionadas ao carro
                            AtualizarListaPecas(idCarro);

                            // Verificar se há peças relacionadas
                            if (lstPecas.Items.Count > 0)
                            {
                                // Selecionar a primeira peça automaticamente
                                lstPecas.SelectedIndex = 0;

                                // Obter a primeira peça e carregar as características
                                DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                                if (pecaSelecionada != null)
                                {
                                    int idPeca = Convert.ToInt32(pecaSelecionada["IdPeca"]);
                                    AtualizarListaCaracteristicas(idPeca);
                                }
                            }
                            else
                            {
                                // Limpar características se não houver peças
                                lstCaracteristicas.DataSource = null;
                            }
                        }
                    }

                    carregandoCarro = false;
                }
            }
        }

        private void lstPecas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                if (lstPecas.SelectedItem != null)
                {
                    DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                    int idPeca = Convert.ToInt32(pecaSelecionada["IdPeca"]);

                    AtualizarListaCodigos(idPeca); // Atualiza os códigos da peça
                    AtualizarListaCaracteristicas(idPeca); // Atualiza as características da peça
                }
            }
        }

        private void AtualizarListaCarrosPorPeca(int idPeca)
        {
            string query = "SELECT c.IdCarro, c.Nome FROM Carros c " +
                           "INNER JOIN Pecas p ON c.IdCarro = p.IdCarro " +
                           "WHERE p.IdPeca = @idPeca";
            List<OleDbParameter> parametros = new List<OleDbParameter>
    {
        new OleDbParameter("@idPeca", idPeca)
    };

            DataTable carros = DB.ExecutarConsulta(query, parametros);

            if (carros.Rows.Count > 0)
            {
                lstCarros.DataSource = carros;
                lstCarros.DisplayMember = "Nome";
                lstCarros.ValueMember = "IdCarro";
            }
            else
            {
                lstCarros.DataSource = null; // Caso nenhum carro esteja vinculado
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            string termoPesquisa = txtPesquisa.Text.Trim();
            string filtro = cmbFiltro.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(termoPesquisa))
            {
                MessageBox.Show("Digite um termo para pesquisar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AtualizarListBoxesComPesquisa(termoPesquisa, filtro);
        }

        private void AtualizarListBoxesComPesquisa(string termoPesquisa, string filtro)
        {
            // Limpar os ListBox antes de adicionar os resultados
            lstCarros.DataSource = null;
            lstCarros.Items.Clear();
            lstPecas.DataSource = null;
            lstPecas.Items.Clear();
            lstCaracteristicas.DataSource = null;
            lstCaracteristicas.Items.Clear();

            string termoLike = $"%{termoPesquisa}%";

            // Pesquisa em Carros
            if (filtro == "Todos Tipos" || filtro == "Carros")
            {
                string queryCarros = "SELECT Nome FROM Carros WHERE Nome LIKE @termo";
                List<OleDbParameter> parametrosCarros = new List<OleDbParameter>
        {
            new OleDbParameter("@termo", termoLike)
        };
                DataTable carros = DB.ExecutarConsulta(queryCarros, parametrosCarros);
                foreach (DataRow row in carros.Rows)
                {
                    lstCarros.Items.Add(row["Nome"].ToString());
                }
            }

            // Pesquisa em Peças
            if (filtro == "Todos Tipos" || filtro == "Peças")
            {
                string queryPecas = "SELECT Nome FROM Pecas WHERE Nome LIKE @termo";
                List<OleDbParameter> parametrosPecas = new List<OleDbParameter>
        {
            new OleDbParameter("@termo", termoLike)
        };
                DataTable pecas = DB.ExecutarConsulta(queryPecas, parametrosPecas);
                foreach (DataRow row in pecas.Rows)
                {
                    lstPecas.Items.Add(row["Nome"].ToString());
                }
            }

            // Pesquisa em Características
            if (filtro == "Todos Tipos" || filtro == "Características")
            {
                string queryCaracteristicas = "SELECT Descricao FROM Caracteristicas WHERE Descricao LIKE @termo";
                List<OleDbParameter> parametrosCaracteristicas = new List<OleDbParameter>
        {
            new OleDbParameter("@termo", termoLike)
        };
                DataTable caracteristicas = DB.ExecutarConsulta(queryCaracteristicas, parametrosCaracteristicas);
                foreach (DataRow row in caracteristicas.Rows)
                {
                    lstCaracteristicas.Items.Add(row["Descricao"].ToString());
                }
            }
        }

        private void lstCarros_DataSourceChanged(object sender, EventArgs e)
        {
            Console.WriteLine("O DataSource do lstCarros foi alterado.");
            if (lstCarros.DataSource != null)
            {
                DataTable carros = lstCarros.DataSource as DataTable;
                if (carros != null)
                {
                    Console.WriteLine("Dados no DataSource:");
                    foreach (DataRow row in carros.Rows)
                    {
                        Console.WriteLine($"IdCarro: {row["IdCarro"]}, Nome: {row["Nome"]}");
                    }
                }
            }
            else
            {
                Console.WriteLine("O DataSource foi configurado como null.");
            }
        }

        private void txtPesquisa_Click(object sender, EventArgs e)
        {
            Inicializar();
        }

        private void AtualizarListaCodigos(int idPeca)
        {
            DataTable codigos = pecasDao.GetCodigosByPecaId(idPeca);
            if (codigos.Rows.Count > 0)
            {
                lstCodigos.DataSource = codigos;
                lstCodigos.DisplayMember = "Codigo";
                lstCodigos.ValueMember = "IdCodigo";
            }
            else
            {
                lstCodigos.DataSource = null; // Nenhum código para a peça selecionada
            }
        }

        private void lstCodigos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void btnAddCodigo_Click(object sender, EventArgs e)
        {
            string novoCodigo = txtCodigo.Text.Trim();
            if (!string.IsNullOrWhiteSpace(novoCodigo) && lstPecas.SelectedItem != null)
            {
                DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                int idPeca = Convert.ToInt32(pecaSelecionada["IdPeca"]);
                pecasDao.InsertCodigo(idPeca, novoCodigo);
                AtualizarListaCodigos(idPeca); // Atualiza a lista de códigos
                txtCodigo.Clear();
            }
        }

        private void btnAddCodigos_Click(object sender, EventArgs e)
        {
            string novoCodigo = txtCodigo.Text.Trim();
            if (!string.IsNullOrWhiteSpace(novoCodigo) && lstPecas.SelectedItem != null)
            {
                DataRowView pecaSelecionada = lstPecas.SelectedItem as DataRowView;
                int idPeca = Convert.ToInt32(pecaSelecionada["IdPeca"]);
                pecasDao.InsertCodigo(idPeca, novoCodigo);
                AtualizarListaCodigos(idPeca); // Atualiza a lista de códigos
                txtCodigo.Clear();
            }
        }
    }
}

