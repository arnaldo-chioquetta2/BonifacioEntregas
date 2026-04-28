using System;
using System.Linq;
using System.Drawing;
using TeleBonifacio.tb;
using TeleBonifacio.dao;
using TeleBonifacio.gen;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Collections.Generic;

// 4.2.0 Poder colocar letras nos numeros da prateleira
// 4.1.9 Retorno da edição dos códigos da prateleira
// 4.1.8 Edição nos numeros dos códigos de prateleira
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
        private bool _pulandoPeloEnter = false;

        #region Inicialização

        public operPartilheira()
        {
            InitializeComponent();
        }

        private void operPartilheira_Load(object sender, EventArgs e)
        {
            ConfigurarFormulario();

            dao = new CodigoPartilheiraDAO();

            ConfigurarGrid();
            ConfigurarImpressao();

            // Visual e Layout
            AplicarModoVisualGrande();
            AjustarLayoutVisual();

            // Carga inicial
            CarregarGrid();
            txCodigo.Focus();
        }

        private void ConfigurarFormulario()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - 20;
            this.Top = 0;
            this.MinimumSize = new Size(1000, 700);
            this.Padding = new Padding(10);

            // Eventos do Form
            this.Resize += (s, ev) => AjustarLayoutVisual();

            // Garante que o evento de Enter no campo de busca/código funcione
            txCodigo.KeyDown -= txCodigo_KeyDown;
            txCodigo.KeyDown += txCodigo_KeyDown;
        }

        private void ConfigurarGrid()
        {
            gridCodigos.AutoGenerateColumns = false;

            // --- 1. COLUNA DE ID (OCULTA) ---
            if (!gridCodigos.Columns.Contains("colId"))
            {
                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.Name = "colId";
                colId.DataPropertyName = "Id";
                colId.Visible = false;
                gridCodigos.Columns.Add(colId);
            }

            // --- 2. COLUNA DE ENDEREÇO (Nº) ---
            if (!gridCodigos.Columns.Contains("colEndereco"))
            {
                DataGridViewTextBoxColumn colEndereco = new DataGridViewTextBoxColumn();
                colEndereco.Name = "colEndereco";
                colEndereco.HeaderText = "Nº";
                colEndereco.DataPropertyName = "Endereco";

                // AUMENTADO: De 90 para 135 (metade a mais)
                colEndereco.Width = 135;

                colEndereco.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colEndereco.ReadOnly = false;
                colEndereco.SortMode = DataGridViewColumnSortMode.NotSortable;

                // Mantendo o "Meio-Termo" da fonte em 20
                colEndereco.DefaultCellStyle.Font = new Font("Segoe UI", 20f, FontStyle.Bold);
                colEndereco.DefaultCellStyle.ForeColor = Color.Blue;

                gridCodigos.Columns.Insert(0, colEndereco);
            }

            // --- 3. COLUNA DE CÓDIGO ---
            colCodigo.DataPropertyName = "Codigo";
            colCodigo.SortMode = DataGridViewColumnSortMode.NotSortable;
            colCodigo.ReadOnly = false;
            colCodigo.DefaultCellStyle.Font = new Font("Segoe UI", 14f);

            // --- CONFIGURAÇÕES GERAIS ---
            gridCodigos.RowTemplate.Height = 45;
            gridCodigos.ColumnHeadersHeight = 40;

            gridCodigos.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            gridCodigos.SelectionMode = DataGridViewSelectionMode.CellSelect;
            gridCodigos.RowHeadersVisible = true;
            gridCodigos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            gridCodigos.CellValueChanged -= gridCodigos_CellValueChanged;
            gridCodigos.CellValueChanged += gridCodigos_CellValueChanged;
        }

        private void ConfigurarImpressao()
        {
            _printDocument = new PrintDocument();
            _printDocument.PrintPage += PrintDocument_PrintPage;
            _printDocument.BeginPrint += PrintDocument_BeginPrint;

            _printPreview = new PrintPreviewDialog
            {
                Document = _printDocument,
                WindowState = FormWindowState.Maximized
            };
        }

        private void gridCodigos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                int idItem = Convert.ToInt32(gridCodigos.Rows[e.RowIndex].Cells["colId"].Value);
                string novoValor = gridCodigos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
                string nomeColuna = gridCodigos.Columns[e.ColumnIndex].Name;

                // CASO 1: Ele editou o Número/Endereço (Agora aceita Letras!)
                if (nomeColuna == "colEndereco")
                {
                    // Passa o texto direto, sem tryParse
                    dao.AtualizarEndereco(idItem, novoValor.Trim());
                    glo.Loga($"Denis mudou Nº do ID {idItem} para '{novoValor}'");
                }
                // CASO 2: Ele editou o Texto do Código
                else if (nomeColuna == "colCodigo")
                {
                    dao.Atualizar(idItem, novoValor.ToUpper().Trim());
                    glo.Loga($"Denis mudou Código do ID {idItem} para '{novoValor}'");
                }
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao salvar edição direta na grid: " + ex.Message);
            }
        }

        private void PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            glo.Loga("PrintDocument_BeginPrint V1 INICIO");
            _indiceImpressao = 0;
            glo.Loga("PrintDocument_BeginPrint V1 indice resetado");
        }

        private void AplicarModoVisualGrande()
        {
            // Fonte 16 para o resto da tela (Labels, Botoes, TextBox)
            float fontePadrao = 16f;

            txCodigo.Font = new Font("Segoe UI", fontePadrao);
            txBuscar.Font = new Font("Segoe UI", fontePadrao);

            // Altura dos campos mais discreta
            txCodigo.AutoSize = false;
            txCodigo.Height = 35;
            txBuscar.AutoSize = false;
            txBuscar.Height = 35;

            foreach (Control c in this.Controls)
            {
                if (c is Label)
                {
                    c.Font = new Font("Segoe UI", fontePadrao, FontStyle.Bold);
                }
                else if (c is Button)
                {
                    c.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
                    c.Height = 45; // Botão funcional e elegante
                }
            }
        }


        private void AjustarLayoutVisual()
        {
            int margem = 25;

            lblCodigo.Location = new Point(margem, 30);
            txCodigo.Location = new Point(150, 25);
            txCodigo.Width = 300;

            btAdicionar.Location = new Point(470, 23);
            btAdicionar.Width = 170;

            lblBuscar.Location = new Point(margem, 90);
            txBuscar.Location = new Point(150, 85);
            txBuscar.Width = 490;

            int topoGrid = 150;
            int rodapeAltura = 80;

            gridCodigos.Location = new Point(margem, topoGrid);
            gridCodigos.Size = new Size(
                this.ClientSize.Width - (margem * 2),
                this.ClientSize.Height - topoGrid - rodapeAltura
            );

            int yBotoes = this.ClientSize.Height - 60;

            btExcluir.Width = 150;
            btLimpar.Width = 180;
            btReiniciar.Width = 150;
            btImprimir.Width = 150;
            btImprLista.Width = 150;

            btExcluir.Location = new Point(margem, yBotoes);
            btExcluir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btLimpar.Location = new Point(margem + 170, yBotoes);
            btLimpar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btReiniciar.Location = new Point(margem + 370, yBotoes);
            btReiniciar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btReiniciar.Height = btLimpar.Height;

            btImprimir.Location = new Point(this.ClientSize.Width - 175, yBotoes);
            btImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

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
            glo.Loga("ImprimirLista V3 INICIO (Com Endereço)");

            // Mantive a fonte Segoe UI Negrito 16 como padrão para lista
            Font fonte = new Font("Segoe UI", 16, FontStyle.Bold);
            Rectangle area = e.MarginBounds;
            int y = area.Top;
            int alturaLinha = 40;

            // Percorre a lista a partir do índice de interrupção (caso haja mais de uma página)
            while (_indiceImpressao < _listaParaImpressao.Count)
            {
                // Pegamos o objeto completo da lista
                var itemAtual = _listaParaImpressao[_indiceImpressao];

                // =========================================================
                // AJUSTE: Montamos o texto com o Endereço (Nº) + Código
                // Exemplo: "299 - PD24"
                // =========================================================
                string textoParaImprimir = $"{itemAtual.Endereco} - {itemAtual.Codigo}";

                Rectangle linhaArea = new Rectangle(
                    area.Left,
                    y,
                    area.Width,
                    alturaLinha
                );

                // Desenha o texto na página
                e.Graphics.DrawString(
                    textoParaImprimir,
                    fonte,
                    Brushes.Black,
                    linhaArea
                );

                // Incrementa o Y para a próxima linha
                y += alturaLinha;

                // Move para o próximo item da lista
                _indiceImpressao++;

                // Verificação de fim de página
                if (y + alturaLinha > area.Bottom)
                {
                    glo.Loga("ImprimirLista V3 - Nova página necessária. Índice=" + _indiceImpressao);
                    e.HasMorePages = true;
                    return;
                }
            }

            // Se saiu do laço, a impressão terminou
            e.HasMorePages = false;
            glo.Loga("ImprimirLista V3 FIM");
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
            glo.Loga("ImprimirEtiquetas V4 INICIO (Com Sequência Correta)");

            try
            {
                if (_listaParaImpressao == null || _listaParaImpressao.Count == 0)
                {
                    glo.Loga("ImprimirEtiquetas V4 - lista vazia");
                    e.HasMorePages = false;
                    return;
                }

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
                            x, y, larguraColuna, alturaLinha
                        );

                        e.Graphics.DrawRectangle(Pens.Black, etiqueta);
                        

                        using (StringFormat sf = new StringFormat()
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        })
                        {
                            var itemAtual = _listaParaImpressao[_indiceImpressao];
                            string codigoPuro = ExtrairCodigo(itemAtual.Codigo);

                            // =========================================================
                            // AQUI ESTAVA O ERRO!
                            // Antes estava: itemAtual.Sequencia
                            // Agora deve ser: itemAtual.Endereco
                            // =========================================================
                            string textoExibicao = $"{itemAtual.Endereco} - {codigoPuro}";

                            e.Graphics.DrawString(
                                textoExibicao,
                                fonteEtiqueta,
                                Brushes.Black,
                                etiqueta,
                                sf);
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
            // 1. Captura e limpa o texto digitado
            string codigo = (txCodigo.Text ?? "").Trim().ToUpper();

            // 2. Validação básica de campo vazio
            if (string.IsNullOrEmpty(codigo))
                return;

            // 3. Trava de segurança: impede que o Denis tente digitar o número manualmente no campo de código
            if (codigo.Contains("-"))
            {
                MessageBox.Show("Denis, digite apenas o código da peça.\nO número da prateleira será gerado no final da lista!",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txCodigo.SelectAll();
                txCodigo.Focus();
                return;
            }

            try
            {
                // 4. Lógica para descobrir o próximo número de endereço
                var listaAtual = dao.ListarTodos();
                int maxNumero = 0;

                if (listaAtual.Count > 0)
                {
                    // Percorre a lista para achar o maior número puro já usado
                    foreach (var item in listaAtual)
                    {
                        // Como Endereco agora é string, tentamos converter para ver se é um número
                        // Se for "A1", o TryParse ignora. Se for "299", ele considera no cálculo.
                        if (int.TryParse(item.Endereco, out int numAtual))
                        {
                            if (numAtual > maxNumero)
                                maxNumero = numAtual;
                        }
                    }
                }

                // 5. Define o próximo endereço como string (ex: "300")
                string proximoEndereco = (maxNumero + 1).ToString();

                // 6. Insere no banco via DAO (passando código e o novo endereço)
                dao.Inserir(codigo, proximoEndereco);

                glo.Loga($"AdicionarCodigo - Inserido: {codigo} no endereço {proximoEndereco}");

                // 7. Limpeza da interface e atualização da Grid
                txCodigo.Clear();
                txCodigo.Focus();
                CarregarGrid();
            }
            catch (Exception ex)
            {
                glo.Loga("Erro em AdicionarCodigo: " + ex.Message);
                MessageBox.Show("Erro ao inserir: " + ex.Message, "Erro Técnico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            // Atribuição de Sequência mantida para fins de ordenação interna da tela
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
            if (_recarregando) return;
            if (e.RowIndex < 0) return;

            int idAtual = Convert.ToInt32(gridCodigos.Rows[e.RowIndex].Cells["colId"].Value);
            int col = e.ColumnIndex;

            // Salva a intenção de pular (que veio do ProcessCmdKey) e já desliga a chave
            bool devePular = _pulandoPeloEnter;
            _pulandoPeloEnter = false;

            BeginInvoke(new Action(() =>
            {
                _recarregando = true;
                CarregarGrid();
                _recarregando = false;

                // Se a ordem de pular foi dada pelo ENTER:
                if (devePular)
                {
                    // Caça onde a linha foi parar após recarregar
                    for (int i = 0; i < gridCodigos.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(gridCodigos.Rows[i].Cells["colId"].Value) == idAtual)
                        {
                            MoverParaBaixoOuFocarCodigo(i, col);
                            break;
                        }
                    }
                }
            }));
        }

        private void gridCodigos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _pulandoPeloEnter = true; // "Sim, eu quero que você pule no final desta edição"
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }



        private void MoverParaBaixoOuFocarCodigo(int indiceLinhaReferencia, int indiceColuna)
        {
            // Tenta ir para a linha logo abaixo da que foi editada
            int proxima = indiceLinhaReferencia + 1;

            if (proxima < gridCodigos.Rows.Count)
            {
                gridCodigos.Focus();
                gridCodigos.CurrentCell = gridCodigos.Rows[proxima].Cells[indiceColuna];
                gridCodigos.BeginEdit(true);
            }
            else
            {
                // Se era a última, volta para o campo de cima
                txCodigo.Focus();
                txCodigo.SelectAll();
            }
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
            _pulandoPeloEnter = false;
            glo.Loga($"Edição iniciada na Linha={e.RowIndex}. Pulo automático cancelado.");
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
            string mensagem = "ATENÇÃO: Esta operação vai limpar os números dos códigos (Ex: '100-PD24' vira 'PD24').\n\n" +
                               "Isso deixará sua lista muito mais limpa. Deseja continuar?";

            if (MessageBox.Show(mensagem, "Limpeza Inteligente", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                int totalCorrigidos = ProcessarLimpezaEmMassa();

                CarregarGrid();

                MessageBox.Show($"Sucesso! {totalCorrigidos} códigos foram limpos.", "Concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Esconde o botão após o uso para ele não clicar sem querer depois
                btReiniciar.Visible = false;
            }
            catch (Exception ex)
            {
                glo.Loga("Erro no Reiniciar: " + ex.Message);
                MessageBox.Show("Erro ao processar: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
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
                    string novoCodigo = AnalisarEExtrairCodigo(item.Codigo);

                    if (novoCodigo != null)
                    {
                        glo.Loga($"ProcessarLimpezaEmMassa - ALTERANDO ID {item.Id}: '{item.Codigo}' -> '{novoCodigo}'");
                        dao.Atualizar(item.Id, novoCodigo);
                        corrigidos++;
                    }
                }
            }
            return corrigidos;
        }

        private string AnalisarEExtrairCodigo(string codigoOriginal)
        {
            string[] partes = codigoOriginal.Split('-');
            string prefixo = partes[0].Trim();

            if (string.IsNullOrEmpty(prefixo))
                return null;

            foreach (char c in prefixo)
            {
                if (!char.IsDigit(c))
                    return null;
            }

            int posicaoHifen = codigoOriginal.IndexOf('-');
            string novoCodigo = codigoOriginal.Substring(posicaoHifen + 1).Trim();

            if (string.IsNullOrEmpty(novoCodigo))
                return null;

            return novoCodigo;
        }

        private void gridCodigos_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Verifica se o controle de edição que abriu é uma caixa de texto
            if (e.Control is TextBox txEdicao)
            {
                // Removemos o evento antigo para não duplicar cliques
                txEdicao.KeyDown -= TxEdicao_KeyDown;
                // Adicionamos o nosso evento espião
                txEdicao.KeyDown += TxEdicao_KeyDown;
            }
        }

        private void TxEdicao_KeyDown(object sender, KeyEventArgs e)
        {
            // Se ele apertar ENTER **DENTRO** da célula que está editando
            if (e.KeyCode == Keys.Enter)
            {
                _pulandoPeloEnter = true; // Avisamos: "Pode pular para baixo!"
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                // CENÁRIO 1: Ele está com o cursor piscando DENTRO da célula (digitando "13a")
                if (gridCodigos.EditingControl != null)
                {
                    _pulandoPeloEnter = true; // Avisa o sistema que o pulo é obrigatório!
                    gridCodigos.EndEdit(); // Força o salvamento (vai chamar o CellEndEdit)
                    return true; // Diz ao Windows: "Já resolvi esse Enter, não faça mais nada"
                }
                // CENÁRIO 2: Ele está apenas navegando na Grid com as setas, sem digitar
                else if (gridCodigos.Focused && gridCodigos.CurrentCell != null)
                {
                    _pulandoPeloEnter = true;
                    MoverParaBaixoOuFocarCodigo(gridCodigos.CurrentCell.RowIndex, gridCodigos.CurrentCell.ColumnIndex);
                    _pulandoPeloEnter = false;
                    return true;
                }
            }

            // Para as outras teclas, segue a vida normalmente
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }

    enum TipoImpressao
    {
        Etiquetas,
        Lista
    }
}