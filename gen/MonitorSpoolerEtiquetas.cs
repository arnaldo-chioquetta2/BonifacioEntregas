using System;
using System.Collections.Generic;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public static class MonitorSpoolerEtiquetas
    {
        private const int TimeoutSegundos = 20;
        private const int IntervaloMilissegundos = 500;

        public static void Iniciar(string nomeDocumento, string impressora, EtiquetaModel etiqueta, int quantidade, string tentativaId)
        {
            try
            {
                if (!glo.LogRemoto || string.IsNullOrWhiteSpace(nomeDocumento))
                {
                    return;
                }

                string codigo = etiqueta != null ? etiqueta.Codigo : "";
                string nomeEtiqueta = etiqueta != null ? etiqueta.NomeEtiqueta : "";
                string tentativaIdLocal = tentativaId ?? "";
                Task.Run(() => Monitorar(nomeDocumento, impressora, quantidade, codigo, nomeEtiqueta, tentativaIdLocal));
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.RegistrarErroPendente("ErroMonitorSpooler", ex, impressora, quantidade, etiqueta != null ? etiqueta.Codigo : "", etiqueta != null ? etiqueta.NomeEtiqueta : "", tentativaId);
                EnvioLogRemotoEtiquetas.DispararEnvioAssincrono();
            }
        }

        private static void Monitorar(string nomeDocumento, string impressora, int quantidade, string codigo, string nomeEtiqueta, string tentativaId)
        {
            try
            {
                DateTime inicio = DateTime.Now;
                bool encontrado = false;
                bool houveErro = false;
                string ultimoStatus = "";
                TrabalhoSpooler ultimoTrabalho = null;

                while ((DateTime.Now - inicio).TotalSeconds <= TimeoutSegundos)
                {
                    if (!glo.LogRemoto)
                    {
                        return;
                    }

                    List<TrabalhoSpooler> trabalhos = ConsultarTrabalhos(impressora, nomeDocumento);
                    if (trabalhos.Count > 0)
                    {
                        encontrado = true;

                        foreach (TrabalhoSpooler trabalho in trabalhos)
                        {
                            ultimoTrabalho = trabalho;
                            string status = string.IsNullOrWhiteSpace(trabalho.Status) ? "Desconhecido" : trabalho.Status;
                            if (!string.Equals(status, ultimoStatus, StringComparison.OrdinalIgnoreCase))
                            {
                                ultimoStatus = status;
                                bool estadoErro = EhEstadoErro(status, DateTime.Now - inicio);
                                houveErro = houveErro || estadoErro;
                                string etapa = estadoErro ? "SpoolerErro" : "SpoolerEncontrado";
                                string resultado = estadoErro ? "ERRO" : "OK";
                                string mensagem = CriarMensagemTrabalho(nomeDocumento, impressora, trabalho, inicio);
                                if (estadoErro)
                                {
                                    LogRemotoEtiquetas.RegistrarPendente(etapa, resultado, mensagem, impressora, quantidade, codigo, nomeEtiqueta, tentativaId);
                                    EnvioLogRemotoEtiquetas.DispararEnvioAssincrono();
                                }
                                else
                                {
                                    LogRemotoEtiquetas.Registrar(etapa, resultado, mensagem, impressora, quantidade, codigo, nomeEtiqueta, tentativaId);
                                }
                            }
                        }
                    }
                    else if (encontrado && !houveErro)
                    {
                        LogRemotoEtiquetas.RegistrarPendente(
                            "SpoolerProcessado",
                            "OK",
                            "Trabalho nao esta mais presente na fila; processado pelo spooler. " + CriarMensagemTrabalho(nomeDocumento, impressora, ultimoTrabalho, inicio),
                            impressora,
                            quantidade,
                            codigo,
                            nomeEtiqueta,
                            tentativaId);
                        EnvioLogRemotoEtiquetas.DispararEnvioAssincrono();
                        return;
                    }

                    Thread.Sleep(IntervaloMilissegundos);
                }

                string etapaTimeout = encontrado ? "SpoolerTimeout" : "SpoolerNaoEncontrado";
                string mensagemTimeout = encontrado
                    ? "Trabalho permaneceu na fila ate o timeout. " + CriarMensagemTrabalho(nomeDocumento, impressora, ultimoTrabalho, inicio)
                    : "Trabalho nao encontrado na fila dentro do tempo limite. DocumentName=" + nomeDocumento + "; Impressora=" + impressora + "; TempoDecorridoMs=" + ObterTempoDecorridoMs(inicio);
                LogRemotoEtiquetas.RegistrarPendente(
                    etapaTimeout,
                    "NAO_CONFIRMADO",
                    mensagemTimeout,
                    impressora,
                    quantidade,
                    codigo,
                    nomeEtiqueta,
                    tentativaId);
                EnvioLogRemotoEtiquetas.DispararEnvioAssincrono();
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.RegistrarErroPendente("ErroMonitorSpooler", ex, impressora, quantidade, codigo, nomeEtiqueta, tentativaId);
                EnvioLogRemotoEtiquetas.DispararEnvioAssincrono();
            }
        }

        private static List<TrabalhoSpooler> ConsultarTrabalhos(string impressora, string nomeDocumento)
        {
            List<TrabalhoSpooler> trabalhos = new List<TrabalhoSpooler>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "SELECT JobId, Name, Document, Owner, Status, JobStatus, TotalPages, PagesPrinted, Size, TimeSubmitted FROM Win32_PrintJob"))
            using (ManagementObjectCollection resultados = searcher.Get())
            {
                foreach (ManagementObject job in resultados)
                {
                    using (job)
                    {
                        string document = ObterTextoWmi(job, "Document");
                        string name = ObterTextoWmi(job, "Name");
                        if (!string.Equals(document, nomeDocumento, StringComparison.OrdinalIgnoreCase) || !EhImpressora(name, impressora))
                        {
                            continue;
                        }

                        trabalhos.Add(new TrabalhoSpooler
                        {
                            JobId = ObterTextoWmi(job, "JobId"),
                            Name = name,
                            Document = document,
                            Owner = ObterTextoWmi(job, "Owner"),
                            Status = ObterStatus(job),
                            JobStatus = ObterTextoWmi(job, "JobStatus"),
                            TotalPages = ConverterInteiro(ObterValorWmi(job, "TotalPages")),
                            PagesPrinted = ConverterInteiro(ObterValorWmi(job, "PagesPrinted")),
                            Size = ObterTextoWmi(job, "Size"),
                            TimeSubmitted = ObterTextoWmi(job, "TimeSubmitted")
                        });
                    }
                }
            }

            return trabalhos;
        }

        private static bool EhImpressora(string nomeTrabalho, string impressora)
        {
            if (string.IsNullOrWhiteSpace(nomeTrabalho))
            {
                return false;
            }

            int separador = nomeTrabalho.LastIndexOf(',');
            string nome = separador > 0 ? nomeTrabalho.Substring(0, separador) : nomeTrabalho;
            return string.Equals(nome.Trim(), (impressora ?? "").Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private static string ObterStatus(ManagementObject job)
        {
            string status = ObterTextoWmi(job, "JobStatus");
            if (string.IsNullOrWhiteSpace(status) || status == "(nulo)")
            {
                status = ObterTextoWmi(job, "Status");
            }

            return status;
        }

        private static object ObterValorWmi(ManagementObject job, string propriedade)
        {
            try
            {
                return job[propriedade];
            }
            catch
            {
                return null;
            }
        }

        private static string ObterTextoWmi(ManagementObject job, string propriedade)
        {
            object valor = ObterValorWmi(job, propriedade);
            return valor == null || Convert.IsDBNull(valor) ? "(nulo)" : Convert.ToString(valor);
        }

        private static int ConverterInteiro(object valor)
        {
            int resultado;
            return int.TryParse(Convert.ToString(valor), out resultado) ? resultado : 0;
        }

        private static long ObterTempoDecorridoMs(DateTime inicio)
        {
            return (long)(DateTime.Now - inicio).TotalMilliseconds;
        }

        private static string CriarMensagemTrabalho(string nomeDocumento, string impressora, TrabalhoSpooler trabalho, DateTime inicio)
        {
            if (trabalho == null)
            {
                return "DocumentName=" + nomeDocumento + "; Impressora=" + impressora + "; TempoDecorridoMs=" + ObterTempoDecorridoMs(inicio);
            }

            return "DocumentName=" + nomeDocumento +
                "; Impressora=" + impressora +
                "; JobId=" + trabalho.JobId +
                "; Owner=" + trabalho.Owner +
                "; Status=" + trabalho.Status +
                "; JobStatus=" + trabalho.JobStatus +
                "; TotalPages=" + trabalho.TotalPages +
                "; PagesPrinted=" + trabalho.PagesPrinted +
                "; Size=" + trabalho.Size +
                "; TimeSubmitted=" + trabalho.TimeSubmitted +
                "; TempoDecorridoMs=" + ObterTempoDecorridoMs(inicio);
        }

        private static bool EhEstadoErro(string status, TimeSpan tempoDecorrido)
        {
            string valor = status ?? "";
            return valor.IndexOf("Error", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   valor.IndexOf("Offline", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   valor.IndexOf("PaperOut", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   valor.IndexOf("Paused", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   valor.IndexOf("UserIntervention", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   valor.IndexOf("Blocked", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   (valor.IndexOf("Deleting", StringComparison.OrdinalIgnoreCase) >= 0 && tempoDecorrido.TotalSeconds >= 5);
        }

        private sealed class TrabalhoSpooler
        {
            public string JobId { get; set; }
            public string Name { get; set; }
            public string Document { get; set; }
            public string Owner { get; set; }
            public string Status { get; set; }
            public string JobStatus { get; set; }
            public int TotalPages { get; set; }
            public int PagesPrinted { get; set; }
            public string Size { get; set; }
            public string TimeSubmitted { get; set; }
        }
    }
}
