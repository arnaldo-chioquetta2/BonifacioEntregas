
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(opRecibos));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtnDtFim = new System.Windows.Forms.DateTimePicker();
            this.dtpDataIN = new System.Windows.Forms.DateTimePicker();
            this.ltVlr = new System.Windows.Forms.Label();
            this.btExtrato = new System.Windows.Forms.Button();
            this.btPagar = new System.Windows.Forms.Button();
            this.cmbVendedor = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGrid1 = new SourceGrid.DataGrid();
            this.btAtu = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btAtu);
            this.panel1.Controls.Add(this.dtnDtFim);
            this.panel1.Controls.Add(this.dtpDataIN);
            this.panel1.Controls.Add(this.ltVlr);
            this.panel1.Controls.Add(this.btExtrato);
            this.panel1.Controls.Add(this.btPagar);
            this.panel1.Controls.Add(this.cmbVendedor);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(833, 67);
            this.panel1.TabIndex = 0;
            // 
            // dtnDtFim
            // 
            this.dtnDtFim.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtnDtFim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtnDtFim.Location = new System.Drawing.Point(420, 18);
            this.dtnDtFim.Name = "dtnDtFim";
            this.dtnDtFim.Size = new System.Drawing.Size(90, 23);
            this.dtnDtFim.TabIndex = 45;
            this.dtnDtFim.Tag = "H";
            // 
            // dtpDataIN
            // 
            this.dtpDataIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataIN.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataIN.Location = new System.Drawing.Point(324, 19);
            this.dtpDataIN.Name = "dtpDataIN";
            this.dtpDataIN.Size = new System.Drawing.Size(90, 23);
            this.dtpDataIN.TabIndex = 44;
            this.dtpDataIN.Tag = "H";
            // 
            // ltVlr
            // 
            this.ltVlr.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltVlr.Location = new System.Drawing.Point(595, 20);
            this.ltVlr.Name = "ltVlr";
            this.ltVlr.Size = new System.Drawing.Size(74, 17);
            this.ltVlr.TabIndex = 16;
            // 
            // btExtrato
            // 
            this.btExtrato.Enabled = false;
            this.btExtrato.Location = new System.Drawing.Point(754, 17);
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
            this.btPagar.Location = new System.Drawing.Point(675, 17);
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
            this.cmbVendedor.Location = new System.Drawing.Point(86, 19);
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
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vendedor:";
            // 
            // dataGrid1
            // 
            this.dataGrid1.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.EnableSort = false;
            this.dataGrid1.FixedRows = 1;
            this.dataGrid1.Location = new System.Drawing.Point(0, 67);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.SelectionMode = SourceGrid.GridSelectionMode.Column;
            this.dataGrid1.Size = new System.Drawing.Size(833, 74);
            this.dataGrid1.TabIndex = 2;
            this.dataGrid1.TabStop = true;
            this.dataGrid1.ToolTipText = "";
            this.dataGrid1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGrid1_MouseDown);
            // 
            // btAtu
            // 
            this.btAtu.Location = new System.Drawing.Point(516, 17);
            this.btAtu.Name = "btAtu";
            this.btAtu.Size = new System.Drawing.Size(73, 23);
            this.btAtu.TabIndex = 46;
            this.btAtu.Text = "Atualizar";
            this.btAtu.UseVisualStyleBackColor = true;
            this.btAtu.Click += new System.EventHandler(this.btAtu_Click);
            // 
            // opRecibos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 141);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "opRecibos";
            this.Text = "Recibos";
            this.Load += new System.EventHandler(this.opRecibos_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private SourceGrid.DataGrid dataGrid1;
        private System.Windows.Forms.ComboBox cmbVendedor;
        private System.Windows.Forms.Label ltVlr;
        private System.Windows.Forms.Button btExtrato;
        private System.Windows.Forms.Button btPagar;
        private System.Windows.Forms.DateTimePicker dtnDtFim;
        private System.Windows.Forms.DateTimePicker dtpDataIN;
        private System.Windows.Forms.Button btAtu;
    }
}