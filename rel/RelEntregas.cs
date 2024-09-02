using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio.rel
{
    public partial class RelEntegas : Form
    {

        private bool ativou = false;
        private EntregasDAO entregasDAO;

        private List<Lanctos> rel { get; set; }
        public DateTime Data { get; internal set; }

        public RelEntegas()
        {
            InitializeComponent();
            SetStartPosition();
            rt.AdjustFormComponents(this);
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private List<Lanctos> CarregaRelEntregas()
        {
            DateTime? dataInicio = dtpDataIN.Value;
            DateTime dataFim = dtnDtFim.Value;
            string SQL = $@"SELECT DateValue(Data) AS Data,
                            SUM(VlNota - Desconto) AS Vendas,
                            SUM(Valor) AS Entregas,
                            COUNT(*) AS Qtd
                        FROM Entregas
                        Where Data Between #{ dataInicio:MM/dd/yyyy 00:00}# and #{ dataFim:MM/dd/yyyy 23:59}#
                        GROUP BY DateValue(Data)";
            List<Lanctos> lancamentos = new List<Lanctos>();
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
                                Lanctos lancamento = new Lanctos();
                                lancamento.Data = (DateTime)reader["Data"];
                                lancamento.Vendas = reader["Vendas"] != DBNull.Value ? (decimal)reader["Vendas"] : 0;
                                lancamento.Entregas = reader["Entregas"] != DBNull.Value ? (decimal)reader["Entregas"] : 0;
                                lancamento.Qtd = reader["Qtd"] != DBNull.Value ? (int)reader["Qtd"] : 0;
                                lancamentos.Add(lancamento);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            return lancamentos;
        }

        public void GerarRelEntregas()
        {
            entregasDAO = new EntregasDAO();
            DataTable dados = entregasDAO.getDados(dtpDataIN.Value, dtnDtFim.Value, null);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Relatório de Entregas");
            sb.AppendLine();
            sb.AppendLine($"Período: {dtpDataIN.Value.ToString("dd/MM/yyyy")} a {dtnDtFim.Value.ToString("dd/MM/yyyy")}");
            sb.AppendLine();
            sb.AppendLine("Data       | MotoBoy         | Valor | Desconto | Compra | Pagamento | Cliente          | Vendedor        | Obs");
            sb.AppendLine(new string('-', 100));

            HashSet<string> uniqueDates = new HashSet<string>();
            string dataAnterior = "";
            decimal vendasDiarias = 0, entregasDiarias = 0, totdesc = 0, totalVendas = 0, totalEntregas = 0;

            foreach (DataRow row in dados.Rows)
            {
                string dataAtual = Convert.ToDateTime(row["Data"]).ToString("dd/MM/yyyy");
                uniqueDates.Add(dataAtual);  // Armazena cada data única

                if (dataAtual != dataAnterior && !string.IsNullOrEmpty(dataAnterior))
                {
                    sb.AppendLine($"{dataAnterior} Vendas Total {vendasDiarias:N2} | Valores das Entregas {entregasDiarias:N2}");
                    sb.AppendLine(new string('-', 100));
                    vendasDiarias = entregasDiarias = totdesc = 0;
                }

                decimal valor = Convert.ToDecimal(row["Valor"]);
                decimal compra = Convert.ToDecimal(row["Compra"]);
                decimal desconto = Convert.ToDecimal(row["Desconto"]);

                vendasDiarias += compra;
                entregasDiarias += valor;
                totdesc += desconto;

                // Acumular totais gerais
                totalVendas += compra - desconto;
                totalEntregas += valor;

                sb.AppendFormat("{0} | {1,-15} | {2,6:N2} | {3,8:N2} | {4,6:N2} | {5,-8} | {6,-16} | {7,-15} | {8}\n",
                    dataAtual,
                    FormatField(row["MotoBoy"].ToString(), 15),
                    valor,
                    desconto,
                    compra,
                    FormatField(row["Pagamento"].ToString(), 8),
                    FormatField(row["Cliente"].ToString(), 16),
                    row["Vendedor"],
                    row["Obs"]);

                dataAnterior = dataAtual;
                sb.AppendLine("");
            }

            if (!string.IsNullOrEmpty(dataAnterior))
            {
                vendasDiarias = vendasDiarias - totdesc;
                sb.AppendLine($"{dataAnterior} Vendas Total {vendasDiarias:N2} Valores das Entregas {entregasDiarias:N2}");
                sb.AppendLine(new string('-', 100));
            }

            // Verifica se há mais de uma data e adiciona o total geral
            if (uniqueDates.Count > 1)
            {
                sb.AppendLine($"Total Geral | Vendas {totalVendas:N2} Entregas {totalEntregas:N2}");
                sb.AppendLine(new string('=', 100));
            }

            textBox1.Text = sb.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }


        //public void GerarRelEntregas()
        //{
        //    entregasDAO = new EntregasDAO();
        //    DataTable dados = entregasDAO.getDados(dtpDataIN.Value, dtnDtFim.Value);
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("Relatório de Entregas");
        //    sb.AppendLine();
        //    sb.AppendLine($"Período: {dtpDataIN.Value.ToString("dd/MM/yyyy")} a {dtnDtFim.Value.ToString("dd/MM/yyyy")}");
        //    sb.AppendLine();
        //    sb.AppendLine("Data       | MotoBoy         | Valor | Desconto | Compra | Pagamento | Cliente          | Vendedor        | Obs");
        //    sb.AppendLine(new string('-', 100));
        //    string dataAnterior = "";
        //    decimal vendasDiarias = 0;
        //    decimal entregasDiarias = 0;
        //    int quantidadeDiaria = 0;
        //    decimal totdesc = 0;
        //    foreach (DataRow row in dados.Rows)
        //    {
        //        string dataAtual = Convert.ToDateTime(row["Data"]).ToString("dd/MM/yyyy");
        //        if (dataAtual != dataAnterior && !string.IsNullOrEmpty(dataAnterior))
        //        {
        //            sb.AppendLine($"{dataAnterior} Vendas Total {vendasDiarias:N2} | Valores das Entregas {entregasDiarias:N2}");
        //            sb.AppendLine(new string('-', 100));
        //            vendasDiarias = entregasDiarias = 0;
        //        }
        //        decimal valor = Convert.ToDecimal(row["Valor"]);
        //        decimal compra = Convert.ToDecimal(row["Compra"]);
        //        decimal desconto = Convert.ToDecimal(row["Desconto"]);
        //        vendasDiarias += valor;
        //        entregasDiarias += compra;
        //        totdesc += desconto;
        //        string formattedValor = String.Format("{0,6:N2}", Convert.ToDecimal(row["Valor"]));
        //        string formattedDesconto = String.Format("{0,8:N2}", Convert.ToDecimal(row["Desconto"]));
        //        string formattedCompra = String.Format("{0,6:N2}", desconto);
        //        string formattedPagamento = FormatField(row["Pagamento"].ToString(), 8);
        //        string formattedCliente = FormatField(row["Cliente"].ToString(), 16);
        //        sb.AppendFormat("{0} | {1,-15} | {2} | {3} | {4} | {5} | {6} | {7,-15} | {8}\n",
        //            dataAtual,
        //            FormatField(row["MotoBoy"].ToString(), 15),
        //            formattedValor,
        //            formattedDesconto,
        //            formattedCompra,
        //            formattedPagamento,
        //            formattedCliente,
        //            row["Vendedor"],
        //            row["Obs"]);
        //        dataAnterior = dataAtual; // Atualizar a data anterior
        //        quantidadeDiaria += 1;
        //        sb.AppendLine("");

        //    }
        //    if (!string.IsNullOrEmpty(dataAnterior))
        //    {
        //        vendasDiarias = vendasDiarias - totdesc;
        //        sb.AppendLine($"{dataAnterior}   Vendas Total {vendasDiarias:N2} Valores das Entregas {entregasDiarias:N2}");
        //        sb.AppendLine(new string('-', 100));
        //    }
        //    textBox1.Text = sb.ToString();
        //    textBox1.SelectionStart = textBox1.Text.Length;
        //    textBox1.ScrollToCaret();
        //}

        private string FormatField(string input, int length)
        {
            return input.Length > length ? input.Substring(0, length) : input.PadRight(length);
        }

        private void Extrato_Activated(object sender, EventArgs e)
        {
            if (!ativou)
            {                
                ativou = true;
                dtnDtFim.Value = Data;
                dtpDataIN.Value = Data.AddDays(-30);
                GerarRelEntregas();

            }                            
        }
        
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            GerarRelEntregas();
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintPageHandler);
            printDocument.Print();
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Courier New", 12);
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
        
        #region Classes

        private class Lanctos
        {
            public DateTime Data;
            public decimal Vendas;
            public decimal Entregas;
            public int Qtd;            
        }

        #endregion

    }

}