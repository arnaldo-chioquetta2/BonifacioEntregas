using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TeleBonifacio.rel
{
    public partial class Extrato : Form
    {

        private bool ativou = false;
        public string Vendedor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        private List<ComissaoPaga> ComissoesPagas { get; set; }
        private List<ComissaoPendente> ComissoesPendentes { get; set; }

        private int ID = 0;

        private string Texto = "";

        public Extrato()
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

        public void SetNome(string text)
        {
            this.Vendedor = text;
            this.lblTitulo.Text = "Extrato: " + text;
        }

        private void Mostra(DateTime dataInicio, DateTime dataFim)
        {
            // Vendedor = vendedor;
            DataInicio = dataInicio;
            DataFim = dataFim;
            ComissoesPagas = CarregarComissoesPagas(dataInicio, dataFim);
            ComissoesPendentes = CarregarComissoesPendentes(dataInicio, dataFim);
            textBox1.Text=GerarExtrato();
        }

        private List<ComissaoPendente> CarregarComissoesPendentes(DateTime dataInicio, DateTime dataFim)
        {
            // this.ID
            return null;
        }

        private List<ComissaoPaga> CarregarComissoesPagas(DateTime dataInicio, DateTime dataFim)
        {
            // this.ID
            return null;
        }

        public string GerarExtrato()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Extrato de Movimentação de Comissões");
            sb.AppendLine();
            sb.AppendLine($"Vendedor: {Vendedor}");
            sb.AppendLine();
            sb.AppendLine($"Período: {DataInicio:dd/MM/yyyy} a {DataFim:dd/MM/yyyy}");
            sb.AppendLine();
            sb.AppendLine("Comissões Pagas:");
            sb.AppendLine();
            string txt = @"    Data       |qtd|     Valor
--------------------------------
1 | 01/03/2024 | 2 | R$ 1.000,00
2 | 15/03/2024 | 3 |   R$ 500,00

Comissões Pendentes:

     Venda |  Hora |     Valor | Forma de Pagamento
---------------------------------------------------
05/03/2024 | 10:00 | R$ 500,00 | Cartão de Crédito
07/03/2024 | 14:00 | R$ 700,00 | Dinheiro
08/03/2024 | 18:00 | R$ 400,00 | Pix
09/03/2024 | 12:00 | R$ 400,00 | Boleto";
            sb.AppendLine(txt);
            // Resumo
            //sb.AppendLine($"Vendedor: {Vendedor}");
            //sb.AppendLine($"Período: {DataInicio:dd/MM/yyyy} a {DataFim:dd/MM/yyyy}");
            //sb.AppendLine();

            //sb.AppendLine("**Comissões Pagas:**");
            //// sb.AppendLine($"Período: {DT1:dd/MM/yyyy} a {DT2:dd/MM/yyyy}");
            //// sb.AppendLine($"Quantidade de Pagamentos: {ComissoesPagas.Count}");
            //sb.AppendLine($"Data do Último Pagamento: {GetLastPagamentoDate(ComissoesPagas):dd/MM/yyyy}");
            //_ = sb.AppendLine($"Valor Total Pago: R$ {GetTotalPago(ComissoesPagas):F2}");
            //sb.AppendLine();

            //sb.AppendLine("**Comissões Pendentes:**");
            //// sb.AppendLine($"Quantidade de Vendas: {ComissoesPendentes.Count}");
            //sb.AppendLine($"Valor Total: R$ {GetTotalPendente(ComissoesPendentes):F2}");
            //sb.AppendLine();

            //// Detalhes das comissões pagas
            //sb.AppendLine("**Detalhes das Comissões Pagas:**");
            ////foreach (var comissao in ComissoesPagas)
            ////{
            ////    sb.AppendLine($"{comissao.DataPagamento:dd/MM/yyyy} | {comissao.Quantidade} | R$ {comissao.Valor:F2}");
            ////}
            //sb.AppendLine();

            //// Detalhes das comissões pendentes
            //sb.AppendLine("**Detalhes das Comissões Pendentes:**");
            //foreach (var comissao in ComissoesPendentes)
            //{
            //    sb.AppendLine($"{comissao.DataVenda:dd/MM/yyyy} | {comissao.HoraVenda:HH:mm} | R$ {comissao.Valor:F2} | {comissao.FormaPagamento}");
            //}

            return sb.ToString();
        }

        private object GetTotalPago(List<ComissaoPaga> comissoesPagas)
        {
            return null;
        }

        private object GetTotalPendente(List<ComissaoPendente> comissoesPendentes)
        {
            // throw new NotImplementedException();
            return null;
        }

        private object GetLastPagamentoDate(List<ComissaoPaga> comissoesPagas)
        {
            // throw new NotImplementedException();
            return null;
        }

        public void SetId(int selectedValue)
        {
            this.ID = selectedValue;
        }

        private void Extrato_Activated(object sender, EventArgs e)
        {
            if (!ativou)
                ativou = true;
            DateTime dataFim = DateTime.Now;
            DateTime dataInicio = dataFim.AddYears(-1);
            Mostra(dataInicio, dataFim);
        }

        #region Classes

        private class ComissaoPaga
        {
            public DateTime DataPagamento { get; set; }
            public int Quantidade { get; set; }
            public decimal Valor { get; set; }
        }

        private class ComissaoPendente
        {
            public DateTime DataVenda { get; set; }
            public TimeSpan HoraVenda { get; set; }
            public decimal Valor { get; set; }
            public string FormaPagamento { get; set; }
        }


        #endregion

    }

}