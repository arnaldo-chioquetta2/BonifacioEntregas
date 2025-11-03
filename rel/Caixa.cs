using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.dao;

// 3.9.9 Operador Caixa não vê lançamentos de outros
// 3.9.7 Correção da forma de pagamento Vale no relatório de caixa

namespace TeleBonifacio.rel
{
    public partial class Caixa : Form
    {

        private bool ativou = false;
        public string txtForma="";

        private List<Lanctos> relcaixa { get; set; }
        public DateTime DT1 { get; set; }
        public DateTime DT2 { get; set; }
        public int Forma { get; internal set; }

        Dictionary<int, int> mapaFormas = null;

        public Caixa()
        {
            InitializeComponent();
            SetStartPosition();
            FormasDAO cFormas = new FormasDAO();
            glo.CarregarComboBox<tb.Forma>(cmbTipo, cFormas, " ");
            rt.AdjustFormComponents(this);
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }        

        private List<int> CarregaIdsCaixa() 
        {
            List<int> ids = new List<int>();
            DateTime dataInicio = this.DT1.Date;
            DateTime dataFim = this.DT2.Date.AddDays(1).AddSeconds(-1);
            string filtroForma = ObterFiltroForma(); 

            string SQL = $@"SELECT C.ID
                FROM Caixa C                
                WHERE C.Data BETWEEN #{dataInicio:yyyy-MM-dd HH:mm:ss}# AND #{dataFim:yyyy-MM-dd HH:mm:ss}#{filtroForma}";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(SQL, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ids.Add(Convert.ToInt32(reader["ID"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar IDs do caixa: {ex.Message}");
            }

            return ids;
        }

        private string ObterFiltroForma()
        {
            if (cmbTipo.SelectedIndex > 0)
            {
                int idFormaSelecionada = ((tb.ComboBoxItem)cmbTipo.SelectedItem).Id;
                return $" AND C.idForma = {idFormaSelecionada - 1}";
            }
            return "";
        }        

        private Lanctos CriarLancamento(OleDbDataReader reader)
        {
            int idFormaCaixa = Convert.ToInt32(reader["FormaPagto"]);
            int idFormaReal = 0;
            if (idFormaCaixa==22)
            {
                idFormaReal = 23;
            } else {
                idFormaReal = mapaFormas.ContainsKey(idFormaCaixa) ? mapaFormas[idFormaCaixa] : idFormaCaixa + 1;
            }            
            var lancamento = new Lanctos
            {
                ID = Convert.ToInt32(reader["ID"]),
                DataPagamento = Convert.ToDateTime(reader["Data"]),
                Desconto = Convert.ToDecimal(reader["Desconto"]),
                idFormaPagto = idFormaReal,
                Entrada = 0,
                Saida = 0,
                Obs = reader["Obs"].ToString()
            };
            decimal valor = Convert.ToDecimal(reader["Valor"]);
            object oFormaTipo = reader["FormaTipo"];
            int formaTipo = 0;
            try
            {
                formaTipo = Convert.ToInt32(oFormaTipo);
            }
            catch (Exception)
            {

            }
            if (formaTipo == 1)
            {
                lancamento.Saida = valor;
            }
            else
            {
                lancamento.Entrada = valor;
            }
            lancamento.Saldo = lancamento.Entrada - lancamento.Desconto - lancamento.Saida;
            return lancamento;
        }

