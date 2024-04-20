using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class oprConfig : Form        
    {

        private INI cINI;

        public oprConfig()
        {
            InitializeComponent();
            textBox1.Text = glo.CaminhoBase;
            cINI = new INI();
            txNome.Text = cINI.ReadString("Identidade", "Nome", "");
            txEndereco.Text = cINI.ReadString("Identidade", "Endereco", "");
            txFone.Text = cINI.ReadString("Identidade", "Fone", "");
            string NovaVersao = cINI.ReadString("Config", "NovaVersao", "");
            if (NovaVersao.Length>0)
            {
                string VersaoAtual = cINI.ReadString("Config", "VersaoAtual", "");
                if (VersaoAtual!= NovaVersao)
                {
                    btAtu.Text = "Atualizar para versão " + NovaVersao;
                    btAtu.Visible = true;
                }
            }            
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
            cINI.WriteString("Identidade", "Nome", txNome.Text);
            cINI.WriteString("Identidade", "Endereco", txEndereco.Text);
            cINI.WriteString("Identidade", "Fone", txFone.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblNome_DoubleClick(object sender, EventArgs e)
        {

            operSQL foperSQL = new operSQL();
            foperSQL.Show();
        }

        private void btAtu_Click(object sender, EventArgs e)
        {
            string PastaAtu = cINI.ReadString("Config", "Atualizador", "");
            Process.Start(PastaAtu + @"\ATCAtualizeitor.exe");
            Environment.Exit(0);
        }
    }
}
