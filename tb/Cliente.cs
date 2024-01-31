namespace TeleBonifacio.tb
{
    public class Cliente : IDataEntity
    {
        public int Id { get; set; }
        public bool Adicao { get; set; }

        public string Nome { get; set; }

        public string Telefone { get; set; }

        public string email { get; set; }

        public string Ender { get; set; }

        public string NrOutro { get; set; }
    }
}
