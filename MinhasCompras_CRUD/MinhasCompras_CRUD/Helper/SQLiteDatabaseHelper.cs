using MinhasCompras_CRUD.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MinhasCompras_CRUD.Helper
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conexao;

        public SQLiteDatabaseHelper(string path)
        {
            _conexao = new SQLiteAsyncConnection(path);
            _conexao.CreateTableAsync<Produto>().Wait();
        }
        public Task<int> Insert(Produto p)
        {
            return _conexao.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Nome = ?, Quantidade = ?, Preco = ? WHERE id = ?";

            return _conexao.QueryAsync<Produto>(sql, p.Nome, p.Quantidade, p.Preco, p.Id);
        }

        public Task<List<Produto>> GellAll()
        {
            return _conexao.Table<Produto>().ToListAsync();
        }

        public Produto GetById()
        {
            return new Produto();
        }

        public Task<int> Delete(int id)
        {
            return _conexao.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> Search(string pesquisa)
        {
            string sql = "SELECT * FROM Produto WHERE nome LIKE '%" + pesquisa + "%'";

            return _conexao.QueryAsync<Produto>(sql);
        }
    }
}




