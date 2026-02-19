using System;
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

        // v1.1 – 12/02/2026
        // Inclusão de filtro opcional por período (Data DE / ATÉ)
        // Mantido comportamento original quando usarPeriodo = false
        // Log de versão incluído
        //
        // v1.0 – Método original sem filtro por período

        public DataTable getDados(
            int tipo,
            int idForn,
            string codigo,
            string quantidade,
            string marca,
            string Obs,
            string Descr,
            DateTime? dataDe,
            DateTime? dataAte,
            bool usarPeriodo)
        {
            const string VERSAO = "ProdutosDAO.getDados v1.1";

            try
            {
                glo.Loga($"{VERSAO} executando");

                StringBuilder query = new StringBuilder();
                query.Append(@"SELECT F.Compra, '' as Forn, F.ID, 
                       FORMAT(F.Data, 'dd/MM/yy') as Data, 
                       F.Codigo, F.Valor, F.Quant, F.Marca, 
                       F.Descricao, F.UID, F.Tipo, 
                       F.Tipo as TipoOrig, F.idForn, F.Obs 
                       FROM Produtos F ");

                StringBuilder alteracoes = new StringBuilder();

                // ===== FILTROS EXISTENTES (v1.0) =====
                if (tipo > 0)
                    alteracoes.Append($@" F.Tipo = '{tipo}' and ");

                if (idForn > 0)
                    alteracoes.Append($@" F.idForn = {idForn} and ");

                if (!string.IsNullOrWhiteSpace(codigo))
                    alteracoes.Append($" F.Codigo LIKE '{codigo}%' and ");

                if (!string.IsNullOrWhiteSpace(quantidade))
                    alteracoes.Append($" F.Quant LIKE '{quantidade}%' and ");

                if (!string.IsNullOrWhiteSpace(marca))
                    alteracoes.Append($" F.Marca LIKE '{marca}%' and ");

                if (!string.IsNullOrWhiteSpace(Obs))
                    alteracoes.Append($" F.Obs LIKE '{Obs}%' and ");

                if (!string.IsNullOrWhiteSpace(Descr))
                    alteracoes.Append($" F.Descricao LIKE '{Descr}%' and ");

                // ===== NOVO FILTRO – v1.1 =====
                if (usarPeriodo && dataDe.HasValue && dataAte.HasValue)
                {
                    string dtDe = dataDe.Value.ToString("MM/dd/yyyy HH:mm:ss");
                    string dtAte = dataAte.Value.ToString("MM/dd/yyyy HH:mm:ss");

                    alteracoes.Append($" F.Data BETWEEN #{dtDe}# AND #{dtAte}# and ");
                }

                if (alteracoes.Length > 0)
                {
                    alteracoes.Length -= 4; // Remove último 'and'
                    query.Append($@" WHERE {alteracoes}");
                }

                query.Append(" ORDER BY F.Descricao ");

                return DB.ExecutarConsulta(query.ToString());
            }
            catch (Exception ex)
            {
                glo.Loga($"Erro em {VERSAO}: {ex.Message}");
                throw;
            }
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
