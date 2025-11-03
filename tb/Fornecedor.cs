namespace TeleBonifacio.tb
{
    public class Fornecedor : IDataEntity
    {
        public string Nome { get; set; }
        public int Id { get; set; }
        public bool Adicao { get; set; }

        public bool EhForn { get; set; }

        public string email { get; set; }
    }
}
