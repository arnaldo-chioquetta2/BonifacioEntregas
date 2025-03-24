namespace TeleBonifacio
{
    partial class operDevedores
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpCompra = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpVencimento = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.txNota = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txObs = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btOK = new System.Windows.Forms.Button();
            this.btFechar = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.txValor = new System.Windows.Forms.TextBox();
            this.btExcluir = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txNrOutro = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmbCliente
            // 
            this.cmbCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cmbCliente.FormattingEnabled = true;
            this.cmbCliente.Location = new System.Drawing.Point(120, 20);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(300, 28);
            this.cmbCliente.TabIndex = 0;
            this.cmbCliente.Click += new System.EventHandler(this.cmbCliente_Click);
            this.cmbCliente.Enter += new System.EventHandler(this.cmbCliente_Enter);
            this.cmbCliente.Leave += new System.EventHandler(this.cmbCliente_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(20, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Cliente";
            // 
            // dtpCompra
            // 
            this.dtpCompra.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpCompra.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpCompra.Location = new System.Drawing.Point(120, 103);
            this.dtpCompra.Name = "dtpCompra";
            this.dtpCompra.Size = new System.Drawing.Size(120, 26);
            this.dtpCompra.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(20, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Compra";
            // 
            // cmbStatus
            // 
            this.cmbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(120, 188);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(120, 28);
            this.cmbStatus.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(20, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Status";
            // 
            // dtpVencimento
            // 
            this.dtpVencimento.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpVencimento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpVencimento.Location = new System.Drawing.Point(120, 233);
            this.dtpVencimento.Name = "dtpVencimento";
            this.dtpVencimento.Size = new System.Drawing.Size(120, 26);
            this.dtpVencimento.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(20, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Vencimento";
            // 
            // txNota
            // 
            this.txNota.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txNota.Location = new System.Drawing.Point(120, 273);
            this.txNota.Name = "txNota";
            this.txNota.Size = new System.Drawing.Size(120, 26);
            this.txNota.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(20, 276);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Nota";
            // 
            // txObs
            // 
            this.txObs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txObs.Location = new System.Drawing.Point(120, 313);
            this.txObs.Multiline = true;
            this.txObs.Name = "txObs";
            this.txObs.Size = new System.Drawing.Size(300, 80);
            this.txObs.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(20, 316);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Observação";
            // 
            // btOK
            // 
            this.btOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btOK.Location = new System.Drawing.Point(14, 411);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(100, 35);
            this.btOK.TabIndex = 7;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click_1);
            // 
            // btFechar
            // 
            this.btFechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btFechar.Location = new System.Drawing.Point(324, 409);
            this.btFechar.Name = "btFechar";
            this.btFechar.Size = new System.Drawing.Size(100, 35);
            this.btFechar.TabIndex = 8;
            this.btFechar.Text = "Fechar";
            this.btFechar.UseVisualStyleBackColor = true;
            this.btFechar.Click += new System.EventHandler(this.btFechar_Click_1);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label22.Location = new System.Drawing.Point(20, 148);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(46, 20);
            this.label22.TabIndex = 12;
            this.label22.Text = "Valor";
            // 
            // txValor
            // 
            this.txValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txValor.Location = new System.Drawing.Point(120, 145);
            this.txValor.Name = "txValor";
            this.txValor.Size = new System.Drawing.Size(120, 26);
            this.txValor.TabIndex = 2;
            // 
            // btExcluir
            // 
            this.btExcluir.Enabled = false;
            this.btExcluir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btExcluir.Location = new System.Drawing.Point(169, 409);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(100, 35);
            this.btExcluir.TabIndex = 13;
            this.btExcluir.Text = "Excluir";
            this.btExcluir.UseVisualStyleBackColor = true;
            this.btExcluir.Click += new System.EventHandler(this.btExcluir_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(20, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "NrOutro";
            // 
            // txNrOutro
            // 
            this.txNrOutro.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txNrOutro.Location = new System.Drawing.Point(120, 63);
            this.txNrOutro.Name = "txNrOutro";
            this.txNrOutro.ReadOnly = true;
            this.txNrOutro.Size = new System.Drawing.Size(120, 26);
            this.txNrOutro.TabIndex = 14;
            // 
            // operDevedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 459);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txNrOutro);
            this.Controls.Add(this.btExcluir);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.txValor);
            this.Controls.Add(this.btFechar);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txObs);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txNota);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpVencimento);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpCompra);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCliente);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "operDevedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cadastro de Devedor";
            this.Activated += new System.EventHandler(this.operDevedores_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.operDevedores_FormClosing);
            this.Load += new System.EventHandler(this.operDevedores_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpCompra;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpVencimento;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txNota;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txObs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btFechar;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txValor;
        private System.Windows.Forms.Button btExcluir;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txNrOutro;
    }
}
