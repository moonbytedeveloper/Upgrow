using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;

namespace VerifyIndia.Infrastructure.Repositories.Master
{
    public class WLMasterPermissionRepository : MasterRepositoryBase<WL_MasterPermission>, IWLMasterPermissionRepository
    {
        public WLMasterPermissionRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<PagedResult<WL_MasterPermission>> GetPagedAsync(
            Expression<Func<WL_MasterPermission, bool>>? filter,
            PaginationParams pagination,
            Func<IQueryable<WL_MasterPermission>, IOrderedQueryable<WL_MasterPermission>>? orderBy = null,
            Func<IQueryable<WL_MasterPermission>, IQueryable<WL_MasterPermission>>? queryModifier = null)
        {
            // Join with PermissionGroup to project friendly names
            return await base.GetPagedAsync(
                filter,
                pagination,
                orderBy,
                query => from e in _context.WL_MasterPermission
                         join pg in _context.WL_MasterPermissionGroup
                             on e.PermissionGroupUUID equals pg.UUID into pgGroup
                         from pg in pgGroup.DefaultIfEmpty()
                         select new WL_MasterPermission
                         {
                             UUID = e.UUID,
                             Name = e.Name,
                             Description = e.Description,
                             // Map display value (group title) into the PermissionGroupUUID field for UI display
                             PermissionGroupUUID = pg != null ? pg.Title : e.PermissionGroupUUID,
                             IsActive = e.IsActive,
                             Id = e.Id
                         });
        }
    }
}