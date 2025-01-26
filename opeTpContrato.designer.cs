namespace TeleBonifacio
{
    partial class opeTpContrato
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(opeTpContrato));
            this.cmbTiposContrato = new System.Windows.Forms.ComboBox();
            this.lblTipoContrato = new System.Windows.Forms.Label();
            this.txtNovoTipoContrato = new System.Windows.Forms.TextBox();
            this.lblNovoTipoContrato = new System.Windows.Forms.Label();
            this.cmbCadastroAssociado = new System.Windows.Forms.ComboBox();
            this.lblCadastroAssociado = new System.Windows.Forms.Label();
            this.btnAdicionarTipo = new System.Windows.Forms.Button();
            this.lstClausulas = new System.Windows.Forms.DataGridView();
            this.ColunaNumero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaTexto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaAcoes = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblClausulas = new System.Windows.Forms.Label();
            this.txtNovaClausula = new System.Windows.Forms.TextBox();
            this.lblNovaClausula = new System.Windows.Forms.Label();
            this.btnSalvarClausula = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnExcluirClausula = new System.Windows.Forms.Button();
            this.btnAdicionarClausula = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lstClausulas)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbTiposContrato
            // 
            this.cmbTiposContrato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTiposContrato.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTiposContrato.FormattingEnabled = true;
            this.cmbTiposContrato.Location = new System.Drawing.Point(18, 55);
            this.cmbTiposContrato.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbTiposContrato.Name = "cmbTiposContrato";
            this.cmbTiposContrato.Size = new System.Drawing.Size(320, 29);
            this.cmbTiposContrato.TabIndex = 1;
            this.cmbTiposContrato.SelectedIndexChanged += new System.EventHandler(this.cmbTiposContrato_SelectedIndexChanged);
            // 
            // lblTipoContrato
            // 
            this.lblTipoContrato.AutoSize = true;
            this.lblTipoContrato.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoContrato.Location = new System.Drawing.Point(18, 21);
            this.lblTipoContrato.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTipoContrato.Name = "lblTipoContrato";
            this.lblTipoContrato.Size = new System.Drawing.Size(171, 20);
            this.lblTipoContrato.TabIndex = 2;
            this.lblTipoContrato.Text = "Selecione o Tipo Atual:";
            // 
            // txtNovoTipoContrato
            // 
            this.txtNovoTipoContrato.Location = new System.Drawing.Point(18, 138);
            this.txtNovoTipoContrato.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNovoTipoContrato.Name = "txtNovoTipoContrato";
            this.txtNovoTipoContrato.Size = new System.Drawing.Size(291, 26);
            this.txtNovoTipoContrato.TabIndex = 3;
            // 
            // lblNovoTipoContrato
            // 
            this.lblNovoTipoContrato.AutoSize = true;
            this.lblNovoTipoContrato.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNovoTipoContrato.Location = new System.Drawing.Point(18, 108);
            this.lblNovoTipoContrato.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNovoTipoContrato.Name = "lblNovoTipoContrato";
            this.lblNovoTipoContrato.Size = new System.Drawing.Size(160, 20);
            this.lblNovoTipoContrato.TabIndex = 4;
            this.lblNovoTipoContrato.Text = "Novo tipo de contrato";
            // 
            // cmbCadastroAssociado
            // 
            this.cmbCadastroAssociado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCadastroAssociado.FormattingEnabled = true;
            this.cmbCadastroAssociado.Items.AddRange(new object[] {
            "Funcionários",
            "Clientes",
            "Entregadores",
            "Fornecedores"});
            this.cmbCadastroAssociado.Location = new System.Drawing.Point(317, 136);
            this.cmbCadastroAssociado.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbCadastroAssociado.Name = "cmbCadastroAssociado";
            this.cmbCadastroAssociado.Size = new System.Drawing.Size(256, 28);
            this.cmbCadastroAssociado.TabIndex = 5;
            // 
            // lblCadastroAssociado
            // 
            this.lblCadastroAssociado.AutoSize = true;
            this.lblCadastroAssociado.Location = new System.Drawing.Point(313, 108);
            this.lblCadastroAssociado.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCadastroAssociado.Name = "lblCadastroAssociado";
            this.lblCadastroAssociado.Size = new System.Drawing.Size(216, 20);
            this.lblCadastroAssociado.TabIndex = 6;
            this.lblCadastroAssociado.Text = "Associar Dados ao Cadastro:";
            // 
            // btnAdicionarTipo
            // 
            this.btnAdicionarTipo.Location = new System.Drawing.Point(581, 133);
            this.btnAdicionarTipo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdicionarTipo.Name = "btnAdicionarTipo";
            this.btnAdicionarTipo.Size = new System.Drawing.Size(129, 31);
            this.btnAdicionarTipo.TabIndex = 7;
            this.btnAdicionarTipo.Text = "Adicionar Tipo";
            this.btnAdicionarTipo.UseVisualStyleBackColor = true;
            // 
            // lstClausulas
            // 
            this.lstClausulas.AllowUserToAddRows = false;
            this.lstClausulas.AllowUserToDeleteRows = false;
            this.lstClausulas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lstClausulas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColunaNumero,
            this.ColunaTexto,
            this.ColunaAcoes});
            this.lstClausulas.Location = new System.Drawing.Point(18, 206);
            this.lstClausulas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstClausulas.Name = "lstClausulas";
            this.lstClausulas.ReadOnly = true;
            this.lstClausulas.RowTemplate.Height = 25;
            this.lstClausulas.Size = new System.Drawing.Size(694, 200);
            this.lstClausulas.TabIndex = 8;
            this.lstClausulas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lstClausulas_CellClick);
            // 
            // ColunaNumero
            // 
            this.ColunaNumero.HeaderText = "#";
            this.ColunaNumero.Name = "ColunaNumero";
            this.ColunaNumero.ReadOnly = true;
            this.ColunaNumero.Width = 30;
            // 
            // ColunaTexto
            // 
            this.ColunaTexto.HeaderText = "Cláusula";
            this.ColunaTexto.Name = "ColunaTexto";
            this.ColunaTexto.ReadOnly = true;
            this.ColunaTexto.Width = 400;
            // 
            // ColunaAcoes
            // 
            this.ColunaAcoes.HeaderText = "Ações";
            this.ColunaAcoes.Name = "ColunaAcoes";
            this.ColunaAcoes.ReadOnly = true;
            this.ColunaAcoes.Text = "Editar/Remover";
            this.ColunaAcoes.UseColumnTextForButtonValue = true;
            this.ColunaAcoes.Width = 80;
            // 
            // lblClausulas
            // 
            this.lblClausulas.AutoSize = true;
            this.lblClausulas.Location = new System.Drawing.Point(18, 180);
            this.lblClausulas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClausulas.Name = "lblClausulas";
            this.lblClausulas.Size = new System.Drawing.Size(170, 20);
            this.lblClausulas.TabIndex = 9;
            this.lblClausulas.Text = "Cláusulas do Contrato:";
            // 
            // txtNovaClausula
            // 
            this.txtNovaClausula.Location = new System.Drawing.Point(18, 436);
            this.txtNovaClausula.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNovaClausula.Multiline = true;
            this.txtNovaClausula.Name = "txtNovaClausula";
            this.txtNovaClausula.Size = new System.Drawing.Size(692, 132);
            this.txtNovaClausula.TabIndex = 10;
            // 
            // lblNovaClausula
            // 
            this.lblNovaClausula.AutoSize = true;
            this.lblNovaClausula.Location = new System.Drawing.Point(18, 411);
            this.lblNovaClausula.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNovaClausula.Name = "lblNovaClausula";
            this.lblNovaClausula.Size = new System.Drawing.Size(114, 20);
            this.lblNovaClausula.TabIndex = 11;
            this.lblNovaClausula.Text = "Nova Cláusula:";
            // 
            // btnSalvarClausula
            // 
            this.btnSalvarClausula.Location = new System.Drawing.Point(292, 578);
            this.btnSalvarClausula.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSalvarClausula.Name = "btnSalvarClausula";
            this.btnSalvarClausula.Size = new System.Drawing.Size(129, 31);
            this.btnSalvarClausula.TabIndex = 12;
            this.btnSalvarClausula.Text = "Salvar Cláusula";
            this.btnSalvarClausula.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(517, 578);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(193, 31);
            this.button1.TabIndex = 13;
            this.button1.Text = "Salvar Tipo de Contrato";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnExcluirClausula
            // 
            this.btnExcluirClausula.Location = new System.Drawing.Point(155, 580);
            this.btnExcluirClausula.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExcluirClausula.Name = "btnExcluirClausula";
            this.btnExcluirClausula.Size = new System.Drawing.Size(129, 31);
            this.btnExcluirClausula.TabIndex = 14;
            this.btnExcluirClausula.Text = "Excluir Cláusula";
            this.btnExcluirClausula.UseVisualStyleBackColor = true;
            this.btnExcluirClausula.Click += new System.EventHandler(this.btnExcluirClausula_Click);
            // 
            // btnAdicionarClausula
            // 
            this.btnAdicionarClausula.Location = new System.Drawing.Point(18, 578);
            this.btnAdicionarClausula.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdicionarClausula.Name = "btnAdicionarClausula";
            this.btnAdicionarClausula.Size = new System.Drawing.Size(129, 31);
            this.btnAdicionarClausula.TabIndex = 15;
            this.btnAdicionarClausula.Text = "Adicionar Cláusula";
            this.btnAdicionarClausula.UseVisualStyleBackColor = true;
            // 
            // opeTpContrato
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 623);
            this.Controls.Add(this.btnAdicionarClausula);
            this.Controls.Add(this.btnExcluirClausula);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSalvarClausula);
            this.Controls.Add(this.lblNovaClausula);
            this.Controls.Add(this.txtNovaClausula);
            this.Controls.Add(this.lblClausulas);
            this.Controls.Add(this.lstClausulas);
            this.Controls.Add(this.btnAdicionarTipo);
            this.Controls.Add(this.lblCadastroAssociado);
            this.Controls.Add(this.cmbCadastroAssociado);
            this.Controls.Add(this.lblNovoTipoContrato);
            this.Controls.Add(this.txtNovoTipoContrato);
            this.Controls.Add(this.lblTipoContrato);
            this.Controls.Add(this.cmbTiposContrato);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "opeTpContrato";
            this.Text = "Configuração de Tipos de Contratos";
            ((System.ComponentModel.ISupportInitialize)(this.lstClausulas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbTiposContrato;
        private System.Windows.Forms.Label lblTipoContrato;
        private System.Windows.Forms.TextBox txtNovoTipoContrato;
        private System.Windows.Forms.Label lblNovoTipoContrato;
        private System.Windows.Forms.ComboBox cmbCadastroAssociado;
        private System.Windows.Forms.Label lblCadastroAssociado;
        private System.Windows.Forms.Button btnAdicionarTipo;
        private System.Windows.Forms.DataGridView lstClausulas;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaNumero;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaTexto;
        private System.Windows.Forms.DataGridViewButtonColumn ColunaAcoes;
        private System.Windows.Forms.Label lblClausulas;
        private System.Windows.Forms.TextBox txtNovaClausula;
        private System.Windows.Forms.Label lblNovaClausula;
        private System.Windows.Forms.Button btnSalvarClausula;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnExcluirClausula;
        private System.Windows.Forms.Button btnAdicionarClausula;
    }
}
