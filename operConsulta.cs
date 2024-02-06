using SourceGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class Consultas : Form
    {

        private decimal totalGeral = 0;
        private DataTable Dados = null;

        public Consultas()
        {
            InitializeComponent();
            
        }

        private void operConsulta_Load(object sender, EventArgs e)
        {
            DateTime Agora = DateTime.Now;
            int Ano = Agora.Year;
            int Mes = Agora.Month;
            int Dia = Agora.Day+1;
            DateTime DT1 = new DateTime(Ano, 1, 1);
            DateTime DT2 = DateTime.Now;
            if (gen.IsDateTimeValid(Ano, Mes, Dia))
            {
                DT2 = new DateTime(Ano, Mes, Dia);
            }
            else
            {
                Mes++;
                if (gen.IsDateTimeValid(Ano, Mes, Dia))
                {
                    Ano++;
                    Mes = 1;
                    DT2 = new DateTime(Ano, Mes, 1);
                }                     
            }
            dtpDataIniicio.Value = DT1;
            dtpDataFim.Value = DT2;
            CarregaGrid(DT1, DT2);
            ConfigurarGrid(); 
        }

        private void CarregaGrid(DateTime? DT1 = null, DateTime? DT2 = null)
        {
            EntregasDAO entregasDAO = new EntregasDAO();
            DataTable DadosTemp = entregasDAO.getDadosC(DT1, DT2);
            if (DadosTemp.Rows.Count == 0)
            {
                dataGrid1.DataSource = null;
            } else
            {
                Dados = OrganizarDadosEmDataTable(DadosTemp);
                DevAge.ComponentModel.BoundDataView boundDataView = new DevAge.ComponentModel.BoundDataView(Dados.DefaultView);
                dataGrid1.DataSource = boundDataView;
            }
        }

        private void ConfigurarGrid()
        {
            dataGrid1.Columns[0].Width = 100;
            for (int i = 1; i < dataGrid1.Columns.Count; i++)
            {
                dataGrid1.Columns[i].Width = 90;
            }
            dataGrid1.Invalidate();
        }

#region Grid

        private DataTable OrganizarDadosEmDataTable(DataTable dados)
        {
            var dadosOrganizados = InicializarDataTable();
            var formasPagamento = ObterFormasDePagamento();
            AdicionarFormasDePagamento(dadosOrganizados, formasPagamento);
            var motoBoys = ObterMotoBoys(dados);
            AdicionarColunasMotoBoys(dadosOrganizados, motoBoys);
            var totaisEntregadores = CalcularValoresPorEntregador(dados, dadosOrganizados, formasPagamento, motoBoys);
            AdicionarLinhaTotal(dadosOrganizados, totaisEntregadores);
            AtualizarValorTotalInterface(totaisEntregadores);
            return dadosOrganizados;
        }

        private DataTable InicializarDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Forma de Pagamento", typeof(string));
            return dataTable;
        }

        private Dictionary<int, string> ObterFormasDePagamento()
        {
            return new Dictionary<int, string>
            {
                { 0, "Anotado" },
                { 1, "Cartão" },
                { 2, "Dinheiro" },
                { 3, "Pix" },
                { 4, "Troca" }
            };
        }

        private void AdicionarFormasDePagamento(DataTable dataTable, Dictionary<int, string> formasPagamento)
        {
            foreach (var forma in formasPagamento)
            {
                dataTable.Rows.Add(forma.Value);
            }
        }

        private List<string> ObterMotoBoys(DataTable dados)
        {
            return dados.AsEnumerable()
                .Select(row => row.Field<string>("MotoBoy"))
                .Distinct()
                .Where(motoBoy => !string.IsNullOrEmpty(motoBoy))
                .ToList();
        }

        private void AdicionarColunasMotoBoys(DataTable dataTable, List<string> motoBoys)
        {
            foreach (var motoBoy in motoBoys)
            {
                dataTable.Columns.Add(motoBoy, typeof(string));
            }
        }

        private Dictionary<string, decimal> CalcularValoresPorEntregador(DataTable dados, DataTable dadosOrganizados, Dictionary<int, string> formasPagamento, List<string> motoBoys)
        {
            var totaisEntregadores = motoBoys.ToDictionary(motoBoy => motoBoy, motoBoy => 0M);
            foreach (DataRow row in dados.Rows)
            {
                string motoBoy = row.Field<string>("MotoBoy");
                int idForma = row.Field<int>("idForma");
                decimal valor = row.Field<decimal>("Valor");
                DataRow linhaForma = dadosOrganizados.AsEnumerable().FirstOrDefault(r => r.Field<string>("Forma de Pagamento") == formasPagamento[idForma]);
                if (linhaForma != null && dadosOrganizados.Columns.Contains(motoBoy))
                {
                    decimal valorAtual = linhaForma.IsNull(motoBoy) ? 0 : decimal.Parse(linhaForma[motoBoy].ToString());
                    decimal valorSomado = valorAtual + valor;
                    linhaForma[motoBoy] = valorSomado.ToString("N2");
                    totaisEntregadores[motoBoy] += valor;
                }
            }
            return totaisEntregadores;
        }

        private void AdicionarLinhaTotal(DataTable dataTable, Dictionary<string, decimal> totaisEntregadores)
        {
            DataRow linhaTotal = dataTable.NewRow();
            linhaTotal["Forma de Pagamento"] = "TOTAL";
            foreach (var motoBoy in totaisEntregadores.Keys)
            {
                linhaTotal[motoBoy] = totaisEntregadores[motoBoy].ToString("N2");
            }
            dataTable.Rows.Add(linhaTotal);
        }

        private void AtualizarValorTotalInterface(Dictionary<string, decimal> totaisEntregadores)
        {
            totalGeral = totaisEntregadores.Values.Sum();
            txtValor.Text = totalGeral.ToString("N2");
            ConfigDAO config = new ConfigDAO();
            decimal perc = config.getPercentual();
            AtualizaValores(perc);
        }

        #endregion

        private void AtualizaValores(decimal perc)
        {
            txPerc.Text = perc.ToString("F2");
            decimal comiss = totalGeral * perc / 100;
            txComiss.Text = comiss.ToString("F2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CarregaGrid(dtpDataIniicio.Value, dtpDataFim.Value);
        }

        private void txPerc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                float perc = gen.LeValor(txPerc.Text);
                AtualizaValores((decimal)perc);
                ConfigDAO config = new ConfigDAO();
                config.SetPerc(perc);
            }
        }

        private void txPerc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.' && e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Left && e.KeyChar != (char)Keys.Right && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImprimirConteudo();
        }

        #region Impressão

        //private void ImprimirConteudo()
        //{
        //    PrintDocument printDocument = new PrintDocument();
        //    printDocument.DefaultPageSettings.Landscape = true;
        //    printDocument.PrintPage += new PrintPageEventHandler(OnPrintPage);
        //    PrintDialog printDialog = new PrintDialog();
        //    printDialog.Document = printDocument;
        //    if (printDialog.ShowDialog() == DialogResult.OK)
        //    {                
        //        printDocument.Print();
        //    }
        //}

        private void ImprimirConteudo()
        {
            PrintDocument printDocument = new PrintDocument();

            // Defina a orientação da página para paisagem antes de criar o PrintDialog
            printDocument.DefaultPageSettings.Landscape = true;

            printDocument.PrintPage += new PrintPageEventHandler(OnPrintPage);

            // Cria o PrintDialog e define suas configurações
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;

            // Tente definir a orientação para paisagem nas configurações do PrintDialog também
            printDialog.Document.DefaultPageSettings.Landscape = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }


        //private void OnPrintPage(object sender, PrintPageEventArgs e)
        //{
        //    Graphics graphics = e.Graphics;
        //    Font font = new Font("Arial", 10);
        //    Brush brush = Brushes.Black;
        //    float lineHeight = font.GetHeight();
        //    float x = e.MarginBounds.Left;
        //    float y = e.MarginBounds.Top;
        //    float[] columnWidths = { 200, 225, 225, 225, 225, 225 };

        //    // Supondo que o cabeçalho e a ordem das colunas do DataTable estejam corretos
        //    string[] columnNames = Dados.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();
        //    // Desenha os cabeçalhos das colunas
        //    for (int i = 0; i < columnNames.Length; i++)
        //    {
        //        graphics.DrawString(columnNames[i], font, brush, x, y);
        //        x += columnWidths[i]; // Move X para a próxima coluna
        //    }
        //    y += lineHeight; // Move para a próxima linha após o cabeçalho

        //    // Desenha os valores de cada linha
        //    foreach (DataRow row in Dados.Rows)
        //    {
        //        x = e.MarginBounds.Left; // Reseta X para o início de cada nova linha
        //        for (int i = 0; i < columnNames.Length; i++)
        //        {
        //            string text = row[i].ToString();
        //            graphics.DrawString(text, font, brush, x, y);
        //            x += columnWidths[i]; // Move X para a próxima coluna
        //        }
        //        y += lineHeight; // Move para a próxima linha após cada linha de dados
        //    }

        //    // Desenha a linha de separação
        //    y += lineHeight / 2; // Pequeno espaço antes da linha de separação
        //    graphics.DrawLine(Pens.Black, e.MarginBounds.Left, y, e.MarginBounds.Right, y);
        //    y += lineHeight / 2; // Pequeno espaço após a linha de separação

        //    // Desenha o total geral e a data da consulta
        //    graphics.DrawString($"Valor Total: {totalGeral.ToString("N2")}", font, brush, e.MarginBounds.Left, y);
        //    y += lineHeight;
        //    graphics.DrawString($"Data da Consulta: {dtpDataIniicio.Value.ToShortDateString()} até {dtpDataFim.Value.ToShortDateString()}", font, brush, e.MarginBounds.Left, y);

        //    // Verifica se há mais páginas
        //    e.HasMorePages = false; // Defina como true se precisar continuar em uma nova página
        //}

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Arial", 10);
            Brush brush = Brushes.Black;
            float lineHeight = font.GetHeight();
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float pageWidth = e.MarginBounds.Width;
            string[] columnNames = Dados.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();
            int columnCount = columnNames.Length;
            float[] columnWidths = Enumerable.Repeat(pageWidth / columnCount, columnCount).ToArray();
            columnWidths[columnWidths.Length - 1] = pageWidth - columnWidths.Take(columnWidths.Length - 1).Sum();
            for (int i = 0; i < columnCount; i++)
            {
                graphics.DrawString(columnNames[i], font, brush, x, y);
                x += columnWidths[i]; 
            }
            y += lineHeight; 
            foreach (DataRow row in Dados.Rows)
            {
                x = e.MarginBounds.Left; 
                for (int i = 0; i < columnCount; i++)
                {
                    string text = row[i].ToString();
                    graphics.DrawString(text, font, brush, x, y);
                    x += columnWidths[i]; 
                }
                y += lineHeight; 
            }
            y += lineHeight / 2; 
            graphics.DrawLine(Pens.Black, e.MarginBounds.Left, y, e.MarginBounds.Right, y);
            y += lineHeight / 2; 
            graphics.DrawString($"Valor Total: {totalGeral.ToString("N2")}", font, brush, e.MarginBounds.Left, y);
            y += lineHeight;
            graphics.DrawString($"Data da Consulta: {dtpDataIniicio.Value.ToShortDateString()} até {dtpDataFim.Value.ToShortDateString()}", font, brush, e.MarginBounds.Left, y);
            e.HasMorePages = false; 
        }


        #endregion
    }
}
