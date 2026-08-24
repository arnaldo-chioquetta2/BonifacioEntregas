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
            // Se já está usando ODBC, tenta direto.
            if (glo.ODBC)
            {
                try
                {
                    return odbc.ExecutarConsulta(sQL);
                }
                catch (Exception ex)
                {
                    glo.Loga($"Erro ODBC em ExecutarConsulta: {ex.Message} | SQL: {sQL}");

                    System.Windows.Forms.MessageBox.Show(
                        "Não foi possível acessar o banco de dados.\n\n" +
                        "Entre em contato com o suporte.",
                        "Erro de acesso ao banco de dados",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error
                    );

                    throw;
                }
            }

            // Primeiro tenta ADO
            try
            {
                return ADO.ExecutarConsulta(sQL);
            }
            catch (Exception exADO)
            {
                glo.Loga(
                    $"Erro ADO em ExecutarConsulta: {exADO.Message} | " +
                    $"Tentando via ODBC | SQL: {sQL}"
                );

                // Se ADO falhou, tenta ODBC
                try
                {
                    DataTable dados = odbc.ExecutarConsulta(sQL);

                    // ODBC funcionou, passa a usar ODBC nas próximas operações
                    glo.ODBC = true;

                    glo.Loga(
                        $"Consulta executada com sucesso via ODBC. " +
                        $"glo.ODBC alterado para true | SQL: {sQL}"
                    );

                    return dados;
                }
                catch (Exception exODBC)
                {
                    glo.Loga(
                        $"Erro também via ODBC em ExecutarConsulta: {exODBC.Message} | " +
                        $"Erro ADO: {exADO.Message} | SQL: {sQL}"
                    );

                    System.Windows.Forms.MessageBox.Show(
                        "Não foi possível acessar o banco de dados pelo modo normal nem pelo modo alternativo.\n\n" +
                        "Entre em contato com o suporte.",
                        "Erro de acesso ao banco de dados",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error
                    );

                    throw;
                }
            }
        }

        //public static DataTable ExecutarConsulta(string sQL)
        //{
        //    try
        //    {
        //        DataTable dados;
        //        if (glo.ODBC)
        //        {
        //            dados = odbc.ExecutarConsulta(sQL);
        //        }
        //        else
        //        {
        //            dados = ADO.ExecutarConsulta(sQL);
        //        }
        //        return dados;
        //    }
        //    catch (Exception ex)
        //    {
        //        glo.Loga($"Erro em ExecutarConsulta: {ex.Message} | SQL: {sQL}");
        //        throw;
        //    }
        //}        

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
            try
            {
                if (glo.ODBC)
                {
                    odbc.ExecutarComandoSQL(query, parameters);
                }
                else
                {
                    ADO.ExecutarComandoSQL(query, parameters);
                }
            }
            catch (Exception ex)
            {
                glo.Loga($"Erro em ExecutarComandoSQL: {ex.Message} | Query: {query}");
                throw;
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
