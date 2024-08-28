
namespace TeleBonifacio
{
    partial class oprConfig
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
            this.lblNome = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txNome = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txEndereco = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txFone = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btAtu = new System.Windows.Forms.Button();
            this.ckBackup = new System.Windows.Forms.CheckBox();
            this.btRetVersao = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.txDocs = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ckAdaptar = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.btFormas = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(83, 19);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(109, 13);
            this.lblNome.TabIndex = 5;
            this.lblNome.Text = "Localização da Base:";
            this.lblNome.DoubleClick += new System.EventHandler(this.lblNome_DoubleClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(198, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(303, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(507, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = ". . . ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(507, 192);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Cancelar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(29, 192);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "OK";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.mdb";
            this.openFileDialog1.Filter = "Banco de dados Access (*.mdb)|*.mdb";
            // 
            // txNome
            // 
            this.txNome.Location = new System.Drawing.Point(198, 64);
            this.txNome.Name = "txNome";
            this.txNome.Size = new System.Drawing.Size(303, 20);
            this.txNome.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Nome:";
            this.label1.DoubleClick += new System.EventHandler(this.label1_DoubleClick);
            // 
            // txEndereco
            // 
            this.txEndereco.Location = new System.Drawing.Point(198, 90);
            this.txEndereco.Name = "txEndereco";
            this.txEndereco.Size = new System.Drawing.Size(303, 20);
            this.txEndereco.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Endereço:";
            // 
            // txFone
            // 
            this.txFone.Location = new System.Drawing.Point(198, 116);
            this.txFone.Name = "txFone";
            this.txFone.Size = new System.Drawing.Size(303, 20);
            this.txFone.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(158, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Fone:";
            // 
            // btAtu
            // 
            this.btAtu.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btAtu.Location = new System.Drawing.Point(145, 161);
            this.btAtu.Name = "btAtu";
            this.btAtu.Size = new System.Drawing.Size(121, 23);
            this.btAtu.TabIndex = 8;
            this.btAtu.Text = "Procurar Atualização";
            this.btAtu.UseVisualStyleBackColor = true;
            this.btAtu.Click += new System.EventHandler(this.btAtu_Click);
            // 
            // ckBackup
            // 
            this.ckBackup.AutoSize = true;
            this.ckBackup.Location = new System.Drawing.Point(29, 144);
            this.ckBackup.Name = "ckBackup";
            this.ckBackup.Size = new System.Drawing.Size(119, 17);
            this.ckBackup.TabIndex = 7;
            this.ckBackup.Text = "Backup Automático";
            this.ckBackup.UseVisualStyleBackColor = true;
            // 
            // btRetVersao
            // 
            this.btRetVersao.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btRetVersao.Location = new System.Drawing.Point(271, 161);
            this.btRetVersao.Name = "btRetVersao";
            this.btRetVersao.Size = new System.Drawing.Size(98, 23);
            this.btRetVersao.TabIndex = 9;
            this.btRetVersao.Text = "Retornar Versão";
            this.btRetVersao.UseVisualStyleBackColor = true;
            this.btRetVersao.Click += new System.EventHandler(this.btRetVersao_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(507, 36);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = ". . . ";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txDocs
            // 
            this.txDocs.Location = new System.Drawing.Point(198, 38);
            this.txDocs.Name = "txDocs";
            this.txDocs.Size = new System.Drawing.Size(303, 20);
            this.txDocs.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Localização dos Documentos";
            // 
            // ckAdaptar
            // 
            this.ckAdaptar.AutoSize = true;
            this.ckAdaptar.Location = new System.Drawing.Point(29, 167);
            this.ckAdaptar.Name = "ckAdaptar";
            this.ckAdaptar.Size = new System.Drawing.Size(92, 17);
            this.ckAdaptar.TabIndex = 20;
            this.ckAdaptar.Text = "Adaptar a tela";
            this.ckAdaptar.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button5.Location = new System.Drawing.Point(374, 161);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 21;
            this.button5.Text = "Percentuais";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // btFormas
            // 
            this.btFormas.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btFormas.Location = new System.Drawing.Point(455, 161);
            this.btFormas.Name = "btFormas";
            this.btFormas.Size = new System.Drawing.Size(75, 23);
            this.btFormas.TabIndex = 22;
            this.btFormas.Text = "Formas";
            this.btFormas.UseVisualStyleBackColor = true;
            this.btFormas.Click += new System.EventHandler(this.btFormas_Click);
            // 
            // oprConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 227);
            this.Controls.Add(this.btFormas);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.ckAdaptar);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txDocs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btRetVersao);
            this.Controls.Add(this.ckBackup);
            this.Controls.Add(this.btAtu);
            this.Controls.Add(this.txFone);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txEndereco);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txNome);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblNome);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "oprConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurações";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txNome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txEndereco;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txFone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btAtu;
        private System.Windows.Forms.CheckBox ckBackup;
        private System.Windows.Forms.Button btRetVersao;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txDocs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox ckAdaptar;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btFormas;
    }
}