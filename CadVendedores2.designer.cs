
namespace TeleBonifacio
{
    partial class CadVendedores2
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoja = new System.Windows.Forms.TextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.chkAtende = new System.Windows.Forms.CheckBox();
            this.txtNro = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cntrole1
            // 
            this.cntrole1.Size = new System.Drawing.Size(334, 54);
            this.cntrole1.AcaoRealizada += new System.EventHandler<AcaoEventArgs>(this.cntrole1_AcaoRealizada);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Loja";
            // 
            // txtLoja
            // 
            this.txtLoja.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoja.Location = new System.Drawing.Point(15, 135);
            this.txtLoja.MaxLength = 100;
            this.txtLoja.Name = "txtLoja";
            this.txtLoja.Size = new System.Drawing.Size(298, 23);
            this.txtLoja.TabIndex = 10;
            this.txtLoja.Tag = "O";
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(12, 65);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(35, 13);
            this.lblNome.TabIndex = 11;
            this.lblNome.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.Location = new System.Drawing.Point(12, 85);
            this.txtNome.MaxLength = 100;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(298, 23);
            this.txtNome.TabIndex = 9;
            this.txtNome.Tag = "O";
            // 
            // chkAtende
            // 
            this.chkAtende.AutoSize = true;
            this.chkAtende.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAtende.Location = new System.Drawing.Point(18, 174);
            this.chkAtende.Name = "chkAtende";
            this.chkAtende.Size = new System.Drawing.Size(101, 19);
            this.chkAtende.TabIndex = 13;
            this.chkAtende.Text = "Atende Whats";
            this.chkAtende.UseVisualStyleBackColor = true;
            this.chkAtende.CheckStateChanged += new System.EventHandler(this.chkAtende_CheckStateChanged);
            // 
            // txtNro
            // 
            this.txtNro.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNro.Location = new System.Drawing.Point(258, 172);
            this.txtNro.MaxLength = 100;
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(55, 23);
            this.txtNro.TabIndex = 14;
            this.txtNro.Tag = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Nro";
            // 
            // CadVendedores2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(334, 211);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNro);
            this.Controls.Add(this.chkAtende);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLoja);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Name = "CadVendedores2";
            this.Text = "Vendedores";
            this.Activated += new System.EventHandler(this.CadVendedores2_Activated);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CadVendedor_KeyUp);
            this.Controls.SetChildIndex(this.cntrole1, 0);
            this.Controls.SetChildIndex(this.txtNome, 0);
            this.Controls.SetChildIndex(this.lblNome, 0);
            this.Controls.SetChildIndex(this.txtLoja, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.chkAtende, 0);
            this.Controls.SetChildIndex(this.txtNro, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoja;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.CheckBox chkAtende;
        private System.Windows.Forms.TextBox txtNro;
        private System.Windows.Forms.Label label2;
    }
}
