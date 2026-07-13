using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;
using Upgrow.Domain.IRepositories;
using Upgrow.Infrastructure.Extensions;

namespace Upgrow.Infrastructure.Repositories
{
    public class MasterRepositoryBase<TEntity> : IMasterRepository<TEntity>
    where TEntity : class, IMasterEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public MasterRepositoryBase(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByUuidAsync(string uuid)
            => await _dbSet.FirstOrDefaultAsync(x => x.UUID == uuid);

        public virtual async Task<List<TEntity>> GetAllActiveAsync()
            => await _dbSet.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();

        public virtual async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }
        public virtual async Task AddAsync(TEntity entity, bool saveChanges = true)
        {
            await _dbSet.AddAsync(entity);

            if (saveChanges)
                await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity, bool saveChanges = true)
        {
            _dbSet.Update(entity);

            if (saveChanges)
                await _context.SaveChangesAsync();
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public virtual async Task<PagedResult<TEntity>> GetPagedAsync(
            Expression<Func<TEntity, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null)

        {
            var query = _dbSet.AsNoTracking();

            if (queryModifier != null)
                query = queryModifier(query);

            if (filter != null)
                query = query.Where(filter);

            var total = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);
            else
                query = query.OrderBy(x => x.Id);

            return await query.ToPagedResultAsync(pagination, total);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
            => await _dbSet.AsNoTracking().AnyAsync(predicate);
    }
}
