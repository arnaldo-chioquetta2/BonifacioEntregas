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
            string usuarioNormalizado = user.Trim();
            glo.Loga("LOGIN - início da tentativa");
            glo.Loga("LOGIN - usuário informado: " + usuarioNormalizado);
            glo.Loga("LOGIN - Banco utilizado: " + (glo.ODBC ? "DSN=MbCarros" : glo.CaminhoBase));
            glo.Loga("LOGIN - Modo conexão: " + (glo.ODBC ? "ODBC" : "OleDb"));

            string senha = Cripto.Encrypt(txSenha.Text);
            glo.Loga("LOGIN - senha informada foi criptografada");
            glo.Loga("LOGIN - tamanho do valor criptografado: " + senha.Length);

            try
            {
                DataTable usuarios = DB.ExecutarConsulta(
                    $"Select Nro, Nome, Usuario, Senha, Nivel From Vendedores Where Usuario = '{user}'");

                glo.Loga("LOGIN - quantidade de registros para " + usuarioNormalizado + ": " + usuarios.Rows.Count);
                bool senhaCorrespondente = false;
                foreach (DataRow usuario in usuarios.Rows)
                {
                    string usuarioBanco = Convert.ToString(usuario["Usuario"]);
                    bool possuiEspacosExtras = usuarioBanco != usuarioBanco.Trim();
                    senhaCorrespondente = senhaCorrespondente ||
                        string.Equals(Convert.ToString(usuario["Senha"]), senha, StringComparison.Ordinal);

                    glo.Loga(
                        "LOGIN - usuário " + usuarioNormalizado +
                        " encontrado: SIM; Nro: " + Convert.ToString(usuario["Nro"]) +
                        "; Nome: " + Convert.ToString(usuario["Nome"]) +
                        "; Nivel cadastrado: " + Convert.ToString(usuario["Nivel"]) +
                        "; espaços extras no Usuario: " + (possuiEspacosExtras ? "SIM" : "NÃO") +
                        "; Usuario normalizado: " + usuarioBanco.Trim());
                }

                if (usuarios.Rows.Count == 0)
                {
                    glo.Loga("LOGIN - usuário " + usuarioNormalizado + " encontrado: NÃO");
                }

                glo.Loga("LOGIN - senha criptografada corresponde ao cadastro: " +
                    (senhaCorrespondente ? "SIM" : "NÃO"));
            }
            catch (Exception exDiagnostico)
            {
                glo.Loga("LOGIN - falha na consulta diagnóstica: " + exDiagnostico.GetType().Name);
            }

            glo.Loga("LOGIN - executando consulta de autenticação em Vendedores");
            string SQL = $"Select Nro, Usuario From Vendedores Where Usuario = '{user}' and Senha = '{senha}' ";
            DataTable dados = DB.ExecutarConsulta(SQL);
            glo.Loga("LOGIN - consulta original encontrou registro: " + (dados.Rows.Count > 0 ? "SIM" : "NÃO"));
            if (dados.Rows.Count == 0)
            {
                glo.Loga("LOGIN - autenticação: FALHOU");
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
                glo.Loga("LOGIN - autenticação: OK; nivel da sessão: " + glo.Nivel);
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

                // glo.Nivel = 0;  // Balcão
                // glo.Nivel = 1;  // Caixa
                glo.Nivel = 2;  // Escritório

                //glo.iUsuario = 4;
                glo.iUsuario = 1;

                Form1 Form = new Form1();
                Form.Show();
                this.Visible = false;
            }
        }

    }
}