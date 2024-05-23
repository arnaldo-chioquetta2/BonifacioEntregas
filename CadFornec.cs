using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class CadFornec : TeleBonifacio.FormBase
    {

        private tb.Fornecedor clienteEspecifico;

        private bool Adicionando = false;
        private bool Carregando = true;
        private int ID = 0;

        public CadFornec()
        {
            InitializeComponent();
            base.DAO = new dao.FornecedorDao();
            clienteEspecifico = DAO.GetUltimo() as tb.Fornecedor;
            base.reg = getUlt();
            ID = base.reg.Id;
            base.Mostra();
            base.LerTagsDosCamposDeTexto();
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
            base.cntrole1_AcaoRealizada(sender, e, base.reg);
            Carregando = false;
        }

        private void fCadClientes_KeyUp(object sender, KeyEventArgs e)
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

        private void fCadClientes_Activated(object sender, EventArgs e)
        {
            if (glo.IdAdicionado == -1)
            {
                glo.IdAdicionado = 0;
                Adicionando = true;
                base.Adicionar();
            }
        }

    }
}
