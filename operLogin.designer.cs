
namespace TeleBonifacio
{
    partial class operLogin
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
            this.txYser = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txSenha = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblNome.Location = new System.Drawing.Point(3, 22);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(67, 21);
            this.lblNome.TabIndex = 5;
            this.lblNome.Text = "Usuário:";
            // 
            // txYser
            // 
            this.txYser.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txYser.Location = new System.Drawing.Point(76, 16);
            this.txYser.Name = "txYser";
            this.txYser.Size = new System.Drawing.Size(155, 29);
            this.txYser.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.mdb";
            this.openFileDialog1.Filter = "Banco de dados Access (*.mdb)|*.mdb";
            // 
            // txSenha
            // 
            this.txSenha.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txSenha.Location = new System.Drawing.Point(76, 51);
            this.txSenha.Name = "txSenha";
            this.txSenha.Size = new System.Drawing.Size(155, 29);
            this.txSenha.TabIndex = 11;
            this.txSenha.UseSystemPasswordChar = true;
            this.txSenha.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txSenha_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(12, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 21);
            this.label1.TabIndex = 10;
            this.label1.Text = "Senha:";
            this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.label1_MouseClick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.button1.Location = new System.Drawing.Point(76, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 31);
            this.button1.TabIndex = 12;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // operLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 125);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txSenha);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txYser);
            this.Controls.Add(this.lblNome);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "operLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txYser;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txSenha;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}