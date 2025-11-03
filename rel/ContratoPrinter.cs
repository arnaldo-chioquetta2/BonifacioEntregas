using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
// Assumindo que 'glo' e sua função Loga() estão acessíveis neste namespace
// using static SeuNamespace.SuaClasseDeLogGlobal; // Ou a forma como você acessa glo.Loga

namespace TeleBonifacio.rel
{
    public class ContratoPrinter
    {
        // --- Membros da Classe (como definidos anteriormente) ---
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
        // Removido eMarginBoundsWidth pois usamos e.MarginBounds.Width diretamente
        private int clausulaAtual = 0;
        private int paginaAtual = 1;

        // --- Construtor (como definido anteriormente) ---
        public ContratoPrinter(
            string contratante, string contratanteCNPJ, string contratanteEndereco,
            string contratada, string contratadaCPF, string contratadaEndereco,
            string nomeEmpresa, string cnpjEmpresa,
            decimal valorContrato, string descricaoContrato, string[] clausulas,
            DateTime dataInicio, DateTime dataTermino, string obs)
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
            this.clausulas = clausulas ?? new string[0]; // Garante que não seja nulo
            this.descricaoContrato = descricaoContrato;
            this.dataInicio = dataInicio;
            this.dataTermino = dataTermino;
            this.obs = obs;
        }


        // --- Método Imprimir (Atualizado com Logs e Handlers) ---
        public void Imprimir()
        {
            glo.Loga("--- Iniciando Processo Imprimir() ---");
            // Resetar estado antes de iniciar a impressão
            this.clausulaAtual = 0;
            this.paginaAtual = 1;
            glo.Loga($"Estado resetado: clausulaAtual={this.clausulaAtual}, paginaAtual={this.paginaAtual}");

            PrintDocument pd = new PrintDocument();
            pd.DocumentName = "Contrato"; // Definir um nome para o documento
            pd.PrintPage += PrintPageHandler;
            pd.EndPrint += Pd_EndPrint; // Adicionar handler para evento de fim da impressão
            pd.QueryPageSettings += Pd_QueryPageSettings; // Handler para inspecionar configurações (opcional)

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = pd,
                WindowState = FormWindowState.Maximized // Maximiza a janela de preview
            };

