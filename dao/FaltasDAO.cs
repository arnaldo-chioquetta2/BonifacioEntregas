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

        public DataTable getDados(int tipo, int idForn, int Comprado)
        {
            StringBuilder query = new StringBuilder();

            query.Append(@"SELECT F.Compra, '' as Forn, F.ID, F.IDBalconista, F.Data, F.Codigo, F.Quant, F.Marca, F.Descricao, 
                    V.Nome AS Balconista, F.UID, F.Tipo, F.Tipo as TipoOrig, F.idForn, F.Obs 
                FROM Faltas F
                INNER JOIN Vendedores V ON V.ID = F.IDBalconista ");
            int SmIfs = tipo + idForn + Comprado;
            if (SmIfs > 0)
            {                
                string sTipo = "";
                string sForn = "";
                string sAnd1 = "";
                string sAnd2 = "";
                string sCompra = "";
                int Ifs = 0;
                if (tipo > 0)
                {
                    sTipo = $@" F.Tipo = '{tipo}' ";
                    Ifs++;
                }
                if (idForn > 0)
                {
                    sForn = $@" F.idForn = {idForn} ";
                    Ifs++;
                }
                if (Comprado > 0)
                {
                    sCompra = $@" F.Compra is not null ";
                    Ifs++;
                }
                if (Ifs>1)
                {
                    sAnd1 = " and ";
                    if (Ifs > 2)
                    {
                        sAnd2 = " and ";
                    }
                }                
                query.Append($@" Where {sTipo} {sAnd1} {sForn} {sAnd2} {sCompra} ");
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

        public void Atualiza(int iID, int iTpo, int idForn)
        {
            string sTipo = "";
            if (iTpo>0)
            {
                sTipo = $@" Tipo = {iTpo}";
            }
            string sForn = "";
            if (idForn > 0)
            {
                sForn = $@" idForn = {idForn}";
            }
            string Virgula = ((sTipo.Length > 0) && (sForn.Length>0)) ? " , " : "";
            string sql = $@"UPDATE Faltas SET {sTipo} {Virgula} {sForn} WHERE ID = {iID}";
            glo.ExecutarComandoSQL(sql);
        }

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
