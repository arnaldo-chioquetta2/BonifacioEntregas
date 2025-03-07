using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace TeleBonifacio
{
    public class BackupManager
    {
        private string _pastaOper;
        private INI _ini;
        private FTP _ftp;
        private bool usaProgress = false;

        public BackupManager()
        {

        }

        private void ConfigurarFTP()
        {
            string URL = _ini.ReadString("FTP", "URL", "");
            string user = gen.Cripto.Decrypt(_ini.ReadString("FTP", "user", ""));
            string senha = gen.Cripto.Decrypt(_ini.ReadString("FTP", "pass", ""));
            _ftp = new FTP(URL, user, senha);
        }

        //public void RealizarBackup(bool usaProgress)
        //{
        //    try
        //    {
        //        this.usaProgress = usaProgress;
        //        _ini = new INI();
        //        _pastaOper = _ini.ReadString("Backup", "pastaOper", "");
        //        ConfigurarFTP();
        //        if (!Directory.Exists(_pastaOper))
        //        {
        //            Directory.CreateDirectory(_pastaOper);
        //        }

        //        LimparPastaDeBackup();
        //        List<string> arquivosParaZipar = CopiarArquivosParaPastaOper();
        //        string caminhoArquivoZip = CompactarArquivos(arquivosParaZipar);

        //        EnviarBackupParaServidor(caminhoArquivoZip);
        //    }
        //    catch (IOException ioEx)
        //    {
        //        // Log e manipulação específica para problemas de IO, como falha ao acessar arquivos de rede
        //        glo.Loga("Erro de I/O durante o backup: " + ioEx.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log e manipulação genérica
        //        glo.Loga("Erro durante o backup: " + ex.Message);
        //    }
        //}
        public void RealizarBackup(bool usaProgress)
        {
            this.usaProgress = usaProgress;
            _ini = new INI();
            _pastaOper = _ini.ReadString("Backup", "pastaOper", "");
            ConfigurarFTP();
            if (!Directory.Exists(_pastaOper))
            {
                Directory.CreateDirectory(_pastaOper);
            }

            LimparPastaDeBackup();
            List<string> arquivosParaZipar = CopiarArquivosParaPastaOper();
            string caminhoArquivoZip = CompactarArquivos(arquivosParaZipar);

            EnviarBackupParaServidor(caminhoArquivoZip);
        }


        private void LimparPastaDeBackup()
        {
            DirectoryInfo di = new DirectoryInfo(_pastaOper);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        private List<string> CopiarArquivosParaPastaOper()
        {
            List<string> arquivosParaZipar = new List<string>();
            DateTime hoje = DateTime.Today;

            // 🔹 Passo 1: Adicionar arquivos definidos no INI
            int contador = 1;
            while (true)
            {
                string nomeArquivo = _ini.ReadString("Backup", "Arq" + contador.ToString(), "");
                if (string.IsNullOrEmpty(nomeArquivo)) break;

                string caminhoArquivoOrigem = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);
                string caminhoArquivoDestino = Path.Combine(_pastaOper, Path.GetFileName(nomeArquivo));

                if (File.Exists(caminhoArquivoOrigem))
                {
                    File.Copy(caminhoArquivoOrigem, caminhoArquivoDestino, true);
                    arquivosParaZipar.Add(caminhoArquivoDestino);
                    Console.WriteLine($"✅ DEBUG: Adicionado {nomeArquivo} via INI.");
                }
                else
                {
                    Console.WriteLine($"⚠ AVISO: Arquivo {nomeArquivo} do INI não encontrado.");
                }
                contador++;
            }

            // 🔹 Passo 2: Adicionar arquivos .txt modificados hoje
            string diretorioEntregas = @"C:\Entregas";
            if (Directory.Exists(diretorioEntregas))
            {
                string[] arquivosTxt = Directory.GetFiles(diretorioEntregas, "*.txt");

                foreach (string arquivo in arquivosTxt)
                {
                    FileInfo fileInfo = new FileInfo(arquivo);
                    if (fileInfo.LastWriteTime.Date == hoje)
                    {
                        string destinoArquivo = Path.Combine(_pastaOper, fileInfo.Name);
                        File.Copy(arquivo, destinoArquivo, true);
                        arquivosParaZipar.Add(destinoArquivo);
                        Console.WriteLine($"✅ DEBUG: Adicionado {arquivo} à lista de zip.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"❌ ERRO: Diretório {diretorioEntregas} não encontrado.");
            }

            Console.WriteLine($"📦 DEBUG: Total de arquivos para ZIP: {arquivosParaZipar.Count}");
            return arquivosParaZipar;
        }

        //private List<string> CopiarArquivosParaPastaOper()
        //{
        //    List<string> arquivosParaZipar = new List<string>();
        //    int contador = 1;
        //    while (true)
        //    {
        //        string nomeArquivo = _ini.ReadString("Backup", "Arq" + contador.ToString(), "");
        //        if (string.IsNullOrEmpty(nomeArquivo)) break;

        //        string caminhoArquivoOrigem = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);
        //        string caminhoArquivoDestino = Path.Combine(_pastaOper, Path.GetFileName(nomeArquivo));
        //        File.Copy(caminhoArquivoOrigem, caminhoArquivoDestino, true);
        //        arquivosParaZipar.Add(caminhoArquivoDestino);
        //        contador++;
        //    }
        //    string arquivoExtra = @"C:\Entregas\Entregas.txt";
        //    string destinoExtra = Path.Combine(_pastaOper, Path.GetFileName(arquivoExtra));
        //    File.Copy(arquivoExtra, destinoExtra, true);
        //    arquivosParaZipar.Add(destinoExtra);
        //    Console.WriteLine($"✅ DEBUG: Adicionado manualmente {arquivoExtra} à lista de zip.");
        //    return arquivosParaZipar;
        //}

        private string CompactarArquivos(List<string> arquivosParaZipar)
        {
            string PriParteNome = "Backup" + ((int)DateTime.Now.DayOfWeek).ToString();
            string caminhoArquivoZip = Path.Combine(_pastaOper, $"{PriParteNome}.zip");
            using (ZipArchive zip = ZipFile.Open(caminhoArquivoZip, ZipArchiveMode.Create))
            {
                foreach (string arquivo in arquivosParaZipar)
                {
                    zip.CreateEntryFromFile(arquivo, Path.GetFileName(arquivo));
                }
            }
            return caminhoArquivoZip;
        }

        private void EnviarBackupParaServidor(string caminhoArquivoZip)
        {
            string PastaBaseFTP = _ini.ReadString("Backup", "PastaBaseFTP", "");
            _ftp.Upload(caminhoArquivoZip, PastaBaseFTP, this.usaProgress);
        }
    }
}
