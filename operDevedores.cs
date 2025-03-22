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
            rt.AdjustFormComponents(this);
        }

        private void operDevedores_Load(object sender, EventArgs e)
        {
            cmbStatus.Items.AddRange(new string[] { "Aberto", "Fechado", "Atrasado" });
            cmbStatus.SelectedIndex = 0;
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
                    nrCli = clienteDAO.InserirNovoCliente(nomeCliente, "");
                    if (nrCli <= 0)
                    {
                        MessageBox.Show("Erro ao adicionar novo cliente.");
                        return;
                    }
                }

                if (this.ID == 0)
                {
                    devDao.Adiciona(nrCli, 1, dataCompra, vencimento, nota, observacao, valor);
                }
                else
                {
                    devDao.Edita(this.ID, nrCli, dataCompra, 1, vencimento, nota, observacao, valor);
                }

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
    }
}
