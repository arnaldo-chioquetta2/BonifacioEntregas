using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;

namespace TeleBonifacio
{
    public static class gen
    {
        private static string caminhoBase;

        // -1 Avisa para o formulário que é uma adição
        // 0 Modo neutro
        // < 0 ID adicionado
        public static int IdAdicionado=0;

        public static string CaminhoBase
        {
            get
            {
                if (string.IsNullOrEmpty(caminhoBase))
                {
                    INI MeuIni = new INI();
                    caminhoBase = MeuIni.ReadString("Config", "Base", "");
                }
                return caminhoBase;
            }
            set
            {
                caminhoBase = value;
                INI MeuIni = new INI();
                MeuIni.WriteString("Config", "Base", value);
            }
        }

        public static string connectionString
        {
            get
            {
                return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CaminhoBase + ";";
            }
        }

        public static float LeValor(string valorTexto)
        {
            string valorLimpo = new string(valorTexto.Where(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());
            char decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            if (valorLimpo.Contains('.') && valorLimpo.Contains(','))
            {
                valorLimpo = valorLimpo.Replace(".", decimalSeparator.ToString());
            }
            else if (valorLimpo.Contains('.') || valorLimpo.Contains(','))
            {
                valorLimpo = valorLimpo.Replace(',', decimalSeparator).Replace('.', decimalSeparator);
            }
            if (float.TryParse(valorLimpo, out float valorFloat))
            {
                return valorFloat;
            }
            else
            {
                return 0.0f;
            }
        }

        public static string fmtVlr(string input)
        {
            string cleanValue = new string(input.Where(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());
            char decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            if (cleanValue.Contains('.') && cleanValue.Contains(','))
            {
                cleanValue = cleanValue.Replace(".", decimalSeparator.ToString());
            }
            else if (cleanValue.Contains('.') || cleanValue.Contains(','))
            {
                cleanValue = cleanValue.Replace(',', decimalSeparator).Replace('.', decimalSeparator);
            }
            if (decimal.TryParse(cleanValue, out decimal value))
            {
                if (value == 0)
                {
                    return "";
                }
                return value.ToString("0.00"); 
            }
            else
            {
                return input;
            }
        }

        public static int ConvOjbInt(object obj)
        {
            if (obj == null || !int.TryParse(obj.ToString(), out int i))
            {
                return 0;
            }
            return i;
        }

        public static string ConvOjbStr(object obj)
        {
            string str = obj.ToString();
            return str;
        }

        public static string ConvObjStrFormatado(object obj)
        {
            if (obj == null || (decimal.TryParse(obj.ToString(), out decimal num) && num == 0))
            {
                return string.Empty;
            }
            return string.Format("{0,10} ", obj);
        }

        public static string fa(string str)
        {
            return "'" + str + "'";
        }

        public static string sv(float vlr)
        {
            string str = vlr.ToString();
            return str.Replace(",", ".");
        }

        public static bool IsDateTimeValid(int year, int month, int day)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                return false;
            if (month < 1 || month > 12)
                return false;
            if (day < 1 || day > DateTime.DaysInMonth(year, month))
                return false;

            return true; // A data é válida
        }



        #region DB

        public static void ExecutarComandoSQL(string query, List<OleDbParameter> parameters=null)
        {
            using (OleDbConnection connection = new OleDbConnection(gen.connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.Add(param);
                        }
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}