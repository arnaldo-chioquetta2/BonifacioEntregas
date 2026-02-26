using System.Text.RegularExpressions;

namespace TeleBonifacio.gen
{
    public class CodigoEstruturado
    {
        public string Prefixo { get; set; }
        public int Numero { get; set; }
        public string Sufixo { get; set; }
    }

    public static class CodigoPartilheiraParser
    {
        public static CodigoEstruturado Parse(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return new CodigoEstruturado
                {
                    Prefixo = "",
                    Numero = 0,
                    Sufixo = ""
                };

            var match = Regex.Match(codigo.ToUpper(), @"^([A-Z]*)(\d+)([A-Z]*)$");

            if (!match.Success)
                return new CodigoEstruturado
                {
                    Prefixo = codigo.ToUpper(),
                    Numero = 0,
                    Sufixo = ""
                };

            return new CodigoEstruturado
            {
                Prefixo = match.Groups[1].Value,
                Numero = int.Parse(match.Groups[2].Value),
                Sufixo = match.Groups[3].Value
            };
        }
    }
}