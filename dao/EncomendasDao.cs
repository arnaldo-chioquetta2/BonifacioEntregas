using System.Data;
using System.Text;

namespace TeleBonifacio.dao
{
    public class EncomendasDao
    {
        public void Adiciona(int idCliente, float quantidade, string codigo, string marca, string descricao, string obs, int idForn, int tipo, string UID)
        {
            string sql = $@"INSERT INTO Encomendas (idCliente, Quant, Codigo, Marca, Descricao, Obs, Tipo, idForn, UID, Data) VALUES (
                {idCliente},
                {quantidade}, 
                '{codigo}', 
                '{marca}', 
                '{descricao}', 
                '{obs}', 
                '{tipo}', 
                {idForn}, 
                '{UID}', 
                Now())";
            glo.ExecutarComandoSQL(sql);
        }

        public DataTable getDados(int tipo, int idForn, string codigo, int quantidade, string marca, string Obs)
        {
            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT E.ID, IIf(E.Nome IS NULL OR E.Nome = '', Clientes.Nome, E.Nome) AS Nome, E.Data, E.Codigo, E.Quant, 
                           E.Marca, E.Descricao, E.UID, E.Tipo, E.Compra, '' as Forn, E.idForn, E.Obs 
                           FROM Encomendas E 
                           Left Join Clientes on Clientes.NrCli = E.idCliente ");
            StringBuilder alteracoes = new StringBuilder();
            if (tipo > 0)
            {
                alteracoes.Append($@" E.Tipo = '{tipo}' AND ");
            }
            if (idForn > 0)
            {
                alteracoes.Append($@" E.idForn = {idForn} AND ");
            }
            if (!string.IsNullOrEmpty(codigo))
            {
                alteracoes.Append($@" E.Codigo = '{codigo}' AND ");
            }
            if (quantidade > -1)
            {
                alteracoes.Append($@" E.Quant = {quantidade} AND ");
            }
            if (!string.IsNullOrEmpty(marca))
            {
                alteracoes.Append($@" E.Marca = '{marca}' AND ");
            }
            if (!string.IsNullOrEmpty(Obs))
            {
                alteracoes.Append($@" E.Obs = '{Obs}' AND ");
            }
            if (alteracoes.Length > 0)
            {
                alteracoes.Length -= 4; 
                query.Append($@" WHERE {alteracoes}");
            }
            query.Append(" ORDER BY E.Data DESC");
            DataTable dt = glo.ExecutarConsulta(query.ToString());
            return dt;
        }

        public void Exclui(int id)
        {
            string sql = $@"DELETE FROM Encomendas WHERE ID = {id}";
            glo.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, int idCliente, float quantidade, string codigo, string marca, string descricao, int idForn, int tipo)
        {
            string sql = $@"UPDATE Encomendas SET 
                idCliente = {idCliente}, 
                Quant = {quantidade}, 
                Codigo = '{codigo}',
                Marca = '{marca}', 
                Descricao = '{descricao}',
                idForn = {idForn},
                Tipo = '{tipo}'
                WHERE ID = {id}";
            glo.ExecutarComandoSQL(sql);
        }

        public void Atualiza(int iID, int idCliente, int idForn, string codigo, int quantidade, string marca, string obs)
        {
            StringBuilder alteracoes = new StringBuilder();
            if (idCliente > 0)
            {
                alteracoes.Append($"idCliente = {idCliente}, ");
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
            if (!string.IsNullOrEmpty(obs))
            {
                alteracoes.Append($"Obs = '{obs}', ");
            }
            if (alteracoes.Length > 0)
            {
                alteracoes.Length -= 2; // Remove the last comma and space
            }
            string sql = $@"UPDATE Encomendas SET {alteracoes} WHERE ID = {iID}";
            glo.ExecutarComandoSQL(sql);
        }
    }
}
