using System;
using System.Linq;
using System.Drawing;
using TeleBonifacio.tb;
using TeleBonifacio.dao;
using TeleBonifacio.gen;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Collections.Generic;

// 4.1.7 Alteração do funcionamento dos códigos de prateleira
// 4.1.5 Impressão em lista na prateleira
// 4.1.4 Tres colunas na impressão
// 4.1.2 Correção de bug na tela de códigos de prateleira
// 4.1.1 Tela de códigos de pratileira

namespace TeleBonifacio
{
    public partial class operPartilheira : Form
    {

        private bool _recarregando;
        private int _indiceImpressao;
        private CodigoPartilheiraDAO dao;
        private PrintDocument _printDocument;
        private PrintPreviewDialog _printPreview;
        private List<CodigoPartilheira> _listaParaImpressao;                
        private TipoImpressao _modoImpressao = TipoImpressao.Etiquetas;

        #region Inicialização

        public operPartilheira()
        {
            InitializeComponent();

        }

        private void operPartilheira_Load(object sender, EventArgs e)
        {
            // ===============================
            // CONFIGURAÇÃO DA TELA
            // ===============================

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 20;
            this.Top = 0;
            this.MinimumSize = new Size(1000, 700);
            this.Padding = new Padding(10);

            // ===============================
            // DAO
            // ===============================

            dao = new CodigoPartilheiraDAO();

            // ===============================
            // GRID - CONFIGURAÇÃO E NOVA COLUNA
            // ===============================

            gridCodigos.AutoGenerateColumns = false;

            // --- NOVA COLUNA DE SEQUÊNCIA (ENDEREÇO) ---
            DataGridViewTextBoxColumn colSequencia = new DataGridViewTextBoxColumn();
            colSequencia.Name = "colSequencia";
            colSequencia.HeaderText = "Nº";
            colSequencia.DataPropertyName = "Sequencia"; // Liga com a nova propriedade da classe
            colSequencia.Width = 70;
            colSequencia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colSequencia.ReadOnly = true;
            colSequencia.SortMode = DataGridViewColumnSortMode.NotSortable;
            colSequencia.DefaultCellStyle.ForeColor = Color.Blue; // Cor diferenciada para o Denis ver que é o endereço

            // Insere como a primeira coluna
            gridCodigos.Columns.Insert(0, colSequencia);
            // -------------------------------------------

            colCodigo.DataPropertyName = "Codigo";
            colCodigo.SortMode = DataGridViewColumnSortMode.NotSortable;

            gridCodigos.AllowUserToAddRows = false;
            gridCodigos.AllowUserToDeleteRows = false;
            gridCodigos.AllowUserToResizeRows = false;

            gridCodigos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridCodigos.MultiSelect = false;
            gridCodigos.RowHeadersVisible = false;

            gridCodigos.Anchor = AnchorStyles.Top
                                | AnchorStyles.Bottom
                                | AnchorStyles.Left
                                | AnchorStyles.Right;

            // ===============================
            // VISUAL GRANDE
            // ===============================

            AplicarModoVisualGrande();

            // ===============================
            // LAYOUT
            // ===============================

            AjustarLayoutVisual();

            // ===============================
            // IMPRESSÃO
            // ===============================

            _printDocument = new PrintDocument();
            // _printDocument.DefaultPageSettings.Landscape = true;
            _printDocument.PrintPage += PrintDocument_PrintPage;

            _printPreview = new PrintPreviewDialog();
            _printPreview.Document = _printDocument;

            // ===============================
            // DADOS
            // ===============================

            // O CarregarGrid() agora deve conter a lógica de looping 
            // para preencher a propriedade .Sequencia (1, 2, 3...)
            CarregarGrid();

            this.Resize += (s, ev) => AjustarLayoutVisual();

            _printDocument.BeginPrint += PrintDocument_BeginPrint;

            txCodigo.Focus();
        }

        private void PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            glo.Loga("PrintDocument_BeginPrint V1 INICIO");

            _indiceImpressao = 0;

            glo.Loga("PrintDocument_BeginPrint V1 indice resetado");
        }

