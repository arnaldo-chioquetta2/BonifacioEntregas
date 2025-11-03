using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Forms;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class fCadFormas : TeleBonifacio.FormBase
    {

        private tb.Forma clienteEspecifico;

        private bool Carregando = true;
        private int ID = 0;

        public fCadFormas()
        {
            InitializeComponent();
            base.DAO = new dao.FormasDAO();
            clienteEspecifico = DAO.GetUltimo() as tb.Forma;
            base.reg = getUlt();
            ID = base.reg.Id;
            base.Mostra();
            base.LerTagsDosCamposDeTexto();
            List<string> lista = new List<string>();
            foreach (var item in cmbTipo.Items)
            {
                lista.Add(item.ToString());
            }
            base.setListCombo(lista);
            rt.AdjustFormComponents(this);
            Carregando = false;
        }

        private tb.Forma getUlt()
        {
            string query = "SELECT TOP 1 * FROM Formas ORDER BY ID Desc";            
            return ExecutarConsulta(query); ;
        }

        private tb.Forma ExecutarConsulta(string query)
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
                            Forma ret = new Forma();
                            while (reader.Read())
                            {
                                ret.Id = (int)reader["ID"];
                                ret.Nome = (string)reader["Nome"];
                                ret.Ativo = (int)reader["Ativo"];
                            }
                            return ret;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string erro = ex.ToString();
                    throw;
                }

            }
        }

        private void cntrole1_AcaoRealizada(object sender, AcaoEventArgs e)
        {
            Carregando = true;
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
                //Adicionando = true;
                base.Adicionar();
            }
        }

        private void chkAtivo_CheckedChanged(object sender, EventArgs e)
        {
            if (!Carregando)
            {
                cntrole1.EmEdicao = true;
            }
        }
    }
}
