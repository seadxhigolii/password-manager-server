using PasswordManager.Core.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static PasswordManager.Core.Shared.IncludeDelegateDomain;

namespace PasswordManager.Services.Interfaces.Shared
{
    public interface IGenericService<TEntity, TId> where TEntity : BaseEntity<TId>
    {
        Task<TEntity> GetByIdAsync(TId id, bool useSqlCache = false, bool useSqlCacheForAll = false, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<IList<TEntity>> GetAllAsync(bool useSqlCache = false, bool useSqlCacheForAll = true, CancellationToken cancellationToken = default, Expression<Func<TEntity, bool>> expression = null, params IncludeDelegate<TEntity>[] includeDelegates);

        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression);

        IQueryable<TEntity> GetAllQueryable();

        Task<TEntity> CreateAsync(TEntity entity, bool useSqlCache = false, bool useSqlCacheForAll = true, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<IList<TEntity>> CreateRangeAsync(IList<TEntity> entities, bool useSqlCache = false, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegate);

        Task<TEntity> UpdateAsync(TEntity entity, bool useSqlCache = false, bool useSqlCacheForAll = true, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task UpdateRangeAsync(List<TEntity> entities, bool useSqlCache = false, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<bool> DeleteAsync(TId id, bool useSqlCache = false, bool useSqlCacheForAll = true, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<bool> DeleteRangeAsync(List<TEntity> entities, bool useSqlCache, CancellationToken cancellationToken, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<bool> DeleteRangeByCondition(Expression<Func<TEntity, bool>> expression, bool useSqlCache = false, CancellationToken cancellationToken = default);
    }

}
