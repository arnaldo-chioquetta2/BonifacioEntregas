
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class FormBase : Form
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
            this.cntrole1 = new ATCRecordNavigator.Cntrole();
            this.SuspendLayout();
            // 
            // cntrole1
            // 
            this.cntrole1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cntrole1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cntrole1.EmAdicao = false;
            this.cntrole1.EmEdicao = false;
            this.cntrole1.Location = new System.Drawing.Point(0, 0);
            this.cntrole1.Name = "cntrole1";
            this.cntrole1.Primeiro = false;
            this.cntrole1.Size = new System.Drawing.Size(800, 54);
            this.cntrole1.TabIndex = 0;
            this.cntrole1.Ultimo = false;
            this.cntrole1.Vazio = false;
            // 
            // FormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cntrole1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormBase";
            this.Text = "FormBase";
            this.ResumeLayout(false);

        }

        #endregion

        protected ATCRecordNavigator.Cntrole cntrole1;
    }
}