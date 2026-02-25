using System;
using System.Linq;
using TeleBonifacio.gen;
using System.Windows.Forms;
using System.Collections.Generic;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class operPartilheira : Form
    {

        private CodigoPartilheiraDAO dao;
        private BindingSource bindingSource;
        private bool _atualizandoGrid = false;
        private bool _processandoEdicao = false;

        public operPartilheira()
        {
            InitializeComponent();

        }

        private void operPartilheira_Load(object sender, EventArgs e)
        {

            dao = new CodigoPartilheiraDAO();
            CarregarGrid();

            // (deixe sem rt por enquanto)
            //listaCodigos = new List<CodigoItem>();
            //bindingSource = new BindingSource();

            //gridCodigos.AutoGenerateColumns = false;
            //gridCodigos.DataSource = bindingSource;

            //// coluna do designer
            //colCodigo.DataPropertyName = "Codigo";
            //colCodigo.SortMode = DataGridViewColumnSortMode.NotSortable;

            //AtualizarGrid();
            txCodigo.Focus();
        }

        private void CarregarGrid()
        {
            //var lista = dao.ListarTodos();

            //// Ordenação natural
            //var ordenada = lista
            //    .OrderBy(x => CodigoPartilheiraParser.Parse(x.Codigo).Prefixo)
            //    .ThenBy(x => CodigoPartilheiraParser.Parse(x.Codigo).Numero)
            //    .ThenBy(x => CodigoPartilheiraParser.Parse(x.Codigo).Sufixo)
            //    .ThenBy(x => x.Codigo)
            //    .ThenBy(x => x.Id)
            //    .ToList();

            //string filtro = (txBuscar.Text ?? "").Trim().ToUpper();

            //if (!string.IsNullOrEmpty(filtro))
            //    ordenada = ordenada
            //        .Where(x => x.Codigo.ToUpper().Contains(filtro))
            //        .ToList();

            //gridCodigos.AutoGenerateColumns = false;
            //gridCodigos.DataSource = ordenada;

            //gridCodigos.ClearSelection();
        }
        
        #region Eventos
        private void btAdicionar_Click(object sender, EventArgs e)
        {
            //AdicionarCodigo();
        }

        private void txCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // evita "beep"
                //AdicionarCodigo();
            }
        }

        private void txBuscar_TextChanged(object sender, EventArgs e)
        {
            //AtualizarGrid();
        }

        private void btExcluir_Click(object sender, EventArgs e)
        {
            //if (listaCodigos == null || listaCodigos.Count == 0)
            //    return;

            //if (gridCodigos.CurrentRow == null)
            //    return;

            //var item = gridCodigos.CurrentRow.DataBoundItem as CodigoItem;
            //if (item == null)
            //    return;

            //// Confirmação (opcional, mas recomendado)
            //var resp = MessageBox.Show(
            //    $"Deseja excluir o código '{item.Codigo}'?",
            //    "Confirmar exclusão",
            //    MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question);

            //if (resp != DialogResult.Yes)
            //    return;

            //// Remove pelo Id (não erra com duplicados)
            //listaCodigos.RemoveAll(x => x.Id == item.Id);

            //AtualizarGrid();
            //txCodigo.Focus();
        }

        private void btLimpar_Click(object sender, EventArgs e)
        {
            //if (listaCodigos == null || listaCodigos.Count == 0)
            //    return;

            //var resp = MessageBox.Show(
            //    "Deseja limpar toda a lista?",
            //    "Confirmar",
            //    MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question);

            //if (resp != DialogResult.Yes)
            //    return;

            //listaCodigos.Clear();
            //AtualizarGrid();

            //txBuscar.Clear();   // opcional: limpa filtro junto
            //txCodigo.Clear();   // opcional: limpa campo de entrada
            //txCodigo.Focus();
        }

        #endregion

        #region Grid

        //private void AtualizarGrid()
        //{
        //    if (listaCodigos == null || bindingSource == null) return;
        //    if (_atualizandoGrid) return;

        //    _atualizandoGrid = true;
        //    gridCodigos.SuspendLayout();

        //    try
        //    {
        //        // Normaliza a lista principal (sem disparar refresh da grid)
        //        foreach (var it in listaCodigos)
        //            it.Codigo = (it.Codigo ?? "").Trim().ToUpper();

        //        // Monta lista de exibição (mesmas instâncias)
        //        var listaExibicao = listaCodigos.ToList();

        //        // Filtro simples
        //        string filtro = (txBuscar.Text ?? "").Trim().ToUpper();
        //        if (!string.IsNullOrEmpty(filtro))
        //        {
        //            listaExibicao = listaExibicao
        //                .Where(x => !string.IsNullOrWhiteSpace(x.Codigo) && x.Codigo.Contains(filtro))
        //                .ToList();
        //        }

        //        // Atualiza binding
        //        bindingSource.DataSource = listaExibicao;
        //        bindingSource.ResetBindings(false);
        //    }
        //    finally
        //    {
        //        gridCodigos.ResumeLayout();
        //        _atualizandoGrid = false;
        //    }
        //}

        private void gridCodigos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (_atualizandoGrid || _processandoEdicao) return;
            //if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            //try
            //{
            //    _processandoEdicao = true;

            //    var itemExibido = gridCodigos.Rows[e.RowIndex].DataBoundItem as CodigoItem;
            //    if (itemExibido == null) return;

            //    string novoCodigo = (itemExibido.Codigo ?? "").Trim().ToUpper();

            //    // Se ficar vazio, restaura (ou você pode optar por remover)
            //    if (string.IsNullOrEmpty(novoCodigo))
            //    {
            //        var original = listaCodigos.FirstOrDefault(x => x.Id == itemExibido.Id);
            //        itemExibido.Codigo = original?.Codigo ?? "";
            //        bindingSource.ResetBindings(false);
            //        return;
            //    }

            //    // Atualiza o item na lista principal
            //    var itemOriginal = listaCodigos.FirstOrDefault(x => x.Id == itemExibido.Id);
            //    if (itemOriginal != null)
            //        itemOriginal.Codigo = novoCodigo;

            //    // Atualiza o exibido
            //    itemExibido.Codigo = novoCodigo;

            //    // Agendar refresh depois do fim do ciclo de edição (evita recursão)
            //    this.BeginInvoke((Action)(() =>
            //    {
            //        AtualizarGrid();
            //        // opcional: voltar foco para digitação contínua
            //        txCodigo.Focus();
            //    }));
            //}
            //finally
            //{
            //    {
            //        _processandoEdicao = false;
            //    }
            //}

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

            if (gridCodigos.IsCurrentCellInEditMode)
                return;

            // ... (resto do delete igual acima)
        }

        #endregion


    }

    //public class CodigoItem
    //{
    //    public Guid Id { get; set; } = Guid.NewGuid();
    //    public string Codigo { get; set; }
    //}
}