            try
            {
                glo.Loga("Exibindo PrintPreviewDialog...");
                previewDialog.ShowDialog(); // Dispara PrintPage para a pré-visualização
                glo.Loga("PrintPreviewDialog fechado.");
                // Clicar em "Imprimir" na pré-visualização disparará PrintPage novamente para o job real.
            }
            catch (Exception ex)
            {
                glo.Loga($"ERRO ao exibir PrintPreviewDialog ou durante impressão: {ex.ToString()}");
                MessageBox.Show($"Erro durante a impressão: {ex.Message}", "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Handlers Auxiliares para PrintDocument ---
        private void Pd_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            // Log para inspecionar configurações da página ANTES de imprimir a página
            glo.Loga($"QueryPageSettings: Margins={e.PageSettings.Margins}, Bounds={e.PageSettings.Bounds}, PrinterRes={e.PageSettings.PrinterResolution}");
        }

        private void Pd_EndPrint(object sender, PrintEventArgs e)
        {
            // Log para quando o processo de impressão termina (seja cancelado, concluído ou com erro)
            glo.Loga($"--- Impressão Finalizada: Action={e.PrintAction} ---");
            // Resetar estado aqui também pode ser uma boa prática, embora já resetado no início.
            this.clausulaAtual = 0;
            this.paginaAtual = 1;
        }


        // --- PrintPageHandler (Atualizado com Logs) ---
        // (Inclui a chamada para ImprimirClausulas e a lógica simplificada do rodapé)
        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            glo.Loga($"--- Iniciando PrintPageHandler - Página: {paginaAtual} ---");
            glo.Loga($"HasMorePages Inicial: {e.HasMorePages}");
            glo.Loga($"Bounds: {e.PageBounds}, Margins: {e.MarginBounds}");
            glo.Loga($"Cláusula Atual Inicial: {clausulaAtual}");

            Graphics g = e.Graphics;
            float y = e.MarginBounds.Top;
            float x = e.MarginBounds.Left;
            float pageHeight = e.MarginBounds.Height;
            float pageWidth = e.MarginBounds.Width;
            glo.Loga($"Graphics DPI={g.DpiX}x{g.DpiY}, PageUnit={g.PageUnit}");

            Font headerFont = new Font("Arial", 14, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 12);
            Font boldBodyFont = new Font("Arial", 12, FontStyle.Bold);
            Font titleFont = new Font("Arial", 12, FontStyle.Bold);
            Font pageNumberFont = new Font("Arial", 10);
            Brush brush = Brushes.Black;
            Brush backgroundBrush = Brushes.Gray;

            try // Envolve a lógica de desenho em try-catch
            {
                // --- Desenhar Cabeçalho e Seções (Apenas na Primeira Página) ---
                if (paginaAtual == 1)
                {
                    glo.Loga($"Página 1: Desenhando cabeçalho e seções. Y inicial={y}");
                    // Título geral
                    g.DrawString("INSTRUMENTO PARTICULAR DE PRESTAÇÃO DE SERVIÇOS E OUTRAS AVENÇAS",
                                 headerFont, brush, new RectangleF(x, y, pageWidth, 50), new StringFormat { Alignment = StringAlignment.Center });
                    y += 60;

                    // Texto introdutório
                    string textoIntro = "Por este instrumento particular (o 'Contrato'), a CONTRATANTE e a CONTRATADA, ambas identificadas no Quadro Resumo a seguir (em conjunto, as 'Partes' e, individualmente, uma 'Parte'), têm entre si, justo e contratado, a prestação de serviços identificada no presente contrato pelas seguintes cláusulas e condições:";
                    // Corrigido para usar int na largura
                    SizeF textoIntroSize = g.MeasureString(textoIntro, bodyFont, (int)(pageWidth - 10));
                    g.DrawString(textoIntro, bodyFont, brush, new RectangleF(x, y, pageWidth, textoIntroSize.Height));
                    y += textoIntroSize.Height + 20;
                    glo.Loga($"Após texto introdutório. Y={y}");

                    // Seções do contrato (Chamar ImprimirSecao para cada uma)
                    ImprimirSecao(g, e, "DESCRIÇÃO DO CONTRATO:", descricaoContrato, titleFont, bodyFont, backgroundBrush, brush, ref y, pageWidth);
                    if (e.HasMorePages) { glo.Loga("Page break DENTRO de ImprimirSecao para Descrição"); return; } // Verifica se seção causou page break

                    ImprimirSecao(g, e, "CONTRATANTE:", $"Nome: {contratante}\nCNPJ: {contratanteCNPJ}\nEndereço: {contratanteEndereco}", titleFont, bodyFont, backgroundBrush, brush, ref y, pageWidth);
                    if (e.HasMorePages) { glo.Loga("Page break DENTRO de ImprimirSecao para Contratante"); return; }

                    ImprimirSecao(g, e, "CONTRATADA:", !string.IsNullOrWhiteSpace(nomeEmpresa)
                                         ? $"Empresa: {nomeEmpresa}\nCNPJ: {cnpjEmpresa}\nCPF: {contratadaCPF}\nEndereço: {contratadaEndereco}"
                                         : $"Nome: {contratada}\nCPF: {contratadaCPF}\nEndereço: {contratadaEndereco}",
                                         titleFont, bodyFont, backgroundBrush, brush, ref y, pageWidth);
                    if (e.HasMorePages) { glo.Loga("Page break DENTRO de ImprimirSecao para Contratada"); return; }

                    ImprimirSecao(g, e, "VALOR DO CONTRATO:", $"R$ {valorContrato:F2}", titleFont, bodyFont, backgroundBrush, brush, ref y, pageWidth);
                    if (e.HasMorePages) { glo.Loga("Page break DENTRO de ImprimirSecao para Valor"); return; }

                    ImprimirSecao(g, e, "PERÍODO DO CONTRATO:", $"Início: {dataInicio:dd/MM/yyyy}  -  Término: {dataTermino:dd/MM/yyyy}", titleFont, bodyFont, backgroundBrush, brush, ref y, pageWidth);
                    if (e.HasMorePages) { glo.Loga("Page break DENTRO de ImprimirSecao para Período"); return; }

                    ImprimirSecao(g, e, "OBSERVAÇÕES:", this.obs, titleFont, bodyFont, backgroundBrush, brush, ref y, pageWidth);
                    if (e.HasMorePages) { glo.Loga("Page break DENTRO de ImprimirSecao para Observações"); return; }

                    glo.Loga($"Após seções. Y={y}");

                    // Título das Cláusulas
                    g.DrawString("Cláusulas do Contrato:", headerFont, brush, x, y);
                    y += 30;
                    glo.Loga($"Após título das cláusulas. Y={y}");
                }
                else
                {
                    glo.Loga($"Página {paginaAtual}: Iniciando diretamente nas cláusulas. Y inicial={y}");
                }

                // Verificação extra: se HasMorePages já é true (p.ex., vindo de ImprimirSecao), não prosseguir
                if (e.HasMorePages)
                {
                    glo.Loga("e.HasMorePages já é true antes de imprimir cláusulas/rodapé. Retornando.");
                    DesenharNumeroPagina(g, e, pageNumberFont, brush); // Desenha número da página mesmo assim
                    return;
                }

                // --- Imprimir Cláusulas ---
                glo.Loga($"Chamando ImprimirClausulas. Y atual={y}, clausulaAtual={clausulaAtual}");
                ImprimirClausulas(g, e, x, ref y, pageWidth, bodyFont, brush);
                glo.Loga($"Retornou de ImprimirClausulas. Y={y}, HasMorePages={e.HasMorePages}");


                // --- Desenhar Rodapé (Apenas se for REALMENTE a última página APÓS cláusulas) ---
                if (!e.HasMorePages)
                {
                    glo.Loga("É a última página após cláusulas. Tentando desenhar rodapé.");
                    // Lógica Simplificada do Rodapé
                    float espacoMinimoRodape = 80; // Espaço mínimo estimado para assinaturas + data

                    if (y < e.MarginBounds.Bottom - espacoMinimoRodape)
                    {
                        glo.Loga($"Espaço suficiente para rodapé (Y={y}, Bottom={e.MarginBounds.Bottom}). Desenhando...");
                        y += 50; // Espaço antes das assinaturas

                        float signatureLineY = y;
                        float signatureWidth = (pageWidth / 2) - 50;
                        float espacamentoAssinaturas = 100;

                        // Assinatura Contratante
                        g.DrawLine(Pens.Black, x, signatureLineY, x + signatureWidth, signatureLineY);
                        g.DrawString("Assinatura Contratante", bodyFont, brush, x + (signatureWidth - g.MeasureString("Assinatura Contratante", bodyFont).Width) / 2, signatureLineY + 5);

                        // Assinatura Contratada
                        float assinaturaContratadaX = x + signatureWidth + espacamentoAssinaturas;
                        // Verifica se a segunda assinatura cabe na largura
                        if (assinaturaContratadaX + signatureWidth <= e.MarginBounds.Right)
                        {
                            g.DrawLine(Pens.Black, assinaturaContratadaX, signatureLineY, assinaturaContratadaX + signatureWidth, signatureLineY);
                            g.DrawString("Assinatura Contratada", bodyFont, brush, assinaturaContratadaX + (signatureWidth - g.MeasureString("Assinatura Contratada", bodyFont).Width) / 2, signatureLineY + 5);
                        }
                        else
                        {
                            glo.Loga("AVISO: Segunda assinatura não cabe na largura da página.");
                        }


                        y += 40; // Espaço após assinaturas (estimado)

                        // Data por extenso
                        if (y < e.MarginBounds.Bottom - 20) // Verifica espaço para data
                        {
                            string dataExtenso = $"Porto Alegre, {DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("pt-BR"))}";
                            SizeF dataSize = g.MeasureString(dataExtenso, bodyFont);
                            float dataX = x + (pageWidth - dataSize.Width) / 2; // Centraliza data
                            g.DrawString(dataExtenso, bodyFont, brush, dataX, y);
                            glo.Loga("Rodapé desenhado.");
                        }
                        else
                        {
                            glo.Loga("AVISO: Espaço insuficiente para desenhar data.");
                        }

                    }
                    else
                    {
                        glo.Loga("AVISO: Espaço insuficiente detectado para desenhar rodapé completo.");
                        // Nesta versão simplificada, não forçamos uma nova página apenas para o rodapé.
                        // Se quiser isso, a lógica precisaria ser mais complexa com flags.
                    }
                }


                // --- Desenhar Número da Página (Em todas as páginas) ---
                DesenharNumeroPagina(g, e, pageNumberFont, brush);

            }
            catch (Exception ex)
            {
                glo.Loga($"!!!! ERRO DENTRO do PrintPageHandler: {ex.ToString()}");
                e.Cancel = true; // Tenta cancelar o job de impressão
            }
            finally // Garante que logs finais e incremento de página ocorram
            {
                glo.Loga($"--- Finalizando PrintPageHandler - Página: {paginaAtual} ---");
                glo.Loga($"HasMorePages Final: {e.HasMorePages}");
                glo.Loga($"Cláusula Atual Final: {clausulaAtual}");

                // Incrementa o número da página SOMENTE SE o evento indicar que há mais páginas
                if (e.HasMorePages)
                {
                    // Verifica se o incremento já foi feito por outra lógica (pouco provável aqui)
                    // Esta é a forma padrão: o handler terminou, e o sistema pede a próxima página.
                    paginaAtual++;
                    glo.Loga($"Incrementando paginaAtual para {paginaAtual}");
                }
            }
        }

