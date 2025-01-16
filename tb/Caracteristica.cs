using System;

namespace TeleBonifacio.tb
{
    public class Caracteristica : IDataEntity
    {
        public int Id { get; set; } // Identificador único da característica
        public string Nome { get; set; } // Descrição da característica
        public int IdPeca { get; set; } // Relacionamento com a peça
        public bool Adicao { get; set; }
    }
}
