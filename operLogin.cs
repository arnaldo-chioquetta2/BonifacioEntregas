using System;
using System.Data;
using TeleBonifacio.gen;
using System.Windows.Forms;

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
            glo.LogRemoto = cINI.ReadString("Config", "LogRemoto", "") == "1";
            rt.AdjustFormComponents(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Busca();
        }

        private void Busca()
        {
            // Cada tentativa começa sem privilégios residuais de uma sessão anterior.
            glo.Nivel = glo.NIVEL_BALCONISTA;
            glo.iUsuario = 0;

            string user = txYser.Text;
            string senha = Cripto.Encrypt(txSenha.Text);
            string SQL = $"Select Nro, Usuario From Vendedores Where Usuario = '{user}' and Senha = '{senha}' ";
            DataTable dados = DB.ExecutarConsulta(SQL);
            if (dados.Rows.Count == 0)
            {
                MessageBox.Show("Usuário não reconhecido", "Login Inválido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataRow Row = dados.Rows[0];
                glo.iUsuario = Convert.ToInt16(Row["Nro"]);
                string usuarioAutenticado = Convert.ToString(Row["Usuario"]).Trim();

                if (string.Equals(usuarioAutenticado, glo.LOGIN_BALCAO, StringComparison.OrdinalIgnoreCase))
                {
                    glo.Nivel = glo.NIVEL_BALCONISTA;
                }
                else if (string.Equals(usuarioAutenticado, glo.LOGIN_ESCRITORIO, StringComparison.OrdinalIgnoreCase))
                {
                    glo.Nivel = glo.NIVEL_ESCRITORIO;
                }
                else
                {
                    glo.Nivel = glo.NIVEL_BALCONISTA;
                    glo.iUsuario = 0;
                    MessageBox.Show("Usuário não autorizado para acesso ao sistema.", "Login inválido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
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

    }
}