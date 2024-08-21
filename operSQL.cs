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
    public partial class operSQL : Form
    {
        public operSQL()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = textBox1.Text;

            // Dividir o comando SQL pelo caractere ';'
            string[] sqlCommands = sql.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in sqlCommands)
            {
                // Trim para garantir que não haja espaços em branco no início ou no fim do comando
                string trimmedCommand = command.Trim();

                if (!string.IsNullOrEmpty(trimmedCommand))
                {
                    DB.ExecutarComandoSQL(trimmedCommand);
                }
            }

            this.Close();
        }

    }
}
