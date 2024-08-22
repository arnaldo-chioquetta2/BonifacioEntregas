using System;

namespace TeleBonifacio.tb
{
    public class Recibo
    {
        public int ID { get; set; }
        public int IdOperador { get; set; }
        public DateTime Data { get; set; }
        public string Valor { get; set; }
        public DateTime Pago { get; set; }
        public int Tipo { get; set; }
        public string Periodo { get; set; }
    }
}
