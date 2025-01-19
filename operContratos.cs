using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TeleBonifacio.dao;

namespace TeleBonifacio
{
    public partial class operContratos : Form
    {
        private decimal totalGeral = 0;
        private DataTable Dados = null;

        public operContratos()
        {
            InitializeComponent();

        }

        private void operContratos_Load(object sender, EventArgs e)
        {
            CarregarContratos();
        }

        private void CarregarContratos(string filtroDescricao = "", string filtroStatus = "Todos", DateTime? inicio = null, DateTime? fim = null)
        {
            ContratosDAO contratosDAO = new ContratosDAO();
            DataTable contratos = contratosDAO.GetAllContratos();

            // Filtro por descrição
            if (!string.IsNullOrWhiteSpace(filtroDescricao))
            {
                contratos = contratos.Select($"Descricao LIKE '%{filtroDescricao}%'").CopyToDataTable();
            }

            // Filtro por status
            if (filtroStatus != "Todos")
            {
                contratos = contratos.Select($"Status = '{filtroStatus}'").CopyToDataTable();
            }

            // Filtro por período
            if (inicio.HasValue && fim.HasValue)
            {
                contratos = contratos.Select($"DataInicio >= #{inicio.Value:yyyy-MM-dd}# AND DataTermino <= #{fim.Value:yyyy-MM-dd}#").CopyToDataTable();
            }

            dgvContratos.DataSource = contratos;
            dgvContratos.Columns[0].Visible = false;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            operEdContratos foperSQL = new operEdContratos();
            foperSQL.Show();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            operEdContratos foperSQL = new operEdContratos();
            foperSQL.ID = 1;
            foperSQL.Show();
        }
    }

}