        public void GerarRelCaixa()
        {
            var formas = CarregaFormas();
            int? formaPagamentoFiltro = null;
            if (cmbTipo.SelectedIndex > 0) // Certifique-se de que cmbTipo é o nome correto do ComboBox de filtro
            {
                formaPagamentoFiltro = ((tb.ComboBoxItem)cmbTipo.SelectedItem).Id;
            }
            this.DT1 = dtpDataIN.Value.Date;
            this.DT2 = dtnDtFim.Value.Date.AddHours(23).AddMinutes(59); // Certifique-se de que dtnDtFim é o nome correto

            // 1. Carrega apenas os IDs dos lançamentos
            var idsLancamentos = CarregaIdsCaixa(); // Chama o método modificado
            // var formas = CarregaFormas(); // Carrega a lista de todas as formas de pagamento

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Extrato de Movimentação do Caixa");
            sb.AppendLine();
            sb.AppendLine($"Período: {this.DT1:dd/MM/yyyy} a {this.DT2:dd/MM/yyyy}");

            var formaSelecionada = formaPagamentoFiltro.HasValue
                ? formas.FirstOrDefault(f => f.Id == formaPagamentoFiltro.Value)
                : null;

            if (formaSelecionada != null)
            {
                sb.AppendLine($"Forma de Pagamento: {formaSelecionada.Nome}");
            }
            sb.AppendLine();
            sb.AppendLine("ID       Data     |  Entrada | Desconto |  Saídas |  FormaPagto |   Valor   | Observação");
            decimal totalEntradas = 0m;
            decimal totalSaidas = 0m;

            // 2. Itera pelos IDs
            foreach (var idLancamento in idsLancamentos)
            {
                // 3. Para cada ID, carrega os detalhes completos
                var lancos = CarregaLancamentoPorId(idLancamento);

                // Verifica se o lançamento foi carregado com sucesso
                if (lancos == null)
                {
                    // Opcional: Logar ou pular registros não encontrados
                    System.Diagnostics.Debug.WriteLine($"Aviso: Lançamento com ID {idLancamento} não encontrado ou erro ao carregar.");
                    continue; // Pula para o próximo ID
                }

                // 4. Aplica o filtro de forma de pagamento (se houver)
                if (formaPagamentoFiltro.HasValue && lancos.idFormaPagto != formaPagamentoFiltro.Value)
                {
                    continue; // Pula este lançamento se não corresponder ao filtro
                }

                // 5. Busca o objeto da forma de pagamento (com a gambiarra, se ainda for necessária)
                int idFormaCorreto = lancos.idFormaPagto;
                System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - idFormaPagto carregado (candidato a ID correto): {lancos.idFormaPagto}");

                if (lancos.idFormaPagto == 23)
                {
                    idFormaCorreto = 23; // Explicitamente mantem como 23 (VALE)
                    System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - ID 23 mantido como VALE (ID 23 na tabela Formas). Nenhuma correcao aplicada.");
                }
                else
                {
                    idFormaCorreto = lancos.idFormaPagto;
                    System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - ID {lancos.idFormaPagto} assumido como correto na tabela Formas.");
                }

                var formaPagamento = formas.FirstOrDefault(f => f.Id == idFormaCorreto);

                // Verificação de segurança (opcional)
                if (formaPagamento == null)
                {
                    formaPagamento = new tb.Forma { Id = idFormaCorreto, Nome = "DESCONHECIDO" };
                }


                // 6. Formata e adiciona a linha ao relatório
                string ID = glo.ComplStr(lancos.ID.ToString(), 5, 2);
                string Data = glo.ComplStr(lancos.DataPagamento.ToString("dd/MM/yyyy"), 10, 2);
                string Entrada = glo.ComplStr(lancos.Entrada.ToString("N2"), 8, 3);
                string Desconto = glo.ComplStr(lancos.Desconto.ToString("N2"), 8, 3);
                string Saidas = glo.ComplStr(lancos.Saida.ToString("N2"), 7, 3);
                string Forma = glo.ComplStr(formaPagamento.Nome, 11, 2); // Usa o nome da forma ajustada
                string Valor = glo.ComplStr((lancos.Entrada - lancos.Desconto - lancos.Saida).ToString("N2"), 9, 2);
                string Obs = lancos.Obs.Substring(0, Math.Min(lancos.Obs.Length, 20));

                sb.AppendLine($"{ID}   {Data}   {Entrada}   {Desconto}   {Saidas}   {Forma}   {Valor} {Obs}");

                // 7. Atualiza os totais
                totalEntradas += lancos.Entrada;
                totalSaidas += lancos.Saida;
            }

            // 8. Finaliza o relatório (restante do código permanece igual)
            sb.AppendLine();
            decimal totalLiquido = totalEntradas - totalSaidas;

            if (formaSelecionada != null)
            {
                string nome = formaSelecionada.Nome.PadRight(11);
                string valorFormatado = Math.Abs(totalLiquido).ToString("N2").PadLeft(12);
                sb.AppendLine($"{nome}: {valorFormatado}");
            }
            else
            {
                GerarTotaisFormas(sb, idsLancamentos, formas, formaPagamentoFiltro);
            }

            sb.AppendLine("");
            sb.AppendLine("Total de entradas:" + glo.ComplStr(totalEntradas.ToString("N2"), 10, 2));
            sb.AppendLine("Total de saídas  :" + glo.ComplStr(totalSaidas.ToString("N2"), 10, 2));
            sb.AppendLine("Saldo            :" + glo.ComplStr(totalLiquido.ToString("N2"), 10, 2));

            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        // Processamento INICIO
        // Refatorado em 19/04/23 Original 88 linhas, resultado 65 linhas
        private void GerarTotaisFormas(StringBuilder sb, List<int> idsLancamentos, List<tb.Forma> formas, int? formaPagamentoFiltro)
        {
            // Se houver um filtro de forma de pagamento, não é necessário calcular os totais gerais
            if (formaPagamentoFiltro.HasValue)
            {
                System.Diagnostics.Debug.WriteLine("[Totais] Filtro de forma aplicado, saindo do metodo sem calcular totais gerais.");
                return;
            }

            // Verifica se as listas de entrada sao validas
            if (idsLancamentos == null || formas == null)
            {
                System.Diagnostics.Debug.WriteLine("[Totais] ERRO: Lista de IDs ou lista de Formas e nula.");
                return;
            }

            // Dicionário para acumular os totais por ID de forma
            var totaisPorForma = formas.ToDictionary(f => f.Id, _ => 0m);

            int lancamentosProcessados = 0;
            int lancamentosComErro = 0;
            int lancamentosIgnoradosForma = 0;

            // Itera pelos IDs dos lançamentos
            foreach (var idLancamento in idsLancamentos)
            {
                Lanctos lancos = CarregaLancamentoPorId(idLancamento, ref lancamentosComErro, ref lancamentosProcessados);
                if (lancos == null)
                    continue;

                AtualizaTotaisPorForma(lancos, formas, totaisPorForma, ref lancamentosIgnoradosForma);
            }

            ExibeTotaisPorForma(sb, formas, totaisPorForma);
        }

        private Lanctos CarregaLancamentoPorId(int idLancamento, ref int lancamentosComErro, ref int lancamentosProcessados)
        {
            Lanctos lancos = null;
            try
            {
                lancos = CarregaLancamentoPorId(idLancamento); // Chama o metodo existente
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Totais] ERRO AO CARREGAR lancamento ID {idLancamento}: {ex.Message}");
                lancamentosComErro++;
            }
            if (lancos == null)
            {
                System.Diagnostics.Debug.WriteLine($"[Totais] AVISO: Lancamento com ID {idLancamento} nao encontrado ou erro ao carregar (retorno null).");
                lancamentosComErro++;
            }
            else
            {
                lancamentosProcessados++;
                System.Diagnostics.Debug.WriteLine($"[Totais] Lancamento ID {idLancamento} carregado com sucesso.");
                // Opcional: Adicionar log dos dados brutos aqui se precisar depurar mais
                // System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {idLancamento} - Dados brutos: ID={lancos.ID}, idFormaPagto={lancos.idFormaPagto}, Entrada={lancos.Entrada}, Desconto={lancos.Desconto}, Saida={lancos.Saida}, Obs='{lancos.Obs}'");
            }
            return lancos;
        }

