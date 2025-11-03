using System;

namespace TeleBonifacio.tb
{
    public class Garantias : IDataEntity
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }

        public int idForn { get; set; }
        public string Nota { get; set; }

        public DateTime Prometida { get; set; } 

        public DateTime DataDoForn { get; set; }
        public bool Adicao { get; set; }
        public string Nome { get; set; }
    }
}
