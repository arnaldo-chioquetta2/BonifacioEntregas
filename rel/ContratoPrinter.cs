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
            Brush brush = Brushes.Black;
            Font boldBodyFont = new Font("Arial", 12, FontStyle.Bold);
            float y = 50;

            // Título
            g.DrawString("INSTRUMENTO PARTICULAR DE PRESTAÇÃO DE SERVIÇOS E OUTRAS AVENÇAS", headerFont, brush, new RectangleF(50, y, e.PageBounds.Width - 100, 50));
            y += 60;

            // Contratante
            g.DrawString("CONTRATANTE:", headerFont, brush, 50, y);
            y += 30;
            g.DrawString($"Nome: {contratante}", bodyFont, brush, 50, y);
            y += 20;
            g.DrawString($"CNPJ: {contratanteCNPJ}", bodyFont, brush, 50, y);
            y += 20;
            g.DrawString($"Endereço: {contratanteEndereco}", bodyFont, brush, 50, y);
            y += 40;

            // Contratada
            g.DrawString("CONTRATADA:", headerFont, brush, 50, y);
            y += 30;
            g.DrawString($"Nome: {contratada}", bodyFont, brush, 50, y);
            y += 20;
            g.DrawString($"CNPJ/CPF: {contratadaCNPJ}", bodyFont, brush, 50, y);
            y += 20;
            g.DrawString($"Endereço: {contratadaEndereco}", bodyFont, brush, 50, y);
            y += 40;

            // Cláusulas
            g.DrawString("Cláusulas do Contrato:", headerFont, brush, 50, y);
            y += 30;
            int clausulaNumero = 1;
            foreach (string clausula in clausulas)
            {
                g.DrawString($"Cláusula {clausulaNumero}: ", boldBodyFont, brush, 50, y); // Cláusula em negrito
                g.DrawString(clausula, bodyFont, brush, 150, y); // Descrição da cláusula
                y += 20;
                clausulaNumero++;
            }

            y += 40;

            // Linhas de Assinatura
            float signatureLineY = y; // Posição vertical das linhas de assinatura
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
