
namespace TeleBonifacio
{
    partial class fCadTiposFaltas
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNome = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCor = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cntrole1
            // 
            this.cntrole1.Size = new System.Drawing.Size(348, 54);
            this.cntrole1.AcaoRealizada += new System.EventHandler<AcaoEventArgs>(this.cntrole1_AcaoRealizada);
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(12, 57);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(35, 13);
            this.lblNome.TabIndex = 6;
            this.lblNome.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.Location = new System.Drawing.Point(12, 77);
            this.txtNome.MaxLength = 100;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(298, 23);
            this.txtNome.TabIndex = 5;
            this.txtNome.Tag = "O";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Cor";
            // 
            // cmbCor
            // 
            this.cmbCor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCor.FormattingEnabled = true;
            this.cmbCor.Location = new System.Drawing.Point(15, 119);
            this.cmbCor.Name = "cmbCor";
            this.cmbCor.Size = new System.Drawing.Size(121, 24);
            this.cmbCor.TabIndex = 8;
            this.cmbCor.Tag = "cor";
            this.cmbCor.SelectedIndexChanged += new System.EventHandler(this.cmbCores_SelectedIndexChanged);
            // 
            // fCadTiposFaltas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(348, 169);
            this.Controls.Add(this.cmbCor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Name = "fCadTiposFaltas";
            this.Text = "Cadastro de Tipos";
            this.Activated += new System.EventHandler(this.fCadClientes_Activated);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.fCadClientes_KeyUp);
            this.Controls.SetChildIndex(this.cntrole1, 0);
            this.Controls.SetChildIndex(this.txtNome, 0);
            this.Controls.SetChildIndex(this.lblNome, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbCor, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCor;
    }
}
