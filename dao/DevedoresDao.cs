using System;
using System.Data;

namespace TeleBonifacio.dao
{
    public class DevedoresDao
    {
        
        public void Exclui(int id)
        {
            string sql = $@"DELETE FROM Devedores WHERE ID = {id}";
            glo.Loga($@"ED,{id}");
            DB.ExecutarComandoSQL(sql);
        }

        public void Adiciona(int cliente, int status, DateTime dataCompra, DateTime vencimento, string nota, string observacao, decimal valor)
        {
            glo.Loga($@"ID,{cliente},{dataCompra},{status}, {vencimento}, {nota}, {observacao}, {valor}");
            string sql = $@"
        INSERT INTO Devedores (Cliente, DataCompra, Status, Vencimento, Nota, Observacao, Valor)
        VALUES (
            {cliente},
            #{dataCompra:MM/dd/yyyy}#,
            {status},
            #{vencimento:MM/dd/yyyy}#,
            '{nota.Replace("'", "''")}',
            '{observacao.Replace("'", "''")}',
            {glo.sv(valor)}
        )";
            DB.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, int cliente, DateTime dataCompra, int status, DateTime vencimento, string nota, string observacao, decimal valor)
        {
            glo.Loga($@"UD,{cliente},{dataCompra},{status}, {vencimento}, {nota}, {observacao}, {valor}");
            string sql = $@"
        UPDATE Devedores SET 
            Cliente = {cliente}, 
            DataCompra = '{dataCompra:yyyy-MM-dd HH:mm:ss}', 
            Status = {status}, 
            Vencimento = '{vencimento:yyyy-MM-dd HH:mm:ss}', 
            Nota = '{nota.Replace("'", "''")}', 
            Observacao = '{observacao.Replace("'", "''")}',
            Valor = {glo.sv(valor)}
        WHERE ID = {id}";

            DB.ExecutarComandoSQL(sql);
        }

        // 🔹 Altera o status da dívida
        public void MudaStatus(int id, int novoStatus)
        {
            string sql = $@"
                UPDATE Devedores SET 
                    Status = {novoStatus} 
                WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public DataTable getDados(int idCliente = 0)
        {
            string sWhe = "";
            if (idCliente > 0)
            {
                sWhe = $" WHERE Devedores.Cliente = {idCliente}";
            }
            string sql = $@"
SELECT 
    1 AS Contador,
    Devedores.ID, 
    Clientes.Nome AS ClienteNome, 
    Devedores.DataCompra, 
    Devedores.Status,
    '' AS StatusDescricao, 
    Devedores.Vencimento, 
    Devedores.Valor, 
    Devedores.Nota, 
    Devedores.Observacao,
    Devedores.Cliente
FROM Devedores
INNER JOIN Clientes ON Clientes.NrCli = Devedores.Cliente
{sWhe}
ORDER BY Clientes.Nome ASC";
            return DB.ExecutarConsulta(sql);
        }        

    }
}
