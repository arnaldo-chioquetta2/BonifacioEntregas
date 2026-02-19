using System;

namespace TeleBonifacio.tb
{
    public class Lanctos
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
        public decimal Valor;
        public string Descricao;
        public string Vendedor;
        public object Usuario { get; internal set; }
    }
}
