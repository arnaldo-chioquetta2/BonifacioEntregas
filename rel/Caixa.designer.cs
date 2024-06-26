
namespace TeleBonifacio.rel
{
    partial class Caixa
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btImprimir = new System.Windows.Forms.Button();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtnDtFim = new System.Windows.Forms.DateTimePicker();
            this.dtpDataIN = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(12, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(62, 24);
            this.lblTitulo.TabIndex = 8;
            this.lblTitulo.Text = "Caixa:";
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 49);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(975, 401);
            this.textBox1.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btImprimir);
            this.panel1.Controls.Add(this.btnFiltrar);
            this.panel1.Controls.Add(this.cmbTipo);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dtnDtFim);
            this.panel1.Controls.Add(this.dtpDataIN);
            this.panel1.Controls.Add(this.lblTitulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(975, 49);
            this.panel1.TabIndex = 10;
            // 
            // btImprimir
            // 
            this.btImprimir.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btImprimir.Location = new System.Drawing.Point(892, 12);
            this.btImprimir.Name = "btImprimir";
            this.btImprimir.Size = new System.Drawing.Size(75, 23);
            this.btImprimir.TabIndex = 47;
            this.btImprimir.Text = "Imprimir";
            this.btImprimir.Click += new System.EventHandler(this.btImprimir_Click);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnFiltrar.Location = new System.Drawing.Point(811, 12);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(75, 23);
            this.btnFiltrar.TabIndex = 46;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // cmbTipo
            // 
            this.cmbTipo.DisplayMember = "Nome";
            this.cmbTipo.Items.AddRange(new object[] {
            "Dinheiro",
            "Cartão",
            "Pix",
            "Despesa",
            "Itaú",
            "Sicred"});
            this.cmbTipo.Location = new System.Drawing.Point(374, 14);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(225, 21);
            this.cmbTipo.TabIndex = 45;
            this.cmbTipo.ValueMember = "Id";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(332, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 16);
            this.label5.TabIndex = 44;
            this.label5.Text = "Tipo";
            // 
            // dtnDtFim
            // 
            this.dtnDtFim.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtnDtFim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtnDtFim.Location = new System.Drawing.Point(217, 12);
            this.dtnDtFim.Name = "dtnDtFim";
            this.dtnDtFim.Size = new System.Drawing.Size(90, 23);
            this.dtnDtFim.TabIndex = 43;
            this.dtnDtFim.Tag = "H";
            // 
            // dtpDataIN
            // 
            this.dtpDataIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataIN.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataIN.Location = new System.Drawing.Point(95, 12);
            this.dtpDataIN.Name = "dtpDataIN";
            this.dtpDataIN.Size = new System.Drawing.Size(90, 23);
            this.dtpDataIN.TabIndex = 42;
            this.dtpDataIN.Tag = "H";
            // 
            // Caixa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "Caixa";
            this.Text = "Relatório de Caixa";
            this.Activated += new System.EventHandler(this.Extrato_Activated);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtnDtFim;
        private System.Windows.Forms.DateTimePicker dtpDataIN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.Button btImprimir;
        private System.Windows.Forms.Button btnFiltrar;
    }
}

