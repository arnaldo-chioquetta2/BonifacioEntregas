using System;

namespace TeleBonifacio.tb
{
    public class EtiquetaModel
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Preco { get; set; }
        public string Observacao { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AlteradoEm { get; set; }
    }
}
