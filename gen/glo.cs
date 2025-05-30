﻿// #define ODBC

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Reflection;

namespace TeleBonifacio
{
    public static class glo
    {
        private static string caminhoBase;

        // -1 Avisa para o formulário que é uma adição
        // 0 Modo neutro
        // < 0 ID adicionado
        public static int IdAdicionado=0;
        //public static bool Inicializando = false;

        public static int Nivel = 0;
        // 0 Balconista, ve só as faltas  
        // 1 Caixa
        // 2 Adm

        public static int iUsuario = 0;
        public static bool ODBC = false;

        public static bool Adaptar = false;

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
                if (ODBC)
                {
                    glo.Loga("DSN=MbCarros;");
                    return "connectionString = 'DSN=MbCarros;' ";
                    // return "Driver={Microsoft Access Driver (*.mdb)};DBQ=\\SERVIDOR\\MbCarros\\MbCarros.mdb;";
                } else
                {
                    return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CaminhoBase + ";";
                }
            }
        }

        public static DateTime D0 = new DateTime(1, 01, 01).Date;
        public static DateTime D1 = new DateTime(2001, 01, 01).Date;        


        #region Conversões

        public static float LeValor(string valorTexto)
        {
            // Remove espaços e caracteres não numéricos, exceto ',' e '.'
            string valorLimpo = new string(valorTexto.Where(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());

            // Detecta o separador decimal do sistema
            char decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];

            // Ajusta o valorLimpo para garantir que o último separador seja o separador decimal correto
            if (valorLimpo.Contains('.') && valorLimpo.Contains(','))
            {
                // Assume que o último separador é o decimal
                int lastComma = valorLimpo.LastIndexOf(',');
                int lastDot = valorLimpo.LastIndexOf('.');

                if (lastComma > lastDot)
                {
                    valorLimpo = valorLimpo.Replace(".", "").Replace(",", decimalSeparator.ToString());
                }
                else
                {
                    valorLimpo = valorLimpo.Replace(",", "").Replace(".", decimalSeparator.ToString());
                }
            }
            else if (valorLimpo.Contains(',') || valorLimpo.Contains('.'))
            {
                // Substitui qualquer separador único pelo separador decimal do sistema
                valorLimpo = valorLimpo.Replace(',', decimalSeparator).Replace('.', decimalSeparator);
            }

            // Tenta converter o valor ajustado para float
            if (float.TryParse(valorLimpo, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out float valorFloat))
            {
                return valorFloat;
            }

            // Retorna 0.0f em caso de erro na conversão
            return 0.0f;
        }


        //public static float LeValor(string valorTexto)
        //{
        //    string valorLimpo = new string(valorTexto.Where(c => char.IsDigit(c) || c == ',' || c == '.').ToArray());
        //    char decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
        //    if (valorLimpo.Contains('.') && valorLimpo.Contains(','))
        //    {
        //        valorLimpo = valorLimpo.Replace(".", decimalSeparator.ToString());
        //    }
        //    else if (valorLimpo.Contains('.') || valorLimpo.Contains(','))
        //    {
        //        valorLimpo = valorLimpo.Replace(',', decimalSeparator).Replace('.', decimalSeparator);
        //    }
        //    if (float.TryParse(valorLimpo, out float valorFloat))
        //    {
        //        return valorFloat;
        //    }
        //    else
        //    {
        //        return 0.0f;
        //    }
        //}

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

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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

        public static string sv(decimal vlr)
        {
            string str = vlr.ToString();
            return str.Replace(",", ".");
        }

        public static string ComplStr(string dado, int Tam, int Tipo)
        {
            // Tipo é o alinhamento
            // 0=Esquerda,
            // 1=Central
            // 2=Direita
            // 3=Direita e se não tiver nada, retorna nada
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
                    case 3:
                        // Direita e se não tiver nada, retorna nada
                        if (dado == "0,00")
                        {
                            dado = new string(' ', Tam);
                        }
                        else
                        {
                            // CASO NORMAL
                            dado = dado.PadLeft(Tam, Convert.ToChar(Prench));
                        }
                        break;
                }
                return dado;
            }
        }

        #endregion

        #region FormatacaoTela

        public static string GenerateUID()
        {
            string dateTimePart = DateTime.Now.ToString("ddMMyyyyHHmmss");
            int QtdCarac = 20 - dateTimePart.Length;
            string randomChars = RandomString(QtdCarac);
            return dateTimePart + randomChars;
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

        #endregion

        #region DB

        #region Log

        private static string logFilePath;
        private static bool isNetworkPathChecked = false;

        public static void Loga(string message)
        {
            if (!isNetworkPathChecked)
            {
                VerificarCaminhoDeExecucao();
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    string texto = $"{DateTime.Now}: {message}";
                    writer.WriteLine(texto);
                    Console.WriteLine(texto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ ERRO AO ESCREVER LOG: {ex.Message}");
            }
        }

        private static void VerificarCaminhoDeExecucao()
        {
            string caminhoExecucao = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (caminhoExecucao.StartsWith(@"\\") || DriveIsNetwork(caminhoExecucao))
            {
                string nomeComputador = Dns.GetHostName();
                logFilePath = Path.Combine(caminhoExecucao, $"{nomeComputador}.txt");
            }
            else
            {
                logFilePath = @"C:\Entregas\Entregas.txt";
            }

            isNetworkPathChecked = true;
            Console.WriteLine($"📌 LOG definido para: {logFilePath}");
        }

        private static bool DriveIsNetwork(string path)
        {
            try
            {
                DriveInfo drive = new DriveInfo(Path.GetPathRoot(path));
                return drive.DriveType == DriveType.Network;
            }
            catch
            {
                return false;
            }
        }

        //public static void Loga(string message)
        //{
        //    string logFilePath = @"C:\Entregas\Entregas.txt";
        //    using (StreamWriter writer = new StreamWriter(logFilePath, true))
        //    {
        //        string Texto = $"{DateTime.Now}: {message}";
        //        writer.WriteLine(Texto);
        //        Console.WriteLine(Texto);
        //    }
        //}

        #endregion

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

        public static void CarregarComboBoxComCores<T>(ComboBox comboBox, dao.BaseDAO classe, string ItemZero = "", string filtro = "", string ordem = "", string ItemFinal = "", string ItemFinal2 = "", Dictionary<int, Color> tipoFaltaCores = null) where T : tb.IDataEntity, new()
        {
            DataTable dados = classe.GetDadosOrdenados(filtro, ordem);
            List<tb.ComboBoxItemComCor> lista = new List<tb.ComboBoxItemComCor>();

            // Adiciona opção inicial (Sem Cor)
            if (ItemZero.Length > 0)
            {
                lista.Add(new tb.ComboBoxItemComCor(0, ItemZero, Color.Transparent));
            }

            // Adiciona os itens do banco com suas respectivas cores
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                string corString = row.Table.Columns.Contains("Cor") ? row["Cor"].ToString() : "";

                // Converte a cor corretamente
                Color cor = glo.ConverterParaCor(corString);

                lista.Add(new tb.ComboBoxItemComCor(id, nome, cor));

                // Armazena a cor no array global
                if (tipoFaltaCores != null)
                {
                    tipoFaltaCores[id] = cor;
                }
            }

            // Adiciona itens finais, se houver
            if (ItemFinal.Length > 0)
            {
                lista.Add(new tb.ComboBoxItemComCor(0, ItemFinal, Color.Transparent));
                if (ItemFinal2.Length > 0)
                {
                    lista.Add(new tb.ComboBoxItemComCor(0, ItemFinal2, Color.Transparent));
                }
            }

            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Nome";
            comboBox.ValueMember = "Id";

            // Customiza a exibição do ComboBox para exibir cores nos itens
            comboBox.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox.DrawItem -= ComboBox_DrawItem; // Remove eventos duplicados
            comboBox.DrawItem += ComboBox_DrawItem;
        }


        public static Color ConverterParaCor(string corString)
        {
            if (string.IsNullOrEmpty(corString) || corString == "Sem Cor")
                return Color.Transparent;

            // 🔹 Mapeamento manual de algumas cores conhecidas que podem vir com nomes diferentes
            Dictionary<string, string> mapaCores = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Amarelo", "Yellow" },
                { "Azul", "Blue" },
                { "Vermelho", "Red" },
                { "Verde", "Green" },
                { "Laranja", "Orange" },
                { "Roxo", "Purple" },
                { "Cinza", "Gray" },
                { "Preto", "Black" },
                { "Branco", "White" }
            };
            // Se a cor estiver no dicionário, substitui pelo nome correto
            if (mapaCores.TryGetValue(corString, out string nomeCorCorrigido))
            {
                return Color.FromName(nomeCorCorrigido);
            }

            // Se for formato [Nome, Color [Cor]], extrai a parte útil
            if (corString.Contains("["))
            {
                int startIndex = corString.IndexOf("[") + 1;
                int endIndex = corString.IndexOf(",");
                if (startIndex > 0 && endIndex > startIndex)
                {
                    string nomeCorExtraido = corString.Substring(startIndex, endIndex - startIndex).Trim();

                    // Verifica se está no mapeamento e retorna o nome correto
                    if (mapaCores.TryGetValue(nomeCorExtraido, out string corCorrigida))
                    {
                        return Color.FromName(corCorrigida);
                    }

                    return Color.FromName(nomeCorExtraido); // Caso não esteja no mapeamento, tenta usar diretamente
                }
            }

            // Se for hexadecimal, tenta converter
            if (corString.StartsWith("#"))
            {
                try
                {
                    return ColorTranslator.FromHtml(corString);
                }
                catch { }
            }

            return Color.Transparent; // Retorna sem cor caso não consiga converter
        }


        // 🔹 Método para desenhar os itens do ComboBox com cores
        private static void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox comboBox = sender as ComboBox;
            tb.ComboBoxItemComCor item = (tb.ComboBoxItemComCor)comboBox.Items[e.Index];

            e.DrawBackground();
            using (SolidBrush brush = new SolidBrush(item.Cor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
            e.Graphics.DrawString(item.Nome, comboBox.Font, Brushes.Black, e.Bounds.X + 5, e.Bounds.Y + 2);
            e.DrawFocusRectangle();
        }

        public static void CarregarComboBoxU<T>(ComboBox comboBox, DataTable dados, string ItemZero = "", string filtro = "", string ordem = "", string ItemFinal = "", string ItemFinal2 = "") where T : tb.IDataEntity, new()
        {
            List<tb.ComboBoxItem> lista = new List<tb.ComboBoxItem>();
            if (ItemZero.Length > 0)
            {
                tb.ComboBoxItem item = new tb.ComboBoxItem(0, ItemZero);
                lista.Add(item);
            }
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                tb.ComboBoxItem item = new tb.ComboBoxItem(id, nome);
                lista.Add(item);
            }
            if (ItemFinal.Length > 0)
            {
                tb.ComboBoxItem item = new tb.ComboBoxItem(0, ItemFinal);
                lista.Add(item);
                if (ItemFinal2.Length > 0)
                {
                    tb.ComboBoxItem item2 = new tb.ComboBoxItem(0, ItemFinal2);
                    lista.Add(item2);
                }
            }
            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Nome";
            comboBox.ValueMember = "Id";
        }

        public static void CarregarComboBox<T>(ComboBox comboBox, dao.BaseDAO classe, string ItemZero = "", string filtro = "", string ordem = "", string ItemFinal = "", string ItemFinal2 = "") where T : tb.IDataEntity, new()
        {
            DataTable dados = classe.GetDadosOrdenados(filtro, ordem);
            List<tb.ComboBoxItem> lista = new List<tb.ComboBoxItem>();
            if (ItemZero.Length > 0)
            {
                tb.ComboBoxItem item = new tb.ComboBoxItem(0, ItemZero);
                lista.Add(item);
            }
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                tb.ComboBoxItem item = new tb.ComboBoxItem(id, nome);
                lista.Add(item);
            }
            if (ItemFinal.Length > 0)
            {
                tb.ComboBoxItem item = new tb.ComboBoxItem(0, ItemFinal);
                lista.Add(item);
                if (ItemFinal2.Length > 0)
                {
                    tb.ComboBoxItem item2 = new tb.ComboBoxItem(0, ItemFinal2);
                    lista.Add(item2);
                }
            }
            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Nome";
            comboBox.ValueMember = "Id";
        }

        #endregion

        public static decimal ObterPercentualVariavel(decimal valorTotal, float fator)
        {
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                float VlrFatorado = (float)(valorTotal / Convert.ToDecimal(fator));
                string vVlr = glo.sv(VlrFatorado);
                string query = $@"SELECT TOP 1 Perc FROM Percents WHERE Valor > {vVlr} ORDER BY Valor ASC";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@valorTotal", valorTotal);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToDecimal(result);
                    }
                    else
                    {
                        // Se nenhum valor maior for encontrado, retorna o percentual com valor nulo (acima disso)
                        query = "SELECT TOP 1 Perc FROM Percents WHERE Valor IS NULL";
                        command.CommandText = query;
                        result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToDecimal(result);
                        }
                        else
                        {
                            throw new Exception("Nenhum percentual configurado encontrado.");
                        }
                    }
                }
            }
        }

        public static string ShowDialog(string text, string caption)
        {
            // Defina a fonte padrão para todo o formulário
            Font font = new Font("Arial", 12);

            Form prompt = new Form()
            {
                Width = 350,
                Height = 180,
                Text = caption,
                Font = font,
                StartPosition = FormStartPosition.CenterScreen // Centraliza a janela na tela
            };

            Label textLabel = new Label()
            {
                Left = 20,
                Top = 20,
                Width = 300,
                Text = text,
                Font = font
            };

            TextBox textBox = new TextBox()
            {
                Left = 20,
                Top = 60,
                Width = 300,
                Font = font
            };

            Button confirmation = new Button()
            {
                Text = "Ok",
                Left = 220,
                Width = 100,
                Top = 100,
                Font = font
            };

            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            prompt.ShowDialog();
            return textBox.Text;
        }


    }
}