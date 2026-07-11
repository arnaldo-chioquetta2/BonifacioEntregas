using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public class FormNomeEtiqueta : Form
    {
        private readonly Label lbMensagem;
        private readonly TextBox txtNome;
        private readonly Button btOk;
        private readonly Button btCancelar;

        public string NomeEtiqueta
        {
            get { return txtNome.Text.Trim(); }
            set { txtNome.Text = value ?? string.Empty; }
        }

        public FormNomeEtiqueta()
        {
            lbMensagem = new Label();
            txtNome = new TextBox();
            btOk = new Button();
            btCancelar = new Button();

            SuspendLayout();

            lbMensagem.AutoSize = true;
            lbMensagem.Location = new Point(12, 15);
            lbMensagem.Name = "lbMensagem";
            lbMensagem.Size = new Size(170, 13);
            lbMensagem.Text = "Informe um nome para esta etiqueta:";

            txtNome.Location = new Point(15, 38);
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(357, 20);
            txtNome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btOk.Location = new Point(216, 72);
            btOk.Name = "btOk";
            btOk.Size = new Size(75, 23);
            btOk.Text = "OK";
            btOk.UseVisualStyleBackColor = true;
            btOk.Click += btOk_Click;

            btCancelar.DialogResult = DialogResult.Cancel;
            btCancelar.Location = new Point(297, 72);
            btCancelar.Name = "btCancelar";
            btCancelar.Size = new Size(75, 23);
            btCancelar.Text = "Cancelar";
            btCancelar.UseVisualStyleBackColor = true;

            AcceptButton = btOk;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btCancelar;
            ClientSize = new Size(389, 108);
            Controls.Add(btCancelar);
            Controls.Add(btOk);
            Controls.Add(txtNome);
            Controls.Add(lbMensagem);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormNomeEtiqueta";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nome da etiqueta";

            ResumeLayout(false);
            PerformLayout();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txtNome.Focus();
            txtNome.SelectAll();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomeEtiqueta))
            {
                MessageBox.Show("Informe um nome para a etiqueta.");
                txtNome.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
