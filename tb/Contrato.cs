using System;

namespace TeleBonifacio.tb
{
    public class Contrato : IDataEntity
    {
        public int Id { get; set; } // Identificador único do contrato
        public bool Adicao { get; set; } // Indica se é uma nova adição ou edição

        public string Descricao { get; set; } // Descrição do contrato
        public int IdEntregador { get; set; } // ID do entregador vinculado ao contrato
        public decimal Valor { get; set; } // Valor do contrato
        public string Status { get; set; } // Status do contrato (Ativo, Cancelado, Finalizado)
        public DateTime DataInicio { get; set; } // Data de início do contrato
        public DateTime DataTermino { get; set; } // Data de término do contrato
        public string Pix { get; set; } // Dados PIX do contratado
        public string Observacoes { get; set; } // Observações adicionais
        public string Nome { get; set; }
    }
}
