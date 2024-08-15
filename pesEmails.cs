using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.gen;

namespace TeleBonifacio
{
    public partial class pesEmails : Form
    {
        private bool carregando = true;
        private FornecedorDao forn;
        private INI cINI;

        public string Arquivo = "";
        private string ArquivoEmail = "";

        public pesEmails()
        {
            InitializeComponent();
        }

        private void pesEmails_Load(object sender, System.EventArgs e)
        {
            forn = new FornecedorDao();
            cINI = new INI();
            txTitulo.Text = cINI.ReadString("Email", "Titulo", "");
            ArquivoEmail = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "email.txt");
            try
            {
                string Texto = File.ReadAllText(ArquivoEmail);
                txTexto.Text = Texto;
            }
            catch (Exception)
            {
                // throw;
            }
            CarregarComboBox();
            carregando = false;
        }

        private async void btOK_Click(object sender, EventArgs e)
        {
            glo.Loga("btOK_Click na tela de emails");
            string Texto = txTexto.Text;
            if (Texto.Length == 0)
            {
                MessageBox.Show("É necessário um conteúdo no email", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    Cursor = Cursors.WaitCursor; // Define o cursor como ampulheta
                    this.Text = "Enviando Email";
                    File.WriteAllText(ArquivoEmail, Texto);                    
                    cINI.WriteString("Email", "Titulo", txTitulo.Text);
                    Email cEmail = new Email();
                    string Remetente = cINI.ReadString("Email", "Remetente", "");
                    glo.Loga("Remetente = " + Remetente);

                    // AJUSTAR A CRIPTOGRAFIA
                    // SENHA ORIGINAL   "uhkikktxafjvpwem"
                    // SENHA RESULTANTE "uhkikktxafjvp2em"
                    // string senhaCri = cINI.ReadString("Email", "senha", "");
                    // string senha = Cripto.Decrypt(senhaCri);

                    // Minha
                    // string senha = "uhkikktxafjvpwem";

                    // Denis
                    string senha = "vxytmxwmbbipbwcg";

                    bool enviado = cEmail.EnviarEmail(
                    Remetente,
                        Remetente,
                        senha,
                        lbEmail.Text,
                        Arquivo,
                        txTitulo.Text,
                        Texto
                    );

                    if (enviado)
                    {
                        Cursor = Cursors.Default;
                        this.Close();
                    }
                    else
                    {
                        this.Text = "Email não foi enviado";
                    }
                }
                finally
                {
                    Cursor = Cursors.Default; // Restaura o cursor padrão
                }
            }
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
                MostraEmail();
            }
        }

        private void MostraEmail()
        {
            string emailSelecionado = null;
            var selectedItem = cmbEmails.SelectedItem as tb.ComboBoxItem;
            if (selectedItem != null)
            {
                emailSelecionado = selectedItem.Email;
            }
            else if (!string.IsNullOrEmpty(cmbEmails.Text))
            {
                var item = cmbEmails.Items.Cast<tb.ComboBoxItem>()
                                     .FirstOrDefault(i => i.Nome.Equals(cmbEmails.Text, StringComparison.OrdinalIgnoreCase));

                if (item != null)
                {
                    emailSelecionado = item.Email;
                }
            }
            lbEmail.Text = emailSelecionado ?? string.Empty;
            VeSeHab();
        }

        private void VeSeHab()
        {
            bool ok = true;
            if (cmbEmails.SelectedIndex == -1) ok = false;
            if (txTitulo.Text.Length==0) ok = false;
            btOK.Enabled = ok;
        }

        private void txTelefone_TextChanged(object sender, EventArgs e)
        {
            VeSeHab();
        }

        private void cmbEmails_KeyUp(object sender, KeyEventArgs e)
        {
            MostraEmail();
        }
    }

}