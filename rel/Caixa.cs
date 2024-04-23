using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;

namespace TeleBonifacio.rel
{
    public partial class Caixa : Form
    {

        private bool ativou = false;
        private DateTime? DataInicio { get; set; }
        private DateTime DataFim { get; set; }
        private List<Lanctos> relcaixa { get; set; }

        public Caixa()
        {
            InitializeComponent();
            SetStartPosition();
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private List<Lanctos> CarregaCaixa(DateTime? dataInicio, DateTime dataFim)
        {
            string SQL = @"SELECT C.ID, C.Data, C.Valor, C.Desconto, 
                            C.idForma AS FormaPagto
                            FROM Caixa C";
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
                                lancamento.ID = (int)reader["ID"];
                                lancamento.DataPagamento = (DateTime)reader["Data"];                                
                                lancamento.Desconto = (decimal)reader["Desconto"];
                                lancamento.idFormaPagto = (int)reader["FormaPagto"];
                                if (lancamento.idFormaPagto == 5)
                                {
                                    lancamento.Entrada = 0;
                                    lancamento.Saida = (decimal)reader["Valor"];
                                }
                                else
                                {
                                    lancamento.Entrada = (decimal)reader["Valor"];
                                    lancamento.Saida = 0;
                                }
                                lancamento.Forma = retFormaId(lancamento.idFormaPagto);
                                lancamento.Saldo = lancamento.Entrada - lancamento.Desconto - lancamento.Saida;
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

        private string retFormaId(int idForma)
        {
            string ret = "";
            switch (idForma)
            {
                case 0:
                    ret = "Dinheiro";
                    break;
                case 1:
                    ret = "Cartão";
                    break;
                case 2:
                    ret = "Anotado";
                    break;
                case 3:
                    ret = "Pix";
                    break;
                case 5:
                    ret = "Despesa";
                    break;
            }
            return ret;
        }

        public string GerarRelCaixa(DateTime? dataInicio, DateTime dataFim)
        {
            relcaixa = CarregaCaixa(dataInicio, dataFim);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Extrato de Movimentação do Caixa");
            sb.AppendLine();
            sb.AppendLine($"Período: {dataInicio?.ToString("dd/MM/yyyy")} a {dataFim.ToString("dd/MM/yyyy")}");
            sb.AppendLine();
            sb.AppendLine("ID       Data     |  Entrada | Desconto |  Saídas |  FormaPagto |  Saldo ");
            //decimal total = 0;
            decimal TDinheiro = 0;
            decimal TCartao = 0;
            decimal TPix = 0;
            decimal TDespesa = 0;
            foreach (var lancos in relcaixa)
            {
                string ID = glo.ComplStr(lancos.ID.ToString(), 4, 2); // ID    
                string Data = glo.ComplStr(lancos.DataPagamento.ToString("dd/MM/yyyy"), 10, 2); // Data      | 
                string Entrada = glo.ComplStr(lancos.Entrada.ToString("N2"), 8, 3); // Entrada | 
                string Desconto = glo.ComplStr(lancos.Desconto.ToString("N2"), 8, 3); // Desconto | 
                string Saidas = glo.ComplStr(lancos.Saida.ToString("N2"), 7, 3); // Saídas | 
                string Forma = glo.ComplStr(lancos.Forma, 11, 2); // FormaPagto | 
                string Saldo = glo.ComplStr(lancos.Saldo.ToString("N2"), 6, 2); // Saldo
                sb.AppendLine($"{ID}   {Data}   {Entrada}   {Desconto}   {Saidas}   {Forma}   {Saldo}");
                //total += lancos.Saldo;
                switch (lancos.idFormaPagto)
                {
                    case 0:
                        TDinheiro += lancos.Entrada - lancos.Desconto;
                    break;
                    case 1:
                        TCartao += lancos.Entrada - lancos.Desconto;
                    break;
                    case 3:
                        TPix += lancos.Entrada - lancos.Desconto;
                    break;
                    case 5:
                        TDespesa += lancos.Saida;
                    break;
                }
            }
            sb.AppendLine();
            sb.AppendLine($"Dinheiro: " + glo.ComplStr(TDinheiro.ToString("N2"), 9, 2));
            sb.AppendLine($"Cartão:   " + glo.ComplStr(TCartao.ToString("N2"), 9, 2));
            sb.AppendLine($"Pix:      " + glo.ComplStr(TPix.ToString("N2"), 9, 2));
            sb.AppendLine($"Despesas: " + glo.ComplStr(TDespesa.ToString("N2"), 9, 2));
            decimal total = TDinheiro + TCartao + TPix - TDespesa;
            string totalString = glo.ComplStr(total.ToString("N2"), 9, 2); 
            string EspacosAjustes = "";
            if (total > 0)
            {
                EspacosAjustes = "                        ";
            }
            string final = $"Saldo:                                 {EspacosAjustes}{totalString}";
            sb.AppendLine();
            sb.AppendLine(final);
            return sb.ToString();
        }

        private DateTime? PrimData(DateTime DtAlternativa)
        {
            string SQL = @"SELECT Data FROM Caixa" ;
            DataTable ret = glo.getDados(SQL);
            if (ret.Rows.Count > 0)
            {
                DateTime DtRet = (DateTime)ret.Rows[0]["Data"];
                if (DtRet < DtAlternativa)
                {
                    DtRet = DtAlternativa;
                }
                return DtRet;
            }
            else
            {
                return null;
            }
        }

        private void Extrato_Activated(object sender, EventArgs e)
        {
            if (!ativou)
                ativou = true;
            this.DataFim = DateTime.Now;
            this.DataInicio = PrimData(this.DataFim.AddYears(-1));
            textBox1.Text = GerarRelCaixa(this.DataInicio, this.DataFim);
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        #region Classes

        private class Lanctos
        {
            internal string Forma;

            public int ID { get; set; }
            public DateTime DataPagamento { get; set; }
            public decimal Entrada { get; set; }
            public decimal Desconto { get; set; }
            public decimal Saida { get; set; }
            public int idFormaPagto { get; set; }            
            public decimal Saldo { get; set; }
            public int Quantidade { get; set; }
            
        }
                       
        #endregion

    }

}