using System;
using System.Windows.Forms;

namespace TeleBonifacio.rel
{
    public partial class Extrato : Form
    {

        private DataGridView dataGridView;

        public Extrato()
        {
            InitializeComponent();
            InitializeDataGridView();
            SetStartPosition();
        }

        private void InitializeDataGridView()
        {
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            Controls.Add(dataGridView);
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        public void SetNome(string text)
        {
            this.lblTitulo.Text = "Extrato: " + text;
        }
    }
}
