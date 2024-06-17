using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class pesCliente : Form        
    {
        private ClienteDAO Cliente;
        private DataTable dadosCli;
        public string Nome = "";
        public string Fone = "";
        private string ultimoTexto = "";
        private string telefone = "";
        public int ClienteLocalizado = 0;
        public bool OK = false;        
        private bool achou = false;        
        private bool carregando = false;
        private bool JaMostrouCombo = false;

        public pesCliente()
        {
            InitializeComponent();
        }

        private void CarregarComboBox<T>(ComboBox comboBox)
        {
            List<tb.ComboBoxItem> lista = new List<tb.ComboBoxItem>();
            foreach (DataRow row in dadosCli.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                tb.ComboBoxItem item = new tb.ComboBoxItem(id, nome);
                lista.Add(item);
            }
            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Nome";
            comboBox.ValueMember = "Id";
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
            carregando = false;
            this.Visible = false;
        }        

        private void cmbCliente_TextChanged(object sender, EventArgs e)
        {
            if (carregando==false)
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

        public void RecebeDadosCli(ref DataTable dados)
        {
            dadosCli = dados;
        }

        private void pesCliente_Activated(object sender, EventArgs e)
        {
            if (carregando==false)
            {
                carregando = true;
                if (JaMostrouCombo==false)
                {
                    // Cliente = new ClienteDAO();
                    this.Cursor = Cursors.WaitCursor;
                    CarregarComboBox<tb.Cliente>(cmbCliente);
                    dateTimePicker1.Value = DateTime.Now.AddDays(7);
                }
                this.Cursor = Cursors.Default;
                carregando = false;
            }
        }
    }
}
