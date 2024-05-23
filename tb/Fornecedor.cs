using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleBonifacio.tb
{
    public class Fornecedor : IDataEntity
    {
        public string Nome { get; set; }
        public int Id { get; set; }
        public bool Adicao { get; set; }
    }
}