        // --- Método ImprimirSecao (Atualizado com Logs e Verificação de Espaço) ---
        private void ImprimirSecao(Graphics g, PrintPageEventArgs e, string titulo, string conteudo, Font titleFont, Font bodyFont, Brush backgroundBrush, Brush brush, ref float y, float larguraDisponivel)
        {
            glo.Loga($"ImprimirSecao: Título='{titulo}', Y inicial={y}");
            float alturaTitulo = 25;
            // Corrigido para usar int na largura
            SizeF conteudoSize = g.MeasureString(conteudo ?? "", bodyFont, (int)(larguraDisponivel - 10));
            float alturaConteudo = conteudoSize.Height + 10; // Borda + Padding
            float alturaTotalSecao = alturaTitulo + 5 + alturaConteudo + 20; // Título + espaço + Conteúdo + espaço final
            glo.Loga($"Altura calculada da seção: {alturaTotalSecao}");

            // Verifica se a seção INTEIRA cabe. Se não, vai para a próxima página.
            if (y + alturaTotalSecao > e.MarginBounds.Bottom)
            {
                glo.Loga($"Seção '{titulo}' não cabe na página {paginaAtual} (Y={y}, Altura={alturaTotalSecao}, Limite={e.MarginBounds.Bottom}). Definindo HasMorePages=true.");
                e.HasMorePages = true;
                return; // Não desenha esta seção nesta página, PrintPageHandler será chamado de novo
            }

            // Desenha Título da Seção
            g.FillRectangle(backgroundBrush, e.MarginBounds.Left, y, larguraDisponivel, alturaTitulo);
            g.DrawString(titulo, titleFont, Brushes.White, e.MarginBounds.Left + 5, y + (alturaTitulo - titleFont.GetHeight()) / 2); // Centraliza verticalmente
            y += alturaTitulo + 5;

            // Desenha Conteúdo da Seção
            g.DrawRectangle(Pens.Black, e.MarginBounds.Left, y, larguraDisponivel, alturaConteudo);
            g.DrawString(conteudo ?? "", bodyFont, brush, new RectangleF(e.MarginBounds.Left + 5, y + 5, larguraDisponivel - 10, conteudoSize.Height));
            y += alturaConteudo + 20;
            glo.Loga($"Seção '{titulo}' impressa. Y final={y}");
        }


