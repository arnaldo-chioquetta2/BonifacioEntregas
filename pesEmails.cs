using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class pesEmails : Form
    {
        private bool carregando = true;
        private FornecedorDao forn;

        public pesEmails()
        {
            InitializeComponent();
        }

        private void pesEmails_Load(object sender, System.EventArgs e)
        {
            forn = new FornecedorDao();
            CarregarComboBox();
            carregando = false;
        }

        private void CarregarComboBox()
        {
            List<tb.ComboBoxItem> lista = new List<tb.ComboBoxItem>();
            DataTable dados = forn.getEmails();
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                string email = row["email"].ToString();

                tb.ComboBoxItem item = new tb.ComboBoxItem(id, nome, email);
                lista.Add(item);
            }
            cmbEmails.DataSource = lista;
            cmbEmails.DisplayMember = "Nome";
            cmbEmails.ValueMember = "Id";
            cmbEmails.SelectedIndex = -1;
        }

        private void btFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbEmails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                int x = 0;
            }
        }

        private void cmbEmails_TextChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                var selectedItem = cmbEmails.SelectedItem as tb.ComboBoxItem;
                if (selectedItem != null)
                {
                    lbEmail.Text = selectedItem.Email;
                }
            }
        }

    }

}