        private void AplicarModoVisualGrande()
        {
            float tamanhoFonte = 16f;

            // GRID
            gridCodigos.DefaultCellStyle.Font = new Font("Segoe UI", tamanhoFonte);
            gridCodigos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", tamanhoFonte, FontStyle.Bold);
            gridCodigos.RowTemplate.Height = 38;
            gridCodigos.ColumnHeadersHeight = 45;

            // TextBox precisam de AutoSize false
            txCodigo.AutoSize = false;
            txBuscar.AutoSize = false;

            txCodigo.Height = 36;
            txBuscar.Height = 36;

            // Aplicar fonte geral
            foreach (Control c in this.Controls)
            {
                if (c is Label || c is Button || c is TextBox)
                {
                    c.Font = new Font("Segoe UI", tamanhoFonte);
                }
            }

            // Botões altura padronizada
            btAdicionar.Height = 42;
            btExcluir.Height = 42;
            btLimpar.Height = 42;
            btImprimir.Height = 42;
        }

        private void AjustarLayoutVisual()
        {
            int margem = 25;

            // ===============================
            // LINHA 1 - CÓDIGO
            // ===============================

            lblCodigo.Location = new Point(margem, 30);

            txCodigo.Location = new Point(150, 25);
            txCodigo.Width = 300;

            btAdicionar.Location = new Point(470, 23);
            btAdicionar.Width = 170;

            // ===============================
            // LINHA 2 - BUSCAR
            // ===============================

            lblBuscar.Location = new Point(margem, 90);

            txBuscar.Location = new Point(150, 85);
            txBuscar.Width = 490;

            // ===============================
            // GRID
            // ===============================

            int topoGrid = 150;
            int rodapeAltura = 80;

            gridCodigos.Location = new Point(margem, topoGrid);
            gridCodigos.Size = new Size(
                this.ClientSize.Width - (margem * 2),
                this.ClientSize.Height - topoGrid - rodapeAltura
            );

            // ===============================
            // BOTÕES INFERIORES
            // ===============================

            int yBotoes = this.ClientSize.Height - 60;

            // Padronização de Larguras
            btExcluir.Width = 150;
            btLimpar.Width = 180;
            btReiniciar.Width = 150; // Definindo largura para o novo botão
            btImprimir.Width = 150;
            btImprLista.Width = 150;

            // --- LADO ESQUERDO (Ações de Manutenção) ---

            // Botão Excluir
            btExcluir.Location = new Point(margem, yBotoes);
            btExcluir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // Botão Limpar (fica a 170px do início)
            btLimpar.Location = new Point(margem + 170, yBotoes);
            btLimpar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // Botão Reiniciar (Fica ao lado do Limpar, com um pequeno recuo)
            // Cálculo: margem(25) + 170 + largura do btLimpar(180) + gap(20) = 395 aprox.
            btReiniciar.Location = new Point(margem + 370, yBotoes);
            btReiniciar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btReiniciar.Height = btLimpar.Height; // Garante que a altura seja igual aos outros

            // --- LADO DIREITO (Ações de Saída) ---

            // Botão Imprimir (Extrema direita)
            btImprimir.Location = new Point(this.ClientSize.Width - 175, yBotoes);
            btImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Botão Imprimir Lista (Ao lado do Imprimir)
            btImprLista.Location = new Point(this.ClientSize.Width - 350, yBotoes);
            btImprLista.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btImprLista.Height = btImprimir.Height;
        }

        #endregion

        #region Impressão

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            glo.Loga("PrintDocument_PrintPage V4 INICIO");

