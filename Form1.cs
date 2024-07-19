using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class Form1 : Form
    {

        private bool ativou = false;
        private INI cINI;
        private bool DentroDoTimer = false;

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
            AbrirOuFocarFormulario<CadVendedores2>();
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

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<OperRH>();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<OperFalta>();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<CadFornec>();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<OperPagar>();
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
                cINI = new INI();
                if (!VerificaNovaVersao(version))
                {
                    if (!Environment.CurrentDirectory.Contains("\\\\"))
                    {
                        if (cINI.ReadString("Backup", "Ativo", "") == "1")
                        {
                            string HoraBckup = cINI.ReadString("Backup", "Hora", "");
                            timer1.Tag = HoraBckup;
                            timer1.Enabled = true;
                        }
                    }
                }
                this.Text = titulo;
                VeSeTemQueApagar();
            }
        }

        private void VeSeTemQueApagar()
        {
            int nro = cINI.ReadInt("Apagar", "Nro", 0);
            for (int i = 0; i < nro; i++)
            {
                string nmArq = cINI.ReadString("Apagar", "Arq" + nro, "");
                try
                {
                    File.Delete(nmArq);
                }
                catch (Exception)
                {
                    // NÃO FAZ NADA
                }                
            }
            cINI.WriteInt("Apagar", "Nro", 0);
        }

        #region Atualizacao 
        private bool VerificaNovaVersao(Version version)
        {
            bool ret = false;
            string pastaAtual = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            NovoLog();
            glo.Loga("Executado a partir de "+ pastaAtual);            
            int diaAtual = DateTime.Now.Day;
            int UltExec = cINI.ReadInt("INI", "UltExec", 0);
            cINI.WriteInt("INI", "UltExec", diaAtual);

            bool atualizar = (diaAtual != UltExec);
            // atualizar = true;
            
            glo.Loga("atualizar = "+atualizar.ToString());
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
                    int versaoFtp = cFPT.LerVersaoDoFtp();
                    int versionInt = (version.Major * 100) + (version.Minor * 10) + version.Build;
                    glo.Loga("Versão Atual " + versionInt.ToString());
                    glo.Loga("Versão no FTP " + versaoFtp.ToString());

                    // if (1==1)
                    if (versaoFtp > versionInt)
                    {
                        string versaoNovaStr = $"{versaoFtp / 100}.{(versaoFtp / 10) % 10}.{versaoFtp % 10}";
                        string NovaVersaoINI = cINI.ReadString("Config", "NovaVersao", "");
                        glo.Loga("Versão no INI " + NovaVersaoINI);
                        if (versaoNovaStr != NovaVersaoINI)
                        {
                            ret = true;
                            string versaoAtualStr = version.ToString().Substring(0, version.ToString().Length - 2);
                            string Mensagem = cFPT.retMensagem();
                            if (Mensagem.Length>0)
                            {
                                Mensagem = $"" +Mensagem + "\n\n";
                            }
                            string mensagem = $"Existe uma nova versão do programa disponível.\n\n" +
                                                    $"Versão atual: {versaoAtualStr}\n" +
                                                    $"Nova versão: {versaoNovaStr}\n\n" +
                                                    Mensagem +
                                                    "Deseja baixá-la agora?";
                            DialogResult dialogResult = MessageBox.Show(
                                mensagem,
                                "Atualização Disponível",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1);
                            if (dialogResult == DialogResult.Yes)
                            {
                                glo.Loga("Optado por atualizar");                                
                                string EstouEm = Path.Combine(pastaAtual, "TeleBonifacio.exe");
                                cINI.WriteString("Atualizador", "EstouEm", EstouEm);
                                string pastaAtualizador = Path.Combine(pastaAtual, "Atualizacao");
                                string progAtualizador = Path.Combine(pastaAtualizador, "ATCAtualizeitor.exe");
                                glo.Loga("pasta Atual: "+ pastaAtual);
                                glo.Loga("pasta Atualizador: " + pastaAtualizador);
                                glo.Loga("Programa Atualizador : " + progAtualizador);
                                Process.Start(progAtualizador);
                                Environment.Exit(0);
                            }
                            else
                            {
                                glo.Loga("Optado por NÃO atualizar");
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
            return ret;
        }

        private static void NovoLog()
        {
            string baseDirectory = @"C:\Entregas";
            string logFileName = "Entregas.txt";
            string logFilePath = Path.Combine(baseDirectory, logFileName);
            string backupPath = Path.Combine(baseDirectory, "Logs", $"{DateTime.Now:yyyyMMdd}_Entregas.txt"); // Rename based on current date
            Directory.CreateDirectory(Path.GetDirectoryName(backupPath));
            if (!File.Exists(logFilePath))
            {
                if (File.Exists(backupPath))
                {
                    File.Copy(backupPath, logFilePath, true);
                    Console.WriteLine($"Log do dia anterior copiado para {logFilePath}");
                }
            }
            else
            {
                FileInfo fileInfo = new FileInfo(logFilePath);
                DateTime logCreationDate = fileInfo.CreationTime.Date;
                if (logCreationDate < DateTime.Today)
                {
                    string backupName = $"{DateTime.Now:yyyyMMdd}_{logFileName}";
                    string backupFullPath = Path.Combine(baseDirectory, "Logs", backupName);
                    File.Copy(logFilePath, backupFullPath, true);
                    using (StreamWriter sw = File.CreateText(logFilePath))
                    {
                        sw.WriteLine($"Log de {logCreationDate.ToShortDateString()} copiado para Logs/{backupName}");
                    }
                }
            }
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!this.DentroDoTimer)
            {
                this.DentroDoTimer = true;
                lbMens.Text = "Fazendo Backup OnLine";
                string Caption = this.Text;
                this.Text = "Fazendo Backup OnLine";
                this.Height = 171;
                string HoraBckup = timer1.Tag.ToString() + ":00";
                DateTime horaBackup = DateTime.ParseExact(HoraBckup, "HH:mm", CultureInfo.InvariantCulture);
                DateTime horaAtual = DateTime.Now;

                bool Fazer = horaAtual.Hour > horaBackup.Hour || (horaAtual.Hour == horaBackup.Hour && horaAtual.Minute <= horaBackup.Minute + 1);
                // bool Fazer = true;

                if (Fazer)
                {
                    string caminhoArquivoZip = "";
                    string pastaOper = cINI.ReadString("Backup", "pastaOper", "");
                    if (Directory.Exists(pastaOper))
                    {
                        DirectoryInfo di = new DirectoryInfo(pastaOper);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(pastaOper);
                    }
                    List<string> arquivosParaZipar = new List<string>();
                    int contador = 1;
                    this.Text = "Copiando arquivos";
                    while (true)
                    {
                        string nomeArquivo = cINI.ReadString("Backup", "Arq" + contador.ToString(), "");
                        if (string.IsNullOrEmpty(nomeArquivo))
                        {
                            break;
                        }
                        string caminhoArquivoOrigem = Path.Combine(Application.StartupPath, nomeArquivo);
                        string caminhoArquivoDestino = Path.Combine(pastaOper, Path.GetFileName(nomeArquivo));
                        File.Copy(caminhoArquivoOrigem, caminhoArquivoDestino, true);
                        contador++;
                        arquivosParaZipar.Add(caminhoArquivoDestino);
                    }
                    string PriParteNome = "Backup" + ((int)DateTime.Now.DayOfWeek).ToString();
                    string NomeArq = $"{PriParteNome}.zip";
                    caminhoArquivoZip = Path.Combine(pastaOper, NomeArq);
                    contador = 0;
                    this.Text = "Compactando";
                    while (true)
                    {
                        try
                        {
                            using (ZipArchive zip = ZipFile.Open(caminhoArquivoZip, ZipArchiveMode.Create))
                            {
                                foreach (string caminhoArquivoOrigem in arquivosParaZipar)
                                {
                                    zip.CreateEntryFromFile(caminhoArquivoOrigem, Path.GetFileName(caminhoArquivoOrigem));
                                }
                            }
                            break;
                        }
                        catch (IOException ex)
                        {
                            contador++;
                            NomeArq = $"{PriParteNome}{contador}";
                            caminhoArquivoZip = Path.Combine(pastaOper, $"{NomeArq}.zip");
                        }
                    }
                    string URL = cINI.ReadString("FTP", "URL", "");
                    string user = gen.Cripto.Decrypt(cINI.ReadString("FTP", "user", ""));
                    string senha = gen.Cripto.Decrypt(cINI.ReadString("FTP", "pass", ""));
                    this.Text = "Enviando ao servidor";
                    FTP cFPT = new FTP(URL, user, senha);
                    string PastaBaseFTP = cINI.ReadString("Backup", "PastaBaseFTP", "");
                    int pos = caminhoArquivoZip.IndexOf(pastaOper.Replace("/", "\\"));
                    string CamfTP = caminhoArquivoZip.Substring(pos);
                    if (CamfTP.EndsWith(NomeArq))
                    {
                        CamfTP = CamfTP.Remove(CamfTP.Length - NomeArq.Length);
                    }
                    cFPT.setBarra(ref progressBar1);
                    cFPT.Upload(caminhoArquivoZip, PastaBaseFTP);
                    this.Text = Caption;
                    this.Height = 171;
                    this.DentroDoTimer = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (glo.ODBC)
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                lbMens.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
                label13.Visible = false;
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                pictureBox5.Visible = false;
                pictureBox6.Visible = false;
                pictureBox7.Visible = false;
                pictureBox8.Visible = false;
                pictureBox9.Visible = false;
                pictureBox10.Visible = false;
                pictureBox11.Visible = false;
                pictureBox13.Visible = false;                                                                
                this.Width = 300;
                this.Height = 200;
                int posL2 = (this.Width - pictureBox10.Width - pictureBox11.Width) / 2;
                posL2 = +92;
                label12.Visible = true;
                pictureBox12.Visible = true;
                label12.Top = 20;
                pictureBox12.Left = label12.Left = posL2;               
            }
            else
            {
                switch (glo.Nivel)
                {
                    case 0: // Balconista, ve só as faltas    
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        label4.Visible = false;
                        label5.Visible = false;
                        label6.Visible = false;
                        label7.Visible = false;
                        label8.Visible = false;
                        lbMens.Visible = false;
                        label9.Visible = false;
                        label10.Visible = false;
                        label11.Visible = false;
                        label13.Visible = false;
                        pictureBox1.Visible = false;
                        pictureBox2.Visible = false;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;
                        pictureBox5.Visible = false;
                        pictureBox6.Visible = false;
                        pictureBox7.Visible = false;
                        pictureBox8.Visible = false;
                        pictureBox9.Visible = false;
                        pictureBox10.Visible = false;
                        pictureBox11.Visible = false;
                        pictureBox13.Visible = false;
                        this.Width = 350;
                        this.Height = 200;
                        int posL = (this.Width - pictureBox12.Width) / 2;
                        pictureBox12.Left = label12.Left = posL;
                        break;
                    case 1: // Caixa
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        label4.Visible = false;
                        label5.Visible = false;
                        label6.Visible = false;
                        label7.Visible = false;
                        label8.Visible = false;
                        lbMens.Visible = false;
                        label9.Visible = false;
                        label12.Visible = false;
                        label13.Visible = false;
                        label14.Visible = false;
                        pictureBox1.Visible = false;
                        pictureBox2.Visible = false;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;
                        pictureBox5.Visible = false;
                        pictureBox6.Visible = false;
                        pictureBox7.Visible = false;
                        pictureBox8.Visible = false;
                        pictureBox9.Visible = false;
                        pictureBox12.Visible = false;
                        pictureBox13.Visible = false;
                        pictureBox14.Visible = false;
                        label10.Visible = true;
                        pictureBox10.Visible = true;
                        label11.Visible = true;
                        pictureBox11.Visible = true;
                        this.Width = 300;
                        this.Height = 200;
                        int posL2 = (this.Width - pictureBox10.Width - pictureBox11.Width) / 2;
                        pictureBox10.Left = label10.Left = posL2;
                        pictureBox11.Left = label11.Left = posL2 + pictureBox10.Width;
                        label10.Top = 30;
                        pictureBox10.Top = 50;
                        label11.Top = 30;
                        pictureBox11.Top = 50;
                        break;
                    case 2:
                        // Escritório Vê tudo
                        break;
                }
            }            
            glo.AdjustFormComponents(this);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Form form = sender as Form;
            if (form != null)
            {
                string sResize = $@"Form1_Resize: Form size is now Width={form.Width}, Height={form.Height}";
                glo.Loga(sResize);
            }
            else
            {
                glo.Loga("Form1_Resize: Sender is not a Form.");
            }
        }
    }

}
