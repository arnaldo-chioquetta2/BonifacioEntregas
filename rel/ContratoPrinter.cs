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
        private string contratadaCNPJ;
        private string contratadaEndereco;
        private string[] clausulas;

        public ContratoPrinter(
            string contratante,
            string contratanteCNPJ,
            string contratanteEndereco,
            string contratada,
            string contratadaCNPJ,
            string contratadaEndereco,
            string[] clausulas)
        {
            this.contratante = contratante;
            this.contratanteCNPJ = contratanteCNPJ;
            this.contratanteEndereco = contratanteEndereco;
            this.contratada = contratada;
            this.contratadaCNPJ = contratadaCNPJ;
            this.contratadaEndereco = contratadaEndereco;
            this.clausulas = clausulas;
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
            Font headerFont = new Font("Arial", 14, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 12);
            Font boldBodyFont = new Font("Arial", 12, FontStyle.Bold);
            Font titleFont = new Font("Arial", 12, FontStyle.Bold);
            Brush brush = Brushes.Black;
            Brush backgroundBrush = Brushes.Gray;
            float y = 50;

            // Título geral
            g.DrawString("INSTRUMENTO PARTICULAR DE PRESTAÇÃO DE SERVIÇOS E OUTRAS AVENÇAS", headerFont, brush, new RectangleF(50, y, e.PageBounds.Width - 100, 50));
            y += 60;

            // Texto introdutório
            string textoIntro = "Por este instrumento particular (o 'Contrato'), a CONTRATANTE e a CONTRATADA, ambas identificadas no Quadro Resumo a seguir (em conjunto, as 'Partes' e, individualmente, uma 'Parte'), têm entre si, justo e contratado, a prestação de serviços identificada no presente contrato pelas seguintes cláusulas e condições:";
            g.DrawString(textoIntro, bodyFont, brush, new RectangleF(50, y, e.PageBounds.Width - 100, 80));
            y += 90;

            // Caixa Contratante
            g.FillRectangle(backgroundBrush, 50, y, e.PageBounds.Width - 100, 25);
            g.DrawString("CONTRATANTE:", titleFont, Brushes.White, 55, y + 5);
            y += 30;
            g.DrawRectangle(Pens.Black, 50, y, e.PageBounds.Width - 100, 70);
            g.DrawString($"Nome: {contratante}", bodyFont, brush, 55, y + 5);
            g.DrawString($"CNPJ: {contratanteCNPJ}", bodyFont, brush, 55, y + 25);
            g.DrawString($"Endereço: {contratanteEndereco}", bodyFont, brush, 55, y + 45);
            y += 80;

            // Caixa Contratada
            g.FillRectangle(backgroundBrush, 50, y, e.PageBounds.Width - 100, 25);
            g.DrawString("CONTRATADA:", titleFont, Brushes.White, 55, y + 5);
            y += 30;
            g.DrawRectangle(Pens.Black, 50, y, e.PageBounds.Width - 100, 70);
            g.DrawString($"Nome: {contratada}", bodyFont, brush, 55, y + 5);
            g.DrawString($"CNPJ/CPF: {contratadaCNPJ}", bodyFont, brush, 55, y + 25);
            g.DrawString($"Endereço: {contratadaEndereco}", bodyFont, brush, 55, y + 45);
            y += 80;

            // Cláusulas
            g.DrawString("Cláusulas do Contrato:", headerFont, brush, 50, y);
            y += 30;

            foreach (string clausula in clausulas)
            {
                if (clausula.TrimStart().StartsWith("Cláusula", StringComparison.OrdinalIgnoreCase))
                {
                    // Extrai a palavra "Cláusula" e o restante do texto
                    string[] partes = clausula.Split(new[] { ' ' }, 2); // Divide o texto após a palavra "Cláusula"
                    string palavraNegrito = partes[0];
                    string restanteTexto = partes.Length > 1 ? partes[1] : "";

                    // Calcula a posição inicial para o texto regular
                    SizeF tamanhoPalavraNegrito = g.MeasureString(palavraNegrito + " ", boldBodyFont);

                    // Desenha a palavra "Cláusula" em negrito
                    g.DrawString(palavraNegrito, boldBodyFont, brush, 55, y);

                    // Desenha o restante do texto em fonte regular
                    g.DrawString(restanteTexto, bodyFont, brush, 55 + tamanhoPalavraNegrito.Width, y);
                }
                else
                {
                    // Desenha linhas que não começam com "Cláusula" normalmente
                    g.DrawString(clausula, bodyFont, brush, 55, y);
                }

                // Calcula a altura do texto e ajusta a posição vertical
                SizeF clausulaSize = g.MeasureString(clausula, bodyFont, e.PageBounds.Width - 100);
                y += clausulaSize.Height + 5; // Adiciona um pequeno espaçamento entre as cláusulas
            }

            // Adiciona espaço extra após todas as cláusulas
            y += 40;

            // Linhas de Assinatura
            float signatureLineY = y; // Posição vertical ajustada dinamicamente
            float firstSignatureX = 50; // Posição horizontal inicial da linha de assinatura do Contratante
            float secondSignatureX = e.PageBounds.Width - 400; // Posição horizontal inicial da linha de assinatura do Contratada

            float signatureWidth = 300; // Largura das linhas de assinatura

            // Linha de assinatura da Contratante
            g.DrawLine(Pens.Black, firstSignatureX, signatureLineY, firstSignatureX + signatureWidth, signatureLineY);
            g.DrawString("Assinatura Contratante", bodyFont, brush, firstSignatureX + (signatureWidth / 4), signatureLineY + 10);

            // Linha de assinatura da Contratada
            g.DrawLine(Pens.Black, secondSignatureX, signatureLineY, secondSignatureX + signatureWidth, signatureLineY);
            g.DrawString("Assinatura Contratada", bodyFont, brush, secondSignatureX + (signatureWidth / 4), signatureLineY + 10);

            e.HasMorePages = false;

        }


    }
}

