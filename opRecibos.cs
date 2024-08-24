using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TeleBonifacio.dao;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class opRecibos : Form
    {

        private ReciboDAO Recibo;
        private VendedoresDAO Vendedor;
        private decimal VlrComiss = 0;
        private float fator;
        private INI cINI;
        private bool carregando = true;
        private DateTime dtFim;
        private int OpcPerc = 0;
        private int DiasAtras = 30;

        public opRecibos()
        {
            InitializeComponent();            
        }

        private void opRecibos_Load(object sender, EventArgs e)
        {
            PopulaVendedores();
            Recibo = new ReciboDAO();
            cINI = new INI();
            OpcPerc = cINI.ReadInt("Config", "OptPerc", 0);            
            switch (OpcPerc)
            {
                case 0: // Mensal
                    rdMensal.Checked = true;
                    rdQuinzenal.Checked = false;
                    rdSemanal.Checked = false;
                    fator = 1f;
                    DiasAtras = 30;
                    break;
                case 1: // Quinzenal
                    rdQuinzenal.Checked = true;
                    rdMensal.Checked = false;
                    rdSemanal.Checked = false;
                    fator = 1f / 2f;
                    DiasAtras = 15;
                    break;
                case 2: // Semanal
                    rdSemanal.Checked = true;
                    rdMensal.Checked = false;
                    rdQuinzenal.Checked = false;
                    fator = 12f / 52f;
                    DiasAtras = 7;
                    break;
                default:
                    throw new ArgumentException("Invalid option selected");
            }
            dtFim = DateTime.Today;
            FazCarregamento();
            rt.AdjustFormComponents(this);
            carregando = false;
        }

        #region Carregamento       

        private void FazCarregamento()
        {
            lbFim.Text = dtFim.ToShortDateString();
            DateTime dtIN = dtFim.AddDays(-DiasAtras);
            dtpDataIN.Value = dtIN;
            btAtu.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            CarregaGrid();
            ConfigurarGrid();
            Cursor.Current = Cursors.Default;
            btAtu.Enabled = true;
        }

        // CarregaGrid:Refatorado em 23/08/24 Original 72 linhas, resultado 25 linhas
        private void CarregaGrid()
        {
            DateTime DT1 = dtpDataIN.Value.Date;
            DateTime DT2 = dtFim;
            Recibo = new ReciboDAO();
            DataTable Dados = Recibo.ValoresAPagar(DT1, DT2, fator);

            if (Dados.Rows.Count == 0)
            {
                dataGrid1.DataSource = null;
            }
            else
            {
                DataTable DadosFormatados = FormatarDados(Dados);
                dataGrid1.AutoGenerateColumns = false;
                dataGrid1.Columns.Clear();
                ConfigurarColunas(DadosFormatados);
                dataGrid1.DataSource = DadosFormatados;
                dataGrid1.SelectionMode = DataGridViewSelectionMode.CellSelect;
                ConfigurarGrid();
                AjustarAlinhamento();
                Console.WriteLine($"Número de linhas em DadosFormatados: {DadosFormatados.Rows.Count}");
                Console.WriteLine($"Número de colunas em DadosFormatados: {DadosFormatados.Columns.Count}");
            }
            dataGrid1.ClearSelection();
            dataGrid1.CurrentCell = null;
        }

        private DataTable FormatarDados(DataTable Dados)
        {
            DataTable DadosFormatados = new DataTable();
            DadosFormatados.Columns.Add("Descrição");
            int NrDoItemNoCombo = AdicionarColunas(Dados, DadosFormatados);

            DataRow vendasRow = DadosFormatados.NewRow();
            vendasRow[0] = "Vendas";
            DataRow percentualRow = DadosFormatados.NewRow();
            percentualRow[0] = "Percentual";
            DataRow comissoesRow = DadosFormatados.NewRow();
            comissoesRow[0] = "Comissões";

            PreencherLinhas(Dados, vendasRow, percentualRow, comissoesRow, NrDoItemNoCombo);
            DadosFormatados.Rows.Add(vendasRow);
            DadosFormatados.Rows.Add(percentualRow);
            DadosFormatados.Rows.Add(comissoesRow);

            return DadosFormatados;
        }

        private int AdicionarColunas(DataTable Dados, DataTable DadosFormatados)
        {
            int NrDoItemNoCombo = 0;
            int c = 0;
            foreach (DataRow row in Dados.Rows)
            {
                string Nome = row["Nome"].ToString();
                DadosFormatados.Columns.Add(Nome);
                c++;
                if (Nome == cmbVendedor.Text)
                {
                    NrDoItemNoCombo = c;
                }
            }
            return NrDoItemNoCombo;
        }

        private void PreencherLinhas(DataTable Dados, DataRow vendasRow, DataRow percentualRow, DataRow comissoesRow, int NrDoItemNoCombo)
        {
            for (int i = 0; i < Dados.Rows.Count; i++)
            {
                decimal totalVendas = Convert.ToDecimal(Dados.Rows[i]["TotalVendas"]);
                decimal percentual = glo.ObterPercentualVariavel(totalVendas, fator);
                decimal comissao = Convert.ToDecimal(Dados.Rows[i]["Valor"]);

                vendasRow[i + 1] = totalVendas.ToString("0.00");
                percentualRow[i + 1] = percentual.ToString("0.00") + "%";
                comissoesRow[i + 1] = comissao.ToString("0.00");

                if (i + 1 == NrDoItemNoCombo)
                {
                    ltVlr.Text = comissoesRow[i + 1].ToString();
                }
            }
        }

        private void ConfigurarColunas(DataTable DadosFormatados)
        {
            foreach (DataColumn column in DadosFormatados.Columns)
            {
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn
                {
                    Name = column.ColumnName,
                    HeaderText = column.ColumnName,
                    DataPropertyName = column.ColumnName,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                dataGrid1.Columns.Add(dgvColumn);
            }
        }

        private void AjustarAlinhamento()
        {
            for (int i = 0; i < dataGrid1.Columns.Count; i++)
            {
                for (int j = 0; j < dataGrid1.Rows.Count; j++)
                {
                    dataGrid1.Rows[j].Cells[i].Style.Alignment = (i == 0)
                        ? DataGridViewContentAlignment.MiddleLeft
                        : DataGridViewContentAlignment.MiddleRight;
                }
            }
        }        

        private void ConfigurarGrid()
        {
            if (dataGrid1.Columns.Count == 0) return;

            // Definir a fonte para as células
            Font fonte = new Font("Arial", 12, FontStyle.Regular);

            // Configurar a largura das colunas e o alinhamento
            for (int i = 0; i < dataGrid1.Columns.Count; i++)
            {
                DataGridViewColumn column = dataGrid1.Columns[i];
                column.Width = 90;
                column.DefaultCellStyle.Font = fonte;
                if (i == 0) // Primeira coluna (descrições)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else // Colunas de dados
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            // Atualizar o DataGridView para refletir as mudanças
            dataGrid1.Refresh();
        }

        #endregion

        private void PopulaVendedores()
        {
            Vendedor = new VendedoresDAO();
            DataTable dados = Vendedor.GetDadosOrdenados();
            List<ComboBoxItem> lista = new List<ComboBoxItem>();
            ComboBoxItem item0 = new ComboBoxItem(0, "SELECIONE");
            lista.Add(item0);
            foreach (DataRow row in dados.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                ComboBoxItem item = new ComboBoxItem(id, nome);
                lista.Add(item);
            }
            cmbVendedor.DataSource = lista;
            cmbVendedor.DisplayMember = "Nome";
            cmbVendedor.ValueMember = "Id";
        }

        #region Grid               

        private void dataGrid1_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            DataGridView.HitTestInfo hitTest = grid.HitTest(e.X, e.Y);

            if (hitTest.Type == DataGridViewHitTestType.ColumnHeader)
            {
                int colunaClicada = hitTest.ColumnIndex;

                if (colunaClicada >= 0 && colunaClicada < grid.Columns.Count)
                {
                    // Restaurar a cor de fundo das colunas anteriormente selecionadas
                    foreach (DataGridViewColumn column in grid.Columns)
                    {
                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            row.Cells[column.Index].Style.BackColor = grid.DefaultCellStyle.BackColor;
                        }
                    }

                    // Seleciona a coluna clicada
                    grid.ClearSelection();
                    grid.Columns[colunaClicada].Selected = true;

                    // Define a cor de fundo para a coluna selecionada
                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        row.Cells[colunaClicada].Style.BackColor = grid.DefaultCellStyle.SelectionBackColor;
                    }

                    // Obtém o nome do vendedor da coluna clicada
                    string nome = grid.Columns[colunaClicada].HeaderText;

                    // Atualiza o ComboBox
                    foreach (var item in cmbVendedor.Items)
                    {
                        if (item.ToString() == nome)
                        {
                            cmbVendedor.SelectedItem = item;
                            break;
                        }
                    }

                    // Atualiza o valor na label ltVlr
                    if (grid.Rows.Count >= 3) // Assumindo que a terceira linha contém as comissões
                    {
                        ltVlr.Text = grid.Rows[2].Cells[colunaClicada].Value.ToString();
                    }
                }
            }
        }        

        private void dataGrid1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGrid1.Columns[e.ColumnIndex].Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(dataGrid1.DefaultCellStyle.SelectionBackColor), e.CellBounds);
                e.PaintContent(e.ClipBounds);
                e.Handled = true;
            }
        }

        #endregion

        #region Eventos        

        private void cmbVendedor_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(cmbVendedor.SelectedValue);
                if (Recibo != null)
                {
                    DateTime DT1 = dtpDataIN.Value.Date;
                    DateTime DT2 = dtFim;
                    decimal ret = Recibo.VlrPend(id, DT1, DT2, fator);
                    ; ;  decimal roundedRet = Math.Round(ret, 0);
                    this.VlrComiss = Math.Round(ret, 0);
                    ltVlr.Text = this.VlrComiss.ToString("C");
                    btPagar.Enabled = (roundedRet > 0);
                    btExtrato.Enabled = true;
                }
            }
            catch (Exception)
            {
                // throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(cmbVendedor.SelectedValue);
            glo.Loga("Acionado pagamento de comissões ID do vendedor = "+ id.ToString());
            DateTime DataIni = dtpDataIN.Value;
            string dataPagamento = "";
            string dtFinal = dtFim.ToString("dd/MM/yyyy");
            if (DataIni.Date == DateTime.Now.Date)
            {
                dataPagamento = "do dia " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                dataPagamento = "de " + DataIni.ToString("dd/MM/yyyy") + " a " + dtFinal;
            }
            glo.Loga("dataPagamento = " + dataPagamento);
            Recibo.Pagar(id, ltVlr.Text, dataPagamento, dtpDataIN.Value, dtFim, fator);
            INI MeuIni = new INI();
            using (var receipt = new rel.Receipt())
            {
                string storeName = MeuIni.ReadString("Identidade", "Nome", "");
                string storeAddress = MeuIni.ReadString("Identidade", "Endereco", "");
                string storePhone = MeuIni.ReadString("Identidade", "Fone", "");
                string customerName = cmbVendedor.Text;
                receipt.Print(storeName, storeAddress, storePhone, customerName, ltVlr.Text, DataIni, dataPagamento);
            }
            this.Close();
        }

        private void btExtrato_Click(object sender, EventArgs e)
        {
            rel.Extrato fExtr = new rel.Extrato();
            fExtr.SetNome(cmbVendedor.Text);
            int id = Convert.ToInt32(cmbVendedor.SelectedValue);
            fExtr.SetId(id);
            fExtr.Show();
        }

        private void btAtu_Click(object sender, EventArgs e)
        {
            CarregaGrid();
        }        

        private void rdMensal_CheckedChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                cINI.WriteInt("Config", "OptPerc", 0);
                fator = 1f;
                OpcPerc = 0;
                DiasAtras = 30;
                FazCarregamento();                
            }
        }

        private void rdQuinzenal_CheckedChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                cINI.WriteInt("Config", "OptPerc", 1);
                fator = 1f / 2f;
                OpcPerc = 1;
                DiasAtras = 15;
                FazCarregamento(); 
            }
        }

        private void rdSemanal_CheckedChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                cINI.WriteInt("Config", "OptPerc", 2);
                fator = 12f / 52f;
                OpcPerc = 2;
                DiasAtras=7;
                FazCarregamento();
            }
        }

        private void dtpDataIN_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                DateTime Data = dtpDataIN.Value;
                dtFim = Data.AddDays(DiasAtras);
                FazCarregamento();
            }
        }

        #endregion
    }
}
