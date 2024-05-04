using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleBonifacio.dao
{
    public class RHDAO
    {

        public void AddHorario(int idFunc, DateTime txInMan, DateTime txFmMan, DateTime txInTrd, DateTime txFnTrd, string UID, DateTime dtHorario)
        {
            string sData = dtHorario.ToString("MM/dd/yyyy");
            String fdsql = $@"#{sData}#";
            string sql = @"INSERT INTO Horarios (idFunc, txInMan, txFmMan, txInTrd, txFnTrd, UID, Data) VALUES ("
                + idFunc + ", #"
                + txInMan.ToString("yyyy-MM-dd HH:mm:ss") + "#, #"
                + txFmMan.ToString("yyyy-MM-dd HH:mm:ss") + "#, #"
                + txInTrd.ToString("yyyy-MM-dd HH:mm:ss") + "#, #"
                + txFnTrd.ToString("yyyy-MM-dd HH:mm:ss") + "#, " 
                + glo.fa(UID) + ", "
                + fdsql + " )";
            glo.ExecutarComandoSQL(sql);
        }

        public DataTable getDados(DateTime? DT1, DateTime? DT2, int idFunc)
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"Select Horarios.ID, Horarios.uid, 
                Horarios.Data, Vendedores.Nome, 
                IIF(FORMAT(Horarios.txInMan, 'hh:mm') = '00:00', '', FORMAT(Horarios.txInMan, 'hh:mm')) AS InMan,
                IIF(FORMAT(Horarios.txFmMan, 'hh:mm') = '00:00', '', FORMAT(Horarios.txFmMan, 'hh:mm')) AS FmMan,
                IIF(FORMAT(Horarios.txInTrd, 'hh:mm') = '00:00', '', FORMAT(Horarios.txInTrd, 'hh:mm')) AS InTrd,
                IIF(FORMAT(Horarios.txFnTrd, 'hh:mm') = '00:00', '', FORMAT(Horarios.txFnTrd, 'hh:mm')) AS FnTrd,
                Vendedores.ID as FuncID
            From Horarios
            Inner Join Vendedores on Vendedores.ID = Horarios.idFunc ");
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
            DataTable dt = glo.ExecutarConsulta(query.ToString());
            return dt;
        }

        public void Exclui(int iID)
        {
            String sql = @"Delete From Horarios WHERE ID = " + iID.ToString();
            glo.ExecutarComandoSQL(sql);
        }

        public void EdHorario(int iID, int idFunc, DateTime dInMan, DateTime dFmMan, DateTime dInTrd, DateTime dFnTrd, DateTime dtHorario)
        {
            string sql = @"UPDATE Horarios SET 
                      idFunc = " + idFunc +
                          ", txInMan = '" + dInMan.ToString("HH:mm") + "'" +
                          ", txFmMan = '" + dFmMan.ToString("HH:mm") + "'" +
                          ", txInTrd = '" + dInTrd.ToString("HH:mm") + "'" +
                          ", txFnTrd = '" + dFnTrd.ToString("HH:mm") + "'" +
                          ", Data = '" + dtHorario.ToString("yyyy-MM-dd") + "'" +
                          " WHERE ID = " + iID;
            glo.ExecutarComandoSQL(sql);
        }
    }
}
