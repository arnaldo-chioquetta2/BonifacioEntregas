using System;
using System.Data;

namespace TeleBonifacio.dao
{
    public class CategoriaDao
    {
        public DataTable GetDadosOrdenados()
        {
            const string query = "SELECT IdCategoria AS Id, Nome FROM Categorias WHERE Nome > '' ORDER BY Nome";
            return DB.ExecutarConsulta(query);
        }

        public bool Existe(string descricao)
        {
            string valor = (descricao ?? "").Trim().Replace("'", "''");
            string query = $"SELECT Count(*) FROM Categorias WHERE UCase(Trim(Nome)) = UCase(Trim('{valor}'))";
            return DB.ExecutarConsultaCount(query) > 0;
        }

        public int Adiciona(string descricao)
        {
            string valor = (descricao ?? "").Trim();
            if (valor.Length == 0)
                throw new ArgumentException("A Categoria deve ser informada.", nameof(descricao));
            if (valor.Length > 20)
                throw new ArgumentException("A Categoria deve possuir no máximo 20 caracteres.", nameof(descricao));
            if (Existe(valor))
                return 0;

            string valorSql = valor.Replace("'", "''");
            DB.ExecutarComandoSQL($"INSERT INTO Categorias (Nome) VALUES ('{valorSql}')");
            return DB.ExecutarConsultaCount("SELECT Max(IdCategoria) FROM Categorias");
        }
    }
}