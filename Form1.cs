using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class Form1 : Form
    {

        private bool ativou = false;

        public Form1()
        {
            InitializeComponent();
        }

        #region Botões

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<operLancamento>();
        }

        private void AbrirOuFocarFormulario<T>() where T : Form, new()
        {
            T formExistente = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (formExistente != null)
            {
                if (formExistente.WindowState == FormWindowState.Minimized)
                {
                    formExistente.WindowState = FormWindowState.Normal;
                }
                formExistente.BringToFront();
                formExistente.Focus();
            }
            else
            {
                T novoForm = new T();
                novoForm.Show();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<fCadEntregadores>();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                pb.BorderStyle = BorderStyle.Fixed3D; // Efeito de pressionado
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                pb.BorderStyle = BorderStyle.FixedSingle; // Volta ao estilo original
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<fCadClientes>();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<oprConfig>();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<CadVendedores>();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<Consultas>();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<Dashboard>();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<opRecibos>();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<operAvanc>();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<operCaixa>();
        }

        #endregion

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!this.ativou)
            {
                this.ativou = true;
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                string titulo;
                if (version.Revision > 0)
                {
                    titulo = $"Bonifácio TeleEntregas {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
                }
                else
                {
                    titulo = $"Bonifácio TeleEntregas {version.Major}.{version.Minor}.{version.Build}";
                }
                VerificaNovaVersao(version);
                this.Text = titulo;
            }
        }

        #region Atualizacao 
        private void VerificaNovaVersao(Version version)
        {
            string pastaAtual = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Loga("Executado a partir de "+ pastaAtual);
            INI cINI = new INI();
            int diaAtual = DateTime.Now.Day;
            int UltExec = cINI.ReadInt("INI", "UltExec", 0);

            bool atualizar = (diaAtual != UltExec);
            cINI.WriteInt("INI", "UltExec", diaAtual);
            // bool atualizar = true;

            Loga("atualizar = "+atualizar.ToString());
            if (atualizar)
            {
                string URL = cINI.ReadString("FTP", "URL", "");
                if (URL.Length > 0)
                {
                    string user = gen.Cripto.Decrypt(cINI.ReadString("FTP", "user", ""));
                    string senha = gen.Cripto.Decrypt(cINI.ReadString("FTP", "pass", ""));
                    FTP cFPT = new FTP(URL, user, senha);
                    string BakTitulo = this.Text;
                    this.Text = "PROCURANDO NOVA VERSÃO";
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    int versaoFtp = cFPT.LerVersaoDoFtp();
                    stopwatch.Stop();
                    string Tempo = stopwatch.ElapsedMilliseconds.ToString();
                    cINI.WriteString("FTP", "tempo", Tempo);
                    int versionInt = (version.Major * 100) + (version.Minor * 10) + version.Build;
                    Loga("Versão Atual " + versionInt.ToString());
                    Loga("Versão no FTP " + versaoFtp.ToString());
                    // if (1==1)
                    if (versaoFtp > versionInt)
                    {
                        string versaoNovaStr = $"{versaoFtp / 100}.{(versaoFtp / 10) % 10}.{versaoFtp % 10}";
                        string NovaVersaoINI = cINI.ReadString("Config", "NovaVersao", "");
                        Loga("Versão no INI " + NovaVersaoINI);
                        if (versaoNovaStr != NovaVersaoINI)
                        {
                            string versaoAtualStr = version.ToString().Substring(0, version.ToString().Length - 2);
                            string mensagem = $"Existe uma nova versão do programa disponível.\n\n" +
                                                    $"Versão atual: {versaoAtualStr}\n" +
                                                    $"Nova versão: {versaoNovaStr}\n\n" +
                                                    "Deseja baixá-la agora?";
                            DialogResult dialogResult = MessageBox.Show(
                                mensagem,
                                "Atualização Disponível",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1);
                            if (dialogResult == DialogResult.Yes)
                            {
                                Loga("Optado por atualizar");                                
                                string EstouEm = Path.Combine(pastaAtual, "TeleBonifacio.exe");
                                cINI.WriteString("Atualizador", "EstouEm", EstouEm);
                                string pastaAtualizador = Path.Combine(pastaAtual, "Atualizador");
                                string progAtualizador = Path.Combine(pastaAtualizador, "ATCAtualizeitor.exe");
                                Loga("pasta Atual: "+ pastaAtual);
                                Loga("pasta Atualizador: " + pastaAtualizador);
                                Loga("Programa Atualizador : " + progAtualizador);
                                Process.Start(progAtualizador);
                                Environment.Exit(0);
                            }
                            else
                            {
                                Loga("Optado por NÃO atualizar");
                                this.Text = BakTitulo;
                                cINI.WriteString("Config", "NovaVersao", versaoNovaStr);
                                cINI.WriteString("Config", "VersaoAtual", versaoAtualStr);
                                MessageBox.Show(
                                    "Poderá atualizar quando quiser\n" +
                                    "indo nas configurações",
                                    "Atualização Disponível",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }
                        }
                    }
                }                
            }
        }

        private bool PathIsLocal(string path)
        {
            if (path.Length < 3)
                return false;
            return path[1] == ':' && path[2] == '\\';
        }

        private void Loga(string message)
        {
            string logFilePath = @"C:\Entregas\Entregas.txt";
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }

        #endregion

    }

}
