using System;

namespace TeleBonifacio.tb
{
    public class Devedores
    {
        public int ID { get; set; } // Identificador único
        public int Cliente { get; set; } // ID do Cliente
        public DateTime DataCompra { get; set; } // Data da compra
        public int Status { get; set; } // Status da dívida (Exemplo: 0 = Em Aberto, 1 = Pago)
        public DateTime Vencimento { get; set; } // Data de vencimento
        public string Nota { get; set; } // Número da nota fiscal (ou referência)
        public string Observacao { get; set; } // Observações adicionais
    }
}
