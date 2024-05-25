using System;
using System.Data;
using System.Text;

namespace TeleBonifacio.dao
{
    public class FaltasDAO
    {
        public void Adiciona(int idBalconista, float quantidade, string codigo, string Marca, string Descr, string Obs, string UID)
        {
            string sql = $@"INSERT INTO Faltas (IDBalconista, Quant, Codigo, Marca, Data, Descricao, Obs, UID) VALUES (
                {idBalconista}, 
                {quantidade}, 
                '{codigo}', 
                '{Marca}', 
                Now, 
                '{Descr}', 
                '{Obs}', 
                '{UID}')";
            glo.ExecutarComandoSQL(sql); 
        }

        public DataTable getDados(int tipo, int idForn, int Comprado, string codigo, int quantidade, string marca, string Obs)
        {
            StringBuilder query = new StringBuilder();

            query.Append(@"SELECT F.Compra, '' as Forn, F.ID, F.IDBalconista, F.Data, F.Codigo, F.Quant, F.Marca, F.Descricao, 
                    V.Nome AS Balconista, F.UID, F.Tipo, F.Tipo as TipoOrig, F.idForn, F.Obs 
                FROM Faltas F
                INNER JOIN Vendedores V ON V.ID = F.IDBalconista ");
            StringBuilder alteracoes = new StringBuilder();
            if (tipo > 0)
            {
                alteracoes.Append($@" F.Tipo = {tipo} and ");
            }
            if (idForn > 0)
            {
                alteracoes.Append($@" F.idForn = {idForn} and ");
            }
            if (Comprado > 0)
            {
                alteracoes.Append(" F.Compra is not null and ");
            }
            if (codigo.Length>0)
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
            if (alteracoes.Length>0)
            {
                alteracoes.Length -= 4;
                query.Append($@" Where {alteracoes} ");
            }
            query.Append(" ORDER BY F.Data, V.Nome");
            DataTable dt = glo.ExecutarConsulta(query.ToString());
            return dt;
        }

        public void Exclui(int id)
        {
            string sql = $@"DELETE FROM Faltas WHERE ID = {id}";
            glo.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, int idBalconista, float quantidade, string codigo)
        {
            string sql = $@"UPDATE Faltas SET 
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
            string sql = $@"UPDATE Faltas SET {alteracoes} WHERE ID = {iID}";
            glo.ExecutarComandoSQL(sql);
        }

        //public void Atualiza(int iID, int iTpo, int idForn, string codigo, int quantidade, string marca, string Obs)
        //{
        //    string sTipo = "";
        //    if (iTpo>0)
        //    {
        //        sTipo = $@" Tipo = {iTpo}, ";
        //    }
        //    string sForn = "";
        //    if (idForn > 0)
        //    {
        //        sForn = $@" idForn = {idForn}, ";
        //    }
        //    if (codigo.Length>0)
        //    {
        //        codigo = $@" Codigo = '{codigo}', ";
        //    }
        //    string sQuant = "";
        //    if (quantidade>-1)
        //    {
        //        sQuant = $@" Quant = {quantidade}, ";
        //    }
        //    if (marca.Length > 0)
        //    {
        //        marca = $@" Marca = '{marca}', ";
        //    }
        //    if (Obs.Length>0)
        //    {                
        //        marca = $@" Obs = '{Obs}', ";
        //    }
        //    string Alterar = $@" { sTipo } { sForn} {codigo} {sQuant} {marca} {marca} ";
        //    // RETIRAR A ÚLTIMA VIRGULA DA STRING
        //    string sql = $@"UPDATE Faltas SET {Alterar} WHERE ID = {iID}";
        //    glo.ExecutarComandoSQL(sql);
        //}

        public void Comprou(int iID)
        {
            string sql = $@"UPDATE Faltas SET Compra = Now WHERE ID = {iID}";
            glo.ExecutarComandoSQL(sql);
        }
        
        public string VeSeJaTemAFalta(string codigo)
        {
            string query = $@"SELECT Count(*) FROM Faltas Where Codigo = '{codigo}' ";
            int count = glo.ExecutarConsultaCount(query);
            string ret = "";
            if (count > 0)
            {
                ret= "Já existe um falta com este código.";
            }
            return ret;
        }

    }
}
