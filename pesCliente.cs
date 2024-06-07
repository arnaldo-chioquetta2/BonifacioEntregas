using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class pesCliente : Form        
    {
        private bool JaCarregouClientes = false;

        public pesCliente()
        {
            InitializeComponent();
        }

        private void pesCliente_Load(object sender, EventArgs e)
        {
            if (JaCarregouClientes==false)
            {
                ClienteDAO Cliente = new ClienteDAO();
                this.Cursor = Cursors.WaitCursor;
                glo.CarregarComboBox<tb.Cliente>(cmbCliente, Cliente);
                this.Cursor = Cursors.Default;
                JaCarregouClientes = true;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            glo.IdAdicionado = Convert.ToInt32(cmbCliente.SelectedValue);
            this.Visible = false;
        }
    }    
}
