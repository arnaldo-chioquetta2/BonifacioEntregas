using System;

namespace TeleBonifacio.tb
{
    public class Encomendas : IDataEntity
    {
        public int Id { get; set; }
        public int idCliente { get; set; }
        public DateTime Data { get; set; }
        public float Quant { get; set; }
        public string Codigo { get; set; }
        public string Marca { get; set; }
        public string UID { get; set; }
        public string Tipo { get; set; }
        public DateTime Compra { get; set; }
        public string Descricao { get; set; }
        public int idForn { get; set; }
        public string Obs { get; set; }
        public bool Adicao { get; set; }
        public string Nome { get; set; }
    }
}
