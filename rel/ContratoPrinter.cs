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
        private DateTime dataInicio;
        private DateTime dataTermino;
        private string[] clausulas;
        private decimal valorContrato;
        private string obs;
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
                string[] clausulas,
                DateTime dataInicio,
                DateTime dataTermino,
                string obs)
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
            this.dataInicio = dataInicio;
            this.dataTermino = dataTermino;
            this.obs = obs;            
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
            g.DrawString("INSTRUMENTO PARTICULAR DE PRESTAÇÃO DE SERVIÇOS E OUTRAS AVENÇAS",
                         headerFont, brush, new RectangleF(e.MarginBounds.Left, y, this.eMarginBoundsWidth, 50));
            y += 60;

            // Texto introdutório
            string textoIntro = "Por este instrumento particular (o 'Contrato'), a CONTRATANTE e a CONTRATADA, ambas identificadas no Quadro Resumo a seguir (em conjunto, as 'Partes' e, individualmente, uma 'Parte'), têm entre si, justo e contratado, a prestação de serviços identificada no presente contrato pelas seguintes cláusulas e condições:";
            SizeF textoIntroSize = g.MeasureString(textoIntro, bodyFont, this.eMarginBoundsWidth - 10);
            g.DrawString(textoIntro, bodyFont, brush, new RectangleF(e.MarginBounds.Left, y, this.eMarginBoundsWidth, textoIntroSize.Height));
            y += textoIntroSize.Height + 20;

            // Seções do contrato
            ImprimirSecao(g, e, "DESCRIÇÃO DO CONTRATO:", descricaoContrato, titleFont, bodyFont, backgroundBrush, brush, ref y);
            ImprimirSecao(g, e, "CONTRATANTE:", $"Nome: {contratante}\nCNPJ: {contratanteCNPJ}\nEndereço: {contratanteEndereco}",
                          titleFont, bodyFont, backgroundBrush, brush, ref y);
            ImprimirSecao(g, e, "CONTRATADA:", !string.IsNullOrWhiteSpace(nomeEmpresa)
                          ? $"Empresa: {nomeEmpresa}\nCNPJ: {cnpjEmpresa}\nCPF: {contratadaCPF}\nEndereço: {contratadaEndereco}"
                          : $"Nome: {contratada}\nCPF: {contratadaCPF}\nEndereço: {contratadaEndereco}",
                          titleFont, bodyFont, backgroundBrush, brush, ref y);
            ImprimirSecao(g, e, "VALOR DO CONTRATO:", $"R$ {valorContrato:F2}", titleFont, bodyFont, backgroundBrush, brush, ref y);
            ImprimirSecao(g, e, "PERÍODO DO CONTRATO:", $"Início: {dataInicio:dd/MM/yyyy}  -  Término: {dataTermino:dd/MM/yyyy}",
                          titleFont, bodyFont, backgroundBrush, brush, ref y);

            // **Observações do contrato**
            if (!string.IsNullOrWhiteSpace(this.obs))
            {
                ImprimirSecao(g, e, "OBSERVAÇÕES:", this.obs, titleFont, bodyFont, backgroundBrush, brush, ref y);
            }

            // **Cláusulas do contrato**
            g.DrawString("Cláusulas do Contrato:", headerFont, brush, e.MarginBounds.Left, y);
            y += 30;


            ImprimirClausulas(g, e, e.MarginBounds.Left, ref y, this.eMarginBoundsWidth, bodyFont, brush);

            y += 40;

            // Linhas de assinatura
            float signatureLineY = y;
            float signatureWidth = 300;

            g.DrawLine(Pens.Black, e.MarginBounds.Left, signatureLineY, e.MarginBounds.Left + signatureWidth, signatureLineY);
            g.DrawString("Assinatura Contratante", bodyFont, brush, e.MarginBounds.Left + (signatureWidth / 4), signatureLineY + 10);

            float espacamentoAssinaturas = 200;
            float assinaturaContratadaX = e.MarginBounds.Left + signatureWidth + espacamentoAssinaturas;
            g.DrawLine(Pens.Black, assinaturaContratadaX, signatureLineY, assinaturaContratadaX + signatureWidth, signatureLineY);
            g.DrawString("Assinatura Contratada", bodyFont, brush, assinaturaContratadaX + (signatureWidth / 4), signatureLineY + 10);

            e.HasMorePages = false;
        }

        // Método auxiliar para imprimir seções formatadas do contrato
        private void ImprimirSecao(Graphics g, PrintPageEventArgs e, string titulo, string conteudo, Font titleFont, Font bodyFont, Brush backgroundBrush, Brush brush, ref float y)
        {
            g.FillRectangle(backgroundBrush, e.MarginBounds.Left, y, this.eMarginBoundsWidth, 25);
            g.DrawString(titulo, titleFont, Brushes.White, e.MarginBounds.Left + 5, y + 5);
            y += 30;

            SizeF conteudoSize = g.MeasureString(conteudo, bodyFont, this.eMarginBoundsWidth - 10);
            g.DrawRectangle(Pens.Black, e.MarginBounds.Left, y, this.eMarginBoundsWidth, conteudoSize.Height + 10);
            g.DrawString(conteudo, bodyFont, brush, new RectangleF(e.MarginBounds.Left + 5, y + 5, this.eMarginBoundsWidth - 10, conteudoSize.Height));
            y += conteudoSize.Height + 20;
        }

        // Método para imprimir as cláusulas dentro da mesma largura das outras caixas
        private void ImprimirClausulas(Graphics g, PrintPageEventArgs e, float x, ref float y, float larguraCaixa, Font bodyFont, Brush brush)
        {
            foreach (string clausula in clausulas)
            {
                if (y + 50 > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                // Define a largura do texto para corresponder às caixas
                int larguraTexto = (int)(larguraCaixa - 10);

                SizeF clausulaSize = g.MeasureString(clausula, bodyFont, larguraTexto);
                g.DrawString(clausula, bodyFont, brush, new RectangleF(x + 5, y, larguraTexto, clausulaSize.Height));

                y += clausulaSize.Height + 5;
            }
        }        

    }
}

