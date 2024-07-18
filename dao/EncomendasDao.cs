using System;
using System.Data;
using System.Text;

namespace TeleBonifacio.dao
{
    public class EncomendasDao
    {

        public DataTable getDados(int tipo, int idForn, string codigo, string quantidade, string marca, string Obs, string Descr)
        {
            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT E.ID, IIf(E.Nome IS NULL OR E.Nome = '', Clientes.Nome, E.Nome) AS Nome, FORMAT(E.Data, 'dd/MM/yy') as Data, E.Codigo, E.Valor, E.Quant, 
                   E.Marca, E.Descricao, E.UID, E.Tipo, FORMAT(E.Compra, 'dd/MM/yy') as Compra, '' as Forn, E.idForn, E.Obs, E.idCliente, Clientes.Telefone, E.DtPrometida, E.idCliente 
                   FROM Encomendas E 
                   LEFT JOIN Clientes ON Clientes.NrCli = E.idCliente ");
            StringBuilder alteracoes = new StringBuilder();
            if (tipo > 0)
            {
                alteracoes.Append($@" E.Tipo = '{tipo}' AND ");
            }
            if (idForn > 0)
            {
                alteracoes.Append($@" E.idForn = {idForn} AND ");
            }
            if (!string.IsNullOrEmpty(codigo))
            {
                alteracoes.Append($" E.Codigo LIKE '{codigo}%' AND "); 
            }
            if (!string.IsNullOrEmpty(quantidade))
            {
                alteracoes.Append($" E.Quant LIKE '{quantidade}%' AND ");
            }
            if (!string.IsNullOrEmpty(marca))
            {
                alteracoes.Append($" E.Marca LIKE '{marca}%' AND "); // Modificado para usar LIKE com % após o valor de pesquisa
            }
            if (!string.IsNullOrEmpty(Obs))
            {
                alteracoes.Append($" E.Obs LIKE '{Obs}%'' AND "); // Modificado para usar LIKE com % após o valor de pesquisa
            }
            if (Descr.Length > 0)
            {
                alteracoes.Append($" E.Descricao LIKE '{Descr}%' AND "); // Modificado para usar LIKE com % após o valor de pesquisa
            }
            if (alteracoes.Length > 0)
            {
                alteracoes.Length -= 4; // Remove o último 'AND'
                query.Append($@" WHERE {alteracoes}");
            }
            query.Append(" ORDER BY E.Data DESC");
            DataTable dt = DB.ExecutarConsulta(query.ToString());
            return dt;
        }

