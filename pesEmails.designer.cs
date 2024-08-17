
namespace TeleBonifacio
{
    partial class pesEmails
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbEmails = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txTitulo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txTexto = new System.Windows.Forms.TextBox();
            this.btFechar = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.lbEmail = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbEmails
            // 
            this.cmbEmails.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbEmails.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbEmails.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEmails.FormattingEnabled = true;
            this.cmbEmails.Location = new System.Drawing.Point(97, 6);
            this.cmbEmails.Name = "cmbEmails";
            this.cmbEmails.Size = new System.Drawing.Size(287, 28);
            this.cmbEmails.Sorted = true;
            this.cmbEmails.TabIndex = 36;
            this.cmbEmails.SelectedIndexChanged += new System.EventHandler(this.cmbEmails_SelectedIndexChanged);
            this.cmbEmails.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbEmails_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 37;
            this.label1.Text = "Destinatário";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(44, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 20);
            this.label2.TabIndex = 38;
            this.label2.Text = "Título";
            // 
            // txTitulo
            // 
            this.txTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txTitulo.Location = new System.Drawing.Point(97, 72);
            this.txTitulo.Name = "txTitulo";
            this.txTitulo.Size = new System.Drawing.Size(287, 26);
            this.txTitulo.TabIndex = 39;
            this.txTitulo.TextChanged += new System.EventHandler(this.txTelefone_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txTexto);
            this.groupBox1.Controls.Add(this.btFechar);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 253);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Assunto";
            // 
            // txTexto
            // 
            this.txTexto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txTexto.Location = new System.Drawing.Point(6, 22);
            this.txTexto.Multiline = true;
            this.txTexto.Name = "txTexto";
            this.txTexto.Size = new System.Drawing.Size(372, 171);
            this.txTexto.TabIndex = 50;
            // 
            // btFechar
            // 
            this.btFechar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btFechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btFechar.Location = new System.Drawing.Point(273, 199);
            this.btFechar.Name = "btFechar";
            this.btFechar.Size = new System.Drawing.Size(105, 39);
            this.btFechar.TabIndex = 49;
            this.btFechar.Text = "Fechar";
            this.btFechar.UseVisualStyleBackColor = true;
            this.btFechar.Click += new System.EventHandler(this.btFechar_Click);
            // 
            // btOK
            // 
            this.btOK.Enabled = false;
            this.btOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btOK.Location = new System.Drawing.Point(12, 303);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(105, 39);
            this.btOK.TabIndex = 48;
            this.btOK.Text = "Enviar";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // lbEmail
            // 
            this.lbEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEmail.Location = new System.Drawing.Point(8, 42);
            this.lbEmail.Name = "lbEmail";
            this.lbEmail.Size = new System.Drawing.Size(376, 20);
            this.lbEmail.TabIndex = 50;
            this.lbEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pesEmails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btFechar;
            this.ClientSize = new System.Drawing.Size(395, 352);
            this.Controls.Add(this.lbEmail);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txTitulo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbEmails);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "pesEmails";
            this.Text = "Enviar email";
            this.Load += new System.EventHandler(this.pesEmails_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEmails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txTitulo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btFechar;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label lbEmail;
        private System.Windows.Forms.TextBox txTexto;
    }
}

