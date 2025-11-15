using System;

namespace TeleBonifacio.rel
{
    /// <summary>
    /// Representa um lançamento consolidado no relatório de caixa.
    /// </summary>
    public class Lanctos
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }

        // Valores financeiros
        public decimal Entrada { get; set; }
        public decimal Saida { get; set; }
        public decimal Desconto { get; set; }
        public decimal Saldo { get; set; }

        // Informações adicionais
        public string FormaPagamento { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string Usuario { get; set; } = "";
        public string Tipo { get; set; } = "";
        public bool Estornado { get; set; }

        public string Obs { get; set; } = "";
        public decimal Valor { get; set; }

        public int idFormaPagto { get; set; }
    }
}
