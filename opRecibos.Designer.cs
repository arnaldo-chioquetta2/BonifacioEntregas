
using System.Drawing;
using System.Windows.Forms;

namespace TeleBonifacio
{
    partial class opRecibos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(opRecibos));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdSemanal = new System.Windows.Forms.RadioButton();
            this.rdQuinzenal = new System.Windows.Forms.RadioButton();
            this.rdMensal = new System.Windows.Forms.RadioButton();
            this.lbFim = new System.Windows.Forms.Label();
            this.btAtu = new System.Windows.Forms.Button();
            this.dtpDataIN = new System.Windows.Forms.DateTimePicker();
            this.ltVlr = new System.Windows.Forms.Label();
            this.btExtrato = new System.Windows.Forms.Button();
            this.btPagar = new System.Windows.Forms.Button();
            this.cmbVendedor = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGrid1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.lbFim);
            this.panel1.Controls.Add(this.btAtu);
            this.panel1.Controls.Add(this.dtpDataIN);
            this.panel1.Controls.Add(this.ltVlr);
            this.panel1.Controls.Add(this.btExtrato);
            this.panel1.Controls.Add(this.btPagar);
            this.panel1.Controls.Add(this.cmbVendedor);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(608, 82);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdSemanal);
            this.groupBox1.Controls.Add(this.rdQuinzenal);
            this.groupBox1.Controls.Add(this.rdMensal);
            this.groupBox1.Location = new System.Drawing.Point(9, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 38);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            // 
            // rdSemanal
            // 
            this.rdSemanal.AutoSize = true;
            this.rdSemanal.Location = new System.Drawing.Point(234, 10);
            this.rdSemanal.Name = "rdSemanal";
            this.rdSemanal.Size = new System.Drawing.Size(66, 17);
            this.rdSemanal.TabIndex = 2;
            this.rdSemanal.TabStop = true;
            this.rdSemanal.Text = "Semanal";
            this.rdSemanal.UseVisualStyleBackColor = true;
            this.rdSemanal.CheckedChanged += new System.EventHandler(this.rdSemanal_CheckedChanged);
            // 
            // rdQuinzenal
            // 
            this.rdQuinzenal.AutoSize = true;
            this.rdQuinzenal.Location = new System.Drawing.Point(125, 10);
            this.rdQuinzenal.Name = "rdQuinzenal";
            this.rdQuinzenal.Size = new System.Drawing.Size(72, 17);
            this.rdQuinzenal.TabIndex = 1;
            this.rdQuinzenal.TabStop = true;
            this.rdQuinzenal.Text = "Quinzenal";
            this.rdQuinzenal.UseVisualStyleBackColor = true;
            this.rdQuinzenal.CheckedChanged += new System.EventHandler(this.rdQuinzenal_CheckedChanged);
            // 
            // rdMensal
            // 
            this.rdMensal.AutoSize = true;
            this.rdMensal.Location = new System.Drawing.Point(11, 10);
            this.rdMensal.Name = "rdMensal";
            this.rdMensal.Size = new System.Drawing.Size(59, 17);
            this.rdMensal.TabIndex = 0;
            this.rdMensal.TabStop = true;
            this.rdMensal.Text = "Mensal";
            this.rdMensal.UseVisualStyleBackColor = true;
            this.rdMensal.CheckedChanged += new System.EventHandler(this.rdMensal_CheckedChanged);
            // 
            // lbFim
            // 
            this.lbFim.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFim.Location = new System.Drawing.Point(435, 45);
            this.lbFim.Name = "lbFim";
            this.lbFim.Size = new System.Drawing.Size(74, 22);
            this.lbFim.TabIndex = 47;
            // 
            // btAtu
            // 
            this.btAtu.Location = new System.Drawing.Point(336, 14);
            this.btAtu.Name = "btAtu";
            this.btAtu.Size = new System.Drawing.Size(73, 23);
            this.btAtu.TabIndex = 46;
            this.btAtu.Text = "Atualizar";
            this.btAtu.UseVisualStyleBackColor = true;
            this.btAtu.Click += new System.EventHandler(this.btAtu_Click);
            // 
            // dtpDataIN
            // 
            this.dtpDataIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataIN.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataIN.Location = new System.Drawing.Point(339, 44);
            this.dtpDataIN.Name = "dtpDataIN";
            this.dtpDataIN.Size = new System.Drawing.Size(90, 23);
            this.dtpDataIN.TabIndex = 44;
            this.dtpDataIN.Tag = "H";
            this.dtpDataIN.ValueChanged += new System.EventHandler(this.dtpDataIN_ValueChanged);
            // 
            // ltVlr
            // 
            this.ltVlr.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltVlr.Location = new System.Drawing.Point(435, 17);
            this.ltVlr.Name = "ltVlr";
            this.ltVlr.Size = new System.Drawing.Size(74, 17);
            this.ltVlr.TabIndex = 16;
            // 
            // btExtrato
            // 
            this.btExtrato.Enabled = false;
            this.btExtrato.Location = new System.Drawing.Point(526, 43);
            this.btExtrato.Name = "btExtrato";
            this.btExtrato.Size = new System.Drawing.Size(73, 23);
            this.btExtrato.TabIndex = 15;
            this.btExtrato.Text = "Extrato";
            this.btExtrato.UseVisualStyleBackColor = true;
            this.btExtrato.Click += new System.EventHandler(this.btExtrato_Click);
            // 
            // btPagar
            // 
            this.btPagar.Enabled = false;
            this.btPagar.Location = new System.Drawing.Point(526, 14);
            this.btPagar.Name = "btPagar";
            this.btPagar.Size = new System.Drawing.Size(73, 23);
            this.btPagar.TabIndex = 14;
            this.btPagar.Text = "Pagar";
            this.btPagar.UseVisualStyleBackColor = true;
            this.btPagar.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbVendedor
            // 
            this.cmbVendedor.DisplayMember = "Nome";
            this.cmbVendedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVendedor.Location = new System.Drawing.Point(101, 44);
            this.cmbVendedor.Name = "cmbVendedor";
            this.cmbVendedor.Size = new System.Drawing.Size(232, 21);
            this.cmbVendedor.TabIndex = 13;
            this.cmbVendedor.ValueMember = "Id";
            this.cmbVendedor.SelectedIndexChanged += new System.EventHandler(this.cmbVendedor_SelectedIndexChanged_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vendedor:";
            // 
            // dataGrid1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.EnableHeadersVisualStyles = false;
            this.dataGrid1.Location = new System.Drawing.Point(0, 82);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(608, 120);
            this.dataGrid1.TabIndex = 1;
            this.dataGrid1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGrid1_MouseDown);
            // 
            // opRecibos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 202);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "opRecibos";
            this.Text = "Recibos";
            this.Load += new System.EventHandler(this.opRecibos_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbVendedor;
        private System.Windows.Forms.Label ltVlr;
        private System.Windows.Forms.Button btExtrato;
        private System.Windows.Forms.Button btPagar;
        private System.Windows.Forms.DateTimePicker dtpDataIN;
        private System.Windows.Forms.Button btAtu;
        private System.Windows.Forms.DataGridView dataGrid1;
        private Label lbFim;
        private GroupBox groupBox1;
        private RadioButton rdSemanal;
        private RadioButton rdQuinzenal;
        private RadioButton rdMensal;
    }
}