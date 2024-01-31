namespace TeleBonifacio
{
    partial class fCadEntregadores
    {
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblTelefone;
        private System.Windows.Forms.TextBox txtTelefone;
        private System.Windows.Forms.Label lblCNH;
        private System.Windows.Forms.TextBox txtCNH;
        private System.Windows.Forms.Label lblValidadeCNH;
        private System.Windows.Forms.DateTimePicker dtpDataValidadeCNH;


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fCadEntregadores));
            this.lblNome = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.txtTelefone = new System.Windows.Forms.TextBox();
            this.lblCNH = new System.Windows.Forms.Label();
            this.txtCNH = new System.Windows.Forms.TextBox();
            this.lblValidadeCNH = new System.Windows.Forms.Label();
            this.dtpDataValidadeCNH = new System.Windows.Forms.DateTimePicker();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cntrole1
            // 
            this.cntrole1.Size = new System.Drawing.Size(330, 54);
            this.cntrole1.AcaoRealizada += new System.EventHandler<AcaoEventArgs>(this.cntrole1_AcaoRealizada_1);
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(14, 93);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(35, 13);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.Location = new System.Drawing.Point(14, 113);
            this.txtNome.MaxLength = 100;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(250, 23);
            this.txtNome.TabIndex = 0;
            this.txtNome.Tag = "O";
            // 
            // lblTelefone
            // 
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Location = new System.Drawing.Point(14, 143);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(49, 13);
            this.lblTelefone.TabIndex = 2;
            this.lblTelefone.Text = "Telefone";
            // 
            // txtTelefone
            // 
            this.txtTelefone.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefone.Location = new System.Drawing.Point(14, 163);
            this.txtTelefone.MaxLength = 20;
            this.txtTelefone.Name = "txtTelefone";
            this.txtTelefone.Size = new System.Drawing.Size(108, 23);
            this.txtTelefone.TabIndex = 1;
            this.txtTelefone.Tag = "0";
            this.txtTelefone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTelefone_KeyPress);
            // 
            // lblCNH
            // 
            this.lblCNH.AutoSize = true;
            this.lblCNH.Location = new System.Drawing.Point(14, 198);
            this.lblCNH.Name = "lblCNH";
            this.lblCNH.Size = new System.Drawing.Size(30, 13);
            this.lblCNH.TabIndex = 6;
            this.lblCNH.Text = "CNH";
            // 
            // txtCNH
            // 
            this.txtCNH.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCNH.Location = new System.Drawing.Point(14, 218);
            this.txtCNH.MaxLength = 12;
            this.txtCNH.Name = "txtCNH";
            this.txtCNH.Size = new System.Drawing.Size(108, 23);
            this.txtCNH.TabIndex = 2;
            this.txtCNH.Tag = "";
            // 
            // lblValidadeCNH
            // 
            this.lblValidadeCNH.AutoSize = true;
            this.lblValidadeCNH.Location = new System.Drawing.Point(190, 198);
            this.lblValidadeCNH.Name = "lblValidadeCNH";
            this.lblValidadeCNH.Size = new System.Drawing.Size(74, 13);
            this.lblValidadeCNH.TabIndex = 8;
            this.lblValidadeCNH.Text = "Validade CNH";
            // 
            // dtpDataValidadeCNH
            // 
            this.dtpDataValidadeCNH.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataValidadeCNH.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataValidadeCNH.Location = new System.Drawing.Point(190, 218);
            this.dtpDataValidadeCNH.Name = "dtpDataValidadeCNH";
            this.dtpDataValidadeCNH.Size = new System.Drawing.Size(100, 23);
            this.dtpDataValidadeCNH.TabIndex = 3;
            this.dtpDataValidadeCNH.Tag = "H";
            this.dtpDataValidadeCNH.ValueChanged += new System.EventHandler(this.dtpValidadeCNH_ValueChanged);
            // 
            // txtId
            // 
            this.txtId.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtId.Location = new System.Drawing.Point(43, 60);
            this.txtId.MaxLength = 20;
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(61, 23);
            this.txtId.TabIndex = 14;
            this.txtId.Tag = "";
            this.txtId.Enter += new System.EventHandler(this.txtId_Enter);
            this.txtId.Leave += new System.EventHandler(this.txtId_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Nr:";
            // 
            // fCadEntregadores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 258);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblTelefone);
            this.Controls.Add(this.txtTelefone);
            this.Controls.Add(this.lblCNH);
            this.Controls.Add(this.txtCNH);
            this.Controls.Add(this.lblValidadeCNH);
            this.Controls.Add(this.dtpDataValidadeCNH);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "fCadEntregadores";
            this.Text = "Cadastro de Entregador";
            this.Activated += new System.EventHandler(this.fCadEntregadores_Activated);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Teclou);
            this.Controls.SetChildIndex(this.cntrole1, 0);
            this.Controls.SetChildIndex(this.dtpDataValidadeCNH, 0);
            this.Controls.SetChildIndex(this.lblValidadeCNH, 0);
            this.Controls.SetChildIndex(this.txtCNH, 0);
            this.Controls.SetChildIndex(this.lblCNH, 0);
            this.Controls.SetChildIndex(this.txtTelefone, 0);
            this.Controls.SetChildIndex(this.lblTelefone, 0);
            this.Controls.SetChildIndex(this.txtNome, 0);
            this.Controls.SetChildIndex(this.lblNome, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtId, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label3;
    }
}
