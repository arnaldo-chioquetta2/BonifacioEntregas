using System;

namespace TeleBonifacio
{
    partial class operPartilheira
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txCodigo;
        private System.Windows.Forms.Button btAdicionar;

        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txBuscar;

        private System.Windows.Forms.DataGridView gridCodigos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;

        private System.Windows.Forms.Button btExcluir;
        private System.Windows.Forms.Button btLimpar;
        private System.Windows.Forms.Button btImprimir;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(operPartilheira));
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txCodigo = new System.Windows.Forms.TextBox();
            this.btAdicionar = new System.Windows.Forms.Button();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txBuscar = new System.Windows.Forms.TextBox();
            this.gridCodigos = new System.Windows.Forms.DataGridView();
            this.colCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btExcluir = new System.Windows.Forms.Button();
            this.btLimpar = new System.Windows.Forms.Button();
            this.btImprimir = new System.Windows.Forms.Button();
            this.btImprLista = new System.Windows.Forms.Button();
            this.btReiniciar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridCodigos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(17, 17);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(40, 13);
            this.lblCodigo.TabIndex = 8;
            this.lblCodigo.Text = "Código";
            // 
            // txCodigo
            // 
            this.txCodigo.Location = new System.Drawing.Point(69, 15);
            this.txCodigo.Name = "txCodigo";
            this.txCodigo.Size = new System.Drawing.Size(172, 20);
            this.txCodigo.TabIndex = 7;
            this.txCodigo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txCodigo_KeyDown);
            // 
            // btAdicionar
            // 
            this.btAdicionar.Location = new System.Drawing.Point(249, 14);
            this.btAdicionar.Name = "btAdicionar";
            this.btAdicionar.Size = new System.Drawing.Size(86, 22);
            this.btAdicionar.TabIndex = 6;
            this.btAdicionar.Text = "Adicionar";
            this.btAdicionar.UseVisualStyleBackColor = true;
            this.btAdicionar.Click += new System.EventHandler(this.btAdicionar_Click);
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new System.Drawing.Point(17, 48);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(40, 13);
            this.lblBuscar.TabIndex = 5;
            this.lblBuscar.Text = "Buscar";
            // 
            // txBuscar
            // 
            this.txBuscar.Location = new System.Drawing.Point(69, 45);
            this.txBuscar.Name = "txBuscar";
            this.txBuscar.Size = new System.Drawing.Size(266, 20);
            this.txBuscar.TabIndex = 4;
            this.txBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txBuscar_KeyDown);
            // 
            // gridCodigos
            // 
            this.gridCodigos.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.gridCodigos.AllowUserToAddRows = false;
            this.gridCodigos.AllowUserToDeleteRows = false;
            this.gridCodigos.AllowUserToResizeRows = false;
            this.gridCodigos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCodigos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridCodigos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCodigos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCodigo});
            this.gridCodigos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridCodigos.Location = new System.Drawing.Point(17, 70);
            this.gridCodigos.MultiSelect = false;
            this.gridCodigos.Name = "gridCodigos";
            this.gridCodigos.RowHeadersVisible = false;
            this.gridCodigos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCodigos.Size = new System.Drawing.Size(651, 280);
            this.gridCodigos.TabIndex = 3;
            this.gridCodigos.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gridCodigos_CellBeginEdit);
            this.gridCodigos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCodigos_CellClick);
            this.gridCodigos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCodigos_CellDoubleClick);
            this.gridCodigos.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCodigos_CellEndEdit);
            this.gridCodigos.CurrentCellChanged += new System.EventHandler(this.gridCodigos_CurrentCellChanged);
            this.gridCodigos.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.gridCodigos_EditingControlShowing);
            this.gridCodigos.SelectionChanged += new System.EventHandler(this.gridCodigos_SelectionChanged);
            this.gridCodigos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridCodigos_KeyDown);
            // 
            // colCodigo
            // 
            this.colCodigo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCodigo.HeaderText = "Código";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btExcluir
            // 
            this.btExcluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btExcluir.Location = new System.Drawing.Point(17, 365);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(77, 26);
            this.btExcluir.TabIndex = 2;
            this.btExcluir.Text = "Excluir";
            this.btExcluir.UseVisualStyleBackColor = true;
            this.btExcluir.Click += new System.EventHandler(this.btExcluir_Click);
            // 
            // btLimpar
            // 
            this.btLimpar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btLimpar.Location = new System.Drawing.Point(103, 365);
            this.btLimpar.Name = "btLimpar";
            this.btLimpar.Size = new System.Drawing.Size(86, 26);
            this.btLimpar.TabIndex = 1;
            this.btLimpar.Text = "Limpar Lista";
            this.btLimpar.UseVisualStyleBackColor = true;
            this.btLimpar.Click += new System.EventHandler(this.btLimpar_Click);
            // 
            // btImprimir
            // 
            this.btImprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btImprimir.Location = new System.Drawing.Point(591, 365);
            this.btImprimir.Name = "btImprimir";
            this.btImprimir.Size = new System.Drawing.Size(77, 26);
            this.btImprimir.TabIndex = 0;
            this.btImprimir.Text = "Etiqueta";
            this.btImprimir.UseVisualStyleBackColor = true;
            this.btImprimir.Click += new System.EventHandler(this.btImprimir_Click);
            // 
            // btImprLista
            // 
            this.btImprLista.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btImprLista.Location = new System.Drawing.Point(508, 365);
            this.btImprLista.Name = "btImprLista";
            this.btImprLista.Size = new System.Drawing.Size(77, 26);
            this.btImprLista.TabIndex = 9;
            this.btImprLista.Text = "Lista";
            this.btImprLista.UseVisualStyleBackColor = true;
            this.btImprLista.Click += new System.EventHandler(this.btImprLista_Click);
            // 
            // btReiniciar
            // 
            this.btReiniciar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btReiniciar.Location = new System.Drawing.Point(195, 365);
            this.btReiniciar.Name = "btReiniciar";
            this.btReiniciar.Size = new System.Drawing.Size(77, 26);
            this.btReiniciar.TabIndex = 10;
            this.btReiniciar.Text = "Limpar";
            this.btReiniciar.UseVisualStyleBackColor = true;
            this.btReiniciar.Click += new System.EventHandler(this.btReiniciar_Click);
            // 
            // operPartilheira
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 417);
            this.Controls.Add(this.btReiniciar);
            this.Controls.Add(this.btImprLista);
            this.Controls.Add(this.gridCodigos);
            this.Controls.Add(this.btImprimir);
            this.Controls.Add(this.btLimpar);
            this.Controls.Add(this.btExcluir);
            this.Controls.Add(this.txBuscar);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.btAdicionar);
            this.Controls.Add(this.txCodigo);
            this.Controls.Add(this.lblCodigo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(619, 456);
            this.Name = "operPartilheira";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Organização de Partilheira";
            this.Load += new System.EventHandler(this.operPartilheira_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridCodigos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btImprLista;
        private System.Windows.Forms.Button btReiniciar;
    }
}