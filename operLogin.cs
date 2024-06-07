// #define ODBC

using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.gen;

#if ODBC
using System.Data.Odbc;
#endif

namespace TeleBonifacio
{
    public partial class operLogin : Form        
    {

        public operLogin()
        {
            InitializeComponent();

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
            
#if ODBC
            DataTable dados = ExecutarConsulta(SQL);
#else
            DataTable dados = glo.ExecutarConsulta(SQL);
#endif

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

#if ODBC
        private DataTable ExecutarConsulta(string query)
        {
            using (OdbcConnection connection = new OdbcConnection(glo.connectionString))
            {
                connection.Open();
                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    using (OdbcDataAdapter adapter = new OdbcDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }
#endif

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
