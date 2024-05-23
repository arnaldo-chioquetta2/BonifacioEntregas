using System;
using System.Data.OleDb;
using System.Windows.Forms;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class fCadTiposFaltas : TeleBonifacio.FormBase
    {

        private tb.TpoFalta clienteEspecifico;

        private bool Adicionando = false;
        private bool Carregando = true;
        private int ID = 0;

        public fCadTiposFaltas()
        {
            InitializeComponent();
            base.DAO = new dao.TpoFaltaDAO();
            clienteEspecifico = DAO.GetUltimo() as tb.TpoFalta;
            base.reg = getUlt();
            ID = base.reg.Id;
            base.Mostra();
            base.LerTagsDosCamposDeTexto();
            Carregando = false;
        }

        private tb.TpoFalta getUlt()
        {
            string query = "SELECT TOP 1 * FROM TpoFalta ORDER BY idFalta Desc";            
            return ExecutarConsulta(query); ;
        }

        private tb.TpoFalta ExecutarConsulta(string query)
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
                            TpoFalta ret = new TpoFalta();
                            while (reader.Read())
                            {
                                ret.Id = (int)reader["IdFalta"];
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
            //base.cntrole1_AcaoRealizada(sender, e, clienteEspecifico);
            //if (Adicionando)
            //{
            //    switch (e.Acao)
            //    {
            //        case "CANC":
            //            Cancela();
            //            break;
            //        case "OK":
            //            Grava();
            //            base.reg = DAO.GetUltimo() as tb.TpoFalta;
            //            glo.IdAdicionado = base.reg.Id;
            //            this.Close();
            //            break;
            //    }
            //}
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
