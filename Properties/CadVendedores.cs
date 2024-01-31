using TeleBonifacio.tb;
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
    public partial class CadVendedores : TeleBonifacio.FormBase
    {
        
        public CadVendedores()
        {
            InitializeComponent();
            base.DAO = new dao.VendedoresDAO();
            base.reg = getUlt();
            base.Mostra();
            base.LerTagsDosCamposDeTexto();
        }

        private tb.Vendedor getUlt()
        {
            string query = "SELECT TOP 1 * FROM Vendedores ORDER BY ID Desc";
            return ExecutarConsulta(query); ;
        }

        private Vendedor ExecutarConsulta(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(gen.connectionString))
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
            base.cntrole1_AcaoRealizada(sender, e, base.reg);
        }

        private void CadVendedores_KeyUp(object sender, KeyEventArgs e)
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
}
