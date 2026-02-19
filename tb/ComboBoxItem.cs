namespace TeleBonifacio.tb
{
    public class ComboBoxItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public ComboBoxItem(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public ComboBoxItem(int id, string nome, string email) : this(id, nome)
        {
            Email = email;
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}