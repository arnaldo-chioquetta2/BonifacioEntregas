using System;

namespace TeleBonifacio.tb
{
    public class Carro : IDataEntity
    {
        public int Id { get; set; } // Identificador único do carro
        public string Nome { get; set; } // Nome do carro
        public bool Adicao { get; set; }
    }
}
