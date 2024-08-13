using Microsoft.Win32;
using System;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TeleBonifacio;

namespace TeleBonifacio
{
    public partial class operAnota : Form
    {
        private LancamentoStatus lancamentoStatus;
        //private AtcCtrl.ATCRTF atcrtf1;
        private DateTime currentTime;

        public operAnota()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            //this.atcrtf1 = new AtcCtrl.ATCRTF();
            this.SuspendLayout();
            // 
            // atcrtf1
            // 
            //this.atcrtf1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.atcrtf1.Location = new System.Drawing.Point(0, 0);
            //this.atcrtf1.Name = "atcrtf1";
            //this.atcrtf1.Size = new System.Drawing.Size(876, 380);
            //this.atcrtf1.TabIndex = 0;
            //this.atcrtf1.Load += new System.EventHandler(this.atcrtf1_Load);
            // 
            // operAnota
            // 
            this.ClientSize = new System.Drawing.Size(876, 380);
            //this.Controls.Add(this.atcrtf1);
            this.Name = "operAnota";
            this.Text = "Anotações";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.operAnota_FormClosing);
            this.Load += new System.EventHandler(this.operAnota_Load);
            this.ResumeLayout(false);

        }

        private void atcrtf1_Load(object sender, EventArgs e)
        {
            string nmArquivo = "Anotacoes" + glo.iUsuario.ToString() + ".rtf";
            //atcrtf1.caminhoDoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nmArquivo);
            //atcrtf1.Carrega();
        }

        private void operAnota_Load(object sender, EventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"DenisEntregas\Anotacoes"))
                {
                    if (key != null)
                    {
                        int x = (int)key.GetValue("WindowPositionX", this.Left);
                        int y = (int)key.GetValue("WindowPositionY", this.Top);
                        int width = (int)key.GetValue("WindowWidth", this.Width);
                        int height = (int)key.GetValue("WindowHeight", this.Height);

                        this.StartPosition = FormStartPosition.Manual;
                        this.Left = x;
                        this.Top = y;
                        this.Width = width;
                        this.Height = height;
                    }
                }
            }
            catch (Exception ex)
            {
                this.StartPosition = FormStartPosition.WindowsDefaultLocation;
                this.Size = new Size(800, 600);
             }
        }

        private void operAnota_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"DenisEntregas\Anotacoes"))
                {
                    if (key != null)
                    {
                        key.SetValue("WindowPositionX", this.Left);
                        key.SetValue("WindowPositionY", this.Top);
                        key.SetValue("WindowWidth", this.Width);
                        key.SetValue("WindowHeight", this.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                // 
            }

        }
    }
}

#region Classes

public class LancamentoInfo
{
    public DateTime? TxInMan { get; set; }
    public DateTime? TxFmMan { get; set; }
    public DateTime? TxInTrd { get; set; }
    public DateTime? TxFnTrd { get; set; }
    public DateTime? TxInCafeMan { get; set; }
    public DateTime? TxFmCafeMan { get; set; }
    public DateTime? TxInCafeTrd { get; set; }
    public DateTime? TxFmCafeTrd { get; set; }
}

public enum LancamentoStatus
{
    Vazio,
    IniciarExpediente,
    EntradaCafeManha,
    SaidaCafeManha,
    SaidaManha,
    EntradaTarde,
    EntradaCafeTarde,
    SaidaCafeTarde,
    SaidaTarde,
    Completo,        
}

#endregion