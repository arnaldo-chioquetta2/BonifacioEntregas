
namespace TeleBonifacio
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtLucroTotal = new System.Windows.Forms.Label();
            this.txtTotalEntregadores = new System.Windows.Forms.Label();
            this.txtTotalVendas = new System.Windows.Forms.Label();
            this.dtpDataFim = new System.Windows.Forms.DateTimePicker();
            this.dtpDataIniicio = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtComiss = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtComiss);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtLucroTotal);
            this.panel2.Controls.Add(this.txtTotalEntregadores);
            this.panel2.Controls.Add(this.txtTotalVendas);
            this.panel2.Controls.Add(this.dtpDataFim);
            this.panel2.Controls.Add(this.dtpDataIniicio);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 56);
            this.panel2.TabIndex = 1;
            // 
            // txtLucroTotal
            // 
            this.txtLucroTotal.AutoSize = true;
            this.txtLucroTotal.Location = new System.Drawing.Point(729, 37);
            this.txtLucroTotal.Name = "txtLucroTotal";
            this.txtLucroTotal.Size = new System.Drawing.Size(49, 13);
            this.txtLucroTotal.TabIndex = 16;
            this.txtLucroTotal.Text = "8.888,88";
            this.txtLucroTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalEntregadores
            // 
            this.txtTotalEntregadores.AutoSize = true;
            this.txtTotalEntregadores.Location = new System.Drawing.Point(515, 37);
            this.txtTotalEntregadores.Name = "txtTotalEntregadores";
            this.txtTotalEntregadores.Size = new System.Drawing.Size(52, 13);
            this.txtTotalEntregadores.TabIndex = 15;
            this.txtTotalEntregadores.Text = " 8.888,88";
            this.txtTotalEntregadores.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotalVendas
            // 
            this.txtTotalVendas.AutoSize = true;
            this.txtTotalVendas.Location = new System.Drawing.Point(417, 37);
            this.txtTotalVendas.Name = "txtTotalVendas";
            this.txtTotalVendas.Size = new System.Drawing.Size(55, 13);
            this.txtTotalVendas.TabIndex = 14;
            this.txtTotalVendas.Text = "88.888,88";
            this.txtTotalVendas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDataFim
            // 
            this.dtpDataFim.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataFim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataFim.Location = new System.Drawing.Point(233, 12);
            this.dtpDataFim.Name = "dtpDataFim";
            this.dtpDataFim.Size = new System.Drawing.Size(85, 23);
            this.dtpDataFim.TabIndex = 13;
            this.dtpDataFim.Tag = "H";
            // 
            // dtpDataIniicio
            // 
            this.dtpDataIniicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataIniicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataIniicio.Location = new System.Drawing.Point(78, 12);
            this.dtpDataIniicio.Name = "dtpDataIniicio";
            this.dtpDataIniicio.Size = new System.Drawing.Size(85, 23);
            this.dtpDataIniicio.TabIndex = 12;
            this.dtpDataIniicio.Tag = "H";
            this.dtpDataIniicio.Value = new System.DateTime(2024, 1, 5, 0, 0, 0, 0);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(328, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Pesquisar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Data Final";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Inicial";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 50);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(417, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Vendas: ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(512, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Entregas: ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(621, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Comissões :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtComiss
            // 
            this.txtComiss.AutoSize = true;
            this.txtComiss.Location = new System.Drawing.Point(621, 37);
            this.txtComiss.Name = "txtComiss";
            this.txtComiss.Size = new System.Drawing.Size(52, 13);
            this.txtComiss.TabIndex = 20;
            this.txtComiss.Text = " 8.888,88";
            this.txtComiss.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(738, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Lucro :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpDataFim;
        private System.Windows.Forms.DateTimePicker dtpDataIniicio;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label txtLucroTotal;
        private System.Windows.Forms.Label txtTotalEntregadores;
        private System.Windows.Forms.Label txtTotalVendas;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label txtComiss;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}