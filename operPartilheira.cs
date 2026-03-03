using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.gen;
using TeleBonifacio.tb;

// 4.1.2 Correção de bug na tela de códigos de prateleira
// 4.1.1 Tela de códigos de pratileira

namespace TeleBonifacio
{
    public partial class operPartilheira : Form
    {

        private CodigoPartilheiraDAO dao;
        // private Timer _timerBusca;

        private PrintDocument _printDocument;
        private PrintPreviewDialog _printPreview;
        private List<CodigoPartilheira> _listaParaImpressao;
        private int _indiceImpressao;
        private bool _recarregando;

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
            // GRID
            // ===============================

            gridCodigos.AutoGenerateColumns = false;
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
            _printDocument.DefaultPageSettings.Landscape = true;
            _printDocument.PrintPage += PrintDocument_PrintPage;

            _printPreview = new PrintPreviewDialog();
            _printPreview.Document = _printDocument;

            // ===============================
            // DADOS
            // ===============================

            CarregarGrid();
            this.Resize += (s, ev) => AjustarLayoutVisual();

            txCodigo.Focus();
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

            btExcluir.Width = 150;
            btLimpar.Width = 180;
            btImprimir.Width = 150;

            btExcluir.Location = new Point(margem, yBotoes);
            btExcluir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btLimpar.Location = new Point(margem + 170, yBotoes);
            btLimpar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            btImprimir.Location = new Point(this.ClientSize.Width - 175, yBotoes);
            btImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }

        #endregion

        #region Impressão

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font fonteEtiqueta = new Font("Segoe UI", 28, FontStyle.Bold);

            int margemEsquerda = e.MarginBounds.Left;
            int margemTopo = e.MarginBounds.Top;

            int larguraTotal = e.MarginBounds.Width;
            int alturaTotal = e.MarginBounds.Height;

            int colunas = 2;
            int larguraColuna = larguraTotal / colunas;
            int alturaLinha = 80; // altura grande para etiqueta

            int y = margemTopo;

            while (_indiceImpressao < _listaParaImpressao.Count)
            {
                for (int col = 0; col < colunas; col++)
                {
                    if (_indiceImpressao >= _listaParaImpressao.Count)
                        break;

                    string codigo = _listaParaImpressao[_indiceImpressao].Codigo;

                    int posX = margemEsquerda + (col * larguraColuna);

                    Rectangle area = new Rectangle(
                        posX,
                        y,
                        larguraColuna,
                        alturaLinha
                    );

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    // borda para recorte
                    e.Graphics.DrawRectangle(Pens.Black, area);

                    e.Graphics.DrawString(codigo, fonteEtiqueta, Brushes.Black, area, sf);

                    _indiceImpressao++;
                }

                y += alturaLinha;

                if (y + alturaLinha > margemTopo + alturaTotal)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            _listaParaImpressao = (gridCodigos.DataSource as List<CodigoPartilheira>)?.ToList();

            if (_listaParaImpressao == null || _listaParaImpressao.Count == 0)
            {
                MessageBox.Show("Não há dados para imprimir.");
                return;
            }

            _indiceImpressao = 0;

            _printPreview.Width = 1000;
            _printPreview.Height = 700;
            _printPreview.ShowDialog();
        }

        #endregion

        //private void TimerBusca_Tick(object sender, EventArgs e)
        //{
        //    _timerBusca.Stop();
        //    CarregarGrid();
        //}

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

            if (string.IsNullOrEmpty(codigo))
                return;

            // opcional: evitar inserir com espaços / caracteres estranhos
            // codigo = codigo.Replace(" ", "");

            dao.Inserir(codigo);

            txCodigo.Clear();
            txCodigo.Focus();

            CarregarGrid();
        }

        //private void txBuscar_TextChanged(object sender, EventArgs e)
        //{
        //    //_timerBusca.Stop();
        //    //_timerBusca.Start();
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        e.SuppressKeyPress = true;
        //        CarregarGrid();
        //    }
        //}

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

            var estruturada = lista
                .Select(x => new
                {
                    Original = x,
                    Estrutura = CodigoPartilheiraParser.Parse(x.Codigo)
                })
                .ToList();

            var ordenada = estruturada
                .OrderBy(x => x.Estrutura.Prefixo)
                .ThenBy(x => x.Estrutura.Numero)
                .ThenBy(x => x.Estrutura.Sufixo)
                .ThenBy(x => x.Original.Codigo)
                .ThenBy(x => x.Original.Id)
                .Select(x => x.Original)
                .ToList();

            string filtro = (txBuscar.Text ?? "").Trim().ToUpper();

            if (!string.IsNullOrEmpty(filtro))
            {
                glo.Loga("Filtro aplicado: " + filtro);
                ordenada = ordenada
                    .Where(x => x.Codigo.ToUpper().Contains(filtro))
                    .ToList();
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

    }

}


