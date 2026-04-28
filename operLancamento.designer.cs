namespace TeleBonifacio
{
    partial class operLancamento
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtInicial = new System.Windows.Forms.DateTimePicker();
            this.btRelat = new System.Windows.Forms.Button();
            this.cmbVendedor = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbLiberadoPor = new System.Windows.Forms.ComboBox();
            this.labelLiberadoPor = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txDesc = new System.Windows.Forms.TextBox();
            this.lbTotal = new System.Windows.Forms.Label();
            this.dtpData = new System.Windows.Forms.DateTimePicker();
            this.txObs = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txCompra = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbMoto = new System.Windows.Forms.Label();
            this.cmbMotoBoy = new System.Windows.Forms.ComboBox();
            this.cmbFormaPagamento = new System.Windows.Forms.ComboBox();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnNovoCliente = new System.Windows.Forms.Button();
            this.dataGrid1 = new SourceGrid.DataGrid();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtInicial);
            this.panel1.Controls.Add(this.btRelat);
            this.panel1.Controls.Add(this.cmbVendedor);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cmbLiberadoPor);
            this.panel1.Controls.Add(this.labelLiberadoPor);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txDesc);
            this.panel1.Controls.Add(this.lbTotal);
            this.panel1.Controls.Add(this.dtpData);
            this.panel1.Controls.Add(this.txObs);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txCompra);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbCliente);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lbMoto);
            this.panel1.Controls.Add(this.cmbMotoBoy);
            this.panel1.Controls.Add(this.cmbFormaPagamento);
            this.panel1.Controls.Add(this.txtValor);
            this.panel1.Controls.Add(this.btnLimpar);
            this.panel1.Controls.Add(this.btnAdicionar);
            this.panel1.Controls.Add(this.btnFiltrar);
            this.panel1.Controls.Add(this.btnNovoCliente);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(920, 144);
            this.panel1.TabIndex = 11;
            // 
            // dtInicial
            // 
            this.dtInicial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtInicial.Location = new System.Drawing.Point(627, 111);
            this.dtInicial.Name = "dtInicial";
            this.dtInicial.Size = new System.Drawing.Size(90, 23);
            this.dtInicial.TabIndex = 29;
            this.dtInicial.Tag = "H";
            // 
            // btRelat
            // 
            this.btRelat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRelat.Location = new System.Drawing.Point(817, 11);
            this.btRelat.Name = "btRelat";
            this.btRelat.Size = new System.Drawing.Size(91, 23);
            this.btRelat.TabIndex = 35;
            this.btRelat.Text = "Relatório";
            this.btRelat.Click += new System.EventHandler(this.btRelat_Click);
            // 
            // cmbVendedor
            // 
            this.cmbVendedor.DisplayMember = "Nome";
            this.cmbVendedor.Location = new System.Drawing.Point(82, 65);
            this.cmbVendedor.Name = "cmbVendedor";
            this.cmbVendedor.Size = new System.Drawing.Size(257, 21);
            this.cmbVendedor.TabIndex = 34;
            this.cmbVendedor.ValueMember = "Id";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label7.Location = new System.Drawing.Point(12, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 16);
            this.label7.TabIndex = 33;
            this.label7.Text = "Vendedor";
            // 
            // cmbLiberadoPor
            // 
            this.cmbLiberadoPor.DisplayMember = "Nome";
            this.cmbLiberadoPor.Location = new System.Drawing.Point(82, 91);
            this.cmbLiberadoPor.Name = "cmbLiberadoPor";
            this.cmbLiberadoPor.Size = new System.Drawing.Size(257, 21);
            this.cmbLiberadoPor.TabIndex = 36;
            this.cmbLiberadoPor.ValueMember = "Id";
            // 
            // labelLiberadoPor
            // 
            this.labelLiberadoPor.AutoSize = true;
            this.labelLiberadoPor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelLiberadoPor.Location = new System.Drawing.Point(1, 92);
            this.labelLiberadoPor.Name = "labelLiberadoPor";
            this.labelLiberadoPor.Size = new System.Drawing.Size(84, 16);
            this.labelLiberadoPor.TabIndex = 37;
            this.labelLiberadoPor.Text = "Liberado por";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label6.Location = new System.Drawing.Point(575, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 16);
            this.label6.TabIndex = 32;
            this.label6.Text = "Desconto";
            // 
            // txDesc
            // 
            this.txDesc.Location = new System.Drawing.Point(647, 66);
            this.txDesc.Name = "txDesc";
            this.txDesc.Size = new System.Drawing.Size(56, 20);
            this.txDesc.TabIndex = 31;
            this.txDesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txDesc.Enter += new System.EventHandler(this.txtValor_Enter);
            this.txDesc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            this.txDesc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtValor_KeyUp);
            // 
            // lbTotal
            // 
            this.lbTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lbTotal.Location = new System.Drawing.Point(709, 68);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(83, 23);
            this.lbTotal.TabIndex = 30;
            this.lbTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpData
            // 
            this.dtpData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpData.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpData.Location = new System.Drawing.Point(723, 112);
            this.dtpData.Name = "dtpData";
            this.dtpData.Size = new System.Drawing.Size(90, 23);
            this.dtpData.TabIndex = 30;
            this.dtpData.Tag = "H";
            // 
            // txObs
            // 
            this.txObs.Location = new System.Drawing.Point(447, 114);
            this.txObs.Name = "txObs";
            this.txObs.Size = new System.Drawing.Size(174, 20);
            this.txObs.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label5.Location = new System.Drawing.Point(358, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 16);
            this.label5.TabIndex = 27;
            this.label5.Text = "Observação";
            // 
            // txCompra
            // 
            this.txCompra.Location = new System.Drawing.Point(647, 38);
            this.txCompra.Name = "txCompra";
            this.txCompra.Size = new System.Drawing.Size(56, 20);
            this.txCompra.TabIndex = 15;
            this.txCompra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txCompra.Enter += new System.EventHandler(this.txtValor_Enter);
            this.txCompra.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            this.txCompra.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtValor_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label4.Location = new System.Drawing.Point(585, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Compra";
            // 
            // cmbCliente
            // 
            this.cmbCliente.DisplayMember = "Nome";
            this.cmbCliente.Location = new System.Drawing.Point(82, 38);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(257, 21);
            this.cmbCliente.TabIndex = 24;
            this.cmbCliente.ValueMember = "Id";
            this.cmbCliente.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbCliente_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label3.Location = new System.Drawing.Point(30, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 16);
            this.label3.TabIndex = 23;
            this.label3.Text = "Cliente";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label2.Location = new System.Drawing.Point(601, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 22;
            this.label2.Text = "Valor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label1.Location = new System.Drawing.Point(345, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 16);
            this.label1.TabIndex = 21;
            this.label1.Text = "Forma de Pagamento";
            // 
            // lbMoto
            // 
            this.lbMoto.AutoSize = true;
            this.lbMoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lbMoto.Location = new System.Drawing.Point(15, 16);
            this.lbMoto.Name = "lbMoto";
            this.lbMoto.Size = new System.Drawing.Size(60, 16);
            this.lbMoto.TabIndex = 20;
            this.lbMoto.Text = "Motoboy";
            // 
            // cmbMotoBoy
            // 
            this.cmbMotoBoy.DisplayMember = "Nome";
            this.cmbMotoBoy.Location = new System.Drawing.Point(82, 13);
            this.cmbMotoBoy.Name = "cmbMotoBoy";
            this.cmbMotoBoy.Size = new System.Drawing.Size(257, 21);
            this.cmbMotoBoy.TabIndex = 12;
            this.cmbMotoBoy.ValueMember = "Id";
            this.cmbMotoBoy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbMotoBoy_KeyPress);
            // 
            // cmbFormaPagamento
            // 
            this.cmbFormaPagamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormaPagamento.Items.AddRange(new object[] {
            "Anotado",
            "Cartão,",
            "Dinheiro",
            "Pix",
            "Troca",
            "Boleto"});
            this.cmbFormaPagamento.Location = new System.Drawing.Point(490, 10);
            this.cmbFormaPagamento.Name = "cmbFormaPagamento";
            this.cmbFormaPagamento.Size = new System.Drawing.Size(95, 21);
            this.cmbFormaPagamento.TabIndex = 13;
            this.cmbFormaPagamento.SelectedIndexChanged += new System.EventHandler(this.cmbFormaPagamento_SelectedIndexChanged);
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(647, 11);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(56, 20);
            this.txtValor.TabIndex = 14;
            this.txtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtValor.Enter += new System.EventHandler(this.txtValor_Enter);
            this.txtValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            this.txtValor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtValor_KeyUp);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpar.Location = new System.Drawing.Point(817, 44);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(91, 23);
            this.btnLimpar.TabIndex = 16;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdicionar.Enabled = false;
            this.btnAdicionar.Location = new System.Drawing.Point(817, 77);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(91, 23);
            this.btnAdicionar.TabIndex = 17;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFiltrar.Location = new System.Drawing.Point(817, 110);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(91, 23);
            this.btnFiltrar.TabIndex = 18;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // btnNovoCliente
            // 
            this.btnNovoCliente.Location = new System.Drawing.Point(12, 115);
            this.btnNovoCliente.Name = "btnNovoCliente";
            this.btnNovoCliente.Size = new System.Drawing.Size(96, 23);
            this.btnNovoCliente.TabIndex = 19;
            this.btnNovoCliente.Text = "Novo Cliente";
            this.btnNovoCliente.Click += new System.EventHandler(this.btnNovoCliente_Click);
            // 
            // dataGrid1
            // 
            this.dataGrid1.DeleteQuestionMessage = "Are you sure to delete all the selected rows?";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.EnableSort = false;
            this.dataGrid1.FixedRows = 1;
            this.dataGrid1.Location = new System.Drawing.Point(0, 144);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.SelectionMode = SourceGrid.GridSelectionMode.Row;
            this.dataGrid1.Size = new System.Drawing.Size(920, 417);
            this.dataGrid1.TabIndex = 12;
            this.dataGrid1.TabStop = true;
            this.dataGrid1.ToolTipText = "";
            this.dataGrid1.Click += new System.EventHandler(this.dataGrid1_Click);
            // 
            // operLancamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 561);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "operLancamento";
            this.Text = "Lançamento de Entregas";
            this.Activated += new System.EventHandler(this.operLancamento_Activated);
            this.Load += new System.EventHandler(this.operLancamento_Load);
            this.Resize += new System.EventHandler(this.operLancamento_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmbFormaPagamento;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnNovoCliente;
        private SourceGrid.DataGrid dataGrid1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbMoto;
        private System.Windows.Forms.TextBox txObs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txCompra;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbMotoBoy;
        private System.Windows.Forms.DateTimePicker dtpData;
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txDesc;
        private System.Windows.Forms.ComboBox cmbVendedor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbLiberadoPor;
        private System.Windows.Forms.Label labelLiberadoPor;
        private System.Windows.Forms.Button btRelat;
        private System.Windows.Forms.DateTimePicker dtInicial;
    }
}