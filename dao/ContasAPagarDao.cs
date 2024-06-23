using System;
using System.Data;
using System.IO;

namespace TeleBonifacio.dao
{
    public class ContasAPagarDao
    {
        public void Adiciona(int idFornecedor, DateTime dataEmissao, DateTime dataVencimento, float valorTotal, string chaveNotaFiscal, string descricao, string caminhoPDF, bool pago, DateTime? dataPagamento, string observacoes, bool perm, string UID)
        {
            string sql = $@"INSERT INTO ContasAPagar (idFornecedor, DataEmissao, DataVencimento, ValorTotal, ChaveNotaFiscal, Descricao, CaminhoPDF, Pago, DataPagamento, Observacoes, Perm, UID) VALUES (
                {idFornecedor}, 
                '{dataEmissao.ToString("yyyy-MM-dd HH:mm:ss")}', 
                '{dataVencimento.ToString("yyyy-MM-dd HH:mm:ss")}', 
                {glo.sv(valorTotal)}, 
                '{chaveNotaFiscal}', 
                '{descricao}', 
                '{caminhoPDF}', 
                {(pago ? 1 : 0)}, 
                {(dataPagamento.HasValue ? $"'{dataPagamento.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL")}, 
                '{observacoes}',
                {(perm ? 1 : 0)},
                '{UID}')";
            DB.ExecutarComandoSQL(sql);
        }

        public void Exclui(string id, string CaminhoPDF)
        {
            string sql = $@"DELETE FROM ContasAPagar WHERE ID = {id} ";
            DB.ExecutarComandoSQL(sql);
            try
            {
                File.Delete(CaminhoPDF);
            }
            catch (Exception ex)
            {
                // 

            }
        }

        public void Edita(int id, int idFornecedor, DateTime dataEmissao, DateTime dataVencimento, float valorTotal, string chaveNotaFiscal, string descricao, string caminhoPDF, bool pago, DateTime? dataPagamento, string observacoes, bool perm)
        {
            string sql = $@"UPDATE ContasAPagar SET 
                idFornecedor = {idFornecedor}, 
                DataEmissao = '{dataEmissao.ToString("yyyy-MM-dd HH:mm:ss")}', 
                DataVencimento = '{dataVencimento.ToString("yyyy-MM-dd HH:mm:ss")}', 
                ValorTotal = {glo.sv(valorTotal)}, 
                ChaveNotaFiscal = '{chaveNotaFiscal}', 
                Descricao = '{descricao}', 
                CaminhoPDF = '{caminhoPDF}', 
                Pago = {(pago ? 1 : 0)}, 
                Perm = {(perm ? 1 : 0)}, 
                DataPagamento = {(dataPagamento.HasValue ? $"'{dataPagamento.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL")}, 
                Observacoes = '{observacoes}'
                WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public void MudaFornecedor(int id, int idFornecedor)
        {
            string sql = $@"UPDATE ContasAPagar SET 
                idFornecedor = {idFornecedor} 
                WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public DataTable GetDados(int idFornecedor, int Tipo)
        {
            string sWhe = "";
            if (idFornecedor > 0)
            {
                sWhe = " And ContasAPagar.idFornecedor = " + idFornecedor;
            }           
            string sql = $@"SELECT ContasAPagar.ID, ContasAPagar.idFornecedor, Fornecedores.Nome as Fornecedor, ContasAPagar.DataEmissao, ContasAPagar.DataVencimento, ContasAPagar.ValorTotal, ContasAPagar.ChaveNotaFiscal, ContasAPagar.Descricao, ContasAPagar.CaminhoPDF, ContasAPagar.Pago, ContasAPagar.DataPagamento, ContasAPagar.Observacoes, ContasAPagar.Perm, ContasAPagar.UID 
                            FROM ContasAPagar                            
                            LEFT JOIN Fornecedores ON Fornecedores.IdForn = ContasAPagar.idFornecedor
                            WHERE ContasAPagar.Perm = {Tipo} {sWhe} 
                            ORDER BY ContasAPagar.ID DESC ";
            DataTable dt = DB.ExecutarConsulta(sql);
            return dt;
        }
    }
}
