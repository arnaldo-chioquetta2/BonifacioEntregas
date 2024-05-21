using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TeleBonifacio.tb;

namespace TeleBonifacio.dao
{
    public class TpoFaltaDAO : BaseDAO
    {
        public void Adiciona(string descricao)
        {
            string sql = $"INSERT INTO TpoFalta (Nome) VALUES ('{descricao}')";
            glo.ExecutarComandoSQL(sql);
        }

        public override IDataEntity Apagar(int direcao, IDataEntity entidade)
        {
            // 
            return null;
        }

        public override DataTable getDados()
        {
            return null;
        }

        public override DataTable GetDadosOrdenados(string filtro = "", string ordem = "")
        {
            string query = @"SELECT IdFalta AS id, Nome  
                FROM TpoFalta 
                Order By Nome ";
            DataTable dt = glo.ExecutarConsulta(query);
            return dt;
        }

        public override IDataEntity GetEsse()
        {
            throw new NotImplementedException();
        }

        public override IDataEntity GetPeloID(string id)
        {
            throw new NotImplementedException();
        }

        public override object GetUltimo()
        {
            throw new NotImplementedException();
        }

        public override void Grava(object obj)
        {
            throw new NotImplementedException();
        }

        public override IDataEntity ParaFrente()
        {
            throw new NotImplementedException();
        }

        public override IDataEntity ParaTraz()
        {
            throw new NotImplementedException();
        }

        public override string VeSeJaTem(object obj)
        {
            throw new NotImplementedException();
        }

        public List<tb.TpoFalta> getTipos()
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT IdFalta, Nome FROM TpoFalta");
            DataTable dt = glo.ExecutarConsulta(query.ToString());
            List<tb.TpoFalta> tipos = new List<tb.TpoFalta>();
            foreach (DataRow row in dt.Rows)
            {
                tipos.Add(new TpoFalta { Id = Convert.ToInt32(row["IdFalta"]), Nome = row["Nome"].ToString() });
            }
            return tipos;
        }


    }
}