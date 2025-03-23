using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class fCadClientes : FormBase
    {

        private tb.Cliente clienteEspecifico;

        private bool Adicionando=false;
        public fCadClientes()
        {
            InitializeComponent();
            base.DAO = new dao.ClienteDAO();
            clienteEspecifico = DAO.GetUltimo() as tb.Cliente;
            base.reg = DAO.GetUltimo() as tb.Cliente;
            base.Mostra();
            base.LerTagsDosCamposDeTexto();
            rt.AdjustFormComponents(this);
        }

        private void cntrole1_AcaoRealizada(object sender, AcaoEventArgs e)
        {
            base.cntrole1_AcaoRealizada(sender, e, clienteEspecifico);
            if (Adicionando)
            {
                switch (e.Acao)
                {
                    case "CANC":
                        Cancela();
                        break;
                    case "OK":
                        Grava();
                        base.reg = DAO.GetUltimo() as tb.Cliente;
                        glo.IdAdicionado = base.reg.Id;
                        this.Close();
                        break;
                }
            }
        }

        private void fCadClientes_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && !(e.Control && e.KeyCode == Keys.C) && !(e.Control && e.KeyCode == Keys.A))
            {
                if (e.KeyCode == Keys.Escape)
                {
                    base.Cancela();
                }
                else
                {
                    if (!base.Pesquisando)
                    {
                        base.cntrole1.EmEdicao = true;
                    }
                }
            }
        }

        private void cntrole1_Load(object sender, System.EventArgs e)
        {

        }

        private void txtTelefone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.Equals(e.KeyChar, '-') && !char.Equals(e.KeyChar, '/') && !char.Equals(e.KeyChar, '(') && !char.Equals(e.KeyChar, ')'))
            {
                e.Handled = true;
            }
        }

        private void fCadClientes_Activated(object sender, EventArgs e)
        {
            if (glo.IdAdicionado == -1)
            {
                glo.IdAdicionado = 0;
                Adicionando = true;
                base.Adicionar();                
            }
        }

        private void txtNrOutro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) )
            {
                e.Handled = true;
            }
        }

        #region Vale        

        private void btVale_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.Length==0)
            {
                MessageBox.Show("É necessário informar o cliente");
            } else
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Informe o valor do vale");
                if (input.Length > 0)
                {
                    float valor;
                    if (float.TryParse(input, out valor))
                    {
                        PrintDocument printDoc = new PrintDocument();
                        printDoc.PrintPage += (s, ev) => PrintVale(ev, txtNome.Text, valor);
                        PrintDialog printDialog = new PrintDialog();
                        printDialog.Document = printDoc;
                        if (printDialog.ShowDialog() == DialogResult.OK)
                        {
                            printDoc.Print();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Valor inválido! Insira um valor válido.");
                    }
                }
            }
        }

        private void PrintVale(PrintPageEventArgs e, string cliente, float valor)
        {
            Graphics g = e.Graphics;
            Font fonteTitulo = new Font("Arial", 12, FontStyle.Bold);
            Font fonteTexto = new Font("Arial", 10, FontStyle.Regular);
            Brush corTexto = Brushes.Black;
            float linhaAltura = fonteTexto.GetHeight(g);
            float margemEsquerda = e.MarginBounds.Left;
            float larguraPagina = e.MarginBounds.Width;
            float margemSuperior = e.MarginBounds.Top;
            float posY = margemSuperior;
            string cabecalho = "BONIFACIO COMERCIO 51-984-10-8208";
            string titulo = "VALE REFERENTE A TROCA OU DEVOLUÇÃO";
            SizeF tamanhoCabecalho = g.MeasureString(cabecalho, fonteTitulo);
            SizeF tamanhoTitulo = g.MeasureString(titulo, fonteTitulo);
            g.DrawString(cabecalho, fonteTitulo, corTexto, margemEsquerda + 25, posY);
            posY += linhaAltura;
            g.DrawString(titulo, fonteTitulo, corTexto, margemEsquerda, posY);
            posY += linhaAltura * 2; 
            g.DrawString($"VALOR: R$ {valor:0.00}", fonteTexto, corTexto, margemEsquerda, posY);
            posY += linhaAltura;
            g.DrawString($"NOME: {cliente}", fonteTexto, corTexto, margemEsquerda, posY);
            posY += linhaAltura;
            g.DrawString($"DATA: {DateTime.Now.ToShortDateString()}", fonteTexto, corTexto, margemEsquerda, posY);
            posY += linhaAltura * 2; 
            g.DrawString("ASSINATURA DO GERENTE", fonteTexto, corTexto, margemEsquerda, posY);
            posY += linhaAltura * 2; 
            g.DrawString("ESTE PAPEL É SEU DINHEIRO ENTÃO NÃO PERCA", fonteTexto, corTexto, margemEsquerda, posY);
        }

        #endregion
    }
}
