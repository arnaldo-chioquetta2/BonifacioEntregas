namespace TeleBonifacio.tb
{
    public class Forma : IDataEntity
    {
        public int Id { get; set; }         // Mapeia a coluna ID
        public string Nome { get; set; }    // Mapeia a coluna Descricao
        public int Tipo { get; set; } = 0;  // Mapeia a coluna Tipo, com valor padrão 0

        public bool Adicao { get; set; }

        public int Ativo { get; set; } = 1;
    }
}
