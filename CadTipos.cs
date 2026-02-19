using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using TeleBonifacio.tb;

namespace TeleBonifacio
{
    public partial class fCadTiposFaltas : TeleBonifacio.FormBase
    {

        private tb.TpoFalta clienteEspecifico;

        private int ID = 0;
        private bool mudando = true;

        public fCadTiposFaltas()
        {
            InitializeComponent();
            base.DAO = new dao.TpoFaltaDAO();
            clienteEspecifico = DAO.GetUltimo() as tb.TpoFalta;
            base.reg = getUlt();
            ID = base.reg.Id;

            Dictionary<string, Color?> coresDisponiveis = new Dictionary<string, Color?>
            {
                { "Sem Cor", null },  
                { "Vermelho", Color.Red },
                { "Azul", Color.Blue },
                { "Verde", Color.Green },
                { "Amarelo", Color.Yellow },
                { "Roxo", Color.Purple },
                { "Laranja", Color.Orange },
                { "Cinza", Color.Gray }
            };

            // Associar as cores ao ComboBox
            cmbCor.DataSource = new BindingSource(coresDisponiveis, null);
            cmbCor.DisplayMember = "Key";  // Nome visível no ComboBox
            cmbCor.ValueMember = "Value";  // Valor real (cor)

            // Exibir a cor no fundo do ComboBox
            cmbCor.DrawMode = DrawMode.OwnerDrawFixed;
            cmbCor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCor.DrawItem += new DrawItemEventHandler(cmbCores_DrawItem);

            base.Mostra();
            base.LerTagsDosCamposDeTexto();

            rt.AdjustFormComponents(this);

            List<string> lista = new List<string>();
            foreach (var item in cmbCor.Items)
            {
                lista.Add(item.ToString());
            }
            base.setListCombo(lista);

            mudando = false;
        }

        private void cmbCores_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox cmb = (ComboBox)sender;
            KeyValuePair<string, Color?> item = (KeyValuePair<string, Color?>)cmb.Items[e.Index];

            e.DrawBackground();
            if (item.Value != null)
            {
                using (SolidBrush brush = new SolidBrush(item.Value.Value))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
            e.Graphics.DrawString(item.Key, cmb.Font, Brushes.Black, e.Bounds.X + 5, e.Bounds.Y + 2);
            e.DrawFocusRectangle();
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

                                if (reader["Cor"] != DBNull.Value)
                                {
                                    ret.Cor = (string)reader["Cor"];
                                }
                                else
                                {
                                    ret.Cor = "";
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
            if (base.reg.Id!=null)
            {
                ID = base.reg.Id;
            }
            base.cntrole1_AcaoRealizada(sender, e, base.reg);
            if (e.Acao== "CANC")
            {
                base.reg.Id = ID;
                base.Mostra();
            }
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
                base.Adicionar();
            }
        }

        private void cmbCores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!mudando)
            {
                base.cntrole1.EmEdicao = true;
            }
        }
    }
}
