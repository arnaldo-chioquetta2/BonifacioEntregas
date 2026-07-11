namespace TeleBonifacio
{
    partial class operEtiquetas
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(operEtiquetas));
            this.lblCodigo = new System.Windows.Forms.Label();
            this.lblNomeEmpresa = new System.Windows.Forms.Label();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.lblTeleEntrega = new System.Windows.Forms.Label();
            this.lblLocal = new System.Windows.Forms.Label();
            this.lblLinha = new System.Windows.Forms.Label();
            this.lblFonte = new System.Windows.Forms.Label();
            this.lblTamanho = new System.Windows.Forms.Label();
            this.lblNegrito = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.lblPreco = new System.Windows.Forms.Label();
            this.lblObservacao = new System.Windows.Forms.Label();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.lblImpressora = new System.Windows.Forms.Label();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.txtNomeEmpresa = new System.Windows.Forms.TextBox();
            this.txtTelefone = new System.Windows.Forms.TextBox();
            this.txtTeleEntrega = new System.Windows.Forms.TextBox();
            this.txtLocal = new System.Windows.Forms.TextBox();
            this.cmbLinhaFormatacao = new System.Windows.Forms.ComboBox();
            this.cmbFonte = new System.Windows.Forms.ComboBox();
            this.numTamanhoFonte = new System.Windows.Forms.NumericUpDown();
            this.chkNegrito = new System.Windows.Forms.CheckBox();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.txtPreco = new System.Windows.Forms.TextBox();
            this.txtObservacao = new System.Windows.Forms.TextBox();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.numQuantidade = new System.Windows.Forms.NumericUpDown();
            this.cmbImpressora = new System.Windows.Forms.ComboBox();
            this.gridEtiquetas = new System.Windows.Forms.DataGridView();
            this.pnlPreview = new System.Windows.Forms.Panel();
            this.btNovo = new System.Windows.Forms.Button();
            this.btSalvar = new System.Windows.Forms.Button();
            this.btExcluir = new System.Windows.Forms.Button();
            this.btLimpar = new System.Windows.Forms.Button();
            this.btImprimir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numTamanhoFonte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEtiquetas)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(18, 18);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(40, 13);
            this.lblCodigo.TabIndex = 21;
            this.lblCodigo.Text = "Código";
            // 
            // lblNomeEmpresa
            // 
            this.lblNomeEmpresa.AutoSize = true;
            this.lblNomeEmpresa.Location = new System.Drawing.Point(154, 18);
            this.lblNomeEmpresa.Name = "lblNomeEmpresa";
            this.lblNomeEmpresa.Size = new System.Drawing.Size(93, 13);
            this.lblNomeEmpresa.TabIndex = 35;
            this.lblNomeEmpresa.Text = "Nome da empresa";
            // 
            // lblTelefone
            // 
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Location = new System.Drawing.Point(429, 18);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(49, 13);
            this.lblTelefone.TabIndex = 33;
            this.lblTelefone.Text = "Telefone";
            // 
            // lblTeleEntrega
            // 
            this.lblTeleEntrega.AutoSize = true;
            this.lblTeleEntrega.Location = new System.Drawing.Point(615, 18);
            this.lblTeleEntrega.Name = "lblTeleEntrega";
            this.lblTeleEntrega.Size = new System.Drawing.Size(67, 13);
            this.lblTeleEntrega.TabIndex = 31;
            this.lblTeleEntrega.Text = "Tele-entrega";
            // 
            // lblLinha
            // 
            this.lblLinha.AutoSize = true;
            this.lblLinha.Location = new System.Drawing.Point(18, 70);
            this.lblLinha.Name = "lblLinha";
            this.lblLinha.Size = new System.Drawing.Size(33, 13);
            this.lblLinha.TabIndex = 29;
            this.lblLinha.Text = "Linha";
            // 
            // lblFonte
            // 
            this.lblFonte.AutoSize = true;
            this.lblFonte.Location = new System.Drawing.Point(233, 70);
            this.lblFonte.Name = "lblFonte";
            this.lblFonte.Size = new System.Drawing.Size(34, 13);
            this.lblFonte.TabIndex = 28;
            this.lblFonte.Text = "Fonte";
            // 
            // lblTamanho
            // 
            this.lblTamanho.AutoSize = true;
            this.lblTamanho.Location = new System.Drawing.Point(605, 70);
            this.lblTamanho.Name = "lblTamanho";
            this.lblTamanho.Size = new System.Drawing.Size(52, 13);
            this.lblTamanho.TabIndex = 27;
            this.lblTamanho.Text = "Tamanho";
            // 
            // lblNegrito
            // 
            this.lblNegrito.AutoSize = true;
            this.lblNegrito.Location = new System.Drawing.Point(720, 70);
            this.lblNegrito.Name = "lblNegrito";
            this.lblNegrito.Size = new System.Drawing.Size(41, 13);
            this.lblNegrito.TabIndex = 26;
            this.lblNegrito.Text = "Negrito";
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Location = new System.Drawing.Point(18, 122);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(55, 13);
            this.lblDescricao.TabIndex = 19;
            this.lblDescricao.Text = "Descrição";
            // 
            // lblPreco
            // 
            this.lblPreco.AutoSize = true;
            this.lblPreco.Location = new System.Drawing.Point(315, 122);
            this.lblPreco.Name = "lblPreco";
            this.lblPreco.Size = new System.Drawing.Size(35, 13);
            this.lblPreco.TabIndex = 17;
            this.lblPreco.Text = "Preço";
            // 
            // lblObservacao
            // 
            this.lblObservacao.AutoSize = true;
            this.lblObservacao.Location = new System.Drawing.Point(18, 174);
            this.lblObservacao.Name = "lblObservacao";
            this.lblObservacao.Size = new System.Drawing.Size(65, 13);
            this.lblObservacao.TabIndex = 11;
            this.lblObservacao.Text = "Observação";
            // 
            // lblLocal
            // 
            this.lblLocal.AutoSize = true;
            this.lblLocal.Location = new System.Drawing.Point(423, 174);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.Size = new System.Drawing.Size(33, 13);
            this.lblLocal.TabIndex = 36;
            this.lblLocal.Text = "Local";
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.Location = new System.Drawing.Point(429, 122);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(62, 13);
            this.lblQuantidade.TabIndex = 15;
            this.lblQuantidade.Text = "Quantidade";
            // 
            // lblImpressora
            // 
            this.lblImpressora.AutoSize = true;
            this.lblImpressora.Location = new System.Drawing.Point(530, 122);
            this.lblImpressora.Name = "lblImpressora";
            this.lblImpressora.Size = new System.Drawing.Size(58, 13);
            this.lblImpressora.TabIndex = 13;
            this.lblImpressora.Text = "Impressora";
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new System.Drawing.Point(615, 174);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(40, 13);
            this.lblBuscar.TabIndex = 9;
            this.lblBuscar.Text = "Buscar";
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Location = new System.Drawing.Point(18, 274);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(84, 13);
            this.lblPreview.TabIndex = 2;
            this.lblPreview.Text = "Pré-visualização";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(21, 34);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(120, 20);
            this.txtCodigo.TabIndex = 20;
            this.txtCodigo.TextChanged += new System.EventHandler(this.CamposPreview_TextChanged);
            // 
            // txtNomeEmpresa
            // 
            this.txtNomeEmpresa.Location = new System.Drawing.Point(157, 34);
            this.txtNomeEmpresa.Name = "txtNomeEmpresa";
            this.txtNomeEmpresa.Size = new System.Drawing.Size(260, 20);
            this.txtNomeEmpresa.TabIndex = 34;
            this.txtNomeEmpresa.TextChanged += new System.EventHandler(this.CamposPreview_TextChanged);
            // 
            // txtTelefone
            // 
            this.txtTelefone.Location = new System.Drawing.Point(432, 34);
            this.txtTelefone.Name = "txtTelefone";
            this.txtTelefone.Size = new System.Drawing.Size(170, 20);
            this.txtTelefone.TabIndex = 32;
            this.txtTelefone.TextChanged += new System.EventHandler(this.CamposPreview_TextChanged);
            // 
            // txtTeleEntrega
            // 
            this.txtTeleEntrega.Location = new System.Drawing.Point(618, 34);
            this.txtTeleEntrega.Name = "txtTeleEntrega";
            this.txtTeleEntrega.Size = new System.Drawing.Size(329, 20);
            this.txtTeleEntrega.TabIndex = 30;
            this.txtTeleEntrega.TextChanged += new System.EventHandler(this.CamposPreview_TextChanged);
            // 
            // cmbLinhaFormatacao
            // 
            this.cmbLinhaFormatacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLinhaFormatacao.FormattingEnabled = true;
            this.cmbLinhaFormatacao.Location = new System.Drawing.Point(21, 86);
            this.cmbLinhaFormatacao.Name = "cmbLinhaFormatacao";
            this.cmbLinhaFormatacao.Size = new System.Drawing.Size(190, 21);
            this.cmbLinhaFormatacao.TabIndex = 25;
            // 
            // cmbFonte
            // 
            this.cmbFonte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFonte.FormattingEnabled = true;
            this.cmbFonte.Location = new System.Drawing.Point(236, 86);
            this.cmbFonte.Name = "cmbFonte";
            this.cmbFonte.Size = new System.Drawing.Size(350, 21);
            this.cmbFonte.TabIndex = 24;
            // 
            // numTamanhoFonte
            // 
            this.numTamanhoFonte.DecimalPlaces = 1;
            this.numTamanhoFonte.Location = new System.Drawing.Point(608, 86);
            this.numTamanhoFonte.Maximum = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.numTamanhoFonte.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTamanhoFonte.Name = "numTamanhoFonte";
            this.numTamanhoFonte.Size = new System.Drawing.Size(90, 20);
            this.numTamanhoFonte.TabIndex = 23;
            this.numTamanhoFonte.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // chkNegrito
            // 
            this.chkNegrito.AutoSize = true;
            this.chkNegrito.Location = new System.Drawing.Point(770, 86);
            this.chkNegrito.Name = "chkNegrito";
            this.chkNegrito.Size = new System.Drawing.Size(15, 14);
            this.chkNegrito.TabIndex = 22;
            this.chkNegrito.UseVisualStyleBackColor = true;
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(21, 138);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(280, 20);
            this.txtDescricao.TabIndex = 18;
            this.txtDescricao.TextChanged += new System.EventHandler(this.CamposPreview_TextChanged);
            // 
            // txtPreco
            // 
            this.txtPreco.Location = new System.Drawing.Point(318, 138);
            this.txtPreco.Name = "txtPreco";
            this.txtPreco.Size = new System.Drawing.Size(90, 20);
            this.txtPreco.TabIndex = 16;
            this.txtPreco.TextChanged += new System.EventHandler(this.CamposPreview_TextChanged);
            // 
            // txtObservacao
            // 
            this.txtObservacao.Location = new System.Drawing.Point(21, 190);
            this.txtObservacao.Name = "txtObservacao";
            this.txtObservacao.Size = new System.Drawing.Size(390, 20);
            this.txtObservacao.TabIndex = 10;
            this.txtObservacao.TextChanged += new System.EventHandler(this.CamposPreview_TextChanged);
            // 
            // txtLocal
            // 
            this.txtLocal.Location = new System.Drawing.Point(426, 190);
            this.txtLocal.Name = "txtLocal";
            this.txtLocal.Size = new System.Drawing.Size(155, 20);
            this.txtLocal.TabIndex = 37;
            this.txtLocal.TextChanged += new System.EventHandler(this.CamposPreview_TextChanged);
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(618, 190);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(329, 20);
            this.txtBuscar.TabIndex = 8;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // numQuantidade
            // 
            this.numQuantidade.Location = new System.Drawing.Point(432, 138);
            this.numQuantidade.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numQuantidade.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantidade.Name = "numQuantidade";
            this.numQuantidade.Size = new System.Drawing.Size(80, 20);
            this.numQuantidade.TabIndex = 14;
            this.numQuantidade.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbImpressora
            // 
            this.cmbImpressora.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImpressora.Location = new System.Drawing.Point(533, 138);
            this.cmbImpressora.Name = "cmbImpressora";
            this.cmbImpressora.Size = new System.Drawing.Size(414, 21);
            this.cmbImpressora.TabIndex = 12;
            // 
            // gridEtiquetas
            // 
            this.gridEtiquetas.AllowUserToAddRows = false;
            this.gridEtiquetas.AllowUserToDeleteRows = false;
            this.gridEtiquetas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridEtiquetas.Location = new System.Drawing.Point(470, 294);
            this.gridEtiquetas.MultiSelect = false;
            this.gridEtiquetas.Name = "gridEtiquetas";
            this.gridEtiquetas.ReadOnly = true;
            this.gridEtiquetas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridEtiquetas.Size = new System.Drawing.Size(477, 228);
            this.gridEtiquetas.TabIndex = 0;
            this.gridEtiquetas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridEtiquetas_CellClick);
            // 
            // pnlPreview
            // 
            this.pnlPreview.BackColor = System.Drawing.Color.White;
            this.pnlPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPreview.Location = new System.Drawing.Point(21, 294);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(430, 228);
            this.pnlPreview.TabIndex = 1;
            this.pnlPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPreview_Paint);
            this.pnlPreview.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlPreview_MouseClick);
            // 
            // btNovo
            // 
            this.btNovo.Location = new System.Drawing.Point(21, 228);
            this.btNovo.Name = "btNovo";
            this.btNovo.Size = new System.Drawing.Size(90, 28);
            this.btNovo.TabIndex = 7;
            this.btNovo.Text = "Novo";
            this.btNovo.Click += new System.EventHandler(this.btNovo_Click);
            // 
            // btSalvar
            // 
            this.btSalvar.Location = new System.Drawing.Point(121, 228);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(90, 28);
            this.btSalvar.TabIndex = 6;
            this.btSalvar.Text = "Salvar";
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // btExcluir
            // 
            this.btExcluir.Location = new System.Drawing.Point(221, 228);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(90, 28);
            this.btExcluir.TabIndex = 5;
            this.btExcluir.Text = "Excluir";
            this.btExcluir.Click += new System.EventHandler(this.btExcluir_Click);
            // 
            // btLimpar
            // 
            this.btLimpar.Location = new System.Drawing.Point(321, 228);
            this.btLimpar.Name = "btLimpar";
            this.btLimpar.Size = new System.Drawing.Size(90, 28);
            this.btLimpar.TabIndex = 4;
            this.btLimpar.Text = "Limpar";
            this.btLimpar.Click += new System.EventHandler(this.btLimpar_Click);
            // 
            // btImprimir
            // 
            this.btImprimir.Location = new System.Drawing.Point(421, 228);
            this.btImprimir.Name = "btImprimir";
            this.btImprimir.Size = new System.Drawing.Size(90, 28);
            this.btImprimir.TabIndex = 3;
            this.btImprimir.Text = "Imprimir";
            this.btImprimir.Click += new System.EventHandler(this.btImprimir_Click);
            // 
            // operEtiquetas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 541);
            this.Controls.Add(this.gridEtiquetas);
            this.Controls.Add(this.pnlPreview);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.btImprimir);
            this.Controls.Add(this.btLimpar);
            this.Controls.Add(this.btExcluir);
            this.Controls.Add(this.btSalvar);
            this.Controls.Add(this.btNovo);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.txtObservacao);
            this.Controls.Add(this.lblLocal);
            this.Controls.Add(this.txtLocal);
            this.Controls.Add(this.lblObservacao);
            this.Controls.Add(this.cmbImpressora);
            this.Controls.Add(this.lblImpressora);
            this.Controls.Add(this.numQuantidade);
            this.Controls.Add(this.lblQuantidade);
            this.Controls.Add(this.txtPreco);
            this.Controls.Add(this.lblPreco);
            this.Controls.Add(this.txtDescricao);
            this.Controls.Add(this.lblDescricao);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.chkNegrito);
            this.Controls.Add(this.numTamanhoFonte);
            this.Controls.Add(this.cmbFonte);
            this.Controls.Add(this.cmbLinhaFormatacao);
            this.Controls.Add(this.lblNegrito);
            this.Controls.Add(this.lblTamanho);
            this.Controls.Add(this.lblFonte);
            this.Controls.Add(this.lblLinha);
            this.Controls.Add(this.txtTeleEntrega);
            this.Controls.Add(this.lblTeleEntrega);
            this.Controls.Add(this.txtTelefone);
            this.Controls.Add(this.lblTelefone);
            this.Controls.Add(this.txtNomeEmpresa);
            this.Controls.Add(this.lblNomeEmpresa);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(985, 580);
            this.Name = "operEtiquetas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Etiquetas";
            this.Load += new System.EventHandler(this.operEtiquetas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numTamanhoFonte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEtiquetas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Label lblNomeEmpresa;
        private System.Windows.Forms.Label lblTelefone;
        private System.Windows.Forms.Label lblTeleEntrega;
        private System.Windows.Forms.Label lblLocal;
        private System.Windows.Forms.Label lblLinha;
        private System.Windows.Forms.Label lblFonte;
        private System.Windows.Forms.Label lblTamanho;
        private System.Windows.Forms.Label lblNegrito;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Label lblPreco;
        private System.Windows.Forms.Label lblObservacao;
        private System.Windows.Forms.Label lblQuantidade;
        private System.Windows.Forms.Label lblImpressora;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.TextBox txtNomeEmpresa;
        private System.Windows.Forms.TextBox txtTelefone;
        private System.Windows.Forms.TextBox txtTeleEntrega;
        private System.Windows.Forms.TextBox txtLocal;
        private System.Windows.Forms.ComboBox cmbLinhaFormatacao;
        private System.Windows.Forms.ComboBox cmbFonte;
        private System.Windows.Forms.NumericUpDown numTamanhoFonte;
        private System.Windows.Forms.CheckBox chkNegrito;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.TextBox txtPreco;
        private System.Windows.Forms.TextBox txtObservacao;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.NumericUpDown numQuantidade;
        private System.Windows.Forms.ComboBox cmbImpressora;
        private System.Windows.Forms.DataGridView gridEtiquetas;
        private System.Windows.Forms.Panel pnlPreview;
        private System.Windows.Forms.Button btNovo;
        private System.Windows.Forms.Button btSalvar;
        private System.Windows.Forms.Button btExcluir;
        private System.Windows.Forms.Button btLimpar;
        private System.Windows.Forms.Button btImprimir;
    }
}
