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
            glo.ExecutarComandoSQL(sql);
            this.Close();
        }
    }
}
