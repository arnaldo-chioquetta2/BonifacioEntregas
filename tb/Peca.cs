using System;

namespace TeleBonifacio.tb
{
    public class Peca : IDataEntity
    {
        public int Id { get; set; } // Identificador único da peça
        public string Nome { get; set; } // Nome da peça
        public int IdCarro { get; set; } // Relacionamento com o carro
        public bool Adicao { get; set; }
    }
}
