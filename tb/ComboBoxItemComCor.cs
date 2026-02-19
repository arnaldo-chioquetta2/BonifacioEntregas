namespace TeleBonifacio.tb
{
    public class ComboBoxItemComCor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public System.Drawing.Color Cor { get; set; }

        public ComboBoxItemComCor(int id, string nome, System.Drawing.Color cor)
        {
            Id = id;
            Nome = nome;
            Cor = cor;
        }

        public override string ToString()
        {
            return Nome; // Para exibição no ComboBox
        }
    }
}
