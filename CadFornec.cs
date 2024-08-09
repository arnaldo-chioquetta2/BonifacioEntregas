using System;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class CadFornec : TeleBonifacio.FormBase
    {

        private tb.Fornecedor clienteEspecifico;

        private bool Adicionando = false;
        private int ID = 0;
        private bool Carregando=true;

        public CadFornec()
        {
            InitializeComponent();
            base.DAO = new dao.FornecedorDao();
            clienteEspecifico = DAO.GetUltimo() as tb.Fornecedor;
            base.reg = getUlt();
            ID = base.reg.Id;
            base.Mostra();
            base.LerTagsDosCamposDeTexto(); 
            rt.AdjustFormComponents(this);
            Carregando = false;
        }

        private tb.Fornecedor getUlt()
        {
            string query = "SELECT TOP 1 * FROM Fornecedores ORDER BY IdForn Desc";
            return ExecutarConsulta(query); ;
        }

        private tb.Fornecedor ExecutarConsulta(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            tb.Fornecedor ret = new tb.Fornecedor();
                            while (reader.Read())
                            {
                                ret.Id = (int)reader["IdForn"];
                                ret.Nome = (string)reader["Nome"];
                                ret.EhForn = (((int)reader["EhForn"])==1);
                                if (reader["email"] != DBNull.Value)
                                {
                                    ret.email = (string)reader["email"];
                                } else
                                {
                                    ret.email = "";
                                }                                
                            }
                            return ret;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Aqui você pode decidir como lidar com a exceção
                    throw;
                }

            }
        }

        private void cntrole1_AcaoRealizada(object sender, AcaoEventArgs e)
        {
            Carregando = true;
            if (!Adicionando)
            {
                base.DAO.SetId(ID);
            } 
            if (e.Acao == "OK")
            {
                if (!ValidarEmail(txtemail.Text))
                {
                    MessageBox.Show("Email inválido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            } else
            {
                if (e.Acao == "Adicionar")
                {
                    chkEhForn.Checked = true;
                }
            }
            base.cntrole1_AcaoRealizada(sender, e, base.reg);
            Carregando = false;
        }

        private bool ValidarEmail(string email)
        {
            // Considera um e-mail vazio como válido
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            // Expressão Regular para validar o formato do e-mail
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";

            // Cria uma instância do Regex com o padrão definido
            Regex regex = new Regex(pattern);

            // Tenta corresponder o e-mail ao padrão
            return regex.IsMatch(email);
        }

        private void CadFornec_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
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
                        base.cntrole1.EmEdicao = true;
                    }
                }
            }
        }

        private void chkEhForn_CheckedChanged(object sender, EventArgs e)
        {
            if (!Carregando)
            {
                base.cntrole1.EmEdicao = true;
            }
        }

        //    private void fCadClientes_Activated(object sender, EventArgs e)
        //    {
        //        if (glo.IdAdicionado == -1)
        //        {
        //            glo.IdAdicionado = 0;
        //            Adicionando = true;
        //            base.Adicionar();
        //        }
        //    }

    }
}
