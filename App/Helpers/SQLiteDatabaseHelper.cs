using App.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Helpers
{
    public class SQLiteDatabaseHelper
    {
        private readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            // Inicializando a conexão com o banco SQLite
            _conn = new SQLiteAsyncConnection(path);

            // Garantindo que a tabela Produto seja criada
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Método para criar uma tabela genérica
        public Task CreateTable<T>() where T : new()
        {
            return _conn.CreateTableAsync<T>();
        }

        // Inserção de dados
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // Atualização de dados
        public Task<int> Update(Produto p)
        {
            return _conn.UpdateAsync(p);
        }

        // Exclusão de dados
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // Busca por todos os produtos
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        // Busca por produtos com consulta dinâmica
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }

        // Consulta genérica para acessar uma tabela
        public AsyncTableQuery<T> Table<T>() where T : new()
        {
            return _conn.Table<T>();
        }

        public Task<int> DeleteProdutoAsync(Produto produto)
        {
            return _conn.DeleteAsync(produto);
        }

        public Task<int> UpdateProdutoAsync(Produto produto)
        {
            return _conn.UpdateAsync(produto);
        }


    }
}