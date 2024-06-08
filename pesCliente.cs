using System;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class pesCliente : Form        
    {
        public int ClienteLocalizado = 0;
        public string Nome = "";
        public string Fone = "";
        public bool OK = false;
        private bool JaCarregouClientes = false;
        private string ultimoTexto = "";
        private bool achou = false;
        private string telefone = "";
        private ClienteDAO Cliente;        

        public pesCliente()
        {
            InitializeComponent();
        }

        private void pesCliente_Load(object sender, EventArgs e)
        {
            if (JaCarregouClientes==false)
            {
                Cliente = new ClienteDAO();
                this.Cursor = Cursors.WaitCursor;
                glo.CarregarComboBox<tb.Cliente>(cmbCliente, Cliente);
                this.Cursor = Cursors.Default;
                dateTimePicker1.Value = DateTime.Now.AddDays(7);
                JaCarregouClientes = true;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (achou)
            {
                ClienteLocalizado = Convert.ToInt32(cmbCliente.SelectedValue);
                glo.IdAdicionado = ClienteLocalizado;
                if (telefone!= txTelefone.Text)
                {
                    Cliente.SetFone(txTelefone.Text);
                }
            } else
            {
                ClienteLocalizado = 0;
                Nome = cmbCliente.Text;
                Fone = txTelefone.Text;
            }
            this.OK = true;
            this.Visible = false;
        }        

        private void cmbCliente_TextChanged(object sender, EventArgs e)
        {
            string textoAtual = cmbCliente.Text;
            if (textoAtual.Length >= 3 && textoAtual != ultimoTexto)
            {
                ultimoTexto = textoAtual;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    telefone = Cliente.BuscarTelefonePorNomeParcial(textoAtual);
                    int nrCli = Cliente.getIdAtual();
                    txTelefone.Text = telefone;
                    achou = (telefone.Length > 0);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        public void SetDescricao(string Descricao, bool ProdNovo)
        {
            if (Descricao.Length > 0)
            {                
                txDescr.Text = Descricao;
                txDescr.Enabled = false;
            }
            else {
                if (ProdNovo)
                {
                    txDescr.Text = "";
                    txDescr.Enabled = true;
                } else
                {
                    txDescr.Text = "Vários";
                    txDescr.Enabled = false;
                }
            }
        }

        public string getDescricao()
        {
            return txDescr.Text;
        }

        public DateTime getDtAgora()
        {
            return dtpData.Value.Date;
        }

        public DateTime getDtEnc()
        {
            return dateTimePicker1.Value.Date;
        }
    }
}
