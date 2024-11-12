using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using PasswordManager.Core.Domain.Shared;
using PasswordManager.Core.Dto.Shared;
using PasswordManager.Core.Enum;
using PasswordManager.Core.Shared;
using PasswordManager.Core.Extensions;
using PasswordManager.Services.Interfaces.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static PasswordManager.Core.Shared.IncludeDelegateDomain;

namespace PasswordManager.Services.Services.Shared
{
    public class GenericService<TEntity, TId> : IGenericService<TEntity, TId> where TEntity : BaseEntity<TId>
    {
        #region Properties

        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        protected readonly IConfiguration _configuration;

        #endregion Properties

        #region Ctor

        public GenericService(
                               DbContext dbContext,
                               IConfiguration configuration
                             )
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
            _configuration = configuration;
        }

        #endregion Ctor

        #region Get-ByIdAsync

        public async Task<TEntity> GetByIdAsync(TId id, bool useSqlCache = false, bool useSqlCacheForAll = false, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates)
        {
            var typeOfId = typeof(TId) == typeof(int) ? (object)Convert.ToInt32(id) : id.ToString();

            var data = await GetAllAsync(
                                          useSqlCache: useSqlCache,
                                          useSqlCacheForAll: useSqlCacheForAll,
                                          cancellationToken: cancellationToken,
                                          expression: x => x.Id.Equals(typeOfId),
                                          includeDelegates: includeDelegates
                                        );

            if (data != null)
            {
                var getData = data.FirstOrDefault(x => x.Id.Equals(typeOfId));
                if (getData != null)
                    return getData;
            }

            return null;
        }

        #endregion Get-ByIdAsync

        #region Get-AllAsync

        public async Task<IList<TEntity>> GetAllAsync(bool useSqlCache = false, bool useSqlCacheForAll = true, CancellationToken cancellationToken = default, Expression<Func<TEntity, bool>> expression = null, params IncludeDelegate<TEntity>[] includeDelegates)
        {
            return await GetQueryWithIncludedData(expression, includeDelegates).ToListAsync(cancellationToken);
        }

        #endregion Get-AllAsync

        #region Get-ByCondition

