using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class oprConfig : Form        
    {

        private INI cINI;

        public oprConfig()
        {
            InitializeComponent();
            textBox1.Text = glo.CaminhoBase;
            cINI = new INI();
            txNome.Text = cINI.ReadString("Identidade", "Nome", "");
            txEndereco.Text = cINI.ReadString("Identidade", "Endereco", "");
            txFone.Text = cINI.ReadString("Identidade", "Fone", "");

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            int versionInt = (version.Major * 100) + (version.Minor * 10) + version.Build;

            string NovaVersao = cINI.ReadString("Config", "NovaVersao", "");            
            if (NovaVersao.Length>0)
            {
                int NovaVersaoInt = int.Parse(NovaVersao.Replace(".", ""));
                if (versionInt < NovaVersaoInt)
                {
                    btAtu.Text = "Atualizar para versão " + NovaVersao;
                }
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = (openFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            glo.CaminhoBase = textBox1.Text;
            cINI.WriteString("Identidade", "Nome", txNome.Text);
            cINI.WriteString("Identidade", "Endereco", txEndereco.Text);
            cINI.WriteString("Identidade", "Fone", txFone.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblNome_DoubleClick(object sender, EventArgs e)
        {

            operSQL foperSQL = new operSQL();
            foperSQL.Show();
        }

        private void btAtu_Click(object sender, EventArgs e)
        {
            string PastaAtu = cINI.ReadString("Config", "Atualizador", "");
            Loga("Config : PastaAtu = " + PastaAtu);
            string Executar = PastaAtu + @"\ATCAtualizeitor.exe";
            Loga("Config : Executar o atualizador em " + Executar);
            Process.Start(Executar);
            Environment.Exit(0);
        }

        private void Loga(string message)
        {
            string logFilePath = @"C:\Entregas\Entregas.txt";
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ArqLogs = openFileDialog1.FileName;
                List<string> logs = new List<string>(File.ReadAllLines(ArqLogs));
                foreach (var log in logs)
                {
                    // Encontra a posição do primeiro ':' na linha para separar a data
                    int colonIndex = log.IndexOf(':');
                    if (colonIndex == -1)
                    {
                        Console.WriteLine($"Formato de log inválido: {log}");
                        continue; // Pula para a próxima linha do arquivo de log
                    }

                    // Extrai a data do log
                    string dataString = log.Substring(0, 19);
                    DateTime dataLog;
                    if (!DateTime.TryParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataLog))
                    {
                        Console.WriteLine($"Formato de data inválido: {dataString}");
                        continue; // Pula para a próxima linha do arquivo de log
                    }

                    string logWithoutDate = log.Substring(colonIndex + 1);
                    string[] fields = logWithoutDate.Split(',');
                    string idForma = fields[1];
                    string valor = fields[2];                    
                    string idCliente = fields[3];
                    string obs = "";
                    string desc = "";
                    string idVend = "";
                    if (fields.Length >= 8)
                    {
                        // Registro diferente
                        obs = fields[4].Trim();
                        desc = fields[6];
                        idVend = fields[7];
                    }
                    else
                    {
                        // Registro normal
                        obs = fields[4].Trim();
                        desc = fields[5];
                        idVend = fields[6];
                    }                    
                    if (obs == "")
                    {
                        obs = " ";
                    }
                    decimal dDesc = Convert.ToDecimal(desc);
                    decimal dvalor = Convert.ToDecimal(valor);
                    decimal dvlNota = dvalor - dDesc;
                    string vlNota = dvlNota.ToString();
                    string sql = $@"INSERT INTO Caixa (idCliente, idForma, Valor, VlNota, Obs, Desconto, idVend, Data) VALUES (
                {idCliente}, {idForma}, {valor}, {vlNota}, '{obs}', {desc}, {idVend}, '{dataLog.ToString("yyyy-MM-dd HH:mm:ss")}');";

                    glo.ExecutarComandoSQL(sql);
                }
                MessageBox.Show("Atualizado", "Atualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }    
}
