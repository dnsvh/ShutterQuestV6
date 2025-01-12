using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using ShutterQuestV6.MVVM.Models;

namespace ShutterQuestV6.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Post>();
            await _database.CreateTableAsync<Assignment>();
            await _database.CreateTableAsync<Review>();
            await _database.CreateTableAsync<UserAssignment>();
            await _database.CreateTableAsync<Membership>();
        }

        public async Task<int> InsertAsync<T>(T item) where T : new()
        {
            return await _database.InsertAsync(item);
        }

        public async Task<int> UpdateAsync<T>(T item) where T : new()
        {
            return await _database.UpdateAsync(item);
        }

        public async Task<List<T>> GetAllAsync<T>() where T : new()
        {
            return await _database.Table<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : new()
        {
            return await _database.FindAsync<T>(id);
        }

        public async Task<int> DeleteAsync<T>(T item) where T : new()
        {
            return await _database.DeleteAsync(item);
        }
    }
}
