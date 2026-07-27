using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class operEtiquetas : Form
    {
        private readonly EtiquetaRepository repository = new EtiquetaRepository();
        private List<EtiquetaModel> etiquetas = new List<EtiquetaModel>();
        private string etiquetaSelecionadaId = "";
        private string nomeEtiquetaSelecionada = "";
        private string linhaSelecionada = "Codigo";
        private bool carregandoFormatacao;
        private Dictionary<string, EtiquetaFonteConfig> fontesEdicao;
        private readonly Dictionary<string, RectangleF> areasPreview = new Dictionary<string, RectangleF>();
        private EtiquetaModel etiquetaImpressao;
        private int copiasRestantes;
        private int copiasTotais;
        private string impressoraImpressao = "";
        private string tentativaImpressaoAtual = "";
        private bool erroPrintPageTratado;
        private static readonly Dictionary<string, string> mapaLinhaFormatacao = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Nome da empresa", "NomeEmpresa" },
            { "NomeEmpresa", "NomeEmpresa" },
            { "Telefone", "Telefone" },
            { "Codigo", "Codigo" },
            { "Código", "Codigo" },
            { "Descricao", "Descricao" },
            { "Descrição", "Descricao" },
            { "Preco", "Preco" },
            { "Preço", "Preco" },
            { "Observacao", "Observacao" },
            { "Observação", "Observacao" },
            { "Texto livre", "Observacao" },
            { "TeleEntrega", "TeleEntrega" },
            { "Tele-entrega", "TeleEntrega" },
            { "Local", "Local" },
        };

        public operEtiquetas()
        {
            InitializeComponent();
            txtNomeEmpresa.TextChanged += CamposPreview_TextChanged;
            txtTelefone.TextChanged += CamposPreview_TextChanged;
            txtTeleEntrega.TextChanged += CamposPreview_TextChanged;
            txtCodigo.TextChanged += CamposPreview_TextChanged;
            txtDescricao.TextChanged += CamposPreview_TextChanged;
            txtPreco.TextChanged += CamposPreview_TextChanged;

            txtObservacao.Enter += txtObservacao_Enter;
            txtObservacao.Click += txtObservacao_Click;
            cmbLinhaFormatacao.SelectedIndexChanged += cmbLinhaFormatacao_SelectedIndexChanged;
            cmbFonte.SelectedIndexChanged += cmbFonte_SelectedIndexChanged;
            numTamanhoFonte.ValueChanged += numTamanhoFonte_ValueChanged;
            chkNegrito.CheckedChanged += chkNegrito_CheckedChanged;
        }

        private void operEtiquetas_Load(object sender, EventArgs e)
        {
            try
            {
                etiquetas = repository.Listar();
                LogRemotoEtiquetas.Registrar("CarregarEtiquetas", "OK", "Etiquetas carregadas: " + etiquetas.Count);
                ConfigurarGrid();
                CarregarGrid(etiquetas);
                CarregarImpressoras();
                cmbLinhaFormatacao.Items.Clear();
                cmbLinhaFormatacao.Items.Add("Nome da empresa");
                cmbLinhaFormatacao.Items.Add("Telefone");
                cmbLinhaFormatacao.Items.Add("Código");
                cmbLinhaFormatacao.Items.Add("Descrição");
                cmbLinhaFormatacao.Items.Add("Preço");
                cmbLinhaFormatacao.Items.Add("Observação");
                cmbLinhaFormatacao.Items.Add("Texto livre");
                cmbLinhaFormatacao.Items.Add("Tele-entrega");
                cmbLinhaFormatacao.Items.Add("Local");
                cmbLinhaFormatacao.SelectedItem = "Código";
                CarregarListaFontes();
                InicializarFontesEdicao(new EtiquetaModel());
                linhaSelecionada = "Codigo";
                CarregarControlesFormatacao();
                numQuantidade.Value = 1;
                pnlPreview.Invalidate();
                LogRemotoEtiquetas.Registrar("AberturaTela", "OK", "Tela de etiquetas aberta");
                EnvioLogRemotoEtiquetas.DispararEnvioDeTodasPendenciasAssincrono();
            }
            catch (Exception ex)
            {
                glo.Loga("Erro operEtiquetas_Load: " + ex.Message);
                LogRemotoEtiquetas.RegistrarErro("AberturaTela", ex);
            }
        }

        private void btNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            string codigoLog = txtCodigo.Text.Trim();
            string nomeEtiquetaLog = nomeEtiquetaSelecionada;
            LogRemotoEtiquetas.Registrar("InicioSalvar", "INICIO", "Início do salvamento", codigo: codigoLog, nomeEtiqueta: nomeEtiquetaLog);

            try
            {
                string codigo = txtCodigo.Text.Trim();
                string descricao = txtDescricao.Text.Trim();

                if (string.IsNullOrWhiteSpace(codigo) && string.IsNullOrWhiteSpace(descricao))
                {
                    MessageBox.Show("Informe o código ou a descrição da etiqueta.");
                    return;
                }

                EtiquetaModel etiqueta = ObterEtiquetaDaTela();
                codigoLog = etiqueta.Codigo;
                nomeEtiquetaLog = etiqueta.NomeEtiqueta;
                string idParaSelecionar = etiqueta.Id;
                bool atualizacao = !string.IsNullOrWhiteSpace(etiqueta.Id);

                if (string.IsNullOrWhiteSpace(etiqueta.Id))
                {
                    EtiquetaModel existente = !string.IsNullOrWhiteSpace(etiqueta.Codigo)
                        ? repository.BuscarPorCodigo(etiqueta.Codigo)
                        : null;

                    if (existente != null)
                    {
                        DialogResult pergunta = MessageBox.Show(
                            "Já existe uma etiqueta com este código. Deseja atualizar a etiqueta existente?",
                            "Confirmação",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (pergunta != DialogResult.Yes)
                        {
                            return;
                        }

                        etiqueta.Id = existente.Id;
                        idParaSelecionar = existente.Id;
                        atualizacao = true;
                        if (string.IsNullOrWhiteSpace(etiqueta.NomeEtiqueta))
                        {
                            etiqueta.NomeEtiqueta = existente.NomeEtiqueta;
                        }
                    }
                }

                string nomeEtiqueta = SolicitarNomeEtiqueta(etiqueta);
                if (nomeEtiqueta == null)
                {
                    return;
                }

                etiqueta.NomeEtiqueta = nomeEtiqueta;
                nomeEtiquetaSelecionada = nomeEtiqueta;
                nomeEtiquetaLog = nomeEtiqueta;

                repository.Salvar(etiqueta);
                etiquetas = repository.Listar();
                CarregarGrid(etiquetas);

                etiquetaSelecionadaId = etiqueta.Id;
                SelecionarEtiquetaNoGrid(idParaSelecionar);
                pnlPreview.Invalidate();

                LogRemotoEtiquetas.Registrar(
                    "SalvarEtiqueta",
                    "OK",
                    atualizacao ? "Etiqueta atualizada com sucesso" : "Etiqueta incluída com sucesso",
                    codigo: etiqueta.Codigo,
                    nomeEtiqueta: etiqueta.NomeEtiqueta);

                MessageBox.Show("Etiqueta salva com sucesso.");
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao salvar etiqueta: " + ex.Message);
                LogRemotoEtiquetas.RegistrarErro("ErroSalvar", ex, codigo: codigoLog, nomeEtiqueta: nomeEtiquetaLog);
                MessageBox.Show("Não foi possível salvar a etiqueta. Tente novamente.");
            }
        }

        private void btExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(etiquetaSelecionadaId))
                {
                    MessageBox.Show("Selecione uma etiqueta para excluir.");
                    return;
                }

                DialogResult confirmar = MessageBox.Show(
                    "Deseja realmente excluir esta etiqueta?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmar != DialogResult.Yes)
                {
                    return;
                }

                EtiquetaModel etiquetaExclusao = etiquetas.FirstOrDefault(item => string.Equals(item.Id, etiquetaSelecionadaId, StringComparison.OrdinalIgnoreCase));
                LogRemotoEtiquetas.Registrar(
                    "InicioExcluir",
                    "INICIO",
                    "Início da exclusão",
                    codigo: etiquetaExclusao != null ? etiquetaExclusao.Codigo : txtCodigo.Text.Trim(),
                    nomeEtiqueta: etiquetaExclusao != null ? ObterNomeEtiquetaSugerido(etiquetaExclusao) : nomeEtiquetaSelecionada);

                repository.Excluir(etiquetaSelecionadaId);
                etiquetas = repository.Listar();
                CarregarGrid(etiquetas);
                LimparCampos();
                pnlPreview.Invalidate();

                LogRemotoEtiquetas.Registrar(
                    "ExcluirEtiqueta",
                    "OK",
                    "Etiqueta excluída com sucesso",
                    codigo: etiquetaExclusao != null ? etiquetaExclusao.Codigo : txtCodigo.Text.Trim(),
                    nomeEtiqueta: etiquetaExclusao != null ? ObterNomeEtiquetaSugerido(etiquetaExclusao) : nomeEtiquetaSelecionada);

                MessageBox.Show("Etiqueta excluída com sucesso.");
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao excluir etiqueta: " + ex.Message);
                LogRemotoEtiquetas.RegistrarErro("ErroExcluir", ex, codigo: txtCodigo.Text.Trim(), nomeEtiqueta: nomeEtiquetaSelecionada);
                MessageBox.Show("Não foi possível excluir a etiqueta. Tente novamente.");
            }
        }

        private void btLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            string tentativaId = "";
            try
            {
                EtiquetaModel etiqueta = ObterEtiquetaDaTela();

                if (string.IsNullOrWhiteSpace(etiqueta.Id) &&
                    string.IsNullOrWhiteSpace(etiqueta.Codigo) &&
                    string.IsNullOrWhiteSpace(etiqueta.Descricao))
                {
                    MessageBox.Show("Informe ou selecione uma etiqueta para imprimir.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(etiqueta.Codigo) && string.IsNullOrWhiteSpace(etiqueta.Descricao))
                {
                    MessageBox.Show("Informe ou selecione uma etiqueta para imprimir.");
                    return;
                }

                int quantidade = (int)numQuantidade.Value;
                if (quantidade <= 0)
                {
                    MessageBox.Show("A quantidade deve ser maior que zero.");
                    return;
                }

                if (cmbImpressora.SelectedItem == null)
                {
                    MessageBox.Show("Selecione uma impressora.");
                    return;
                }

                string impressora = cmbImpressora.SelectedItem.ToString();
                tentativaId = Guid.NewGuid().ToString("N");
                LogRemotoEtiquetas.Registrar(
                    "InicioImpressao",
                    "INICIO",
                    "Início da impressão",
                    impressora,
                    quantidade,
                    etiqueta.Codigo,
                    etiqueta.NomeEtiqueta,
                    tentativaId);
                RegistrarAmbienteImpressao(impressora, etiqueta, quantidade, tentativaId);
                RegistrarDetalhesWmiImpressora(impressora, etiqueta, quantidade, tentativaId);
                RegistrarEstadoServicoSpooler(impressora, etiqueta, quantidade, tentativaId);
                RegistrarDriverEPortaImpressora(impressora, etiqueta, quantidade, tentativaId);
                ImprimirEtiqueta(etiqueta, impressora, quantidade, tentativaId);
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao imprimir etiqueta: " + ex.Message);
                LogRemotoEtiquetas.RegistrarErro(
                    "ErroImpressao",
                    ex,
                    cmbImpressora.SelectedItem != null ? cmbImpressora.SelectedItem.ToString() : "",
                    (int)numQuantidade.Value,
                    txtCodigo.Text.Trim(),
                    nomeEtiquetaSelecionada,
                    tentativaId);
                MessageBox.Show("Não foi possível imprimir a etiqueta. Tente novamente.");
            }
        }

        private void RegistrarAmbienteImpressao(string impressora, EtiquetaModel etiqueta, int quantidade, string tentativaId)
        {
            try
            {
                List<string> impressorasInstaladas = new List<string>();
                foreach (string item in PrinterSettings.InstalledPrinters)
                {
                    impressorasInstaladas.Add(item);
                }

                PrinterSettings settings = new PrinterSettings();
                string impressoraPadrao = settings.PrinterName;

                Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                string caminhoExecutavel = assembly.Location;
                string versaoExecutavel = assembly.GetName().Version != null
                    ? assembly.GetName().Version.ToString()
                    : "";
                string nomeLimpo = (impressora ?? "").Trim();
                bool apareceNaListaIgnorandoCaixa = impressorasInstaladas.Any(item =>
                    string.Equals(item, impressora, StringComparison.OrdinalIgnoreCase));
                bool apareceNaListaExata = impressorasInstaladas.Any(item => item == impressora);
                bool possuiEspacosNasExtremidades = !string.Equals(impressora, nomeLimpo, StringComparison.Ordinal);
                bool pareceCompartilhamentoUnc = nomeLimpo.StartsWith("\\\\", StringComparison.Ordinal);
                bool pareceCaminhoRede = pareceCompartilhamentoUnc ||
                    nomeLimpo.IndexOf("\\\\", StringComparison.Ordinal) >= 0 ||
                    nomeLimpo.IndexOf("//", StringComparison.Ordinal) >= 0;

                string mensagem =
                    "MachineName=" + Environment.MachineName +
                    "; UserName=" + Environment.UserName +
                    "; OSVersion=" + Environment.OSVersion +
                    "; Is64BitOS=" + Environment.Is64BitOperatingSystem +
                    "; Is64BitProcess=" + Environment.Is64BitProcess +
                    "; EnvironmentVersion=" + Environment.Version +
                    "; StartupPath=" + Application.StartupPath +
                    "; ExecutablePath=" + caminhoExecutavel +
                    "; ExecutableVersion=" + versaoExecutavel +
                    "; UserInteractive=" + Environment.UserInteractive +
                    "; ImpressoraEscolhida=" + impressora +
                    "; ImpressoraPadrao=" + impressoraPadrao +
                    "; QuantidadeImpressoras=" + impressorasInstaladas.Count +
                    "; ImpressorasInstaladas=" + string.Join(" | ", impressorasInstaladas) +
                    "; ApareceIgnorandoCaixa=" + apareceNaListaIgnorandoCaixa +
                    "; ApareceExata=" + apareceNaListaExata +
                    "; EspacosNasExtremidades=" + possuiEspacosNasExtremidades +
                    "; ComprimentoNome=" + (impressora ?? "").Length +
                    "; PareceUNC=" + pareceCompartilhamentoUnc +
                    "; PareceCaminhoRede=" + pareceCaminhoRede +
                    "; Codigo=" + (etiqueta != null ? etiqueta.Codigo : "") +
                    "; Etiqueta=" + (etiqueta != null ? etiqueta.NomeEtiqueta : "") +
                    "; Quantidade=" + quantidade;

                LogRemotoEtiquetas.Registrar("AmbienteImpressao", "INFO", mensagem, impressora, quantidade, etiqueta != null ? etiqueta.Codigo : "", etiqueta != null ? etiqueta.NomeEtiqueta : "", tentativaId);
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.Registrar("ErroAmbienteImpressao", "ERRO_LOCAL", ex.Message, impressora, quantidade, etiqueta != null ? etiqueta.Codigo : "", etiqueta != null ? etiqueta.NomeEtiqueta : "", tentativaId);
            }
        }

        private void RegistrarDetalhesWmiImpressora(string impressora, EtiquetaModel etiqueta, int quantidade, string tentativaId)
        {
            if (!glo.LogRemoto)
            {
                return;
            }

            try
            {
                string nomeProcurado = impressora ?? "";
                List<string> nomesWmi = new List<string>();
                List<string> correspondenciasExatas = new List<string>();
                List<string> correspondenciasIgnorandoCaixa = new List<string>();

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer"))
                using (ManagementObjectCollection resultados = searcher.Get())
                {
                    foreach (ManagementBaseObject objetoBase in resultados)
                    {
                        using (ManagementObject objeto = objetoBase as ManagementObject)
                        {
                            if (objeto == null)
                            {
                                continue;
                            }

                            string nomeWmi = ObterValorWmi(objeto, "Name");
                            nomesWmi.Add(nomeWmi);

                            if (string.Equals(nomeWmi, nomeProcurado, StringComparison.Ordinal))
                            {
                                correspondenciasExatas.Add(ObterDetalhesWmi(objeto));
                            }

                            if (string.Equals(nomeWmi, nomeProcurado, StringComparison.OrdinalIgnoreCase))
                            {
                                correspondenciasIgnorandoCaixa.Add(ObterDetalhesWmi(objeto));
                            }
                        }
                    }
                }

                if (correspondenciasIgnorandoCaixa.Count == 0)
                {
                    LogRemotoEtiquetas.Registrar(
                        "ImpressoraNaoEncontradaWmi",
                        "INFO",
                        "Nome procurado=" + nomeProcurado + "; QuantidadeWmi=" + nomesWmi.Count + "; NomesWmi=" + string.Join(" | ", nomesWmi),
                        impressora,
                        quantidade,
                        etiqueta != null ? etiqueta.Codigo : "",
                        etiqueta != null ? etiqueta.NomeEtiqueta : "",
                        tentativaId);
                    return;
                }

                string tipoCorrespondencia = correspondenciasExatas.Count > 0 ? "Exata" : "IgnorandoCaixa";
                string mensagem = "Correspondencia=" + tipoCorrespondencia +
                    "; QuantidadeCorrespondencias=" + correspondenciasIgnorandoCaixa.Count +
                    "; Detalhes=" + string.Join(" || ", correspondenciasIgnorandoCaixa);
                LogRemotoEtiquetas.Registrar(
                    "DetalhesWmiImpressora",
                    "INFO",
                    mensagem,
                    impressora,
                    quantidade,
                    etiqueta != null ? etiqueta.Codigo : "",
                    etiqueta != null ? etiqueta.NomeEtiqueta : "",
                    tentativaId);
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.Registrar(
                    "ErroDetalhesWmiImpressora",
                    "ERRO_LOCAL",
                    ex.Message,
                    impressora,
                    quantidade,
                    etiqueta != null ? etiqueta.Codigo : "",
                    etiqueta != null ? etiqueta.NomeEtiqueta : "",
                    tentativaId);
            }
        }

        private static string ObterValorWmi(ManagementBaseObject objeto, string propriedade)
        {
            try
            {
                object valor = objeto[propriedade];
                return ConverterValorWmiParaTexto(valor);
            }
            catch
            {
                return "(nulo)";
            }
        }

        private static string ConverterValorWmiParaTexto(object valor)
        {
            if (valor == null || Convert.IsDBNull(valor))
            {
                return "(nulo)";
            }

            Array array = valor as Array;
            if (array != null)
            {
                List<string> itens = new List<string>();
                foreach (object item in array)
                {
                    itens.Add(ConverterValorWmiParaTexto(item));
                }

                return string.Join(" | ", itens);
            }

            IFormattable formatavel = valor as IFormattable;
            return formatavel != null
                ? formatavel.ToString(null, CultureInfo.InvariantCulture)
                : Convert.ToString(valor, CultureInfo.InvariantCulture);
        }

        private static string ObterDetalhesWmi(ManagementBaseObject objeto)
        {
            string[] propriedades =
            {
                "Name", "Caption", "DeviceID", "DriverName", "PortName", "ServerName", "ShareName",
                "SystemName", "Location", "Comment", "Default", "Local", "Network", "Shared", "WorkOffline",
                "Direct", "EnableBIDI", "Published", "PrinterStatus", "ExtendedPrinterStatus", "DetectedErrorState",
                "ExtendedDetectedErrorState", "Status", "StatusInfo", "Availability", "JobCountSinceLastReset",
                "HorizontalResolution", "VerticalResolution"
            };

            List<string> valores = new List<string>();
            foreach (string propriedade in propriedades)
            {
                valores.Add(propriedade + "=" + ObterValorWmi(objeto, propriedade));
            }

            return string.Join("; ", valores);
        }

        private void RegistrarEstadoServicoSpooler(
            string impressora,
            EtiquetaModel etiqueta,
            int quantidade,
            string tentativaId)
        {
            if (!glo.LogRemoto)
            {
                return;
            }

            string codigo = etiqueta != null ? etiqueta.Codigo : "";
            string nomeEtiqueta = etiqueta != null ? etiqueta.NomeEtiqueta : "";

            try
            {
                bool encontrado = false;
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_Service WHERE Name = 'Spooler'"))
                using (ManagementObjectCollection resultados = searcher.Get())
                {
                    foreach (ManagementBaseObject objetoBase in resultados)
                    {
                        using (ManagementObject objeto = objetoBase as ManagementObject)
                        {
                            if (objeto == null)
                            {
                                continue;
                            }

                            encontrado = true;
                            string state = ObterValorWmi(objeto, "State");
                            string started = ObterValorWmi(objeto, "Started");
                            bool alertaEstado = !string.Equals(state, "Running", StringComparison.OrdinalIgnoreCase) ||
                                !string.Equals(started, "True", StringComparison.OrdinalIgnoreCase);
                            string mensagem = ObterDetalhesServicoWmi(objeto) + "; AlertaEstado=" + alertaEstado;

                            LogRemotoEtiquetas.Registrar(
                                "EstadoServicoSpooler",
                                "INFO",
                                mensagem,
                                impressora,
                                quantidade,
                                codigo,
                                nomeEtiqueta,
                                tentativaId);
                        }
                    }
                }

                if (!encontrado)
                {
                    LogRemotoEtiquetas.Registrar(
                        "ServicoSpoolerNaoEncontrado",
                        "INFO",
                        "Serviço Windows Spooler não foi localizado pela consulta Win32_Service.",
                        impressora,
                        quantidade,
                        codigo,
                        nomeEtiqueta,
                        tentativaId);
                }
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.Registrar(
                    "ErroEstadoServicoSpooler",
                    "ERRO_LOCAL",
                    ex.Message,
                    impressora,
                    quantidade,
                    codigo,
                    nomeEtiqueta,
                    tentativaId);
            }
        }

        private static string ObterDetalhesServicoWmi(ManagementBaseObject objeto)
        {
            string[] propriedades =
            {
                "Name", "DisplayName", "State", "Status", "Started", "StartMode", "StartName",
                "ProcessId", "ExitCode", "ServiceSpecificExitCode", "AcceptPause", "AcceptStop",
                "DesktopInteract", "PathName", "Description"
            };

            List<string> valores = new List<string>();
            foreach (string propriedade in propriedades)
            {
                valores.Add(propriedade + "=" + ObterValorWmi(objeto, propriedade));
            }

            return string.Join("; ", valores);
        }

        private void RegistrarDriverEPortaImpressora(
            string impressora,
            EtiquetaModel etiqueta,
            int quantidade,
            string tentativaId)
        {
            if (!glo.LogRemoto)
            {
                return;
            }

            string codigo = etiqueta != null ? etiqueta.Codigo : "";
            string nomeEtiqueta = etiqueta != null ? etiqueta.NomeEtiqueta : "";

            try
            {
                List<string> detalhesImpressora = new List<string>();
                List<string> detalhesDrivers = new List<string>();
                List<string> detalhesPortas = new List<string>();
                List<string> nomesDrivers = new List<string>();
                List<string> nomesPortas = new List<string>();
                bool impressoraEncontrada = false;
                bool driverEncontrado = false;
                bool portaTcpIpEncontrada = false;
                string driverName = "";
                string portName = "";
                string tipoPorta = "Desconhecida/Outra";
                bool consultarPortaTcpIp = false;

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer"))
                using (ManagementObjectCollection resultados = searcher.Get())
                {
                    foreach (ManagementBaseObject objetoBase in resultados)
                    {
                        using (ManagementObject objeto = objetoBase as ManagementObject)
                        {
                            if (objeto == null || !string.Equals(ObterValorWmi(objeto, "Name"), impressora, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            impressoraEncontrada = true;
                            string driverAtual = ObterValorWmi(objeto, "DriverName");
                            string portaAtual = ObterValorWmi(objeto, "PortName");
                            driverName = string.IsNullOrWhiteSpace(driverName) || driverName == "(nulo)" ? driverAtual : driverName;
                            portName = string.IsNullOrWhiteSpace(portName) || portName == "(nulo)" ? portaAtual : portName;
                            tipoPorta = ClassificarTipoPorta(portaAtual);
                            consultarPortaTcpIp = consultarPortaTcpIp || EhPortaTcpIp(tipoPorta);
                            detalhesImpressora.Add("DriverName=" + driverAtual + "; PortName=" + portaAtual + "; ServerName=" + ObterValorWmi(objeto, "ServerName") + "; Local=" + ObterValorWmi(objeto, "Local") + "; Network=" + ObterValorWmi(objeto, "Network") + "; Shared=" + ObterValorWmi(objeto, "Shared"));
                        }
                    }
                }

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PrinterDriver"))
                using (ManagementObjectCollection resultados = searcher.Get())
                {
                    foreach (ManagementBaseObject objetoBase in resultados)
                    {
                        using (ManagementObject objeto = objetoBase as ManagementObject)
                        {
                            string nomeDriverWmi = objeto == null ? "" : ObterValorWmi(objeto, "Name");
                            if (!string.IsNullOrWhiteSpace(nomeDriverWmi) && nomeDriverWmi != "(nulo)")
                            {
                                nomesDrivers.Add(nomeDriverWmi);
                            }

                            if (objeto != null && EhDriverCorrespondente(nomeDriverWmi, driverName))
                            {
                                driverEncontrado = true;
                                detalhesDrivers.Add(ObterDetalhesDriverWmi(objeto));
                            }
                        }
                    }
                }

                if (impressoraEncontrada && !string.IsNullOrWhiteSpace(driverName) && driverName != "(nulo)" && !driverEncontrado)
                {
                    LogRemotoEtiquetas.Registrar(
                        "DriverImpressoraNaoEncontradoWmi",
                        "INFO",
                        "DriverName procurado=" + driverName + "; QuantidadeDriversWmi=" + nomesDrivers.Count + "; NomesDriversWmi=" + string.Join(" | ", nomesDrivers),
                        impressora,
                        quantidade,
                        codigo,
                        nomeEtiqueta,
                        tentativaId);
                }

                if (impressoraEncontrada && EhPortaTcpIp(tipoPorta))
                {
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_TCPIPPrinterPort"))
                    using (ManagementObjectCollection resultados = searcher.Get())
                    {
                        foreach (ManagementBaseObject objetoBase in resultados)
                        {
                            using (ManagementObject objeto = objetoBase as ManagementObject)
                            {
                                string nomePortaWmi = objeto == null ? "" : ObterValorWmi(objeto, "Name");
                                if (!string.IsNullOrWhiteSpace(nomePortaWmi) && nomePortaWmi != "(nulo)")
                                {
                                    nomesPortas.Add(nomePortaWmi);
                                }

                                if (objeto != null && string.Equals(nomePortaWmi, portName, StringComparison.OrdinalIgnoreCase))
                                {
                                    portaTcpIpEncontrada = true;
                                    detalhesPortas.Add(ObterDetalhesPortaTcpIpWmi(objeto));
                                }
                            }
                        }
                    }
                }

                string mensagem =
                    "ImpressoraEncontrada=" + impressoraEncontrada +
                    "; Impressora=" + impressora +
                    "; DetalhesImpressora=" + string.Join(" || ", detalhesImpressora) +
                    "; DriverName=" + driverName +
                    "; DriverEncontrado=" + driverEncontrado +
                    "; DriversCorrespondentes=" + detalhesDrivers.Count +
                    "; DriverWmiDetalhes=" + string.Join(" || ", detalhesDrivers) +
                    "; DriversWmiNomes=" + string.Join(" | ", nomesDrivers) +
                    "; PortName=" + portName +
                    "; TipoPortaProvavel=" + tipoPorta +
                    "; PortaTcpIpConsultada=" + consultarPortaTcpIp +
                    "; PortaTcpIpEncontrada=" + (consultarPortaTcpIp && portaTcpIpEncontrada) +
                    "; PortasTcpIpNomes=" + string.Join(" | ", nomesPortas) +
                    "; PortaTcpIpDetalhes=" + string.Join(" || ", detalhesPortas);

                LogRemotoEtiquetas.Registrar(
                    "DriverEPortaImpressora",
                    "INFO",
                    mensagem,
                    impressora,
                    quantidade,
                    codigo,
                    nomeEtiqueta,
                    tentativaId);
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.RegistrarErro(
                    "ErroDriverEPortaImpressora",
                    ex,
                    impressora,
                    quantidade,
                    codigo,
                    nomeEtiqueta,
                    tentativaId);
            }
        }

        private static bool EhDriverCorrespondente(string nomeDriverWmi, string driverName)
        {
            if (string.IsNullOrWhiteSpace(nomeDriverWmi) || nomeDriverWmi == "(nulo)" || string.IsNullOrWhiteSpace(driverName) || driverName == "(nulo)")
            {
                return false;
            }

            return string.Equals(nomeDriverWmi, driverName, StringComparison.Ordinal) ||
                string.Equals(nomeDriverWmi, driverName, StringComparison.OrdinalIgnoreCase) ||
                nomeDriverWmi.StartsWith(driverName + ",", StringComparison.OrdinalIgnoreCase);
        }

        private static string ClassificarTipoPorta(string portName)
        {
            string valor = (portName ?? "").Trim();
            if (valor.StartsWith("USB", StringComparison.OrdinalIgnoreCase)) return "USB";
            if (valor.StartsWith("LPT", StringComparison.OrdinalIgnoreCase)) return "Paralela";
            if (valor.StartsWith("COM", StringComparison.OrdinalIgnoreCase)) return "Serial";
            if (valor.StartsWith("IP_", StringComparison.OrdinalIgnoreCase) || ContemEnderecoIp(valor)) return "TCP/IP";
            if (valor.StartsWith("\\\\", StringComparison.Ordinal)) return "Compartilhamento UNC";
            return "Desconhecida/Outra";
        }

        private static bool EhPortaTcpIp(string tipoPorta)
        {
            return string.Equals(tipoPorta, "TCP/IP", StringComparison.OrdinalIgnoreCase);
        }

        private static bool ContemEnderecoIp(string valor)
        {
            char[] separadores = { ':', '[', ']', ';', ',', ' ', '\\', '/' };
            foreach (string parte in (valor ?? "").Split(separadores, StringSplitOptions.RemoveEmptyEntries))
            {
                System.Net.IPAddress endereco;
                if (System.Net.IPAddress.TryParse(parte, out endereco))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ObterDetalhesDriverWmi(ManagementBaseObject objeto)
        {
            string[] propriedades =
            {
                "Name", "DriverPath", "InfName", "ConfigFile", "DataFile", "HelpFile", "MonitorName",
                "OEMUrl", "SupportedPlatform", "Version", "FilePath", "DefaultDataType", "DependentFiles"
            };

            List<string> valores = new List<string>();
            foreach (string propriedade in propriedades)
            {
                valores.Add(propriedade + "=" + ObterValorWmi(objeto, propriedade));
            }

            return string.Join("; ", valores);
        }

        private static string ObterDetalhesPortaTcpIpWmi(ManagementBaseObject objeto)
        {
            string[] propriedades =
            {
                "Name", "HostAddress", "PortNumber", "Protocol", "Queue", "SNMPEnabled",
                "SNMPDevIndex", "ByteCount", "DoubleSpool", "Enabled"
            };

            List<string> valores = new List<string>();
            foreach (string propriedade in propriedades)
            {
                valores.Add(propriedade + "=" + ObterValorWmi(objeto, propriedade));
            }

            string comunidade = ObterValorWmi(objeto, "SNMPCommunity");
            valores.Add("SNMPCommunityConfigurada=" + (!string.IsNullOrWhiteSpace(comunidade) && comunidade != "(nulo)"));
            return string.Join("; ", valores);
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void CamposPreview_TextChanged(object sender, EventArgs e)
        {
            pnlPreview.Invalidate();
        }

        private void gridEtiquetas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= gridEtiquetas.Rows.Count)
                {
                    return;
                }

                DataGridViewRow row = gridEtiquetas.Rows[e.RowIndex];
                if (row.IsNewRow || row.Cells["Id"].Value == null)
                {
                    return;
                }

                etiquetaSelecionadaId = Convert.ToString(row.Cells["Id"].Value);
                nomeEtiquetaSelecionada = Convert.ToString(row.Cells["Nome"].Value);
                txtNomeEmpresa.Text = Convert.ToString(row.Cells["NomeEmpresa"].Value);
                txtTelefone.Text = Convert.ToString(row.Cells["Telefone"].Value);
                txtTeleEntrega.Text = Convert.ToString(row.Cells["TeleEntrega"].Value);
                txtLocal.Text = NormalizarLocalCampo(Convert.ToString(row.Cells["Local"].Value));
                txtCodigo.Text = NormalizarCodigoCampo(Convert.ToString(row.Cells["Codigo"].Value));
                txtDescricao.Text = Convert.ToString(row.Cells["Descricao"].Value);
                txtPreco.Text = NormalizarPrecoCampo(Convert.ToString(row.Cells["Preco"].Value));
                txtObservacao.Text = Convert.ToString(row.Cells["Observacao"].Value);
                EtiquetaModel etiquetaSelecionada = etiquetas.FirstOrDefault(item => string.Equals(item.Id, etiquetaSelecionadaId, StringComparison.OrdinalIgnoreCase));
                InicializarFontesEdicao(etiquetaSelecionada ?? new EtiquetaModel());
                linhaSelecionada = etiquetaSelecionada != null && etiquetaSelecionada.ModoTextoLivre
                    ? "Observacao"
                    : "Codigo";
                CarregarControlesFormatacao();
                pnlPreview.Invalidate();
            }
            catch (Exception ex)
            {
                glo.Loga("Erro gridEtiquetas_CellClick: " + ex.Message);
            }
        }

        private RectangleF ObterAreaTextoLivre(RectangleF areaBase)
        {
            float margemHorizontal = areaBase.Width * 0.04f;
            float margemVertical = areaBase.Height * 0.04f;
            return new RectangleF(
                areaBase.X + margemHorizontal,
                areaBase.Y + margemVertical,
                areaBase.Width - (margemHorizontal * 2),
                areaBase.Height - (margemVertical * 2));
        }

        private bool EstaEmModoTextoLivre()
        {
            if (string.IsNullOrWhiteSpace(txtObservacao.Text) || cmbLinhaFormatacao.SelectedItem == null)
            {
                return false;
            }

            string itemSelecionado = cmbLinhaFormatacao.SelectedItem.ToString();
            return string.Equals(itemSelecionado, "Texto livre", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ObterChaveLinhaFormatacao(itemSelecionado), "Observacao", StringComparison.OrdinalIgnoreCase);
        }

        private void pnlPreview_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                areasPreview.Clear();
                Rectangle area = pnlPreview.ClientRectangle;
                e.Graphics.Clear(Color.White);

                int margem = 12;
                int larguraDisponivel = Math.Max(1, area.Width - (margem * 2));
                int alturaDisponivel = Math.Max(1, area.Height - (margem * 2));
                int larguraEtiqueta = larguraDisponivel;
                int alturaEtiqueta = (int)(larguraEtiqueta / 2.0);

                if (alturaEtiqueta > alturaDisponivel)
                {
                    alturaEtiqueta = alturaDisponivel;
                    larguraEtiqueta = (int)(alturaEtiqueta * 2.0);
                }

                int x = (area.Width - larguraEtiqueta) / 2;
                int y = (area.Height - alturaEtiqueta) / 2;
                Rectangle etiquetaRect = new Rectangle(x, y, larguraEtiqueta, alturaEtiqueta);

                using (Pen pen = new Pen(Color.Black, 1))
                {
                    e.Graphics.DrawRectangle(pen, etiquetaRect);
                }

                if (EstaEmModoTextoLivre())
                {
                    RectangleF areaTextoLivre = ObterAreaTextoLivre(etiquetaRect);

                    areasPreview["Observacao"] = areaTextoLivre;
                    using (StringFormat textoLivreFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Near,
                        Trimming = StringTrimming.None
                    })
                    {
                        DesenharLinhaPreview(e.Graphics, "Observacao", txtObservacao.Text, areaTextoLivre, textoLivreFormat, Color.Black, false);
                    }

                    return;
                }

                float margemHorizontal = 4f;
                float larguraTexto = etiquetaRect.Width - (margemHorizontal * 2);
                float altura = etiquetaRect.Height;
                RectangleF linhaNomeEmpresa = new RectangleF(etiquetaRect.X + margemHorizontal, etiquetaRect.Y + (altura * 0.03f), larguraTexto, altura * 0.13f);
                RectangleF linhaDescricao = new RectangleF(etiquetaRect.X + margemHorizontal, etiquetaRect.Y + (altura * 0.18f), larguraTexto, altura * 0.15f);
                RectangleF linhaPreco = new RectangleF(etiquetaRect.X + margemHorizontal, etiquetaRect.Y + (altura * 0.35f), larguraTexto, altura * 0.18f);
                RectangleF linhaObservacao = new RectangleF(etiquetaRect.X + margemHorizontal, etiquetaRect.Y + (altura * 0.55f), larguraTexto, altura * 0.10f);
                float larguraLocal = larguraTexto * 0.45f;
                float margemDireitaCodigo = Math.Max(1f, larguraTexto * 0.02f);
                float espacoEntreLinhas = 2f;
                float xLinha = etiquetaRect.X + margemHorizontal;
                float xCodigo = xLinha + larguraLocal + espacoEntreLinhas;
                float larguraCodigo = larguraTexto - larguraLocal - espacoEntreLinhas - margemDireitaCodigo;
                RectangleF linhaLocal = new RectangleF(xLinha, etiquetaRect.Y + (altura * 0.59f), larguraLocal, altura * 0.14f);
                RectangleF linhaCodigo = new RectangleF(xCodigo, etiquetaRect.Y + (altura * 0.59f), larguraCodigo, altura * 0.14f);
                RectangleF linhaTelefone = new RectangleF(etiquetaRect.X + margemHorizontal, etiquetaRect.Y + (altura * 0.74f), larguraTexto, altura * 0.12f);
                RectangleF linhaTeleEntrega = new RectangleF(etiquetaRect.X + margemHorizontal, etiquetaRect.Y + (altura * 0.87f), larguraTexto, altura * 0.12f);

                areasPreview["NomeEmpresa"] = linhaNomeEmpresa;
                areasPreview["Telefone"] = linhaTelefone;
                areasPreview["Codigo"] = linhaCodigo;
                areasPreview["Descricao"] = linhaDescricao;
                areasPreview["Preco"] = linhaPreco;
                areasPreview["Observacao"] = linhaObservacao;
                areasPreview["Local"] = linhaLocal;
                areasPreview["TeleEntrega"] = linhaTeleEntrega;

                using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (StringFormat observacaoFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near, Trimming = StringTrimming.None })
                using (StringFormat left = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
                using (StringFormat right = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.None, FormatFlags = StringFormatFlags.NoWrap })
                {
                    DesenharLinhaPreview(e.Graphics, "NomeEmpresa", txtNomeEmpresa.Text.Trim(), linhaNomeEmpresa, center, Color.Black, true);
                    DesenharLinhaPreview(e.Graphics, "Descricao", txtDescricao.Text.Trim(), linhaDescricao, center, Color.Black, false);
                    DesenharLinhaPreview(e.Graphics, "Preco", FormatarPrecoEtiqueta(txtPreco.Text), linhaPreco, center, Color.Black, true);
                    DesenharLinhaPreview(e.Graphics, "Observacao", txtObservacao.Text, linhaObservacao, observacaoFormat, Color.Black, false);
                    DesenharLinhaPreview(e.Graphics, "Local", FormatarLocalEtiqueta(txtLocal.Text), linhaLocal, left, Color.Black, true);
                    DesenharLinhaPreview(e.Graphics, "Codigo", FormatarCodigoEtiqueta(txtCodigo.Text), linhaCodigo, right, Color.Black, true);
                    DesenharLinhaPreview(e.Graphics, "Telefone", txtTelefone.Text.Trim(), linhaTelefone, center, Color.Black, false);
                    DesenharLinhaPreview(e.Graphics, "TeleEntrega", txtTeleEntrega.Text.Trim(), linhaTeleEntrega, center, Color.Black, true);
                }
            }
            catch (Exception ex)
            {
                glo.Loga("Erro pnlPreview_Paint: " + ex.Message);
            }
        }

        private void DesenharLinhaPreview(Graphics graphics, string linha, string texto, RectangleF areaLinha, StringFormat center, Color cor, bool negritoPadrao)
        {
            EtiquetaFonteConfig fonteConfig = ObterFonteLinha(linha);
            FontStyle estilo = fonteConfig.Negrito ? FontStyle.Bold : FontStyle.Regular;
            if (negritoPadrao && !fonteConfig.Negrito)
            {
                estilo = FontStyle.Regular;
            }

            using (Font fonte = CriarFonteAjustada(
                graphics,
                texto,
                areaLinha,
                string.IsNullOrWhiteSpace(fonteConfig.NomeFonte) ? "Arial" : fonteConfig.NomeFonte,
                fonteConfig.Tamanho > 0 ? fonteConfig.Tamanho : 8f,
                estilo == FontStyle.Bold,
                5f,
                !EhLinhaComTamanhoPrioritario(linha) && !string.Equals(linha, "Observacao", StringComparison.OrdinalIgnoreCase)))
            {
                if (string.Equals(linhaSelecionada, linha, StringComparison.OrdinalIgnoreCase))
                {
                    using (Brush brushSel = new SolidBrush(Color.FromArgb(35, Color.SteelBlue)))
                    using (Pen penSel = new Pen(Color.SteelBlue, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                    {
                        graphics.FillRectangle(brushSel, areaLinha.X - 1, areaLinha.Y - 1, areaLinha.Width + 2, areaLinha.Height + 2);
                        graphics.DrawRectangle(penSel, Rectangle.Round(new RectangleF(areaLinha.X - 1, areaLinha.Y - 1, areaLinha.Width + 2, areaLinha.Height + 2)));
                    }
                }

                using (Brush brushTexto = new SolidBrush(cor))
                {
                    graphics.DrawString(texto, fonte, brushTexto, areaLinha, center);
                }
            }
        }

        private EtiquetaFonteConfig ObterFonteLinha(string linha)
        {
            if (fontesEdicao == null)
            {
                InicializarFontesEdicao(new EtiquetaModel());
            }

            EtiquetaFonteConfig fonte;
            if (!fontesEdicao.TryGetValue(linha, out fonte) || fonte == null)
            {
                Dictionary<string, EtiquetaFonteConfig> padroes = new EtiquetaModel().ObterFontesComPadrao();
                if (padroes.TryGetValue(linha, out fonte) && fonte != null)
                {
                    return fonte;
                }

                return new EtiquetaFonteConfig { NomeFonte = "Arial", Tamanho = 8f, Negrito = false };
            }

            return fonte;
        }

        private static bool EhLinhaComTamanhoPrioritario(string linha)
        {
            return string.Equals(linha, "Codigo", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(linha, "Local", StringComparison.OrdinalIgnoreCase);
        }

        private void ConfigurarGrid()
        {
            try
            {
                gridEtiquetas.Columns.Clear();
                gridEtiquetas.AutoGenerateColumns = false;
                gridEtiquetas.ReadOnly = true;
                gridEtiquetas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                gridEtiquetas.MultiSelect = false;
                gridEtiquetas.AllowUserToAddRows = false;
                gridEtiquetas.AllowUserToDeleteRows = false;
                gridEtiquetas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Numero", HeaderText = "Nº", ReadOnly = true, FillWeight = 45 });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "Id", ReadOnly = true, Visible = false });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nome", HeaderText = "Nome", ReadOnly = true, FillWeight = 120 });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "NomeEmpresa", HeaderText = "Nome da empresa", ReadOnly = true, Visible = false });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Telefone", HeaderText = "Telefone", ReadOnly = true, Visible = false });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Codigo", HeaderText = "Código", ReadOnly = true, FillWeight = 85 });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Descricao", HeaderText = "Descrição", ReadOnly = true, FillWeight = 190 });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Preco", HeaderText = "Preço", ReadOnly = true, FillWeight = 75 });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Observacao", HeaderText = "Observação", ReadOnly = true, FillWeight = 160 });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "TeleEntrega", HeaderText = "Tele-entrega", ReadOnly = true, Visible = false });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Local", HeaderText = "Local", ReadOnly = true, Visible = false });
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ConfigurarGrid: " + ex.Message);
            }
        }

        private void CarregarGrid(IEnumerable<EtiquetaModel> lista)
        {
            try
            {
                gridEtiquetas.Rows.Clear();
                int numero = 1;
                foreach (EtiquetaModel etiqueta in lista ?? Enumerable.Empty<EtiquetaModel>())
                {
                    gridEtiquetas.Rows.Add(
                        numero.ToString("000"),
                        etiqueta.Id,
                        ObterNomeEtiquetaSugerido(etiqueta),
                        etiqueta.NomeEmpresa,
                        etiqueta.Telefone,
                        FormatarCodigoEtiqueta(etiqueta.Codigo),
                        etiqueta.Descricao,
                        FormatarPrecoEtiqueta(etiqueta.Preco),
                        etiqueta.Observacao,
                        etiqueta.TeleEntrega,
                        etiqueta.Local);
                    numero++;
                }
            }
            catch (Exception ex)
            {
                glo.Loga("Erro CarregarGrid: " + ex.Message);
            }
        }

        private void CarregarImpressoras()
        {
            try
            {
                cmbImpressora.Items.Clear();
                string impressoraWaytec = null;

                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    cmbImpressora.Items.Add(printer);
                    if (impressoraWaytec == null && printer.IndexOf("WAYTEC", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        impressoraWaytec = printer;
                    }
                }

                if (impressoraWaytec != null)
                {
                    cmbImpressora.SelectedItem = impressoraWaytec;
                }
                else if (cmbImpressora.Items.Count > 0)
                {
                    cmbImpressora.SelectedIndex = 0;
                }

                LogRemotoEtiquetas.Registrar("CarregarImpressoras", "OK", "Impressoras instaladas: " + cmbImpressora.Items.Count);
                for (int i = 0; i < cmbImpressora.Items.Count; i++)
                {
                    LogRemotoEtiquetas.Registrar("ImpressoraInstalada", "OK", "Impressora instalada: " + cmbImpressora.Items[i]);
                }

                LogRemotoEtiquetas.Registrar(
                    "ImpressoraSelecionada",
                    "OK",
                    "Impressora selecionada: " + (cmbImpressora.SelectedItem ?? ""),
                    impressora: cmbImpressora.SelectedItem != null ? cmbImpressora.SelectedItem.ToString() : "");
            }
            catch (Exception ex)
            {
                glo.Loga("Erro CarregarImpressoras: " + ex.Message);
                LogRemotoEtiquetas.RegistrarErro("CarregarImpressoras", ex);
            }
        }

        private EtiquetaModel ObterEtiquetaDaTela()
        {
            return new EtiquetaModel
            {
                Id = etiquetaSelecionadaId,
                ModoTextoLivre = EstaEmModoTextoLivre(),
                NomeEtiqueta = nomeEtiquetaSelecionada,
                NomeEmpresa = txtNomeEmpresa.Text.Trim(),
                Telefone = txtTelefone.Text.Trim(),
                TeleEntrega = txtTeleEntrega.Text.Trim(),
                Local = NormalizarLocalCampo(txtLocal.Text),
                Codigo = NormalizarCodigoCampo(txtCodigo.Text),
                Descricao = txtDescricao.Text.Trim(),
                Preco = NormalizarPrecoCampo(txtPreco.Text),
                Observacao = txtObservacao.Text,
                Fontes = CopiarFontesEdicao()
            };
        }

        private string NormalizarCodigoCampo(string codigo)
        {
            string valor = (codigo ?? string.Empty).Trim();
            if (valor.StartsWith("COD:", StringComparison.OrdinalIgnoreCase))
            {
                valor = valor.Substring(4);
            }

            return valor.Trim();
        }

        private string NormalizarPrecoCampo(string preco)
        {
            string valor = (preco ?? string.Empty).Trim();
            if (valor.StartsWith("R$", StringComparison.OrdinalIgnoreCase))
            {
                valor = valor.Substring(2);
            }

            return valor.Trim();
        }

        private string FormatarCodigoEtiqueta(string codigo)
        {
            string valor = NormalizarCodigoCampo(codigo);
            return string.IsNullOrWhiteSpace(valor) ? string.Empty : "COD: " + valor;
        }

        private string FormatarPrecoEtiqueta(string preco)
        {
            string valor = NormalizarPrecoCampo(preco);
            return string.IsNullOrWhiteSpace(valor) ? string.Empty : "R$ " + valor;
        }

        private string NormalizarLocalCampo(string local)
        {
            string valor = (local ?? string.Empty).Trim();
            if (valor.StartsWith("LOCAL:", StringComparison.OrdinalIgnoreCase))
            {
                valor = valor.Substring(6);
            }

            return valor.Trim();
        }

        private string FormatarLocalEtiqueta(string local)
        {
            string valor = NormalizarLocalCampo(local);
            return string.IsNullOrWhiteSpace(valor) ? string.Empty : "LC: " + valor;
        }

        private string SolicitarNomeEtiqueta(EtiquetaModel etiqueta)
        {
            using (FormNomeEtiqueta form = new FormNomeEtiqueta())
            {
                form.NomeEtiqueta = ObterNomeEtiquetaSugerido(etiqueta);
                return form.ShowDialog(this) == DialogResult.OK ? form.NomeEtiqueta : null;
            }
        }

        private string ObterNomeEtiquetaSugerido(EtiquetaModel etiqueta)
        {
            if (etiqueta != null)
            {
                if (!string.IsNullOrWhiteSpace(etiqueta.NomeEtiqueta))
                {
                    return etiqueta.NomeEtiqueta.Trim();
                }

                if (!string.IsNullOrWhiteSpace(etiqueta.Descricao))
                {
                    return etiqueta.Descricao.Trim();
                }

                if (!string.IsNullOrWhiteSpace(etiqueta.Codigo))
                {
                    return NormalizarCodigoCampo(etiqueta.Codigo);
                }
            }

            return "Etiqueta";
        }

        private string ObterChaveLinhaFormatacao(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return "Codigo";
            }

            string chave;
            if (mapaLinhaFormatacao.TryGetValue(texto.Trim(), out chave))
            {
                return chave;
            }

            return texto.Trim();
        }

        private string ObterTextoLinhaFormatacao(string chave)
        {
            if (string.IsNullOrWhiteSpace(chave))
            {
                return "Código";
            }

            if (string.Equals(chave, "Observacao", StringComparison.OrdinalIgnoreCase))
            {
                return "Texto livre";
            }

            foreach (KeyValuePair<string, string> item in mapaLinhaFormatacao)
            {
                if (string.Equals(item.Value, chave, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(item.Key, item.Value, StringComparison.OrdinalIgnoreCase))
                {
                    return item.Key;
                }
            }

            return chave;
        }

        private void txtObservacao_Enter(object sender, EventArgs e)
        {
            SelecionarLinhaFormatacao("Texto livre");
        }

        private void txtObservacao_Click(object sender, EventArgs e)
        {
            SelecionarLinhaFormatacao("Texto livre");
        }

        private void LimparCampos()
        {
            try
            {
                etiquetaSelecionadaId = "";
                nomeEtiquetaSelecionada = "";
                InicializarFontesEdicao(new EtiquetaModel());
                linhaSelecionada = "Codigo";
                txtNomeEmpresa.Clear();
                txtTelefone.Clear();
                txtTeleEntrega.Clear();
                txtLocal.Clear();
                txtCodigo.Clear();
                txtDescricao.Clear();
                txtPreco.Clear();
                txtObservacao.Clear();
                txtBuscar.Clear();
                numQuantidade.Value = 1;
                gridEtiquetas.ClearSelection();
                CarregarControlesFormatacao();
                pnlPreview.Invalidate();
            }
            catch (Exception ex)
            {
                glo.Loga("Erro LimparCampos: " + ex.Message);
            }
        }

        private void CarregarListaFontes()
        {
            try
            {
                cmbFonte.Items.Clear();
                foreach (FontFamily familia in FontFamily.Families)
                {
                    cmbFonte.Items.Add(familia.Name);
                }
            }
            catch (Exception ex)
            {
                glo.Loga("Erro CarregarListaFontes: " + ex.Message);
            }
        }

        private void InicializarFontesEdicao(EtiquetaModel etiqueta)
        {
            EtiquetaModel baseEtiqueta = etiqueta ?? new EtiquetaModel();
            fontesEdicao = baseEtiqueta.ObterFontesComPadrao()
                .ToDictionary(
                    item => item.Key,
                    item => new EtiquetaFonteConfig
                    {
                        NomeFonte = item.Value != null ? item.Value.NomeFonte : null,
                        Tamanho = item.Value != null ? item.Value.Tamanho : 0f,
                        Negrito = item.Value != null && item.Value.Negrito
                    });
        }

        private void CarregarControlesFormatacao()
        {
            try
            {
                carregandoFormatacao = true;

                if (fontesEdicao == null)
                {
                    InicializarFontesEdicao(new EtiquetaModel());
                }

                string chave = string.IsNullOrWhiteSpace(linhaSelecionada) ? "Codigo" : linhaSelecionada;
                string textoLinha = ObterTextoLinhaFormatacao(chave);
                if (cmbLinhaFormatacao.Items.Contains(textoLinha))
                {
                    cmbLinhaFormatacao.SelectedItem = textoLinha;
                }

                EtiquetaFonteConfig fonte = null;
                if (!fontesEdicao.TryGetValue(chave, out fonte) || fonte == null)
                {
                    fonte = new EtiquetaFonteConfig
                    {
                        NomeFonte = "Arial",
                        Tamanho = 8f,
                        Negrito = false
                    };
                }

                if (!string.IsNullOrWhiteSpace(fonte.NomeFonte) && cmbFonte.Items.Contains(fonte.NomeFonte))
                {
                    cmbFonte.SelectedItem = fonte.NomeFonte;
                }
                else if (cmbFonte.Items.Count > 0)
                {
                    cmbFonte.SelectedIndex = 0;
                }

                decimal tamanho = (decimal)fonte.Tamanho;
                if (tamanho < numTamanhoFonte.Minimum)
                {
                    tamanho = numTamanhoFonte.Minimum;
                }
                else if (tamanho > numTamanhoFonte.Maximum)
                {
                    tamanho = numTamanhoFonte.Maximum;
                }

                numTamanhoFonte.Value = tamanho;
                chkNegrito.Checked = fonte.Negrito;
            }
            catch (Exception ex)
            {
                glo.Loga("Erro CarregarControlesFormatacao: " + ex.Message);
            }
            finally
            {
                carregandoFormatacao = false;
            }
        }

        private void AplicarFormatacaoSelecionada()
        {
            if (carregandoFormatacao)
            {
                return;
            }

            if (fontesEdicao == null)
            {
                InicializarFontesEdicao(new EtiquetaModel());
            }

            string chave = string.IsNullOrWhiteSpace(linhaSelecionada) ? "Codigo" : linhaSelecionada;
            if (!fontesEdicao.ContainsKey(chave))
            {
                fontesEdicao[chave] = new EtiquetaFonteConfig();
            }

            EtiquetaFonteConfig fonte = fontesEdicao[chave];
            fonte.NomeFonte = cmbFonte.SelectedItem != null ? cmbFonte.SelectedItem.ToString() : "Arial";
            fonte.Tamanho = (float)numTamanhoFonte.Value;
            fonte.Negrito = chkNegrito.Checked;
            pnlPreview.Invalidate();
        }

        private Dictionary<string, EtiquetaFonteConfig> CopiarFontesEdicao()
        {
            if (fontesEdicao == null)
            {
                InicializarFontesEdicao(new EtiquetaModel());
            }

            return fontesEdicao.ToDictionary(
                item => item.Key,
                item => new EtiquetaFonteConfig
                {
                    NomeFonte = item.Value != null ? item.Value.NomeFonte : null,
                    Tamanho = item.Value != null ? item.Value.Tamanho : 0f,
                    Negrito = item.Value != null && item.Value.Negrito
                });
        }

        private void cmbLinhaFormatacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (carregandoFormatacao)
            {
                return;
            }

            if (cmbLinhaFormatacao.SelectedItem != null)
            {
                SelecionarLinhaFormatacao(ObterChaveLinhaFormatacao(cmbLinhaFormatacao.SelectedItem.ToString()));
            }
        }

        private void cmbFonte_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFormatacaoSelecionada();
        }

        private void numTamanhoFonte_ValueChanged(object sender, EventArgs e)
        {
            AplicarFormatacaoSelecionada();
        }

        private void chkNegrito_CheckedChanged(object sender, EventArgs e)
        {
            AplicarFormatacaoSelecionada();
        }

        private void pnlPreview_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                foreach (KeyValuePair<string, RectangleF> item in areasPreview)
                {
                    if (item.Value.Contains(new PointF(e.Location.X, e.Location.Y)))
                    {
                        SelecionarLinhaFormatacao(item.Key);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                glo.Loga("Erro pnlPreview_MouseClick: " + ex.Message);
            }
        }

        private void SelecionarLinhaFormatacao(string linha)
        {
            if (string.IsNullOrWhiteSpace(linha))
            {
                return;
            }

            linha = ObterChaveLinhaFormatacao(linha);

            if (fontesEdicao == null)
            {
                InicializarFontesEdicao(new EtiquetaModel());
            }

            if (!fontesEdicao.ContainsKey(linha))
            {
                return;
            }

            linhaSelecionada = linha;

            try
            {
                carregandoFormatacao = true;
                string textoLinha = ObterTextoLinhaFormatacao(linha);
                if (cmbLinhaFormatacao.Items.Contains(textoLinha))
                {
                    cmbLinhaFormatacao.SelectedItem = textoLinha;
                }
            }
            finally
            {
                carregandoFormatacao = false;
            }

            CarregarControlesFormatacao();
            pnlPreview.Invalidate();
        }

        private void SelecionarEtiquetaNoGrid(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return;
                }

                foreach (DataGridViewRow row in gridEtiquetas.Rows)
                {
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    object valorId = row.Cells["Id"].Value;
                    if (valorId != null && string.Equals(valorId.ToString(), id, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        gridEtiquetas.CurrentCell = row.Cells["Codigo"];
                        gridEtiquetas.FirstDisplayedScrollingRowIndex = row.Index;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                glo.Loga("Erro SelecionarEtiquetaNoGrid: " + ex.Message);
            }
        }

        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return string.Empty;
            }

            string normalizado = texto.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalizado)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void AplicarFiltro()
        {
            try
            {
                string termo = NormalizarTexto(txtBuscar.Text);
                if (string.IsNullOrWhiteSpace(termo))
                {
                    CarregarGrid(etiquetas);
                    return;
                }

                List<EtiquetaModel> listaFiltrada = etiquetas
                    .Where(etiqueta =>
                        NormalizarTexto(etiqueta.Codigo).Contains(termo) ||
                        NormalizarTexto(etiqueta.Descricao).Contains(termo) ||
                        NormalizarTexto(etiqueta.Preco).Contains(termo) ||
                        NormalizarTexto(etiqueta.Observacao).Contains(termo))
                    .ToList();

                CarregarGrid(listaFiltrada);
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao filtrar etiquetas: " + ex.Message);
            }
        }

        private void RegistrarConfiguracaoEssencialImpressao(
            PrintDocument doc,
            string impressora,
            EtiquetaModel etiqueta,
            int quantidade,
            string tentativaId)
        {
            if (!glo.LogRemoto)
            {
                return;
            }

            string codigo = etiqueta != null ? etiqueta.Codigo : "";
            string nomeEtiqueta = etiqueta != null ? etiqueta.NomeEtiqueta : "";

            try
            {
                PrinterSettings settings = doc.PrinterSettings;
                PageSettings pagina = doc.DefaultPageSettings;
                PaperSize papel = pagina.PaperSize;
                PrinterResolution resolucao = pagina.PrinterResolution;
                RectangleF areaImprimivel = pagina.PrintableArea;
                bool impressoraValida = settings.IsValid;
                int quantidadePapeisSuportados = settings.PaperSizes.Count;
                bool papelEncontradoPorNome = false;
                bool papelEncontradoPorDimensao = false;
                bool papelEncontradoPorDimensaoInvertida = false;

                foreach (PaperSize papelSuportado in settings.PaperSizes)
                {
                    if (string.Equals(papelSuportado.PaperName, papel.PaperName, StringComparison.OrdinalIgnoreCase))
                    {
                        papelEncontradoPorNome = true;
                    }

                    if (papelSuportado.Width == papel.Width && papelSuportado.Height == papel.Height)
                    {
                        papelEncontradoPorDimensao = true;
                    }

                    if (papelSuportado.Width == papel.Height && papelSuportado.Height == papel.Width)
                    {
                        papelEncontradoPorDimensaoInvertida = true;
                    }
                }

                bool alertaPapelNaoEncontrado = !papelEncontradoPorNome &&
                    !papelEncontradoPorDimensao &&
                    !papelEncontradoPorDimensaoInvertida;
                bool alertaAreaImprimivelMenorQuePapel = areaImprimivel.Width < papel.Width ||
                    areaImprimivel.Height < papel.Height;
                string mensagem =
                    "DocumentName=" + doc.DocumentName +
                    "; PrinterName=" + settings.PrinterName +
                    "; PrinterSettings.IsValid=" + impressoraValida +
                    "; PaperName=" + papel.PaperName +
                    "; PaperKind=" + papel.Kind +
                    "; PaperRawKind=" + papel.RawKind +
                    "; PaperWidth=" + papel.Width +
                    "; PaperHeight=" + papel.Height +
                    "; Landscape=" + pagina.Landscape +
                    "; PrinterResolutionKind=" + (resolucao == null ? "(nulo)" : resolucao.Kind.ToString()) +
                    "; PrinterResolutionX=" + (resolucao == null ? 0 : resolucao.X) +
                    "; PrinterResolutionY=" + (resolucao == null ? 0 : resolucao.Y) +
                    "; MarginsLeft=" + pagina.Margins.Left +
                    "; MarginsRight=" + pagina.Margins.Right +
                    "; MarginsTop=" + pagina.Margins.Top +
                    "; MarginsBottom=" + pagina.Margins.Bottom +
                    "; HardMarginX=" + pagina.HardMarginX +
                    "; HardMarginY=" + pagina.HardMarginY +
                    "; PrintableAreaWidth=" + areaImprimivel.Width +
                    "; PrintableAreaHeight=" + areaImprimivel.Height +
                    "; UnidadeMedida=CentesimosDePolegada" +
                    "; QuantidadePapeisSuportados=" + quantidadePapeisSuportados +
                    "; PapelEncontradoPorNome=" + papelEncontradoPorNome +
                    "; PapelEncontradoPorDimensao=" + papelEncontradoPorDimensao +
                    "; PapelEncontradoPorDimensaoInvertida=" + papelEncontradoPorDimensaoInvertida +
                    "; AlertaImpressoraInvalida=" + !impressoraValida +
                    "; AlertaPapelNaoEncontrado=" + alertaPapelNaoEncontrado +
                    "; AlertaAreaImprimivelMenorQuePapel=" + alertaAreaImprimivelMenorQuePapel;

                LogRemotoEtiquetas.Registrar(
                    "ConfiguracaoEssencialImpressao",
                    "INFO",
                    mensagem,
                    impressora,
                    quantidade,
                    codigo,
                    nomeEtiqueta,
                    tentativaId);
            }
            catch (Exception ex)
            {
                LogRemotoEtiquetas.RegistrarErro(
                    "ErroConfiguracaoEssencialImpressao",
                    ex,
                    impressora,
                    quantidade,
                    codigo,
                    nomeEtiqueta,
                    tentativaId);
            }
        }

        private void ImprimirEtiqueta(EtiquetaModel etiqueta, string impressora, int quantidade, string tentativaId)
        {
            try
            {
                etiquetaImpressao = etiqueta;
                copiasRestantes = quantidade;
                copiasTotais = quantidade;
                impressoraImpressao = impressora;
                tentativaImpressaoAtual = tentativaId ?? "";
                erroPrintPageTratado = false;

                PrintDocument doc = new PrintDocument();
                doc.PrinterSettings.PrinterName = impressora;
                doc.DocumentName = "Etiqueta_" +
                    (string.IsNullOrWhiteSpace(etiqueta != null ? etiqueta.Id : "") ? "SemId" : etiqueta.Id) +
                    "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                doc.DefaultPageSettings.PaperSize = new PaperSize("Etiqueta 60x30mm", 236, 118);
                doc.DefaultPageSettings.Margins = new Margins(2, 2, 2, 2);
                doc.PrintPage += Doc_PrintPage;
                LogRemotoEtiquetas.Registrar(
                    "ConfigurarImpressao",
                    "OK",
                    "DocumentName=" + doc.DocumentName + "; PrinterName=" + doc.PrinterSettings.PrinterName + "; PaperSize=" + doc.DefaultPageSettings.PaperSize.PaperName + "; Largura=236; Altura=118; Margens=2,2,2,2; Quantidade=" + quantidade,
                    impressora,
                    quantidade,
                    etiqueta != null ? etiqueta.Codigo : "",
                    etiqueta != null ? etiqueta.NomeEtiqueta : "",
                    tentativaId);
                RegistrarConfiguracaoEssencialImpressao(doc, impressora, etiqueta, quantidade, tentativaId);

                bool impressoraValida;
                try
                {
                    impressoraValida = doc.PrinterSettings.IsValid;
                }
                catch (Exception ex)
                {
                    glo.Loga("Erro ao validar impressora: " + ex.Message);
                    LogRemotoEtiquetas.RegistrarErroPendente(
                        "ErroValidarImpressora",
                        ex,
                        impressora,
                        quantidade,
                        etiqueta != null ? etiqueta.Codigo : "",
                        etiqueta != null ? etiqueta.NomeEtiqueta : "",
                        tentativaId);
                    EnvioLogRemotoEtiquetas.DispararEnvioAssincrono();
                    MessageBox.Show(
                        "Não foi possível validar a impressora selecionada. A impressão não foi enviada. Confira a instalação, o compartilhamento ou o nome da impressora.",
                        "Impressão",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (!impressoraValida)
                {
                    LogRemotoEtiquetas.RegistrarPendente(
                        "ImpressoraInvalida",
                        "ERRO",
                        "Nome configurado=" + impressora + "; PrinterSettings.IsValid=False; impressão não foi enviada ao Windows.",
                        impressora,
                        quantidade,
                        etiqueta != null ? etiqueta.Codigo : "",
                        etiqueta != null ? etiqueta.NomeEtiqueta : "",
                        tentativaId);
                    EnvioLogRemotoEtiquetas.DispararEnvioAssincrono();
                    MessageBox.Show(
                        "A impressora selecionada não está disponível ou não é válida no Windows. A impressão não foi enviada. Confira a instalação, o compartilhamento ou o nome da impressora.",
                        "Impressão",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                LogRemotoEtiquetas.Registrar(
                    "ValidarImpressora",
                    "OK",
                    "Impressora válida para o Windows. PrinterSettings.IsValid=True; Nome=" + impressora,
                    impressora,
                    quantidade,
                    etiqueta != null ? etiqueta.Codigo : "",
                    etiqueta != null ? etiqueta.NomeEtiqueta : "",
                    tentativaId);

                doc.Print();
                LogRemotoEtiquetas.Registrar(
                    "PrintRetornou",
                    "OK",
                    "PrintDocument.Print retornou sem exceção",
                    impressora,
                    quantidade,
                    etiqueta != null ? etiqueta.Codigo : "",
                    etiqueta != null ? etiqueta.NomeEtiqueta : "",
                    tentativaId);
                MonitorSpoolerEtiquetas.Iniciar(
                    doc.DocumentName,
                    impressora,
                    etiqueta,
                    quantidade,
                    tentativaId);
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao imprimir etiqueta: " + ex.Message);
                if (!erroPrintPageTratado)
                {
                    LogRemotoEtiquetas.RegistrarErroPendente(
                        "ErroImpressao",
                        ex,
                        impressora,
                        quantidade,
                        etiqueta != null ? etiqueta.Codigo : "",
                        etiqueta != null ? etiqueta.NomeEtiqueta : "",
                        tentativaId);
                    EnvioLogRemotoEtiquetas.DispararEnvioAssincrono();
                }
                throw;
            }
            finally
            {
                tentativaImpressaoAtual = string.Empty;
            }
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                LogRemotoEtiquetas.Registrar(
                    "PrintPage",
                    "INFO",
                    "Cópia atual: " + (copiasTotais - copiasRestantes + 1) + "; Cópias restantes: " + copiasRestantes,
                    impressoraImpressao,
                    copiasRestantes,
                    etiquetaImpressao != null ? etiquetaImpressao.Codigo : "",
                    etiquetaImpressao != null ? etiquetaImpressao.NomeEtiqueta : "",
                    tentativaImpressaoAtual);
                Rectangle area = e.MarginBounds;
                e.Graphics.Clear(Color.White);

                if (etiquetaImpressao != null)
                {
                    etiquetaImpressao.Fontes = etiquetaImpressao.ObterFontesComPadrao();
                }

                string nomeEmpresa = etiquetaImpressao?.NomeEmpresa ?? string.Empty;
                string telefone = etiquetaImpressao?.Telefone ?? string.Empty;
                string codigo = FormatarCodigoEtiqueta(etiquetaImpressao?.Codigo);
                string descricao = etiquetaImpressao?.Descricao ?? string.Empty;
                string preco = FormatarPrecoEtiqueta(etiquetaImpressao?.Preco);
                string observacao = etiquetaImpressao?.Observacao ?? string.Empty;
                string teleEntrega = etiquetaImpressao?.TeleEntrega ?? string.Empty;
                string local = FormatarLocalEtiqueta(etiquetaImpressao?.Local);

                using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (StringFormat observacaoFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap })
                using (StringFormat right = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.None, FormatFlags = StringFormatFlags.NoWrap })
                {
                    float margemHorizontal = 4f;
                    float larguraTexto = area.Width - (margemHorizontal * 2);
                    float altura = area.Height;
                    RectangleF nomeEmpresaRect = new RectangleF(area.X + margemHorizontal, area.Y + (altura * 0.03f), larguraTexto, altura * 0.13f);
                    RectangleF descricaoRect = new RectangleF(area.X + margemHorizontal, area.Y + (altura * 0.18f), larguraTexto, altura * 0.15f);
                    RectangleF precoRect = new RectangleF(area.X + margemHorizontal, area.Y + (altura * 0.35f), larguraTexto, altura * 0.18f);
                    RectangleF observacaoRect = new RectangleF(area.X + margemHorizontal, area.Y + (altura * 0.55f), larguraTexto, altura * 0.10f);
                    float larguraLocal = larguraTexto * 0.45f;
                    float margemDireitaCodigo = Math.Max(1f, larguraTexto * 0.02f);
                    float espacoEntreLinhas = 2f;
                    float xLinha = area.X + margemHorizontal;
                    float xCodigo = xLinha + larguraLocal + espacoEntreLinhas;
                    float larguraCodigo = larguraTexto - larguraLocal - espacoEntreLinhas - margemDireitaCodigo;
                    RectangleF localRect = new RectangleF(xLinha, area.Y + (altura * 0.59f), larguraLocal, altura * 0.14f);
                    RectangleF codigoRect = new RectangleF(xCodigo, area.Y + (altura * 0.59f), larguraCodigo, altura * 0.14f);
                    RectangleF telefoneRect = new RectangleF(area.X + margemHorizontal, area.Y + (altura * 0.74f), larguraTexto, altura * 0.12f);
                    RectangleF teleEntregaRect = new RectangleF(area.X + margemHorizontal, area.Y + (altura * 0.87f), larguraTexto, altura * 0.12f);

                    if (EstaEmModoTextoLivre())
                    {
                        RectangleF areaTextoLivre = ObterAreaTextoLivre(area);
                        using (Font fontTextoLivre = CriarFonteImpressao("Observacao", e.Graphics, observacao, areaTextoLivre, false))
                        using (StringFormat textoLivreFormat = new StringFormat
                        {
                            Alignment = StringAlignment.Near,
                            LineAlignment = StringAlignment.Near,
                            Trimming = StringTrimming.None
                        })
                        using (Brush brushTextoLivre = new SolidBrush(Color.Black))
                        {
                            e.Graphics.DrawString(observacao, fontTextoLivre, brushTextoLivre, areaTextoLivre, textoLivreFormat);
                        }
                    }
                    else
                    {
                    using (Font fontNomeEmpresa = CriarFonteImpressao("NomeEmpresa", e.Graphics, nomeEmpresa, nomeEmpresaRect))
                    using (Font fontTelefone = CriarFonteImpressao("Telefone", e.Graphics, telefone, telefoneRect))
                    using (Font fontCodigo = CriarFonteImpressao("Codigo", e.Graphics, codigo, codigoRect))
                    using (Font fontDescricao = CriarFonteImpressao("Descricao", e.Graphics, descricao, descricaoRect))
                    using (Font fontPreco = CriarFonteImpressao("Preco", e.Graphics, preco, precoRect))
                    using (Font fontObservacao = CriarFonteImpressao("Observacao", e.Graphics, observacao, observacaoRect))
                    using (Font fontTeleEntrega = CriarFonteImpressao("TeleEntrega", e.Graphics, teleEntrega, teleEntregaRect))
                    using (Font fontLocal = CriarFonteImpressao("Local", e.Graphics, local, localRect))
                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        e.Graphics.DrawString(nomeEmpresa, fontNomeEmpresa, brush, nomeEmpresaRect, center);
                        e.Graphics.DrawString(descricao, fontDescricao, brush, descricaoRect, center);
                        e.Graphics.DrawString(preco, fontPreco, brush, precoRect, center);
                        e.Graphics.DrawString(observacao, fontObservacao, brush, observacaoRect, observacaoFormat);
                        e.Graphics.DrawString(codigo, fontCodigo, brush, codigoRect, right);
                        e.Graphics.DrawString(telefone, fontTelefone, brush, telefoneRect, center);
                        e.Graphics.DrawString(teleEntrega, fontTeleEntrega, brush, teleEntregaRect, center);
                        using (StringFormat left = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
                        {
                            e.Graphics.DrawString(local, fontLocal, brush, localRect, left);
                        }
                    }
                    }
                }

                copiasRestantes--;
                e.HasMorePages = copiasRestantes > 0;
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao imprimir etiqueta: " + ex.Message);
                erroPrintPageTratado = true;
                LogRemotoEtiquetas.RegistrarErroPendente(
                    "ErroImpressao",
                    ex,
                    impressoraImpressao,
                    copiasRestantes,
                    etiquetaImpressao != null ? etiquetaImpressao.Codigo : "",
                    etiquetaImpressao != null ? etiquetaImpressao.NomeEtiqueta : "",
                    tentativaImpressaoAtual);
                EnvioLogRemotoEtiquetas.DispararEnvioAssincrono();
                e.HasMorePages = false;
                throw;
            }
        }

        private Font CriarFonteAjustada(Graphics graphics, string texto, RectangleF area, string nomeFonte, float tamanhoDesejado, bool negrito, float tamanhoMinimo, bool ajustarAutomaticamente = true)
        {
            float tamanhoAtual = tamanhoDesejado > 0 ? tamanhoDesejado : 8f;
            float tamanhoLimite = tamanhoMinimo > 0 ? tamanhoMinimo : 5f;
            FontStyle style = negrito ? FontStyle.Bold : FontStyle.Regular;
            string fonteFinal = nomeFonte;

            if (string.IsNullOrWhiteSpace(fonteFinal) || !FontFamily.Families.Any(f => string.Equals(f.Name, fonteFinal, StringComparison.OrdinalIgnoreCase)))
            {
                fonteFinal = "Arial";
            }

            while (tamanhoAtual >= tamanhoLimite)
            {
                Font fonte;
                try
                {
                    fonte = new Font(fonteFinal, tamanhoAtual, style);
                }
                catch
                {
                    fonteFinal = "Arial";
                    fonte = new Font(fonteFinal, tamanhoAtual, style);
                }

                using (StringFormat format = new StringFormat(StringFormat.GenericTypographic))
                {
                    if (!ajustarAutomaticamente)
                    {
                        return fonte;
                    }

                    format.FormatFlags |= StringFormatFlags.NoWrap;
                    SizeF medida = graphics.MeasureString(texto ?? string.Empty, fonte, new SizeF(area.Width, area.Height), format);
                    if (medida.Width <= area.Width && medida.Height <= area.Height)
                    {
                        return fonte;
                    }
                }

                fonte.Dispose();
                tamanhoAtual -= 0.5f;
            }

            float tamanhoFinal = tamanhoLimite;
            try
            {
                return new Font(fonteFinal, tamanhoFinal, style);
            }
            catch
            {
                return new Font("Arial", tamanhoFinal, style);
            }
        }

        private Font CriarFonteImpressao(string linha, Graphics graphics, string texto, RectangleF area, bool ajustarAutomaticamente = true)
        {
            string nomeFonte = "Arial";
            float tamanho = 8f;
            bool negrito = false;

            if (etiquetaImpressao != null)
            {
                Dictionary<string, EtiquetaFonteConfig> fontes = etiquetaImpressao.ObterFontesComPadrao();
                EtiquetaFonteConfig config;
                if (fontes != null && fontes.TryGetValue(linha, out config) && config != null)
                {
                    if (!string.IsNullOrWhiteSpace(config.NomeFonte))
                    {
                        nomeFonte = config.NomeFonte;
                    }

                    if (config.Tamanho > 0)
                    {
                        tamanho = config.Tamanho;
                    }

                    negrito = config.Negrito;
                }
            }

            try
            {
                if (!FontFamily.Families.Any(f => string.Equals(f.Name, nomeFonte, StringComparison.OrdinalIgnoreCase)))
                {
                    nomeFonte = "Arial";
                }

                return CriarFonteAjustada(graphics, texto, area, nomeFonte, tamanho, negrito, 5f, ajustarAutomaticamente && !EhLinhaComTamanhoPrioritario(linha));
            }
            catch
            {
                return CriarFonteAjustada(graphics, texto, area, "Arial", tamanho > 0 ? tamanho : 8f, negrito, 5f, ajustarAutomaticamente && !EhLinhaComTamanhoPrioritario(linha));
            }
        }
    }
}
