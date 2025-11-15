using System;

namespace TeleBonifacio.tb
{
    public class Lancamento
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Obs { get; set; }
        public string Forma { get; set; }

        public string FormaPagamento
        {
            get => Forma;
            set => Forma = value;
        }

        public string Vendedor { get; set; }

        public decimal Valor { get; set; }
        public decimal Entrada { get; set; }
        public decimal Saida { get; set; }
        public decimal Desconto { get; set; }

        public decimal Saldo => Entrada - Saida - Desconto;

        // ✅ Compatibilidade com código antigo
        public string Descricao
        {
            get => Obs;
            set => Obs = value;
        }

        public string Usuario
        {
            get => Vendedor;
            set => Vendedor = value;
        }
    }
}
