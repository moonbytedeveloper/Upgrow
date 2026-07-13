using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;

namespace Upgrow.Infrastructure.Repositories
{
    public class MasterApiXCategoryRepository : MasterRepositoryBase<ApiXCategory>
    {
        public MasterApiXCategoryRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override GetPagedAsync to include LEFT JOIN with Master_PermissionGroup
        /// Maps permission group title to PermissionGroupUUID for display
        /// </summary>
        public override async Task<PagedResult<ApiXCategory>> GetPagedAsync(
            Expression<Func<ApiXCategory, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<ApiXCategory>, IOrderedQueryable<ApiXCategory>>? orderBy = null,
            Func<IQueryable<ApiXCategory>, IQueryable<ApiXCategory>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from p in _context.ApiXCategory
                         join pg in _context.Api_Category
                             on p.CategoryUUID equals pg.UUID into pgGroup
                         from pg in pgGroup.DefaultIfEmpty()
                         select new ApiXCategory
                         {
                             Id = p.Id,
                             UUID = p.UUID,
                             Title = p.Title,
                             Icon = p.Icon,
                             SequenceNo = p.SequenceNo,
                             Description = p.Description,
                             CategoryUUID = pg != null ? pg.CategoryName : "N/A",
                             IsActive = p.IsActive
                         });
        }
    }
}