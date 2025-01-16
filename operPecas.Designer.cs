using System;

namespace TeleBonifacio
{
    partial class operPecas
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(operPecas));
            this.lstCarros = new System.Windows.Forms.ListBox();
            this.lstPecas = new System.Windows.Forms.ListBox();
            this.lstCaracteristicas = new System.Windows.Forms.ListBox();
            this.txtCarro = new System.Windows.Forms.TextBox();
            this.txtPeca = new System.Windows.Forms.TextBox();
            this.txtCaracteristica = new System.Windows.Forms.TextBox();
            this.btnAddCarro = new System.Windows.Forms.Button();
            this.btnAddPeca = new System.Windows.Forms.Button();
            this.btnAddCaracteristica = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstCarros
            // 
            this.lstCarros.Font = new System.Drawing.Font("Arial", 16F);
            this.lstCarros.FormattingEnabled = true;
            this.lstCarros.ItemHeight = 24;
            this.lstCarros.Location = new System.Drawing.Point(9, 37);
            this.lstCarros.Name = "lstCarros";
            this.lstCarros.Size = new System.Drawing.Size(294, 388);
            this.lstCarros.TabIndex = 0;
            this.lstCarros.SelectedIndexChanged += new System.EventHandler(this.lstCarros_SelectedIndexChanged);
            // 
            // lstPecas
            // 
            this.lstPecas.Font = new System.Drawing.Font("Arial", 16F);
            this.lstPecas.FormattingEnabled = true;
            this.lstPecas.ItemHeight = 24;
            this.lstPecas.Location = new System.Drawing.Point(309, 37);
            this.lstPecas.Name = "lstPecas";
            this.lstPecas.Size = new System.Drawing.Size(294, 388);
            this.lstPecas.TabIndex = 1;
            this.lstPecas.SelectedIndexChanged += new System.EventHandler(this.lstPecas_SelectedIndexChanged);
            // 
            // lstCaracteristicas
            // 
            this.lstCaracteristicas.Font = new System.Drawing.Font("Arial", 16F);
            this.lstCaracteristicas.FormattingEnabled = true;
            this.lstCaracteristicas.ItemHeight = 24;
            this.lstCaracteristicas.Location = new System.Drawing.Point(614, 37);
            this.lstCaracteristicas.Name = "lstCaracteristicas";
            this.lstCaracteristicas.Size = new System.Drawing.Size(294, 388);
            this.lstCaracteristicas.TabIndex = 2;
            // 
            // txtCarro
            // 
            this.txtCarro.Font = new System.Drawing.Font("Arial", 16F);
            this.txtCarro.Location = new System.Drawing.Point(9, 431);
            this.txtCarro.Name = "txtCarro";
            this.txtCarro.Size = new System.Drawing.Size(248, 32);
            this.txtCarro.TabIndex = 3;
            // 
            // txtPeca
            // 
            this.txtPeca.Font = new System.Drawing.Font("Arial", 16F);
            this.txtPeca.Location = new System.Drawing.Point(309, 431);
            this.txtPeca.Name = "txtPeca";
            this.txtPeca.Size = new System.Drawing.Size(248, 32);
            this.txtPeca.TabIndex = 5;
            // 
            // txtCaracteristica
            // 
            this.txtCaracteristica.Font = new System.Drawing.Font("Arial", 16F);
            this.txtCaracteristica.Location = new System.Drawing.Point(614, 431);
            this.txtCaracteristica.Name = "txtCaracteristica";
            this.txtCaracteristica.Size = new System.Drawing.Size(248, 32);
            this.txtCaracteristica.TabIndex = 7;
            // 
            // btnAddCarro
            // 
            this.btnAddCarro.Font = new System.Drawing.Font("Arial", 16F);
            this.btnAddCarro.Location = new System.Drawing.Point(263, 430);
            this.btnAddCarro.Name = "btnAddCarro";
            this.btnAddCarro.Size = new System.Drawing.Size(40, 32);
            this.btnAddCarro.TabIndex = 4;
            this.btnAddCarro.Text = "+";
            this.btnAddCarro.UseVisualStyleBackColor = true;
            this.btnAddCarro.Click += new System.EventHandler(this.btnAddCarro_Click);
            // 
            // btnAddPeca
            // 
            this.btnAddPeca.Font = new System.Drawing.Font("Arial", 16F);
            this.btnAddPeca.Location = new System.Drawing.Point(563, 431);
            this.btnAddPeca.Name = "btnAddPeca";
            this.btnAddPeca.Size = new System.Drawing.Size(40, 32);
            this.btnAddPeca.TabIndex = 6;
            this.btnAddPeca.Text = "+";
            this.btnAddPeca.UseVisualStyleBackColor = true;
            this.btnAddPeca.Click += new System.EventHandler(this.btnAddPeca_Click);
            // 
            // btnAddCaracteristica
            // 
            this.btnAddCaracteristica.Font = new System.Drawing.Font("Arial", 16F);
            this.btnAddCaracteristica.Location = new System.Drawing.Point(868, 430);
            this.btnAddCaracteristica.Name = "btnAddCaracteristica";
            this.btnAddCaracteristica.Size = new System.Drawing.Size(40, 32);
            this.btnAddCaracteristica.TabIndex = 8;
            this.btnAddCaracteristica.Text = "+";
            this.btnAddCaracteristica.UseVisualStyleBackColor = true;
            this.btnAddCaracteristica.Click += new System.EventHandler(this.btnAddCaracteristica_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "Carros";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(307, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(296, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "Peças";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(609, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(299, 23);
            this.label3.TabIndex = 11;
            this.label3.Text = "Características";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // operPecas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 475);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddCaracteristica);
            this.Controls.Add(this.btnAddPeca);
            this.Controls.Add(this.btnAddCarro);
            this.Controls.Add(this.txtCaracteristica);
            this.Controls.Add(this.txtPeca);
            this.Controls.Add(this.txtCarro);
            this.Controls.Add(this.lstCaracteristicas);
            this.Controls.Add(this.lstPecas);
            this.Controls.Add(this.lstCarros);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(597, 752);
            this.MaximizeBox = false;
            this.Name = "operPecas";
            this.Text = "Consulta de Peças";
            this.Load += new System.EventHandler(this.operPecas_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.ListBox lstCarros;
        private System.Windows.Forms.ListBox lstPecas;
        private System.Windows.Forms.ListBox lstCaracteristicas;
        private System.Windows.Forms.TextBox txtCarro;
        private System.Windows.Forms.TextBox txtPeca;
        private System.Windows.Forms.TextBox txtCaracteristica;
        private System.Windows.Forms.Button btnAddCarro;
        private System.Windows.Forms.Button btnAddPeca;
        private System.Windows.Forms.Button btnAddCaracteristica;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

