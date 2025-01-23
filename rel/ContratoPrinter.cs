using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace TeleBonifacio.rel
{
    public class ContratoPrinter
    {
        private string contratante;
        private string contratanteCNPJ;
        private string contratanteEndereco;
        private string contratada;
        private string contratadaCPF;
        private string contratadaEndereco;
        private string nomeEmpresa; 
        private string cnpjEmpresa; 
        private string descricaoContrato; 
        private string[] clausulas;
        private decimal valorContrato;
        private int eMarginBoundsWidth = 800;

        public ContratoPrinter(
                string contratante,
                string contratanteCNPJ,
                string contratanteEndereco,
                string contratada,
                string contratadaCPF,
                string contratadaEndereco,
                string nomeEmpresa, // Novo parâmetro
                string cnpjEmpresa, // Novo parâmetro
                decimal valorContrato,
                string descricaoContrato,
                string[] clausulas)
        {
            this.contratante = contratante;
            this.contratanteCNPJ = contratanteCNPJ;
            this.contratanteEndereco = contratanteEndereco;
            this.contratada = contratada;
            this.contratadaCPF = contratadaCPF;
            this.contratadaEndereco = contratadaEndereco;
            this.nomeEmpresa = nomeEmpresa;
            this.cnpjEmpresa = cnpjEmpresa;
            this.valorContrato = valorContrato;
            this.clausulas = clausulas;
            this.descricaoContrato = descricaoContrato;
        }

        public void Imprimir()
        {
            // Gera o contrato no console para depuração (opcional)
            GerarContrato();

            // Configura e exibe o preview de impressão
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPageHandler;
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = pd
            };
            previewDialog.ShowDialog();
        }

        private void GerarContrato()
        {
            // Defina e inicialize as variáveis necessárias
            string contratante = "Empresa A";
            string contratanteCNPJ = "12.345.678/0001-99";
            string contratanteEndereco = "Rua A, 123, Cidade X";

            string contratada = "Empresa B";
            string contratadaCNPJ = "98.765.432/0001-11";
            string contratadaEndereco = "Avenida B, 456, Cidade Y";

            // Gerar o conteúdo principal do contrato
            Console.WriteLine($"Contratante: {contratante}, CNPJ: {contratanteCNPJ}, Endereço: {contratanteEndereco}");
            Console.WriteLine($"Contratada: {contratada}, CNPJ: {contratadaCNPJ}, Endereço: {contratadaEndereco}");
            Console.WriteLine("\nCláusulas do Contrato:");

            // Imprimir cada cláusula
            if (clausulas != null && clausulas.Length > 0)
            {
                int numero = 1;
                foreach (string clausula in clausulas)
                {
                    Console.WriteLine($"Cláusula {numero}: {clausula}");
                    numero++;
                }
            }
            else
            {
                Console.WriteLine("Nenhuma cláusula disponível.");
            }
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            float baseWidth = 800f;
            float baseHeight = 1200f;
            float scaleX = this.eMarginBoundsWidth / baseWidth;
            float scaleY = e.MarginBounds.Height / baseHeight;
            float scale = Math.Min(scaleX, scaleY);
            g.ScaleTransform(scale, scale);
            Font headerFont = new Font("Arial", 14, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 12);
            Font boldBodyFont = new Font("Arial", 12, FontStyle.Bold);
            Font titleFont = new Font("Arial", 12, FontStyle.Bold);
            Brush brush = Brushes.Black;
            Brush backgroundBrush = Brushes.Gray;
            float y = e.MarginBounds.Top;

            // Título geral
            g.DrawString("INSTRUMENTO PARTICULAR DE PRESTAÇÃO DE SERVIÇOS E OUTRAS AVENÇAS", headerFont, brush, new RectangleF(e.MarginBounds.Left, y, this.eMarginBoundsWidth, 50));
            y += 60;

            // Texto introdutório
            string textoIntro = "Por este instrumento particular (o 'Contrato'), a CONTRATANTE e a CONTRATADA, ambas identificadas no Quadro Resumo a seguir (em conjunto, as 'Partes' e, individualmente, uma 'Parte'), têm entre si, justo e contratado, a prestação de serviços identificada no presente contrato pelas seguintes cláusulas e condições:";
            SizeF textoIntroSize = g.MeasureString(textoIntro, bodyFont, this.eMarginBoundsWidth - 10);
            g.DrawString(textoIntro, bodyFont, brush, new RectangleF(e.MarginBounds.Left, y, this.eMarginBoundsWidth, textoIntroSize.Height));
            y += textoIntroSize.Height + 20;

            g.FillRectangle(backgroundBrush, e.MarginBounds.Left, y, this.eMarginBoundsWidth, 25);
            g.DrawString("DESCRIÇÃO DO CONTRATO:", titleFont, Brushes.White, e.MarginBounds.Left + 5, y + 5);
            y += 30;

            SizeF descricaoSize = g.MeasureString(descricaoContrato, bodyFont, this.eMarginBoundsWidth - 10);
            g.DrawRectangle(Pens.Black, e.MarginBounds.Left, y, this.eMarginBoundsWidth, descricaoSize.Height + 10);
            g.DrawString(descricaoContrato, bodyFont, brush, new RectangleF(e.MarginBounds.Left + 5, y + 5, this.eMarginBoundsWidth - 10, descricaoSize.Height));
            y += descricaoSize.Height + 20;

            // Caixa Contratante
            g.FillRectangle(backgroundBrush, e.MarginBounds.Left, y, this.eMarginBoundsWidth, 25);
            g.DrawString("CONTRATANTE:", titleFont, Brushes.White, e.MarginBounds.Left + 5, y + 5);
            y += 30;

            string contratanteInfo = $"Nome: {contratante}\nCNPJ: {contratanteCNPJ}\nEndereço: {contratanteEndereco}";
            SizeF contratanteSize = g.MeasureString(contratanteInfo, bodyFont, this.eMarginBoundsWidth - 10);
            g.DrawRectangle(Pens.Black, e.MarginBounds.Left, y, this.eMarginBoundsWidth, contratanteSize.Height + 10);
            g.DrawString(contratanteInfo, bodyFont, brush, new RectangleF(e.MarginBounds.Left + 5, y + 5, this.eMarginBoundsWidth - 10, contratanteSize.Height));
            y += contratanteSize.Height + 20;

            // Caixa Contratada
            g.FillRectangle(backgroundBrush, e.MarginBounds.Left, y, this.eMarginBoundsWidth, 25);
            g.DrawString("CONTRATADA:", titleFont, Brushes.White, e.MarginBounds.Left + 5, y + 5);
            y += 30;

            string contratadaInfo = !string.IsNullOrWhiteSpace(nomeEmpresa)
                ? $"Empresa: {nomeEmpresa}\nCNPJ: {cnpjEmpresa}\nCPF: {contratadaCPF}\nEndereço: {contratadaEndereco}"
                : $"Nome: {contratada}\nCPF: {contratadaCPF}\nEndereço: {contratadaEndereco}";
            SizeF contratadaSize = g.MeasureString(contratadaInfo, bodyFont, this.eMarginBoundsWidth - 10);
            g.DrawRectangle(Pens.Black, e.MarginBounds.Left, y, this.eMarginBoundsWidth, contratadaSize.Height + 10);
            g.DrawString(contratadaInfo, bodyFont, brush, new RectangleF(e.MarginBounds.Left + 5, y + 5, this.eMarginBoundsWidth - 10, contratadaSize.Height));
            y += contratadaSize.Height + 20;

            // Valor do Contrato
            g.FillRectangle(backgroundBrush, e.MarginBounds.Left, y, this.eMarginBoundsWidth, 25);
            g.DrawString("VALOR DO CONTRATO:", titleFont, Brushes.White, e.MarginBounds.Left + 5, y + 5);
            y += 30;

            string valorInfo = $"R$ {valorContrato:F2}";
            SizeF valorSize = g.MeasureString(valorInfo, bodyFont, this.eMarginBoundsWidth - 10);
            g.DrawRectangle(Pens.Black, e.MarginBounds.Left, y, this.eMarginBoundsWidth, valorSize.Height + 10);
            g.DrawString(valorInfo, bodyFont, brush, new RectangleF(e.MarginBounds.Left + 5, y + 5, this.eMarginBoundsWidth - 10, valorSize.Height));
            y += valorSize.Height + 20;

            // Cláusulas do contrato
            g.DrawString("Cláusulas do Contrato:", headerFont, brush, e.MarginBounds.Left, y);
            y += 30;

            foreach (string clausula in clausulas)
            {
                if (y + 50 > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                if (clausula.TrimStart().StartsWith("Cláusula", StringComparison.OrdinalIgnoreCase))
                {
                    string[] partes = clausula.Split(new[] { ' ' }, 2);
                    string palavraNegrito = partes[0];
                    string restanteTexto = partes.Length > 1 ? partes[1] : "";

                    SizeF tamanhoPalavraNegrito = g.MeasureString(palavraNegrito + " ", boldBodyFont);
                    g.DrawString(palavraNegrito, boldBodyFont, brush, e.MarginBounds.Left + 5, y);
                    g.DrawString(restanteTexto, bodyFont, brush, e.MarginBounds.Left + 5 + tamanhoPalavraNegrito.Width, y);
                }
                else
                {
                    g.DrawString(clausula, bodyFont, brush, e.MarginBounds.Left + 5, y);
                }

                SizeF clausulaSize = g.MeasureString(clausula, bodyFont, this.eMarginBoundsWidth - 10);
                y += clausulaSize.Height + 5;
            }

            y += 40;

            // Linhas de assinatura
            float signatureLineY = y;
            float signatureWidth = 300;

            g.DrawLine(Pens.Black, e.MarginBounds.Left, signatureLineY, e.MarginBounds.Left + signatureWidth, signatureLineY);
            g.DrawString("Assinatura Contratante", bodyFont, brush, e.MarginBounds.Left + (signatureWidth / 4), signatureLineY + 10);

            g.DrawLine(Pens.Black, e.MarginBounds.Right - signatureWidth, signatureLineY, e.MarginBounds.Right, signatureLineY);
            g.DrawString("Assinatura Contratada", bodyFont, brush, e.MarginBounds.Right - signatureWidth + (signatureWidth / 4), signatureLineY + 10);

            e.HasMorePages = false;
        }

    }
}

