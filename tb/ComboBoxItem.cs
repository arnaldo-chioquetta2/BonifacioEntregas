namespace TeleBonifacio.tb
{
    public class ComboBoxItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ComboBoxItem(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
