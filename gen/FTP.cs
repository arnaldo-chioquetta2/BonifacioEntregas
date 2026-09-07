using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

// 3.9.7 Não fecha mais o programa caso não tenha como ver se teve atualização

namespace TeleBonifacio
{
    public class FTP
    {
        private int _tamanhoConteudo = 0;
        private int Tot = 0;
        string ftpIPServidor = "";
        string ftpUsuarioID = "";
        string ftpSenha = "";
        private string Erro = "";
        private ProgressBar ProgressBar1= null;
        private string Mensagem = "";
        private bool TemProgress;

        public int tamanhoConteudo
        {
            get
            {
                return _tamanhoConteudo;
            }
            set
            {
                _tamanhoConteudo = value;
                Tot += value;
                if (this.TemProgress && this.ProgressBar1 != null)
                {
                    int valorProgresso = Math.Max(
                        this.ProgressBar1.Minimum,
                        Math.Min(this.ProgressBar1.Maximum, Tot));
                    this.ProgressBar1.Value = valorProgresso;
                }
                
            }
        }

        public FTP(string ftpIPServidor, string ftpUsuarioID, string ftpSenha)
        {
            this.ftpIPServidor = ftpIPServidor;
            this.ftpUsuarioID = ftpUsuarioID;
            this.ftpSenha = ftpSenha;
        }

        public FTP()
        {
        }

        private void RegistrarDiagnosticoUpload(string etapa, string status, string mensagem)
        {
            glo.Loga(
                "FTP Upload | Etapa=" + etapa +
                " | Status=" + status +
                " | " + (mensagem ?? ""));
        }

        private void RegistrarExcecaoUpload(string etapa, Exception ex)
        {
            string mensagem = ex == null ? "(excecao nula)" : ex.Message;
            string tipo = ex == null || ex.GetType() == null ? "(nulo)" : ex.GetType().FullName;
            string detalhes = "Tipo=" + tipo + "; Mensagem=" + mensagem;

            WebException webException = ex as WebException;
            if (webException != null)
            {
                detalhes += "; WebStatus=" + webException.Status;
                FtpWebResponse ftpResponse = webException.Response as FtpWebResponse;
                if (ftpResponse != null)
                {
                    try
                    {
                        detalhes += "; FtpStatusCode=" + ftpResponse.StatusCode +
                            "; FtpStatusDescription=" + ftpResponse.StatusDescription;
                    }
                    finally
                    {
                        ftpResponse.Close();
                    }
                }
            }

            if (ex != null && ex.InnerException != null)
            {
                detalhes += "; InnerException=" + ex.InnerException.GetType().FullName +
                    ": " + ex.InnerException.Message;
            }

            RegistrarDiagnosticoUpload(etapa, "ERRO", detalhes);
        }

