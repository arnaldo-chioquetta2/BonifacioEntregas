using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TeleBonifacio
{
    public static class EnvioLogRemotoEtiquetas
    {
        private static readonly object SincronizacaoEnvio = new object();
        private static bool envioEmAndamento;

        public static void DispararEnvioAssincrono()
        {
            try
            {
                if (!glo.LogRemoto)
                {
                    return;
                }

                Task.Run(() =>
                {
                    try
                    {
                        string mensagemErro;
                        EnviarPendenciasDoDia(out mensagemErro);
                    }
                    catch (Exception ex)
                    {
                        LogRemotoEtiquetas.Registrar("ErroDisparoEnvioFTP", "ERRO_LOCAL", ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.Registrar("ErroDisparoEnvioFTP", "ERRO_LOCAL", ex.Message);
            }
        }

        public static bool EnviarPendenciasDoDia(out string mensagemErro)
        {
            mensagemErro = "";
            if (!glo.LogRemoto)
            {
                return true;
            }

            if (!IniciarEnvio(out mensagemErro))
            {
                return false;
            }

            try
            {
                int numeroDia = LogRemotoEtiquetas.DiaDaSemanaParaNumero(DateTime.Now.DayOfWeek);
                return EnviarPendenciasDoArquivo(numeroDia, out mensagemErro);
            }
            finally
            {
                FinalizarEnvio();
            }
        }

        public static void DispararEnvioDeTodasPendenciasAssincrono()
        {
            try
            {
                if (!glo.LogRemoto)
                {
                    return;
                }

                Task.Run(() =>
                {
                    try
                    {
                        EnviarPendenciasDeTodas();
                    }
                    catch (Exception ex)
                    {
                        LogRemotoEtiquetas.Registrar("ErroDisparoEnvioFTP", "ERRO_LOCAL", ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.Registrar("ErroDisparoEnvioFTP", "ERRO_LOCAL", ex.Message);
            }
        }

        private static void EnviarPendenciasDeTodas()
        {
            string mensagemErro;
            if (!IniciarEnvio(out mensagemErro))
            {
                return;
            }

            try
            {
                int hoje = LogRemotoEtiquetas.DiaDaSemanaParaNumero(DateTime.Now.DayOfWeek);
                for (int deslocamento = 1; deslocamento <= 7; deslocamento++)
                {
                    int numeroDia = ((hoje - 1 + deslocamento) % 7) + 1;
                    EnviarPendenciasDoArquivo(numeroDia, out mensagemErro);
                }
            }
            finally
            {
                FinalizarEnvio();
            }
        }

        private static bool IniciarEnvio(out string mensagemErro)
        {
            mensagemErro = "";
            lock (SincronizacaoEnvio)
            {
                if (envioEmAndamento)
                {
                    mensagemErro = "Ja existe um envio de log remoto em andamento.";
                    return false;
                }

                envioEmAndamento = true;
                return true;
            }
        }

        private static void FinalizarEnvio()
        {
            lock (SincronizacaoEnvio)
            {
                envioEmAndamento = false;
            }
        }

        private static bool EnviarPendenciasDoArquivo(int numeroDia, out string mensagemErro)
        {
            mensagemErro = "";
            string caminhoPendente = LogRemotoEtiquetas.ObterCaminhoPendente(numeroDia);
            string caminhoTemporario = "";
            string lote = "";
            bool loteRemovido = false;

            try
            {
                lock (LogRemotoEtiquetas.SincronizacaoArquivos)
                {
                    if (!File.Exists(caminhoPendente))
                    {
                        return true;
                    }

                    lote = File.ReadAllText(caminhoPendente);
                    if (string.IsNullOrEmpty(lote))
                    {
                        return true;
                    }

                    caminhoTemporario = Path.Combine(
                        Path.GetDirectoryName(caminhoPendente),
                        numeroDia + "logremoto_envio_" + Guid.NewGuid().ToString("N") + ".tmp");
                    File.WriteAllText(caminhoTemporario, lote, Encoding.UTF8);
                    File.WriteAllText(caminhoPendente, "", Encoding.UTF8);
                    loteRemovido = true;
                }

                string nomeRemoto = numeroDia + "logremoto.log";
                string erroFtp;
                bool enviado = FTP.Append(caminhoTemporario, "/public/entregas/log", nomeRemoto, out erroFtp);
                if (!enviado)
                {
                    mensagemErro = string.IsNullOrWhiteSpace(erroFtp) ? "Falha ao enviar pendencias por FTP." : erroFtp;
                    RestaurarLote(caminhoPendente, lote);
                    loteRemovido = false;
                    LogRemotoEtiquetas.Registrar("FalhaEnvioFTP", "ERRO_LOCAL", mensagemErro + "; Arquivo remoto=" + nomeRemoto);
                    ExcluirTemporario(caminhoTemporario);
                    return false;
                }

                long bytesEnviados = new FileInfo(caminhoTemporario).Length;
                LogRemotoEtiquetas.Registrar("EnvioFTPSucesso", "OK", "Arquivo remoto=" + nomeRemoto + "; Bytes enviados=" + bytesEnviados);
                ExcluirTemporario(caminhoTemporario);
                return true;
            }
            catch (IOException ex)
            {
                return FalhaLocal(ex, caminhoPendente, caminhoTemporario, lote, ref loteRemovido, out mensagemErro);
            }
            catch (UnauthorizedAccessException ex)
            {
                return FalhaLocal(ex, caminhoPendente, caminhoTemporario, lote, ref loteRemovido, out mensagemErro);
            }
            catch (Exception ex)
            {
                return FalhaLocal(ex, caminhoPendente, caminhoTemporario, lote, ref loteRemovido, out mensagemErro);
            }
        }

        private static bool FalhaLocal(Exception ex, string caminhoPendente, string caminhoTemporario, string lote, ref bool loteRemovido, out string mensagemErro)
        {
            mensagemErro = ex.Message;
            try
            {
                if (loteRemovido)
                {
                    RestaurarLote(caminhoPendente, lote);
                    loteRemovido = false;
                }

                LogRemotoEtiquetas.Registrar("FalhaEnvioFTP", "ERRO_LOCAL", mensagemErro);
                ExcluirTemporario(caminhoTemporario);
            }
            catch (Exception registroEx)
            {
                mensagemErro = mensagemErro + " | Falha ao preservar o lote: " + registroEx.Message;
            }

            return false;
        }

        private static void RestaurarLote(string caminhoPendente, string lote)
        {
            if (string.IsNullOrEmpty(lote))
            {
                return;
            }

            lock (LogRemotoEtiquetas.SincronizacaoArquivos)
            {
                string novosRegistros = File.Exists(caminhoPendente) ? File.ReadAllText(caminhoPendente) : "";
                File.WriteAllText(caminhoPendente, lote + novosRegistros, Encoding.UTF8);
            }
        }

        private static void ExcluirTemporario(string caminhoTemporario)
        {
            if (string.IsNullOrWhiteSpace(caminhoTemporario) || !File.Exists(caminhoTemporario))
            {
                return;
            }

            try
            {
                File.Delete(caminhoTemporario);
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.Registrar("FalhaLimpezaTemporario", "ERRO_LOCAL", ex.Message);
            }
        }
    }
}
