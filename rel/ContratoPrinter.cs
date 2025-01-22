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
        private string nomeEmpresa; // Novo campo
        private string cnpjEmpresa; // Novo campo
        private string[] clausulas;

        public ContratoPrinter(
                string contratante,
                string contratanteCNPJ,
                string contratanteEndereco,
                string contratada,
                string contratadaCPF,
                string contratadaEndereco,
                string nomeEmpresa, // Novo parâmetro
                string cnpjEmpresa, // Novo parâmetro
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

            float scaleX = e.PageBounds.Width / baseWidth;
            float scaleY = e.PageBounds.Height / baseHeight;
            float scale = Math.Min(scaleX, scaleY);

            g.ScaleTransform(scale, scale);

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
            g.DrawRectangle(Pens.Black, 50, y, e.PageBounds.Width - 100, 100); // Aumenta altura para incluir mais campos

            if (!string.IsNullOrWhiteSpace(nomeEmpresa))
            {
                // Imprime nome da empresa, CNPJ e CPF
                g.DrawString($"Empresa: {nomeEmpresa}", bodyFont, brush, 55, y + 5);
                g.DrawString($"CNPJ: {cnpjEmpresa}", bodyFont, brush, 55, y + 25);
                g.DrawString($"CPF: {contratadaCPF}", bodyFont, brush, 55, y + 45);
                g.DrawString($"Endereço: {contratadaEndereco}", bodyFont, brush, 55, y + 65);
            }
            else
            {
                // Imprime apenas o nome, CPF e endereço
                g.DrawString($"Nome: {contratada}", bodyFont, brush, 55, y + 5);
                g.DrawString($"CPF: {contratadaCPF}", bodyFont, brush, 55, y + 25);
                g.DrawString($"Endereço: {contratadaEndereco}", bodyFont, brush, 55, y + 45);
            }

            y += 110;

            // Cláusulas do contrato
            g.DrawString("Cláusulas do Contrato:", headerFont, brush, 50, y);
            y += 30;

            foreach (string clausula in clausulas)
            {
                if (clausula.TrimStart().StartsWith("Cláusula", StringComparison.OrdinalIgnoreCase))
                {
                    string[] partes = clausula.Split(new[] { ' ' }, 2);
                    string palavraNegrito = partes[0];
                    string restanteTexto = partes.Length > 1 ? partes[1] : "";

                    SizeF tamanhoPalavraNegrito = g.MeasureString(palavraNegrito + " ", boldBodyFont);
                    g.DrawString(palavraNegrito, boldBodyFont, brush, 55, y);
                    g.DrawString(restanteTexto, bodyFont, brush, 55 + tamanhoPalavraNegrito.Width, y);
                }
                else
                {
                    g.DrawString(clausula, bodyFont, brush, 55, y);
                }

                SizeF clausulaSize = g.MeasureString(clausula, bodyFont, e.PageBounds.Width - 100);
                y += clausulaSize.Height + 5;
            }

            y += 40;

            // Linhas de assinatura
            float signatureLineY = y;
            float firstSignatureX = 50;
            float secondSignatureX = e.PageBounds.Width - 400;
            float signatureWidth = 300;

            g.DrawLine(Pens.Black, firstSignatureX, signatureLineY, firstSignatureX + signatureWidth, signatureLineY);
            g.DrawString("Assinatura Contratante", bodyFont, brush, firstSignatureX + (signatureWidth / 4), signatureLineY + 10);

            g.DrawLine(Pens.Black, secondSignatureX, signatureLineY, secondSignatureX + signatureWidth, signatureLineY);
            g.DrawString("Assinatura Contratada", bodyFont, brush, secondSignatureX + (signatureWidth / 4), signatureLineY + 10);

            e.HasMorePages = false;
        }

    }
}

