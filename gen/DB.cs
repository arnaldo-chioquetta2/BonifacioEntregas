using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using TeleBonifacio.gen;

namespace TeleBonifacio
{
    public static class DB
    {
        public static DataTable ExecutarConsulta(string sQL)
        {
            DataTable dados;
            if (glo.ODBC)
            {
                dados = odbc.ExecutarConsulta(sQL);
            } else
            {
                dados = ADO.ExecutarConsulta(sQL);
            }
            return dados;
        }

        public static DataTable ExecutarConsulta(string query, List<OleDbParameter> parametros = null)
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        if (parametros != null)
                        {
                            command.Parameters.AddRange(parametros.ToArray());
                        }
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao executar consulta: {ex.Message}");
                }
            }
            return dataTable;
        }


        public static void ExecutarComandoSQL(string query, List<OleDbParameter> parameters = null)
        {
            // COLOCAR O TRATAMENTO AQUI
            if (glo.ODBC)
            {
                odbc.ExecutarComandoSQL(query, parameters);
            } else
            {
                ADO.ExecutarComandoSQL(query, parameters);
            }
        }

        public static int ExecutarConsultaCount(string query)
        {
            if (glo.ODBC)
            {
                return odbc.ExecutarConsultaCount(query);
            }
            else
            {
                return ADO.ExecutarConsultaCount(query);
            }
        }

    }
}
