using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class oprConfig : Form        
    {

        private INI MeuIni;

        public oprConfig()
        {
            InitializeComponent();
            textBox1.Text = glo.CaminhoBase;
            MeuIni = new INI();
            txNome.Text = MeuIni.ReadString("Identidade", "Nome", "");
            txEndereco.Text = MeuIni.ReadString("Identidade", "Endereco", "");
            txFone.Text = MeuIni.ReadString("Identidade", "Fone", "");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = (openFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            glo.CaminhoBase = textBox1.Text;
            MeuIni.WriteString("Identidade", "Nome", txNome.Text);
            MeuIni.WriteString("Identidade", "Endereco", txEndereco.Text);
            MeuIni.WriteString("Identidade", "Fone", txFone.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblNome_DoubleClick(object sender, EventArgs e)
        {
            string sql = Microsoft.VisualBasic.Interaction.InputBox("Digite o comando SQL:", "Comando SQL", "", 0, 0);
            glo.ExecutarComandoSQL(sql);
            MessageBox.Show("O comando SQL foi executado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
