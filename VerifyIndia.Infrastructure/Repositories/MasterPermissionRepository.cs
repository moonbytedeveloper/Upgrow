using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Infrastructure.Repositories
{
    public class MasterPermissionRepository : MasterRepositoryBase<Master_Permission>
    {
        public MasterPermissionRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Override GetPagedAsync to include LEFT JOIN with Master_PermissionGroup
        /// Maps permission group title to PermissionGroupUUID for display
        /// </summary>
        public override async Task<PagedResult<Master_Permission>> GetPagedAsync(
            Expression<Func<Master_Permission, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<Master_Permission>, IOrderedQueryable<Master_Permission>>? orderBy = null,
            Func<IQueryable<Master_Permission>, IQueryable<Master_Permission>>? queryModifier = null)
        {
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from p in _context.Master_Permission
                         join pg in _context.Master_PermissionGroup
                             on p.PermissionGroupUUID equals pg.UUID into pgGroup
                         from pg in pgGroup.DefaultIfEmpty()
                         select new Master_Permission
                         {
                             Id = p.Id,
                             UUID = p.UUID,
                             Name = p.Name,
                             Description = p.Description,
                             PermissionGroupUUID = pg != null ? pg.Title : "N/A",
                             IsActive = p.IsActive
                         });
        }
    }
}