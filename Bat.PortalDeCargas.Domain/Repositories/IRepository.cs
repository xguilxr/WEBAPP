using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Bat.PortalDeCargas.Domain.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<int> AddRangeAsync(IEnumerable<TEntity> entities, IDbTransaction tran = null);
        Task<TEntity> Add(object parameters = null, IDbTransaction tran = null);
        Task<T> Add<T>(object parameters = null, IDbTransaction tran = null);
        Task<int> Remove(object parameters = null, IDbTransaction tran = null);
        Task<int> Update(object parameters = null, IDbTransaction tran = null);
        Task<T> Update<T>(object parameters = null, IDbTransaction tran = null);
        Task<T> Find<T>(object parameters = null);
        Task<TEntity> Find(object parameters = null);
        Task<IEnumerable<T>> FindAll<T>(object parameters = null);
        Task<IEnumerable<TEntity>> FindAll(object parameters = null);
        Task<int> ExcecutRange<T>(string sql, IEnumerable<T> parameters, IDbTransaction tran = null);
        Task<int> ExecuteComand(string sql, object parameters = null, IDbTransaction tran = null);
        Task<IEnumerable<T>> QueryComand<T>(string sql, object parameters = null);
        Task<int> Execute(string sql, object parameters = null, IDbTransaction tran = null);
        Task<IEnumerable<T>> Query<T>(string sql, object parameters = null);
        Task<IEnumerable<TEntity>> Query(string sql, object parameters = null);
        Task<T> FirstOrDefault<T>(string sql, object parameters = null, IDbTransaction tran = null);
        Task<TEntity> FirstOrDefault(string sql, object parameters = null);
    }
}
