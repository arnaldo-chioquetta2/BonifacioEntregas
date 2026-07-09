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
        private EtiquetaModel etiquetaImpressao;
        private int copiasRestantes;

        public operEtiquetas()
        {
            InitializeComponent();
            txtCodigo.TextChanged += CamposPreview_TextChanged;
            txtDescricao.TextChanged += CamposPreview_TextChanged;
            txtPreco.TextChanged += CamposPreview_TextChanged;
            txtObservacao.TextChanged += CamposPreview_TextChanged;
        }

        private void operEtiquetas_Load(object sender, EventArgs e)
        {
            try
            {
                etiquetas = repository.Listar();
                ConfigurarGrid();
                CarregarGrid(etiquetas);
                CarregarImpressoras();
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
                    }
                }

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
                txtCodigo.Text = Convert.ToString(row.Cells["Codigo"].Value);
                txtDescricao.Text = Convert.ToString(row.Cells["Descricao"].Value);
                txtPreco.Text = Convert.ToString(row.Cells["Preco"].Value);
                txtObservacao.Text = Convert.ToString(row.Cells["Observacao"].Value);
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

                string codigo = txtCodigo.Text.Trim();
                string descricao = txtDescricao.Text.Trim();
                string preco = txtPreco.Text.Trim();
                string observacao = txtObservacao.Text.Trim();

                using (Font fontCodigo = new Font("Arial", 8, FontStyle.Bold))
                using (Font fontDescricao = new Font("Arial", 10, FontStyle.Regular))
                using (Font fontPreco = new Font("Arial", 16, FontStyle.Bold))
                using (Font fontObs = new Font("Arial", 8, FontStyle.Regular))
                using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    Rectangle codigoRect = new Rectangle(etiquetaRect.X + 4, etiquetaRect.Y + 4, etiquetaRect.Width - 8, 16);
                    Rectangle descricaoRect = new Rectangle(etiquetaRect.X + 4, etiquetaRect.Y + (etiquetaRect.Height / 4) - 4, etiquetaRect.Width - 8, 24);
                    Rectangle precoRect = new Rectangle(etiquetaRect.X + 4, etiquetaRect.Y + (etiquetaRect.Height / 2) - 10, etiquetaRect.Width - 8, 28);
                    Rectangle obsRect = new Rectangle(etiquetaRect.X + 4, etiquetaRect.Bottom - 22, etiquetaRect.Width - 8, 16);

                    e.Graphics.DrawString(codigo, fontCodigo, Brushes.Black, codigoRect, center);
                    e.Graphics.DrawString(descricao, fontDescricao, Brushes.Black, descricaoRect, center);
                    e.Graphics.DrawString(preco, fontPreco, Brushes.Black, precoRect, center);
                    e.Graphics.DrawString(observacao, fontObs, Brushes.Black, obsRect, center);
                }
            }
            catch (Exception ex)
            {
                glo.Loga("Erro pnlPreview_Paint: " + ex.Message);
            }
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
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Codigo", HeaderText = "Código", ReadOnly = true, FillWeight = 85 });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Descricao", HeaderText = "Descrição", ReadOnly = true, FillWeight = 190 });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Preco", HeaderText = "Preço", ReadOnly = true, FillWeight = 75 });
                gridEtiquetas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Observacao", HeaderText = "Observação", ReadOnly = true, FillWeight = 160 });
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
                    gridEtiquetas.Rows.Add(numero.ToString("000"), etiqueta.Id, etiqueta.Codigo, etiqueta.Descricao, etiqueta.Preco, etiqueta.Observacao);
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
                Codigo = txtCodigo.Text.Trim(),
                Descricao = txtDescricao.Text.Trim(),
                Preco = txtPreco.Text.Trim(),
                Observacao = txtObservacao.Text.Trim()
            };
        }

        private void LimparCampos()
        {
            try
            {
                etiquetaSelecionadaId = "";
                txtCodigo.Clear();
                txtDescricao.Clear();
                txtPreco.Clear();
                txtObservacao.Clear();
                txtBuscar.Clear();
                numQuantidade.Value = 1;
                gridEtiquetas.ClearSelection();
                pnlPreview.Invalidate();
            }
            catch (Exception ex)
            {
                glo.Loga("Erro LimparCampos: " + ex.Message);
            }
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
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    e.Graphics.DrawRectangle(pen, area);
                }

                string codigo = etiquetaImpressao?.Codigo ?? string.Empty;
                string descricao = etiquetaImpressao?.Descricao ?? string.Empty;
                string preco = etiquetaImpressao?.Preco ?? string.Empty;
                string observacao = etiquetaImpressao?.Observacao ?? string.Empty;

                using (Font fontCodigo = new Font("Arial", 8, FontStyle.Bold))
                using (Font fontDescricao = new Font("Arial", 7, FontStyle.Regular))
                using (Font fontPreco = new Font("Arial", 12, FontStyle.Bold))
                using (Font fontObs = new Font("Arial", 7, FontStyle.Regular))
                using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    Rectangle codigoRect = new Rectangle(area.X + 4, area.Y + 4, area.Width - 8, 14);
                    Rectangle descricaoRect = new Rectangle(area.X + 4, area.Y + (area.Height / 4) - 2, area.Width - 8, 18);
                    Rectangle precoRect = new Rectangle(area.X + 4, area.Y + (area.Height / 2) - 10, area.Width - 8, 24);
                    Rectangle obsRect = new Rectangle(area.X + 4, area.Bottom - 18, area.Width - 8, 14);

                    e.Graphics.DrawString(codigo, fontCodigo, Brushes.Black, codigoRect, center);
                    e.Graphics.DrawString(descricao, fontDescricao, Brushes.Black, descricaoRect, center);
                    e.Graphics.DrawString(preco, fontPreco, Brushes.Black, precoRect, center);
                    e.Graphics.DrawString(observacao, fontObs, Brushes.Black, obsRect, center);
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
    }
}
