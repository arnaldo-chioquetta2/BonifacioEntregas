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
            this.txtClausula = new System.Windows.Forms.TextBox();
            this.lblEditarClausula = new System.Windows.Forms.Label();
            this.btnAddClausula = new System.Windows.Forms.Button();
            this.btnEditClausula = new System.Windows.Forms.Button();
            this.btnRemoveClausula = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDescricao
            // 
            this.txtDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtDescricao.Location = new System.Drawing.Point(140, 20);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(400, 26);
            this.txtDescricao.TabIndex = 0;
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblDescricao.Location = new System.Drawing.Point(12, 23);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(84, 20);
            this.lblDescricao.TabIndex = 1;
            this.lblDescricao.Text = "Descrição:";
            // 
            // txtValor
            // 
            this.txtValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtValor.Location = new System.Drawing.Point(140, 60);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(200, 26);
            this.txtValor.TabIndex = 2;
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblValor.Location = new System.Drawing.Point(12, 63);
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
            this.cmbStatus.Location = new System.Drawing.Point(140, 100);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(200, 28);
            this.cmbStatus.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblStatus.Location = new System.Drawing.Point(12, 103);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 20);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Status:";
            // 
            // dtpInicio
            // 
            this.dtpInicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInicio.Location = new System.Drawing.Point(140, 140);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.Size = new System.Drawing.Size(120, 26);
            this.dtpInicio.TabIndex = 6;
            // 
            // dtpFim
            // 
            this.dtpFim.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dtpFim.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFim.Location = new System.Drawing.Point(300, 140);
            this.dtpFim.Name = "dtpFim";
            this.dtpFim.Size = new System.Drawing.Size(120, 26);
            this.dtpFim.TabIndex = 7;
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblPeriodo.Location = new System.Drawing.Point(12, 143);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(67, 20);
            this.lblPeriodo.TabIndex = 8;
            this.lblPeriodo.Text = "Período:";
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtObservacoes.Location = new System.Drawing.Point(140, 177);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.Size = new System.Drawing.Size(400, 60);
            this.txtObservacoes.TabIndex = 9;
            // 
            // lblObservacoes
            // 
            this.lblObservacoes.AutoSize = true;
            this.lblObservacoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblObservacoes.Location = new System.Drawing.Point(12, 180);
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
            this.lstClausulas.Location = new System.Drawing.Point(140, 257);
            this.lstClausulas.Name = "lstClausulas";
            this.lstClausulas.Size = new System.Drawing.Size(400, 124);
            this.lstClausulas.TabIndex = 11;
            // 
            // lblClausulas
            // 
            this.lblClausulas.AutoSize = true;
            this.lblClausulas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblClausulas.Location = new System.Drawing.Point(12, 257);
            this.lblClausulas.Name = "lblClausulas";
            this.lblClausulas.Size = new System.Drawing.Size(82, 20);
            this.lblClausulas.TabIndex = 12;
            this.lblClausulas.Text = "Cláusulas:";
            // 
            // txtClausula
            // 
            this.txtClausula.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtClausula.Location = new System.Drawing.Point(143, 426);
            this.txtClausula.Multiline = true;
            this.txtClausula.Name = "txtClausula";
            this.txtClausula.Size = new System.Drawing.Size(400, 60);
            this.txtClausula.TabIndex = 13;
            // 
            // lblEditarClausula
            // 
            this.lblEditarClausula.AutoSize = true;
            this.lblEditarClausula.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblEditarClausula.Location = new System.Drawing.Point(15, 426);
            this.lblEditarClausula.Name = "lblEditarClausula";
            this.lblEditarClausula.Size = new System.Drawing.Size(125, 20);
            this.lblEditarClausula.TabIndex = 14;
            this.lblEditarClausula.Text = "Editar/Adicionar:";
            // 
            // btnAddClausula
            // 
            this.btnAddClausula.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnAddClausula.Location = new System.Drawing.Point(140, 387);
            this.btnAddClausula.Name = "btnAddClausula";
            this.btnAddClausula.Size = new System.Drawing.Size(100, 30);
            this.btnAddClausula.TabIndex = 15;
            this.btnAddClausula.Text = "Adicionar";
            this.btnAddClausula.UseVisualStyleBackColor = true;
            // 
            // btnEditClausula
            // 
            this.btnEditClausula.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnEditClausula.Location = new System.Drawing.Point(246, 387);
            this.btnEditClausula.Name = "btnEditClausula";
            this.btnEditClausula.Size = new System.Drawing.Size(100, 30);
            this.btnEditClausula.TabIndex = 16;
            this.btnEditClausula.Text = "Editar";
            this.btnEditClausula.UseVisualStyleBackColor = true;
            // 
            // btnRemoveClausula
            // 
            this.btnRemoveClausula.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnRemoveClausula.Location = new System.Drawing.Point(352, 387);
            this.btnRemoveClausula.Name = "btnRemoveClausula";
            this.btnRemoveClausula.Size = new System.Drawing.Size(100, 30);
            this.btnRemoveClausula.TabIndex = 17;
            this.btnRemoveClausula.Text = "Remover";
            this.btnRemoveClausula.UseVisualStyleBackColor = true;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSalvar.Location = new System.Drawing.Point(232, 492);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(100, 30);
            this.btnSalvar.TabIndex = 18;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancelar.Location = new System.Drawing.Point(352, 492);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 30);
            this.btnCancelar.TabIndex = 19;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // operEdContratos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 531);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnRemoveClausula);
            this.Controls.Add(this.btnEditClausula);
            this.Controls.Add(this.btnAddClausula);
            this.Controls.Add(this.lblEditarClausula);
            this.Controls.Add(this.txtClausula);
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
        private System.Windows.Forms.TextBox txtClausula;
        private System.Windows.Forms.Label lblEditarClausula;
        private System.Windows.Forms.Button btnAddClausula;
        private System.Windows.Forms.Button btnEditClausula;
        private System.Windows.Forms.Button btnRemoveClausula;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;
    }
}