namespace TeleBonifacio.tb
{
    public class Percents : IDataEntity
    {
        public int Id { get; set; } // Identificador único
        public double Perc { get; set; } // Percentual (ex.: 1%, 3%, 5%)
        public double? Valor { get; set; } // Valor associado ao percentual
        public bool Adicao { get; set; }
        public string Nome { get; set; }
    }
}
