using HappyBudget.Helpers;
using HappyBudget.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyBudget.Services.Database
{
    public class DatabaseService<T> : IDatabaseService<T> where T : IEntity, new()
    {
        readonly Lazy<SQLiteAsyncConnection> connection = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        });

        public SQLiteAsyncConnection Database => connection.Value;

        public DatabaseService()
        {
            InitializeAsync().FireAndForget(false);
        }

        async Task InitializeAsync()
        { 

            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(T).Name))
            {
                await Database.CreateTableAsync(typeof(T)).ConfigureAwait(false);
              
            }
           
        }

        public Task<T> GetById(int id)
        {
            return Database.Table<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<T>> GetAllAsync()
        {
            return Database.Table<T>().ToListAsync();
        }

        public Task<int> SaveAsync(T entity)
        {
            if (entity.Id != 0)
            {
                return Database.UpdateAsync(entity);
            }
            else
            {
                return Database.InsertAsync(entity);
            }
        }

        public Task<int> DeleteAsync(T entity)
        {
            return Database.DeleteAsync(entity);
        }
    }
}
