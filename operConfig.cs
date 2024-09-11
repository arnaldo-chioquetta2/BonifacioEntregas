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
        private bool Carregando = true;

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
            txDocs.Text = cINI.ReadString("Config", "Docs", "");
            string NovaVersao = cINI.ReadString("Config", "NovaVersao", "");
            ckAdaptar.Checked = (cINI.ReadString("Config", "Adaptar", "") == "1");
            if (NovaVersao.Length>0)
            {
                int NovaVersaoInt = int.Parse(NovaVersao.Replace(".", ""));
                if (versionInt < NovaVersaoInt)
                {
                    btAtu.Text = "Atualizar para versão " + NovaVersao;
                }
            }
            if (Environment.CurrentDirectory.Contains("\\\\"))
            {
                ckBackup.Visible = false; 
            }
            else
            {
                ckBackup.Visible = true;
                ckBackup.Checked = (cINI.ReadString("Backup", "Ativo", "")=="1");
            }
            rt.AdjustFormComponents(this);
            Carregando = false;
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
            string Backup = ckBackup.Checked ? "1" : "0";
            cINI.WriteString("Backup", "Ativo", Backup);
            cINI.WriteString("Config", "Docs", txDocs.Text);
            glo.Adaptar = ckAdaptar.Checked;
            string sAdaptar = (glo.Adaptar) ? "1" : "0";
            cINI.WriteString("Config", "Adaptar", sAdaptar);
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
            glo.Loga("Config : PastaAtu = " + PastaAtu);
            string Executar = PastaAtu + @"\ATCAtualizeitor.exe";
            glo.Loga("Config : Executar o atualizador em " + Executar);
            Process.Start(Executar);
            Environment.Exit(0);
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
                    int colonIndex = log.IndexOf(':');
                    if (colonIndex == -1)
                    {
                        MessageBox.Show($"Formato de log inválido: {log}", "Atualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } else
                    {
                        string dataString = log.Substring(0, 19);
                        DateTime dataLog;
                        if (!DateTime.TryParseExact(dataString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataLog))
                        {
                            MessageBox.Show($"Formato de data inválido: {dataString}", "Atualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
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
                                obs = fields[4].Trim();
                                desc = fields[6];
                                idVend = fields[7];
                            }
                            else
                            {
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
                            DB.ExecutarComandoSQL(sql);
                        }
                    }
                    MessageBox.Show("Atualizado", "Atualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }
        }

        private void btRetVersao_Click(object sender, EventArgs e)
        {
            string PastaAtu = cINI.ReadString("Config", "Atualizador", "");
            cINI.WriteString("Atualizador", "Retornar", "1");
            glo.Loga("Config : PastaAtu = " + PastaAtu);
            string Executar = PastaAtu + @"\ATCAtualizeitor.exe";
            glo.Loga("Config : Executar o atualizador em " + Executar);
            Process.Start(Executar);
            Environment.Exit(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txDocs.Text = folderBrowserDialog1.SelectedPath;
            }            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CadPercs fCad = new CadPercs();
            fCad.Show();
        }

        private void btFormas_Click(object sender, EventArgs e)
        {
            fCadFormas fCad = new fCadFormas();
            fCad.Show();
        }

        private void btBackup_Click(object sender, EventArgs e)
        {
            try
            {
                // Desabilitar a tela
                DesabilitarControles(this);

                // Criar e realizar o backup
                BackupManager backupManager = new BackupManager();
                backupManager.RealizarBackup(false);
                button2.Text = "Fechar";
                MessageBox.Show("Backup realizado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar o backup: " + ex.Message);
            }
            finally
            {
                // Reabilitar a tela
                HabilitarControles(this);
            }
        }

        // Método para desabilitar todos os controles da tela
        private void DesabilitarControles(Control control)
        {
            foreach (Control c in control.Controls)
            {
                c.Enabled = false;
                if (c.HasChildren)
                {
                    DesabilitarControles(c);
                }
            }
        }

        // Método para habilitar todos os controles da tela
        private void HabilitarControles(Control control)
        {
            foreach (Control c in control.Controls)
            {
                c.Enabled = true;
                if (c.HasChildren)
                {
                    HabilitarControles(c);
                }
            }
        }

    }
}