        public static bool Append(
            string caminhoLocal,
            string pastaRemota,
            string nomeArquivoRemoto,
            out string mensagemErro)
        {
            mensagemErro = "";

            try
            {
                if (string.IsNullOrWhiteSpace(caminhoLocal) || !File.Exists(caminhoLocal))
                {
                    mensagemErro = "Arquivo local não encontrado: " + caminhoLocal;
                    return false;
                }

                FileInfo arquivo = new FileInfo(caminhoLocal);
                if (arquivo.Length == 0)
                {
                    return true;
                }

                INI ini = new INI();
                string host = ini.ReadString("FTP", "URL", "");
                string usuario = gen.Cripto.Decrypt(ini.ReadString("FTP", "user", ""));
                string senha = gen.Cripto.Decrypt(ini.ReadString("FTP", "pass", ""));
                Uri uri = MontarUrlFtp(host, pastaRemota, nomeArquivoRemoto);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Credentials = new NetworkCredential(usuario, senha);
                request.Method = WebRequestMethods.Ftp.AppendFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = false;
                request.Timeout = 15000;
                request.ReadWriteTimeout = 15000;
                request.ContentLength = arquivo.Length;

                using (FileStream streamArquivo = new FileStream(caminhoLocal, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Stream streamRequisicao = request.GetRequestStream())
                {
                    streamArquivo.CopyTo(streamRequisicao);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return true;
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse ftpResponse = ex.Response as FtpWebResponse;
                if (ftpResponse != null)
                {
                    mensagemErro = "FTP " + ftpResponse.StatusCode + ": " + ftpResponse.StatusDescription;
                    ftpResponse.Close();
                }
                else
                {
                    mensagemErro = "Erro FTP: " + ex.Message;
                }

                return false;
            }
            catch (IOException ex)
            {
                mensagemErro = "Erro de arquivo: " + ex.Message;
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                mensagemErro = "Acesso negado: " + ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                mensagemErro = "Erro ao anexar arquivo via FTP: " + ex.Message;
                return false;
            }
        }

        private static Uri MontarUrlFtp(string host, string pastaRemota, string nomeArquivoRemoto)
        {
            string hostNormalizado = (host ?? "").Trim().Replace('\\', '/').TrimEnd('/');
            if (hostNormalizado.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
            {
                hostNormalizado = hostNormalizado.Substring("ftp://".Length).Trim('/');
            }

            string pastaNormalizada = (pastaRemota ?? "").Replace('\\', '/').Trim('/');
            string nomeNormalizado = (nomeArquivoRemoto ?? "").Replace('\\', '/').Trim('/');
            if (string.IsNullOrWhiteSpace(hostNormalizado))
            {
                throw new InvalidOperationException("Host FTP não configurado.");
            }

            if (string.IsNullOrWhiteSpace(nomeNormalizado))
            {
                throw new ArgumentException("Nome do arquivo remoto não informado.", "nomeArquivoRemoto");
            }

            string caminho = string.IsNullOrWhiteSpace(pastaNormalizada)
                ? Uri.EscapeDataString(nomeNormalizado)
                : pastaNormalizada + "/" + Uri.EscapeDataString(nomeNormalizado);
            return new Uri("ftp://" + hostNormalizado + "/" + caminho);
        }

        public bool Upload(string _nomeArquivo, string Caminho, bool v)
        {
            try
            {
                this.Tot = 0;
                string arquivoLocal = _nomeArquivo ?? "";
                string diretorioRemoto = Caminho == null ? "(null)" : Caminho;
                bool arquivoExiste = !string.IsNullOrWhiteSpace(_nomeArquivo) && File.Exists(_nomeArquivo);
                long tamanhoArquivo = arquivoExiste ? new FileInfo(_nomeArquivo).Length : 0;
                RegistrarDiagnosticoUpload(
                    "Inicio",
                    "INICIO",
                    "Arquivo=" + arquivoLocal +
                    "; Existe=" + arquivoExiste +
                    "; Tamanho=" + tamanhoArquivo +
                    "; Servidor=" + (this.ftpIPServidor ?? "") +
                    "; DiretorioRemoto=" + diretorioRemoto +
                    "; UsuarioConfigurado=" + !string.IsNullOrWhiteSpace(this.ftpUsuarioID) +
                    "; SenhaConfigurada=" + !string.IsNullOrWhiteSpace(this.ftpSenha) +
                    "; UsaProgress=" + v +
                    "; ProgressBarConfigurada=" + (this.ProgressBar1 != null));

                if (string.IsNullOrWhiteSpace(_nomeArquivo))
                {
                    throw new ArgumentException("Arquivo local não informado.", "_nomeArquivo");
                }

                if (!File.Exists(_nomeArquivo))
                {
                    throw new FileNotFoundException("Arquivo local não existe: " + _nomeArquivo, _nomeArquivo);
                }

                if (string.IsNullOrWhiteSpace(this.ftpIPServidor))
                {
                    throw new InvalidOperationException("Servidor FTP não configurado.");
                }

                if (Caminho == null)
                {
                    throw new ArgumentNullException("Caminho", "Diretório remoto FTP não informado.");
                }

                string Cam = Caminho.Replace(@"\", @"/");
                FileInfo _arquivoInfo = new FileInfo(_nomeArquivo);
                string Suri = "ftp://" + this.ftpIPServidor + @"/" + Cam + @"/" + _arquivoInfo.Name;
                RegistrarDiagnosticoUpload(
                    "ConfigurarRequisicao",
                    "INFO",
                    "Url=" + Suri +
                    "; Arquivo=" + _arquivoInfo.FullName +
                    "; Tamanho=" + _arquivoInfo.Length +
                    "; UsuarioConfigurado=" + !string.IsNullOrWhiteSpace(this.ftpUsuarioID) +
                    "; SenhaConfigurada=" + !string.IsNullOrWhiteSpace(this.ftpSenha) +
                    "; UsaProgress=" + v +
                    "; ProgressBarConfigurada=" + (this.ProgressBar1 != null));

                this.TemProgress = v && this.ProgressBar1 != null;
                if (this.TemProgress)
                {
                    this.ProgressBar1.Visible = true;
                    this.ProgressBar1.Maximum = (int)_arquivoInfo.Length;
                    this.ProgressBar1.Enabled = true;
                }
                bool sair = false;
                bool bReturn = false;
                int tentativa = 0;
                while (sair==false) {
                    tentativa++;
                    RegistrarDiagnosticoUpload("Upload", "TENTATIVA", "Numero=" + tentativa + "; Arquivo=" + _arquivoInfo.FullName);

                    FtpWebRequest requisicaoFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(Suri));
                    requisicaoFTP.Credentials = new NetworkCredential(this.ftpUsuarioID, this.ftpSenha);
                    requisicaoFTP.KeepAlive = false;
                    requisicaoFTP.Method = WebRequestMethods.Ftp.UploadFile;
                    requisicaoFTP.UseBinary = true;
                    requisicaoFTP.ContentLength = _arquivoInfo.Length;

                    string ret;
                    using (FileStream fs = _arquivoInfo.OpenRead())
                    {
                        ret = this.UploadEmSi(requisicaoFTP, fs);
                    }
                    if (ret=="")
                    {
                        bReturn = true;
                        sair = true;
                        RegistrarDiagnosticoUpload("Upload", "SUCESSO", "Arquivo=" + _arquivoInfo.FullName + "; Tamanho=" + _arquivoInfo.Length);
                    } else
                    {
                        if (ret == null)
                        {
                            RegistrarDiagnosticoUpload(
                                "UploadEmSi",
                                "RETORNO_NULO",
                                "O upload não retornou confirmação de sucesso nem mensagem de erro.");
                        }
                        else
                        {
                            RegistrarDiagnosticoUpload("UploadEmSi", "ERRO", "Retorno=" + ret);
                        }
                        RegistrarDiagnosticoUpload("Upload", "FALHA", "Arquivo=" + _arquivoInfo.FullName + "; Retorno=" + ret);
                        if (tentativa == 1 && ret != null && ret.IndexOf("553") >= 0)
                        {
                            string sUrlD = "ftp://" + this.ftpIPServidor + Cam;
                            RegistrarDiagnosticoUpload("CriacaoDiretorio", "INICIO", "DiretorioRemoto=" + Cam + "; Url=" + sUrlD + "; Motivo=553");
                            FtpWebRequest requestCD = (FtpWebRequest)FtpWebRequest.Create(new Uri(sUrlD));
                            requestCD.Credentials = new NetworkCredential(this.ftpUsuarioID, this.ftpSenha);
                            requestCD.KeepAlive = false;
                            requestCD.Method = WebRequestMethods.Ftp.MakeDirectory;
                            try
                            {
                                using (var resp = (FtpWebResponse)requestCD.GetResponse())
                                {
                                    Console.WriteLine(resp.StatusCode);
                                    RegistrarDiagnosticoUpload("CriacaoDiretorio", "SUCESSO", "DiretorioRemoto=" + Cam + "; Status=" + resp.StatusCode);
                                }
                                RegistrarDiagnosticoUpload("Upload", "RETRY", "Nova tentativa após criação do diretório; Arquivo=" + _arquivoInfo.FullName);
                            }
                            catch (WebException ex)
                            {
                                RegistrarExcecaoUpload("CriacaoDiretorio", ex);
                                bReturn = false;
                                sair = true;
                            }
                            catch (Exception ex)
                            {
                                RegistrarExcecaoUpload("CriacaoDiretorio", ex);
                                bReturn = false;
                                sair = true;
                            }
                        }
                        else
                        {
                            bReturn = false;
                            sair = true;
                        }
                    }
                }
                RegistrarDiagnosticoUpload("Fim", bReturn ? "SUCESSO" : "FALHA", "Arquivo=" + _arquivoInfo.FullName + "; ResultadoUpload=" + bReturn);
                return bReturn;
            }
            catch (WebException ex)
            {
                RegistrarExcecaoUpload("Upload", ex);
                throw;
            }
            catch (Exception ex)
            {
                RegistrarExcecaoUpload("Upload", ex);
                throw;
            }
        }

        private string UploadEmSi(FtpWebRequest requisicaoFTP, FileStream fs)
        {
            try
            {
                // Stream  para o qual o arquivo a ser enviado será escrito
                using (Stream strm = requisicaoFTP.GetRequestStream())
                {
                    int buffLength = 2048;
                    byte[] buff = new byte[buffLength];

                    // Lê a partir do arquivo stream, 2k por vez
                    this.tamanhoConteudo = fs.Read(buff, 0, buffLength);

                    // ate o conteudo do stream terminar
                    while (this.tamanhoConteudo != 0)
                    {
                        // Escreve o conteudo a partir do arquivo para o stream FTP
                        strm.Write(buff, 0, this.tamanhoConteudo);
                        this.tamanhoConteudo = fs.Read(buff, 0, buffLength);
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                RegistrarExcecaoUpload("UploadEmSi", ex);
                return ex.Message;
            }
        }

        public int LerVersaoDoFtp()
        {
            string caminhoArquivo = "/public_html/public/entregas/versao.txt";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + this.ftpIPServidor + caminhoArquivo));
            request.Credentials = new NetworkCredential(this.ftpUsuarioID, this.ftpSenha);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.UsePassive = true;
            FtpWebResponse response;
            try
            {
                response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                return -1;
                // throw new Exception("Erro ao conectar ao servidor FTP: " + ex.Message);
            }
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string info = reader.ReadToEnd();
            string[] lines = info.Split(';');
            string versaoTexto = lines[0];
            if (lines.Length>1)
            {
                this.Mensagem = lines[1];
            }            
            reader.Close();
            responseStream.Close();
            response.Close();
            int versaoNumero = int.Parse(versaoTexto.Replace(".", ""));
            return versaoNumero;
        }

        public string retMensagem()
        {
            return this.Mensagem;
        }

        public bool Testa()
        {
            string StringTeste = "Teste do FtpTeitor";
            string Suri = "ftp://" + this.ftpIPServidor + @"/Teste.tst";
            FtpWebRequest requisicaoFTP;
            requisicaoFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(Suri));
            requisicaoFTP.Credentials = new NetworkCredential(this.ftpUsuarioID, this.ftpSenha);            
            requisicaoFTP.KeepAlive = false;
            requisicaoFTP.Method = WebRequestMethods.Ftp.UploadFile;
            requisicaoFTP.UseBinary = true;
            requisicaoFTP.ContentLength = 9;
            //int buffLength = 2048;
            byte[] buff = Encoding.ASCII.GetBytes(StringTeste);
            bool ret = false;
            try
            {
                Stream strm = requisicaoFTP.GetRequestStream();
                strm.Write(buff, 0, StringTeste.Length);                
                FtpWebRequest redDown = (FtpWebRequest)WebRequest.Create(Suri);
                redDown.Method = WebRequestMethods.Ftp.DownloadFile;
                redDown.Credentials = new NetworkCredential(this.ftpUsuarioID, this.ftpSenha);
                FtpWebResponse respDown = (FtpWebResponse)redDown.GetResponse();
                Stream responseStream = respDown.GetResponseStream();
                StreamReader readerD = new StreamReader(responseStream);
                string resposta = readerD.ReadToEnd();
                strm.Close();
                readerD.Close();
                respDown.Close();
                ret = true;
            }
            catch (Exception ex)
            {
                ret= false;
            }
            if (ret)
            {
                // Deleção do arquivo de testes, se der erro na deleção ainda assim a conexão é valida, porque será utilizado para upload
                FtpWebRequest redDel = (FtpWebRequest)WebRequest.Create(Suri);
                redDel.Method = WebRequestMethods.Ftp.DeleteFile;
                redDel.Credentials = new NetworkCredential(this.ftpUsuarioID, this.ftpSenha);
                FtpWebResponse response = (FtpWebResponse)redDel.GetResponse();
                response.Close();
            }
            return ret;
        }
        public string getErro()
        {
            return this.Erro;
        }

        public void setBarra(ref ProgressBar ProgressBar1)
        {
            this.ProgressBar1 = ProgressBar1;
            Console.WriteLine("this.ProgressBar1 = ProgressBar1");
        }

    }
}


