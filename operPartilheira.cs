using System.Windows.Forms;
using System.Collections.Generic;

namespace TeleBonifacio
{
    public partial class operPartilheira : Form
    {

        private List<string> listaCodigos;
        private BindingSource bindingSource;

        public operPartilheira()
        {
            InitializeComponent();

        }

        private void operPartilheira_Load(object sender, System.EventArgs e)
        {
            listaCodigos = new List<string>();
            bindingSource = new BindingSource();

            bindingSource.DataSource = listaCodigos;
            gridCodigos.DataSource = bindingSource;

            gridCodigos.AllowUserToAddRows = false;
            gridCodigos.AllowUserToDeleteRows = false;
            gridCodigos.AllowUserToResizeRows = false;
            gridCodigos.MultiSelect = false;
            gridCodigos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridCodigos.RowHeadersVisible = false;
            gridCodigos.EditMode = DataGridViewEditMode.EditOnEnter;
            gridCodigos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridCodigos.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

        }
    }
}
