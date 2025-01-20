using System;

namespace TeleBonifacio.tb
{
    public class Entregador : IDataEntity
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }

        public string CNH { get; set; }

        public string CPF { get; set; }
        
        public string Endereco { get; set; }

        public DateTime DataValidadeCNH { get; set; }


        public bool Adicao { get; set; }
    }
}
