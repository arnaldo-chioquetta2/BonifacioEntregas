namespace TeleBonifacio.tb
{
    public interface IDataEntity
    {
        int Id { get; set; }
        bool Adicao { get; set; }

        string Nome { get; set; }
    }
}

