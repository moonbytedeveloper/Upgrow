using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories
{
    public class ApiInfoSectionRepository : MasterRepositoryBase<ApiInfoSection>
    {
        public ApiInfoSectionRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override GetPagedAsync to include LEFT JOIN with Master_PermissionGroup
        /// Maps permission group title to PermissionGroupUUID for display
        /// </summary>
        public override async Task<PagedResult<ApiInfoSection>> GetPagedAsync(
            Expression<Func<ApiInfoSection, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<ApiInfoSection>, IOrderedQueryable<ApiInfoSection>>? orderBy = null,
            Func<IQueryable<ApiInfoSection>, IQueryable<ApiInfoSection>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from p in _context.ApiInfoSections
                         join pg in _context.Master_Api
                             on p.ApiUUID equals pg.UUID into pgGroup
                         from pg in pgGroup.DefaultIfEmpty()
                         select new ApiInfoSection
                         {
                             Id = p.Id,
                             UUID = p.UUID,
                             Title = p.Title,
                             ApiUUID = pg != null ? pg.ApiName : "N/A",
                             Sequence = p.Sequence,
                             Description = p.Description,
                             IsActive = p.IsActive
                         });
        }
    }
}