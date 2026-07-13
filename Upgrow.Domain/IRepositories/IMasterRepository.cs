using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.IRepositories
{
    public interface IMasterRepository<TEntity> where TEntity : class, IMasterEntity
    {
        Task<TEntity?> GetByUuidAsync(string uuid);
        Task<List<TEntity>> GetAllActiveAsync();
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity, bool saveChanges = true);
        Task UpdateAsync(TEntity entity, bool saveChanges = true);
        Task SaveChangesAsync();

        /// <summary>
        /// Generic paged query - search/sort handled by service layer
        /// </summary>
        Task<PagedResult<TEntity>> GetPagedAsync(
            Expression<Func<TEntity, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null);

        /// <summary>
        /// Check if entity exists matching the condition
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
