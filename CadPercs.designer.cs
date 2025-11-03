
namespace TeleBonifacio
{
    partial class CadPercs
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdSemanal = new System.Windows.Forms.RadioButton();
            this.rdQuinzenal = new System.Windows.Forms.RadioButton();
            this.rdMensal = new System.Windows.Forms.RadioButton();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btAdic = new System.Windows.Forms.Button();
            this.txValor = new System.Windows.Forms.TextBox();
            this.txPerc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnExcluir);
            this.panel1.Controls.Add(this.btAdic);
            this.panel1.Controls.Add(this.txValor);
            this.panel1.Controls.Add(this.txPerc);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 121);
            this.panel1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdSemanal);
            this.groupBox1.Controls.Add(this.rdQuinzenal);
            this.groupBox1.Controls.Add(this.rdMensal);
            this.groupBox1.Location = new System.Drawing.Point(3, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 38);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // rdSemanal
            // 
            this.rdSemanal.AutoSize = true;
            this.rdSemanal.Location = new System.Drawing.Point(234, 10);
            this.rdSemanal.Name = "rdSemanal";
            this.rdSemanal.Size = new System.Drawing.Size(66, 17);
            this.rdSemanal.TabIndex = 2;
            this.rdSemanal.TabStop = true;
            this.rdSemanal.Text = "Semanal";
            this.rdSemanal.UseVisualStyleBackColor = true;
            this.rdSemanal.CheckedChanged += new System.EventHandler(this.rdSemanal_CheckedChanged);
            // 
            // rdQuinzenal
            // 
            this.rdQuinzenal.AutoSize = true;
            this.rdQuinzenal.Location = new System.Drawing.Point(125, 10);
            this.rdQuinzenal.Name = "rdQuinzenal";
            this.rdQuinzenal.Size = new System.Drawing.Size(72, 17);
            this.rdQuinzenal.TabIndex = 1;
            this.rdQuinzenal.TabStop = true;
            this.rdQuinzenal.Text = "Quinzenal";
            this.rdQuinzenal.UseVisualStyleBackColor = true;
            this.rdQuinzenal.CheckedChanged += new System.EventHandler(this.rdQuinzenal_CheckedChanged);
            // 
            // rdMensal
            // 
            this.rdMensal.AutoSize = true;
            this.rdMensal.Location = new System.Drawing.Point(11, 10);
            this.rdMensal.Name = "rdMensal";
            this.rdMensal.Size = new System.Drawing.Size(59, 17);
            this.rdMensal.TabIndex = 0;
            this.rdMensal.TabStop = true;
            this.rdMensal.Text = "Mensal";
            this.rdMensal.UseVisualStyleBackColor = true;
            this.rdMensal.CheckedChanged += new System.EventHandler(this.rdMensal_CheckedChanged);
            // 
            // btnExcluir
            // 
            this.btnExcluir.Enabled = false;
            this.btnExcluir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcluir.Location = new System.Drawing.Point(218, 50);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(100, 26);
            this.btnExcluir.TabIndex = 9;
            this.btnExcluir.Text = "Deletar";
            this.btnExcluir.UseVisualStyleBackColor = true;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btAdic
            // 
            this.btAdic.Enabled = false;
            this.btAdic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAdic.Location = new System.Drawing.Point(17, 50);
            this.btAdic.Name = "btAdic";
            this.btAdic.Size = new System.Drawing.Size(100, 26);
            this.btAdic.TabIndex = 8;
            this.btAdic.Text = "Adicionar";
            this.btAdic.UseVisualStyleBackColor = true;
            this.btAdic.Click += new System.EventHandler(this.btAdic_Click);
            // 
            // txValor
            // 
            this.txValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txValor.Location = new System.Drawing.Point(216, 12);
            this.txValor.Name = "txValor";
            this.txValor.Size = new System.Drawing.Size(101, 26);
            this.txValor.TabIndex = 7;
            this.txValor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txPerc_KeyUp);
            // 
            // txPerc
            // 
            this.txPerc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txPerc.Location = new System.Drawing.Point(108, 12);
            this.txPerc.Name = "txPerc";
            this.txPerc.Size = new System.Drawing.Size(50, 26);
            this.txPerc.TabIndex = 6;
            this.txPerc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txPerc_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(164, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Valor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Percentual:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 121);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(330, 241);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // CadPercs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 362);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "CadPercs";
            this.Text = "Cadastro de percentuais";
            this.Load += new System.EventHandler(this.CadPercs_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Button btAdic;
        private System.Windows.Forms.TextBox txValor;
        private System.Windows.Forms.TextBox txPerc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdSemanal;
        private System.Windows.Forms.RadioButton rdQuinzenal;
        private System.Windows.Forms.RadioButton rdMensal;
    }
}

