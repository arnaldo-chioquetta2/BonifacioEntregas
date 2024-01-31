using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleBonifacio.tb
{
    public class Entregador : IDataEntity
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }

        public string CNH { get; set; }

        public DateTime DataValidadeCNH { get; set; }


        public bool Adicao { get; set; }
    }
}