            if (_modoImpressao == TipoImpressao.Etiquetas)
            {
                ImprimirEtiquetas(e);
            }
            else
            {
                ImprimirLista(e);
            }
        }

        private void ImprimirLista(PrintPageEventArgs e)
        {
            glo.Loga("ImprimirLista V2 INICIO");

            Font fonte = new Font("Segoe UI", 16, FontStyle.Bold);

            Rectangle area = e.MarginBounds;

            int y = area.Top;

            int alturaLinha = 40;

            while (_indiceImpressao < _listaParaImpressao.Count)
            {
                string texto = _listaParaImpressao[_indiceImpressao].Codigo;

                Rectangle linhaArea = new Rectangle(
                    area.Left,
                    y,
                    area.Width,
                    alturaLinha
                );

                e.Graphics.DrawString(
                    texto,
                    fonte,
                    Brushes.Black,
                    linhaArea
                );

                y += alturaLinha;

                _indiceImpressao++;

                if (y + alturaLinha > area.Bottom)
                {
                    glo.Loga("ImprimirLista V2 nova pagina indice=" + _indiceImpressao);
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;

            glo.Loga("ImprimirLista V2 FIM");
        }

        private string ExtrairCodigo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";
            string[] partes = texto.Split(' ');
            return partes[0];
        }

        private void ImprimirEtiquetas(PrintPageEventArgs e)
        {
            glo.Loga("ImprimirEtiquetas V4 INICIO (Com Sequência)");

            try
            {
                if (_listaParaImpressao == null || _listaParaImpressao.Count == 0)
                {
                    glo.Loga("ImprimirEtiquetas V4 - lista vazia");
                    e.HasMorePages = false;
                    return;
                }

                // Mantive a fonte Segoe UI Negrito 22 como estava
                Font fonteEtiqueta = new Font("Segoe UI", 22, FontStyle.Bold);

                Rectangle area = e.MarginBounds;

                int colunas = 3;
                int larguraColuna = area.Width / colunas;
                int alturaLinha = 90;

                int y = area.Top;

                while (_indiceImpressao < _listaParaImpressao.Count)
                {
                    if (y + alturaLinha > area.Bottom)
                    {
                        glo.Loga("ImprimirEtiquetas V4 - nova página indice=" + _indiceImpressao);
                        e.HasMorePages = true;
                        return;
                    }

                    for (int col = 0; col < colunas; col++)
                    {
                        if (_indiceImpressao >= _listaParaImpressao.Count)
                            break;

                        int x = area.Left + (col * larguraColuna);

                        Rectangle etiqueta = new Rectangle(
                            x,
                            y,
                            larguraColuna,
                            alturaLinha
                        );

                        // Desenha a borda da etiqueta
                        e.Graphics.DrawRectangle(Pens.Black, etiqueta);

                        using (StringFormat sf = new StringFormat()
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        })
                        {
                            // ============================================================
                            // AJUSTE: Pegamos o item completo para usar a Sequência + Código
                            // ============================================================
                            var itemAtual = _listaParaImpressao[_indiceImpressao];

                            // Extraímos o código puro (limpando lixos se houver)
                            string codigoPuro = ExtrairCodigo(itemAtual.Codigo);

                            // Montamos a string final: "Nº - CÓDIGO"
                            string textoExibicao = $"{itemAtual.Sequencia} - {codigoPuro}";

                            e.Graphics.DrawString(
                                textoExibicao,
                                fonteEtiqueta,
                                Brushes.Black,
                                etiqueta,
                                sf);
                            // ============================================================
                        }

                        _indiceImpressao++;
                    }

                    y += alturaLinha;
                }

                glo.Loga("ImprimirEtiquetas V4 - fim impressão indice=" + _indiceImpressao);
                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                glo.Loga("ImprimirEtiquetas V4 ERRO: " + ex.Message);
                throw;
            }
            finally
            {
                glo.Loga("ImprimirEtiquetas V4 FIM");
            }
        }

        private void btImprLista_Click(object sender, EventArgs e)
        {
            glo.Loga("btImprLista_Click V1 INICIO");
            _modoImpressao = TipoImpressao.Lista;
            AbrirPreview();
        }

        private void AbrirPreview()
        {
            glo.Loga("AbrirPreview V1 INICIO");
            var ds = gridCodigos.DataSource as IEnumerable<CodigoPartilheira>;
            _listaParaImpressao = ds?.ToList();
            if (_listaParaImpressao == null || _listaParaImpressao.Count == 0)
            {
                MessageBox.Show("Não há dados para imprimir.");
                return;
            }
            _printPreview.ShowDialog();
        }

        #endregion

        #region Eventos
        private void btAdicionar_Click(object sender, EventArgs e)
        {
            AdicionarCodigo();
        }

        private void txCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // evita "beep"
                AdicionarCodigo();
            }
        }

        private void AdicionarCodigo()
        {
            string codigo = (txCodigo.Text ?? "").Trim().ToUpper();

            // 1. Validação de campo vazio
            if (string.IsNullOrEmpty(codigo))
                return;

            // ============================================================
            // TRAVA DE SEGURANÇA: Evita que o Denis digite "1-BC5"
            // ============================================================
            // Se o código contiver um hífen, significa que ele tentou 
            // colocar a numeração manualmente.
            if (codigo.Contains("-"))
            {
                glo.Loga("AdicionarCodigo - Tentativa de inserir código com numeração manual: " + codigo);

                MessageBox.Show(
                    "Denis, não precisa mais colocar o número e o traço!\n\n" +
                    "Digite apenas o código da peça (exemplo: BC5).\n" +
                    "O sistema vai colocar o número da prateleira sozinho agora.",
                    "Aviso do Sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txCodigo.SelectAll();
                txCodigo.Focus();
                return; // Interrompe a inserção
            }
            // ============================================================

            // 2. Inserção no Banco (via DAO)
            // Agora temos a certeza que o código está limpo (Ex: "BC5")
            dao.Inserir(codigo);

            glo.Loga("AdicionarCodigo - Inserido com sucesso: " + codigo);

            // 3. Limpeza e Atualização
            txCodigo.Clear();
            txCodigo.Focus();

            // O CarregarGrid vai reordenar tudo e atribuir o novo número 
            // de sequência para este item automaticamente.
            CarregarGrid();
        }

        private void btExcluir_Click(object sender, EventArgs e)
        {
            if (gridCodigos.CurrentRow == null)
                return;

            var item = gridCodigos.CurrentRow.DataBoundItem as CodigoPartilheira;
            if (item == null)
                return;

            var resp = MessageBox.Show(
                $"Deseja excluir o código '{item.Codigo}'?",
                "Confirmar exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resp != DialogResult.Yes)
                return;

            dao.Excluir(item.Id);

            CarregarGrid();
            txCodigo.Focus();
        }

        private void btLimpar_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show(
                "Deseja limpar toda a lista?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resp != DialogResult.Yes)
                return;

            dao.LimparTodos();

            CarregarGrid();

            txBuscar.Clear();
            txCodigo.Clear();
            txCodigo.Focus();
        }

        #endregion

        #region Grid

        private void CarregarGrid()
        {
            glo.Loga(">>> CarregarGrid INICIO");

            var lista = dao.ListarTodos();

            // Sua lógica de parser e ordenação fantástica continua igual
            var estruturada = lista.Select(x => new {
                Original = x,
                Estrutura = CodigoPartilheiraParser.Parse(x.Codigo)
            }).ToList();

            var ordenada = estruturada
                .OrderBy(x => x.Estrutura.Prefixo)
                .ThenBy(x => x.Estrutura.Numero)
                .ThenBy(x => x.Estrutura.Sufixo)
                .ThenBy(x => x.Original.Codigo)
                .ThenBy(x => x.Original.Id)
                .Select(x => x.Original)
                .ToList();

            // ==========================================
            // NOVA LÓGICA: Atribuir a Sequência Dinâmica
            // ==========================================
            int contador = 1;
            foreach (var item in ordenada)
            {
                item.Sequencia = contador;
                contador++;
            }

            gridCodigos.DataSource = null;
            gridCodigos.DataSource = ordenada;

            glo.Loga("<<< CarregarGrid FIM");
        }

        private void gridCodigos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_recarregando)
                return;

            if (e.RowIndex < 0)
                return;

            var item = gridCodigos.Rows[e.RowIndex].DataBoundItem as CodigoPartilheira;
            if (item == null)
                return;

            string novoCodigo = (item.Codigo ?? "").Trim().ToUpper();

            if (string.IsNullOrEmpty(novoCodigo))
            {
                BeginInvoke(new Action(() => CarregarGrid()));
                return;
            }

            dao.Atualizar(item.Id, novoCodigo);

            // Reorganizar depois que a grid terminar o ciclo
            BeginInvoke(new Action(() =>
            {
                _recarregando = true;
                CarregarGrid();
                _recarregando = false;
            }));
        }
        private void gridCodigos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txCodigo.Focus();
                return;
            }

            if (e.KeyCode != Keys.Delete)
                return;

            if (gridCodigos.CurrentRow == null)
                return;

            var item = gridCodigos.CurrentRow.DataBoundItem as CodigoPartilheira;
            if (item == null)
                return;

            var resp = MessageBox.Show(
                $"Excluir '{item.Codigo}'?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resp != DialogResult.Yes)
                return;

            dao.Excluir(item.Id);

            CarregarGrid();
        }

        #endregion

        private void gridCodigos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            glo.Loga($"CellClick Linha={e.RowIndex}");
        }

        private void gridCodigos_SelectionChanged(object sender, EventArgs e)
        {
            glo.Loga("SelectionChanged");
        }

        private void gridCodigos_CurrentCellChanged(object sender, EventArgs e)
        {
            glo.Loga("CurrentCellChanged");
        }

        private void gridCodigos_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            glo.Loga($"CellBeginEdit Linha={e.RowIndex}");
        }

        private void txBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CarregarGrid();
            }
        }

        private void gridCodigos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                gridCodigos.BeginEdit(true);
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            glo.Loga("btImprimir_Click V4 INICIO");
            _modoImpressao = TipoImpressao.Etiquetas;
            AbrirPreview();
        }

        private void btReiniciar_Click(object sender, EventArgs e)
        {
            // 1. CONFIRMAÇÃO
            string mensagemAviso = "ATENÇÃO: Esta operação irá remover as numerações manuais de todos os códigos cadastrados (ex: transformará '1-BC5' em 'BC5').\n\n" +
                                   "Deseja prosseguir com a limpeza agora?";

            DialogResult resultado = MessageBox.Show(mensagemAviso, "Limpeza de Dados",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resultado != DialogResult.Yes)
            {
                glo.Loga("btReiniciar_Click - Operação cancelada pelo usuário.");
                return;
            }

            glo.Loga(">>> btReiniciar_Click - INICIO LIMPEZA INTELIGENTE");
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // 2. CHAMA O PROCESSO DE LIMPEZA (Separado)
                int corrigidos = ProcessarLimpezaEmMassa();

                // 3. FINALIZAÇÃO E FEEDBACK VISUAL
                glo.Loga($"<<< btReiniciar_Click - FIM LIMPEZA. Total corrigidos: {corrigidos}");
                this.Cursor = Cursors.Default;
                MessageBox.Show($"Sucesso! {corrigidos} códigos foram limpos de forma segura.", "Concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Recarrega a grid para mostrar os dados novos
                CarregarGrid();

                // Esconde o botão após o sucesso
                btReiniciar.Visible = false;
                glo.Loga("btReiniciar_Click - Botão de reiniciar ocultado com sucesso.");
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                glo.Loga("ERRO CRÍTICO em btReiniciar_Click: " + ex.Message);
                glo.Loga("Stack Trace: " + ex.StackTrace);
                MessageBox.Show("Erro ao processar limpeza: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ProcessarLimpezaEmMassa()
        {
            var listaSuja = dao.ListarTodos();
            int corrigidos = 0;

            glo.Loga($"ProcessarLimpezaEmMassa - Total de registros lidos para análise: {listaSuja.Count}");

            foreach (var item in listaSuja)
            {
                if (item.Codigo.Contains("-"))
                {
                    // Chama a função que analisa apenas o texto
                    string novoCodigo = AnalisarEExtrairCodigo(item.Codigo);

                    if (novoCodigo != null) // Se retornou um código novo, faz o update!
                    {
                        glo.Loga($"ProcessarLimpezaEmMassa - ALTERANDO ID {item.Id}: '{item.Codigo}' -> '{novoCodigo}'");
                        dao.Atualizar(item.Id, novoCodigo);
                        corrigidos++;
                    }
                    else
                    {
                        glo.Loga($"ProcessarLimpezaEmMassa - IGNORANDO ID {item.Id} ('{item.Codigo}'): Não requer limpeza ou estrutura inválida.");
                    }
                }
            }

            return corrigidos;
        }

        private string AnalisarEExtrairCodigo(string codigoOriginal)
        {
            // Divide o código pelo traço para analisar a primeira parte
            string[] partes = codigoOriginal.Split('-');
            string prefixo = partes[0].Trim();

            // Se não tem nada antes do traço (ex: "-BC5"), ignora
            if (string.IsNullOrEmpty(prefixo))
                return null;

            // Verifica se o prefixo é EXATAMENTE UM NÚMERO (Ex: "1", "298"). 
            foreach (char c in prefixo)
            {
                if (!char.IsDigit(c))
                {
                    // Se achou qualquer letra (Ex: "BC"), aborta a limpeza e retorna nulo
                    return null;
                }
            }

            // Se chegou até aqui, temos certeza que a primeira parte é só número.
            // Vamos cortar do primeiro traço em diante.
            int posicaoHifen = codigoOriginal.IndexOf('-');
            string novoCodigo = codigoOriginal.Substring(posicaoHifen + 1).Trim();

            // Proteção: Se o corte gerasse um código vazio, não faz nada
            if (string.IsNullOrEmpty(novoCodigo))
                return null;

            return novoCodigo; // Retorna o código limpo (ex: "BC5")
        }

    }

    enum TipoImpressao
    {
        Etiquetas,
        Lista
    }

}


