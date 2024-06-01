using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TeleBonifacio.gen;

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
            DataTable dados = glo.ExecutarConsulta($"Select Nivel From Vendedores Where Usuario = '{user}' and Senha = '{senha}' ");
            if (dados.Rows.Count == 0)
            {
                MessageBox.Show("Usuário não reconhecido", "Login Inválido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataRow Row = dados.Rows[0];
                glo.Nivel = Convert.ToInt16(Row["Nivel"]);
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
