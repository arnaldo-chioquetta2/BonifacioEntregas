using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleBonifacio.dao
{
    public class ProdutosDao
    {
        public void Adiciona(int idBalconista, float quantidade, string codigo, string Marca, string Descr, string Obs, int idForn, int idTipo, string UID)
        {
            string sql = $@"INSERT INTO Produtos (Quant, Codigo, Marca, Data, Descricao, Obs, Tipo, idForn, UID) VALUES (
                {quantidade}, 
                '{codigo}', 
                '{Marca}', 
                Now, 
                '{Descr}', 
                '{Obs}', 
                '{idTipo}', 
                {idForn}, 
                '{UID}')";
            glo.ExecutarComandoSQL(sql);
        }

        public DataTable getDados(int tipo, int idForn, string codigo, int quantidade, string marca, string Obs)
        {
            StringBuilder query = new StringBuilder();

            query.Append(@"SELECT F.Compra, '' as Forn, F.ID, F.Data, F.Codigo, F.Quant, F.Marca, F.Descricao, 
                    F.UID, F.Tipo, F.Tipo as TipoOrig, F.idForn, F.Obs 
                FROM Produtos F ");
            StringBuilder alteracoes = new StringBuilder();
            if (tipo > 0)
            {
                alteracoes.Append($@" F.Tipo = '{tipo}' and ");
            }
            if (idForn > 0)
            {
                alteracoes.Append($@" F.idForn = {idForn} and ");
            }
            //if (Comprado > 0)
            //{
            //    alteracoes.Append(" F.Compra is not null and ");
            //}
            if (codigo.Length > 0)
            {
                alteracoes.Append($@" F.Codigo = '{codigo}' and ");
            }
            if (quantidade > -1)
            {
                alteracoes.Append($@" F.Quant = {quantidade} and ");
            }
            if (marca.Length > 0)
            {
                alteracoes.Append($@" F.Marca = '{marca}' and ");
            }
            if (Obs.Length > 0)
            {
                alteracoes.Append($@" F.Obs = '{Obs}' and ");
            }
            if (alteracoes.Length > 0)
            {
                alteracoes.Length -= 4;
                query.Append($@" Where {alteracoes} ");
            }
            query.Append(" ORDER BY F.Descricao ");
            DataTable dt = glo.ExecutarConsulta(query.ToString());
            return dt;
        }

        public void Exclui(int id)
        {
            string sql = $@"DELETE FROM Produtos WHERE ID = {id}";
            glo.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, int idBalconista, float quantidade, string codigo)
        {
            string sql = $@"UPDATE Produtos SET 
                IDBalconista = {idBalconista}, 
                Quant = {quantidade}, 
                Codigo = '{codigo}'
                WHERE ID = {id}";
            glo.ExecutarComandoSQL(sql);
        }

        public void Atualiza(int iID, int iTpo, int idForn, string codigo, int quantidade, string marca, string Obs)
        {
            StringBuilder alteracoes = new StringBuilder();
            if (iTpo > 0)
            {
                alteracoes.Append($"Tipo = {iTpo}, ");
            }
            if (idForn > 0)
            {
                alteracoes.Append($"idForn = {idForn}, ");
            }
            if (!string.IsNullOrEmpty(codigo))
            {
                alteracoes.Append($"Codigo = '{codigo}', ");
            }
            if (quantidade > -1)
            {
                alteracoes.Append($"Quant = {quantidade}, ");
            }
            if (!string.IsNullOrEmpty(marca))
            {
                alteracoes.Append($"Marca = '{marca}', ");
            }
            if (!string.IsNullOrEmpty(Obs))
            {
                alteracoes.Append($"Obs = '{Obs}', ");
            }
            alteracoes.Length -= 2;
            string sql = $@"UPDATE Produtos SET {alteracoes} WHERE ID = {iID}";
            glo.ExecutarComandoSQL(sql);
        }

        public void Comprou(int iID)
        {
            string sql = $@"UPDATE Produtos SET Compra = Now WHERE ID = {iID}";
            glo.ExecutarComandoSQL(sql);
        }

        public string VeSeJaTemAFalta(string codigo)
        {
            string query = $@"SELECT Count(*) FROM Produtos Where Codigo = '{codigo}' ";
            int count = glo.ExecutarConsultaCount(query);
            string ret = "";
            if (count > 0)
            {
                ret = "Já existe um falta com este código.";
            }
            return ret;
        }

    }
}
