using System;
using System.Data;

namespace TeleBonifacio.dao
{
    public class ConfigDAO
    {

        private bool dadosCarregados = false;
        private string empresa;
        private string endereco;
        private string fones;
        private string cnpj;
        private decimal percentual;

        private DataTable GetConfig()
        {
            string query = "Select * From Config";
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            return dt;
        }

        public decimal getPercentual()
        {
            DataTable tb = GetConfig();
            if (tb.Rows.Count > 0 && tb.Columns.Contains("UtComissoes"))
            {
                DataRow row = tb.Rows[0];
                if (row["UtComissoes"] != DBNull.Value)
                {
                    return Convert.ToDecimal(row["UtComissoes"]);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private void CarregarDados()
        {
            if (!dadosCarregados)
            {
                string query = "SELECT * FROM Config";
                DataTable tb = DB.ExecutarConsulta(query);

                if (tb.Rows.Count > 0)
                {
                    DataRow row = tb.Rows[0];
                    empresa = row["Empresa"]?.ToString() ?? "Empresa Não Definida";
                    endereco = row["Endereco"]?.ToString() ?? "Endereço Não Definido";
                    fones = row["Fones"]?.ToString() ?? "Telefones Não Definidos";
                    cnpj = row["CGC"]?.ToString() ?? "CNPJ Não Definido";
                    percentual = row["UtComissoes"] != DBNull.Value ? Convert.ToDecimal(row["UtComissoes"]) : 0;
                }
                else
                {
                    // Valores padrão caso a tabela esteja vazia
                    empresa = "Empresa Não Definida";
                    endereco = "Endereço Não Definido";
                    fones = "Telefones Não Definidos";
                    cnpj = "CNPJ Não Definido";
                    percentual = 0;
                }

                dadosCarregados = true; // Marca os dados como carregados
            }
        }

        /// <summary>
        /// Obtém o percentual de comissões.
        /// </summary>
        public decimal GetPercentual()
        {
            CarregarDados();
            return percentual;
        }

        /// <summary>
        /// Obtém o nome da empresa.
        /// </summary>
        public string GetEmpresa()
        {
            CarregarDados();
            return empresa;
        }

        /// <summary>
        /// Obtém o endereço da empresa.
        /// </summary>
        public string GetEndereco()
        {
            CarregarDados();
            return endereco;
        }

        /// <summary>
        /// Obtém os telefones da empresa.
        /// </summary>
        public string GetFones()
        {
            CarregarDados();
            return fones;
        }

        /// <summary>
        /// Obtém o CNPJ da empresa.
        /// </summary>
        public string GetCNPJ()
        {
            CarregarDados();
            return cnpj;
        }

    }
}
