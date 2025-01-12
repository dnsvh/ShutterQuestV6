using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using ShutterQuestV6.MVVM.Models;

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

    public async Task<int> SaveAsync<T>(T item) where T : new()
    {
        return await _database.InsertOrReplaceAsync(item);
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

