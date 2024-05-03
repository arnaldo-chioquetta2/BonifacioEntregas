using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleBonifacio.dao
{
    public class RHDAO
    {

        public void AddHorario(int idFunc, DateTime txInMan, DateTime txFmMan, DateTime txInTrd, DateTime txFnTrd, string UID)
        {
            string sql = @"INSERT INTO Horarios (idFunc, txInMan, txFmMan, txInTrd, txFnTrd) VALUES ("
                + idFunc + ", #"
                + txInMan.ToString("yyyy-MM-dd HH:mm:ss") + "#, #"
                + txFmMan.ToString("yyyy-MM-dd HH:mm:ss") + "#, #"
                + txInTrd.ToString("yyyy-MM-dd HH:mm:ss") + "#, #"
                + txFnTrd.ToString("yyyy-MM-dd HH:mm:ss") + "#)" 
                + glo.fa(UID) + ")";
            glo.ExecutarComandoSQL(sql);
        }

        //public void AddHorario(string nome, string loja)
        //{
        //    String sql = @"INSERT INTO Vendedores (Nome, Loja) VALUES ('"
        //        + glo.fa(nome) + "', '"
        //        + glo.fa(loja) + "')";
        //    glo.ExecutarComandoSQL(sql);
        //}

    }
}
