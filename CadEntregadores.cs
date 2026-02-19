using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class fCadEntregadores : FormBase
    {
        private tb.Entregador clienteEspecifico;
        private bool txtIdDentro = false;
        //private bool Adicionando = false;

        public fCadEntregadores()
        {
            InitializeComponent();
            base.DAO = new dao.EntregadorDAO();
            clienteEspecifico = DAO.GetUltimo() as tb.Entregador;
            base.reg = DAO.GetUltimo() as tb.Entregador;
            base.Mostra();
            base.LerTagsDosCamposDeTexto();
            rt.AdjustFormComponents(this);
        }

        private void Teclou(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && !(e.Control && e.KeyCode == Keys.C) && !(e.Control && e.KeyCode == Keys.A))
            {
                if (e.KeyCode == Keys.Escape)
                {
                    base.Cancela();
                }
                else
                {
                    if (!base.Pesquisando)
                    {
                        if (!this.txtIdDentro)
                        {
                            base.cntrole1.EmEdicao = true;
                        }
                    }
                }
            }
        }

        private void dtpValidadeCNH_ValueChanged(object sender, EventArgs e)
        {
            if (Mostrando == false)
            {
                base.cntrole1.EmEdicao = true;
                DateTimePicker picker = sender as DateTimePicker;
                if (picker != null)
                {
                    string propertyName = picker.Name.Substring(3); 
                    PropertyInfo propertyInfo = reg.GetType().GetProperty(propertyName);
                    if (propertyInfo != null && (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?)))
                    {
                        if (picker.Value != DateTime.MinValue)
                        {
                            picker.Format = DateTimePickerFormat.Short;
                            propertyInfo.SetValue(reg, picker.Value, null);
                        }
                        else
                        {
                            picker.CustomFormat = " ";
                            picker.Format = DateTimePickerFormat.Custom;
                            propertyInfo.SetValue(reg, null, null);
                        }
                    }
                }
            }
        }

        private void cntrole1_AcaoRealizada_1(object sender, AcaoEventArgs e)
        {
            base.cntrole1_AcaoRealizada(sender, e, clienteEspecifico);

            switch (e.Acao)
            {
                case "OK":
                    List<string> criticas = new List<string>();

                    // Validação do CPF
                    if (string.IsNullOrWhiteSpace(txtCPF.Text) || !IsCPFValido(txtCPF.Text))
                    {
                        criticas.Add("O CPF informado é inválido ou está vazio.");
                    }

                    // Validação do CNPJ (somente se o nome da empresa estiver preenchido)
                    if (!string.IsNullOrWhiteSpace(txtNomeEmpresa.Text))
                    {
                        if (string.IsNullOrWhiteSpace(txtCNPJ.Text) || !IsCNPJValido(txtCNPJ.Text))
                        {
                            criticas.Add("O CNPJ informado é inválido ou está vazio.");
                        }
                    }

                    // Exibe críticas, se houver
                    if (criticas.Count > 0)
                    {
                        MessageBox.Show(string.Join("\n", criticas), "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Impede o processo de gravação caso haja erros
                    }

                    // Gravação, se tudo estiver válido
                    Grava();
                    this.DialogResult = DialogResult.OK;
                    break;
            }

        }

        private bool IsCPFValido(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verifica se o CPF possui 11 dígitos
            if (cpf.Length != 11) return false;

            // Verifica se todos os dígitos são iguais (ex.: 111.111.111-11 é inválido)
            if (cpf.Distinct().Count() == 1) return false;

            // Calcula o primeiro dígito verificador
            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = cpf.Take(9).Select((digit, i) => int.Parse(digit.ToString()) * multiplicadores1[i]).Sum();
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            // Calcula o segundo dígito verificador
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = cpf.Take(10).Select((digit, i) => int.Parse(digit.ToString()) * multiplicadores2[i]).Sum();
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            // Verifica os dígitos verificadores
            return cpf.EndsWith($"{digito1}{digito2}");
        }

        private bool IsCNPJValido(string cnpj)
        {
            // Remove quaisquer caracteres não numéricos
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            // Verifica se o CNPJ tem 14 dígitos
            if (cnpj.Length != 14)
                return false;

            // Verifica se todos os dígitos são iguais (ex.: 11.111.111/1111-11)
            if (cnpj.Distinct().Count() == 1)
                return false;

            // Calcula o primeiro dígito verificador
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicador1[i];
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            // Verifica o primeiro dígito
            if (int.Parse(cnpj[12].ToString()) != digito1)
                return false;

            // Calcula o segundo dígito verificador
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            // Verifica o segundo dígito
            return int.Parse(cnpj[13].ToString()) == digito2;
        }

        private void txtTelefone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.Equals(e.KeyChar, '-') && !char.Equals(e.KeyChar, '/') && !char.Equals(e.KeyChar, '(') && !char.Equals(e.KeyChar, ')'))
            {
                e.Handled = true;
            }
        }

        private void fCadEntregadores_Activated(object sender, EventArgs e)
        {
            
        }

        private void txtId_Enter(object sender, EventArgs e)
        {
            this.txtIdDentro = true;
        }

        private void txtId_Leave(object sender, EventArgs e)
        {
            this.txtIdDentro = false;
        }

        public void Adicao()
        {
            // Adicionando = true;
            base.Adicionar();
        }
    }

}
