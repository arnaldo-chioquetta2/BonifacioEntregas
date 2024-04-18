using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;

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
                LEFT JOIN Clientes c ON c.NrCli = e.idCliente)
                LEFT JOIN Mecanicos m ON m.codi = e.idBoy)
                LEFT JOIN Vendedores v ON v.ID = e.idVend)");
            if (DT.HasValue)
            {
                DateTime dataInicio = DT.Value.Date;
                DateTime dataFim = dataInicio.AddDays(1).AddTicks(-1);
                string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
                string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");
                query.AppendFormat(" WHERE e.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            }
            query.Append(" Order By e.ID desc");
            DataTable dt = ExecutarConsulta(query.ToString());
            return dt;
        }

        public DataTable getDadosC(DateTime? DT1, DateTime? DT2)
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"SELECT
                            m.Nome AS MotoBoy,
                            e.idForma,
                            SUM(e.Valor) AS Valor
                        FROM Entregas e
                        INNER JOIN Mecanicos m ON m.codi = e.idBoy ");
            DateTime dataInicio = DT1.Value.Date;
            DateTime dataFim = DT2.Value.Date;
            string dataInicioStr = dataInicio.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = dataFim.ToString("MM/dd/yyyy 23:59:59");
            query.AppendFormat(" WHERE m.Oper = 3 and e.Data BETWEEN #{0}# AND #{1}#", dataInicioStr, dataFimStr);
            query.Append(" GROUP BY m.Nome, e.idForma");
            query.Append(" ORDER BY m.Nome, e.idForma");
            DataTable dt = ExecutarConsulta(query.ToString());
            return dt;
        }

        public DataTable getQuantidades(DateTime DT1, DateTime DT2, int indiceSelecionado, string Tabela)
        {
            string grp = "";
            string sq = "";
            string or = "";
            string wh = ""; 
            if (indiceSelecionado < 2)
            {
                sq = "SELECT DateSerial(Year(Data), Month(Data), Day(Data)) AS Dado, COUNT(*) AS QTD ";
                grp = "GROUP BY DateSerial(Year(Data), Month(Data), Day(Data)) ";
                or = "ORDER BY DateSerial(Year(Data), Month(Data), Day(Data)) ASC";
            }
            else
            {
                sq = "SELECT Format(DateSerial(Year(Data), Month(Data), 1), 'dd/mm/yyyy') AS Dado, COUNT(*) AS QTD ";
                grp = "GROUP BY Format(DateSerial(Year(Data), Month(Data), 1), 'dd/mm/yyyy') ";
                or = "ORDER BY Format(DateSerial(Year(Data), Month(Data), 1), 'dd/mm/yyyy')";
            }
            string dataInicioStr = DT1.ToString("MM/dd/yyyy");
            string dataFimStr = DT2.AddDays(1).ToString("MM/dd/yyyy");
            wh = "WHERE Data >= #" + dataInicioStr + "# AND Data <= #" + dataFimStr + "# ";
            StringBuilder query = new StringBuilder();
            query.AppendLine(sq);
            query.AppendLine("FROM "+ Tabela);
            query.AppendLine(wh); 
            query.AppendLine(grp);
            query.AppendLine(or);
            DataTable dt = ExecutarConsulta(query.ToString());
            return dt;
        }

        public DataTable GraficEntregadores(DateTime DT1, DateTime DT2)
        {
            string dataInicioStr = DT1.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = DT2.ToString("MM/dd/yyyy 23:59:59");
            string query = ($@"SELECT DateValue(Data) AS DataTruncada,
                                SUM(VlNota) - SUM(VlNota / 1.7) AS LucroBruto,
                                SUM(Valor) AS ValorTotalEntrega,
                                ((SUM(VlNota) - SUM(VlNota / 1.7)) - SUM(Valor)) -(SUM(VlNota) * 0.01) AS LucroTeleentrega,
                                SUM(VlNota) AS VlNota 
                            FROM Entregas
                            WHERE Data BETWEEN #{dataInicioStr}# AND #{dataFimStr}# 
                            GROUP BY DateValue(Data) ");
            Console.WriteLine(query);
            DataTable dt = ExecutarConsulta(query);
            return dt;
        }

        public DataTable GraficVendas(DateTime DT1, DateTime DT2)
        {
            string dataInicioStr = DT1.ToString("MM/dd/yyyy HH:mm:ss");
            string dataFimStr = DT2.ToString("MM/dd/yyyy 23:59:59");
            string query = ($@"SELECT FORMAT([Data], 'dd/mm/yyyy') AS DataFormatada,
                                SUM(Val([VlNota])) AS TotalVendas
                            FROM Entregas
                            WHERE Data BETWEEN #{dataInicioStr}# AND #{dataFimStr}# 
                            GROUP BY FORMAT([Data], 'dd/mm/yyyy') ");
            DataTable dt = ExecutarConsulta(query);
            return dt;
        }

        public DataTable ExecutarConsulta(string query)
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
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
                + glo.sv(valor) + ", "
                + glo.sv(compra) + ", "
                + glo.fa(Obs) + ", "
                + glo.sv(desc) + ", "
                + idVend.ToString()  
                + ",Now)";
            ExecutarComandoSQL(sql);
        }

        private void ExecutarComandoSQL(string query)
        {
            using (OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + glo.CaminhoBase + ";"))
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
                            ",Valor = " + glo.sv(valor) + 
                            ",VlNota = " + glo.sv(compra) + 
                            ",Obs = " + glo.fa(obs) + 
                            ",Desconto = " + glo.sv(desc) +
                            ",idVend = " + idVend.ToString() + 
                            " WHERE ID = " + iID.ToString();
            ExecutarComandoSQL(sql);
        }

    }
}
