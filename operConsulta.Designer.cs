
namespace TeleBonifacio
{
    partial class Consultas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Consultas));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpDataIniicio = new System.Windows.Forms.DateTimePicker();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txComiss = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txPerc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGrid1 = new SourceGrid.DataGrid();
            this.dtpDataFim = new System.Windows.Forms.DateTimePicker();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpDataFim);
            this.panel1.Controls.Add(this.dtpDataIniicio);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.txComiss);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txPerc);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtValor);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(860, 46);
            this.panel1.TabIndex = 0;
            // 
            // dtpDataIniicio
            // 
            this.dtpDataIniicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataIniicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataIniicio.Location = new System.Drawing.Point(451, 10);
            this.dtpDataIniicio.Name = "dtpDataIniicio";
            this.dtpDataIniicio.Size = new System.Drawing.Size(85, 23);
            this.dtpDataIniicio.TabIndex = 12;
            this.dtpDataIniicio.Tag = "H";
            this.dtpDataIniicio.Value = new System.DateTime(2024, 1, 5, 0, 0, 0, 0);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(773, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Imprimir";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(692, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Pesquisar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txComiss
            // 
            this.txComiss.Location = new System.Drawing.Point(299, 12);
            this.txComiss.Name = "txComiss";
            this.txComiss.Size = new System.Drawing.Size(80, 20);
            this.txComiss.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(241, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Comissão";
            // 
            // txPerc
            // 
            this.txPerc.Location = new System.Drawing.Point(195, 12);
            this.txPerc.Name = "txPerc";
            this.txPerc.Size = new System.Drawing.Size(40, 20);
            this.txPerc.TabIndex = 7;
            this.txPerc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txPerc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txPerc_KeyPress);
            this.txPerc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txPerc_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Percentual";
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(45, 12);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(80, 20);
            this.txtValor.TabIndex = 5;
            this.txtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Valor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(545, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Data Final";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(385, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Inicial";
            // 
            // dataGrid1
            // 
            this.dataGrid1.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.EnableSort = false;
            this.dataGrid1.FixedRows = 1;
            this.dataGrid1.Location = new System.Drawing.Point(0, 46);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dataGrid1.Size = new System.Drawing.Size(860, 149);
            this.dataGrid1.TabIndex = 1;
            this.dataGrid1.TabStop = true;
            this.dataGrid1.ToolTipText = "";
            // 
            // dtpDataFim
            // 
            this.dtpDataFim.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataFim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataFim.Location = new System.Drawing.Point(606, 10);
            this.dtpDataFim.Name = "dtpDataFim";
            this.dtpDataFim.Size = new System.Drawing.Size(85, 23);
            this.dtpDataFim.TabIndex = 13;
            this.dtpDataFim.Tag = "H";
            // 
            // Consultas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 195);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Consultas";
            this.Text = "Consulta";
            this.Load += new System.EventHandler(this.operConsulta_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txComiss;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txPerc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private SourceGrid.DataGrid dataGrid1;
        private System.Windows.Forms.DateTimePicker dtpDataIniicio;
        private System.Windows.Forms.DateTimePicker dtpDataFim;
    }
}