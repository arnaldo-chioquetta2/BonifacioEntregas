
namespace TeleBonifacio
{
    partial class OperRH
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpHorario = new System.Windows.Forms.DateTimePicker();
            this.btGravar = new System.Windows.Forms.Button();
            this.txFnTrd = new System.Windows.Forms.TextBox();
            this.txInTrd = new System.Windows.Forms.TextBox();
            this.txFmMan = new System.Windows.Forms.TextBox();
            this.txInMan = new System.Windows.Forms.TextBox();
            this.lbColaborador = new System.Windows.Forms.Label();
            this.btLancar = new System.Windows.Forms.Button();
            this.btImprimir = new System.Windows.Forms.Button();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.cmbVendedor = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtnDtFim = new System.Windows.Forms.DateTimePicker();
            this.dtpDataIN = new System.Windows.Forms.DateTimePicker();
            this.dataGrid1 = new SourceGrid.DataGrid();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lblTitulo.Location = new System.Drawing.Point(12, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(128, 24);
            this.lblTitulo.TabIndex = 8;
            this.lblTitulo.Text = "Lançamentos:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpHorario);
            this.panel1.Controls.Add(this.btGravar);
            this.panel1.Controls.Add(this.txFnTrd);
            this.panel1.Controls.Add(this.txInTrd);
            this.panel1.Controls.Add(this.txFmMan);
            this.panel1.Controls.Add(this.txInMan);
            this.panel1.Controls.Add(this.lbColaborador);
            this.panel1.Controls.Add(this.btLancar);
            this.panel1.Controls.Add(this.btImprimir);
            this.panel1.Controls.Add(this.btnFiltrar);
            this.panel1.Controls.Add(this.cmbVendedor);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dtnDtFim);
            this.panel1.Controls.Add(this.dtpDataIN);
            this.panel1.Controls.Add(this.lblTitulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(851, 77);
            this.panel1.TabIndex = 10;
            // 
            // dtpHorario
            // 
            this.dtpHorario.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpHorario.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHorario.Location = new System.Drawing.Point(442, 41);
            this.dtpHorario.Name = "dtpHorario";
            this.dtpHorario.Size = new System.Drawing.Size(90, 23);
            this.dtpHorario.TabIndex = 55;
            this.dtpHorario.Tag = "H";
            // 
            // btGravar
            // 
            this.btGravar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btGravar.Enabled = false;
            this.btGravar.Location = new System.Drawing.Point(784, 43);
            this.btGravar.Name = "btGravar";
            this.btGravar.Size = new System.Drawing.Size(55, 23);
            this.btGravar.TabIndex = 54;
            this.btGravar.Text = "Gravar";
            this.btGravar.Click += new System.EventHandler(this.button1_Click);
            // 
            // txFnTrd
            // 
            this.txFnTrd.Location = new System.Drawing.Point(723, 45);
            this.txFnTrd.Name = "txFnTrd";
            this.txFnTrd.Size = new System.Drawing.Size(55, 20);
            this.txFnTrd.TabIndex = 53;
            this.txFnTrd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            // 
            // txInTrd
            // 
            this.txInTrd.Location = new System.Drawing.Point(662, 45);
            this.txInTrd.Name = "txInTrd";
            this.txInTrd.Size = new System.Drawing.Size(55, 20);
            this.txInTrd.TabIndex = 52;
            this.txInTrd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            // 
            // txFmMan
            // 
            this.txFmMan.Location = new System.Drawing.Point(601, 45);
            this.txFmMan.Name = "txFmMan";
            this.txFmMan.Size = new System.Drawing.Size(55, 20);
            this.txFmMan.TabIndex = 51;
            this.txFmMan.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            // 
            // txInMan
            // 
            this.txInMan.Location = new System.Drawing.Point(540, 45);
            this.txInMan.Name = "txInMan";
            this.txInMan.Size = new System.Drawing.Size(55, 20);
            this.txInMan.TabIndex = 50;
            this.txInMan.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            // 
            // lbColaborador
            // 
            this.lbColaborador.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbColaborador.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lbColaborador.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbColaborador.Location = new System.Drawing.Point(240, 47);
            this.lbColaborador.Name = "lbColaborador";
            this.lbColaborador.Size = new System.Drawing.Size(196, 19);
            this.lbColaborador.TabIndex = 49;
            this.lbColaborador.Text = "Colaborador";
            this.lbColaborador.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btLancar
            // 
            this.btLancar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btLancar.Enabled = false;
            this.btLancar.Location = new System.Drawing.Point(764, 14);
            this.btLancar.Name = "btLancar";
            this.btLancar.Size = new System.Drawing.Size(75, 23);
            this.btLancar.TabIndex = 48;
            this.btLancar.Text = "Lançar";
            this.btLancar.Click += new System.EventHandler(this.btLancar_Click);
            // 
            // btImprimir
            // 
            this.btImprimir.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btImprimir.Enabled = false;
            this.btImprimir.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btImprimir.Location = new System.Drawing.Point(419, 11);
            this.btImprimir.Name = "btImprimir";
            this.btImprimir.Size = new System.Drawing.Size(75, 23);
            this.btImprimir.TabIndex = 47;
            this.btImprimir.Text = "Imprimir";
            this.btImprimir.Click += new System.EventHandler(this.btImprimir_Click);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnFiltrar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFiltrar.Location = new System.Drawing.Point(338, 11);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(75, 23);
            this.btnFiltrar.TabIndex = 46;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // cmbVendedor
            // 
            this.cmbVendedor.DisplayMember = "Nome";
            this.cmbVendedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVendedor.Items.AddRange(new object[] {
            "Dinheiro",
            "Cartão",
            "Pix",
            "Despesa"});
            this.cmbVendedor.Location = new System.Drawing.Point(605, 16);
            this.cmbVendedor.Name = "cmbVendedor";
            this.cmbVendedor.Size = new System.Drawing.Size(150, 21);
            this.cmbVendedor.TabIndex = 45;
            this.cmbVendedor.ValueMember = "Id";
            this.cmbVendedor.SelectedIndexChanged += new System.EventHandler(this.cmbVendedor_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label5.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label5.Location = new System.Drawing.Point(500, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 16);
            this.label5.TabIndex = 44;
            this.label5.Text = "Colaboradores";
            // 
            // dtnDtFim
            // 
            this.dtnDtFim.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtnDtFim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtnDtFim.Location = new System.Drawing.Point(242, 9);
            this.dtnDtFim.Name = "dtnDtFim";
            this.dtnDtFim.Size = new System.Drawing.Size(90, 23);
            this.dtnDtFim.TabIndex = 43;
            this.dtnDtFim.Tag = "H";
            // 
            // dtpDataIN
            // 
            this.dtpDataIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataIN.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataIN.Location = new System.Drawing.Point(146, 10);
            this.dtpDataIN.Name = "dtpDataIN";
            this.dtpDataIN.Size = new System.Drawing.Size(90, 23);
            this.dtpDataIN.TabIndex = 42;
            this.dtpDataIN.Tag = "H";
            // 
            // dataGrid1
            // 
            this.dataGrid1.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.EnableSort = false;
            this.dataGrid1.FixedRows = 1;
            this.dataGrid1.Location = new System.Drawing.Point(0, 77);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dataGrid1.Size = new System.Drawing.Size(851, 373);
            this.dataGrid1.TabIndex = 13;
            this.dataGrid1.TabStop = true;
            this.dataGrid1.ToolTipText = "";
            // 
            // OperRH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 450);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "OperRH";
            this.Text = "Lançamentos de Folha de Ponto";
            this.Activated += new System.EventHandler(this.Extrato_Activated);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtnDtFim;
        private System.Windows.Forms.DateTimePicker dtpDataIN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbVendedor;
        private System.Windows.Forms.Button btImprimir;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btLancar;
        private System.Windows.Forms.Label lbColaborador;
        private System.Windows.Forms.Button btGravar;
        private System.Windows.Forms.TextBox txFnTrd;
        private System.Windows.Forms.TextBox txInTrd;
        private System.Windows.Forms.TextBox txFmMan;
        private System.Windows.Forms.TextBox txInMan;
        private System.Windows.Forms.DateTimePicker dtpHorario;
        private SourceGrid.DataGrid dataGrid1;
    }
}