        private int ObterIdFormaCorrigido(Lanctos lancos)
        {
            int idFormaCorreto = lancos.idFormaPagto;
            if (lancos.idFormaPagto == 23)
            {
                idFormaCorreto = 23; 
            }
            else
            {
                idFormaCorreto = lancos.idFormaPagto;
            }
            return idFormaCorreto;
        }        

        private void AtualizaTotaisPorForma(Lanctos lancos, List<tb.Forma> formas, Dictionary<int, decimal> totaisPorForma, ref int lancamentosIgnoradosForma)
        {
            // Obtem o ID da forma corrigido
            int idFormaCorreto = ObterIdFormaCorrigido(lancos);

            // Busca o objeto da forma de pagamento usando o ID potencialmente ajustado
            var formaPagamento = formas.FirstOrDefault(f => f.Id == idFormaCorreto);
            System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - Forma de pagamento encontrada: {(formaPagamento != null ? $"'{formaPagamento.Nome}' (ID {formaPagamento.Id})" : "NENHUMA")}");

            if (formaPagamento != null)
            {
                decimal valorParaAcumular = CalculaValorParaAcumular(lancos, formaPagamento);
                decimal totalAntes = totaisPorForma[formaPagamento.Id];
                totaisPorForma[formaPagamento.Id] += valorParaAcumular;
                decimal totalDepois = totaisPorForma[formaPagamento.Id];
                System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - Atualizacao do total para forma '{formaPagamento.Nome}' (ID {formaPagamento.Id}): Antes={totalAntes}, Adicionado={valorParaAcumular}, Depois={totalDepois}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - AVISO: Forma de pagamento com ID corrigido {idFormaCorreto} nao encontrada na lista de formas. Valor NAO acumulado.");
                lancamentosIgnoradosForma++;
            }
        }

