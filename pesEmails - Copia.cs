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
            catch (Exception ex)
            {
                glo.Loga("Erro em pesEmails_Load");
                glo.Loga(ex.ToString());
            }
            CarregarComboBox();
            carregando = false;
        }

        private async void btOK_Click(object sender, EventArgs e)
        {
            glo.Loga("btOK_Click chamado na tela de emails.");

            string Texto = txTexto.Text;
            glo.Loga($"Texto capturado do campo de texto: {(Texto.Length > 0 ? "Texto preenchido" : "Texto vazio")}");

            if (Texto.Length == 0)
            {
                MessageBox.Show("É necessário um conteúdo no email", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                glo.Loga("Erro: O conteúdo do email está vazio.");
            }
            else
            {
                try
                {
                    Cursor = Cursors.WaitCursor; // Define o cursor como ampulheta
                    this.Text = "Enviando Email";
                    glo.Loga("Iniciando processo de envio de email.");

                    // Gravar texto no arquivo
                    File.WriteAllText(ArquivoEmail, Texto);
                    glo.Loga($"Texto gravado no arquivo: {ArquivoEmail}");

                    // Salvar título no arquivo de configuração
                    cINI.WriteString("Email", "Titulo", txTitulo.Text);
                    glo.Loga($"Título do email salvo: {txTitulo.Text}");

                    string destinatario = "";
                    string Remetente = "";
                    string senha = "";
                    bool TESTE = false;
                    glo.Loga($"Modo de teste: {TESTE}");

                    if (TESTE)
                    {
                        Remetente = "xeviousbr@gmail.com";
                        destinatario = "dayse.chioquetta@gmail.com";
                        senha = "uhkikktxafjvpwem";
                        glo.Loga($"Modo de teste ativo. Remetente: {Remetente}, Destinatário: {destinatario}");
                    }
                    else
                    {
                        Remetente = cINI.ReadString("Email", "Remetente", "");
                        destinatario = lbEmail.Text;
                        glo.Loga($"Remetente lido do arquivo de configuração: {Remetente}");
                        glo.Loga($"Destinatário capturado do label: {destinatario}");

                        string senhaCri = cINI.ReadString("Email", "senha", "");
                        if (senhaCri.Length > 0)
                        {
                            senha = Cripto.Decrypt(senhaCri);
                            glo.Loga("Senha descriptografada com sucesso.");
                        }
                        else
                        {
                            glo.Loga("Aviso: Senha não encontrada no arquivo de configuração.");
                        }

                        // Senha padrão para teste
                        senha = "bzbdmrviekwmamcy";
                        glo.Loga("Senha definida manualmente para teste.");
                    }

                    Email cEmail = new Email();
                    glo.Loga("Instância da classe Email criada.");

                    bool enviado = cEmail.EnviarEmail(
                        Remetente,
                        Remetente,
                        senha,
                        destinatario,
                        Arquivo,
                        txTitulo.Text,
                        Texto
                    );
                    glo.Loga($"Resultado do envio de email: {(enviado ? "Sucesso" : "Falha")}");

                    if (enviado)
                    {
                        glo.Loga("Email enviado com sucesso.");
                        Cursor = Cursors.Default;
                        this.Close();
                    }
                    else
                    {
                        glo.Loga("Erro: O email não foi enviado.");
                        this.Text = "Email não foi enviado";
                    }
                }
                catch (Exception ex)
                {
                    glo.Loga($"Erro durante o envio de email: {ex.Message}");
                }
                finally
                {
                    Cursor = Cursors.Default; // Restaura o cursor padrão
                    glo.Loga("Processo concluído. Cursor restaurado para padrão.");
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