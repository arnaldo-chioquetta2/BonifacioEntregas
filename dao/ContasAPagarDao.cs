using System;
using System.Data;
using System.IO;

namespace TeleBonifacio.dao
{
    public class ContasAPagarDao
    {
        public int Adiciona(int idFornecedor, DateTime dataEmissao, DateTime dataVencimento, float valorTotal, string chaveNotaFiscal, string descricao, bool pago, DateTime? dataPagamento, string observacoes, string UID)
        {
            string sql = $@"INSERT INTO ContasAPagar (idFornecedor, DataEmissao, DataVencimento, ValorTotal, ChaveNotaFiscal, Descricao, Pago, DataPagamento, Observacoes, Perm, UID) VALUES (
                {idFornecedor}, 
                '{dataEmissao.ToString("yyyy-MM-dd HH:mm:ss")}', 
                '{dataVencimento.ToString("yyyy-MM-dd HH:mm:ss")}', 
                {glo.sv(valorTotal)}, 
                '{chaveNotaFiscal}', 
                '{descricao}', 
                {(pago ? 1 : 0)}, 
                {(dataPagamento.HasValue ? $"'{dataPagamento.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL")}, 
                '{observacoes}',
                '{UID}' )";
            DB.ExecutarComandoSQL(sql);
            string queryNome = $"SELECT Max(ID) FROM ContasAPagar ";
            return DB.ExecutarConsultaCount(queryNome);
        }

