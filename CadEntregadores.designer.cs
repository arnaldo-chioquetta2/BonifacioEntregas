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
            this.txtCPF = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtEndereco = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cntrole1
            // 
            this.cntrole1.Size = new System.Drawing.Size(342, 54);
            this.cntrole1.AcaoRealizada += new System.EventHandler<AcaoEventArgs>(this.cntrole1_AcaoRealizada_1);
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(12, 57);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(35, 13);
            this.lblNome.TabIndex = 0;
            this.lblNome.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.Location = new System.Drawing.Point(12, 77);
            this.txtNome.MaxLength = 100;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(250, 23);
            this.txtNome.TabIndex = 0;
            this.txtNome.Tag = "O";
            // 
            // lblTelefone
            // 
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Location = new System.Drawing.Point(12, 199);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(49, 13);
            this.lblTelefone.TabIndex = 2;
            this.lblTelefone.Text = "Telefone";
            // 
            // txtTelefone
            // 
            this.txtTelefone.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefone.Location = new System.Drawing.Point(12, 215);
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
            this.lblCNH.Location = new System.Drawing.Point(12, 254);
            this.lblCNH.Name = "lblCNH";
            this.lblCNH.Size = new System.Drawing.Size(30, 13);
            this.lblCNH.TabIndex = 6;
            this.lblCNH.Text = "CNH";
            // 
            // txtCNH
            // 
            this.txtCNH.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCNH.Location = new System.Drawing.Point(12, 270);
            this.txtCNH.MaxLength = 12;
            this.txtCNH.Name = "txtCNH";
            this.txtCNH.Size = new System.Drawing.Size(108, 23);
            this.txtCNH.TabIndex = 2;
            this.txtCNH.Tag = "";
            // 
            // lblValidadeCNH
            // 
            this.lblValidadeCNH.AutoSize = true;
            this.lblValidadeCNH.Location = new System.Drawing.Point(218, 252);
            this.lblValidadeCNH.Name = "lblValidadeCNH";
            this.lblValidadeCNH.Size = new System.Drawing.Size(74, 13);
            this.lblValidadeCNH.TabIndex = 8;
            this.lblValidadeCNH.Text = "Validade CNH";
            // 
            // dtpDataValidadeCNH
            // 
            this.dtpDataValidadeCNH.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataValidadeCNH.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataValidadeCNH.Location = new System.Drawing.Point(218, 272);
            this.dtpDataValidadeCNH.Name = "dtpDataValidadeCNH";
            this.dtpDataValidadeCNH.Size = new System.Drawing.Size(100, 23);
            this.dtpDataValidadeCNH.TabIndex = 3;
            this.dtpDataValidadeCNH.Tag = "H";
            this.dtpDataValidadeCNH.ValueChanged += new System.EventHandler(this.dtpValidadeCNH_ValueChanged);
            // 
            // txtCPF
            // 
            this.txtCPF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtCPF.Location = new System.Drawing.Point(12, 126);
            this.txtCPF.MaxLength = 14;
            this.txtCPF.Name = "txtCPF";
            this.txtCPF.Size = new System.Drawing.Size(150, 23);
            this.txtCPF.TabIndex = 34;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 104);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 13);
            this.label18.TabIndex = 35;
            this.label18.Text = "CPF/CNPj";
            // 
            // txtEndereco
            // 
            this.txtEndereco.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtEndereco.Location = new System.Drawing.Point(12, 173);
            this.txtEndereco.MaxLength = 14;
            this.txtEndereco.Name = "txtEndereco";
            this.txtEndereco.Size = new System.Drawing.Size(306, 23);
            this.txtEndereco.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Endereço";
            // 
            // fCadEntregadores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 325);
            this.Controls.Add(this.txtEndereco);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCPF);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblTelefone);
            this.Controls.Add(this.txtTelefone);
            this.Controls.Add(this.lblCNH);
            this.Controls.Add(this.txtCNH);
            this.Controls.Add(this.lblValidadeCNH);
            this.Controls.Add(this.dtpDataValidadeCNH);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.Controls.SetChildIndex(this.label18, 0);
            this.Controls.SetChildIndex(this.txtCPF, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtEndereco, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtCPF;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtEndereco;
        private System.Windows.Forms.Label label1;
    }
}
