using System;
using System.Data;
using System.Text;

namespace TeleBonifacio.dao
{
    public class FaltasDAO
    {
        public void Adiciona(int idBalconista, float quantidade, string codigo, string Marca, string UID)
        {
            string sql = $@"INSERT INTO Faltas (IDBalconista, Quant, Codigo, Marca, Data, UID) VALUES (
                {idBalconista}, 
                {quantidade}, 
                '{codigo}', 
                '{Marca}', 
                Now, 
                '{UID}')";
            glo.ExecutarComandoSQL(sql);
        }

        public DataTable getDados(int tipo)
        {
            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT F.ID, F.IDBalconista, F.Data, F.Codigo, F.Quant, F.Marca, 
                    V.Nome AS Balconista, F.UID, F.Tipo 
                FROM Faltas F
                INNER JOIN Vendedores V ON V.ID = F.IDBalconista ");
            if (tipo>0)
            {
                query.Append($@" Where F.Tipo = '{tipo}' ");
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

        public void SetaTipo(int iID, int iTpo)
        {
            string sql = $@"UPDATE Faltas SET Tipo = {iTpo} WHERE ID = {iID}";
            glo.ExecutarComandoSQL(sql);
        }
    }
}
