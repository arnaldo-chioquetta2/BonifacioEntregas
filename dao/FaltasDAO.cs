using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace TeleBonifacio.dao
{
    public class FaltasDAO
    {
        public void Adiciona(int idBalconista, string quantidade, string codigo, string Marca, string Descr, string Obs, int idForn, int idTipo, string UID)
        {
            string sql = $@"INSERT INTO Faltas (IDBalconista, Quant, Codigo, Marca, Data, Descricao, Obs, Tipo, idForn, UID) VALUES (
                {idBalconista}, 
                '{quantidade}', 
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

        public DataTable getDados(int tipo, int idForn, int Comprado, string codigo, string quantidade, string marca, string Obs, int idVendedor, int EmFalta, string Descr)
        {
            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT F.Compra, '' as Forn, F.ID, F.IDBalconista, F.Data, F.Codigo, F.Quant, F.Marca, F.Descricao, 
            V.Nome AS Balconista, F.UID, F.Tipo, F.Tipo as TipoOrig, F.idForn, F.Valor, F.Obs 
        FROM Faltas F
        LEFT JOIN Vendedores V ON V.ID = F.IDBalconista ");
            StringBuilder alteracoes = new StringBuilder();
            if (tipo > 0)
            {
                alteracoes.Append($@" F.Tipo = '{tipo}' and ");
            }
            if (idForn > 0)
            {
                alteracoes.Append($@" F.idForn = {idForn} and ");
            }
            if (Comprado > 0)
            {
                alteracoes.Append(" F.Compra is not null and ");
            }
            if (codigo.Length > 0)
            {
                alteracoes.Append($" F.Codigo LIKE '{codigo}%' and "); 
            }
            if (quantidade.Length > 0)
            {
                alteracoes.Append($" F.Quant Like '{quantidade}%' and ");
            }
            if (marca.Length > 0)
            {
                alteracoes.Append($" F.Marca LIKE '{marca}%' and "); 
            }
            if (Obs.Length > 0)
            {
                alteracoes.Append($" F.Obs LIKE '{Obs}%' and "); 
            }
            if (idVendedor > 0)
            {
                alteracoes.Append($@" F.IDBalconista = {idVendedor} and ");
            }
            if (EmFalta > 0)
            {
                alteracoes.Append($@" F.Tipo = '8' and ");
            }
            if (Descr.Length > 0)
            {
                alteracoes.Append($" F.Descricao LIKE '{Descr}%' and "); 
            }
            if (alteracoes.Length > 0)
            {
                alteracoes.Length -= 4; 
                query.Append($@" WHERE {alteracoes} ");
            }
            query.Append(" ORDER BY F.Data DESC, V.Nome");
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            return dt;
        }

        public void Exclui(int id)
        {
            string sql = $@"DELETE FROM Faltas WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, int idBalconista, string quantidade, string codigo)
        {
            string sql = $@"UPDATE Faltas SET 
                IDBalconista = {idBalconista}, 
                Quant = '{quantidade}', 
                Codigo = '{codigo}'
                WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public void Atualiza(int iID, int iTpo, int idForn, string codigo, string quantidade, string marca, string Obs, string Descr, float Valor)
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
            if (quantidade.Length > 0)
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
            if (!string.IsNullOrEmpty(Descr))
            {
                alteracoes.Append($"Descricao = '{Descr}', ");
            }
            // alteracoes.Length -= 2;
            string sValor = glo.sv(Valor);
            string sql = $@"UPDATE Faltas SET {alteracoes} Valor = {sValor} WHERE ID = {iID}";
            DB.ExecutarComandoSQL(sql);
        }

        public void Comprou(int iID, float Valor)
        {
            DataTable faltaData = DB.ExecutarConsulta($"SELECT * FROM Faltas WHERE ID = {iID}");
            DataRow faltaRow = faltaData.Rows[0];
            string UID = glo.GenerateUID();
            string sValor = glo.sv(Valor);
            int idForn = (faltaRow["idForn"].ToString().Length==0) ? 0 : Convert.ToInt16(faltaRow["idForn"]);
            string insertQuery = $@"INSERT INTO Produtos (Data, Quant, Codigo, Marca, UID, Tipo, Compra, Descricao, idForn, Obs, Valor) 
                            VALUES (Now, '{faltaRow["Quant"]}', '{faltaRow["Codigo"]}', '{faltaRow["Marca"]}', '{UID}', '{faltaRow["Tipo"]}', Now(), '{faltaRow["Descricao"]}', {idForn}, '{faltaRow["Obs"]}', {sValor} ) ";
            DB.ExecutarComandoSQL(insertQuery);
            string updateFaltaQuery = $@"Delete From Faltas WHERE ID = {iID}";
            DB.ExecutarComandoSQL(updateFaltaQuery);
        }

        public string VeSeJaTem(string codigo)
        {
            string query = $@"SELECT Count(*) FROM Faltas Where Codigo = '{codigo}' ";
            int count = DB.ExecutarConsultaCount(query);
            string ret = "";
            if (count > 0)
            {
                ret= "Já existe um falta com este código.";
            } else
            {
                string queryP = $@"SELECT FORMAT([Compra], 'dd/MM/yyyy') AS CompraFormatada FROM Produtos WHERE Codigo = '{codigo}' ";
                DataTable dados = DB.ExecutarConsulta(queryP);
                if (dados.Rows.Count > 0)
                {
                    DateTime? dataCompra = Convert.ToDateTime(dados.Rows[0]["CompraFormatada"]);
                    ret = $"Este produto foi comprado em {dataCompra.Value.ToShortDateString()}";
                }
            }
            return ret;
        }

    }
}
