using System;
using System.Globalization;

namespace TeleBonifacio.gen
{
    public class ValorPorExtenso
    {

        public string EscreverPorExtenso(string valor)
        {
            decimal valorDecimal = decimal.Parse(valor.Replace("R$", "").Trim(), CultureInfo.GetCultureInfo("pt-BR"));
            string valorPorExtenso = new NumeroPorExtenso(valorDecimal).ToString();
            return valorPorExtenso;
        }

        private class NumeroPorExtenso
        {
            private static readonly string[] Unidades = { "", "Um", "Dois", "Três", "Quatro", "Cinco", "Seis", "Sete", "Oito", "Nove" };
            private static readonly string[] Dezenas = { "", "Dez", "Vinte", "Trinta", "Quarenta", "Cinquenta", "Sessenta", "Setenta", "Oitenta", "Noventa" };
            private static readonly string[] Centenas = { "", "Cento", "Duzentos", "Trezentos", "Quatrocentos", "Quinhentos", "Seiscentos", "Setecentos", "Oitocentos", "Novecentos" };

            private decimal _numero;

            public NumeroPorExtenso(decimal numero)
            {
                _numero = numero;
            }

            public override string ToString()
            {
                if (_numero == 0)
                    return "Zero";

                string extenso = "";

                int inteiro = (int)_numero;
                int centavos = (int)Math.Round((_numero - inteiro) * 100);

                if (inteiro > 0)
                    extenso = FormatarNumero(inteiro) + (inteiro == 1 ? " Real" : " Reais");

                if (centavos > 0)
                    extenso += (string.IsNullOrEmpty(extenso) ? "" : " e ") + FormatarNumero(centavos) + " Centavo" + (centavos == 1 ? "" : "s");

                return extenso;
            }

            private string FormatarNumero(int numero)
            {
                if (numero == 0)
                    return "";

                if (numero < 0 || numero > 999)
                    throw new ArgumentOutOfRangeException(nameof(numero), "Número deve estar entre 0 e 999.");

                if (numero <= 9)
                    return Unidades[numero];

                if (numero <= 19)
                    return Dezenas[numero - 10];

                if (numero <= 99)
                {
                    int dezena = numero / 10;
                    int unidade = numero % 10;
                    return $"{Dezenas[dezena]} {Unidades[unidade]}".Trim();
                }

                if (numero == 100)
                    return "Cem";

                if (numero <= 999)
                {
                    int centena = numero / 100;
                    int dezena = (numero % 100) / 10;
                    int unidade = numero % 10;

                    string extenso = $"{Centenas[centena]}";

                    if (dezena == 1)
                        extenso += $" e {Dezenas[dezena * 10 + unidade]}";
                    else
                        extenso += $"{(dezena > 0 ? " e " + Dezenas[dezena] : "")} {Unidades[unidade]}";

                    return extenso.Trim();
                }

                return "";
            }
        }
    }

}

