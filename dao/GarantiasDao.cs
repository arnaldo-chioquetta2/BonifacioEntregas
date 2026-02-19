using System;
using System.Data;

namespace TeleBonifacio.dao
{
    public class GarantiasDao
    {
        public void Adiciona(DateTime data, int idForn, string nota, DateTime prometida, DateTime dataDoForn)
        {
            string sql = $@"INSERT INTO Garantias (Data, idForn, Nota, Prometida, DataDoForn) VALUES (
                '{data.ToString("yyyy-MM-dd HH:mm:ss")}', 
                {idForn}, 
                '{nota}', 
                '{prometida.ToString("yyyy-MM-dd HH:mm:ss")}', 
                '{dataDoForn.ToString("yyyy-MM-dd HH:mm:ss")}')";
            DB.ExecutarComandoSQL(sql);
        }

        public void Exclui(int id)
        {
            string sql = $@"DELETE FROM Garantias WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, DateTime data, int idForn, string nota, DateTime prometida, DateTime dataDoForn)
        {
            string sql = $@"UPDATE Garantias SET 
                Data = '{data.ToString("yyyy-MM-dd HH:mm:ss")}', 
                idForn = {idForn}, 
                Nota = '{nota}', 
                Prometida = '{prometida.ToString("yyyy-MM-dd HH:mm:ss")}', 
                DataDoForn = '{dataDoForn.ToString("yyyy-MM-dd HH:mm:ss")}'
                WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public void MudaForn(int id, int idForn)
        {
            string sql = $@"UPDATE Garantias SET 
                idForn = {idForn} 
                WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public DataTable getDados(int idForn)
        {
            string sWhe = "";
            if (idForn > 0)
            {
                sWhe = " Where Garantias.idForn = " + idForn.ToString();
            }
            string sql = $@"SELECT Garantias.*, Fornecedores.Nome as Forn
                            FROM Garantias
                            Inner Join Fornecedores on Fornecedores.idForn = Garantias.idForn
                            {sWhe}
                            ORDER BY Garantias.ID DESC ";
            DataTable dt = DB.ExecutarConsulta(sql);
            return dt;
        }
    }
}
