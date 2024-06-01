
namespace TeleBonifacio
{
    partial class CadVendedores2
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoja = new System.Windows.Forms.TextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.chkAtende = new System.Windows.Forms.CheckBox();
            this.txtNro = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cnbNivel = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cntrole1
            // 
            this.cntrole1.Size = new System.Drawing.Size(334, 54);
            this.cntrole1.AcaoRealizada += new System.EventHandler<AcaoEventArgs>(this.cntrole1_AcaoRealizada);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 220);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Loja";
            // 
            // txtLoja
            // 
            this.txtLoja.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoja.Location = new System.Drawing.Point(12, 240);
            this.txtLoja.MaxLength = 100;
            this.txtLoja.Name = "txtLoja";
            this.txtLoja.Size = new System.Drawing.Size(310, 23);
            this.txtLoja.TabIndex = 4;
            this.txtLoja.Tag = "O";
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(12, 65);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(35, 13);
            this.lblNome.TabIndex = 11;
            this.lblNome.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.Location = new System.Drawing.Point(12, 85);
            this.txtNome.MaxLength = 100;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(310, 23);
            this.txtNome.TabIndex = 0;
            this.txtNome.Tag = "O";
            // 
            // chkAtende
            // 
            this.chkAtende.AutoSize = true;
            this.chkAtende.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAtende.Location = new System.Drawing.Point(15, 279);
            this.chkAtende.Name = "chkAtende";
            this.chkAtende.Size = new System.Drawing.Size(101, 19);
            this.chkAtende.TabIndex = 5;
            this.chkAtende.Text = "Atende Whats";
            this.chkAtende.UseVisualStyleBackColor = true;
            this.chkAtende.CheckStateChanged += new System.EventHandler(this.chkAtende_CheckStateChanged);
            // 
            // txtNro
            // 
            this.txtNro.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNro.Location = new System.Drawing.Point(267, 277);
            this.txtNro.MaxLength = 100;
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(55, 23);
            this.txtNro.TabIndex = 6;
            this.txtNro.Tag = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 282);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Nro";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Usuário";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(12, 140);
            this.txtUsuario.MaxLength = 100;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(150, 23);
            this.txtUsuario.TabIndex = 1;
            this.txtUsuario.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Senha";
            // 
            // txtSenha
            // 
            this.txtSenha.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenha.Location = new System.Drawing.Point(172, 140);
            this.txtSenha.MaxLength = 100;
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(150, 23);
            this.txtSenha.TabIndex = 2;
            this.txtSenha.Tag = "";
            this.txtSenha.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Nivel de Acesso";
            // 
            // cnbNivel
            // 
            this.cnbNivel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cnbNivel.FormattingEnabled = true;
            this.cnbNivel.Items.AddRange(new object[] {
            "Balconísta",
            "Caixa",
            "Escritório"});
            this.cnbNivel.Location = new System.Drawing.Point(12, 192);
            this.cnbNivel.Name = "cnbNivel";
            this.cnbNivel.Size = new System.Drawing.Size(121, 21);
            this.cnbNivel.TabIndex = 3;
            this.cnbNivel.Click += new System.EventHandler(this.cnbNivel_Click);
            // 
            // CadVendedores2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(334, 311);
            this.Controls.Add(this.cnbNivel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNro);
            this.Controls.Add(this.chkAtende);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLoja);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Name = "CadVendedores2";
            this.Text = "Vendedores";
            this.Activated += new System.EventHandler(this.CadVendedores2_Activated);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CadVendedor_KeyUp);
            this.Controls.SetChildIndex(this.cntrole1, 0);
            this.Controls.SetChildIndex(this.txtNome, 0);
            this.Controls.SetChildIndex(this.lblNome, 0);
            this.Controls.SetChildIndex(this.txtLoja, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.chkAtende, 0);
            this.Controls.SetChildIndex(this.txtNro, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtUsuario, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtSenha, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cnbNivel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoja;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.CheckBox chkAtende;
        private System.Windows.Forms.TextBox txtNro;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cnbNivel;
    }
}