        public int AdicObter(DateTime dataEmissao, string NmArq, string UID)
        {
            string sql = $@"INSERT INTO ContasAPagar (DataEmissao, CaminhoPDF, UID) VALUES (
                '{dataEmissao.ToString("yyyy-MM-dd HH:mm:ss")}', 
                '{NmArq}',
                '{UID}' )";
            DB.ExecutarComandoSQL(sql);
            string queryNome = $"SELECT Max(ID) FROM ContasAPagar ";
            return DB.ExecutarConsultaCount(queryNome);
        }

        public void Exclui(string id, string CaminhoPDF)
        {
            string sql = $@"DELETE FROM ContasAPagar WHERE ID = {id} ";
            DB.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, int idFornecedor, DateTime dataEmissao, DateTime dataVencimento, float valorTotal, string chaveNotaFiscal, string caminhoPDF, bool pago, DateTime? dataPagamento, string observacoes)
        {
            string sVenc = "";
            if (dataVencimento!=DateTime.MinValue)
            {
                sVenc = $@"DataVencimento = '{dataVencimento.ToString("yyyy -MM-dd HH:mm:ss")}', ";
            }
            string sql = $@"UPDATE ContasAPagar SET 
                idFornecedor = {idFornecedor}, 
                DataEmissao = '{dataEmissao.ToString("yyyy-MM-dd HH:mm:ss")}', 
                {sVenc}
                ValorTotal = {glo.sv(valorTotal)}, 
                ChaveNotaFiscal = '{chaveNotaFiscal}', 
                Pago = {(pago ? 1 : 0)}, 
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

        public DataTable GetDados(int idFornecedor, DateTime? dataPagamento, DateTime? dataVencimento, DateTime? dataEmissao, string valorTotal, string descricao, string observacoes, bool? pago)
        {
            string sWhe = "";
            if (idFornecedor>0)
            {
                sWhe += " And ContasAPagar.idFornecedor = " + idFornecedor;
            }
            if (dataPagamento.HasValue)
            {
                sWhe += " And ContasAPagar.DataPagamento = #" + dataPagamento.Value.ToString("yyyy-MM-dd") + "#";
            }
            if (dataVencimento.HasValue)
            {
                sWhe += " And ContasAPagar.DataVencimento = #" + dataVencimento.Value.ToString("yyyy-MM-dd") + "#";
            }
            if (dataEmissao.HasValue)
            {
                sWhe += " And ContasAPagar.DataEmissao = #" + dataEmissao.Value.ToString("yyyy-MM-dd") + "#";
            }
            if (!string.IsNullOrEmpty(valorTotal))
            {
                sWhe += " And ContasAPagar.ValorTotal LIKE '%" + valorTotal + "%'";
            }
            if (!string.IsNullOrEmpty(descricao))
            {
                sWhe += " And ContasAPagar.Descricao LIKE '%" + descricao + "%'";
            }
            if (!string.IsNullOrEmpty(observacoes))
            {
                sWhe += " And ContasAPagar.Observacoes LIKE '%" + observacoes + "%'";
            }
            if (pago.HasValue)
            {
                sWhe += " And ContasAPagar.Pago = " + (pago.Value ? "True" : "False");
            }
            if (sWhe.Length>0)
            {

            }
            string sql = $@"SELECT ContasAPagar.CaminhoPDF as Arquivo, ContasAPagar.ID, ContasAPagar.idFornecedor, Fornecedores.Nome as Fornecedor, ContasAPagar.DataEmissao,
                            ContasAPagar.DataVencimento, ContasAPagar.ValorTotal, ContasAPagar.ChaveNotaFiscal, ContasAPagar.Descricao, 
                            ContasAPagar.CaminhoPDF, ContasAPagar.Pago, ContasAPagar.DataPagamento, ContasAPagar.Observacoes, 
                            ContasAPagar.Perm, ContasAPagar.UID, ContasAPagar.idArquivo 
                    FROM ContasAPagar                            
                    LEFT JOIN Fornecedores ON Fornecedores.IdForn = ContasAPagar.idFornecedor
                    {sWhe} 
                    ORDER BY ContasAPagar.ID DESC";
            DataTable dt = DB.ExecutarConsulta(sql);
            return dt;
        }

        //public DataTable GetDados(int idFornecedor, int tipo, DateTime? dataPagamento, DateTime? dataVencimento, DateTime? dataEmissao, string valorTotal, string descricao, string observacoes, bool? pago)
        //{
        //    string sWhe = "";
        //    if (idFornecedor > 0)
        //    {
        //        sWhe += " And ContasAPagar.idFornecedor = " + idFornecedor;
        //    }
        //    if (dataPagamento.HasValue)
        //    {
        //        sWhe += " And ContasAPagar.DataPagamento = #" + dataPagamento.Value.ToString("yyyy-MM-dd") + "#";
        //    }
        //    if (dataVencimento.HasValue)
        //    {
        //        sWhe += " And ContasAPagar.DataVencimento = #" + dataVencimento.Value.ToString("yyyy-MM-dd") + "#";
        //    }
        //    if (dataEmissao.HasValue)
        //    {
        //        sWhe += " And ContasAPagar.DataEmissao = #" + dataEmissao.Value.ToString("yyyy-MM-dd") + "#";
        //    }
        //    if (!string.IsNullOrEmpty(valorTotal))
        //    {
        //        sWhe += " And ContasAPagar.ValorTotal LIKE '%" + valorTotal + "%'";
        //    }
        //    if (!string.IsNullOrEmpty(descricao))
        //    {
        //        sWhe += " And ContasAPagar.Descricao LIKE '%" + descricao + "%'";
        //    }
        //    if (!string.IsNullOrEmpty(observacoes))
        //    {
        //        sWhe += " And ContasAPagar.Observacoes LIKE '%" + observacoes + "%'";
        //    }
        //    if (pago.HasValue)
        //    {
        //        sWhe += " And ContasAPagar.Pago = " + (pago.Value ? "True" : "False");
        //    }

        //    string sql = $@"SELECT ContasAPagar.ID, ContasAPagar.idFornecedor, Fornecedores.Nome as Fornecedor, ContasAPagar.DataEmissao, ContasAPagar.DataVencimento, ContasAPagar.ValorTotal, ContasAPagar.ChaveNotaFiscal, ContasAPagar.Descricao, ContasAPagar.CaminhoPDF, ContasAPagar.Pago, ContasAPagar.DataPagamento, ContasAPagar.Observacoes, ContasAPagar.Perm, ContasAPagar.UID 
        //            FROM ContasAPagar                            
        //            LEFT JOIN Fornecedores ON Fornecedores.IdForn = ContasAPagar.idFornecedor
        //            WHERE ContasAPagar.Perm = {tipo} {sWhe} 
        //            ORDER BY ContasAPagar.ID DESC ";
        //    DataTable dt = DB.ExecutarConsulta(sql);
        //    return dt;
        //}

        //public DataTable GetDados(int idFornecedor, int Tipo)
        //{
        //    string sWhe = "";
        //    if (idFornecedor > 0)
        //    {
        //        sWhe = " And ContasAPagar.idFornecedor = " + idFornecedor;
        //    }           
        //    string sql = $@"SELECT ContasAPagar.ID, ContasAPagar.idFornecedor, Fornecedores.Nome as Fornecedor, ContasAPagar.DataEmissao, ContasAPagar.DataVencimento, ContasAPagar.ValorTotal, ContasAPagar.ChaveNotaFiscal, ContasAPagar.Descricao, ContasAPagar.CaminhoPDF, ContasAPagar.Pago, ContasAPagar.DataPagamento, ContasAPagar.Observacoes, ContasAPagar.Perm, ContasAPagar.UID 
        //                    FROM ContasAPagar                            
        //                    LEFT JOIN Fornecedores ON Fornecedores.IdForn = ContasAPagar.idFornecedor
        //                    WHERE ContasAPagar.Perm = {Tipo} {sWhe} 
        //                    ORDER BY ContasAPagar.ID DESC ";
        //    DataTable dt = DB.ExecutarConsulta(sql);
        //    return dt;
        //}
    }
}
