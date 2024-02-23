using System;
using System.Data;
using System.Data.OleDb;

namespace TeleBonifacio.dao
{
    public class ReciboDAO
    {

        public DataTable ValoresAPagar()
        {
            string query = @"Select Entregas.idVend, Vendedores.Nome, Sum(Entregas.VlNota) / 100 as Valor  
                        From Entregas
                        Inner Join Vendedores on Vendedores.ID = Entregas.idVend
                        Where Entregas.Pago is Null
                        and Entregas.idVend > 0
                        Group by Entregas.idVend, Vendedores.Nome
                        Order by Entregas.idVend, Vendedores.Nome ";

            using (OleDbConnection connection = new OleDbConnection(gen.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }

    }
}
