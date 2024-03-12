using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;

namespace TeleBonifacio
{
    public static class glo
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

        public static string ComplStr(string dado, int Tam, int Tipo)
        {
            // Tipo é o alinhamento
                // 0=Esquerda,
                // 1=Central
                // 2=Direita
            int Aux;
            int Aux2;
            string Aux3;
            Aux = dado.Length;
            string Prench = " ";
            if (Aux >= Tam)
            {
                return dado.Substring(0, Tam);
            }
            else
            {
                switch (Tipo)
                {
                    case 0:
                        // A esquerda
                        if (dado.Length < Tam)
                        {
                            dado = dado.PadRight(Tam, Convert.ToChar(Prench));
                        }
                        break;
                    case 1:
                        // Central
                        Aux2 = (Tam - Aux) / 2;
                        Aux3 = new string(Convert.ToChar(Prench), Aux2) + dado + new string(Convert.ToChar(Prench), Aux2);
                        Aux = Aux3.Length;
                        if (Aux < Tam)
                        {
                            Aux3 = Aux3.PadRight(Tam, Convert.ToChar(Prench));
                        }
                        if (Aux > Tam)
                        {
                            Aux3 = Aux3.Substring(0, Tam);
                        }
                        dado = Aux3;
                        break;
                    case 2:
                        // A Direita
                        if (dado.Length < Tam)
                        {
                            dado = dado.PadLeft(Tam, Convert.ToChar(Prench));
                        }
                        break;
                }
                return dado;
            }
        }

        public static string Encrypt(string input)
        {
            string output = "";
            for (int i = 0; i < input.Length; i++)
            {
                int charCode = (int)input[i];
                int newCharCode = (charCode + i) % 256;
                output += (char)newCharCode;
            }
            return output;
        }

        public static string Decrypt(string input)
        {
            string output = "";
            for (int i = 0; i < input.Length; i++)
            {
                int charCode = (int)input[i];
                int newCharCode = (charCode - i) % 256;
                if (newCharCode < 0)
                {
                    newCharCode += 256;
                }
                output += (char)newCharCode;
            }
            return output;
        }


        #region DB

        public static void ExecutarComandoSQL(string query, List<OleDbParameter> parameters=null)
        {
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
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

        public static int ExecutarConsultaCount(string query)
        {
            int count = 0;
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        count = (int)command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    // Tratamento de exceções adequado
                    throw;
                }
            }
            return count;
        }

        public static DataTable getDados(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }

        #endregion
    }
}