        // --- Método ImprimirClausulas (Atualizado com Logs) ---
        private void ImprimirClausulas(Graphics g, PrintPageEventArgs e, float x, ref float y, float larguraCaixa, Font bodyFont, Brush brush)
        {
            int larguraTexto = (int)(larguraCaixa - 10); // Desconta padding
            float margemFinalDeSeguranca = 5; // Margem mínima no final da página

            glo.Loga($"ImprimirClausulas: Página {paginaAtual}, Iniciando da cláusula {clausulaAtual + 1}/{clausulas.Length}, Y inicial={y}");

            while (clausulaAtual < clausulas.Length)
            {
                string clausulaTexto = clausulas[clausulaAtual];
                // Adiciona "Cláusula X: " ao texto a ser medido e impresso
                string clausulaFormatada = $"Cláusula {clausulaAtual + 1}: {clausulaTexto}";
                glo.Loga($"Processando cláusula {clausulaAtual + 1}: '{clausulaTexto.Substring(0, Math.Min(clausulaTexto.Length, 30))}...'");

                // Mede a altura da cláusula formatada
                SizeF clausulaSize = g.MeasureString(clausulaFormatada, bodyFont, larguraTexto);
                float alturaNecessaria = clausulaSize.Height + 5; // Altura do texto + pequeno espaço abaixo
                glo.Loga($"Altura necessária: {alturaNecessaria}");

                // Verifica se a cláusula CABE na página atual
                if (y + alturaNecessaria + margemFinalDeSeguranca > e.MarginBounds.Bottom)
                {
                    glo.Loga($"Cláusula {clausulaAtual + 1} não cabe na página {paginaAtual}. Y={y}, Altura={alturaNecessaria}, Limite={e.MarginBounds.Bottom}. Definindo HasMorePages=true.");
                    e.HasMorePages = true;
                    return; // Interrompe a impressão de cláusulas NESTA PÁGINA
                }

                // Desenha a cláusula formatada
                g.DrawString(clausulaFormatada, bodyFont, brush, new RectangleF(x + 5, y, larguraTexto, clausulaSize.Height));
                glo.Loga($"Cláusula {clausulaAtual + 1} impressa. Y antes={y}");

                y += alturaNecessaria; // Move Y para a próxima posição
                glo.Loga($"Y depois={y}");
                clausulaAtual++;      // Avança para a próxima cláusula A SER PROCESSADA
            }

            // Se o loop terminou, significa que TODAS as cláusulas foram processadas (ou nesta página ou em anteriores)
            glo.Loga($"Fim do loop de cláusulas na página {paginaAtual}. Todas as cláusulas processadas (clausulaAtual={clausulaAtual}). Definindo HasMorePages=false.");
            e.HasMorePages = false; // Sinaliza que não há mais cláusulas para as próximas chamadas de PrintPage
        }

        // --- Função auxiliar para desenhar número da página ---
        private void DesenharNumeroPagina(Graphics g, PrintPageEventArgs e, Font font, Brush brush)
        {
            string textoPagina = $"Página {paginaAtual}";
            SizeF pageSize = g.MeasureString(textoPagina, font);
            // Posiciona no canto inferior direito da margem
            float pageNumX = e.MarginBounds.Right - pageSize.Width - 5; // Pequeno offset da direita
            float pageNumY = e.MarginBounds.Bottom - pageSize.Height;
            g.DrawString(textoPagina, font, brush, pageNumX, pageNumY);
            glo.Loga($"Número da página {paginaAtual} desenhado em ({pageNumX},{pageNumY})");
        }

    } // Fim da classe ContratoPrinter
} // Fim do namespace