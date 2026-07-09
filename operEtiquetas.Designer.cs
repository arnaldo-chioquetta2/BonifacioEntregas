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
            this.lblCodigo = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.lblPreco = new System.Windows.Forms.Label();
            this.lblObservacao = new System.Windows.Forms.Label();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.lblImpressora = new System.Windows.Forms.Label();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEtiquetas)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(18, 18);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(43, 13);
            this.lblCodigo.Text = "Código";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(21, 34);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(120, 20);
            // 
            // lblDescricao
            // 
            this.lblDescricao.AutoSize = true;
            this.lblDescricao.Location = new System.Drawing.Point(154, 18);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(55, 13);
            this.lblDescricao.Text = "Descrição";
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(157, 34);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(320, 20);
            // 
            // lblPreco
            // 
            this.lblPreco.AutoSize = true;
            this.lblPreco.Location = new System.Drawing.Point(492, 18);
            this.lblPreco.Name = "lblPreco";
            this.lblPreco.Size = new System.Drawing.Size(35, 13);
            this.lblPreco.Text = "Preço";
            // 
            // txtPreco
            // 
            this.txtPreco.Location = new System.Drawing.Point(495, 34);
            this.txtPreco.Name = "txtPreco";
            this.txtPreco.Size = new System.Drawing.Size(90, 20);
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.Location = new System.Drawing.Point(604, 18);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(62, 13);
            this.lblQuantidade.Text = "Quantidade";
            // 
            // numQuantidade
            // 
            this.numQuantidade.Location = new System.Drawing.Point(607, 34);
            this.numQuantidade.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            this.numQuantidade.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numQuantidade.Name = "numQuantidade";
            this.numQuantidade.Size = new System.Drawing.Size(80, 20);
            this.numQuantidade.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblImpressora
            // 
            this.lblImpressora.AutoSize = true;
            this.lblImpressora.Location = new System.Drawing.Point(704, 18);
            this.lblImpressora.Name = "lblImpressora";
            this.lblImpressora.Size = new System.Drawing.Size(60, 13);
            this.lblImpressora.Text = "Impressora";
            // 
            // cmbImpressora
            // 
            this.cmbImpressora.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImpressora.Location = new System.Drawing.Point(707, 34);
            this.cmbImpressora.Name = "cmbImpressora";
            this.cmbImpressora.Size = new System.Drawing.Size(240, 21);
            // 
            // lblObservacao
            // 
            this.lblObservacao.AutoSize = true;
            this.lblObservacao.Location = new System.Drawing.Point(18, 70);
            this.lblObservacao.Name = "lblObservacao";
            this.lblObservacao.Size = new System.Drawing.Size(65, 13);
            this.lblObservacao.Text = "Observação";
            // 
            // txtObservacao
            // 
            this.txtObservacao.Location = new System.Drawing.Point(21, 86);
            this.txtObservacao.Name = "txtObservacao";
            this.txtObservacao.Size = new System.Drawing.Size(560, 20);
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new System.Drawing.Point(604, 70);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(40, 13);
            this.lblBuscar.Text = "Buscar";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(607, 86);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(340, 20);
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // btNovo
            // 
            this.btNovo.Location = new System.Drawing.Point(21, 124);
            this.btNovo.Name = "btNovo";
            this.btNovo.Size = new System.Drawing.Size(90, 28);
            this.btNovo.Text = "Novo";
            this.btNovo.Click += new System.EventHandler(this.btNovo_Click);
            // 
            // btSalvar
            // 
            this.btSalvar.Location = new System.Drawing.Point(121, 124);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(90, 28);
            this.btSalvar.Text = "Salvar";
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // btExcluir
            // 
            this.btExcluir.Location = new System.Drawing.Point(221, 124);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(90, 28);
            this.btExcluir.Text = "Excluir";
            this.btExcluir.Click += new System.EventHandler(this.btExcluir_Click);
            // 
            // btLimpar
            // 
            this.btLimpar.Location = new System.Drawing.Point(321, 124);
            this.btLimpar.Name = "btLimpar";
            this.btLimpar.Size = new System.Drawing.Size(90, 28);
            this.btLimpar.Text = "Limpar";
            this.btLimpar.Click += new System.EventHandler(this.btLimpar_Click);
            // 
            // btImprimir
            // 
            this.btImprimir.Location = new System.Drawing.Point(421, 124);
            this.btImprimir.Name = "btImprimir";
            this.btImprimir.Size = new System.Drawing.Size(90, 28);
            this.btImprimir.Text = "Imprimir";
            this.btImprimir.Click += new System.EventHandler(this.btImprimir_Click);
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Location = new System.Drawing.Point(18, 170);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(88, 13);
            this.lblPreview.Text = "Pré-visualização";
            // 
            // pnlPreview
            // 
            this.pnlPreview.BackColor = System.Drawing.Color.White;
            this.pnlPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPreview.Location = new System.Drawing.Point(21, 190);
            this.pnlPreview.Name = "pnlPreview";
            this.pnlPreview.Size = new System.Drawing.Size(430, 330);
            this.pnlPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPreview_Paint);
            // 
            // gridEtiquetas
            // 
            this.gridEtiquetas.AllowUserToAddRows = false;
            this.gridEtiquetas.AllowUserToDeleteRows = false;
            this.gridEtiquetas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridEtiquetas.Location = new System.Drawing.Point(470, 190);
            this.gridEtiquetas.MultiSelect = false;
            this.gridEtiquetas.Name = "gridEtiquetas";
            this.gridEtiquetas.ReadOnly = true;
            this.gridEtiquetas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridEtiquetas.Size = new System.Drawing.Size(477, 330);
            this.gridEtiquetas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridEtiquetas_CellClick);
            // 
            // operEtiquetas
            // 
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
            this.MinimumSize = new System.Drawing.Size(985, 580);
            this.Name = "operEtiquetas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Etiquetas";
            this.Load += new System.EventHandler(this.operEtiquetas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEtiquetas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Label lblDescricao;
        private System.Windows.Forms.Label lblPreco;
        private System.Windows.Forms.Label lblObservacao;
        private System.Windows.Forms.Label lblQuantidade;
        private System.Windows.Forms.Label lblImpressora;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.TextBox txtCodigo;
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
