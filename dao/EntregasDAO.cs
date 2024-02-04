using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace TeleBonifacio.dao
{
    public class EntregasDAO
    {
        public EntregasDAO()
        {
            
        }

        public DataTable getDados(DateTime? DT)
        {
            StringBuilder query = new StringBuilder();
            int maxLength = 10; 
            query.Append($@"SELECT
                e.ID as Id, 
                e.Data, 
                m.Nome AS MotoBoy, 
                Space({maxLength} - Len(Format(e.Valor, 'Standard'))) & Format(e.Valor, 'Standard') AS Valor, 
                Space({maxLength} - Len(Format(e.Desconto, 'Standard'))) & Format(e.Desconto, 'Standard') AS Desconto,
                Space({maxLength} - Len(Format(e.VlNota, 'Standard'))) & Format(e.VlNota, 'Standard') AS Compra, 
                SWITCH(
                    e.idForma = 0, 'Anotado',
                    e.idForma = 1, 'Cartão',
                    e.idForma = 2, 'Dinheiro',
                    e.idForma = 3, 'Pix',
                    e.idForma = 4, 'Troca',
                    TRUE, 'Desconhecido'
                ) AS Pagamento,
                c.Nome AS Cliente,
                v.Nome AS Vendedor,  
                e.Obs,
                m.codi as idBoy,
                c.NrCli,
                e.idForma,
                e.idVend 
            FROM 
                (((Entregas e
                INNER JOIN Clientes c ON c.NrCli = e.idCliente)
                INNER JOIN Mecanicos m ON m.codi = e.idBoy)
                LEFT JOIN Vendedores v ON v.ID = e.idVend)");
            if (DT.HasValue)
            {
                DateTime dataInicio = DT.Value.Date;
                DateTime dataFim = dataInicio.AddDays(1).AddTicks(-1);
                string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
                string dataFimStr = dataFim.ToString("MM/dd/yyyy HH:mm:ss");
                query.AppendFormat(" WHERE e.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            }
            query.Append(" Order By e.ID desc");
            DataTable dt = ExecutarConsulta(query.ToString());
            return dt;
        }

        public DataTable getDadosC(DateTime? DT1, DateTime? DT2)
        {
            StringBuilder query = new StringBuilder();
            int maxLength = 10;
            query.Append($@"SELECT
                            m.Nome AS MotoBoy,
                            e.idForma,
                            SUM(e.Valor) AS Valor
                        FROM Entregas e
                        INNER JOIN Mecanicos m ON m.codi = e.idBoy ");
            DateTime dataInicio = DT1.Value.Date;
            DateTime dataFim = DT2.Value.Date;
            string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = dataFim.ToString("MM/dd/yyyy HH:mm:ss");
            query.AppendFormat(" WHERE e.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            query.Append(" GROUP BY m.Nome, e.idForma");
            query.Append(" ORDER BY m.Nome, e.idForma");
            DataTable dt = ExecutarConsulta(query.ToString());
            return dt;
        }

        private DataTable ExecutarConsulta(string query)
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(gen.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dataTable;
        }

        public void Adiciona(int idBoy, int idForma, float valor, int idcliente, float compra, string Obs, float desc, int idVend)
        {
            String sql = @"INSERT INTO Entregas (idCliente, idForma, idBoy, Valor, VlNota, Obs, Desconto, idVend, Data) VALUES ("
                + idcliente.ToString() + ", "
                + idForma.ToString() + ", "
                + idBoy.ToString() + ", "                
                + gen.sv(valor) + ", "
                + gen.sv(compra) + ", "
                + gen.fa(Obs) + ", "
                + gen.sv(desc) + ", "
                + idVend.ToString()  
                + ",Now)";
            ExecutarComandoSQL(sql);
        }

        private void ExecutarComandoSQL(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + gen.CaminhoBase + ";"))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Edita(int iID, int idBoy, int idForma, float valor, int idCliente, float compra, string obs, float desc, int idVend)
        {
            String sql = @"UPDATE Entregas SET 
                idCliente = " + idCliente.ToString() + 
                            ",idForma = " + idForma.ToString() + 
                            ",idBoy = " + idBoy.ToString() + 
                            ",Valor = " + gen.sv(valor) + 
                            ",VlNota = " + gen.sv(compra) + 
                            ",Obs = " + gen.fa(obs) + 
                            ",Desconto = " + gen.sv(desc) +
                            ",idVend = " + idVend.ToString() + 
                            " WHERE ID = " + iID.ToString();
            ExecutarComandoSQL(sql);
        }

    }
}
