using System;

namespace TeleBonifacio.tb
{
    public class Vendedor : IDataEntity
    {
        public int Id { get; set; } 

        public string Nome { get; set; } 

        public string Loja { get; set; }

        public bool Adicao { get; set; }
        
        public bool Atende { get; set; }

    }
}