        private decimal CalculaValorParaAcumular(Lanctos lancos, tb.Forma formaPagamento)
        {
            decimal valorParaAcumular = 0m;
            if (formaPagamento.Id == 24 && lancos.Entrada == 0m && lancos.Saida > 0m)
            {
                valorParaAcumular = lancos.Saida; // Usa o valor de 'Saida' como entrada para PIX CPF
                System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - [CORRECAO VALOR ESPECIFICA] Forma '{formaPagamento.Nome}' (PIX CPF). Entrada=0, Saida>0. Usando Saida({lancos.Saida}) como valor de entrada.");
            }
            else
            {
                if (formaPagamento.Tipo == 0) // Entrada
                {
                    valorParaAcumular = lancos.Entrada - lancos.Desconto;
                    System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - Forma '{formaPagamento.Nome}' e ENTRADA (Tipo 0). Valor para acumular: Entrada({lancos.Entrada}) - Desconto({lancos.Desconto}) = {valorParaAcumular}");
                }
                else if (formaPagamento.Tipo == 1) // Saída
                {
                    valorParaAcumular = lancos.Saida;
                    System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - Forma '{formaPagamento.Nome}' e SAIDA (Tipo 1). Valor para acumular: Saida({lancos.Saida}) = {valorParaAcumular}");
                }
            }
            if (valorParaAcumular == 0m)
            {
                if (formaPagamento.Tipo == 0) // Entrada esperada
                {
                    if (lancos.Entrada > 0) valorParaAcumular = lancos.Entrada;
                    else if (lancos.Saida > 0) valorParaAcumular = lancos.Saida;
                    else if (lancos.Desconto > 0) valorParaAcumular = lancos.Desconto;
                    if (valorParaAcumular > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - [CORRECAO VALOR GENÉRICA] Forma '{formaPagamento.Nome}' (Entrada). Valor real encontrado em outro campo: {valorParaAcumular}");
                    }
                }
                else if (formaPagamento.Tipo == 1) // Saída esperada
                {
                    if (lancos.Saida > 0) valorParaAcumular = lancos.Saida;
                    else if (lancos.Entrada > 0) valorParaAcumular = lancos.Entrada;
                    else if (lancos.Desconto > 0) valorParaAcumular = lancos.Desconto;
                    if (valorParaAcumular > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - [CORRECAO VALOR GENÉRICA] Forma '{formaPagamento.Nome}' (Saída). Valor real encontrado em outro campo: {valorParaAcumular}");
                    }
                }
                if (valorParaAcumular == 0m)
                {
                    System.Diagnostics.Debug.WriteLine($"[Totais] Lanc ID {lancos.ID} - [CORRECAO VALOR GENÉRICA] Nao foi possivel encontrar um valor real nao-zero para forma '{formaPagamento.Nome}'. Mantendo valor 0.");
                }
            }
            return valorParaAcumular;
        }

        private void ExibeTotaisPorForma(StringBuilder sb, List<tb.Forma> formas, Dictionary<int, decimal> totaisPorForma)
        {
            System.Diagnostics.Debug.WriteLine("[Totais] --- Iniciando exibicao dos totais finais ---");
            int maxNomeLength = formas.Max(f => f.Nome.Length);
            System.Diagnostics.Debug.WriteLine($"[Totais] Comprimento maximo do nome da forma: {maxNomeLength}");

            int formasExibidas = 0;
            foreach (var forma in formas)
            {
                decimal total = totaisPorForma[forma.Id];
                System.Diagnostics.Debug.WriteLine($"[Totais] Preparando exibicao - Forma '{forma.Nome}' (ID {forma.Id}): Total={total}");

                string nome = forma.Nome.PadRight(maxNomeLength);
                string valorFormatado = total.ToString("N2").PadLeft(12);
                string linha = $"{nome}: {valorFormatado}";
                sb.AppendLine(linha);
                formasExibidas++;

                System.Diagnostics.Debug.WriteLine($"[Totais] Linha adicionada ao StringBuilder: '{linha}'");

                // Logs especificos para formas com problemas conhecidos
                if (forma.Id == 3) // TELE
                {
                    System.Diagnostics.Debug.WriteLine($"[Totais] [FORMA 3 - TELE ESPECIFICA] - Total calculado/final: {total}");
                }
                else if (forma.Id == 6) // Despesa
                {
                    System.Diagnostics.Debug.WriteLine($"[Totais] [FORMA 6 - Despesa ESPECIFICA] - Total calculado/final: {total}");
                }
                else if (forma.Id == 7) // PIX Itau PJ
                {
                    System.Diagnostics.Debug.WriteLine($"[Totais] [FORMA 7 - PIX Itau PJ ESPECIFICA] - Total calculado/final: {total}");
                }
                else if (forma.Id == 23) // VALE
                {
                    System.Diagnostics.Debug.WriteLine($"[Totais] [FORMA 23 - VALE ESPECIFICA] - Total calculado/final: {total}");
                }
                else if (forma.Id == 24) // PIX CPF
                {
                    System.Diagnostics.Debug.WriteLine($"[Totais] [FORMA 24 - PIX CPF ESPECIFICA] - Total calculado/final: {total}");
                }
            }
            System.Diagnostics.Debug.WriteLine("[Totais] --- Finalizada exibicao dos totais finais ---");
            System.Diagnostics.Debug.WriteLine($"[Totais] Numero de formas exibidas: {formasExibidas}");
        }
        // Processamento FIM

