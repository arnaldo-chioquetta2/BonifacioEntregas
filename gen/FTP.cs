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
                if (this.TemProgress)
                {
                    this.ProgressBar1.Value = Tot;
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
            this.Tot = 0;
            string Cam = Caminho.Replace(@"\", @"/");
            FileInfo _arquivoInfo = new FileInfo(_nomeArquivo);
            string Suri = "ftp://" + this.ftpIPServidor + @"/" + Cam + @"/" + _arquivoInfo.Name;
            FtpWebRequest requisicaoFTP;
            requisicaoFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(Suri));
            requisicaoFTP.Credentials = new NetworkCredential(this.ftpUsuarioID, this.ftpSenha);
            requisicaoFTP.KeepAlive = false;
            requisicaoFTP.Method = WebRequestMethods.Ftp.UploadFile;
            requisicaoFTP.UseBinary = true;
            requisicaoFTP.ContentLength = _arquivoInfo.Length;
            this.TemProgress = v;
            if (this.TemProgress)
            {
                this.ProgressBar1.Visible = true;
                this.ProgressBar1.Maximum = (int)_arquivoInfo.Length;
                this.ProgressBar1.Enabled = true;
            }
            FileStream fs = _arquivoInfo.OpenRead();
            bool sair = false;
            bool bReturn = false;
            while (sair==false) {
                string ret = this.UploadEmSi(requisicaoFTP, fs);
                if (ret=="")
                {
                    bReturn = true;
                    sair = true;
                } else
                {
                    if (ret.IndexOf("553") > 0)
                    {
                        string sUrlD = "ftp://" + this.ftpIPServidor + Cam;
                        FtpWebRequest requestCD = (FtpWebRequest)FtpWebRequest.Create(new Uri(sUrlD));
                        requestCD.Credentials = new NetworkCredential(this.ftpUsuarioID, this.ftpSenha);
                        requestCD.KeepAlive = false;
                        requestCD.Method = WebRequestMethods.Ftp.MakeDirectory;
                        requestCD.Credentials = new NetworkCredential("user", "pass");
                        try
                        {
                            using (var resp = (FtpWebResponse)requestCD.GetResponse())
                            {
                                Console.WriteLine(resp.StatusCode);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Não foi possivel enviar arquivo", "É necessário criar o diretório");
                            bReturn = false;
                            sair = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show(ret, "Erro não tratado");
                        bReturn = false;
                        sair = true;
                    }
                } 
            }
            return bReturn;
        }

        private string UploadEmSi(FtpWebRequest requisicaoFTP, FileStream fs)
        {
            try
            {
                // Stream  para o qual o arquivo a ser enviado será escrito
                Stream strm = requisicaoFTP.GetRequestStream();

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

                // Fecha o stream a requisição
                strm.Close();
                fs.Close();
                return "";
            }
            catch (Exception ex)
            {
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


