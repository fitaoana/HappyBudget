using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HappyBudget.Services.Database
{
    public interface IDatabaseService<T> where T : IEntity, new()
    {
        SQLiteAsyncConnection Database { get; }

        Task<T> GetById(int id);
        Task<List<T>> GetAllAsync();
        Task<int> SaveAsync(T entity);
        Task<int> DeleteAsync(T entity);
    }

    public interface IEntity
    {
        int Id { get; set; }
    }
}