        public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression).AsQueryable();
        }

        #endregion Get-ByCondition

        #region Get-AllQueryable

        public IQueryable<TEntity> GetAllQueryable()
        {
            return _dbSet.AsQueryable();
        }

        #endregion Get-AllQueryable

        #region Post

        public async Task<TEntity> CreateAsync(TEntity entity, bool useSqlCache = false, bool useSqlCacheForAll = true, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates)
        {
            await _dbSet.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        #endregion Post

        #region Post-Range

        public async Task<IList<TEntity>> CreateRangeAsync(IList<TEntity> entities, bool useSqlCache = false, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entities;
        }

        #endregion Post-Range

        #region Put

        public async Task<TEntity> UpdateAsync(TEntity entity, bool useSqlCache = false, bool useSqlCacheForAll = true, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates)
        {
            GenerateEntityUpdatedOnValue(entity);
            _dbSet.Update(entity);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result != 0 ? entity : null;
        }

        #endregion Put

        #region Put-Range

        public async Task UpdateRangeAsync(List<TEntity> entities, bool useSqlCache = false, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates)
        {
            try
            {
                _dbSet.UpdateRange(entities);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be updated: {ex.Message}");
            }
        }

        #endregion Put-Range

        #region Delete

        public async Task<bool> DeleteAsync(TId id, bool useSqlCache = false, bool useSqlCacheForAll = true, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates)

        {
            var typeOfId = typeof(TId) == typeof(int) ? (object)Convert.ToInt32(id) : id.ToString();
            var entity = await _dbSet.FindAsync(typeOfId, cancellationToken);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }

        #endregion Delete

        #region Delete-Range

        public async Task<bool> DeleteRangeAsync(List<TEntity> entities, bool useSqlCache = false, CancellationToken cancellationToken = default, params IncludeDelegate<TEntity>[] includeDelegates)
        {
            _dbSet.RemoveRange(entities);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }

        #endregion Delete-Range

        #region Delete-RangeByCondition

        public async Task<bool> DeleteRangeByCondition(Expression<Func<TEntity, bool>> expression, bool useSqlCache = false, CancellationToken cancellationToken = default)
        {
            var entities = _dbSet.Where(expression).AsQueryable();
            _dbSet.RemoveRange(entities);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }

        #endregion Delete-RangeByCondition

        #region Filter-IQuerable

        public IQueryable<T1> ApplyFilterSearchOrderBy<T1, T2>(
                                                                IQueryable<T1> queryable,
                                                                PaginationFilterRequest request,
                                                                Expression<Func<T1, bool>> searchExpression,
                                                                Expression<Func<T1, bool>> filterExpression,
                                                                Expression<Func<T1, T2>> orderByExpression
                                                              )
        {
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                queryable = ApplySearch
                    (
                        queryable,
                        searchExpression
                    );
            }
            if (request.Filter != null && request.Filter.Count > 0)
            {
                queryable = queryable.Where(filterExpression);
            }

            if (request.OrderBy == SortByValueEnum.Ascending)
            {
                queryable = queryable.OrderBy(orderByExpression);
            }
            else
            {
                queryable = queryable.OrderByDescending(orderByExpression);
            }

            return queryable;
        }

        public IQueryable<T1> ApplyFilters<T1>
            (
                IQueryable<T1> queryable,
                PaginationFilterRequest request,
                Expression<Func<T1, bool>> filterExpression
            )
        {
            if (request.Filter != null && request.Filter.Count > 0)
            {
                queryable = queryable.Where(filterExpression);
            }

            return queryable;
        }

        public IQueryable<T1> ApplyOrderBy<T1, T2>
            (
                IQueryable<T1> queryable,
                PaginationFilterRequest request,
                Expression<Func<T1, T2>> orderByExpression
            )
        {
            if (request.OrderBy == SortByValueEnum.Ascending)
            {
                queryable = queryable.OrderBy(orderByExpression);
            }
            else if (request.OrderBy == SortByValueEnum.Descending)
            {
                queryable = queryable.OrderByDescending(orderByExpression);
            }
            else
            {
                queryable = queryable.OrderByDescending(orderByExpression);
            }

            return queryable;
        }

        public IQueryable<T1> ApplySearch<T1>(
                IQueryable<T1> queryable,
                Expression<Func<T1, bool>> searchExpression
            )
        {
            return queryable.Where(searchExpression);
        }

        public IQueryable<int> SearchContent<T1>(
                IQueryable<T1> queryable,
                string search,
                Func<T1, KeyValueSearchDto> selectExpression,
                List<string> delimiters)
        {
            var idsToReturn = new List<int>();
            var keyValueList = new List<KeyValueSearchDto>();

            var listAfterSelect = queryable.Select(selectExpression).ToList();
            foreach (var itemAfterSelect in listAfterSelect)
            {
                var itemsDelimited = itemAfterSelect.Value.Split(delimiters.ToArray(), StringSplitOptions.None);
                foreach (var item in itemsDelimited)
                {
                    keyValueList.Add(new KeyValueSearchDto { Id = itemAfterSelect.Id, Value = item });
                }
            }

            var searchDelimited = search.Split(delimiters.ToArray(), StringSplitOptions.None).ToList();
            searchDelimited = searchDelimited.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            foreach (var searchWord in searchDelimited)
            {
                if (keyValueList.Any(z => z.Value.RemoveDiacritics().Contains(searchWord.RemoveDiacritics(), StringComparison.OrdinalIgnoreCase)))
                {
                    var itemsToReturn = keyValueList.Where(z => z.Value.RemoveDiacritics().Contains(searchWord.RemoveDiacritics(), StringComparison.OrdinalIgnoreCase)).ToList();
                    idsToReturn.AddRange(itemsToReturn.Select(x => x.Id));
                }
            }

            return idsToReturn.Distinct().AsQueryable();
        }

        #endregion Filter-IQuerable

        #region Helpers

        public static object ExtractValue(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return ((ConstantExpression)expression).Value;

                case ExpressionType.MemberAccess:
                    var member = (MemberExpression)expression;
                    var constant = (ConstantExpression)member.Expression;
                    return ((FieldInfo)member.Member).GetValue(constant.Value);
                // You can extend this switch to handle other types of nodes.
                default:
                    throw new InvalidOperationException();
            }
        }

        protected virtual IQueryable<TEntity> GetQueryWithIncludedData(Expression<Func<TEntity, bool>> expression = null, params IncludeDelegate<TEntity>[] includeDelegates)
        {
            IQueryable<TEntity> query;

            if (expression != null)
                query = _dbSet.Where(expression).AsQueryable();
            else
                query = _dbSet.AsQueryable();

            if (includeDelegates?.Length > 0)
            {
                foreach (var includeDelegate in includeDelegates)
                {
                    query = includeDelegate(query);
                }
            }

            return query;
        }

        private static void GenerateEntityUpdatedOnValue(TEntity entity)
        {
            entity.ChangedOn = DateTime.UtcNow;

            foreach (var property in entity.GetType().GetProperties())
            {
                if (property.PropertyType.IsSubclassOf(typeof(BaseEntity<TId>)))
                {
                    if (entity.GetType().GetProperty(property.Name)?.GetValue(entity) == null) continue;
                    entity.GetType().GetProperty(property.Name)?.PropertyType.GetProperty(nameof(entity.ChangedOn))?
                        .SetValue(entity.GetType().GetProperty(property.Name)?.GetValue(entity), DateTime.Now);
                }
            }
        }

        #endregion Helpers
    }
}
