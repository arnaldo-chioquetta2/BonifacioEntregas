using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class pesCliente : Form        
    {
        private ClienteDAO Cliente;
        private DataTable dadosCli;
        private FornecedorDao forn;
        private EncomendasDao EncoDao;
        public string Nome = "";
        public string Fone = "";
        public int ClienteLocalizado = 0;
        public int Operacao = 0; // 1=Adição, 2=Edição
        private string ultimoTexto = "";
        private string telefone = "";        
        public bool OK = false;        
        private bool achou = false;        
        private bool carregando = false;
        private bool JaMostrouCombo = false;
        private int TamNaoAchou = 100;
        private bool _alterado = false;
        private int ID;
        private int selectedIndex;

        public bool alterado
        {
            get
            {
                return _alterado;
            }
            set
            {
                _alterado = value;
                if (value)
                {
                    btOK.Enabled = true;
                }
            }
        }


        public pesCliente()
        {
            InitializeComponent();
        }

        private void pesCliente_Load(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
            dtpHora.Value = nextHour;
        }

        private void CarregarComboBox<T>(ComboBox comboBox)
        {
            List<tb.ComboBoxItem> lista = new List<tb.ComboBoxItem>();
            foreach (DataRow row in dadosCli.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                string nome = row["Nome"].ToString();
                tb.ComboBoxItem item = new tb.ComboBoxItem(id, nome);
                lista.Add(item);
            }
            comboBox.DataSource = lista;
            comboBox.DisplayMember = "Nome";
            comboBox.ValueMember = "Id";
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            string nome = cmbCliente.Text;
            string telefone = txTelefone.Text;
            string descricao = txDescr.Text;
            string codigo = txCodigo.Text;
            int idForn = getidForn();
            decimal Valor = getValor();
            DateTime dataPrometida = dateTimePicker1.Value;
            DateTime HoraEntrega = getHora();
            if (Operacao==1) // Adição
            {
                if (achou)
                {
                    ClienteLocalizado = Convert.ToInt32(cmbCliente.SelectedValue);
                    glo.IdAdicionado = ClienteLocalizado;
                    if (telefone != txTelefone.Text)
                    {
                        Cliente.SetFone(txTelefone.Text);
                    }
                }
                else
                {
                    ClienteLocalizado = 0;
                    Nome = cmbCliente.Text;
                    Fone = txTelefone.Text;
                }
                EncoDao.ConfirmaEncomenda(0, Nome, Fone, descricao, dataPrometida, dataPrometida, HoraEntrega, codigo, Valor, idForn, selectedIndex);
                this.OK = true;
                carregando = false;
                this.Visible = false;
            } else
            {
                if (cmbCliente.Tag == "M")
                {
                    ClienteLocalizado = Convert.ToInt32(cmbCliente.SelectedValue);
                }
                EncoDao.Edita(this.ID, ClienteLocalizado, codigo, descricao, idForn, dataPrometida, HoraEntrega, Valor);
                this.OK = true;
                this.Visible = false;
            }
        }        

        private void cmbCliente_TextChanged(object sender, EventArgs e)
        {
            if (carregando==false)
            {
                string textoAtual = cmbCliente.Text;
                if (textoAtual.Length >= 3 && textoAtual != ultimoTexto)
                {
                    ultimoTexto = textoAtual;
                    if (achou || textoAtual.Length < TamNaoAchou)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        try
                        {
                            telefone = Cliente.BuscarTelefonePorNomeParcial(textoAtual);
                            int nrCli = Cliente.getIdAtual();
                            txTelefone.Text = telefone;
                            achou = (telefone.Length > 0);
                            if (!achou)
                            {
                                TamNaoAchou = textoAtual.Length;
                            }
                        }
                        finally
                        {
                            this.Cursor = Cursors.Default;
                        }
                    } 
                }
            }
        }

        #region Gets        

        public string getDescricao()
        {
            return txDescr.Text;
        }

        public DateTime getDtAgora()
        {
            return dtpData.Value.Date;
        }

        public DateTime getDtEnc()
        {
            return dateTimePicker1.Value.Date;
        }

        public void Ativar()
        {
            carregando = true;
            if (JaMostrouCombo == false)
            {
                Cliente = new ClienteDAO();
                this.Cursor = Cursors.WaitCursor;
                glo.Loga("CarregarComboBox<tb.Cliente>(cmbCliente) em pesCliente");
                CarregarComboBox<tb.Cliente>(cmbCliente);
                dateTimePicker1.Value = DateTime.Now.AddDays(7);
            }
            this.Cursor = Cursors.Default;
            carregando = false;

        }

        public string getcodigo()
        {
            return txCodigo.Text;
        }

        public decimal getValor()
        {
            return (decimal)glo.LeValor(txValor.Text);
        }

        public int getidForn()
        {
            int iForn = cmbForn.SelectedIndex;
            return ((tb.ComboBoxItem)cmbForn.Items[iForn]).Id;
        }
        
        public DateTime getHora()
        {
            return dtpHora.Value;
        }

        public bool getAlterado()
        {
            return alterado;
        }

        public int getCliente()
        {
            return ClienteLocalizado;
        }

        #endregion

        #region Randomico        

        private string GenerateRandomCode(int minLength, int maxLength)
        {
            var random = new Random();
            char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            char[] allCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-".ToCharArray(); // Incluindo '-' para os outros caracteres
            double exponent = 2.2; // Quanto maior menos caracteres
            int length = (int)Math.Round(Math.Pow(random.NextDouble(), exponent) * (maxLength - minLength) + minLength);
            var stringBuilder = new StringBuilder(length);
            stringBuilder.Append(characters[random.Next(characters.Length)]);
            for (int i = 1; i < length; i++)
            {
                char selectedChar = allCharacters[random.Next(allCharacters.Length)];
                stringBuilder.Append(selectedChar);
            }
            return stringBuilder.ToString();
        }

        private string GerarCodigoUnico()
        {
            string codigoGerado;
            bool codigoExiste;
            do
            {
                codigoGerado = GenerateRandomCode(5, 18);
                codigoExiste = false;
                if (VerificarCodigoNaFaltas(codigoGerado))
                {
                    codigoExiste = true;
                    continue;
                }
                if (VerificarCodigoNaProdutos(codigoGerado))
                {
                    codigoExiste = true;
                    continue;
                }
                if (VerificarCodigoNaEncomendas(codigoGerado))
                {
                    codigoExiste = true;
                }
            } while (codigoExiste);
            return codigoGerado;
        }

        private bool VerificarCodigoNaFaltas(string codigo)
        {
            int count = DB.ExecutarConsultaCount($"SELECT COUNT(*) FROM Faltas WHERE Codigo = '{codigo}' ");
            return (count > 0);
        }

        private bool VerificarCodigoNaProdutos(string codigo)
        {
            int count = DB.ExecutarConsultaCount($"SELECT COUNT(*) FROM Produtos WHERE Codigo = '{codigo}' ");
            return (count > 0);
        }

        private bool VerificarCodigoNaEncomendas(string codigo)
        {
            int count = DB.ExecutarConsultaCount($"SELECT COUNT(*) FROM Encomendas WHERE Codigo = '{codigo}' ");
            return (count > 0);
        }

        #endregion

        #region Sets

        public void setId(int oId)
        {
            this.ID = oId;
        }

        public void SetDescricao(string Descricao, bool ProdNovo, string codigo)
        {
            if (Descricao.Length > 0)
            {
                txDescr.Text = Descricao;
                txCodigo.Text = codigo;
                txDescr.Enabled = false;
                txCodigo.Enabled = false;
                lbCodigo.Text = "Código";
            }
            else
            {
                if (ProdNovo)
                {
                    LimpaNovo();
                }
            }
        }

        private void LimpaNovo()
        {
            txDescr.Text = "";
            txCodigo.Text = "";
            txDescr.Enabled = true;
            txValor.Text = "";
            txCodigo.Text = GerarCodigoUnico();
            lbCodigo.Text = "Código Randomico";
            lbCodigo.Refresh();
        }

        public void RecebeDadosCli(ref DataTable dadosC, ref FornecedorDao DadosForn, ref EncomendasDao encoDao, int IselectedIndex)
        {
            dadosCli = dadosC;
            forn = DadosForn;    
            EncoDao = encoDao;
            selectedIndex = IselectedIndex;
            glo.CarregarComboBox<tb.Fornecedor>(cmbForn, forn);
        }

        public void CarregarDados(string nome, string telefone, DateTime data, DateTime dataPrometida, string codigo, decimal valor, string descricao)
        {
            glo.Loga($"cmbCliente.Text = {nome}");
            cmbCliente.Text = nome;
            txTelefone.Text = telefone;
            dtpData.Value = data;
            dateTimePicker1.Value = dataPrometida;
            txCodigo.Text = codigo;
            txValor.Text = valor.ToString();
            txDescr.Text = descricao;
            cmbCliente.Tag = "";            
        }

        public void setOperacao(int iOperacao)
        {
            Operacao = iOperacao;
            if (Operacao==1)
            {
                LimpaNovo();
            }
        }

        public void setidCliente (int idCliente )
        {
            ClienteLocalizado = idCliente;
        }

        #endregion

        #region Altereado       

        private void txDescr_TextChanged(object sender, EventArgs e)
        {            
            if (!carregando)
            {
                btOK.Enabled = (txDescr.Text.Length > 0);
                alterado = true;
            }                
        }

        private void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                alterado = true;
                cmbCliente.Tag = "M";
            }                
        }

        private void txTelefone_TextChanged(object sender, EventArgs e)
        {
            if (!carregando)
                alterado = true;
        }

        private void cmbForn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
                alterado = true;
        }

        private void txCodigo_TextChanged(object sender, EventArgs e)
        {
            if (!carregando)
                alterado = true;
        }

        private void txValor_TextChanged(object sender, EventArgs e)
        {
            if (!carregando)
                alterado = true;
        }

        private void dtpData_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
                alterado = true;        
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
                alterado = true;
        }

        private void dtpHora_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
                alterado = true;
        }

        #endregion

    }
}
