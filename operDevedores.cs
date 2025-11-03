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
        private DataTable dadosCli;
        public bool OK = false;
        private int ID=0;
        private int ClienteLocalizado = 0;
        // private bool fechando=false;
        private bool PassouPeloCliente=false;
        private string OldNrOutro="";

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
            //this.fechando = true;
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
                string nrOutroDigitado = txNrOutro.Text.Trim();

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
                    if (nrCli == 0)
                    {
                        // Verifica duplicidade de NrOutro antes de inserir novo cliente
                        if (!string.IsNullOrEmpty(nrOutroDigitado))
                        {
                            tb.Cliente clienteExistente = (tb.Cliente)Cliente.GetPeloNrOutro(nrOutroDigitado);
                            if (clienteExistente != null)
                            {
                                MessageBox.Show($"⚠ Este número já está em uso pelo cliente:\n\n{clienteExistente.Nome}", "NrOutro já utilizado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txNrOutro.Focus();
                                txNrOutro.SelectAll();
                                return;
                            }
                        }

                        // Insere novo cliente com NrOutro
                        nrCli = clienteDAO.InserirNovoCliente(nomeCliente, nrOutroDigitado);
                        if (nrCli <= 0)
                        {
                            MessageBox.Show("Erro ao adicionar novo cliente.");
                            return;
                        }
                    }
                }

                // Atualiza NrOutro caso esteja em branco
                if (nrCli > 0 && string.IsNullOrWhiteSpace(this.OldNrOutro) && !string.IsNullOrWhiteSpace(nrOutroDigitado))
                {
                    tb.Cliente clienteExistente = (tb.Cliente)Cliente.GetPeloNrOutro(nrOutroDigitado);
                    if (clienteExistente != null && clienteExistente.Id != nrCli)
                    {
                        MessageBox.Show($"⚠ Este número já está em uso por outro cliente:\n\n{clienteExistente.Nome}", "NrOutro já utilizado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txNrOutro.Focus();
                        txNrOutro.SelectAll();
                        return;
                    }

                    clienteDAO.AtualizarNrOutro(nrCli, nrOutroDigitado);
                }

                // Captura o status
                int status = 1;
                string statusTexto = cmbStatus.Text.Trim();
                switch (statusTexto)
                {
                    case "Aberto": status = 1; break;
                    case "Atrasado": status = 2; break;
                    case "Pago": status = 3; break;
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

        //private void btOK_Click_1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string nomeCliente = cmbCliente.Text.Trim();
        //        string nota = txNota.Text.Trim();
        //        string observacao = txObs.Text.Trim();
        //        DateTime dataCompra = dtpCompra.Value.Date;
        //        DateTime vencimento = dtpVencimento.Value.Date;
        //        decimal valor = (decimal)glo.LeValor(txValor.Text);
        //        string nrOutroDigitado = txNrOutro.Text.Trim();

        //        // Verifica ou insere cliente
        //        int nrCli;
        //        ClienteDAO clienteDAO = new ClienteDAO();
        //        if (cmbCliente.SelectedItem is tb.ComboBoxItem itemSelecionado)
        //        {
        //            nrCli = itemSelecionado.Id;
        //        }
        //        else
        //        {
        //            nrCli = clienteDAO.GetIDPeloNome(nomeCliente);
        //            if (nrCli == 0)
        //            {
        //                // Verifica duplicidade de NrOutro
        //                if (!string.IsNullOrEmpty(nrOutroDigitado))
        //                {
        //                    tb.Cliente clienteExistente = (tb.Cliente)Cliente.GetPeloNrOutro(nrOutroDigitado);
        //                    if (clienteExistente != null)
        //                    {
        //                        MessageBox.Show($"⚠ Este número já está em uso pelo cliente:\n\n{clienteExistente.Nome}", "NrOutro já utilizado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                        txNrOutro.Focus();
        //                        txNrOutro.SelectAll();
        //                        return; // Interrompe a gravação
        //                    }
        //                }

        //                // Insere novo cliente com NrOutro informado
        //                nrCli = clienteDAO.InserirNovoCliente(nomeCliente, nrOutro: nrOutroDigitado);
        //                if (nrCli <= 0)
        //                {
        //                    MessageBox.Show("Erro ao adicionar novo cliente.");
        //                    return;
        //                }
        //            }
        //        }

        //        // 🟦 Captura o status selecionado
        //        int status = 1; // valor padrão
        //        string statusTexto = cmbStatus.Text.Trim();

        //        switch (statusTexto)
        //        {
        //            case "Aberto": status = 1; break;
        //            case "Atrasado": status = 2; break;
        //            case "Pago": status = 3; break;
        //        }

        //        // Grava o devedor
        //        if (this.ID == 0)
        //        {
        //            devDao.Adiciona(nrCli, status, dataCompra, vencimento, nota, observacao, valor);
        //        }
        //        else
        //        {
        //            devDao.Edita(this.ID, nrCli, dataCompra, status, vencimento, nota, observacao, valor);
        //        }

        //        this.OK = true;
        //        this.DialogResult = DialogResult.OK;
        //        this.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Erro ao gravar devedor: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void operDevedores_Activated(object sender, EventArgs e)
        {
            if ((this.ID==0) && (ClienteLocalizado==0) && (!this.PassouPeloCliente) )
            {
                this.Cursor = Cursors.WaitCursor;
                Cliente = new ClienteDAO();
                ZeraTela();
                glo.CarregarComboBoxU<tb.Cliente>(cmbCliente, dadosCli, "ESCOLHA ou digite um novo"); 
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
            this.OldNrOutro = "";
            this.PassouPeloCliente = true;
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
                            cmbCliente.SelectedItem = item.Nome;
                            break;
                        }
                    }
                    ClienteLocalizado = reg.Id;
                    txNrOutro.Text = texto;
                    txNrOutro.TabStop = false;
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
                        if (reg != null)
                        {
                            try
                            {
                                this.OldNrOutro = reg.NrOutro;
                                txNrOutro.Text = reg.NrOutro;
                                txNrOutro.TabStop = false;
                            }
                            catch (Exception)
                            {
                                return;
                            }
                            break;
                        }
                    }
                }
            }
            if (this.OldNrOutro == "")
            {
                txNrOutro.TabStop = true;
                txNrOutro.ReadOnly = false;
                txNrOutro.Focus();
            }
        }

        private void cmbCliente_Enter(object sender, EventArgs e)
        {
            cmbCliente.SelectAll();
        }

        private void cmbCliente_Click(object sender, EventArgs e)
        {
            cmbCliente.SelectAll();
        }

        private void operDevedores_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.fechando = true;
        }

        public void RecebeDadosCli(ref DataTable dadosC)
        {
            dadosCli = dadosC;
        }

        public void ZeraTela()
        {
            cmbCliente.SelectedItem = -1;
            cmbStatus.SelectedItem = 0;
            cmbCliente.Text = "ESCOLHA ou digite um novo";
            txNota.Text = "";
            txObs.Text = "";
            txValor.Text = "";
            txNrOutro.Text = "";
            txNrOutro.ReadOnly = true;
            txNrOutro.TabStop = false;
        }
    }
}
