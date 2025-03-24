using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class operDevedores : Form
    {
        private ClienteDAO Cliente;
        private DevedoresDao devDao;
        public bool OK = false;
        private int ID=0;
        private int ClienteLocalizado = 0;

        public operDevedores()
        {
            InitializeComponent();
            cmbStatus.Items.AddRange(new string[] { "Aberto", "Pago", "Atrasado" });
            cmbStatus.SelectedIndex = 0;
            rt.AdjustFormComponents(this);
        }

        private void operDevedores_Load(object sender, EventArgs e)
        {
            dtpVencimento.Value = DateTime.Today.AddDays(7);
            devDao = new DevedoresDao();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            ClienteLocalizado = ((tb.ComboBoxItem)cmbCliente.SelectedItem).Id;
            DateTime vencimento = dtpVencimento.Value;
            DateTime Compra = dtpCompra.Value;            
            int status = cmbStatus.SelectedIndex;
            string nota = txNota.Text;
            string observacao = txObs.Text;            
            decimal valor= (decimal)glo.LeValor(txValor.Text);
            if (ClienteLocalizado<1)
            {
                ClienteLocalizado = Cliente.InserirNovoCliente(cmbCliente.Text, "");
                glo.IdAdicionado = ClienteLocalizado;
            }
            devDao.Adiciona(ClienteLocalizado, status, Compra, vencimento, nota, observacao, valor);
            this.OK = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btFechar_Click(object sender, EventArgs e)
        {
            this.OK = false;
            this.Close();
        }

        private void btFechar_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btOK_Click_1(object sender, EventArgs e)
        {
            try
            {
                string nomeCliente = cmbCliente.Text.Trim();
                string nota = txNota.Text.Trim();
                string observacao = txObs.Text.Trim();
                DateTime dataCompra = dtpCompra.Value.Date;
                DateTime vencimento = dtpVencimento.Value.Date;
                decimal valor = (decimal)glo.LeValor(txValor.Text);

                // Verifica ou insere cliente
                int nrCli;
                ClienteDAO clienteDAO = new ClienteDAO();
                if (cmbCliente.SelectedItem is tb.ComboBoxItem itemSelecionado)
                {
                    nrCli = itemSelecionado.Id;
                }
                else
                {
                    nrCli = clienteDAO.GetIDPeloNome(nomeCliente);
                    if (nrCli==0)
                    {
                        nrCli = clienteDAO.InserirNovoCliente(nomeCliente, nrOutro: txNrOutro.Text);
                    }                    
                }

                // 🟦 Captura o status selecionado
                int status = 1; // valor padrão

                string statusTexto = cmbStatus.Text.Trim(); // ou cmbStatus.SelectedItem.ToString()

                switch (statusTexto)
                {
                    case "Aberto":
                        status = 1;
                        break;
                    case "Atrasado":
                        status = 2;
                        break;
                    case "Pago":
                        status = 3;
                        break;
                    default:
                        status = 1; // valor padrão se algo der errado
                        break;
                }

                // Grava o devedor
                if (this.ID == 0)
                {
                    devDao.Adiciona(nrCli, status, dataCompra, vencimento, nota, observacao, valor);
                }
                else
                {
                    devDao.Edita(this.ID, nrCli, dataCompra, status, vencimento, nota, observacao, valor);
                }

                this.OK = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gravar devedor: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void operDevedores_Activated(object sender, EventArgs e)
        {
            if (this.ID==0)
            {
                this.Cursor = Cursors.WaitCursor;
                Cliente = new ClienteDAO();
                glo.CarregarComboBox<tb.Cliente>(cmbCliente, Cliente, "ESCOLHA ou digite um novo");                
                this.Cursor = Cursors.Default;
            }
        }

        public void CarregarDados(int id, int idCliente, string nome, DateTime data, DateTime vencimento, int status, string nota, string observacao, decimal valor)
        {
            this.ID = id;
            this.ClienteLocalizado = idCliente;
            cmbCliente.Text = nome;
            dtpCompra.Value = data;
            dtpVencimento.Value = vencimento;
            txNota.Text = nota;
            txObs.Text = observacao;
            txValor.Text = valor.ToString("N2");
            string statusTexto = "";

            switch (status)
            {
                case 1: statusTexto = "Aberto"; break;
                case 2: statusTexto = "Atrasado"; break;
                case 3: statusTexto = "Pago"; break;
            }
            cmbStatus.Text = statusTexto;
            btExcluir.Enabled = true;
        }

        private void btExcluir_Click(object sender, EventArgs e)
        {            
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir a dívida?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                devDao.Exclui(this.ID);
                this.ID = 0;
                this.OK = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        internal int getID()
        {
            return this.ID;
        }

        private void cmbCliente_Leave(object sender, EventArgs e)
        {
            string texto = cmbCliente.Text.Trim();

            // Se for número, tenta localizar pelo código NrOutro
            if (int.TryParse(texto, out int numeroCliente))
            {
                tb.Cliente reg = (tb.Cliente)Cliente.GetPeloNrOutro(texto);
                if (reg != null)
                {
                    cmbCliente.Text = reg.Nome;
                    foreach (tb.ComboBoxItem item in cmbCliente.Items)
                    {
                        if (item.Id == reg.Id)
                        {
                            cmbCliente.SelectedItem = item;
                            break;
                        }
                    }
                    ClienteLocalizado = reg.Id;
                    txNrOutro.Text = texto;
                }
                else
                {
                    MessageBox.Show("Cliente não encontrado.");
                    cmbCliente.Focus();
                }
            }
            else
            {
                // Verifica se o texto digitado está em algum item da lista
                bool encontrado = false;
                foreach (tb.ComboBoxItem item in cmbCliente.Items)
                {
                    if (string.Equals(item.Nome, texto, StringComparison.CurrentCultureIgnoreCase))
                    {
                        encontrado = true;
                        ClienteLocalizado = item.Id;
                        tb.Cliente reg = (tb.Cliente)Cliente.GetPeloID(item.Id.ToString());
                        txNrOutro.Text = reg.NrOutro;
                        break;
                    }
                }

                if (!encontrado)
                {
                    // Nome digitado não está na lista → novo cliente
                    txNrOutro.ReadOnly = false;
                    txNrOutro.Focus();
                }
            }
        }

        //private void cmbCliente_Leave(object sender, EventArgs e)
        //{
        //    string texto = cmbCliente.Text.Trim();
        //    if (int.TryParse(texto, out int numeroCliente))
        //    {
        //        tb.Cliente reg = (tb.Cliente)Cliente.GetPeloNrOutro(texto);
        //        if (reg != null)
        //        {
        //            cmbCliente.Text = reg.Nome;
        //            foreach (tb.ComboBoxItem item in cmbCliente.Items)
        //            {
        //                if (item.Id == reg.Id)
        //                {
        //                    cmbCliente.SelectedItem = item;
        //                    break;
        //                }
        //            }
        //            ClienteLocalizado = reg.Id;
        //            txNrOutro.Text = texto;
        //        }
        //        else
        //        {
        //            MessageBox.Show("Cliente não encontrado.");
        //            cmbCliente.Focus();
        //        }
        //    } else
        //    {
        //        int idCli = ((tb.ComboBoxItem)cmbCliente.SelectedItem).Id;
        //        tb.Cliente reg = (tb.Cliente)Cliente.GetPeloID(idCli.ToString());
        //        txNrOutro.Text = reg.NrOutro;
        //    }
        //}

        private void cmbCliente_Enter(object sender, EventArgs e)
        {
            cmbCliente.SelectAll();
        }

        private void cmbCliente_Click(object sender, EventArgs e)
        {
            cmbCliente.SelectAll();
        }
    }
}