        public void ConfirmaEncomenda(int iID, string Nome, string Fone, string NovaDesc, DateTime DtAgora, DateTime DtEnc, DateTime HoraEntrega, string codigo, decimal Valor, int idFornNvProd, int tbIndex)
        {
            int idCliente = glo.IdAdicionado;
            string sCompra = DtEnc.ToString("yyyy-MM-dd");
            string sDtAgora = DtAgora.ToString("yyyy-MM-dd");
            string sValor = glo.sv(Valor);
            string sHoraEntrega = HoraEntrega.ToString("HH:mm:ss");
            if (iID== 0)
            {

                if (idCliente == 0)
                {
                    string insertClienteQuery = $@"INSERT INTO Clientes (Nome, Telefone, Data) VALUES (
                        '{Nome}', '{Fone}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' )";
                    DB.ExecutarComandoSQL(insertClienteQuery);
                    string queryCount = $"SELECT Max(NrCli) as NrCli FROM Clientes ";
                    DataTable dt = DB.ExecutarConsulta(queryCount); 
                    idCliente = (int)dt.Rows[0]["NrCli"];
                    glo.IdAdicionado = idCliente; 
                }
                if (idCliente==-1)
                {
                    idCliente = 0;
                }
                string UID = glo.GenerateUID();
                string insertQuery = $@"INSERT INTO Encomendas (idCliente, Data, UID, Descricao, Nome, Telefone, Compra, codigo, Valor, idForn, HoraEntrega, DtPrometida) 
                        VALUES ({idCliente}, Now, '{UID}', '{NovaDesc}', '{Nome}', '{Fone}', '{sCompra}', '{codigo}', {sValor}, {idFornNvProd}, '{sHoraEntrega}', '{sDtAgora}' )";
                DB.ExecutarComandoSQL(insertQuery);
            }
            else
            {
                string nmTb = "";
                if (tbIndex == 0)
                {
                    nmTb = "Faltas";
                }
                else
                {
                    nmTb = "Produtos";
                }
                string SQL = $"SELECT * From {nmTb} WHERE ID = {iID} ";
                DataTable encomendaData = DB.ExecutarConsulta(SQL);
                DataRow encomendaRow = encomendaData.Rows[0];
                int idForn = (encomendaRow["idForn"].ToString().Length == 0) ? 0 : Convert.ToInt16(encomendaRow["idForn"]);
                string insertQuery = $@"INSERT INTO Encomendas (idCliente, Data, Quant, Codigo, Marca, UID, Tipo, Compra, Descricao, idForn, Obs, Nome, 
                                            Telefone, Valor, HoraEntrega, DtPrometida) 
                                        VALUES ({idCliente}, '{sDtAgora}', '{encomendaRow["Quant"]}', '{encomendaRow["Codigo"]}', '{encomendaRow["Marca"]}', 
                                            '{encomendaRow["UID"]}', '{encomendaRow["Tipo"]}', '{sCompra}', '{encomendaRow["Descricao"]}', {idForn}, '{encomendaRow["Obs"]}',
                                            '{Nome}' ,'{Fone}', {sValor}, '{sHoraEntrega}', '{sDtAgora}' )";
                DB.ExecutarComandoSQL(insertQuery);
                string updateFaltaQuery = $@"DELETE FROM {nmTb} WHERE ID = {iID}";
                DB.ExecutarComandoSQL(updateFaltaQuery);
            }
        }

        public void Exclui(int id)
        {
            string sql = $@"DELETE FROM Encomendas WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public void Edita(int id, int idCliente, string codigo, string descricao, int idForn, DateTime dtPrometida, DateTime horaEntrega, decimal Valor)
        {
            string sql = $@"UPDATE Encomendas SET 
                            idCliente = {idCliente}, 
                            Codigo = '{codigo}',
                            Descricao = '{descricao}',
                            idForn = {idForn},
                            Compra = '{dtPrometida.ToString("yyyy-MM-dd HH:mm:ss")}',
                            HoraEntrega = '{horaEntrega.ToString("yyyy-MM-dd HH:mm:ss")}',  
                            Valor = {glo.sv(Valor)} 
                            WHERE ID = {id}";
            DB.ExecutarComandoSQL(sql);
        }

        public void Atualiza(int iID, int idCliente, int idForn, string codigo, string quantidade, string marca, string obs, string descr)
        {
            StringBuilder alteracoes = new StringBuilder();
            if (idCliente > 0)
            {
                alteracoes.Append($"idCliente = {idCliente}, ");
            }
            if (idForn > 0)
            {
                alteracoes.Append($"idForn = {idForn}, ");
            }
            if (!string.IsNullOrEmpty(codigo))
            {
                alteracoes.Append($"Codigo = '{codigo}', ");
            }
            if (!string.IsNullOrEmpty(quantidade))
            {
                alteracoes.Append($"Quant = '{quantidade}', ");
            }
            if (!string.IsNullOrEmpty(marca))
            {
                alteracoes.Append($"Marca = '{marca}', ");
            }
            if (!string.IsNullOrEmpty(obs))
            {
                alteracoes.Append($"Obs = '{obs}', ");
            }
            if (!string.IsNullOrEmpty(descr))
            {
                alteracoes.Append($"Descricao = '{descr}', ");
            }
            if (alteracoes.Length > 0)
            {
                alteracoes.Length -= 2; 
            }
            string sql = $@"UPDATE Encomendas SET {alteracoes} WHERE ID = {iID}";
            DB.ExecutarComandoSQL(sql);
        }

        public void AtualizarEncomenda(int id, string nome, string fone, DateTime data, DateTime dataPrometida, string codigo, decimal valor, string descricao, string obs, int idFornecedor)
        {
            StringBuilder sql = new StringBuilder("UPDATE Encomendas SET ");
            sql.Append($"Nome = '{nome}', ");
            sql.Append($"Telefone = '{fone}', ");
            sql.Append($"Data = '{data.ToString("yyyy-MM-dd")}', ");
            sql.Append($"DtPrometida = '{dataPrometida.ToString("yyyy-MM-dd")}', ");
            sql.Append($"Codigo = '{codigo}', ");
            sql.Append($"Valor = {valor}, ");
            sql.Append($"Descricao = '{descricao}', ");
            sql.Append($"Obs = '{obs}', ");
            sql.Append($"idForn = {idFornecedor} ");
            sql.Append($"WHERE ID = {id}");
            DB.ExecutarComandoSQL(sql.ToString());
        }


    }
}
