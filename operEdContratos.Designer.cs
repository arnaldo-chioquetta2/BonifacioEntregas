namespace TeleBonifacio
{
    partial class operEdContratos
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
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.lblValor = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dtpInicio = new System.Windows.Forms.DateTimePicker();
            this.dtpFim = new System.Windows.Forms.DateTimePicker();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.txtObservacoes = new System.Windows.Forms.TextBox();
            this.lblObservacoes = new System.Windows.Forms.Label();
            this.lstClausulas = new System.Windows.Forms.ListBox();
            this.lblClausulas = new System.Windows.Forms.Label();
            this.txtEditarAdicionar = new System.Windows.Forms.TextBox();
            this.lblEditarClausula = new System.Windows.Forms.Label();
            this.btnAddClausula = new System.Windows.Forms.Button();
            this.btnEditClausula = new System.Windows.Forms.Button();
            this.btnRemoveClausula = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblContratoId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbMotoboy = new System.Windows.Forms.ComboBox();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.cmbTipoContrato = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtDescricao
            // 
            this.txtDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtDescricao.Location = new System.Drawing.Point(121, 100);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(459, 26);
            this.txtDescricao.TabIndex = 0;
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblDescricao.Location = new System.Drawing.Point(7, 103);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(84, 20);
            this.lblDescricao.TabIndex = 1;
            this.lblDescricao.Text = "Descrição:";
            // 
            // txtValor
            // 
            this.txtValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtValor.Location = new System.Drawing.Point(121, 140);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(200, 26);
            this.txtValor.TabIndex = 2;
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblValor.Location = new System.Drawing.Point(7, 143);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(50, 20);
            this.lblValor.TabIndex = 3;
            this.lblValor.Text = "Valor:";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "Ativo",
            "Cancelado",
            "Finalizado"});
            this.cmbStatus.Location = new System.Drawing.Point(121, 180);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(200, 28);
            this.cmbStatus.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblStatus.Location = new System.Drawing.Point(7, 183);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 20);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Status:";
            // 
            // dtpInicio
            // 
            this.dtpInicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInicio.Location = new System.Drawing.Point(121, 220);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.Size = new System.Drawing.Size(120, 26);
            this.dtpInicio.TabIndex = 6;
            // 
            // dtpFim
            // 
            this.dtpFim.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpFim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFim.Location = new System.Drawing.Point(281, 220);
            this.dtpFim.Name = "dtpFim";
            this.dtpFim.Size = new System.Drawing.Size(120, 26);
            this.dtpFim.TabIndex = 7;
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblPeriodo.Location = new System.Drawing.Point(7, 223);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(67, 20);
            this.lblPeriodo.TabIndex = 8;
            this.lblPeriodo.Text = "Período:";
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtObservacoes.Location = new System.Drawing.Point(121, 257);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.Size = new System.Drawing.Size(459, 60);
            this.txtObservacoes.TabIndex = 9;
            // 
            // lblObservacoes
            // 
            this.lblObservacoes.AutoSize = true;
            this.lblObservacoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblObservacoes.Location = new System.Drawing.Point(7, 260);
            this.lblObservacoes.Name = "lblObservacoes";
            this.lblObservacoes.Size = new System.Drawing.Size(106, 20);
            this.lblObservacoes.TabIndex = 10;
            this.lblObservacoes.Text = "Observações:";
            // 
            // lstClausulas
            // 
            this.lstClausulas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lstClausulas.FormattingEnabled = true;
            this.lstClausulas.ItemHeight = 20;
            this.lstClausulas.Location = new System.Drawing.Point(121, 337);
            this.lstClausulas.Name = "lstClausulas";
            this.lstClausulas.Size = new System.Drawing.Size(459, 124);
            this.lstClausulas.TabIndex = 11;
            this.lstClausulas.SelectedIndexChanged += new System.EventHandler(this.lstClausulas_SelectedIndexChanged);
            // 
            // lblClausulas
            // 
            this.lblClausulas.AutoSize = true;
            this.lblClausulas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblClausulas.Location = new System.Drawing.Point(7, 337);
            this.lblClausulas.Name = "lblClausulas";
            this.lblClausulas.Size = new System.Drawing.Size(82, 20);
            this.lblClausulas.TabIndex = 12;
            this.lblClausulas.Text = "Cláusulas:";
            // 
            // txtEditarAdicionar
            // 
            this.txtEditarAdicionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtEditarAdicionar.Location = new System.Drawing.Point(124, 506);
            this.txtEditarAdicionar.Multiline = true;
            this.txtEditarAdicionar.Name = "txtEditarAdicionar";
            this.txtEditarAdicionar.Size = new System.Drawing.Size(456, 122);
            this.txtEditarAdicionar.TabIndex = 13;
            // 
            // lblEditarClausula
            // 
            this.lblEditarClausula.AutoSize = true;
            this.lblEditarClausula.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblEditarClausula.Location = new System.Drawing.Point(-4, 506);
            this.lblEditarClausula.Name = "lblEditarClausula";
            this.lblEditarClausula.Size = new System.Drawing.Size(125, 20);
            this.lblEditarClausula.TabIndex = 14;
            this.lblEditarClausula.Text = "Editar/Adicionar:";
            // 
            // btnAddClausula
            // 
            this.btnAddClausula.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAddClausula.Location = new System.Drawing.Point(121, 467);
            this.btnAddClausula.Name = "btnAddClausula";
            this.btnAddClausula.Size = new System.Drawing.Size(100, 30);
            this.btnAddClausula.TabIndex = 15;
            this.btnAddClausula.Text = "Adicionar";
            this.btnAddClausula.UseVisualStyleBackColor = true;
            this.btnAddClausula.Click += new System.EventHandler(this.btnAdicionarClausula_Click);
            // 
            // btnEditClausula
            // 
            this.btnEditClausula.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnEditClausula.Location = new System.Drawing.Point(227, 467);
            this.btnEditClausula.Name = "btnEditClausula";
            this.btnEditClausula.Size = new System.Drawing.Size(100, 30);
            this.btnEditClausula.TabIndex = 16;
            this.btnEditClausula.Text = "Editar";
            this.btnEditClausula.UseVisualStyleBackColor = true;
            this.btnEditClausula.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnRemoveClausula
            // 
            this.btnRemoveClausula.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnRemoveClausula.Location = new System.Drawing.Point(336, 467);
            this.btnRemoveClausula.Name = "btnRemoveClausula";
            this.btnRemoveClausula.Size = new System.Drawing.Size(100, 30);
            this.btnRemoveClausula.TabIndex = 17;
            this.btnRemoveClausula.Text = "Remover";
            this.btnRemoveClausula.UseVisualStyleBackColor = true;
            this.btnRemoveClausula.Click += new System.EventHandler(this.btnRemover_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSalvar.Location = new System.Drawing.Point(124, 634);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(100, 30);
            this.btnSalvar.TabIndex = 18;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancelar.Location = new System.Drawing.Point(336, 634);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 30);
            this.btnCancelar.TabIndex = 19;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblContratoId
            // 
            this.lblContratoId.AutoSize = true;
            this.lblContratoId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblContratoId.Location = new System.Drawing.Point(329, 188);
            this.lblContratoId.Name = "lblContratoId";
            this.lblContratoId.Size = new System.Drawing.Size(79, 20);
            this.lblContratoId.TabIndex = 20;
            this.lblContratoId.Text = "Contrato: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(10, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Contratado:";
            // 
            // cmbMotoboy
            // 
            this.cmbMotoboy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMotoboy.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMotoboy.FormattingEnabled = true;
            this.cmbMotoboy.Location = new System.Drawing.Point(121, 55);
            this.cmbMotoboy.Name = "cmbMotoboy";
            this.cmbMotoboy.Size = new System.Drawing.Size(306, 33);
            this.cmbMotoboy.TabIndex = 22;
            // 
            // btnImprimir
            // 
            this.btnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImprimir.Location = new System.Drawing.Point(230, 634);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(100, 30);
            this.btnImprimir.TabIndex = 24;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // cmbTipoContrato
            // 
            this.cmbTipoContrato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoContrato.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipoContrato.FormattingEnabled = true;
            this.cmbTipoContrato.Location = new System.Drawing.Point(124, 12);
            this.cmbTipoContrato.Name = "cmbTipoContrato";
            this.cmbTipoContrato.Size = new System.Drawing.Size(303, 33);
            this.cmbTipoContrato.TabIndex = 26;
            this.cmbTipoContrato.SelectedIndexChanged += new System.EventHandler(this.cmbTipoContrato_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(14, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 25;
            this.label2.Text = "Tipo:";
            // 
            // operEdContratos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 669);
            this.Controls.Add(this.cmbTipoContrato);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.cmbMotoboy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblContratoId);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnRemoveClausula);
            this.Controls.Add(this.btnEditClausula);
            this.Controls.Add(this.btnAddClausula);
            this.Controls.Add(this.lblEditarClausula);
            this.Controls.Add(this.txtEditarAdicionar);
            this.Controls.Add(this.lblClausulas);
            this.Controls.Add(this.lstClausulas);
            this.Controls.Add(this.lblObservacoes);
            this.Controls.Add(this.txtObservacoes);
            this.Controls.Add(this.lblPeriodo);
            this.Controls.Add(this.dtpFim);
            this.Controls.Add(this.dtpInicio);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblValor);
            this.Controls.Add(this.txtValor);
            this.Controls.Add(this.lblDescricao);
            this.Controls.Add(this.txtDescricao);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "operEdContratos";
            this.Text = "Edição de Contratos";
            this.Load += new System.EventHandler(this.operEdContratos_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Label lblValor;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DateTimePicker dtpInicio;
        private System.Windows.Forms.DateTimePicker dtpFim;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.TextBox txtObservacoes;
        private System.Windows.Forms.Label lblObservacoes;
        private System.Windows.Forms.ListBox lstClausulas;
        private System.Windows.Forms.Label lblClausulas;
        private System.Windows.Forms.TextBox txtEditarAdicionar;
        private System.Windows.Forms.Label lblEditarClausula;
        private System.Windows.Forms.Button btnAddClausula;
        private System.Windows.Forms.Button btnEditClausula;
        private System.Windows.Forms.Button btnRemoveClausula;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblContratoId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbMotoboy;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.ComboBox cmbTipoContrato;
        private System.Windows.Forms.Label label2;
    }
}