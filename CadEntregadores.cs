using System;
using TeleBonifacio.tb;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace TeleBonifacio
{
    public partial class fCadEntregadores : FormBase
    {
        private tb.Entregador clienteEspecifico;
        private bool txtIdDentro = false;

        public fCadEntregadores()
        {
            InitializeComponent();
            base.DAO = new dao.EntregadorDAO();
            clienteEspecifico = DAO.GetUltimo() as tb.Entregador;
            base.reg = DAO.GetUltimo() as tb.Entregador;
            base.Mostra();
            base.LerTagsDosCamposDeTexto();
            rt.AdjustFormComponents(this);
        }

        private void cntrole1_Load(object sender, EventArgs e)
        {

        }

        private void Teclou(object sender, KeyEventArgs e)
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
                        if (!this.txtIdDentro)
                        {
                            base.cntrole1.EmEdicao = true;
                        }
                    }
                }
            }
        }

        private void dtpValidadeCNH_ValueChanged(object sender, EventArgs e)
        {
            if (Mostrando == false)
            {
                base.cntrole1.EmEdicao = true;
                DateTimePicker picker = sender as DateTimePicker;
                if (picker != null)
                {
                    string propertyName = picker.Name.Substring(3); 
                    PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);
                    if (propertyInfo != null && (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?)))
                    {
                        if (picker.Value != DateTime.MinValue)
                        {
                            picker.Format = DateTimePickerFormat.Short;
                            propertyInfo.SetValue(reg, picker.Value, null);
                        }
                        else
                        {
                            picker.CustomFormat = " ";
                            picker.Format = DateTimePickerFormat.Custom;
                            propertyInfo.SetValue(reg, null, null);
                        }
                    }
                }
            }
        }

        private void cntrole1_AcaoRealizada_1(object sender, AcaoEventArgs e)
        {
            base.cntrole1_AcaoRealizada(sender, e, clienteEspecifico);
        }

        private void txtTelefone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.Equals(e.KeyChar, '-') && !char.Equals(e.KeyChar, '/') && !char.Equals(e.KeyChar, '(') && !char.Equals(e.KeyChar, ')'))
            {
                e.Handled = true;
            }
        }

        private void fCadEntregadores_Activated(object sender, EventArgs e)
        {
            
        }

        private void txtId_Enter(object sender, EventArgs e)
        {
            this.txtIdDentro = true;
        }

        private void txtId_Leave(object sender, EventArgs e)
        {
            this.txtIdDentro = false;
        }
    }

}
