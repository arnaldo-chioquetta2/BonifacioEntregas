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
        private ContratosDAO contratosDAO;

        public operContratos()
        {
            InitializeComponent();

        }

        private void operContratos_Load(object sender, EventArgs e)
        {
            cmbStatus.SelectedIndex = 0; // Seleciona o status "Todos"
            chkPeriodo.Checked = false; // Desabilita o filtro de período por padrão
            dtpInicio.Enabled = false;
            dtpFim.Enabled = false;
            dtpInicio.Value = DateTime.Now.AddMonths(-1); // Ajusta a data inicial para um mês atrás
            dtpFim.Value = DateTime.Now.AddMonths(1); ; // Ajusta a data final para hoje
            btnPesquisa_Click(null, null); // Aciona a pesquisa automaticamente ao carregar a tela
        }

        private void CarregarContratos(string filtroDescricao = "", string filtroStatus = "Todos")
        {
            contratosDAO = new ContratosDAO();

            // Se o checkbox do período estiver marcado, consideramos as datas no filtro
            DateTime? inicio = chkPeriodo.Checked ? dtpInicio.Value : (DateTime?)null;
            DateTime? fim = chkPeriodo.Checked ? dtpFim.Value : (DateTime?)null;

            DataTable contratos = contratosDAO.GetFilteredContratos(filtroDescricao, filtroStatus, inicio, fim);

            dgvContratos.DataSource = contratos;

            // Configurações da grid
            dgvContratos.Columns["idEntregador"].Visible = false;
            dgvContratos.Columns["PIX"].Visible = false;
            dgvContratos.Columns["DataInicio"].Visible = false;
            dgvContratos.Columns["DataTermino"].Visible = false;

            dgvContratos.Columns["Descricao"].Width = 250;
            dgvContratos.Columns["Status"].Width = 100;
            dgvContratos.Columns["Observacoes"].Width = 300;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            operEdContratos foperSQL = new operEdContratos(0);
            var result = foperSQL.ShowDialog();
            if (result == DialogResult.OK)
            {
                AtualizarListaContratos(0);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int idContratoSelecionado = Convert.ToInt32(dgvContratos.SelectedRows[0].Cells["ID"].Value);
            using (var foperSQL = new operEdContratos(idContratoSelecionado))
            {
                var result = foperSQL.ShowDialog();
                if (result == DialogResult.OK)
                {
                    AtualizarListaContratos(idContratoSelecionado);
                }
            }
        }

        private void AtualizarListaContratos(int ide)
        {
            // Obtém todos os contratos do banco de dados
            DataTable contratos = contratosDAO.GetAllContratos();

            // Atualiza a DataGridView com os dados
            dgvContratos.DataSource = contratos;

            // Configurações adicionais, se necessário
            dgvContratos.Refresh();
            if (ide>0)
            {
                foreach (DataGridViewRow row in dgvContratos.Rows)
                {
                    if (Convert.ToInt32(row.Cells["ID"].Value) == ide)
                    {
                        row.Selected = true;
                        dgvContratos.FirstDisplayedScrollingRowIndex = row.Index; // Garante que a linha fique visível
                        break;
                    }
                }
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            int contratoId = Convert.ToInt32(dgvContratos.SelectedRows[0].Cells["ID"].Value);
            tb.Contrato contrato = contratosDAO.GetContratoCompleto(contratoId);
            rel.ContratoPrinter printer = new rel.ContratoPrinter(
                contrato.Contratante,
                contrato.ContratanteCNPJ,
                contrato.ContratanteEndereco,
                contrato.Contratada,
                contrato.ContratadaCNPJ,
                contrato.ContratadaEndereco,
                contrato.NomeEmpresa, 
                contrato.CNPJEmpresa, 
                contrato.Valor,
                contrato.Descricao,
                contrato.Clausulas.ToArray(),
                contrato.DataInicio, 
                contrato.DataTermino,
                contrato.Observacoes
            );
            printer.Imprimir();
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            string descricao = txtDescricao.Text.Trim();
            string status = cmbStatus.SelectedItem.ToString();
            CarregarContratos(descricao, status);
        }

        private void chkPeriodo_CheckedChanged(object sender, EventArgs e)
        {
            bool habilitado = chkPeriodo.Checked;
            dtpInicio.Enabled = habilitado;
            dtpFim.Enabled = habilitado;

            // Se o período for desabilitado, redefinimos os valores das datas para um intervalo que abranja todos os registros
            if (!habilitado)
            {
                dtpInicio.Value = DateTime.Now.AddYears(-10); // Data inicial padrão
                dtpFim.Value = DateTime.Now.AddYears(10);    // Data final padrão
            }
        }

        private void btnTpContrato_Click(object sender, EventArgs e)
        {
            opeTpContrato foperSQL = new opeTpContrato();
            foperSQL.ShowDialog();
        }
    }

}

