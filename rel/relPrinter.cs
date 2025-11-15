using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace TeleBonifacio.rel
{
    /// <summary>
    /// Impressão de texto simples com título, paginação, preview e rodapé.
    /// </summary>
    public class relPrinter
    {
        private readonly string _conteudo;
        private readonly string _titulo;
        private readonly Font _fontTexto;
        private readonly Font _fontTitulo;
        private readonly Font _fontRodape;

        private string[] _linhas;
        private int _indiceLinhaAtual;
        private int _numeroPagina;

        public relPrinter(string conteudo, string titulo)
        {
            _conteudo = conteudo ?? string.Empty;
            _titulo = string.IsNullOrWhiteSpace(titulo) ? "Relatório" : titulo;
            _fontTexto = new Font("Courier New", 10f);         // monoespaçada p/ colunas
            _fontTitulo = new Font("Segoe UI", 12f, FontStyle.Bold);
            _fontRodape = new Font("Segoe UI", 8f, FontStyle.Regular);

            _linhas = _conteudo.Replace("\r\n", "\n").Split('\n');
            _indiceLinhaAtual = 0;
            _numeroPagina = 1;
        }

        /// <summary>
        /// Abre o Preview e permite imprimir.
        /// </summary>
        public void Imprimir()
        {
            using (var pd = new PrintDocument())
            {
                pd.DocumentName = _titulo;
                pd.PrintPage += PrintPageHandler;
                pd.BeginPrint += (s, e) =>
                {
                    _indiceLinhaAtual = 0;
                    _numeroPagina = 1;
                };

                using (var preview = new PrintPreviewDialog())
                {
                    preview.Document = pd;
                    preview.WindowState = FormWindowState.Maximized;
                    preview.ShowDialog();
                }
            }
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            float top = e.MarginBounds.Top;
            float left = e.MarginBounds.Left;
            float right = e.MarginBounds.Right;

            // Cabeçalho (título + período opcional)
            string titulo = _titulo;
            SizeF sTitulo = e.Graphics.MeasureString(titulo, _fontTitulo, e.MarginBounds.Width);
            e.Graphics.DrawString(titulo, _fontTitulo, Brushes.Black,
                new RectangleF(left, top, e.MarginBounds.Width, sTitulo.Height),
                new StringFormat { Alignment = StringAlignment.Center });
            top += sTitulo.Height + 8;

            // Linha separadora
            e.Graphics.DrawLine(Pens.Black, left, top, right, top);
            top += 6;

            // Calcula quantas linhas cabem na página
            float alturaLinha = _fontTexto.GetHeight(e.Graphics);
            // Reserva espaço para rodapé
            float alturaRodape = _fontRodape.GetHeight(e.Graphics) + 10;
            int linhasPorPagina = (int)((e.MarginBounds.Bottom - top - alturaRodape) / alturaLinha);
            if (linhasPorPagina < 1) linhasPorPagina = 1;

            // Imprime linhas
            int linhasImpressas = 0;
            while (_indiceLinhaAtual < _linhas.Length && linhasImpressas < linhasPorPagina)
            {
                string linha = _linhas[_indiceLinhaAtual];
                e.Graphics.DrawString(linha, _fontTexto, Brushes.Black, left, top);
                top += alturaLinha;
                _indiceLinhaAtual++;
                linhasImpressas++;
            }

            // Rodapé (número da página e data/hora)
            string rodapeEsq = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            string rodapeDir = $"Página {_numeroPagina}";
            SizeF sRodapeDir = e.Graphics.MeasureString(rodapeDir, _fontRodape);

            float yRodape = e.MarginBounds.Bottom - sRodapeDir.Height;
            e.Graphics.DrawString(rodapeEsq, _fontRodape, Brushes.Gray, left, yRodape);
            e.Graphics.DrawString(rodapeDir, _fontRodape, Brushes.Gray, right - sRodapeDir.Width, yRodape);

            // Continua?
            if (_indiceLinhaAtual < _linhas.Length)
            {
                e.HasMorePages = true;
                _numeroPagina++;
            }
            else
            {
                e.HasMorePages = false;
            }
        }
    }
}
