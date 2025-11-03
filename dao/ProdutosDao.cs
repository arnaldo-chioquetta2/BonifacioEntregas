using System.Data;
using System.Text;

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
            DB.ExecutarComandoSQL(sql);
        }

        public DataTable getDados(int tipo, int idForn, string codigo, string quantidade, string marca, string Obs, string Descr)
        {
            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT F.Compra, '' as Forn, F.ID, FORMAT(F.Data, 'dd/MM/yy') as Data, F.Codigo, F.Valor, F.Quant, F.Marca, F.Descricao, 
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
            if (codigo.Length > 0)
            {
                alteracoes.Append($" F.Codigo LIKE '{codigo}%' and "); // Modificado para usar LIKE com % após o valor de pesquisa
            }
            if (quantidade.Length > 0)
            {
                alteracoes.Append($@" F.Quant Like ' {quantidade}%' and ");
            }
            if (marca.Length > 0)
            {
                alteracoes.Append($" F.Marca LIKE '{marca}%' and "); // Modificado para usar LIKE com % após o valor de pesquisa
            }
            if (Obs.Length > 0)
            {
                alteracoes.Append($" F.Obs LIKE '{Obs}%' and "); // Modificado para usar LIKE com % após o valor de pesquisa
            }
            if (Descr.Length > 0)
            {
                alteracoes.Append($" F.Descricao LIKE '{Descr}%' and "); // Modificado para usar LIKE com % após o valor de pesquisa
            }
            if (alteracoes.Length > 0)
            {
                alteracoes.Length -= 4; // Remove o último 'and'
                query.Append($@" WHERE {alteracoes}");
            }
            query.Append(" ORDER BY F.Descricao ");
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            return dt;
        }

        public void Exclui(int id)
        {
            string sql = $@"DELETE FROM Produtos WHERE ID = {id} ";
            DB.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, int idBalconista, string quantidade, string codigo)
        {
            string sql = $@"UPDATE Produtos SET 
                IDBalconista = {idBalconista}, 
                Quant = '{quantidade}', 
                Codigo = '{codigo}'
                WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public void Atualiza(int iID, int iTpo, int idForn, string codigo, string quantidade, string marca, string Obs, string descr, float Valor)
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
            if (!string.IsNullOrEmpty(quantidade))
            {
                alteracoes.Append($"Quant = '{quantidade}', ");
            }
            if (!string.IsNullOrEmpty(marca))
            {
                alteracoes.Append($"Marca = '{marca}', ");
            }
            if (!string.IsNullOrEmpty(Obs))
            {
                alteracoes.Append($"Obs = '{Obs}', ");
            }            
            if (!string.IsNullOrEmpty(descr))
            {
                alteracoes.Append($"Descricao = '{descr}', ");
            }
            string sValor = glo.sv(Valor);
            string sql = $@"UPDATE Produtos SET {alteracoes} Valor = {sValor} WHERE ID = {iID}";
            DB.ExecutarComandoSQL(sql);
        }

        public void Comprou(int iID)
        {
            string sql = $@"UPDATE Produtos SET Compra = Now WHERE ID = {iID}";
            DB.ExecutarComandoSQL(sql);
        }

        public string VeSeJaTemAFalta(string codigo)
        {
            string query = $@"SELECT Count(*) FROM Produtos Where Codigo = '{codigo}' ";
            int count = DB.ExecutarConsultaCount(query);
            string ret = "";
            if (count > 0)
            {
                ret = "Já existe um falta com este código.";
            }
            return ret;
        }

        public void EmFalta(int gID)
        {
            DataTable encomendaData = DB.ExecutarConsulta($"SELECT * FROM Produtos WHERE ID = {gID} ");
            DataRow Row = encomendaData.Rows[0];
            string sValor = Row["Valor"].ToString();
            float fValor = glo.LeValor(sValor);
            sValor = glo.sv(fValor);
            string insertQuery = $@"INSERT INTO Faltas (Data, Quant, Codigo, Marca, UID, Tipo, Descricao, idForn, Obs, Valor) 
                        VALUES (Now, {Row["Quant"]}, '{Row["Codigo"]}', '{Row["Marca"]}', '{Row["UID"]}', '{Row["Tipo"]}', '{Row["Descricao"]}', {Row["idForn"]}, '{Row["Obs"]}', {sValor} )";
            DB.ExecutarComandoSQL(insertQuery);
            string sql = $@"DELETE FROM Produtos WHERE ID = {gID} ";
            DB.ExecutarComandoSQL(sql);
        }        
    }
}
