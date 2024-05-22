using TeleBonifacio.tb;
using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public partial class CadVendedores2 : TeleBonifacio.FormBase
    {
        private bool Carregando = true;
        private int ID = 0;
        private bool Adicionando = false;

        public CadVendedores2()
        {
            InitializeComponent();
            base.DAO = new dao.VendedoresDAO();
            base.reg = getUlt();
            ID = base.reg.Id;
            base.Mostra();
            base.LerTagsDosCamposDeTexto();
            Carregando = false;
        }

        private tb.Vendedor getUlt()
        {
            string query = "SELECT TOP 1 * FROM Vendedores ORDER BY ID Desc";
            return ExecutarConsulta(query); 
        }

        private Vendedor ExecutarConsulta(string query)
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
                            Vendedor ret = new Vendedor();
                            while (reader.Read())
                            {
                                ret.Id = (int)reader["ID"];
                                ret.Nome = (string)reader["Nome"];
                                ret.Loja = (string)reader["Loja"];
                                object oAt = reader["Atende"];
                                if (oAt== System.DBNull.Value)
                                {
                                    ret.Atende = false;
                                } else
                                {
                                    int iAt = (int)oAt;
                                    ret.Atende = (iAt == -1);
                                }
                                object oNro = reader["Nro"];
                                if (oNro == System.DBNull.Value)
                                {
                                    ret.Nro = "0";
                                }
                                else
                                {
                                    ret.Nro = (string)oNro;
                                }
                            }
                            return ret;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
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

        private void CadVendedor_KeyUp(object sender, KeyEventArgs e)
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

        private void chkAtende_CheckStateChanged(object sender, EventArgs e)
        {
            if (Carregando==false)
            {
                base.cntrole1.EmEdicao = true;
            }            
        }

        private void CadVendedores2_Activated(object sender, EventArgs e)
        {
            if (glo.IdAdicionado == -1)
            {
                // glo.IdAdicionado = 0;
                Adicionando = true;
                // base.Adicionar();
            }
        }

    }
}
