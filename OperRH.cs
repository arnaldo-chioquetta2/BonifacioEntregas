using System;
using System.Data;
using System.Drawing;
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
        private const int IN_MAN = 4;
        private const int IN_CAFE_MAN = 5;
        private const int FM_CAFE_MAN = 6;
        private const int DURATION_CAFE_MAN = 7; // Novo índice para a duração do café da manhã
        private const int FM_MAN = 8;
        private const int IN_TRD = 9;
        private const int IN_CAFE_TRD = 10;
        private const int FM_CAFE_TRD = 11;
        private const int DURATION_CAFE_TRD = 12; // Novo índice para a duração do café da tarde
        private const int FN_TRD = 13;         
        private const int TOTAL_TIME = 15; // Se você adicionou uma nova coluna de total

        #region Grid

        private void Mostra()
        {
            this.carregando = true;
            DateTime DT1 = dtpDataIN.Value.Date;
            DateTime DT2 = dtnDtFim.Value.Date;
            int idFunc = Convert.ToInt32(cmbVendedor.SelectedValue.ToString());
            DataTable dados = cRHDAO.getDados(DT1, DT2, idFunc);
            ConfigureDataGridView(dados);
            TimeSpan totalzao = ProcessRows(dados);
            if (idFunc > 0)
            {
                AddTotalRow(dados, totalzao);
            }
            this.carregando = false;
        }

        private void AddTotalRow(DataTable dados, TimeSpan totalzao)
        {
            string[] totalRowData = new string[dados.Columns.Count + 1];
            for (int i = 0; i < dados.Columns.Count; i++)
            {
                totalRowData[i] = "";
            }
            totalRowData[TOTAL_TIME] = totalzao.ToString(@"hh\:mm");
            dataGrid1.Rows.Add(totalRowData);
        }

        private void PreparaGrid()
        {
            dataGrid1.Columns[0].Visible = false;  // ID
            dataGrid1.Columns[1].Visible = false;  // UID
            dataGrid1.Columns[2].Width = 75;       // Data
            dataGrid1.Columns[3].Width = 110;      // Nome
            dataGrid1.Columns[4].Width = 70;       // InMan (Entrada pela manhã)
            dataGrid1.Columns[5].Width = 70;       // InCafeMan (Início do café da manhã)
            dataGrid1.Columns[6].Width = 70;       // FmCafeMan (Fim do café da manhã)
            dataGrid1.Columns[7].Width = 50;       // TOT CAFÉ MAN
            dataGrid1.Columns[8].Width = 70;       // FmMan (Saída pela manhã)
            dataGrid1.Columns[9].Width = 70;       // InTrd (Entrada pela tarde)
            dataGrid1.Columns[10].Width = 70;      // InCafeTrd (Início do café da tarde)
            dataGrid1.Columns[11].Width = 70;      // FmCafeTrd (Fim do café da tarde)
            dataGrid1.Columns[12].Width = 50;      // TOT CAFÉ TAR
            dataGrid1.Columns[13].Width = 70;      // FnTrd (Saída pela tarde)
            dataGrid1.Columns[14].Visible = false; // FuncID
            dataGrid1.Columns[TOTAL_TIME].Width = 70;  // Total
            for (int i = 2; i <= TOTAL_TIME; i++)      // Assuming columns 4 to 11 are the ones with time data
            {
                if (i<4) 
                    dataGrid1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (rt.IsLargeScreen())
                    dataGrid1.Columns[i].Width = (int)(dataGrid1.Columns[i].Width * rt.scaleFactor);
            }
            dataGrid1.Invalidate();
        }

        private void ConfigureDataGridView(DataTable dados)
        {
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
        }

        private TimeSpan ProcessRows(DataTable dados)
        {
            TimeSpan totalzao = TimeSpan.Zero;
            foreach (DataRow row in dados.Rows)
            {
                string[] rowData = ExtractRowData(row, dados.Columns.Count);
                TimeSpan totalDia = CalculateDailyTotal(rowData);
                totalzao += totalDia;

                TimeSpan cafeManDuration = ProcHora(rowData[FM_CAFE_MAN]) - ProcHora(rowData[IN_CAFE_MAN]);
                TimeSpan cafeTrdDuration = ProcHora(rowData[FM_CAFE_TRD]) - ProcHora(rowData[IN_CAFE_TRD]);

                rowData[DURATION_CAFE_MAN] = cafeManDuration > TimeSpan.Zero ? cafeManDuration.ToString(@"hh\:mm") : "";
                rowData[DURATION_CAFE_TRD] = cafeTrdDuration > TimeSpan.Zero ? cafeTrdDuration.ToString(@"hh\:mm") : "";

                int totalHoras = (int)totalDia.TotalHours;
                int totalMinutos = totalDia.Minutes;
                if ((totalHoras+ totalMinutos)>0)
                {
                    rowData[TOTAL_TIME] = $"{totalHoras:00}:{totalMinutos:00}";
                }                

                int rowIndex = dataGrid1.Rows.Add(rowData);

                if (cafeManDuration > TimeSpan.FromMinutes(15))
                {
                    dataGrid1.Rows[rowIndex].Cells[DURATION_CAFE_MAN].Style.BackColor = Color.FromArgb(255, 200, 200);
                }
                if (cafeTrdDuration > TimeSpan.FromMinutes(15))
                {
                    dataGrid1.Rows[rowIndex].Cells[DURATION_CAFE_TRD].Style.BackColor = Color.FromArgb(255, 200, 200);
                }
            }
            return totalzao;
        }

        private string[] ExtractRowData(DataRow row, int columnCount)
        {
            string[] rowData = new string[columnCount + 1];
            for (int i = 0; i < columnCount; i++)
            {
                rowData[i] = row[i].ToString();
            }
            return rowData;
        }

        private TimeSpan CalculateDailyTotal(string[] rowData)
        {
            TimeSpan inMan = ProcHora(rowData[IN_MAN]);             // Entrada da manhã
            TimeSpan inCafeMan = ProcHora(rowData[IN_CAFE_MAN]);    // Início do café da manhã
            TimeSpan fmCafeMan = ProcHora(rowData[FM_CAFE_MAN]);    // Fim do café da manhã
            TimeSpan fmMan = ProcHora(rowData[FM_MAN]);             // Saída da manhã
            TimeSpan inTrd = ProcHora(rowData[IN_TRD]);             // Entrada da tarde
            TimeSpan inCafeTrd = ProcHora(rowData[IN_CAFE_TRD]);    // Início do café da tarde
            TimeSpan fmCafeTrd = ProcHora(rowData[FM_CAFE_TRD]);    // Fim do café da tarde
            TimeSpan fnTrd = ProcHora(rowData[FN_TRD]);             // Saída da tarde

            TimeSpan totalDia = TimeSpan.Zero;

            // Calculando o período da manhã
            totalDia += CalculateTimeSpan(inMan, inCafeMan, fmCafeMan, fmMan);
            // Calculando o período da tarde
            totalDia += CalculateTimeSpan(inTrd, inCafeTrd, fmCafeTrd, fnTrd);

            return totalDia;
        }

        private TimeSpan CalculateTimeSpan(TimeSpan start, TimeSpan breakStart, TimeSpan breakEnd, TimeSpan end)
        {
            TimeSpan total = TimeSpan.Zero;
            // Se não houver intervalo de café registrado, calcular o período total diretamente.
            if (breakStart == TimeSpan.Zero || breakEnd == TimeSpan.Zero)
            {
                if (end > start)
                {
                    total = end - start;
                }
            }
            else
            {
                // Cálculo do trabalho antes do café.
                if (breakStart > start)
                {
                    total += breakStart - start;
                }
                // Cálculo do trabalho após o café até o fim do período da manhã/tarde.
                if (breakEnd < end)
                {
                    total += end - breakEnd;
                }
            }
            return total;
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
                    txInCafeMan.Text = selectedRow.Cells["InCafeMan"].Value.ToString();
                    txFmCafeMan.Text = selectedRow.Cells["FmCafeMan"].Value.ToString();
                    txInCafeTrd.Text = selectedRow.Cells["InCafeTrd"].Value.ToString();
                    txFmCafeTrd.Text = selectedRow.Cells["FmCafeTrd"].Value.ToString();
                }
            }
        }

        #endregion

        #region Inicialização

        public OperRH()
        {
            InitializeComponent();
            SetStartPosition();
            panel1.Height = 43;
            this.toolTip1.SetToolTip(this.txFnTrd, "Fim do Expediête");
            this.toolTip1.SetToolTip(this.txInTrd, "Inicio da Tarde");
            this.toolTip1.SetToolTip(this.txFmMan, "Fim da Manhã");
            this.toolTip1.SetToolTip(this.txInMan, "Inicio do Trabalho");
            this.toolTip1.SetToolTip(this.txFmCafeTrd, "Fim do Café da Tarde");
            this.toolTip1.SetToolTip(this.txInCafeTrd, "Inicio do Café da Tarde");
            this.toolTip1.SetToolTip(this.txFmCafeMan, "Fim do Café da Manhã");
            this.toolTip1.SetToolTip(this.txInCafeMan, "Inicio do Café da Manhã");
            rt.AdjustFormComponents(this);
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

        #endregion

        #region Tooltio

        private void txInMan_MouseHover(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            this.toolTip1.Show(this.toolTip1.GetToolTip(textBox), textBox);
        }

        private void txInMan_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            this.toolTip1.Show(this.toolTip1.GetToolTip(textBox), textBox);
        }

        private void txInCafeMan_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            this.toolTip1.Show(this.toolTip1.GetToolTip(textBox), textBox);
        }

        private void HideAllToolTips()
        {
            this.toolTip1.Hide(this.txFnTrd);
            this.toolTip1.Hide(this.txInTrd);
            this.toolTip1.Hide(this.txFmMan);
            this.toolTip1.Hide(this.txInMan);
            this.toolTip1.Hide(this.txFmCafeTrd);
            this.toolTip1.Hide(this.txInCafeTrd);
            this.toolTip1.Hide(this.txFmCafeMan);
            this.toolTip1.Hide(this.txInCafeMan);
        }


        #endregion

        #region Lançamentos

        private void button1_Click(object sender, EventArgs e)
        {
            HideAllToolTips();
            TimeSpan dInMan = ProcHora(txInMan.Text);
            TimeSpan dFmMan = ProcHora(txFmMan.Text);
            TimeSpan dInTrd = ProcHora(txInTrd.Text);
            TimeSpan dFnTrd = ProcHora(txFnTrd.Text);
            TimeSpan dInCafeMan = ProcHora(txInCafeMan.Text);
            TimeSpan dFmCafeMan = ProcHora(txFmCafeMan.Text);
            TimeSpan dInCafeTrd = ProcHora(txInCafeTrd.Text);
            TimeSpan dFmCafeTrd = ProcHora(txFmCafeTrd.Text);
            string UID = glo.GenerateUID();
            DateTime dtHorario = dtpHorario.Value.Date;

            if (this.iID == 0)
            {
                int idFuncio = Convert.ToInt32(cmbVendedor.SelectedValue.ToString());
                glo.Loga($@"HA,{idFuncio}, {dInMan}, {dFmMan}, {dInTrd}, {dFnTrd}, {dInCafeMan}, {dFmCafeMan}, {dInCafeTrd}, {dFmCafeTrd}, {UID}, {dtHorario}");
                cRHDAO.AddHorario(idFuncio, dInMan, dFmMan, dInTrd, dFnTrd, dInCafeMan, dFmCafeMan, dInCafeTrd, dFmCafeTrd, UID, dtHorario);
                txInMan.Text = "";
                txFmMan.Text = "";
                txInTrd.Text = "";
                txFnTrd.Text = "";
                txInCafeMan.Text = "";
                txFmCafeMan.Text = "";
                txInCafeTrd.Text = "";
                txFmCafeTrd.Text = "";
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
                glo.Loga($@"HE,{this.iID}, {this.idFunc}, {dInMan}, {dFmMan}, {dInTrd}, {dFnTrd}, {dInCafeMan}, {dFmCafeMan}, {dInCafeTrd}, {dFmCafeTrd}, {UID}, {dtHorario}");
                cRHDAO.EdHorario(this.iID, this.idFunc, dInMan, dFmMan, dInTrd, dFnTrd, dInCafeMan, dFmCafeMan, dInCafeTrd, dFmCafeTrd, dtHorario);
                DesfazEdicao();
            }
            Mostra();
        }

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

        private void btImprimir_Click(object sender, EventArgs e)
        {
            rel.RH fRel = new rel.RH();
            fRel.SetDados(cmbVendedor.SelectedIndex, dtpDataIN.Value, dtnDtFim.Value);
            fRel.Show();
        }

        private void dtpHorario_ValueChanged(object sender, EventArgs e)
        {

        }

    }

}