using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories
{
    public class ApiInfoFieldsRepository : MasterRepositoryBase<ApiInfoFields>
    {
        public ApiInfoFieldsRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override GetPagedAsync to include LEFT JOIN with Master_PermissionGroup
        /// Maps permission group title to PermissionGroupUUID for display
        /// </summary>
        public override async Task<PagedResult<ApiInfoFields>> GetPagedAsync(
            Expression<Func<ApiInfoFields, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<ApiInfoFields>, IOrderedQueryable<ApiInfoFields>>? orderBy = null,
            Func<IQueryable<ApiInfoFields>, IQueryable<ApiInfoFields>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from p in _context.ApiInfoFields
                         join pg in _context.ApiInfoSections
                             on p.InfoSectionUUID equals pg.UUID into pgGroup
                         from pg in pgGroup.DefaultIfEmpty()
                         select new ApiInfoFields
                         {
                             Id = p.Id,
                             UUID = p.UUID,
                             Title = p.Title,
                             InfoSectionUUID = pg != null ? pg.Title : "N/A",
                             Sequence = p.Sequence,
                             IsActive = p.IsActive
                         });
        }
    }
}