using System;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class operGarantia : Form        
    {

        public operGarantia()
        {
            InitializeComponent();
            FornecedorDao Forn = new FornecedorDao();
            glo.CarregarComboBox<tb.Fornecedor>(cmbForn, Forn);
            DateTime Agora = DateTime.Now;
            dtpPrometda.Value = dtpFornec.Value = Agora.AddDays(30);
            rt.AdjustFormComponents(this);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            GarantiasDao cClaG = new GarantiasDao();
            int idForn = Convert.ToInt32(cmbForn.SelectedValue.ToString());
            cClaG.Adiciona(dtpData.Value, idForn, txTelefone.Text, dtpPrometda.Value, dtpFornec.Value);
            this.Close();
        }
    }    
}