        private Dictionary<int, int> CriarMapaFormas()
        {
            Dictionary<int, int> mapa = new Dictionary<int, int>();
            string SQL = "SELECT ID FROM Formas ORDER BY ID";

            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(SQL, connection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        int index = 0;
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["ID"]);
                            mapa[index] = id;
                            index++;
                        }
                    }
                }
            }
            return mapa;
        }

        private List<tb.Forma> CarregaFormas()
        {
            string SQL = "SELECT ID, Nome, Tipo, Ativo FROM Formas WHERE Ativo = 1";
            List<tb.Forma> formas = new List<tb.Forma>();

            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(SQL, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                formas.Add(new tb.Forma
                                {
                                    Id = (int)reader["ID"],
                                    Nome = (string)reader["Nome"],
                                    Tipo = (int)reader["Tipo"],
                                    Ativo = (int)reader["Ativo"]
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return new List<tb.Forma>();
                }
            }
            return formas;
        }        

        private void Extrato_Activated(object sender, EventArgs e)
        {
            if (!ativou)
            {
                ativou = true;
                dtpDataIN.Value = this.DT1;
                dtnDtFim.Value = this.DT2.Date.AddDays(1).AddMinutes(-1);
                if (this.Forma>-1)
                {
                    cmbTipo.Text = this.txtForma;
                }
                mapaFormas = CriarMapaFormas();
                GerarRelCaixa();
            }
        }
        
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            GerarRelCaixa();
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintPageHandler);
            printDocument.Print();
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Courier New", 10);
            float yPos = 0;
            int count = 0;
            string[] lines = textBox1.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                yPos = count * font.GetHeight(e.Graphics);
                e.Graphics.DrawString(line, font, Brushes.Black, new PointF(10, yPos));
                count++;
            }
        }

        private Lanctos CarregaLancamentoPorId(int idLancamento)
        {
            try
            {
                string SQL = $@"SELECT C.ID, C.Data, C.Valor, C.Desconto, 
                        C.idForma AS FormaPagto, C.Obs, F.Tipo AS FormaTipo
                        FROM Caixa C                
                        LEFT JOIN Formas f ON F.ID = C.idForma 
                        WHERE C.ID = ?"; // Usando parâmetro para segurança

                using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(SQL, connection))
                    {
                        // Adiciona o parâmetro ID
                        command.Parameters.Add(new OleDbParameter("@ID", OleDbType.Integer) { Value = idLancamento });

                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Deve haver apenas um registro
                            {
                                // Reutiliza o método existente para criar o objeto Lanctos
                                return CriarLancamento(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Logar o erro pode ser útil para depuração
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar lançamento ID {idLancamento}: {ex.Message}");
                // Dependendo da política de erro, você pode lançar a exceção ou retornar null
                // throw; 
            }

            // Retorna null se não encontrar ou ocorrer erro
            return null;
        }

        #region Classes

        private class Lanctos
        {
            public string Forma;
            public int ID;
            public DateTime DataPagamento;
            public decimal Entrada;
            public decimal Desconto;
            public decimal Saida;
            public int idFormaPagto;
            public decimal Saldo;
            public int Quantidade;
            public string Obs;

        }

        #endregion

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ativou) {
                GerarRelCaixa();
            }            
        }
    }

}