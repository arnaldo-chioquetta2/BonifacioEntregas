using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;

namespace TeleBonifacio.rel
{
    public partial class Extrato : Form
    {

        private bool ativou = false;
        public string Vendedor { get; set; }
        private DateTime? DataInicio { get; set; }
        private DateTime DataFim { get; set; }
        private List<ComissaoPaga> ComissoesPagas { get; set; }
        private List<ComissaoPendente> ComissoesPendentes { get; set; }

        private int ID = 0;

        private string DtInicioSis = "#01/28/2024#";

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

        private List<ComissaoPendente> CarregarComissoesPendentes(DateTime? dataInicio, DateTime dataFim)
        {
            string SQL = @"SELECT Data, Valor,  
                            SWITCH(
                                idForma = 0, 'Anotado',
                                idForma = 1, 'Cartão',
                                idForma = 2, 'Dinheiro',
                                idForma = 3, 'Pix',
                                idForma = 4, 'Troca'
                            ) AS FormaPagamento 
                           FROM Entregas 
                           WHERE Data > "+DtInicioSis 
                                +" and idVend = " + this.ID.ToString();

            List <ComissaoPendente> comissoes = new List<ComissaoPendente>();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(SQL, connection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComissaoPendente comissao = new ComissaoPendente();
                            comissao.DataVenda = (DateTime)reader["Data"];
                            comissao.Valor = (decimal)reader["Valor"];
                            comissao.FormaPagamento = (string)reader["FormaPagamento"];
                            comissoes.Add(comissao);
                        }
                    }
                }
            }
            return comissoes;
        }
        private List<ComissaoPaga> CarregarComissoesPagas(DateTime? dataInicio, DateTime dataFim)
        {
            string SQL = @"SELECT
                        V.ID as ID_Vale,
                        V.Data, 
                        V.Valor as Valor_Vale,
                        (
                            SELECT COUNT(*)
                            FROM Entregas E
                            WHERE E.idPagto = V.ID
                        ) as qtd
                    FROM Vales V
                    WHERE V.Data > "+ DtInicioSis
                        +" and V.idOperador = " + this.ID.ToString();
            List<ComissaoPaga> comissoes = new List<ComissaoPaga>();
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
                                ComissaoPaga comissao = new ComissaoPaga();
                                comissao.DataPagamento = (DateTime)reader["Data"];
                                comissao.Quantidade = (int)reader["qtd"];
                                comissao.Valor = (decimal)reader["Valor_Vale"];
                                comissoes.Add(comissao);
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
            return comissoes;
        }

        public string GerarExtrato(DateTime? dataInicio, DateTime dataFim)
        {
            ComissoesPagas = CarregarComissoesPagas(dataInicio, dataFim);
            ComissoesPendentes = CarregarComissoesPendentes(dataInicio, dataFim);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Extrato de Movimentação de Comissões");
            sb.AppendLine();
            sb.AppendLine($"Vendedor: {Vendedor}");
            sb.AppendLine();
            sb.AppendLine($"Período: {DataInicio:dd/MM/yyyy} a {DataFim:dd/MM/yyyy}");
            sb.AppendLine();
            if (ComissoesPagas.Count>0)
            {
                sb.AppendLine("Comissões Pagas:");
                sb.AppendLine();
                sb.AppendLine("    Data   |quantidade|   Valor");
                foreach (var comissao in ComissoesPagas)
                {
                    string vlr = glo.ComplStr(comissao.Valor.ToString("N2"), 6, 2);
                    string qtd = glo.ComplStr(comissao.Quantidade.ToString(), 10, 1);
                    sb.AppendLine($"{comissao.DataPagamento:dd/MM/yyyy} |{qtd}| R$ {vlr}");
                }
            }
            if (ComissoesPendentes.Count>0)
            {
                sb.AppendLine();
                sb.AppendLine("Comissões Pendentes:");
                sb.AppendLine();
                sb.AppendLine("     Venda        |   Valor   | Forma de Pagamento");
                foreach (var comissao in ComissoesPendentes)
                {
                    string forma = glo.ComplStr(comissao.FormaPagamento, 18, 1);
                    string vlr = glo.ComplStr((comissao.Valor / 100).ToString("N2"), 6, 2);
                    sb.AppendLine($" { comissao.DataVenda:dd/MM/yyyy HH:ss} | R$ {vlr} |{forma}");
                }
            }
            return sb.ToString();
        }

        private DateTime? PrimData(DateTime DtAlternativa)
        {
            string SQL = @"SELECT Data 
                           FROM Entregas
                           Where idVend = " + this.ID.ToString();
            DataTable ret = glo.getDados(SQL);
            if (ret.Rows.Count>0)
            {
                DateTime DtRet = (DateTime)ret.Rows[0]["Data"];
                if (DtRet < DtAlternativa)
                {
                    DtRet = DtAlternativa;
                }
                return DtRet;
            } else
            {
                return null;
            }
        }

        public void SetId(int selectedValue)
        {
            this.ID = selectedValue;
        }

        private void Extrato_Activated(object sender, EventArgs e)
        {
            if (!ativou)
                ativou = true;
            this.DataFim = DateTime.Now;
            this.DataInicio = PrimData(this.DataFim.AddYears(-1));
            textBox1.Text= GerarExtrato(this.DataInicio, this.DataFim);
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
            public decimal Valor { get; set; }
            public string FormaPagamento { get; set; }
        }

        #endregion

    }

}