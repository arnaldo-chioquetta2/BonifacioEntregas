using System;

namespace TeleBonifacio.tb
{
    public class Vendedor : IDataEntity
    {
        public bool Adicao { get; set; }

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Loja { get; set; }

        public bool Atende { get; set; }

        public string Nro { get; set; }

        public string Usuario { get; set; }
        public string Senha { get; internal set; }
        public int Nivel { get; set; }

        public DateTime DataNascimento { get; set; }
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
        public TimeSpan HorarioSemanaInicio { get; set; }
        public TimeSpan HorarioSemanaFim { get; set; }
        public TimeSpan HorarioSabadoInicio { get; set; }
        public TimeSpan HorarioSabadoFim { get; set; }
        public string FormaPagamento { get; set; }
        public bool ValeAlimentacao { get; set; }
        public bool ValeTransporte { get; set; }
        public string LinhaOnibus { get; set; }
        public DateTime DataDemissao { get; set; }
        public string MotivoDemissao { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public string Cargo { get; set; }
        public string FoneEmergencia { get; set; }
        public int QtdFilhosMenor14 { get; set; }
        public string CTPS { get; set; }

        public bool FilhoComDeficiencia { get; set; }

        public string Amigo { get; set; }

        public string Fone { get; set; }

    }
}
