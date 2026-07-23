using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public static class LogRemotoEtiquetas
    {
        internal static readonly object SincronizacaoArquivos = new object();

        private static string PastaLogs
        {
            get { return Path.Combine(Application.StartupPath, "Logs", "Etiquetas"); }
        }

        public static void Registrar(
            string etapa,
            string resultado,
            string mensagem,
            string impressora = "",
            int quantidade = 0,
            string codigo = "",
            string nomeEtiqueta = "",
            string tentativaId = "")
        {
            try
            {
                if (!glo.LogRemoto)
                {
                    return;
                }

                lock (SincronizacaoArquivos)
                {
                    GravarRegistro(
                        NomeArquivoPrincipal(DateTime.Now),
                        CriarRegistro(DateTime.Now, etapa, resultado, mensagem, impressora, quantidade, codigo, nomeEtiqueta, "", "", tentativaId));
                }
            }
            catch
            {
                // O logger nunca deve interromper a operacao principal.
            }
        }

        public static void RegistrarErro(
            string etapa,
            Exception ex,
            string impressora = "",
            int quantidade = 0,
            string codigo = "",
            string nomeEtiqueta = "",
            string tentativaId = "")
        {
            try
            {
                Registrar(etapa, "ERRO", FormatarExcecao(ex), impressora, quantidade, codigo, nomeEtiqueta, tentativaId);
            }
            catch
            {
                // O logger nunca deve interromper a operacao principal.
            }
        }

        public static void RegistrarPendente(
            string etapa,
            string resultado,
            string mensagem,
            string impressora = "",
            int quantidade = 0,
            string codigo = "",
            string nomeEtiqueta = "",
            string tentativaId = "")
        {
            try
            {
                if (!glo.LogRemoto)
                {
                    return;
                }

                lock (SincronizacaoArquivos)
                {
                    DateTime agora = DateTime.Now;
                    string eventoId = Guid.NewGuid().ToString("N");
                    string nomeArquivo = NomeArquivoPrincipal(agora);
                    string registro = CriarRegistro(agora, etapa, resultado, mensagem, impressora, quantidade, codigo, nomeEtiqueta, eventoId, nomeArquivo, tentativaId);
                    GravarRegistro(nomeArquivo, registro);
                    GravarRegistro(NomeArquivoPendente(agora), registro);
                }
            }
            catch
            {
                // O logger nunca deve interromper a operacao principal.
            }
        }

        public static void RegistrarErroPendente(
            string etapa,
            Exception ex,
            string impressora = "",
            int quantidade = 0,
            string codigo = "",
            string nomeEtiqueta = "",
            string tentativaId = "")
        {
            try
            {
                RegistrarPendente(etapa, "ERRO", FormatarExcecao(ex), impressora, quantidade, codigo, nomeEtiqueta, tentativaId);
            }
            catch
            {
                // O logger nunca deve interromper a operacao principal.
            }
        }

        public static int DiaDaSemanaParaNumero(DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Sunday:
                    return 1;
                case DayOfWeek.Monday:
                    return 2;
                case DayOfWeek.Tuesday:
                    return 3;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 5;
                case DayOfWeek.Friday:
                    return 6;
                case DayOfWeek.Saturday:
                    return 7;
                default:
                    return 1;
            }
        }

        private static string LimparCampo(string valor)
        {
            return (valor ?? string.Empty)
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("|", "/");
        }

        internal static string ObterCaminhoPendente(DateTime data)
        {
            return Path.Combine(PastaLogs, DiaDaSemanaParaNumero(data.DayOfWeek) + "logremoto.pendente");
        }

        internal static string ObterCaminhoPendente(int numeroDia)
        {
            return Path.Combine(PastaLogs, numeroDia + "logremoto.pendente");
        }

        private static string NomeArquivoPrincipal(DateTime data)
        {
            return DiaDaSemanaParaNumero(data.DayOfWeek) + "logremoto.log";
        }

        private static string NomeArquivoPendente(DateTime data)
        {
            return DiaDaSemanaParaNumero(data.DayOfWeek) + "logremoto.pendente";
        }

        private static string CriarRegistro(
            DateTime data,
            string etapa,
            string resultado,
            string mensagem,
            string impressora,
            int quantidade,
            string codigo,
            string nomeEtiqueta,
            string eventoId,
            string nomeArquivoPrincipal,
            string tentativaId)
        {
            string registro = string.Format(
                "{0} | PC={1} | Usuario={2}{3} | Etapa={4} | Resultado={5} | Impressora={6} | Quantidade={7} | Codigo={8} | Etiqueta={9} | Mensagem={10}",
                data.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                LimparCampo(Environment.MachineName),
                LimparCampo(Environment.UserName),
                string.IsNullOrWhiteSpace(tentativaId) ? "" : " | TentativaId=" + LimparCampo(tentativaId),
                LimparCampo(etapa),
                LimparCampo(resultado),
                LimparCampo(impressora),
                quantidade,
                LimparCampo(codigo),
                LimparCampo(nomeEtiqueta),
                LimparCampo(mensagem));

            if (!string.IsNullOrWhiteSpace(eventoId))
            {
                registro += " | LogPrincipal=" + LimparCampo(nomeArquivoPrincipal) + " | EventoId=" + LimparCampo(eventoId);
            }

            return registro;
        }

        private static void GravarRegistro(string nomeArquivo, string registro)
        {
            Directory.CreateDirectory(PastaLogs);
            File.AppendAllText(Path.Combine(PastaLogs, nomeArquivo), registro + Environment.NewLine, Encoding.UTF8);
        }

        public static string FormatarExcecao(Exception ex)
        {
            try
            {
                if (ex == null)
                {
                    return "Excecao nula";
                }

                StringBuilder mensagem = new StringBuilder();
                Exception atual = ex;
                int nivel = 0;
                while (atual != null && nivel < 5)
                {
                    if (nivel > 0)
                    {
                        mensagem.Append(" | ");
                    }

                    mensagem.Append("Nivel=").Append(nivel)
                        .Append("; Tipo=").Append(atual.GetType().FullName ?? "(nulo)")
                        .Append("; Mensagem=").Append(NormalizarTextoExcecao(atual.Message))
                        .Append("; HResult=").Append(atual.HResult)
                        .Append("; HResultHex=0x").Append(unchecked((uint)atual.HResult).ToString("X8"))
                        .Append("; Source=").Append(NormalizarTextoExcecao(atual.Source))
                        .Append("; TargetSite=").Append(NormalizarTextoExcecao(atual.TargetSite == null ? "" : atual.TargetSite.ToString()))
                        .Append("; StackTrace=").Append(ResumirStackTrace(atual.StackTrace));

                    Win32Exception win32 = atual as Win32Exception;
                    if (win32 != null)
                    {
                        mensagem.Append("; NativeErrorCode=").Append(win32.NativeErrorCode)
                            .Append("; ErrorCode=").Append(win32.ErrorCode);
                    }

                    WebException web = atual as WebException;
                    if (web != null)
                    {
                        mensagem.Append("; WebStatus=").Append(web.Status);
                        FtpWebResponse ftp = web.Response as FtpWebResponse;
                        if (ftp != null)
                        {
                            mensagem.Append("; FtpStatusCode=").Append(ftp.StatusCode)
                                .Append("; FtpStatusDescription=").Append(NormalizarTextoExcecao(ftp.StatusDescription));
                        }
                        else if (web.Response != null)
                        {
                            HttpWebResponse http = web.Response as HttpWebResponse;
                            if (http != null)
                            {
                                mensagem.Append("; HttpStatusCode=").Append(http.StatusCode)
                                    .Append("; HttpStatusDescription=").Append(NormalizarTextoExcecao(http.StatusDescription));
                            }
                        }
                    }

                    atual = atual.InnerException;
                    nivel++;
                }

                if (atual != null)
                {
                    mensagem.Append(" | InnerExceptionLimitReached=True");
                }

                return mensagem.ToString();
            }
            catch (Exception erroFormatacao)
            {
                try
                {
                    return "Falha ao formatar excecao; Tipo=" +
                        (ex == null || ex.GetType() == null ? "(nulo)" : ex.GetType().FullName) +
                        "; Mensagem=" + NormalizarTextoExcecao(ex == null ? "" : ex.Message) +
                        "; ErroFormatacao=" + NormalizarTextoExcecao(erroFormatacao.Message);
                }
                catch
                {
                    return "Falha ao formatar excecao";
                }
            }
        }

        private static string ResumirStackTrace(string stackTrace)
        {
            if (string.IsNullOrWhiteSpace(stackTrace))
            {
                return "(nulo)";
            }

            string[] linhas = stackTrace.Replace("\r", "").Split('\n');
            StringBuilder resumo = new StringBuilder();
            int usadas = 0;
            foreach (string linha in linhas)
            {
                string limpa = NormalizarTextoExcecao(linha);
                if (string.IsNullOrWhiteSpace(limpa))
                {
                    continue;
                }

                if (usadas > 0)
                {
                    resumo.Append(" <- ");
                }

                resumo.Append(limpa);
                usadas++;
                if (usadas >= 3)
                {
                    break;
                }
            }

            return resumo.Length == 0 ? "(nulo)" : resumo.ToString();
        }

        private static string NormalizarTextoExcecao(string texto)
        {
            return (texto ?? "(nulo)")
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\t", " ")
                .Trim();
        }
    }
}
