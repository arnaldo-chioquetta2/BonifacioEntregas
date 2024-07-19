using TeleBonifacio.tb;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SourceGrid;
using System.Data;
using System.Linq;

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
            glo.AdjustFormComponents(this);
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
    }
}
