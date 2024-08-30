using System;
using System.Data;
using System.Text;

namespace TeleBonifacio.dao
{
    public class RHDAO
    {

        public void AddHorario(int idFunc, TimeSpan txInMan, TimeSpan txFmMan, TimeSpan txInTrd, TimeSpan txFnTrd, TimeSpan txInCafeMan, TimeSpan txFmCafeMan, TimeSpan txInCafeTrd, TimeSpan txFmCafeTrd, string UID, DateTime dtHorario)
        {
            string sData = dtHorario.ToString("MM/dd/yyyy");
            String fdsql = $@"#{sData}#";
            string sInMan = "#2000-01-01 " + txInMan.ToString(@"hh\:mm\:ss") + "#, ";
            string sFmMan = "#2000-01-01 " + txFmMan.ToString(@"hh\:mm\:ss") + "#, ";
            string sInTrd = "#2000-01-01 " + txInTrd.ToString(@"hh\:mm\:ss") + "#, ";
            string sFnTrd = "#2000-01-01 " + txFnTrd.ToString(@"hh\:mm\:ss") + "#, ";
            string sInCafeMan = "#2000-01-01 " + txInCafeMan.ToString(@"hh\:mm\:ss") + "#, ";
            string sFmCafeMan = "#2000-01-01 " + txFmCafeMan.ToString(@"hh\:mm\:ss") + "#, ";
            string sInCafeTrd = "#2000-01-01 " + txInCafeTrd.ToString(@"hh\:mm\:ss") + "#, ";
            string sFmCafeTrd = "#2000-01-01 " + txFmCafeTrd.ToString(@"hh\:mm\:ss") + "#, ";
            string sql = @"INSERT INTO Horarios (idFunc, txInMan, txFmMan, txInTrd, txFnTrd, txInCafeMan, txFmCafeMan, txInCafeTrd, txFmCafeTrd, UID, Data) VALUES ("
                + idFunc + ", "
                + sInMan
                + sFmMan
                + sInTrd
                + sFnTrd
                + sInCafeMan
                + sFmCafeMan
                + sInCafeTrd
                + sFmCafeTrd
                + glo.fa(UID) + ", "
                + fdsql + " )";
            DB.ExecutarComandoSQL(sql);
        }

        public DataTable getDados(DateTime? DT1, DateTime? DT2, int idFunc)
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"SELECT Horarios.ID, Horarios.uid, 
                Horarios.Data, Vendedores.usuario as Nome, 
                IIF(FORMAT(Horarios.txInMan, 'hh:mm') = '00:00', '', FORMAT(Horarios.txInMan, 'hh:mm')) AS InMan,
                IIF(FORMAT(Horarios.txInCafeMan, 'hh:mm') = '00:00', '', FORMAT(Horarios.txInCafeMan, 'hh:mm')) AS InCafeMan,
                IIF(FORMAT(Horarios.txFmCafeMan, 'hh:mm') = '00:00', '', FORMAT(Horarios.txFmCafeMan, 'hh:mm')) AS FmCafeMan,
                '' As CafMan,
                IIF(FORMAT(Horarios.txFmMan, 'hh:mm') = '00:00', '', FORMAT(Horarios.txFmMan, 'hh:mm')) AS FmMan,                                
                IIF(FORMAT(Horarios.txInTrd, 'hh:mm') = '00:00', '', FORMAT(Horarios.txInTrd, 'hh:mm')) AS InTrd,
                IIF(FORMAT(Horarios.txInCafeTrd, 'hh:mm') = '00:00', '', FORMAT(Horarios.txInCafeTrd, 'hh:mm')) AS InCafeTrd,
                IIF(FORMAT(Horarios.txFmCafeTrd, 'hh:mm') = '00:00', '', FORMAT(Horarios.txFmCafeTrd, 'hh:mm')) AS FmCafeTrd,
                '' As CafTar,
                IIF(FORMAT(Horarios.txFnTrd, 'hh:mm') = '00:00', '', FORMAT(Horarios.txFnTrd, 'hh:mm')) AS FnTrd,
                Vendedores.ID as FuncID
            FROM Horarios
            INNER JOIN Vendedores ON Vendedores.ID = Horarios.idFunc ");
            DateTime dataInicio = DT1.Value.Date;
            DateTime dataFim = DT2.Value.Date;
            string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");
            query.AppendFormat(" WHERE Horarios.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            if (idFunc>0)
            {
                query.AppendFormat(" And Horarios.idFunc = " + idFunc.ToString());
            }            
            query.Append(" Order By Vendedores.Nome, Horarios.Data");
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            return dt;
        }

        public void Exclui(int iID)
        {
            String sql = @"Delete From Horarios WHERE ID = " + iID.ToString();
            DB.ExecutarComandoSQL(sql);
        }

        public void EdHorario(int iID, int idFunc, TimeSpan dInMan, TimeSpan dFmMan, TimeSpan dInTrd, TimeSpan dFnTrd, TimeSpan dInCafeMan, TimeSpan dFmCafeMan, TimeSpan dInCafeTrd, TimeSpan dFmCafeTrd, DateTime dtHorario)
        {
            string sHI = ", txInMan = '" + dInMan.ToString(@"hh\:mm") + "'";
            string sHF = ", txFmMan = '" + dFmMan.ToString(@"hh\:mm") + "'";
            string sTI = ", txInTrd = '" + dInTrd.ToString(@"hh\:mm") + "'";
            string sTF = ", txFnTrd = '" + dFnTrd.ToString(@"hh\:mm") + "'";
            string sInCafeMan = ", txInCafeMan = '" + dInCafeMan.ToString(@"hh\:mm") + "'";
            string sFmCafeMan = ", txFmCafeMan = '" + dFmCafeMan.ToString(@"hh\:mm") + "'";
            string sInCafeTrd = ", txInCafeTrd = '" + dInCafeTrd.ToString(@"hh\:mm") + "'";
            string sFmCafeTrd = ", txFmCafeTrd = '" + dFmCafeTrd.ToString(@"hh\:mm") + "'";
            string sql = @"UPDATE Horarios SET 
                  idFunc = " + idFunc +
                              sHI +
                              sHF +
                              sTI +
                              sTF +
                              sInCafeMan +
                              sFmCafeMan +
                              sInCafeTrd +
                              sFmCafeTrd +
                              ", Data = '" + dtHorario.ToString("yyyy-MM-dd") + "'" +
                              " WHERE ID = " + iID;
            DB.ExecutarComandoSQL(sql);
        }

    }
}
