using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Bat.PortalDeCargas.Domain.Repositories;
using Bat.PortalDeCargas.Infrastructure.Repositories.TypeMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Bat.PortalDeCargas.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly PortalDeCargasContext DbContext;

        internal Repository(PortalDeCargasContext dbContext)
        {
            SqlMapper.AddTypeHandler(new FileTypeEnumerationTypeMapper());
            SqlMapper.AddTypeHandler(new LogActionEnumerationTypeMapper());
            DbContext = dbContext;
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }

        public virtual Task<T> Add<T>
            (object parameters = null, IDbTransaction tran = null) =>
            FirstOrDefault<T>(GetDefaultProcedureName(Operation.Insert), parameters, tran);

        public virtual Task<TEntity> Add
            (object parameters = null, IDbTransaction tran = null) =>
            Add<TEntity>(parameters, tran);

        public virtual Task<int> AddRangeAsync
            (IEnumerable<TEntity> entities, IDbTransaction tran = null) =>
            Execute(GetDefaultProcedureName(Operation.Insert), entities, tran);

        public virtual Task<int> ExcecutRange<T>
            (string sql, IEnumerable<T> parameters, IDbTransaction tran = null) =>
            DbContext.Database.GetDbConnection()
                .ExecuteAsync(sql, parameters, commandType: CommandType.Text, transaction: tran);

        public virtual Task<int> Execute(string sql, object parameters = null, IDbTransaction tran = null) =>
            DbContext.Database.GetDbConnection().ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure,
                transaction: tran);

        public virtual Task<int> ExecuteComand
            (string sql, object parameters = null, IDbTransaction tran = null) =>
            DbContext.Database.GetDbConnection()
                .ExecuteAsync(sql, parameters, commandType: CommandType.Text, transaction: tran);

        public virtual Task<T> Find<T>
            (object parameters = null) =>
            FirstOrDefault<T>(GetDefaultProcedureName(Operation.Get), parameters);

        public virtual Task<TEntity> Find(object parameters = null) => Find<TEntity>(parameters);

        public virtual Task<IEnumerable<T>> FindAll<T>
            (object parameters = null) =>
            Query<T>(GetDefaultProcedureName(Operation.Get), parameters);

        public virtual Task<IEnumerable<TEntity>> FindAll(object parameters = null) => FindAll<TEntity>(parameters);

        public virtual async Task<T> FirstOrDefault<T>
            (string sql, object parameters = null, IDbTransaction tran = null) =>
            await DbContext.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<T>(sql, parameters, tran, commandType: CommandType.StoredProcedure);

        public virtual Task<TEntity> FirstOrDefault
            (string sql, object parameters = null) =>
            FirstOrDefault<TEntity>(sql, parameters);

        public virtual Task<IEnumerable<T>> Query<T>
            (string sql, object parameters = null) =>
            DbContext.Database.GetDbConnection()
                .QueryAsync<T>(sql, parameters, commandType: CommandType.StoredProcedure);

        public virtual Task<IEnumerable<TEntity>> Query
            (string sql, object parameters = null) =>
            Query<TEntity>(sql, parameters);

        public virtual Task<IEnumerable<T>> QueryComand<T>
            (string sql, object parameters = null) =>
            DbContext.Database.GetDbConnection().QueryAsync<T>(sql, parameters, commandType: CommandType.Text);

        public virtual Task<int> Remove
            (object parameters = null, IDbTransaction tran = null) =>
            Execute(GetDefaultProcedureName(Operation.Delete), parameters, tran);

        public virtual Task<int> Update
            (object parameters = null, IDbTransaction tran = null) =>
            Execute(GetDefaultProcedureName(Operation.Update), parameters, tran);

        public virtual Task<T> Update<T>
            (object parameters = null, IDbTransaction tran = null) =>
            FirstOrDefault<T>(GetDefaultProcedureName(Operation.Update), parameters, tran);

        public virtual async Task<T> SingleOrDefault<T>
            (string sql, object parameters = null, IDbTransaction tran = null) =>
            await DbContext.Database.GetDbConnection()
                .QuerySingleOrDefaultAsync<T>(sql, parameters, tran, commandType: CommandType.StoredProcedure);

        protected string GetDefaultProcedureName(Operation operation)
        {
            var type = typeof(TEntity);
            var operationName = Enum.GetName(typeof(Operation), operation);

            return $"[dbo].[up_{operationName}{type.Name}]";
        }

        protected enum Operation
        {
            Insert,
            Delete,
            Get,
            Update
        }
    }
}
