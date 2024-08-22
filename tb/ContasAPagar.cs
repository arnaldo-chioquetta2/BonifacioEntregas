using System;

namespace TeleBonifacio.tb
{
    public class ContasAPagar : IDataEntity
    {
        public int Id { get; set; }
        public int IdFornecedor { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorTotal { get; set; }
        public string ChaveNotaFiscal { get; set; }
        public string Descricao { get; set; }
        public string CaminhoPDF { get; set; }
        public bool Pago { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string Observacoes { get; set; }
        public string Nome { get; set; }
        public bool Adicao { get; set; }

    }
}
