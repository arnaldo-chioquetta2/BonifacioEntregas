// #define ODBC

using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.gen;

namespace TeleBonifacio
{
    public partial class operLogin : Form        
    {

        public operLogin()
        {
            InitializeComponent();
            INI2 cINI2 = new INI2();
            string sOdbc = cINI2.ReadString("Usuario", "ODBC", "0");
            if (sOdbc=="1")
            {
                glo.ODBC = true;
            }
            INI cINI = new INI();
            glo.Adaptar = cINI.ReadString("Config", "Adaptar", "") == "1";
            glo.AdjustFormComponents(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Busca();
        }

        private void Busca()
        {
            string user = txYser.Text;
            string senha = Cripto.Encrypt(txSenha.Text);
            string SQL = $"Select Nivel, Nro From Vendedores Where Usuario = '{user}' and Senha = '{senha}' ";
            DataTable dados = DB.ExecutarConsulta(SQL);
            if (dados.Rows.Count == 0)
            {
                MessageBox.Show("Usuário não reconhecido", "Login Inválido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataRow Row = dados.Rows[0];
                glo.Nivel = Convert.ToInt16(Row["Nivel"]);
                glo.iUsuario = Convert.ToInt16(Row["Nro"]);
                Form1 Form = new Form1();
                Form.Show();
                this.Visible = false;
            }
        }
        private void txSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Busca();
            }
        }

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                glo.Nivel = 2;
                glo.iUsuario = 1;
                Form1 Form = new Form1();
                Form.Show();
                this.Visible = false;
            }
        }
    }    
}
