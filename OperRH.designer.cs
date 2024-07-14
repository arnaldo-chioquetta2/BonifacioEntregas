
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperRH));
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btExcluir = new System.Windows.Forms.Button();
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
            this.dataGrid1 = new System.Windows.Forms.DataGridView();
            this.txInCafeMan = new System.Windows.Forms.TextBox();
            this.txFmCafeMan = new System.Windows.Forms.TextBox();
            this.txInCafeTrd = new System.Windows.Forms.TextBox();
            this.txFmCafeTrd = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
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
            this.panel1.Controls.Add(this.txFmCafeTrd);
            this.panel1.Controls.Add(this.txInCafeTrd);
            this.panel1.Controls.Add(this.txFmCafeMan);
            this.panel1.Controls.Add(this.txInCafeMan);
            this.panel1.Controls.Add(this.btExcluir);
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
            this.panel1.Size = new System.Drawing.Size(994, 83);
            this.panel1.TabIndex = 10;
            // 
            // btExcluir
            // 
            this.btExcluir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btExcluir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btExcluir.Location = new System.Drawing.Point(142, 46);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(75, 23);
            this.btExcluir.TabIndex = 56;
            this.btExcluir.Text = "Excluir";
            this.btExcluir.UseVisualStyleBackColor = false;
            this.btExcluir.Visible = false;
            this.btExcluir.Click += new System.EventHandler(this.btExcluir_Click);
            // 
            // dtpHorario
            // 
            this.dtpHorario.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpHorario.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHorario.Location = new System.Drawing.Point(314, 46);
            this.dtpHorario.Name = "dtpHorario";
            this.dtpHorario.Size = new System.Drawing.Size(90, 23);
            this.dtpHorario.TabIndex = 50;
            this.dtpHorario.Tag = "H";
            this.dtpHorario.ValueChanged += new System.EventHandler(this.dtpHorario_ValueChanged);
            // 
            // btGravar
            // 
            this.btGravar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btGravar.Enabled = false;
            this.btGravar.Location = new System.Drawing.Point(915, 44);
            this.btGravar.Name = "btGravar";
            this.btGravar.Size = new System.Drawing.Size(55, 23);
            this.btGravar.TabIndex = 54;
            this.btGravar.Text = "Gravar";
            this.btGravar.Click += new System.EventHandler(this.button1_Click);
            // 
            // txFnTrd
            // 
            this.txFnTrd.Location = new System.Drawing.Point(851, 46);
            this.txFnTrd.Name = "txFnTrd";
            this.txFnTrd.Size = new System.Drawing.Size(55, 20);
            this.txFnTrd.TabIndex = 58;
            this.txFnTrd.Enter += new System.EventHandler(this.txInMan_Enter);
            this.txFnTrd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            this.txFnTrd.MouseHover += new System.EventHandler(this.txInMan_MouseHover);
            // 
            // txInTrd
            // 
            this.txInTrd.Location = new System.Drawing.Point(668, 46);
            this.txInTrd.Name = "txInTrd";
            this.txInTrd.Size = new System.Drawing.Size(55, 20);
            this.txInTrd.TabIndex = 55;
            this.txInTrd.Enter += new System.EventHandler(this.txInMan_Enter);
            this.txInTrd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            this.txInTrd.MouseHover += new System.EventHandler(this.txInMan_MouseHover);
            // 
            // txFmMan
            // 
            this.txFmMan.Location = new System.Drawing.Point(607, 46);
            this.txFmMan.Name = "txFmMan";
            this.txFmMan.Size = new System.Drawing.Size(55, 20);
            this.txFmMan.TabIndex = 54;
            this.txFmMan.Enter += new System.EventHandler(this.txInMan_Enter);
            this.txFmMan.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            this.txFmMan.MouseHover += new System.EventHandler(this.txInMan_MouseHover);
            // 
            // txInMan
            // 
            this.txInMan.Location = new System.Drawing.Point(419, 46);
            this.txInMan.Name = "txInMan";
            this.txInMan.Size = new System.Drawing.Size(55, 20);
            this.txInMan.TabIndex = 51;
            this.txInMan.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            this.txInMan.MouseHover += new System.EventHandler(this.txInMan_MouseHover);
            // 
            // lbColaborador
            // 
            this.lbColaborador.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbColaborador.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lbColaborador.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbColaborador.Location = new System.Drawing.Point(108, 48);
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
            this.btLancar.Location = new System.Drawing.Point(836, 14);
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
            this.btImprimir.Location = new System.Drawing.Point(491, 11);
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
            this.btnFiltrar.Location = new System.Drawing.Point(410, 11);
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
            this.cmbVendedor.Location = new System.Drawing.Point(666, 16);
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
            this.label5.Location = new System.Drawing.Point(572, 17);
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
            this.dataGrid1.AllowUserToAddRows = false;
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.Location = new System.Drawing.Point(0, 83);
            this.dataGrid1.MultiSelect = false;
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid1.Size = new System.Drawing.Size(994, 367);
            this.dataGrid1.TabIndex = 11;
            this.dataGrid1.SelectionChanged += new System.EventHandler(this.dataGrid1_SelectionChanged);
            // 
            // txInCafeMan
            // 
            this.txInCafeMan.Location = new System.Drawing.Point(482, 46);
            this.txInCafeMan.Name = "txInCafeMan";
            this.txInCafeMan.Size = new System.Drawing.Size(55, 20);
            this.txInCafeMan.TabIndex = 52;
            this.txInCafeMan.Enter += new System.EventHandler(this.txInCafeMan_Enter);
            this.txInCafeMan.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            this.txInCafeMan.MouseHover += new System.EventHandler(this.txInMan_MouseHover);
            // 
            // txFmCafeMan
            // 
            this.txFmCafeMan.Location = new System.Drawing.Point(543, 46);
            this.txFmCafeMan.Name = "txFmCafeMan";
            this.txFmCafeMan.Size = new System.Drawing.Size(55, 20);
            this.txFmCafeMan.TabIndex = 53;
            this.txFmCafeMan.Enter += new System.EventHandler(this.txInMan_Enter);
            this.txFmCafeMan.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            this.txFmCafeMan.MouseHover += new System.EventHandler(this.txInMan_MouseHover);
            // 
            // txInCafeTrd
            // 
            this.txInCafeTrd.Location = new System.Drawing.Point(729, 46);
            this.txInCafeTrd.Name = "txInCafeTrd";
            this.txInCafeTrd.Size = new System.Drawing.Size(55, 20);
            this.txInCafeTrd.TabIndex = 56;
            this.txInCafeTrd.Enter += new System.EventHandler(this.txInMan_Enter);
            this.txInCafeTrd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            this.txInCafeTrd.MouseHover += new System.EventHandler(this.txInMan_MouseHover);
            // 
            // txFmCafeTrd
            // 
            this.txFmCafeTrd.Location = new System.Drawing.Point(790, 46);
            this.txFmCafeTrd.Name = "txFmCafeTrd";
            this.txFmCafeTrd.Size = new System.Drawing.Size(55, 20);
            this.txFmCafeTrd.TabIndex = 57;
            this.txFmCafeTrd.Enter += new System.EventHandler(this.txInMan_Enter);
            this.txFmCafeTrd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txInMan_KeyUp);
            this.txFmCafeTrd.MouseHover += new System.EventHandler(this.txInMan_MouseHover);
            // 
            // OperRH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 450);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OperRH";
            this.Text = "Lançamentos de Folha de Ponto";
            this.Activated += new System.EventHandler(this.Extrato_Activated);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
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
        private System.Windows.Forms.DataGridView dataGrid1;
        private System.Windows.Forms.Button btExcluir;
        private System.Windows.Forms.TextBox txFmCafeTrd;
        private System.Windows.Forms.TextBox txInCafeTrd;
        private System.Windows.Forms.TextBox txFmCafeMan;
        private System.Windows.Forms.TextBox txInCafeMan;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

