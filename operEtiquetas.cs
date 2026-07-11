using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
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
            txtObservacao.TextChanged += CamposPreview_TextChanged;
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
                cmbLinhaFormatacao.Items.Add("Tele-entrega");
                cmbLinhaFormatacao.Items.Add("Local");
                cmbLinhaFormatacao.SelectedItem = "Código";
                CarregarListaFontes();
                InicializarFontesEdicao(new EtiquetaModel());
                linhaSelecionada = "Codigo";
                CarregarControlesFormatacao();
                numQuantidade.Value = 1;
                pnlPreview.Invalidate();
            }
            catch (Exception ex)
            {
                glo.Loga("Erro operEtiquetas_Load: " + ex.Message);
            }
        }

        private void btNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
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
                string idParaSelecionar = etiqueta.Id;

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

                repository.Salvar(etiqueta);
                etiquetas = repository.Listar();
                CarregarGrid(etiquetas);

                etiquetaSelecionadaId = etiqueta.Id;
                SelecionarEtiquetaNoGrid(idParaSelecionar);
                pnlPreview.Invalidate();

                MessageBox.Show("Etiqueta salva com sucesso.");
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao salvar etiqueta: " + ex.Message);
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

                repository.Excluir(etiquetaSelecionadaId);
                etiquetas = repository.Listar();
                CarregarGrid(etiquetas);
                LimparCampos();
                pnlPreview.Invalidate();

                MessageBox.Show("Etiqueta excluída com sucesso.");
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao excluir etiqueta: " + ex.Message);
                MessageBox.Show("Não foi possível excluir a etiqueta. Tente novamente.");
            }
        }

        private void btLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
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

                ImprimirEtiqueta(etiqueta, cmbImpressora.SelectedItem.ToString(), quantidade);
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao imprimir etiqueta: " + ex.Message);
                MessageBox.Show("Não foi possível imprimir a etiqueta. Tente novamente.");
            }
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
                linhaSelecionada = "Codigo";
                CarregarControlesFormatacao();
                pnlPreview.Invalidate();
            }
            catch (Exception ex)
            {
                glo.Loga("Erro gridEtiquetas_CellClick: " + ex.Message);
            }
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

                RectangleF linhaNomeEmpresa = new RectangleF(etiquetaRect.X + 4, etiquetaRect.Y + 4, etiquetaRect.Width - 8, 14);
                RectangleF linhaTelefone = new RectangleF(etiquetaRect.X + 4, etiquetaRect.Y + 18, etiquetaRect.Width - 8, 14);
                RectangleF linhaCodigo = new RectangleF(etiquetaRect.X + 4, etiquetaRect.Y + 34, etiquetaRect.Width - 8, 14);
                RectangleF linhaDescricao = new RectangleF(etiquetaRect.X + 4, etiquetaRect.Y + 52, etiquetaRect.Width - 8, 16);
                RectangleF linhaPreco = new RectangleF(etiquetaRect.X + 4, etiquetaRect.Y + 70, etiquetaRect.Width - 8, 24);
                RectangleF linhaObservacao = new RectangleF(etiquetaRect.X + 4, etiquetaRect.Y + 96, etiquetaRect.Width - 8, 14);
                RectangleF linhaLocal = new RectangleF(etiquetaRect.X + 4, etiquetaRect.Bottom - 18, (etiquetaRect.Width / 2f) - 6, 14);
                RectangleF linhaTeleEntrega = new RectangleF(etiquetaRect.X + (etiquetaRect.Width / 2f) + 2, etiquetaRect.Bottom - 18, (etiquetaRect.Width / 2f) - 6, 14);

                areasPreview["NomeEmpresa"] = linhaNomeEmpresa;
                areasPreview["Telefone"] = linhaTelefone;
                areasPreview["Codigo"] = linhaCodigo;
                areasPreview["Descricao"] = linhaDescricao;
                areasPreview["Preco"] = linhaPreco;
                areasPreview["Observacao"] = linhaObservacao;
                areasPreview["Local"] = linhaLocal;
                areasPreview["TeleEntrega"] = linhaTeleEntrega;

                using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (StringFormat left = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
                {
                    DesenharLinhaPreview(e.Graphics, "NomeEmpresa", txtNomeEmpresa.Text.Trim(), linhaNomeEmpresa, center, Color.Black, true);
                    DesenharLinhaPreview(e.Graphics, "Telefone", txtTelefone.Text.Trim(), linhaTelefone, center, Color.Black, false);
                    DesenharLinhaPreview(e.Graphics, "Codigo", FormatarCodigoEtiqueta(txtCodigo.Text), linhaCodigo, center, Color.Black, true);
                    DesenharLinhaPreview(e.Graphics, "Descricao", txtDescricao.Text.Trim(), linhaDescricao, center, Color.Black, false);
                    DesenharLinhaPreview(e.Graphics, "Preco", FormatarPrecoEtiqueta(txtPreco.Text), linhaPreco, center, Color.Black, true);
                    DesenharLinhaPreview(e.Graphics, "Observacao", txtObservacao.Text.Trim(), linhaObservacao, center, Color.Black, false);
                    DesenharLinhaPreview(e.Graphics, "Local", FormatarLocalEtiqueta(txtLocal.Text), linhaLocal, left, Color.Black, true);
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
                5f))
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
                return new EtiquetaFonteConfig
                {
                    NomeFonte = "Arial",
                    Tamanho = 8f,
                    Negrito = false
                };
            }

            return fonte;
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
            }
            catch (Exception ex)
            {
                glo.Loga("Erro CarregarImpressoras: " + ex.Message);
            }
        }

        private EtiquetaModel ObterEtiquetaDaTela()
        {
            return new EtiquetaModel
            {
                Id = etiquetaSelecionadaId,
                NomeEtiqueta = nomeEtiquetaSelecionada,
                NomeEmpresa = txtNomeEmpresa.Text.Trim(),
                Telefone = txtTelefone.Text.Trim(),
                TeleEntrega = txtTeleEntrega.Text.Trim(),
                Local = NormalizarLocalCampo(txtLocal.Text),
                Codigo = NormalizarCodigoCampo(txtCodigo.Text),
                Descricao = txtDescricao.Text.Trim(),
                Preco = NormalizarPrecoCampo(txtPreco.Text),
                Observacao = txtObservacao.Text.Trim(),
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
            return string.IsNullOrWhiteSpace(valor) ? string.Empty : "LOCAL: " + valor;
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

        private void ImprimirEtiqueta(EtiquetaModel etiqueta, string impressora, int quantidade)
        {
            try
            {
                etiquetaImpressao = etiqueta;
                copiasRestantes = quantidade;

                PrintDocument doc = new PrintDocument();
                doc.PrinterSettings.PrinterName = impressora;
                doc.DefaultPageSettings.PaperSize = new PaperSize("Etiqueta 60x30mm", 236, 118);
                doc.DefaultPageSettings.Margins = new Margins(2, 2, 2, 2);
                doc.PrintPage += Doc_PrintPage;
                doc.Print();
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao imprimir etiqueta: " + ex.Message);
                throw;
            }
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
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
                {
                    RectangleF nomeEmpresaRect = new RectangleF(area.X + 4, area.Y + 2, area.Width - 8, 12);
                    RectangleF telefoneRect = new RectangleF(area.X + 4, area.Y + 14, area.Width - 8, 12);
                    RectangleF codigoRect = new RectangleF(area.X + 4, area.Y + 28, area.Width - 8, 12);
                    RectangleF descricaoRect = new RectangleF(area.X + 4, area.Y + 43, area.Width - 8, 14);
                    RectangleF precoRect = new RectangleF(area.X + 4, area.Y + 58, area.Width - 8, 20);
                    RectangleF observacaoRect = new RectangleF(area.X + 4, area.Y + 82, area.Width - 8, 12);
                    RectangleF localRect = new RectangleF(area.X + 4, area.Bottom - 16, (area.Width / 2f) - 6, 12);
                    RectangleF teleEntregaRect = new RectangleF(area.X + (area.Width / 2f) + 2, area.Bottom - 16, (area.Width / 2f) - 6, 12);

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
                        e.Graphics.DrawString(telefone, fontTelefone, brush, telefoneRect, center);
                        e.Graphics.DrawString(codigo, fontCodigo, brush, codigoRect, center);
                        e.Graphics.DrawString(descricao, fontDescricao, brush, descricaoRect, center);
                        e.Graphics.DrawString(preco, fontPreco, brush, precoRect, center);
                        e.Graphics.DrawString(observacao, fontObservacao, brush, observacaoRect, center);
                        e.Graphics.DrawString(teleEntrega, fontTeleEntrega, brush, teleEntregaRect, center);
                        using (StringFormat left = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
                        {
                            e.Graphics.DrawString(local, fontLocal, brush, localRect, left);
                        }
                    }
                }

                copiasRestantes--;
                e.HasMorePages = copiasRestantes > 0;
            }
            catch (Exception ex)
            {
                glo.Loga("Erro ao imprimir etiqueta: " + ex.Message);
                e.HasMorePages = false;
                throw;
            }
        }

        private Font CriarFonteAjustada(Graphics graphics, string texto, RectangleF area, string nomeFonte, float tamanhoDesejado, bool negrito, float tamanhoMinimo)
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

        private Font CriarFonteImpressao(string linha, Graphics graphics, string texto, RectangleF area)
        {
            string nomeFonte = "Arial";
            float tamanho = 8f;
            bool negrito = false;

            if (etiquetaImpressao != null)
            {
                Dictionary<string, EtiquetaFonteConfig> fontes = etiquetaImpressao.Fontes ?? etiquetaImpressao.ObterFontesComPadrao();
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

                return CriarFonteAjustada(graphics, texto, area, nomeFonte, tamanho, negrito, 5f);
            }
            catch
            {
                return CriarFonteAjustada(graphics, texto, area, "Arial", tamanho > 0 ? tamanho : 8f, negrito, 5f);
            }
        }
    }
}
