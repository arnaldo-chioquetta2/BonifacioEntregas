using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class OperRH : Form
    {

        private bool ativou = false;
        private RHDAO cRHDAO;
        private int iID = 0;
        private string UID = "";
        private bool carregando = true;
        private int idFunc = 0;
        private bool AdicCol;

        #region Inicialização

        public OperRH()
        {
            InitializeComponent();
            SetStartPosition();
            panel1.Height = 43;
        }

        private void SetStartPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void Extrato_Activated(object sender, EventArgs e)
        {
            if (!ativou)
            {
                ativou = true;
                VendedoresDAO Vendedor = new VendedoresDAO();
                glo.CarregarComboBox<Vendedor>(cmbVendedor, Vendedor, "Selecione");
                dtnDtFim.Value = DateTime.Today;
                dtpDataIN.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                cRHDAO = new RHDAO();
                Mostra();
                PreparaGrid();
            }
        }

        private void PreparaGrid()
        {
            dataGrid1.Columns[0].Visible = false;
            dataGrid1.Columns[1].Visible = false;
            dataGrid1.Columns[2].Width = 75;
            dataGrid1.Columns[3].Width = 110;
            dataGrid1.Columns[4].Width = 70;
            dataGrid1.Columns[5].Width = 70;
            dataGrid1.Columns[6].Width = 70;
            dataGrid1.Columns[7].Width = 70;
            dataGrid1.Columns[8].Width = 70; // .Visible = false;
            dataGrid1.Columns[9].Width = 70;
            dataGrid1.Invalidate();
        }

        #endregion

        #region Lançamentos

        private void btLancar_Click(object sender, EventArgs e)
        {
            LancaRegistro();
        }

        private void LancaRegistro()
        {
            panel1.Height = 91;
            lbColaborador.Text = cmbVendedor.Text;
            btGravar.Enabled = false;
            txInMan.Text = "";
            txFmMan.Text = "";
            txInTrd.Text = "";
            txFnTrd.Text = "";
            txInMan.Focus();
        }

        private void cmbVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            btLancar.Enabled = btImprimir.Enabled = (cmbVendedor.SelectedIndex > 0);
        }

        private TimeSpan ProcHora(string sHora)
        {
            string horafmt = ProcessarHora(sHora);
            string[] arrHora = horafmt.Split(':');
            int iHora = Convert.ToInt16(arrHora[0]);
            int iMin = Convert.ToInt16(arrHora[1]);
            TimeSpan hora = new TimeSpan(iHora, iMin, 0);
            return hora;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeSpan dInMan = ProcHora(txInMan.Text);
            TimeSpan dFmMan = ProcHora(txFmMan.Text);
            TimeSpan dInTrd = ProcHora(txInTrd.Text);
            TimeSpan dFnTrd = ProcHora(txFnTrd.Text);
            string UID = glo.GenerateUID();
            DateTime dtHorario = dtpHorario.Value.Date;
            if (this.iID == 0)
            {
                int idFuncio = Convert.ToInt32(cmbVendedor.SelectedValue.ToString());
                glo.Loga($@"HA,{idFuncio}, {dInMan}, {dFmMan}, {dInTrd}, {dFnTrd}, {UID}, {dtHorario}");
                cRHDAO.AddHorario(idFuncio, dInMan, dFmMan, dInTrd, dFnTrd, UID, dtHorario);
                txInMan.Text = "";
                txFmMan.Text = "";
                txInTrd.Text = "";
                txFnTrd.Text = "";
                if (dtpHorario.Value < DateTime.Now.AddDays(-1))
                {
                    dtpHorario.Value = dtHorario.AddDays(1);
                }
                if (dtpDataIN.Value > dtpHorario.Value)
                {
                    dtpDataIN.Value = dtpHorario.Value;
                }
                txInMan.Focus();
            }
            else
            {
                glo.Loga($@"HE,{this.iID}, {this.idFunc}, {dInMan}, {dFmMan}, {dInTrd}, {dFnTrd}, {UID}, {dtHorario}");
                cRHDAO.EdHorario(this.iID, this.idFunc, dInMan, dFmMan, dInTrd, dFnTrd, dtHorario);
                DesfazEdicao();
            }
            Mostra();
        }

        #endregion

        #region CriticaHoras

        private void txInMan_KeyUp(object sender, KeyEventArgs e)
        {
            string sInMan = ProcessarHora(txInMan.Text);
            string sFmMan = ProcessarHora(txFmMan.Text);
            string sInTrd = ProcessarHora(txInTrd.Text);
            string sFnTrd = ProcessarHora(txFnTrd.Text);
            int temManha = 0;
            int temTarde = 0;
            bool isValid = false;
            if (sInMan.Length > 0)
            {
                temManha++;
                if ((sFmMan.Length > 0))
                {
                    isValid = true;
                    temManha++;
                }
            }
            if (sInTrd == "00:00")
            {
                isValid = false;
            }
            else
            {
                if (sInTrd.Length > 0)
                {
                    temTarde++;
                    if (sFnTrd.Length > 0)
                    {
                        isValid = true;
                        temTarde++;
                    }
                }
            }
            if (temManha > 0)
            {
                isValid = CritDatas(sInMan, sFmMan);
            }
            if (temTarde > 0)
            {
                isValid = CritDatas(sInTrd, sFnTrd);
            }
            btGravar.Enabled = isValid;
        }

        private bool CritDatas(string sInTrd, string sFnTrd)
        {
            DateTime inTar, fmTar;
            bool bIN = TryParseHora(sInTrd, out inTar);
            bool bFN = TryParseHora(sFnTrd, out fmTar);
            if (bIN && bFN)
            {
                if (inTar >= fmTar)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        private bool TryParseHora(string horaString, out DateTime hora)
        {
            string[] formatosHora = { "HH:mm", "H:mm" }; 
            hora = DateTime.MinValue;

            if (DateTime.TryParseExact(horaString, formatosHora, CultureInfo.InvariantCulture, DateTimeStyles.None, out hora))
            {
                if (hora.TimeOfDay == TimeSpan.Zero)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false; 
            }
        }

        private string ProcessarHora(string hora)
        {
            if (hora.Length == 0)
            {
                return "00:00";
            }
            else
            {
                hora = hora.Replace(";", ":").Replace(".", ":").Replace(",", ":").Replace("/", ":").Replace("-", "");
                string[] partesHora = hora.Split(':');
                int pz = Convert.ToInt16(partesHora[0]);
                partesHora[0] = AdicionarZero(pz);
                if ((partesHora.Length > 1) && (partesHora[1]!=""))
                {
                    string s1 = partesHora[1];
                    int i1 = Convert.ToInt16(s1);
                    partesHora[1] = AdicionarZero(i1);
                    return string.Join(":", partesHora);
                }
                else
                {
                    return partesHora[0] + ":00";
                }
            }
        }

        private string AdicionarZero(int numero)
        {
            if (numero < 10)
            {
                return "0" + numero.ToString();
            }
            else
            {
                return numero.ToString();
            }
        }

        #endregion        

        #region Grid

        private void Mostra()
        {
            this.carregando = true;
            DateTime DT1 = dtpDataIN.Value.Date;
            DateTime DT2 = dtnDtFim.Value.Date;
            int idFunc = Convert.ToInt32(cmbVendedor.SelectedValue.ToString());
            DataTable dados = cRHDAO.getDados(DT1, DT2, idFunc);
            dataGrid1.Rows.Clear();
            if (dataGrid1.Columns.Count == 0 && dados.Columns.Count > 0)
            {
                foreach (DataColumn column in dados.Columns)
                {
                    dataGrid1.Columns.Add(column.ColumnName, column.ColumnName);
                }
            }
            if (!this.AdicCol)
            {
                dataGrid1.Columns.Add("Total", "Total");
            }            
            this.AdicCol = true;

            TimeSpan totalzao = TimeSpan.Zero;

            foreach (DataRow row in dados.Rows)
            {
                string[] rowData = new string[dados.Columns.Count + 1];
                for (int i = 0; i < dados.Columns.Count; i++)
                {
                    rowData[i] = row[i].ToString();
                }

                // Processamento dos horários de trabalho e café
                TimeSpan inMan = ProcHora(rowData[4]);  // Entrada pela manhã
                TimeSpan fmMan = ProcHora(rowData[7]);  // Saída pela manhã
                TimeSpan inCafeMan = ProcHora(rowData[5]);  // Início do café da manhã
                TimeSpan fmCafeMan = ProcHora(rowData[6]);  // Fim do café da manhã
                TimeSpan inTrd = ProcHora(rowData[8]);  // Entrada pela tarde
                TimeSpan fnTrd = ProcHora(rowData[11]); // Saída pela tarde
                TimeSpan inCafeTrd = ProcHora(rowData[9]);  // Início do café da tarde
                TimeSpan fmCafeTrd = ProcHora(rowData[10]); // Fim do café da tarde

                TimeSpan totalDia = TimeSpan.Zero;

                // 1) Inicio da manhã até o inicio do café da manhã (só se tiver inicio do café da manhã)
                if (inMan != TimeSpan.Zero && (inCafeMan != TimeSpan.Zero || fmMan != TimeSpan.Zero))
                {
                    if (inCafeMan != TimeSpan.Zero)
                        totalDia += inCafeMan - inMan;
                    else
                        totalDia += fmMan - inMan;
                }

                // 2) Fim do café da manhã até a saída da manhã (só se tiver saída da manhã)
                if (fmCafeMan != TimeSpan.Zero && fmMan != TimeSpan.Zero)
                {
                    totalDia += fmMan - fmCafeMan;
                }
                else if (fmMan != TimeSpan.Zero && inCafeMan != TimeSpan.Zero)
                {
                    totalDia += fmMan - inCafeMan;
                }

                // 3) Inicio da tarde até o inicio do café da tarde (só se tiver inicio de café da tarde)
                if (inTrd != TimeSpan.Zero && (inCafeTrd != TimeSpan.Zero || fnTrd != TimeSpan.Zero))
                {
                    if (inCafeTrd != TimeSpan.Zero)
                        totalDia += inCafeTrd - inTrd;
                    else
                        totalDia += fnTrd - inTrd;
                }

                // 4) Fim do café da tarde até fim do turno (só se tiver fim do turno)
                if (fmCafeTrd != TimeSpan.Zero && fnTrd != TimeSpan.Zero)
                {
                    totalDia += fnTrd - fmCafeTrd;
                }
                else if (fnTrd != TimeSpan.Zero && inCafeTrd != TimeSpan.Zero)
                {
                    totalDia += fnTrd - inCafeTrd;
                }

                totalzao += totalDia;

                // Calcula o total de horas e minutos corretamente, mesmo para valores acima de 24 horas
                int totalHoras = (int)totalDia.TotalHours; // TotalHours inclui horas completas de todos os minutos
                int totalMinutos = totalDia.Minutes;

                rowData[12] = $"{totalHoras:00}:{totalMinutos:00}";  // Exibição correta do total
                dataGrid1.Rows.Add(rowData);
            }
            if (idFunc > 0)
            {
                string[] totalRowData = new string[dados.Columns.Count + 1];
                for (int i = 0; i < dados.Columns.Count; i++)
                {
                    totalRowData[i] = "";
                }
                totalRowData[12] = totalzao.ToString(@"hh\:mm");
                dataGrid1.Rows.Add(totalRowData);
            }
        this.carregando = false;
    }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            Mostra();
        }

        private void btExcluir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Tem certeza que deseja excluir este registro?",
                                                  "Confirmar Deleção",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                glo.Loga($@"HD,{this.iID}, {this.UID}");
                cRHDAO.Exclui(this.iID);
                Mostra();
                DesfazEdicao();
            }
        }

        private void DesfazEdicao()
        {
            btExcluir.Visible = false;
            panel1.Height = 43;
            this.iID = 0;
        }

        private void dataGrid1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.carregando == false)
            {
                if (dataGrid1.SelectedRows.Count > 0)
                {
                    LancaRegistro();
                    btExcluir.Visible = true;
                    DataGridViewRow selectedRow = dataGrid1.SelectedRows[0];
                    this.iID = Convert.ToInt16(selectedRow.Cells["ID"].Value.ToString());
                    this.UID = selectedRow.Cells["UID"].Value.ToString();
                    this.idFunc = Convert.ToInt16(selectedRow.Cells["FuncID"].Value.ToString());
                    dtpHorario.Value = Convert.ToDateTime(selectedRow.Cells["Data"].Value);
                    txInMan.Text = selectedRow.Cells["InMan"].Value.ToString();
                    txFmMan.Text = selectedRow.Cells["FmMan"].Value.ToString();
                    txInTrd.Text = selectedRow.Cells["InTrd"].Value.ToString();
                    txFnTrd.Text = selectedRow.Cells["FnTrd"].Value.ToString();
                }

            }
        }

        #endregion

        private void btImprimir_Click(object sender, EventArgs e)
        {
            rel.RH fRel = new rel.RH();
            fRel.SetDados(cmbVendedor.SelectedIndex, dtpDataIN.Value, dtnDtFim.Value);
            fRel.Show();
        }

        private void dtpHorario_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txInMan_TextChanged(object sender, EventArgs e)
        {

        }

        private void txFmMan_TextChanged(object sender, EventArgs e)
        {

        }

        private void txInTrd_TextChanged(object sender, EventArgs e)
        {

        }

        private void txFnTrd_TextChanged(object sender, EventArgs e)
        {

        }
    }

}