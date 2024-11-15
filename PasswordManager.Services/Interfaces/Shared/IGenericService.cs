using PasswordManager.Core.Domain.Shared;
using System.Linq.Expressions;
using static PasswordManager.Core.Shared.IncludeDelegateDomain;

namespace PasswordManager.Services.Interfaces.Shared
{
    public interface IGenericService<TEntity, TId> where TEntity : BaseEntity<TId>
    {
        Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default, Expression<Func<TEntity, bool>> expression = null, params IncludeDelegate<TEntity>[] includeDelegates);

        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression);

        IQueryable<TEntity> GetAllQueryable();

        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<IList<TEntity>> CreateRangeAsync(IList<TEntity> entities, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegate);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task UpdateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<bool> DeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken, params IncludeDelegate<TEntity>[] includeDelegates);

        Task<bool> DeleteRangeByCondition(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    }

}
